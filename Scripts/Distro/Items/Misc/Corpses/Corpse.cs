using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.ContextMenus;
using Server.Engines.Guilds;
using Server.Engines.PartySystem;
using Server.Engines.Quests;
using Server.Engines.Quests.Doom;
using Server.Misc;
using Server.Mobiles;
using Server.Network;

namespace Server.Items
{
	public interface IDevourer
	{
		bool Devour( Corpse corpse );
	}

	[Flags]
	public enum CorpseFlag
	{
		None = 0x00000000,

		/// <summary>
		/// Has this corpse been carved?
		/// </summary>
		Carved = 0x00000001,

		/// <summary>
		/// If true, this corpse will not turn into bones
		/// </summary>
		NoBones = 0x00000002,

		/// <summary>
		/// If true, the corpse has turned into bones
		/// </summary>
		IsBones = 0x00000004,

		/// <summary>
		/// Has this corpse yet been visited by a taxidermist?
		/// </summary>
		VisitedByTaxidermist = 0x00000008,

		/// <summary>
		/// Has this corpse yet been used to channel spiritual energy? (AOS Spirit Speak)
		/// </summary>
		Channeled = 0x00000010,

		/// <summary>
		/// Was the owner criminal when he died?
		/// </summary>
		Criminal = 0x00000020,

		/// <summary>
		/// Is this corpse splited in separated containers for each looter?
		/// </summary>
		Instanced = 0x00000040
	}

	public class Corpse : Container, ICarvable
	{
		private Mobile m_Owner;					// Whos corpse is this?
		private Mobile m_Killer;				// Who killed the owner?
		private CorpseFlag m_Flags;				// @see CorpseFlag

		private List<Mobile> m_Looters;			// Who's looted this corpse?
		private List<Item> m_EquipItems;		// List of items equiped when the owner died. Ingame, these items display /on/ the corpse, not just inside
		private List<Mobile> m_Aggressors;		// Anyone from this list will be able to loot this corpse; we attacked them, or they attacked us when we were freely attackable
		private List<Item> m_InsuredItems;		// List of items insured when the owner died to reequip when double-clicking the corpse

		private string m_CorpseName;			// Value of the CorpseNameAttribute attached to the owner when he died -or- null if the owner had no CorpseNameAttribute; use "the remains of ~name~"
		private IDevourer m_Devourer;			// The creature that devoured this corpse

		// For notoriety:
		private AccessLevel m_AccessLevel;		// Which AccessLevel the owner had when he died
		private Guild m_Guild;					// Which Guild the owner was in when he died
		private int m_Kills;					// How many kills the owner had when he died

		private DateTime m_TimeOfDeath;			// What time was this corpse created?

		private HairInfo m_Hair;				// This contains the hair of the owner
		private FacialHairInfo m_FacialHair;	// This contains the facial hair of the owner

		public static readonly TimeSpan MonsterLootRightSacrifice = TimeSpan.FromMinutes( 2.0 );
		public static readonly TimeSpan InstancedCorpseTime = TimeSpan.FromMinutes( 3.0 );

		private Timer m_UnsplitTimer;

		public void AssignInstancedLoot()
		{
			/*
			 * As per OSI tests:
			 *  - Each entity (player or party) gets their own instanced corpse.
			 *  - Even if no items are in the corpse or only one instance is created.
			 *  - Each instanced corpse is a 'virtual' container that is actually inside the real corpse.
			 *  - When a player opens an instanced corpse, it displays to him the instance he owns.
			 */

			if ( m_Owner.Player )
				return; // Loot does not instance for players

			if ( m_Owner is BaseCreature && ( (BaseCreature) m_Owner ).Controlled )
				return; // Same for player's pets

			if ( m_Aggressors.Count == 0 )
				return; // Do not instantiate if nobody has looting rights

			// First of all, classify the items already existing inside the corpse

			List<Item> m_Stackables = new List<Item>();
			List<Item> m_Unstackables = new List<Item>();

			for ( int i = 0; i < this.Items.Count; i++ )
			{
				Item item = (Item) this.Items[i];

				if ( item.Stackable )
					m_Stackables.Add( item );
				else
					m_Unstackables.Add( item );
			}

			// Randomize the attacker's list

			Mobile[] attackers = Utility.Shuffle( m_Aggressors.ToArray() );

			// Create the instances for each player

			InstancedCorpse[] corpses = new InstancedCorpse[attackers.Length];

			for ( int i = 0; i < attackers.Length; i++ )
			{
				Party p = Party.Get( attackers[i] );

				if ( p != null )
				{
					for ( int j = 0; j < i; j++ )
					{
						if ( p == Party.Get( attackers[j] ) )
						{
							corpses[j].AddLooter( attackers[i] );
							corpses[i] = corpses[j]; // Players in the same party share the same instanced corpse

							break;
						}
					}
				}

				if ( corpses[i] == null )
					corpses[i] = new InstancedCorpse( this, attackers[i] );
			}

			// stackables first, for the remaining stackables, have those be randomly added after

			for ( int i = 0; i < m_Stackables.Count; i++ )
			{
				Item item = m_Stackables[i];

				if ( item.Amount >= attackers.Length )
				{
					int amountPerAttacker = ( item.Amount / attackers.Length );
					int remainder = ( item.Amount % attackers.Length );

					for ( int j = 0; j < ( ( remainder == 0 ) ? corpses.Length - 1 : corpses.Length ); j++ )
					{
						Item splitItem = Mobile.LiftItemDupe( item, item.Amount - amountPerAttacker );

						corpses[j].TryDropItem( splitItem );

						// What happens to the remaining portion? TEMP FOR NOW UNTIL OSI VERIFICATION: Treat as Single Item.
					}

					if ( remainder == 0 )
					{
						corpses[corpses.Length - 1].TryDropItem( item );
						// Add in the original item (which has an equal amount as the others) to the instance for the last attacker, cause it wasn't added above.
					}
					else
					{
						m_Unstackables.Add( item );
					}
				}
				else
				{
					// What happens in this case? TEMP FOR NOW UNTIL OSI VERIFICATION: Treat as Single Item.
					m_Unstackables.Add( item );
				}
			}

			for ( int i = 0; i < m_Unstackables.Count; i++ )
			{
				// Randomly distribute unstackable items

				corpses[i % corpses.Length].TryDropItem( m_Unstackables[i] );
			}

			// Resend rummaged items to their owner's instance
			if ( m_Owner is BaseCreature )
			{
				BaseCreature bcOwner = m_Owner as BaseCreature;

				foreach ( KeyValuePair<Item, Mobile> kvp in bcOwner.RummagedItems )
				{
					InstancedCorpse corpse = GetInstancedCorpse( kvp.Value );

					if ( corpse != null )
						corpse.DropItem( kvp.Key );
				}
			}

			SetFlag( CorpseFlag.Instanced, true );

			m_UnsplitTimer = Timer.DelayCall( InstancedCorpseTime, new TimerCallback( UnsplitLoot_Callback ) );
		}

		private void UnsplitLoot_Callback()
		{
			SetFlag( CorpseFlag.Instanced, false );

			ArrayList corpses = new ArrayList( this.Items );

			for ( int i = 0; i < corpses.Count; i++ )
			{
				InstancedCorpse corpse = corpses[i] as InstancedCorpse;

				if ( corpse != null )
					corpse.OnUnsplitLoot();
			}
		}

		private InstancedCorpse GetInstancedCorpse( Mobile m )
		{
			InstancedCorpse shared = null;

			for ( int i = 0; i < this.Items.Count; i++ )
			{
				InstancedCorpse corpse = this.Items[i] as InstancedCorpse;

				if ( corpse != null )
				{
					if ( corpse.IsOwner( m ) ) // we have precedence over the instance we own
						return corpse;

					if ( corpse.IsAccessibleTo( m ) )
						shared = corpse;
				}
			}

			return shared;
		}

		public void AddCarvedItem( Item carved, Mobile carver )
		{
			if ( GetFlag( CorpseFlag.Instanced ) )
			{
				InstancedCorpse corpse = GetInstancedCorpse( carver );

				if ( corpse != null )
					corpse.DropItem( carved );
			}
			else
			{
				this.DropItem( carved );
			}
		}

		public override void DropItem( Item dropped )
		{
			if ( GetFlag( CorpseFlag.Instanced ) )
			{
				List<Container> corpses = new List<Container>();

				for ( int i = 0; i < this.Items.Count; i++ )
				{
					InstancedCorpse corpse = this.Items[i] as InstancedCorpse;

					if ( corpse != null )
						corpses.Add( corpse );
				}

				if ( corpses.Count > 0 )
					corpses[Utility.Random( corpses.Count )].DropItem( dropped );
				else
					dropped.Delete();
			}
			else
			{
				base.DropItem( dropped );
			}
		}

		public override void DisplayTo( Mobile to )
		{
			if ( GetFlag( CorpseFlag.Instanced ) && to.AccessLevel < AccessLevel.GameMaster ) // Staff always see the main corpse
			{
				InstancedCorpse corpse = GetInstancedCorpse( to );

				if ( corpse != null )
					corpse.DisplayTo( to );
			}
			else
			{
				base.DisplayTo( to );
			}
		}

		public override bool IsDecoContainer
		{
			get { return false; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public DateTime TimeOfDeath
		{
			get { return m_TimeOfDeath; }
			set { m_TimeOfDeath = value; }
		}

		public override bool DisplayWeight { get { return false; } }

		public HairInfo Hair { get { return m_Hair; } }
		public FacialHairInfo FacialHair { get { return m_FacialHair; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public bool IsBones
		{
			get { return GetFlag( CorpseFlag.IsBones ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Devoured
		{
			get { return ( m_Devourer != null ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Carved
		{
			get { return GetFlag( CorpseFlag.Carved ); }
			set { SetFlag( CorpseFlag.Carved, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool VisitedByTaxidermist
		{
			get { return GetFlag( CorpseFlag.VisitedByTaxidermist ); }
			set { SetFlag( CorpseFlag.VisitedByTaxidermist, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Channeled
		{
			get { return GetFlag( CorpseFlag.Channeled ); }
			set { SetFlag( CorpseFlag.Channeled, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Instanced
		{
			get { return GetFlag( CorpseFlag.Instanced ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public AccessLevel AccessLevel
		{
			get { return m_AccessLevel; }
		}

		public List<Mobile> Aggressors
		{
			get { return m_Aggressors; }
		}

		public List<Mobile> Looters
		{
			get { return m_Looters; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Killer
		{
			get { return m_Killer; }
		}

		public List<Item> EquipItems
		{
			get { return m_EquipItems; }
		}

		public List<Item> InsuredItems
		{
			get { return m_InsuredItems; }
			set { m_InsuredItems = value; }
		}

		public Guild Guild
		{
			get { return m_Guild; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int Kills
		{
			get { return m_Kills; }
			set { m_Kills = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Criminal
		{
			get { return GetFlag( CorpseFlag.Criminal ); }
			set { SetFlag( CorpseFlag.Criminal, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Owner
		{
			get { return m_Owner; }
		}

		public void TurnToBones()
		{
			if ( Deleted )
				return;

			ProcessDelta();
			SendRemovePacket();
			ItemID = Utility.Random( 0xECA, 9 ); // bone graphic
			Hue = 0;
			Direction = Direction.North;
			ProcessDelta();

			SetFlag( CorpseFlag.NoBones, true );
			SetFlag( CorpseFlag.IsBones, true );

			BeginDecay( m_BoneDecayTime );
		}

		private static TimeSpan m_DefaultDecayTime = TimeSpan.FromMinutes( 7.0 );
		private static TimeSpan m_BoneDecayTime = TimeSpan.FromMinutes( 7.0 );

		private Timer m_DecayTimer;
		private DateTime m_DecayTime;

		public void BeginDecay( TimeSpan delay )
		{
			if ( m_DecayTimer != null )
				m_DecayTimer.Stop();

			m_DecayTime = DateTime.UtcNow + delay;

			m_DecayTimer = new InternalTimer( this, delay );
			m_DecayTimer.Start();
		}

		public override void OnAfterDelete()
		{
			if ( m_DecayTimer != null )
				m_DecayTimer.Stop();

			m_DecayTimer = null;

			if ( m_UnsplitTimer != null )
				m_UnsplitTimer.Stop();

			m_UnsplitTimer = null;
		}

		private class InternalTimer : Timer
		{
			private Corpse m_Corpse;

			public InternalTimer( Corpse c, TimeSpan delay )
				: base( delay )
			{
				m_Corpse = c;
			}

			protected override void OnTick()
			{
				if ( !m_Corpse.GetFlag( CorpseFlag.NoBones ) )
					m_Corpse.TurnToBones();
				else
					m_Corpse.Delete();
			}
		}

		public static string GetCorpseName( Mobile m )
		{
			Type t = m.GetType();

			object[] attrs = t.GetCustomAttributes( typeof( CorpseNameAttribute ), true );

			if ( attrs != null && attrs.Length > 0 )
			{
				CorpseNameAttribute attr = attrs[0] as CorpseNameAttribute;

				if ( attr != null )
					return attr.Name;
			}

			return null;
		}

		public static void Initialize()
		{
			Mobile.CreateCorpseHandler += new CreateCorpseHandler( Mobile_CreateCorpseHandler );
		}

		public static Container Mobile_CreateCorpseHandler( Mobile owner, HairInfo hair, FacialHairInfo facialhair, List<Item> initialContent, List<Item> equipItems )
		{
			Corpse c = new Corpse( owner, hair, facialhair, equipItems );
			owner.Corpse = c;

			for ( int i = 0; i < initialContent.Count; ++i )
			{
				Item item = (Item) initialContent[i];

				if ( owner.Player && item.Parent == owner.Backpack )
					c.AddItem( item );
				else
					c.DropItem( item );

				if ( owner.Player )
					c.SetRestoreInfo( item, item.Location );
			}

			Point3D loc = owner.Location;
			Map map = owner.Map;

			if ( map == null || map == Map.Internal )
			{
				loc = owner.LogoutLocation;
				map = owner.LogoutMap;
			}

			c.MoveToWorld( loc, map );

			return c;
		}

		public override bool IsPublicContainer { get { return true; } }

		public Corpse( Mobile owner, List<Item> equipItems )
			: this( owner, null, null, equipItems )
		{
		}

		public Corpse( Mobile owner, HairInfo hair, FacialHairInfo facialhair, List<Item> equipItems )
			: base( 0x2006 )
		{
			// To supress console warnings, stackable must be true
			Stackable = true;
			Amount = owner.Body; // protocol defines that for itemid 0x2006, amount=body
			Stackable = false;

			Movable = false;
			Hue = owner.Hue;
			Direction = owner.Direction;
			Name = owner.Name;

			m_Owner = owner;

			m_CorpseName = GetCorpseName( owner );

			m_TimeOfDeath = DateTime.UtcNow;

			m_AccessLevel = owner.AccessLevel;
			m_Guild = owner.Guild as Guild;
			m_Kills = owner.Kills;
			SetFlag( CorpseFlag.Criminal, owner.Criminal );

			m_Hair = hair;
			m_FacialHair = facialhair;

			// This corpse does not turn to bones if: the owner is not a player
			SetFlag( CorpseFlag.NoBones, !owner.Player );

			m_Looters = new List<Mobile>();
			m_EquipItems = equipItems;

			m_Aggressors = new List<Mobile>( owner.Aggressors.Count + owner.Aggressed.Count );

			bool isBaseCreature = ( owner is BaseCreature );

			TimeSpan lastTime = TimeSpan.MaxValue;

			for ( int i = 0; i < owner.Aggressors.Count; ++i )
			{
				AggressorInfo info = (AggressorInfo) owner.Aggressors[i];

				if ( ( DateTime.UtcNow - info.LastCombatTime ) < lastTime )
				{
					m_Killer = info.Attacker;
					lastTime = ( DateTime.UtcNow - info.LastCombatTime );
				}

				if ( !isBaseCreature && !info.CriminalAggression )
					m_Aggressors.Add( info.Attacker );
			}

			for ( int i = 0; i < owner.Aggressed.Count; ++i )
			{
				AggressorInfo info = (AggressorInfo) owner.Aggressed[i];

				if ( ( DateTime.UtcNow - info.LastCombatTime ) < lastTime )
				{
					m_Killer = info.Defender;
					lastTime = ( DateTime.UtcNow - info.LastCombatTime );
				}

				if ( !isBaseCreature )
					m_Aggressors.Add( info.Defender );
			}

			if ( isBaseCreature )
			{
				BaseCreature bc = (BaseCreature) owner;

				Mobile master = bc.GetMaster();
				if ( master != null )
					m_Aggressors.Add( master );

				List<DamageStore> rights = BaseCreature.GetLootingRights( bc.DamageEntries, bc.HitsMax );
				for ( int i = 0; i < rights.Count; ++i )
				{
					DamageStore ds = rights[i];

					if ( ds.HasRight )
						m_Aggressors.Add( ds.Mobile );
				}
			}

			BeginDecay( m_DefaultDecayTime );

			DevourCorpse();
		}

		public Corpse( Serial serial )
			: base( serial )
		{
		}

		protected bool GetFlag( CorpseFlag flag )
		{
			return ( ( m_Flags & flag ) != 0 );
		}

		protected void SetFlag( CorpseFlag flag, bool on )
		{
			m_Flags = ( on ? m_Flags | flag : m_Flags & ~flag );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 11 ); // version

			writer.Write( (int) m_Flags );

			writer.WriteDeltaTime( m_TimeOfDeath );

			List<KeyValuePair<Item, Point3D>> list = ( m_RestoreTable == null ? null : new List<KeyValuePair<Item, Point3D>>( m_RestoreTable ) );
			int count = ( list == null ? 0 : list.Count );

			writer.Write( count );

			for ( int i = 0; i < count; ++i )
			{
				KeyValuePair<Item, Point3D> kvp = list[i];
				Item item = kvp.Key;
				Point3D loc = kvp.Value;

				writer.Write( item );

				if ( item.Location == loc )
				{
					writer.Write( false );
				}
				else
				{
					writer.Write( true );
					writer.Write( loc );
				}
			}

			writer.Write( m_DecayTimer != null );

			if ( m_DecayTimer != null )
				writer.WriteDeltaTime( m_DecayTime );

			writer.Write( m_Looters );
			writer.Write( m_Killer );

			writer.Write( m_Aggressors );

			writer.Write( m_Owner );

			writer.Write( (string) m_CorpseName );

			writer.Write( (int) m_AccessLevel );
			writer.Write( (Guild) m_Guild );
			writer.Write( (int) m_Kills );

			writer.Write( m_EquipItems );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 11:
					{
						// Version 11, we move all bools to a CorpseFlag
						m_Flags = (CorpseFlag) reader.ReadInt();

						m_TimeOfDeath = reader.ReadDeltaTime();

						int count = reader.ReadInt();

						for ( int i = 0; i < count; ++i )
						{
							Item item = reader.ReadItem();

							if ( reader.ReadBool() )
								SetRestoreInfo( item, reader.ReadPoint3D() );
							else if ( item != null )
								SetRestoreInfo( item, item.Location );
						}

						if ( reader.ReadBool() )
							BeginDecay( reader.ReadDeltaTime() - DateTime.UtcNow );

						m_Looters = reader.ReadStrongMobileList();
						m_Killer = reader.ReadMobile();

						m_Aggressors = reader.ReadStrongMobileList();
						m_Owner = reader.ReadMobile();

						m_CorpseName = reader.ReadString();

						m_AccessLevel = (AccessLevel) reader.ReadInt();
						reader.ReadInt(); // guild reserve
						m_Kills = reader.ReadInt();

						m_EquipItems = reader.ReadStrongItemList();

						SetFlag( CorpseFlag.Instanced, false );

						break;
					}
				case 10:
					{
						m_TimeOfDeath = reader.ReadDeltaTime();

						goto case 9;
					}
				case 9:
					{
						int count = reader.ReadInt();

						for ( int i = 0; i < count; ++i )
						{
							Item item = reader.ReadItem();

							if ( reader.ReadBool() )
								SetRestoreInfo( item, reader.ReadPoint3D() );
							else if ( item != null )
								SetRestoreInfo( item, item.Location );
						}

						goto case 8;
					}
				case 8:
					{
						SetFlag( CorpseFlag.VisitedByTaxidermist, reader.ReadBool() );

						goto case 7;
					}
				case 7:
					{
						if ( reader.ReadBool() )
							BeginDecay( reader.ReadDeltaTime() - DateTime.UtcNow );

						goto case 6;
					}
				case 6:
					{
						m_Looters = reader.ReadStrongMobileList();
						m_Killer = reader.ReadMobile();

						goto case 5;
					}
				case 5:
					{
						SetFlag( CorpseFlag.Carved, reader.ReadBool() );

						goto case 4;
					}
				case 4:
					{
						m_Aggressors = reader.ReadStrongMobileList();

						goto case 3;
					}
				case 3:
					{
						m_Owner = reader.ReadMobile();

						goto case 2;
					}
				case 2:
					{
						SetFlag( CorpseFlag.NoBones, reader.ReadBool() );

						goto case 1;
					}
				case 1:
					{
						m_CorpseName = reader.ReadString();

						goto case 0;
					}
				case 0:
					{
						if ( version < 10 )
							m_TimeOfDeath = DateTime.UtcNow;

						if ( version < 7 )
							BeginDecay( m_DefaultDecayTime );

						if ( version < 6 )
							m_Looters = new List<Mobile>();

						if ( version < 4 )
							m_Aggressors = new List<Mobile>();

						m_AccessLevel = (AccessLevel) reader.ReadInt();
						reader.ReadInt(); // guild reserve
						m_Kills = reader.ReadInt();
						SetFlag( CorpseFlag.Criminal, reader.ReadBool() );

						m_EquipItems = reader.ReadStrongItemList();

						break;
					}
			}
		}

		public bool DevourCorpse()
		{
			if ( Devoured || Deleted || m_Killer == null || m_Killer.Deleted || !m_Killer.Alive || !( m_Killer is IDevourer ) || m_Owner == null || m_Owner.Deleted )
				return false;

			m_Devourer = (IDevourer) m_Killer; // Set the devourer the killer
			return m_Devourer.Devour( this ); // Devour the corpse if it hasn't
		}

		public override void SendInfoTo( NetState state )
		{
			base.SendInfoTo( state );

			if ( ItemID == 0x2006 || GetFlag( CorpseFlag.Instanced ) )
			{
				state.Send( new CorpseContent( state.Mobile, this ) );
				state.Send( new CorpseEquip( state.Mobile, this ) );
			}
		}

		public bool IsCriminalAction( Mobile from )
		{
			if ( from == m_Owner || from.AccessLevel >= AccessLevel.GameMaster )
				return false;

			Party p = Party.Get( m_Owner );

			if ( p != null && p.Contains( from ) )
			{
				PartyMemberInfo pmi = p[m_Owner];

				if ( pmi != null && pmi.CanLoot )
					return false;
			}

			return ( NotorietyHandlers.ComputeCorpse( from, this ) == Notoriety.Innocent );
		}

		public override bool CheckItemUse( Mobile from, Item item )
		{
			if ( !base.CheckItemUse( from, item ) )
				return false;

			if ( item != this )
				return CanLoot( from );

			return true;
		}

		public override bool CheckLift( Mobile from, Item item, ref LRReason reject )
		{
			if ( !base.CheckLift( from, item, ref reject ) )
				return false;

			return CanLoot( from );
		}

		public override void OnItemUsed( Mobile from, Item item )
		{
			base.OnItemUsed( from, item );

			if ( from != m_Owner )
				from.RevealingAction();

			if ( item != this && IsCriminalAction( from ) )
				from.CriminalAction( true );

			if ( !m_Looters.Contains( from ) )
				m_Looters.Add( from );
		}

		public override void OnItemLifted( Mobile from, Item item )
		{
			base.OnItemLifted( from, item );

			if ( item != this && from != m_Owner )
				from.RevealingAction();

			if ( item != this && IsCriminalAction( from ) )
				from.CriminalAction( true );

			if ( !m_Looters.Contains( from ) )
				m_Looters.Add( from );
		}

		private class OpenCorpseEntry : ContextMenuEntry
		{
			public OpenCorpseEntry()
				: base( 6215, 2 )
			{
			}

			public override void OnClick()
			{
				Corpse corpse = Owner.Target as Corpse;

				if ( corpse != null && Owner.From.CheckAlive() )
					corpse.Open( Owner.From, false );
			}
		}

		public override void GetContextMenuEntries( Mobile from, List<ContextMenuEntry> list )
		{
			base.GetContextMenuEntries( from, list );

			if ( m_Owner == from && from.Alive )
				list.Add( new OpenCorpseEntry() );
		}

		private Dictionary<Item, Point3D> m_RestoreTable;

		public bool GetRestoreInfo( Item item, ref Point3D loc )
		{
			if ( m_RestoreTable == null || item == null )
				return false;

			return m_RestoreTable.TryGetValue( item, out loc );
		}

		public void SetRestoreInfo( Item item, Point3D loc )
		{
			if ( item == null )
				return;

			if ( m_RestoreTable == null )
				m_RestoreTable = new Dictionary<Item, Point3D>();

			m_RestoreTable[item] = loc;
		}

		public void ClearRestoreInfo( Item item )
		{
			if ( m_RestoreTable == null || item == null )
				return;

			m_RestoreTable.Remove( item );

			if ( m_RestoreTable.Count == 0 )
				m_RestoreTable = null;
		}

		public bool CanLoot( Mobile from )
		{
			if ( Instanced && GetInstancedCorpse( from ) == null )
				return false;

			if ( !IsCriminalAction( from ) )
				return true;

			Map map = this.Map;

			if ( map == null || ( map.Rules & MapRules.HarmfulRestrictions ) != 0 )
				return false;

			return true;
		}

		public bool CheckLoot( Mobile from )
		{
			Map map = this.Map;

			if ( !CanLoot( from ) )
			{
				if ( m_Owner == null || !m_Owner.Player )
					from.SendLocalizedMessage( 1005035 ); // You did not earn the right to loot this creature!
				else
					from.SendLocalizedMessage( 1010049 ); // You may not loot this corpse.

				return false;
			}
			else if ( IsCriminalAction( from ) )
			{
				if ( m_Owner == null || !m_Owner.Player )
					from.SendLocalizedMessage( 1005036 ); // Looting this monster corpse will be a criminal act!
				else
					from.SendLocalizedMessage( 1005038 ); // Looting this corpse will be a criminal act!
			}
			// From RunUO RE
			else if ( ( map == null || ( map.Rules & MapRules.HarmfulRestrictions ) != 0 ) && m_Owner != null && m_Owner.Player && NotorietyHandlers.ComputeCorpse( from, this ) == Notoriety.Criminal )
			{
				// Really it's not possible to become criminal in trammel/ilshenar/malas at all, but there're some bugs that allow this
				// So we need to prevent looting from criminal corpses
				// TODO: When criminals will be completely fixed remove this
				from.SendLocalizedMessage( 1010049 ); // You may not loot this corpse.
				return false;
			}

			return true;
		}

		public virtual void Open( Mobile from, bool checkSelfLoot )
		{
			if ( from.AccessLevel > AccessLevel.Player || from.InRange( this.GetWorldLocation(), 2 ) )
			{
				#region Self Looting
				bool selfLoot = ( checkSelfLoot && ( from == m_Owner ) );

				if ( selfLoot )
				{
					if ( from is PlayerMobile && from.NetState != null && from.NetState.Version.IsEnhanced )
					{
						PlayerMobile pm = (PlayerMobile) from;

						pm.Send( new RemoveWaypoint( this.Serial.Value ) );
						pm.CheckKRStartingQuestStep( 27 );
					}

					ArrayList items = new ArrayList( this.Items );

					bool gathered = false;
					bool didntFit = false;

					Container pack = from.Backpack;

					bool checkRobe = true;

					for ( int i = 0; !didntFit && i < items.Count; ++i )
					{
						Item item = (Item) items[i];
						Point3D loc = item.Location;

						if ( ( item.Layer == Layer.Hair || item.Layer == Layer.FacialHair ) || !item.Movable || !GetRestoreInfo( item, ref loc ) )
							continue;

						if ( checkRobe )
						{
							DeathRobe robe = from.FindItemOnLayer( Layer.OuterTorso ) as DeathRobe;

							if ( robe != null )
								robe.Delete();
						}

						if ( m_EquipItems.Contains( item ) && from.EquipItem( item ) )
						{
							gathered = true;
						}
						else if ( pack != null && pack.CheckHold( from, item, false, true ) )
						{
							item.Location = loc;
							pack.AddItem( item );
							gathered = true;
						}
						else
						{
							didntFit = true;
						}
					}

					if ( gathered && !didntFit )
					{
						SetFlag( CorpseFlag.Carved, true );

						if ( ItemID == 0x2006 )
						{
							ProcessDelta();
							SendRemovePacket();
							ItemID = Utility.Random( 0xECA, 9 ); // bone graphic
							Hue = 0;
							Direction = Direction.North;
							ProcessDelta();
						}

						from.PlaySound( 0x3E3 );
						from.SendLocalizedMessage( 1062471 ); // You quickly gather all of your belongings.

						try
						{
							if ( from is PlayerMobile && m_InsuredItems != null )
							{
								ArrayList bagitems = new ArrayList( from.Backpack.Items );
								List<Item> insureditems = m_InsuredItems;

								foreach ( Item item in bagitems )
								{
									if ( insureditems.Contains( item ) )
									{
										Item move = from.FindItemOnLayer( item.Layer );

										if ( move == null )
											from.EquipItem( item );
									}
								}

								m_InsuredItems = null;
							}
						}
						catch
						{
						}

						return;
					}

					if ( gathered && didntFit )
						from.SendLocalizedMessage( 1062472 ); // You gather some of your belongings. The rest remain on the corpse.
				}

				#endregion

				if ( !CheckLoot( from ) )
					return;

				#region Quests
				PlayerMobile player = from as PlayerMobile;

				if ( player != null )
				{
					QuestSystem qs = player.Quest;

					if ( qs is TheSummoningQuest )
					{
						var obj = qs.FindObjective<VanquishDaemonObjective>();

						if ( obj != null && obj.Completed && obj.CorpseWithSkull == this )
						{
							var sk = new GoldenSkull();

							if ( player.PlaceInBackpack( sk ) )
							{
								obj.CorpseWithSkull = null;
								player.SendLocalizedMessage( 1050022 ); // For your valor in combating the devourer, you have been awarded a golden skull.
								qs.Complete();
							}
							else
							{
								sk.Delete();
								player.SendLocalizedMessage( 1050023 ); // You find a golden skull, but your backpack is too full to carry it.
							}
						}
					}
				}

				#endregion

				if ( m_Owner is WeakSkeleton && from is PlayerMobile )
					( (PlayerMobile) from ).CheckKRStartingQuestStep( 20 );

				base.OnDoubleClick( from );

				if ( from != m_Owner )
					from.RevealingAction();
			}
			else
			{
				from.SendLocalizedMessage( 500446 ); // That is too far away.
				return;
			}
		}

		public override void OnDoubleClick( Mobile from )
		{
			Open( from, true );
		}

		public override bool CheckContentDisplay( Mobile from )
		{
			return false;
		}

		public override bool DisplaysContent { get { return false; } }

		public override LocalizedText GetNameProperty()
		{
			if ( ItemID == 0x2006 ) // Corpse form
			{
				if ( m_CorpseName != null )
					return new LocalizedText( m_CorpseName );
				else
					return new LocalizedText( 1046414, this.Name ); // the remains of ~1_NAME~
			}
			else // Bone form
			{
				return new LocalizedText( 1046414, this.Name ); // the remains of ~1_NAME~
			}
		}

		public override void OnAosSingleClick( Mobile from )
		{
			int hue = Notoriety.GetHue( NotorietyHandlers.ComputeCorpse( from, this ) );
			ObjectPropertyListPacket opl = this.PropertyList;

			if ( opl.Header > 0 )
				from.Send( new MessageLocalized( Serial, ItemID, MessageType.Label, hue, 3, opl.Header, Name, opl.HeaderArgs ) );
		}

		public void Carve( Mobile from, Item item )
		{
			if ( IsCriminalAction( from ) && this.Map != null && ( this.Map.Rules & MapRules.HarmfulRestrictions ) != 0 || ( Instanced && GetInstancedCorpse( from ) == null ) )
			{
				// You may not dismember that corpse as you do not have rights to it.
				from.SendLocalizedMessage( 1005016 );

				return;
			}

			Mobile dead = m_Owner;

			if ( GetFlag( CorpseFlag.Carved ) || dead == null )
			{
				from.SendLocalizedMessage( 500485 ); // You see nothing useful to carve from the corpse.
			}
			else if ( dead is TyballShadow )
			{
				// You may not dismember that corpse as you do not have rights to it.
				from.SendLocalizedMessage( 1005016 );
			}
			else if ( ( (Body) Amount ).IsHuman && ItemID == 0x2006 )
			{
				new Blood( 0x122D ).MoveToWorld( Location, Map );

				new Torso().MoveToWorld( Location, Map );
				new LeftLeg().MoveToWorld( Location, Map );
				new LeftArm().MoveToWorld( Location, Map );
				new RightLeg().MoveToWorld( Location, Map );
				new RightArm().MoveToWorld( Location, Map );
				new Head( String.Format( "the head of {0}", dead.Name ) ).MoveToWorld( Location, Map );

				SetFlag( CorpseFlag.Carved, true );

				ProcessDelta();
				SendRemovePacket();
				ItemID = Utility.Random( 0xECA, 9 ); // bone graphic
				Hue = 0;
				Direction = Direction.North;
				ProcessDelta();

				if ( IsCriminalAction( from ) )
					from.CriminalAction( true );
			}
			else if ( dead is BaseCreature )
				( (BaseCreature) dead ).OnCarve( from, this, item is ButchersWarCleaver );
			else
				from.SendLocalizedMessage( 500485 ); // You see nothing useful to carve from the corpse.
		}
	}

	public class InstancedCorpse : Container
	{
		private Corpse m_Owner;
		private List<Mobile> m_Looters;

		public InstancedCorpse( Corpse corpse, Mobile m )
			: base( 0x2006 )
		{
			Movable = false;

			m_Owner = corpse;

			m_Looters = new List<Mobile>();
			m_Looters.Add( m );

			corpse.DropItem( this );
			corpse.EquipItems.Add( this );
		}

		public void AddLooter( Mobile m )
		{
			m_Looters.Add( m );
		}

		public override void OnItemUsed( Mobile from, Item item )
		{
			base.OnItemUsed( from, item );

			if ( from != m_Owner.Owner )
				from.RevealingAction();

			if ( !m_Owner.Looters.Contains( from ) )
				m_Owner.Looters.Add( from );
		}

		public override void OnItemLifted( Mobile from, Item item )
		{
			base.OnItemLifted( from, item );

			if ( item != this && from != m_Owner.Owner )
				from.RevealingAction();

			if ( !m_Owner.Looters.Contains( from ) )
				m_Owner.Looters.Add( from );
		}

		public bool IsOwner( Mobile m )
		{
			return m_Looters.Contains( m );
		}

		public override bool IsAccessibleTo( Mobile check )
		{
			if ( !base.IsAccessibleTo( check ) )
				return false;

			if ( !check.Player || check.AccessLevel >= AccessLevel.GameMaster )
				return true;

			if ( m_Looters == null )
				return false;

			if ( m_Looters.Contains( check ) )
				return true;

			Party p = Party.Get( check );

			if ( p != null )
			{
				for ( int i = 0; i < m_Looters.Count; i++ )
				{
					if ( p == Party.Get( m_Looters[i] ) )
						return true;
				}
			}

			return false;
		}

		public void OnUnsplitLoot()
		{
			if ( m_Owner != null && !m_Owner.Deleted )
			{
				ArrayList items = new ArrayList( this.Items );

				for ( int i = 0; i < items.Count; i++ )
				{
					m_Owner.TryDropItem( (Item) items[i] );
				}
			}

			Delete();
		}

		public override void SendInfoTo( NetState state )
		{
		}

		public InstancedCorpse( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.WriteItem( m_Owner );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();

			m_Owner = reader.ReadItem<Corpse>();

			Timer.DelayCall( TimeSpan.Zero, new TimerCallback( OnUnsplitLoot ) );
		}
	}
}
