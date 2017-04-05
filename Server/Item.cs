using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Server;
using Server.ContextMenus;
using Server.Events;
using Server.Items;
using Server.Network;

namespace Server
{
	/// <summary>
	/// Internal flags used to signal how the item should be updated and resent to nearby clients.
	/// </summary>
	[Flags]
	public enum ItemDelta
	{
		/// <summary>
		/// Nothing.
		/// </summary>
		None = 0x00000000,
		/// <summary>
		/// Resend the item.
		/// </summary>
		Update = 0x00000001,
		/// <summary>
		/// Resend the item only if it is equiped.
		/// </summary>
		EquipOnly = 0x00000002,
		/// <summary>
		/// Resend the item's properties.
		/// </summary>
		Properties = 0x00000004
	}

	/// <summary>
	/// Enumeration containing possible ways to handle item ownership on death.
	/// </summary>
	public enum DeathMoveResult
	{
		/// <summary>
		/// The item should be placed onto the corpse.
		/// </summary>
		MoveToCorpse,
		/// <summary>
		/// The item should remain equiped.
		/// </summary>
		RemainEquiped,
		/// <summary>
		/// The item should be placed into the owners backpack.
		/// </summary>
		MoveToBackpack
	}

	/// <summary>
	/// Enumeration of an item's loot and steal state.
	/// </summary>
	public enum LootType : byte
	{
		/// <summary>
		/// Stealable. Lootable.
		/// </summary>
		Regular = 0,
		/// <summary>
		/// Unstealable. Unlootable, unless owned by a murderer.
		/// </summary>
		Newbied = 1,
		/// <summary>
		/// Unstealable. Unlootable, always.
		/// </summary>
		Blessed = 2,
		/// <summary>
		/// Stealable. Lootable, always.
		/// </summary>
		Cursed = 3
	}

	public class BounceInfo
	{
		public Map m_Map;
		public Point3D m_Location, m_WorldLoc;
		public object m_Parent;

		public BounceInfo( Item item )
		{
			m_Map = item.Map;
			m_Location = item.Location;
			m_WorldLoc = item.GetWorldLocation();
			m_Parent = item.Parent;
		}

		private BounceInfo( Map map, Point3D loc, Point3D worldLoc, object parent )
		{
			m_Map = map;
			m_Location = loc;
			m_WorldLoc = worldLoc;
			m_Parent = parent;
		}

		public static BounceInfo Deserialize( GenericReader reader )
		{
			if ( reader.ReadBool() )
			{
				Map map = reader.ReadMap();
				Point3D loc = reader.ReadPoint3D();
				Point3D worldLoc = reader.ReadPoint3D();

				object parent;

				Serial serial = reader.ReadInt();

				if ( serial.IsItem )
					parent = World.FindItem( serial );
				else if ( serial.IsMobile )
					parent = World.FindMobile( serial );
				else
					parent = null;

				return new BounceInfo( map, loc, worldLoc, parent );
			}
			else
			{
				return null;
			}
		}

		public static void Serialize( BounceInfo info, GenericWriter writer )
		{
			if ( info == null )
			{
				writer.Write( false );
			}
			else
			{
				writer.Write( true );

				writer.Write( info.m_Map );
				writer.Write( info.m_Location );
				writer.Write( info.m_WorldLoc );

				if ( info.m_Parent is Mobile )
					writer.Write( (Mobile) info.m_Parent );
				else if ( info.m_Parent is Item )
					writer.Write( (Item) info.m_Parent );
				else
					writer.Write( (int) ( (Serial) 0 ) );
			}
		}
	}

	public class Item : IEntity, IHued, ISerializable, ISpawnable
	{
		private static readonly ILog log = LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

		public static readonly List<Item> EmptyItems = new List<Item>();

		#region Standard fields
		private Point3D m_Location;
		private int m_ItemID;
		private int m_Amount;
		private Layer m_Layer;
		private string m_Name;
		private object m_Parent; // Mobile, Item, or null=World
		private List<Item> m_Items;
		private double m_Weight;
		private int m_TotalItems;
		private int m_TotalWeight;
		private int m_TotalGold;
		private Map m_Map;
		private LootType m_LootType;
		private Direction m_Direction;
		private LightType m_LightType;
		#endregion

		private BounceInfo m_Bounce;

		private ItemDelta m_DeltaFlags;
		private ImplFlag m_Flags;

		#region Packet caches
		private Packet m_WorldPacket;
		private Packet m_RemovePacket;

		private Packet m_OplPacket;
		private ObjectPropertyListPacket m_PropertyList;
		#endregion

		public int TempFlags { get; set; }

		public int SavedFlags { get; set; }

		/// <summary>
		/// The <see cref="Mobile" /> who is currently <see cref="Mobile.Holding">holding</see> this item.
		/// </summary>
		public Mobile HeldBy { get; set; }

		#region Labels
		public static readonly int MaxLabelCount = 3;

		private string[] m_Labels;

		private string GetLabel( int index )
		{
			if ( m_Labels == null )
				return string.Empty;

			return m_Labels[index];
		}

		private void SetLabel( int index, string value )
		{
			if ( string.IsNullOrEmpty( value ) )
			{
				if ( m_Labels != null )
				{
					m_Labels[index] = null;

					bool allNull = true;

					for ( int i = 0; i < m_Labels.Length && allNull; i++ )
						if ( m_Labels[i] != null )
							allNull = false;

					if ( allNull )
						m_Labels = null;
				}
			}
			else
			{
				if ( m_Labels == null )
					m_Labels = new string[MaxLabelCount];

				m_Labels[index] = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public string Label1
		{
			get { return GetLabel( 0 ); }
			set { SetLabel( 0, value ); InvalidateProperties(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public string Label2
		{
			get { return GetLabel( 1 ); }
			set { SetLabel( 1, value ); InvalidateProperties(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public string Label3
		{
			get { return GetLabel( 2 ); }
			set { SetLabel( 2, value ); InvalidateProperties(); }
		}
		#endregion

		// _UOKR_

		[CommandProperty( AccessLevel.GameMaster )]
		public byte GridLocation { get; set; } = 0xFF;

		public void SetGridLocation( byte pos, Container parent )
		{
			if ( parent.IsFreePosition( pos ) )
				GridLocation = pos;
			else
				GridLocation = parent.GetNewPosition();
		}

		#region Instances
		private int m_InstanceID;

		[CommandProperty( AccessLevel.GameMaster )]
		public int InstanceID
		{
			get { return m_InstanceID; }
			set
			{
				if ( m_InstanceID != value )
				{
					if ( m_Parent == null )
						SendRemovePacket();

					if ( m_Items != null )
					{
						for ( int i = 0; i < m_Items.Count; ++i )
							( (Item) m_Items[i] ).InstanceID = value;
					}

					m_InstanceID = value;

					Delta( ItemDelta.Update );
				}
			}
		}
		#endregion

		[Flags]
		private enum ImplFlag : byte
		{
			None = 0x00,
			Visible = 0x01,
			Movable = 0x02,
			Deleted = 0x04,
			Stackable = 0x08,
			InQueue = 0x10,
			Insured = 0x20,
			PayedInsurance = 0x40,
			QuestItem = 0x80
		}

		private void SetFlag( ImplFlag flag, bool value )
		{
			if ( value )
				m_Flags |= flag;
			else
				m_Flags &= ~flag;
		}

		private bool GetFlag( ImplFlag flag )
		{
			return ( ( m_Flags & flag ) != 0 );
		}

		public BounceInfo GetBounce()
		{
			return m_Bounce;
		}

		public void RecordBounce()
		{
			m_Bounce = new BounceInfo( this );
		}

		public void ClearBounce()
		{
			BounceInfo bounce = m_Bounce;

			if ( bounce != null )
			{
				m_Bounce = null;

				if ( bounce.m_Parent is Item )
				{
					Item parent = (Item) bounce.m_Parent;

					if ( !parent.Deleted )
						parent.OnItemBounceCleared( this );
				}
				else if ( bounce.m_Parent is Mobile )
				{
					Mobile parent = (Mobile) bounce.m_Parent;

					if ( !parent.Deleted )
						parent.OnItemBounceCleared( this );
				}
			}
		}

		/// <summary>
		/// Overridable. Virtual event invoked when a client, <paramref name="from" />, invokes a 'help request' for the Item. Seemingly no longer functional in newer clients.
		/// </summary>
		public virtual void OnHelpRequest( Mobile from )
		{
		}

		/// <summary>
		/// Overridable. Method checked to see if the item can be traded.
		/// </summary>
		/// <returns>True if the trade is allowed, false if not.</returns>
		public virtual bool AllowSecureTrade( Mobile from, Mobile to, Mobile newOwner, bool accepted )
		{
			return true;
		}

		/// <summary>
		/// Overridable. Virtual event invoked when a trade has completed, either successfully or not.
		/// </summary>
		public virtual void OnSecureTrade( Mobile from, Mobile to, Mobile newOwner, bool accepted )
		{
		}

		/// <summary>
		/// Overridable. Method checked to see if the elemental resistances of this Item conflict with another Item on the <see cref="Mobile" />.
		/// </summary>
		/// <returns>
		/// <list type="table">
		/// <item>
		/// <term>True</term>
		/// <description>There is a confliction. The elemental resistance bonuses of this Item should not be applied to the <see cref="Mobile" /></description>
		/// </item>
		/// <item>
		/// <term>False</term>
		/// <description>There is no confliction. The bonuses should be applied.</description>
		/// </item>
		/// </list>
		/// </returns>
		public virtual bool CheckPropertyConfliction( Mobile m )
		{
			return false;
		}

		/// <summary>
		/// Overridable. Sends the <see cref="PropertyList">object property list</see> to <paramref name="from" />.
		/// </summary>
		public virtual void SendPropertiesTo( Mobile from )
		{
			from.Send( PropertyList );
		}

		/// <summary>
		/// Overridable. Adds the name of this item to the given <see cref="ObjectPropertyList" />. This method should be overriden if the item requires a complex naming format.
		/// </summary>
		public virtual LocalizedText GetNameProperty()
		{
			if ( m_Name == null )
			{
				if ( m_Amount <= 1 )
					return new LocalizedText( LabelNumber );
				else
					return new LocalizedText( 1050039, "{0}\t#{1}", m_Amount, LabelNumber ); // ~1_NUMBER~ ~2_ITEMNAME~
			}
			else
			{
				if ( m_Amount <= 1 )
					return new LocalizedText( m_Name );
				else
					return new LocalizedText( 1050039, "{0}\t{1}", m_Amount, Name ); // ~1_NUMBER~ ~2_ITEMNAME~
			}
		}

		/// <summary>
		/// Overridable. Adds the loot type of this item to the given <see cref="ObjectPropertyList" />. By default, this will be either 'blessed', 'cursed', or 'insured'.
		/// </summary>
		public virtual void AddLootTypeProperty( ObjectPropertyList list )
		{
			if ( m_LootType == LootType.Blessed )
				list.Add( 1038021 ); // blessed
			else if ( m_LootType == LootType.Cursed )
				list.Add( 1049643 ); // cursed
			else if ( Insured )
				list.Add( 1061682 ); // <b>insured</b>
		}

		/// <summary>
		/// Overridable. Adds any elemental resistances of this item to the given <see cref="ObjectPropertyList" />.
		/// </summary>
		public virtual void AddResistanceProperties( ObjectPropertyList list )
		{
			int v = PhysicalResistance;

			if ( v != 0 )
				list.Add( 1060448, v.ToString() ); // physical resist ~1_val~%

			v = FireResistance;

			if ( v != 0 )
				list.Add( 1060447, v.ToString() ); // fire resist ~1_val~%

			v = ColdResistance;

			if ( v != 0 )
				list.Add( 1060445, v.ToString() ); // cold resist ~1_val~%

			v = PoisonResistance;

			if ( v != 0 )
				list.Add( 1060449, v.ToString() ); // poison resist ~1_val~%

			v = EnergyResistance;

			if ( v != 0 )
				list.Add( 1060446, v.ToString() ); // energy resist ~1_val~%
		}

		/// <summary>
		/// Overridable. Determines whether the item will show <see cref="AddWeightProperty" />.
		/// </summary>
		public virtual bool DisplayWeight => true;

		/// <summary>
		/// Overridable. Displays cliloc 1072788-1072789.
		/// </summary>
		public virtual void AddWeightProperty( ObjectPropertyList list )
		{
			if ( m_Weight == 0 )
				return;

			int weight = PileWeight;
			if ( weight < 1 )
				weight = 1;

			if ( weight == 1 )
				list.Add( 1072788, weight.ToString() ); // Weight: ~1_WEIGHT~ stone
			else
				list.Add( 1072789, weight.ToString() ); // Weight: ~1_WEIGHT~ stones
		}

		/// <summary>
		/// Overridable. Adds header properties. By default, this invokes <see cref="AddNameProperty" />, <see cref="AddBlessedForProperty" /> (if applicable), and <see cref="AddLootTypeProperty" /> (if <see cref="DisplayLootType" />).
		/// </summary>
		public virtual void AddNameProperties( ObjectPropertyList list )
		{
			LocalizedText nameProperty = GetNameProperty();
			nameProperty.AddTo( list );

			if ( DisplayWeight )
				AddWeightProperty( list );

			if ( IsSecure )
				AddSecureProperty( list );
			else if ( IsLockedDown )
				AddLockedDownProperty( list );

			if ( m_BlessedFor != null && !m_BlessedFor.Deleted )
				AddBlessedForProperty( list, m_BlessedFor );

			if ( DisplayLootType )
				AddLootTypeProperty( list );

			if ( RequiredRace == Race.Elf )
				list.Add( 1075086 ); // Elves Only
			else if ( RequiredRace == Race.Gargoyle )
				list.Add( 1111709 ); // Gargoyles Only

			if ( QuestItem )
				list.Add( 1072351 ); // Quest Item

			AddLabelsProperty( list );

			AppendChildNameProperties( list );
		}

		/// <summary>
		/// Overridable. Adds the labels property to the given <see cref="ObjectPropertyList" />.
		/// </summary>
		public virtual void AddLabelsProperty( ObjectPropertyList list )
		{
			if ( m_Labels == null )
				return;

			List<string> validLabels = new List<string>();

			for ( int i = 0; i < m_Labels.Length; i++ )
			{
				if ( m_Labels[i] != null )
					validLabels.Add( m_Labels[i] );
			}

			string args = string.Join( "<br>", validLabels.ToArray() );

			if ( !string.IsNullOrEmpty( args ) )
				list.Add( 1070722, args );
		}

		/// <summary>
		/// Overridable. Adds the "Locked Down & Secure" property to the given <see cref="ObjectPropertyList" />.
		/// </summary>
		public virtual void AddSecureProperty( ObjectPropertyList list )
		{
			list.Add( 501644 ); // locked down & secure
		}

		/// <summary>
		/// Overridable. Adds the "Locked Down" property to the given <see cref="ObjectPropertyList" />.
		/// </summary>
		public virtual void AddLockedDownProperty( ObjectPropertyList list )
		{
			list.Add( 501643 ); // locked down
		}

		/// <summary>
		/// Overridable. Adds the "Blessed for ~1_NAME~" property to the given <see cref="ObjectPropertyList" />.
		/// </summary>
		public virtual void AddBlessedForProperty( ObjectPropertyList list, Mobile m )
		{
			list.Add( 1062203, "{0}", m.Name ); // Blessed for ~1_NAME~
		}

		/// <summary>
		/// Overridable. Fills an <see cref="ObjectPropertyList" /> with everything applicable. By default, this invokes <see cref="AddNameProperties" />, then <see cref="Item.GetChildProperties">Item.GetChildProperties</see> or <see cref="Mobile.GetChildProperties">Mobile.GetChildProperties</see>. This method should be overriden to add any custom properties.
		/// </summary>
		public virtual void GetProperties( ObjectPropertyList list )
		{
			AddNameProperties( list );
		}

		/// <summary>
		/// Overridable. Event invoked when a child (<paramref name="item" />) is building it's <see cref="ObjectPropertyList" />. Recursively calls <see cref="Item.GetChildProperties">Item.GetChildProperties</see> or <see cref="Mobile.GetChildProperties">Mobile.GetChildProperties</see>.
		/// </summary>
		public virtual void GetChildProperties( ObjectPropertyList list, Item item )
		{
			if ( m_Parent is Item )
				( (Item) m_Parent ).GetChildProperties( list, item );
			else if ( m_Parent is Mobile )
				( (Mobile) m_Parent ).GetChildProperties( list, item );
		}

		/// <summary>
		/// Overridable. Event invoked when a child (<paramref name="item" />) is building it's Name <see cref="ObjectPropertyList" />. Recursively calls <see cref="Item.GetChildProperties">Item.GetChildNameProperties</see> or <see cref="Mobile.GetChildProperties">Mobile.GetChildNameProperties</see>.
		/// </summary>
		public virtual void GetChildNameProperties( ObjectPropertyList list, Item item )
		{
			if ( m_Parent is Item )
				( (Item) m_Parent ).GetChildNameProperties( list, item );
			else if ( m_Parent is Mobile )
				( (Mobile) m_Parent ).GetChildNameProperties( list, item );
		}

		public virtual bool IsChildVisibleTo( Mobile m, Item child )
		{
			return true;
		}

		public void Bounce( Mobile from )
		{
			if ( m_Parent is Item )
				( (Item) m_Parent ).RemoveItem( this );
			else if ( m_Parent is Mobile )
				( (Mobile) m_Parent ).RemoveItem( this );

			m_Parent = null;

			if ( m_Bounce != null )
			{
				object parent = m_Bounce.m_Parent;

				if ( parent is Item && !( (Item) parent ).Deleted )
				{
					Item p = (Item) parent;
					object root = p.RootParent;
					if ( p.IsAccessibleTo( from ) && ( !( root is Mobile ) || ( (Mobile) root ).CheckNonlocalDrop( from, this, p ) ) )
					{
						Location = m_Bounce.m_Location;
						p.AddItem( this );
					}
					else
					{
						MoveToWorld( from.Location, from.Map );
					}
				}
				else if ( parent is Mobile && !( (Mobile) parent ).Deleted )
				{
					if ( !( (Mobile) parent ).EquipItem( this ) )
						MoveToWorld( m_Bounce.m_WorldLoc, m_Bounce.m_Map );
				}
				else
				{
					MoveToWorld( m_Bounce.m_WorldLoc, m_Bounce.m_Map );
				}
			}
			else
			{
				MoveToWorld( from.Location, from.Map );
			}

			ClearBounce();
		}

		/// <summary>
		/// Overridable. Method checked to see if this item may be equiped while casting a spell. By default, this returns false. It is overriden on spellbook and spell channeling weapons or shields.
		/// </summary>
		/// <returns>True if it may, false if not.</returns>
		/// <example>
		/// <code>
		///	public override bool AllowEquipedCast( Mobile from )
		///	{
		///		if ( from.Int &gt;= 100 )
		///			return true;
		///
		///		return base.AllowEquipedCast( from );
		/// }</code>
		///
		/// When placed in an Item script, the item may be cast when equiped if the <paramref name="from" /> has 100 or more intelligence. Otherwise, it will drop to their backpack.
		/// </example>
		public virtual bool AllowEquipedCast( Mobile from )
		{
			return false;
		}

		public virtual bool CheckConflictingLayer( Mobile m, Item item, Layer layer )
		{
			return ( m_Layer == layer );
		}

		public virtual bool CanEquip( Mobile m )
		{
			if ( QuestItem )
				return false;

			if ( m.Race == Race.Gargoyle && !WearableByGargoyles )
			{
				m.LocalOverheadMessage( MessageType.Regular, 0x3B2, 1111708 ); // Gargoyles can't wear this.
				return false;
			}
			else if ( RequiredRace == Race.Gargoyle && m.Race != Race.Gargoyle )
			{
				m.LocalOverheadMessage( MessageType.Regular, 0x3B2, 1111707 ); // Only gargoyles can wear this.
				return false;
			}
			else if ( RequiredRace == Race.Elf && m.Race != Race.Elf && !m.OverridesRaceReq )
			{
				m.SendLocalizedMessage( 1072203 ); // Only Elves may use this.
				return false;
			}

			return ( m_Layer != Layer.Invalid && m.FindItemOnLayer( m_Layer ) == null );
		}

		public virtual void GetChildContextMenuEntries( Mobile from, List<ContextMenuEntry> list, Item item )
		{
			if ( m_Parent is Item )
				( (Item) m_Parent ).GetChildContextMenuEntries( from, list, item );
			else if ( m_Parent is Mobile )
				( (Mobile) m_Parent ).GetChildContextMenuEntries( from, list, item );
		}

		public virtual void GetContextMenuEntries( Mobile from, List<ContextMenuEntry> list )
		{
			if ( m_Parent is Item )
				( (Item) m_Parent ).GetChildContextMenuEntries( from, list, this );
			else if ( m_Parent is Mobile )
				( (Mobile) m_Parent ).GetChildContextMenuEntries( from, list, this );
		}

		public virtual bool VerifyMove( Mobile from )
		{
			return Movable;
		}

		public virtual DeathMoveResult OnParentDeath( Mobile parent )
		{
			if ( !Movable )
				return DeathMoveResult.RemainEquiped;
			else if ( parent.KeepsItemsOnDeath )
				return DeathMoveResult.MoveToBackpack;
			else if ( CheckBlessed( parent ) )
				return DeathMoveResult.MoveToBackpack;
			else if ( CheckNewbied() && parent.Kills < 5 )
				return DeathMoveResult.MoveToBackpack;
			else if ( parent.Player && NonTransferable )
				return DeathMoveResult.MoveToBackpack;
			else
				return DeathMoveResult.MoveToCorpse;
		}

		public virtual DeathMoveResult OnInventoryDeath( Mobile parent )
		{
			if ( !Movable )
				return DeathMoveResult.MoveToBackpack;
			else if ( parent.KeepsItemsOnDeath )
				return DeathMoveResult.MoveToBackpack;
			else if ( CheckBlessed( parent ) )
				return DeathMoveResult.MoveToBackpack;
			else if ( CheckNewbied() && parent.Kills < 5 )
				return DeathMoveResult.MoveToBackpack;
			else if ( parent.Player && NonTransferable )
				return DeathMoveResult.MoveToBackpack;
			else
				return DeathMoveResult.MoveToCorpse;
		}

		/// <summary>
		/// Moves the Item to <paramref name="location" />. The Item does not change maps.
		/// </summary>
		public virtual void MoveToWorld( Point3D location )
		{
			MoveToWorld( location, m_Map );
		}

		public void LabelTo( Mobile to, int number )
		{
			to.Send( new MessageLocalized( Serial, m_ItemID, MessageType.Label, 0x3B2, 3, number, "", "" ) );
		}

		public void LabelTo( Mobile to, int number, string args )
		{
			to.Send( new MessageLocalized( Serial, m_ItemID, MessageType.Label, 0x3B2, 3, number, "", args ) );
		}

		public void LabelTo( Mobile to, string text )
		{
			to.Send( new UnicodeMessage( Serial, m_ItemID, MessageType.Label, 0x3B2, 3, "ENU", "", text ) );
		}

		public void LabelTo( Mobile to, string format, params object[] args )
		{
			LabelTo( to, String.Format( format, args ) );
		}

		public void LabelToAffix( Mobile to, int number, AffixType type, string affix )
		{
			to.Send( new MessageLocalizedAffix( Serial, m_ItemID, MessageType.Label, 0x3B2, 3, number, "", type, affix, "" ) );
		}

		public void LabelToAffix( Mobile to, int number, AffixType type, string affix, string args )
		{
			to.Send( new MessageLocalizedAffix( Serial, m_ItemID, MessageType.Label, 0x3B2, 3, number, "", type, affix, args ) );
		}

		public bool AtWorldPoint( int x, int y )
		{
			return ( m_Parent == null && m_Location.X == x && m_Location.Y == y );
		}

		public bool AtPoint( int x, int y )
		{
			return ( m_Location.X == x && m_Location.Y == y );
		}

		/// <summary>
		/// Moves the Item to a given <paramref name="location" /> and <paramref name="map" />.
		/// </summary>
		public void MoveToWorld( Point3D location, Map map )
		{
			if ( Deleted )
				return;

			Point3D oldLocation = GetWorldLocation();
			Point3D oldRealLocation = m_Location;

			SetLastMoved();

			if ( Parent is Mobile )
				( (Mobile) Parent ).RemoveItem( this );
			else if ( Parent is Item )
				( (Item) Parent ).RemoveItem( this );

			if ( m_Map != map )
			{
				Map old = m_Map;

				if ( m_Map != null )
				{
					m_Map.OnLeave( this );

					if ( oldLocation.X != 0 )
					{
						Packet remPacket = null;

						foreach ( NetState state in m_Map.GetClientsInRange( oldLocation, GetMaxUpdateRange() ) )
						{
							Mobile m = state.Mobile;

							if ( m.InRange( oldLocation, GetUpdateRange( m ) ) )
							{
								if ( remPacket == null )
									remPacket = this.RemovePacket;

								state.Send( remPacket );
							}
						}
					}
				}

				m_Location = location;
				this.OnLocationChange( oldRealLocation );

				ReleaseWorldPackets();

				if ( m_Items != null )
				{
					for ( int i = 0; i < m_Items.Count; ++i )
						( (Item) m_Items[i] ).Map = map;
				}

				m_Map = map;

				if ( m_Map != null )
					m_Map.OnEnter( this );

				OnMapChange();

				EventSink.InvokeMapChanged( new MapChangedEventArgs( this, old ) );

				if ( m_Map != null )
				{
					foreach ( NetState state in m_Map.GetClientsInRange( m_Location, GetMaxUpdateRange() ) )
					{
						Mobile m = state.Mobile;

						if ( m.CanSee( this ) && m.InRange( m_Location, GetUpdateRange( m ) ) )
							SendInfoTo( state );
					}
				}

				RemDelta( ItemDelta.Update );

				if ( old == null || old == Map.Internal )
					InvalidateProperties();
			}
			else if ( m_Map != null )
			{
				if ( oldLocation.X != 0 )
				{
					Packet removeThis = null;

					foreach ( NetState state in m_Map.GetClientsInRange( oldLocation, GetMaxUpdateRange() ) )
					{
						Mobile m = state.Mobile;

						if ( !m.InRange( location, GetUpdateRange( m ) ) )
						{
							if ( removeThis == null )
								removeThis = this.RemovePacket;

							state.Send( removeThis );
						}
					}
				}

				Point3D oldInternalLocation = m_Location;

				m_Location = location;
				this.OnLocationChange( oldRealLocation );

				ReleaseWorldPackets();

				foreach ( NetState state in m_Map.GetClientsInRange( m_Location, GetMaxUpdateRange() ) )
				{
					Mobile m = state.Mobile;

					if ( m.CanSee( this ) && m.InRange( m_Location, GetUpdateRange( m ) ) )
						SendInfoTo( state );
				}

				m_Map.OnMove( oldInternalLocation, this );

				RemDelta( ItemDelta.Update );
			}
			else
			{
				Map = map;
				Location = location;
			}
		}

		IPoint3D IEntity.Location => m_Location;

		IMap IEntity.Map => m_Map;

		/// <summary>
		/// Has the item been deleted?
		/// </summary>
		public bool Deleted => GetFlag( ImplFlag.Deleted );

		[CommandProperty( AccessLevel.GameMaster )]
		public LootType LootType
		{
			get
			{
				return m_LootType;
			}
			set
			{
				if ( m_LootType != value )
				{
					m_LootType = value;

					if ( DisplayLootType )
						InvalidateProperties();
				}
			}
		}

		[Obsolete( "Use LootType instead", true )]
		public bool Newbied
		{
			get { return ( m_LootType == LootType.Newbied ); }
			set { m_LootType = ( value ? LootType.Newbied : LootType.Regular ); }
		}

		public static TimeSpan DefaultDecayTime { get; set; } = TimeSpan.FromHours( 1.0 );

		[CommandProperty( AccessLevel.GameMaster )]
		public virtual TimeSpan DecayTime => DefaultDecayTime;

		[CommandProperty( AccessLevel.GameMaster )]
		public virtual bool Decays => ( Movable && Visible );

		public virtual bool OnDecay()
		{
			return ( Decays && Parent == null && Map != Map.Internal && Region.Find( Location, Map ).OnDecay( this ) );
		}

		public void SetLastMoved()
		{
			LastMoved = DateTime.UtcNow;
		}

		public DateTime LastMoved { get; set; }

		public virtual bool StackWith( Mobile from, Item dropped )
		{
			return StackWith( from, dropped, true );
		}

		public virtual bool StackWith( Mobile from, Item dropped, bool playSound )
		{
			if ( Stackable && dropped.Stackable && dropped.GetType() == GetType() && dropped.ItemID == ItemID && dropped.Hue == Hue && ( dropped.Amount + Amount ) <= 60000 )
			{
				if ( m_LootType != dropped.m_LootType )
					m_LootType = LootType.Regular;

				Amount += dropped.Amount;

				dropped.Delete();

				if ( playSound && from != null )
				{
					int soundID = GetDropSound();

					if ( soundID == -1 )
						soundID = 0x42;

					from.SendSound( soundID, GetWorldLocation() );
				}

				return true;
			}

			return false;
		}

		public virtual bool OnDragDrop( Mobile from, Item dropped )
		{
			if ( Parent is Container )
				return ( (Container) Parent ).OnStackAttempt( from, this, dropped );

			return StackWith( from, dropped );
		}

		public Rectangle2D GetGraphicBounds()
		{
			int itemID = m_ItemID;
			bool doubled = m_Amount > 1;

			if ( itemID >= 0xEEA && itemID <= 0xEF2 ) // Are we coins?
			{
				int coinBase = ( itemID - 0xEEA ) / 3;
				coinBase *= 3;
				coinBase += 0xEEA;

				doubled = false;

				if ( m_Amount <= 1 )
				{
					// A single coin
					itemID = coinBase;
				}
				else if ( m_Amount <= 5 )
				{
					// A stack of coins
					itemID = coinBase + 1;
				}
				else // m_Amount > 5
				{
					// A pile of coins
					itemID = coinBase + 2;
				}
			}

			Rectangle2D bounds = ItemBounds.Table[itemID & TileData.MaxItemValue];

			if ( doubled )
				bounds.Set( bounds.X, bounds.Y, bounds.Width + 5, bounds.Height + 5 );

			return bounds;
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Stackable
		{
			get { return GetFlag( ImplFlag.Stackable ); }
			set { SetFlag( ImplFlag.Stackable, value ); }
		}

		public Packet RemovePacket
		{
			get
			{
				if ( m_RemovePacket == null )
				{
					m_RemovePacket = new RemoveItem( this );
					m_RemovePacket.SetStatic();
				}

				return m_RemovePacket;
			}
		}

		public Packet OPLPacket
		{
			get
			{
				if ( m_OplPacket == null )
				{
					m_OplPacket = new OPLInfo( PropertyList );
					m_OplPacket.SetStatic();
				}

				return m_OplPacket;
			}
		}

		public ObjectPropertyListPacket PropertyList
		{
			get
			{
				if ( m_PropertyList == null )
				{
					m_PropertyList = new ObjectPropertyListPacket( this );

					GetProperties( m_PropertyList );
					AppendChildProperties( m_PropertyList );

					m_PropertyList.Terminate();
					m_PropertyList.SetStatic();
				}

				return m_PropertyList;
			}
		}

		public virtual void AppendChildProperties( ObjectPropertyList list )
		{
			if ( m_Parent is Item )
				( (Item) m_Parent ).GetChildProperties( list, this );
			else if ( m_Parent is Mobile )
				( (Mobile) m_Parent ).GetChildProperties( list, this );
		}

		public virtual void AppendChildNameProperties( ObjectPropertyList list )
		{
			if ( m_Parent is Item )
				( (Item) m_Parent ).GetChildNameProperties( list, this );
			else if ( m_Parent is Mobile )
				( (Mobile) m_Parent ).GetChildNameProperties( list, this );
		}

		public void ClearProperties()
		{
			if ( m_PropertyList != null )
			{
				m_PropertyList.Release();
				m_PropertyList = null;
			}

			if ( m_OplPacket != null )
			{
				m_OplPacket.Release();
				m_OplPacket = null;
			}
		}

		public void InvalidateProperties()
		{
			if ( !ObjectPropertyListPacket.Enabled )
				return;

			if ( m_Map != null && m_Map != Map.Internal && !World.Loading )
			{
				ObjectPropertyListPacket oldList = m_PropertyList;
				m_PropertyList = null;
				ObjectPropertyListPacket newList = PropertyList;

				if ( oldList == null || oldList.Hash != newList.Hash )
				{
					Packet.Release( ref m_OplPacket );
					Delta( ItemDelta.Properties );
				}
			}
			else
			{
				ClearProperties();
			}
		}

		public Packet WorldPacket
		{
			get
			{
				// This needs to be invalidated when any of the following changes:
				//  - ItemID
				//  - Amount
				//  - Location
				//  - Hue
				//  - Packet Flags
				//  - Direction
				//  - LightType

				if ( m_WorldPacket == null )
				{
					m_WorldPacket = new WorldItem( this );
					m_WorldPacket.SetStatic();
				}

				return m_WorldPacket;
			}
		}

		public void ReleaseWorldPackets()
		{
			Packet.Release( ref m_WorldPacket );
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Visible
		{
			get { return GetFlag( ImplFlag.Visible ); }
			set
			{
				if ( GetFlag( ImplFlag.Visible ) != value )
				{
					SetFlag( ImplFlag.Visible, value );

					ReleaseWorldPackets();

					if ( m_Map != null )
					{
						Point3D worldLoc = GetWorldLocation();

						foreach ( NetState state in m_Map.GetClientsInRange( worldLoc, GetMaxUpdateRange() ) )
						{
							Mobile m = state.Mobile;

							if ( !m.CanSee( this ) && m.InRange( worldLoc, GetUpdateRange( m ) ) )
								state.Send( RemovePacket );
						}
					}

					Delta( ItemDelta.Update );
				}
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Movable
		{
			get { return GetFlag( ImplFlag.Movable ); }
			set
			{
				if ( GetFlag( ImplFlag.Movable ) != value )
				{
					SetFlag( ImplFlag.Movable, value );
					ReleaseWorldPackets();
					Delta( ItemDelta.Update );
				}
			}
		}

		public virtual bool ForceShowProperties => false;

		public virtual int GetPacketFlags()
		{
			int flags = 0;

			if ( !Visible )
				flags |= 0x80;

			if ( Movable || ForceShowProperties )
				flags |= 0x20;

			return flags;
		}

		public virtual bool OnMoveOff( Mobile m )
		{
			return true;
		}

		public virtual bool OnMoveOver( Mobile m )
		{
			return true;
		}

		public virtual bool HandlesOnMovement => false;

		public virtual void OnMovement( Mobile m, Point3D oldLocation )
		{
		}

		public void Internalize()
		{
			MoveToWorld( Point3D.Zero, Map.Internal );
		}

		public virtual void OnMapChange()
		{
		}

		public virtual void OnRemoved( object parent )
		{
		}

		public virtual void OnAdded( object parent )
		{
		}

		[CommandProperty( AccessLevel.Counselor, AccessLevel.GameMaster )]
		public Map Map
		{
			get
			{
				return m_Map;
			}
			set
			{
				if ( m_Map != value )
				{
					Map old = m_Map;

					if ( m_Map != null )
					{
						m_Map.OnLeave( this );

						if ( m_Parent == null )
							SendRemovePacket();
					}

					if ( m_Items != null )
					{
						foreach ( Item item in m_Items )
							item.Map = value;
					}

					m_Map = value;

					if ( m_Map != null && m_Parent == null )
						m_Map.OnEnter( this );

					Delta( ItemDelta.Update );

					this.OnMapChange();

					EventSink.InvokeMapChanged( new MapChangedEventArgs( this, old ) );

					if ( old == null || old == Map.Internal )
						InvalidateProperties();
				}
			}
		}

		[Flags]
		private enum SaveFlag
		{
			None = 0x00000000,
			Direction = 0x00000001,
			Bounce = 0x00000002,
			LootType = 0x00000004,
			LocationFull = 0x00000008,
			ItemID = 0x00000010,
			Hue = 0x00000020,
			Amount = 0x00000040,
			Layer = 0x00000080,
			Name = 0x00000100,
			Parent = 0x00000200,
			Items = 0x00000400,
			WeightNot1or0 = 0x00000800,
			Map = 0x00001000,
			Visible = 0x00002000,
			Movable = 0x00004000,
			Stackable = 0x00008000,
			WeightIs0 = 0x00010000,
			LocationSByteZ = 0x00020000,
			LocationShortXY = 0x00040000,
			LocationByteXY = 0x00080000,
			ImplFlags = 0x00100000,
			#region Labels
			Labels = 0x00200000,
			#endregion
			BlessedFor = 0x00400000,
			HeldBy = 0x00800000,
			IntWeight = 0x01000000,
			SavedFlags = 0x02000000,
			LightType = 0x04000000,
			LabelNumber = 0x08000000
		}

		private static bool GetSaveFlag( SaveFlag flags, SaveFlag toGet )
		{
			return ( ( flags & toGet ) != 0 );
		}

		int ISerializable.TypeReference => m_TypeRef;

		Serial ISerializable.SerialIdentity => Serial;

		public virtual void Serialize( GenericWriter writer )
		{
			writer.Write( 14 ); // version

			writer.Write( m_InstanceID );

			SaveFlag flags = SaveFlag.None;

			int x = m_Location.X, y = m_Location.Y, z = m_Location.Z;

			if ( x != 0 || y != 0 || z != 0 )
			{
				if ( x >= short.MinValue && x <= short.MaxValue && y >= short.MinValue && y <= short.MaxValue && z >= sbyte.MinValue && z <= sbyte.MaxValue )
				{
					if ( x != 0 || y != 0 )
					{
						if ( x >= byte.MinValue && x <= byte.MaxValue && y >= byte.MinValue && y <= byte.MaxValue )
							flags |= SaveFlag.LocationByteXY;
						else
							flags |= SaveFlag.LocationShortXY;
					}

					if ( z != 0 )
						flags |= SaveFlag.LocationSByteZ;
				}
				else
				{
					flags |= SaveFlag.LocationFull;
				}
			}

			if ( m_Direction != Direction.North )
				flags |= SaveFlag.Direction;
			if ( m_LightType != LightType.ArchedWindowEast )
				flags |= SaveFlag.LightType;
			if ( m_Bounce != null )
				flags |= SaveFlag.Bounce;
			if ( m_LootType != LootType.Regular )
				flags |= SaveFlag.LootType;
			if ( m_ItemID != 0 )
				flags |= SaveFlag.ItemID;
			if ( PrivateHue != 0 )
				flags |= SaveFlag.Hue;
			if ( m_Amount != 1 )
				flags |= SaveFlag.Amount;
			if ( m_Layer != Layer.Invalid )
				flags |= SaveFlag.Layer;
			if ( m_Name != null )
				flags |= SaveFlag.Name;
			if ( m_Parent != null )
				flags |= SaveFlag.Parent;
			if ( m_Items != null && m_Items.Count > 0 )
				flags |= SaveFlag.Items;
			if ( m_Map != Map.Internal )
				flags |= SaveFlag.Map;
			if ( m_BlessedFor != null && !m_BlessedFor.Deleted )
				flags |= SaveFlag.BlessedFor;
			if ( HeldBy != null && !HeldBy.Deleted )
				flags |= SaveFlag.HeldBy;
			#region Labels
			if ( m_Labels != null )
				flags |= SaveFlag.Labels;
			#endregion
			if ( SavedFlags != 0 )
				flags |= SaveFlag.SavedFlags;

			if ( m_Weight == 0.0 )
			{
				flags |= SaveFlag.WeightIs0;
			}
			else if ( m_Weight != 1.0 )
			{
				if ( m_Weight == (int) m_Weight )
					flags |= SaveFlag.IntWeight;
				else
					flags |= SaveFlag.WeightNot1or0;
			}

			if ( m_LabelNumber != 0 )
				flags |= SaveFlag.LabelNumber;

			ImplFlag implFlags = ( m_Flags & ( ImplFlag.Visible | ImplFlag.Movable | ImplFlag.Stackable | ImplFlag.Insured | ImplFlag.PayedInsurance | ImplFlag.QuestItem ) );

			if ( implFlags != ( ImplFlag.Visible | ImplFlag.Movable ) )
				flags |= SaveFlag.ImplFlags;

			writer.Write( (int) flags );

			/* begin last moved time optimization */
			long ticks = LastMoved.Ticks;
			long now = DateTime.UtcNow.Ticks;

			TimeSpan d;

			try { d = new TimeSpan( ticks - now ); }
			catch { if ( ticks < now ) d = TimeSpan.MaxValue; else d = TimeSpan.MaxValue; }

			double minutes = -d.TotalMinutes;

			if ( minutes < int.MinValue )
				minutes = int.MinValue;
			else if ( minutes > int.MaxValue )
				minutes = int.MaxValue;

			writer.WriteEncodedInt( (int) minutes );
			/* end */

			if ( GetSaveFlag( flags, SaveFlag.Direction ) )
				writer.Write( (byte) m_Direction );

			if ( GetSaveFlag( flags, SaveFlag.LightType ) )
				writer.Write( (byte) m_LightType );

			if ( GetSaveFlag( flags, SaveFlag.Bounce ) )
				BounceInfo.Serialize( m_Bounce, writer );

			if ( GetSaveFlag( flags, SaveFlag.LootType ) )
				writer.Write( (byte) m_LootType );

			if ( GetSaveFlag( flags, SaveFlag.LocationFull ) )
			{
				writer.WriteEncodedInt( x );
				writer.WriteEncodedInt( y );
				writer.WriteEncodedInt( z );
			}
			else
			{
				if ( GetSaveFlag( flags, SaveFlag.LocationByteXY ) )
				{
					writer.Write( (byte) x );
					writer.Write( (byte) y );
				}
				else if ( GetSaveFlag( flags, SaveFlag.LocationShortXY ) )
				{
					writer.Write( (short) x );
					writer.Write( (short) y );
				}

				if ( GetSaveFlag( flags, SaveFlag.LocationSByteZ ) )
					writer.Write( (sbyte) z );
			}

			if ( GetSaveFlag( flags, SaveFlag.ItemID ) )
				writer.WriteEncodedInt( (int) m_ItemID );

			if ( GetSaveFlag( flags, SaveFlag.Hue ) )
				writer.WriteEncodedInt( (int) PrivateHue );

			if ( GetSaveFlag( flags, SaveFlag.Amount ) )
				writer.WriteEncodedInt( (int) m_Amount );

			if ( GetSaveFlag( flags, SaveFlag.Layer ) )
				writer.Write( (byte) m_Layer );

			if ( GetSaveFlag( flags, SaveFlag.Name ) )
				writer.Write( (string) m_Name );

			if ( GetSaveFlag( flags, SaveFlag.Parent ) )
			{
				if ( m_Parent is Mobile && !( (Mobile) m_Parent ).Deleted )
					writer.Write( ( (Mobile) m_Parent ).Serial );
				else if ( m_Parent is Item && !( (Item) m_Parent ).Deleted )
					writer.Write( ( (Item) m_Parent ).Serial );
				else
					writer.Write( (int) Serial.MinusOne );
			}

			if ( GetSaveFlag( flags, SaveFlag.Items ) )
				writer.WriteItemList( m_Items, false );

			if ( GetSaveFlag( flags, SaveFlag.IntWeight ) )
				writer.WriteEncodedInt( (int) m_Weight );
			else if ( GetSaveFlag( flags, SaveFlag.WeightNot1or0 ) )
				writer.Write( (double) m_Weight );

			if ( GetSaveFlag( flags, SaveFlag.Map ) )
				writer.Write( (Map) m_Map );

			if ( GetSaveFlag( flags, SaveFlag.ImplFlags ) )
				writer.WriteEncodedInt( (int) implFlags );

			if ( GetSaveFlag( flags, SaveFlag.BlessedFor ) )
				writer.Write( m_BlessedFor );

			if ( GetSaveFlag( flags, SaveFlag.HeldBy ) )
				writer.Write( HeldBy );

			if ( GetSaveFlag( flags, SaveFlag.LabelNumber ) )
				writer.Write( m_LabelNumber );

			#region Labels
			if ( GetSaveFlag( flags, SaveFlag.Labels ) )
			{
				writer.Write( (int) m_Labels.Length );

				for ( int i = 0; i < m_Labels.Length; i++ )
					writer.Write( (string) m_Labels[i] );
			}
			#endregion

			if ( GetSaveFlag( flags, SaveFlag.SavedFlags ) )
				writer.WriteEncodedInt( SavedFlags );
		}

		public IEnumerable<object> GetObjectsInRange( int range )
		{
			Map map = m_Map;

			if ( map == null )
				return Enumerable.Empty<object>();

			if ( m_Parent == null )
				return map.GetObjectsInRange( m_Location, range );

			return map.GetObjectsInRange( GetWorldLocation(), range );
		}

		public IEnumerable<Item> GetItemsInRange( int range )
		{
			Map map = m_Map;

			if ( map == null )
				return Enumerable.Empty<Item>();

			if ( m_Parent == null )
				return map.GetItemsInRange( m_Location, range );

			return map.GetItemsInRange( GetWorldLocation(), range );
		}

		public IEnumerable<Mobile> GetMobilesInRange( int range )
		{
			Map map = m_Map;

			if ( map == null )
				return Enumerable.Empty<Mobile>();

			if ( m_Parent == null )
				return map.GetMobilesInRange( m_Location, range );

			return map.GetMobilesInRange( GetWorldLocation(), range );
		}

		public IEnumerable<NetState> GetClientsInRange( int range )
		{
			Map map = m_Map;

			if ( map == null )
				return Enumerable.Empty<NetState>();

			if ( m_Parent == null )
				return map.GetClientsInRange( m_Location, range );

			return map.GetClientsInRange( GetWorldLocation(), range );
		}

		public static int LockedDownFlag { get; set; }

		public static int SecureFlag { get; set; }

		public bool IsLockedDown
		{
			get { return GetTempFlag( LockedDownFlag ); }
			set
			{
				SetTempFlag( LockedDownFlag, value );
				InvalidateProperties();

				OnLockDownChange();
			}
		}

		public virtual void OnLockDownChange()
		{
		}

		public bool IsSecure
		{
			get { return GetTempFlag( SecureFlag ); }
			set { SetTempFlag( SecureFlag, value ); InvalidateProperties(); }
		}

		private bool GetTempFlag( int flag )
		{
			return ( ( TempFlags & flag ) != 0 );
		}

		private void SetTempFlag( int flag, bool value )
		{
			if ( value )
				TempFlags |= flag;
			else
				TempFlags &= ~flag;
		}

		private bool GetSavedFlag( int flag )
		{
			return ( ( SavedFlags & flag ) != 0 );
		}

		private void SetSavedFlag( int flag, bool value )
		{
			if ( value )
				SavedFlags |= flag;
			else
				SavedFlags &= ~flag;
		}

		public virtual void Deserialize( GenericReader reader )
		{
			int version = reader.ReadInt();

			SetLastMoved();

			switch ( version )
			{
				case 14:
					{
						m_InstanceID = reader.ReadInt();

						SaveFlag flags = (SaveFlag) reader.ReadInt();

						int minutes = reader.ReadEncodedInt();

						try { LastMoved = DateTime.UtcNow - TimeSpan.FromMinutes( minutes ); }
						catch { LastMoved = DateTime.UtcNow; }

						if ( GetSaveFlag( flags, SaveFlag.Direction ) )
							m_Direction = (Direction) reader.ReadByte();

						if ( GetSaveFlag( flags, SaveFlag.LightType ) )
							m_LightType = (LightType) reader.ReadByte();

						if ( GetSaveFlag( flags, SaveFlag.Bounce ) )
							m_Bounce = BounceInfo.Deserialize( reader );

						if ( GetSaveFlag( flags, SaveFlag.LootType ) )
							m_LootType = (LootType) reader.ReadByte();

						int x = 0, y = 0, z = 0;

						if ( GetSaveFlag( flags, SaveFlag.LocationFull ) )
						{
							x = reader.ReadEncodedInt();
							y = reader.ReadEncodedInt();
							z = reader.ReadEncodedInt();
						}
						else
						{
							if ( GetSaveFlag( flags, SaveFlag.LocationByteXY ) )
							{
								x = reader.ReadByte();
								y = reader.ReadByte();
							}
							else if ( GetSaveFlag( flags, SaveFlag.LocationShortXY ) )
							{
								x = reader.ReadShort();
								y = reader.ReadShort();
							}

							if ( GetSaveFlag( flags, SaveFlag.LocationSByteZ ) )
								z = reader.ReadSByte();
						}

						m_Location = new Point3D( x, y, z );

						if ( GetSaveFlag( flags, SaveFlag.ItemID ) )
							m_ItemID = reader.ReadEncodedInt();

						if ( GetSaveFlag( flags, SaveFlag.Hue ) )
							PrivateHue = reader.ReadEncodedInt();

						if ( GetSaveFlag( flags, SaveFlag.Amount ) )
							m_Amount = reader.ReadEncodedInt();
						else
							m_Amount = 1;

						if ( GetSaveFlag( flags, SaveFlag.Layer ) )
							m_Layer = (Layer) reader.ReadByte();

						if ( GetSaveFlag( flags, SaveFlag.Name ) )
							m_Name = string.Intern( reader.ReadString() );

						if ( GetSaveFlag( flags, SaveFlag.Parent ) )
						{
							Serial parent = reader.ReadInt();

							if ( parent.IsMobile )
								m_Parent = World.FindMobile( parent );
							else if ( parent.IsItem )
								m_Parent = World.FindItem( parent );
							else
								m_Parent = null;

							if ( m_Parent == null && ( parent.IsMobile || parent.IsItem ) )
								Delete();
						}

						if ( GetSaveFlag( flags, SaveFlag.Items ) )
							m_Items = reader.ReadStrongItemList();

						if ( GetSaveFlag( flags, SaveFlag.IntWeight ) )
							m_Weight = reader.ReadEncodedInt();
						else if ( GetSaveFlag( flags, SaveFlag.WeightNot1or0 ) )
							m_Weight = reader.ReadDouble();
						else if ( GetSaveFlag( flags, SaveFlag.WeightIs0 ) )
							m_Weight = 0.0;
						else
							m_Weight = 1.0;

						if ( GetSaveFlag( flags, SaveFlag.Map ) )
							m_Map = reader.ReadMap();
						else
							m_Map = Map.Internal;

						if ( GetSaveFlag( flags, SaveFlag.Visible ) )
							SetFlag( ImplFlag.Visible, reader.ReadBool() );
						else
							SetFlag( ImplFlag.Visible, true );

						if ( GetSaveFlag( flags, SaveFlag.Movable ) )
							SetFlag( ImplFlag.Movable, reader.ReadBool() );
						else
							SetFlag( ImplFlag.Movable, true );

						if ( GetSaveFlag( flags, SaveFlag.Stackable ) )
							SetFlag( ImplFlag.Stackable, reader.ReadBool() );

						if ( GetSaveFlag( flags, SaveFlag.ImplFlags ) )
							m_Flags = (ImplFlag) reader.ReadEncodedInt();

						if ( GetSaveFlag( flags, SaveFlag.BlessedFor ) )
							m_BlessedFor = reader.ReadMobile();

						if ( GetSaveFlag( flags, SaveFlag.HeldBy ) )
							HeldBy = reader.ReadMobile();

						if ( GetSaveFlag( flags, SaveFlag.LabelNumber ) )
							m_LabelNumber = reader.ReadInt();

						#region Labels
						if ( GetSaveFlag( flags, SaveFlag.Labels ) )
						{
							int length = reader.ReadInt();

							m_Labels = new string[length];

							for ( int i = 0; i < length; i++ )
								m_Labels[i] = reader.ReadString();
						}
						#endregion

						if ( GetSaveFlag( flags, SaveFlag.SavedFlags ) )
							SavedFlags = reader.ReadEncodedInt();

						if ( m_Map != null && m_Parent == null )
							m_Map.OnEnter( this );

						break;
					}
			}

			if ( HeldBy != null )
				Timer.DelayCall( TimeSpan.Zero, new TimerCallback( FixHolding_Sandbox ) );
		}

		private void FixHolding_Sandbox()
		{
			Mobile heldBy = HeldBy;

			if ( heldBy != null )
			{
				if ( m_Bounce != null )
				{
					Bounce( heldBy );
				}
				else
				{
					heldBy.Holding = null;
					heldBy.AddToBackpack( this );
					ClearBounce();
				}
			}
		}

		public virtual int GetMaxUpdateRange()
		{
			return 18;
		}

		public virtual int GetUpdateRange( Mobile m )
		{
			return 18;
		}

		public virtual bool SendOplPacket => ObjectPropertyListPacket.Enabled && GraphicData == GraphicData.TileData;

		public virtual void SendInfoTo( NetState state )
		{
			state.Send( GetWorldPacketFor( state ) );

			if ( SendOplPacket )
				state.Send( OPLPacket );
		}

		public virtual GraphicData GraphicData => GraphicData.TileData;

		protected virtual Packet GetWorldPacketFor( NetState state )
		{
			return WorldPacket;
		}

		public void SetTotalGold( int value )
		{
			m_TotalGold = value;
		}

		public void SetTotalItems( int value )
		{
			m_TotalItems = value;
		}

		public void SetTotalWeight( int value )
		{
			m_TotalWeight = value;
		}

		public virtual bool IsVirtualItem => false;

		public virtual void UpdateTotals()
		{
			m_TotalGold = 0;
			m_TotalItems = 0;
			m_TotalWeight = 0;

			if ( m_Items == null )
				return;

			for ( int i = 0; i < m_Items.Count; ++i )
			{
				Item item = (Item) m_Items[i];

				item.UpdateTotals();

				if ( !( item is Container && ( (Container) item ).UseLockedRestriction && ( (Container) item ).IsLockedContainer ) )
					m_TotalGold += item.TotalGold;

				m_TotalItems += item.TotalItems;// + item.Items.Count;
				m_TotalWeight += item.TotalWeight + item.PileWeight;

				if ( item.IsVirtualItem )
					--m_TotalItems;
			}

			//if ( this is Gold )
			//	m_TotalGold += m_Amount;

			m_TotalItems += m_Items.Count;
		}

		private int m_LabelNumber;

		public virtual int LabelNumber
		{
			get
			{
				if ( m_LabelNumber != 0 )
					return m_LabelNumber;
				else
					return DefaultLabelNumber;
			}
			set
			{
				m_LabelNumber = value;
			}
		}

		public int DefaultLabelNumber
		{
			get
			{
				if ( m_ItemID < 0x4000 )
					return 1020000 + m_ItemID;
				else
					return 1078872 + m_ItemID;
			}
		}

		public bool IsNamed => m_LabelNumber != 0;

		[CommandProperty( AccessLevel.GameMaster )]
		public int TotalGold
		{
			get
			{
				return m_TotalGold;
			}
			set
			{
				if ( m_TotalGold != value )
				{
					if ( m_Parent is Item )
					{
						Item parent = (Item) m_Parent;

						if ( ( parent.TotalGold - m_TotalGold ) + value > 0 )
							parent.TotalGold = ( parent.TotalGold - m_TotalGold ) + value;
						else
							parent.TotalGold = 0;

					}
					else if ( m_Parent is Mobile && !( this is BankBox ) )
					{
						Mobile parent = (Mobile) m_Parent;

						if ( ( parent.TotalGold - m_TotalGold ) + value > 0 )
							parent.TotalGold = ( parent.TotalGold - m_TotalGold ) + value;
						else
							parent.TotalGold = 0;
					}

					m_TotalGold = value;
				}
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int TotalItems
		{
			get
			{
				return m_TotalItems;
			}
			set
			{
				if ( m_TotalItems != value )
				{
					if ( m_Parent is Item )
					{
						Item parent = (Item) m_Parent;

						parent.TotalItems = ( parent.TotalItems - m_TotalItems ) + value;
						parent.InvalidateProperties();
					}

					m_TotalItems = value;
					InvalidateProperties();
				}
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public virtual int TotalWeight
		{
			get
			{
				return m_TotalWeight;
			}
			set
			{
				if ( m_TotalWeight != value )
				{
					if ( m_Parent is Item )
					{
						Item parent = (Item) m_Parent;

						parent.TotalWeight = ( parent.TotalWeight - m_TotalWeight ) + value;
						parent.InvalidateProperties();
					}
					else if ( m_Parent is Mobile && !( this is BankBox ) )
					{
						Mobile parent = (Mobile) m_Parent;

						parent.TotalWeight = ( parent.TotalWeight - m_TotalWeight ) + value;
					}

					m_TotalWeight = value;
					InvalidateProperties();
				}
			}
		}

		[CommandProperty( AccessLevel.Counselor, AccessLevel.GameMaster )]
		public double Weight
		{
			get
			{
				return m_Weight;
			}
			set
			{
				if ( m_Weight != value )
				{
					int oldPileWeight = PileWeight;

					m_Weight = value;

					if ( m_Parent is Item )
					{
						Item parent = (Item) m_Parent;

						parent.TotalWeight = ( parent.TotalWeight - oldPileWeight ) + PileWeight;
						parent.InvalidateProperties();
					}
					else if ( m_Parent is Mobile && !( this is BankBox ) )
					{
						Mobile parent = (Mobile) m_Parent;

						parent.TotalWeight = ( parent.TotalWeight - oldPileWeight ) + PileWeight;
					}

					InvalidateProperties();
				}
			}
		}

		[CommandProperty( AccessLevel.Counselor, AccessLevel.GameMaster )]
		public int PileWeight => (int)Math.Ceiling( m_Weight * m_Amount );

		public virtual int HuedItemID => ( m_ItemID & TileData.MaxItemValue );

		public int PrivateHue { get; private set; }

		[Hue, CommandProperty( AccessLevel.GameMaster )]
		public virtual int Hue
		{
			get
			{
				return ( QuestItem ? QuestItemHue : PrivateHue );
			}
			set
			{
				if ( PrivateHue != value )
				{
					PrivateHue = value;
					ReleaseWorldPackets();

					Delta( ItemDelta.Update );
				}
			}
		}

		public virtual int QuestItemHue => 0x04EA;

		public virtual bool NonTransferable => QuestItem;

		public virtual void HandleInvalidTransfer( Mobile from )
		{
			if ( QuestItem )
				from.SendLocalizedMessage( 1074769 ); // An item must be in your backpack (and not in a container within) to be toggled as a quest item.
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public virtual Layer Layer
		{
			get
			{
				return m_Layer;
			}
			set
			{
				if ( m_Layer != value )
				{
					m_Layer = value;

					Delta( ItemDelta.EquipOnly );
				}
			}
		}

		public List<Item> Items
		{
			get
			{
				if ( m_Items == null )
					return EmptyItems;

				return m_Items;
			}
		}

		public object RootParent
		{
			get
			{
				object p = m_Parent;

				while ( p is Item )
				{
					Item item = (Item) p;

					if ( item.m_Parent == null )
					{
						break;
					}
					else
					{
						p = item.m_Parent;
					}
				}

				return p;
			}
		}

		public virtual void AddItem( Item item )
		{
			if ( item == null || item.Deleted || item.m_Parent == this )
			{
				return;
			}
			else if ( item == this )
			{
				log.Warning( "Adding item to itself: [0x{0:X} {1}].AddItem( [0x{2:X} {3}] )", this.Serial.Value, this.GetType().Name, item.Serial.Value, item.GetType().Name );
				log.Warning( "{0}", new System.Diagnostics.StackTrace() );
				return;
			}
			else if ( IsChildOf( item ) )
			{
				log.Warning( "Adding parent item to child: [0x{0:X} {1}].AddItem( [0x{2:X} {3}] )", this.Serial.Value, this.GetType().Name, item.Serial.Value, item.GetType().Name );
				log.Warning( "{0}", new System.Diagnostics.StackTrace() );
				return;
			}
			else if ( item.m_Parent is Mobile )
			{
				( (Mobile) item.m_Parent ).RemoveItem( item );
			}
			else if ( item.m_Parent is Item )
			{
				( (Item) item.m_Parent ).RemoveItem( item );
			}
			else
			{
				item.SendRemovePacket();
			}

			item.Parent = this;
			item.Map = m_Map;

			if ( m_Items == null )
				m_Items = new List<Item>( 4 );

			int oldCount = m_Items.Count;
			m_Items.Add( item );

			TotalItems = ( TotalItems - oldCount ) + m_Items.Count + item.TotalItems - ( item.IsVirtualItem ? 1 : 0 );
			TotalWeight += item.TotalWeight + item.PileWeight;

			if ( !( item is Container && ( (Container) item ).UseLockedRestriction && ( (Container) item ).IsLockedContainer ) )
				TotalGold += item.TotalGold;

			item.Delta( ItemDelta.Update );

			item.OnAdded( this );
			OnItemAdded( item );
		}

		private static readonly List<Item> m_DeltaQueue = new List<Item>();

		public void Delta( ItemDelta flags )
		{
			if ( m_Map == null || m_Map == Map.Internal )
				return;

			m_DeltaFlags |= flags;

			if ( !GetFlag( ImplFlag.InQueue ) )
			{
				SetFlag( ImplFlag.InQueue, true );

				m_DeltaQueue.Add( this );
			}

			//Core.WakeUp();
		}

		public void RemDelta( ItemDelta flags )
		{
			m_DeltaFlags &= ~flags;

			if ( GetFlag( ImplFlag.InQueue ) && m_DeltaFlags == ItemDelta.None )
			{
				SetFlag( ImplFlag.InQueue, false );

				m_DeltaQueue.Remove( this );
			}
		}

		public void ProcessDelta()
		{
			ItemDelta flags = m_DeltaFlags;

			SetFlag( ImplFlag.InQueue, false );
			m_DeltaFlags = ItemDelta.None;

			Map map = m_Map;

			if ( map != null && !Deleted )
			{
				bool sendOPLUpdate = ObjectPropertyListPacket.Enabled && ( flags & ItemDelta.Properties ) != 0;

				Container contParent = m_Parent as Container;

				if ( contParent != null && !contParent.IsPublicContainer )
				{
					if ( ( flags & ItemDelta.Update ) != 0 )
					{
						Point3D worldLoc = GetWorldLocation();

						Mobile rootParent = contParent.RootParent as Mobile;
						Mobile tradeRecip = null;

						if ( rootParent != null )
						{
							NetState ns = rootParent.NetState;

							if ( ns != null )
							{
								if ( rootParent.CanSee( this ) && rootParent.InRange( worldLoc, GetUpdateRange( rootParent ) ) )
								{
									ns.Send( new ContainerContentUpdate( this ) );

									if ( ObjectPropertyListPacket.Enabled )
										ns.Send( OPLPacket );
								}
							}
						}

						SecureTradeContainer stc = this.GetSecureTradeCont();

						if ( stc != null )
						{
							SecureTrade st = stc.Trade;

							if ( st != null )
							{
								Mobile test = st.From.Mobile;

								if ( test != null && test != rootParent )
									tradeRecip = test;

								test = st.To.Mobile;

								if ( test != null && test != rootParent )
									tradeRecip = test;

								if ( tradeRecip != null )
								{
									NetState ns = tradeRecip.NetState;

									if ( ns != null )
									{
										if ( tradeRecip.CanSee( this ) && tradeRecip.InRange( worldLoc, GetUpdateRange( tradeRecip ) ) )
										{
											ns.Send( new ContainerContentUpdate( this ) );

											if ( ObjectPropertyListPacket.Enabled )
												ns.Send( OPLPacket );
										}
									}
								}
							}
						}

						List<Mobile> openers = contParent.Openers;

						if ( openers != null )
						{
							for ( int i = 0; i < openers.Count; ++i )
							{
								Mobile mob = openers[i];

								int range = GetUpdateRange( mob );

								if ( mob.Map != map || !mob.InRange( worldLoc, range ) )
								{
									openers.RemoveAt( i-- );
								}
								else
								{
									if ( mob == rootParent || mob == tradeRecip )
										continue;

									NetState ns = mob.NetState;

									if ( ns != null )
									{
										if ( mob.CanSee( this ) )
										{
											ns.Send( new ContainerContentUpdate( this ) );

											if ( ObjectPropertyListPacket.Enabled )
												ns.Send( OPLPacket );
										}
									}
								}
							}

							if ( openers.Count == 0 )
								contParent.Openers = null;
						}

						return;
					}
				}

				if ( ( flags & ItemDelta.Update ) != 0 )
				{
					Point3D worldLoc = GetWorldLocation();

					foreach ( NetState state in map.GetClientsInRange( worldLoc, GetMaxUpdateRange() ) )
					{
						Mobile m = state.Mobile;

						if ( m.CanSee( this ) && m.InRange( worldLoc, GetUpdateRange( m ) ) )
						{
							if ( m_Parent == null )
							{
								SendInfoTo( state );
							}
							else
							{
								if ( m_Parent is Mobile )
									state.Send( new EquipUpdate( this ) );
								else if ( m_Parent is Item )
									state.Send( new ContainerContentUpdate( this ) );

								if ( ObjectPropertyListPacket.Enabled )
									state.Send( OPLPacket );
							}
						}
					}

					sendOPLUpdate = false;
				}
				else if ( ( flags & ItemDelta.EquipOnly ) != 0 )
				{
					if ( m_Parent is Mobile )
					{
						Packet p = null;
						Point3D worldLoc = GetWorldLocation();

						foreach ( NetState state in map.GetClientsInRange( worldLoc, GetMaxUpdateRange() ) )
						{
							Mobile m = state.Mobile;

							if ( m.CanSee( this ) && m.InRange( worldLoc, GetUpdateRange( m ) ) )
							{
								if ( p == null )
									p = Packet.Acquire( new EquipUpdate( this ) );

								state.Send( p );

								if ( ObjectPropertyListPacket.Enabled )
									state.Send( OPLPacket );
							}
						}

						Packet.Release( p );

						sendOPLUpdate = false;
					}
				}

				if ( sendOPLUpdate )
				{
					Point3D worldLoc = GetWorldLocation();

					foreach ( NetState state in map.GetClientsInRange( worldLoc, GetMaxUpdateRange() ) )
					{
						Mobile m = state.Mobile;

						if ( m.CanSee( this ) && m.InRange( worldLoc, GetUpdateRange( m ) ) )
							state.Send( OPLPacket );
					}
				}
			}
		}

		public static void ProcessDeltaQueue()
		{
			int count = m_DeltaQueue.Count;

			for ( int i = 0; i < m_DeltaQueue.Count; ++i )
			{
				Item item = m_DeltaQueue[i];

				try
				{
					item.ProcessDelta();
				}
				catch ( Exception e )
				{
					log.Error( "Exception disarmed in Item.ProcessDeltaQueue in {0}: {1}", item, e );
				}

				if ( i >= count )
					break;
			}

			if ( m_DeltaQueue.Count > 0 )
				m_DeltaQueue.Clear();
		}

		public virtual void OnDelete()
		{
		}

		public virtual void OnParentDeleted( object parent )
		{
			this.Delete();
		}

		public virtual void FreeCache()
		{
			ReleaseWorldPackets();

			ClearProperties();

			if ( m_RemovePacket != null )
			{
				m_RemovePacket.Release();
				m_RemovePacket = null;
			}
		}

		public virtual void Delete()
		{
			if ( Deleted )
				return;
			else if ( !World.OnDelete( this ) )
				return;

			OnDelete();

			if ( m_Items != null )
			{
				for ( int i = m_Items.Count - 1; i >= 0; --i )
				{
					if ( i < m_Items.Count )
						( (Item) m_Items[i] ).OnParentDeleted( this );
				}
			}

			SendRemovePacket();

			SetFlag( ImplFlag.Deleted, true );

			if ( Parent is Mobile )
				( (Mobile) Parent ).RemoveItem( this );
			else if ( Parent is Item )
				( (Item) Parent ).RemoveItem( this );

			ClearBounce();

			if ( Spawner != null )
			{
				Spawner.Remove( this );
				Spawner = null;
			}

			if ( m_Map != null )
			{
				m_Map.OnLeave( this );
				m_Map = null;
			}

			World.RemoveItem( this );

			OnAfterDelete();

			FreeCache();
		}

		public void PublicOverheadMessage( MessageType type, int hue, bool ascii, string text )
		{
			if ( m_Map != null )
			{
				Packet p = null;
				Point3D worldLoc = GetWorldLocation();

				foreach ( NetState state in m_Map.GetClientsInRange( worldLoc, GetMaxUpdateRange() ) )
				{
					Mobile m = state.Mobile;

					if ( m.CanSee( this ) && m.InRange( worldLoc, GetUpdateRange( m ) ) )
					{
						if ( p == null )
						{
							if ( ascii )
								p = new AsciiMessage( Serial, m_ItemID, type, hue, 3, m_Name, text );
							else
								p = new UnicodeMessage( Serial, m_ItemID, type, hue, 3, "ENU", m_Name, text );

							p.Acquire();
						}

						state.Send( p );
					}
				}

				Packet.Release( p );
			}
		}

		public void PublicOverheadMessage( MessageType type, int hue, int number, string args = "" )
		{
			if ( m_Map != null )
			{
				Packet p = null;
				Point3D worldLoc = GetWorldLocation();

				foreach ( NetState state in m_Map.GetClientsInRange( worldLoc, GetMaxUpdateRange() ) )
				{
					Mobile m = state.Mobile;

					if ( m.CanSee( this ) && m.InRange( worldLoc, GetUpdateRange( m ) ) )
					{
						if ( p == null )
							p = Packet.Acquire( new MessageLocalized( Serial, m_ItemID, type, hue, 3, number, m_Name, args ) );

						state.Send( p );
					}
				}

				Packet.Release( p );
			}
		}

		public virtual void OnAfterDelete()
		{
		}

		public virtual void RemoveItem( Item item )
		{
			if ( m_Items != null && m_Items.Contains( item ) )
			{
				item.SendRemovePacket();

				int oldCount = m_Items.Count;

				m_Items.Remove( item );

				TotalItems = ( TotalItems - oldCount ) + m_Items.Count - item.TotalItems + ( item.IsVirtualItem ? 1 : 0 );
				TotalWeight -= item.TotalWeight + item.PileWeight;

				if ( !( item is Container && ( (Container) item ).UseLockedRestriction && ( (Container) item ).IsLockedContainer ) )
				{
					if ( ( TotalGold - item.TotalGold ) > 0 )
						TotalGold -= item.TotalGold;
					else
						TotalGold = 0;
				}

				item.Parent = null;

				item.OnRemoved( this );
				OnItemRemoved( item );
			}
		}

		public virtual void OnAfterDuped( Item newItem )
		{
		}

		public virtual bool OnDragLift( Mobile from )
		{
			//Restore weight of arrows and bolts if they are reduced
			if ( this.GetType().ToString().Equals( "Server.Items.Arrow", StringComparison.OrdinalIgnoreCase )
				|| ( this.GetType().ToString().Equals( "Server.Items.Bolt", StringComparison.OrdinalIgnoreCase ) ) )
			{
				Item i = (Item) Activator.CreateInstance( this.GetType() );
				if ( this.Weight < i.Weight )
					this.Weight = i.Weight;
				i.Delete();
			}
			return true;
		}

		public virtual Race RequiredRace => null;

		public virtual bool WearableByGargoyles => RequiredRace == Race.Gargoyle;

		public virtual bool OnEquip( Mobile from )
		{
			return true;
		}

		public virtual void OnAfterEquip( Mobile from )
		{
		}

		public ISpawner Spawner { get; set; }

		public virtual void OnBeforeSpawn( Point3D location, Map m )
		{
		}

		public virtual void OnAfterSpawn()
		{
		}

		public virtual int PhysicalResistance => 0;

		public virtual int FireResistance => 0;

		public virtual int ColdResistance => 0;

		public virtual int PoisonResistance => 0;

		public virtual int EnergyResistance => 0;

		[CommandProperty( AccessLevel.Counselor )]
		public Serial Serial { get; }

		public virtual void OnLocationChange( Point3D oldLocation )
		{
		}

		[CommandProperty( AccessLevel.Counselor, AccessLevel.GameMaster )]
		public Point3D Location
		{
			get { return m_Location; }
			set { SetLocation( value, true ); }
		}

		public void SetLocation( Point3D newLocation, bool forceResend )
		{
			Point3D oldLocation = m_Location;

			if ( oldLocation != newLocation )
			{
				if ( m_Map != null )
				{
					if ( m_Parent == null )
					{
						if ( m_Location.X != 0 )
						{
							foreach ( NetState state in m_Map.GetClientsInRange( oldLocation, GetMaxUpdateRange() ) )
							{
								Mobile m = state.Mobile;

								if ( !m.InRange( newLocation, GetUpdateRange( m ) ) )
									state.Send( RemovePacket );
							}
						}

						m_Location = newLocation;
						ReleaseWorldPackets();

						SetLastMoved();

						foreach ( NetState state in m_Map.GetClientsInRange( m_Location, GetMaxUpdateRange() ) )
						{
							Mobile m = state.Mobile;

							if ( !m.InUpdateRange( newLocation ) )
								continue;

							if ( ( forceResend || !m.InUpdateRange( oldLocation ) ) && m.CanSee( this ) )
								SendInfoTo( state );
						}

						RemDelta( ItemDelta.Update );
					}
					else if ( m_Parent is Item )
					{
						m_Location = newLocation;
						ReleaseWorldPackets();

						Delta( ItemDelta.Update );
					}
					else
					{
						m_Location = newLocation;
						ReleaseWorldPackets();
					}

					if ( m_Parent == null )
						m_Map.OnMove( oldLocation, this );
				}
				else
				{
					m_Location = newLocation;
					ReleaseWorldPackets();
				}

				this.OnLocationChange( oldLocation );
			}
		}

		[CommandProperty( AccessLevel.Counselor, AccessLevel.GameMaster )]
		public virtual int X
		{
			get { return m_Location.X; }
			set { Location = new Point3D( value, m_Location.Y, m_Location.Z ); }
		}

		[CommandProperty( AccessLevel.Counselor, AccessLevel.GameMaster )]
		public virtual int Y
		{
			get { return m_Location.Y; }
			set { Location = new Point3D( m_Location.X, value, m_Location.Z ); }
		}

		[CommandProperty( AccessLevel.Counselor, AccessLevel.GameMaster )]
		public virtual int Z
		{
			get { return m_Location.Z; }
			set { Location = new Point3D( m_Location.X, m_Location.Y, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public virtual int ItemID
		{
			get
			{
				return m_ItemID;
			}
			set
			{
				if ( m_ItemID != value )
				{
					m_ItemID = value;
					ReleaseWorldPackets();

					InvalidateProperties();
					Delta( ItemDelta.Update );
				}
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public virtual string Name
		{
			get { return m_Name; }
			set
			{
				m_Name = value;

				if ( m_Name != null )
					m_Name = string.Intern( m_Name );

				InvalidateProperties();
			}
		}

		public object Parent
		{
			get { return m_Parent; }
			set
			{
				if ( m_Parent == value )
					return;

				object oldParent = m_Parent;

				m_Parent = value;

				if ( m_Map != null )
				{
					if ( oldParent != null && m_Parent == null )
						m_Map.OnEnter( this );
					else if ( m_Parent != null )
						m_Map.OnLeave( this );
				}
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public LightType Light
		{
			get
			{
				return m_LightType;
			}
			set
			{
				if ( m_LightType != value )
				{
					m_LightType = value;
					ReleaseWorldPackets();

					Delta( ItemDelta.Update );
				}
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Direction Direction
		{
			get
			{
				return m_Direction;
			}
			set
			{
				if ( m_Direction != value )
				{
					m_Direction = value;
					ReleaseWorldPackets();

					Delta( ItemDelta.Update );
				}
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool QuestItem
		{
			get { return GetFlag( ImplFlag.QuestItem ); }
			set
			{
				SetFlag( ImplFlag.QuestItem, value );

				InvalidateProperties();

				ReleaseWorldPackets();

				Delta( ItemDelta.Update );
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int Amount
		{
			get { return m_Amount; }
			set
			{
				int oldValue = m_Amount;

				if ( oldValue != value )
				{
					int oldPileWeight = PileWeight;

					m_Amount = value;
					ReleaseWorldPackets();

					if ( m_Parent is Item )
					{
						Item parent = (Item) m_Parent;

						parent.TotalWeight = ( parent.TotalWeight - oldPileWeight ) + PileWeight;
					}
					else if ( m_Parent is Mobile && !( this is BankBox ) )
					{
						Mobile parent = (Mobile) m_Parent;

						parent.TotalWeight = ( parent.TotalWeight - oldPileWeight ) + PileWeight;
					}

					OnAmountChange( oldValue );

					Delta( ItemDelta.Update );

					if ( oldValue > 1 || value > 1 )
						InvalidateProperties();

					if ( !Stackable && m_Amount > 1 )
						log.Info( "Warning: 0x{0:X}: Amount changed for non-stackable item '{2}'. ({1})", Serial.Value, m_Amount, GetType().Name );
				}
			}
		}

		protected virtual void OnAmountChange( int oldValue )
		{
		}

		public virtual bool HandlesOnSpeech => false;

		public virtual void OnSpeech( SpeechEventArgs e )
		{
		}

		public virtual bool OnDroppedToMobile( Mobile from, Mobile target )
		{
			if ( NonTransferable && from.Player && from.AccessLevel <= AccessLevel.GameMaster )
			{
				HandleInvalidTransfer( from );
				return false;
			}

			return true;
		}

		public virtual bool DropToMobile( Mobile from, Mobile target, Point3D p )
		{
			if ( Deleted || from.Deleted || target.Deleted || from.Map != target.Map || from.Map == null || target.Map == null )
				return false;
			else if ( from.AccessLevel < AccessLevel.GameMaster && !from.InRange( target.Location, 2 ) )
				return false;
			else if ( !from.CanSee( target ) || !from.InLOS( target ) )
				return false;
			else if ( !from.OnDroppedItemToMobile( this, target ) )
				return false;
			else if ( !OnDroppedToMobile( from, target ) )
				return false;
			else if ( !target.OnDragDrop( from, this ) )
				return false;
			else
				return true;
		}

		public virtual bool OnDroppedInto( Mobile from, Container target, Point3D p, byte gridloc )
		{
			if ( IsUnderYourFeet( from, target ) )
			{
				from.SendMessage( "No puedes mover eso, �est�s pisando la bolsa!" );
				return false;
			}
			else if ( !from.OnDroppedItemInto( this, target, p ) )
			{
				return false;
			}
			else if ( NonTransferable && from.Player && target != from.Backpack && from.AccessLevel <= AccessLevel.GameMaster )
			{
				HandleInvalidTransfer( from );
				return false;
			}

			return target.OnDragDropInto( from, this, p, gridloc );
		}

		public virtual bool OnDroppedOnto( Mobile from, Item target )
		{
			if ( IsUnderYourFeet( from, target ) )
			{
				from.SendMessage( "No puedes mover eso, �est�s pisando la bolsa!" );
				return false;
			}
			else if ( Deleted || from.Deleted || target.Deleted || from.Map != target.Map || from.Map == null || target.Map == null )
				return false;
			else if ( from.AccessLevel < AccessLevel.GameMaster && !from.InRange( target.GetWorldLocation(), 2 ) )
				return false;
			else if ( !from.CanSee( target ) || !from.InLOS( target ) )
				return false;
			else if ( !target.IsAccessibleTo( from ) )
				return false;
			else if ( !from.OnDroppedItemOnto( this, target ) )
				return false;
			else if ( NonTransferable && from.Player && from.AccessLevel <= AccessLevel.GameMaster )
			{
				HandleInvalidTransfer( from );
				return false;
			}
			else
				return target.OnDragDrop( from, this );
		}

		private bool IsUnderYourFeet( Mobile from, Item target )
		{
			if ( target.Parent == null )
				return from.Location == target.Location;
			else if ( !( target.Parent is Item ) )
				return false;
			else
				return IsUnderYourFeet( from, (Item) target.Parent );
		}

		public virtual bool DropToItem( Mobile from, Item target, Point3D p, byte gridloc )
		{
			if ( Deleted || from.Deleted || target.Deleted || from.Map != target.Map || from.Map == null || target.Map == null )
				return false;

			object root = target.RootParent;

			if ( from.AccessLevel < AccessLevel.GameMaster && !from.InRange( target.GetWorldLocation(), 2 ) )
				return false;
			else if ( !from.CanSee( target ) || !from.InLOS( target ) )
				return false;
			else if ( !target.IsAccessibleTo( from ) )
				return false;
			else if ( root is Mobile && !( (Mobile) root ).CheckNonlocalDrop( from, this, target ) )
				return false;
			else if ( !from.OnDroppedItemToItem( this, target, p ) )
				return false;
			else if ( target is Container && p.X != -1 && p.Y != -1 )
				return OnDroppedInto( from, (Container) target, p, gridloc );
			else
				return OnDroppedOnto( from, target );
		}

		public virtual bool OnDroppedToWorld( Mobile from, Point3D p )
		{
			if ( NonTransferable && from.Player && from.AccessLevel <= AccessLevel.GameMaster )
			{
				HandleInvalidTransfer( from );
				return false;
			}

			return true;
		}

		public virtual int GetLiftSound( Mobile from )
		{
			return 0x57;
		}

		private static int m_OpenSlots;

		public virtual bool DropToWorld( Mobile from, Point3D p )
		{
			if ( from.Region.CannotDrop )
			{
				from.SendLocalizedMessage( 1042276 ); // You cannot drop that there.
				return false;
			}

			if ( Deleted || from.Deleted || from.Map == null )
				return false;
			else if ( !from.InRange( p, 2 ) )
				return false;

			Map map = from.Map;

			if ( map == null )
				return false;

			int x = p.X, y = p.Y;
			int z = int.MinValue;

			int maxZ = from.Z + 16;

			Tile landTile = map.Tiles.GetLandTile( x, y );
			TileFlag landFlags = TileData.LandTable[landTile.ID & TileData.MaxLandValue].Flags;

			int landZ = 0, landAvg = 0, landTop = 0;
			map.GetAverageZ( x, y, ref landZ, ref landAvg, ref landTop );

			if ( !landTile.Ignored && ( landFlags & TileFlag.Impassable ) == 0 )
			{
				if ( landAvg <= maxZ )
					z = landAvg;
			}

			Tile[] tiles = map.Tiles.GetStaticTiles( x, y, true );

			for ( int i = 0; i < tiles.Length; ++i )
			{
				Tile tile = tiles[i];
				ItemData id = TileData.ItemTable[tile.ID & TileData.MaxItemValue];

				if ( !id.Surface )
					continue;

				int top = tile.Z + id.CalcHeight;

				if ( top > maxZ || top < z )
					continue;

				z = top;
			}

			List<Item> items = new List<Item>();

			foreach ( Item item in map.GetItemsInRange( p, 0 ) )
			{
				if ( item is BaseMulti || item.ItemID > TileData.MaxItemValue )
					continue;

				items.Add( item );

				ItemData id = item.ItemData;

				if ( !id.Surface )
					continue;

				int top = item.Z + id.CalcHeight;

				if ( top > maxZ || top < z )
					continue;

				z = top;
			}

			if ( z == int.MinValue )
				return false;

			if ( z > maxZ )
				return false;

			m_OpenSlots = ( 1 << 20 ) - 1;

			int surfaceZ = z;

			for ( int i = 0; i < tiles.Length; ++i )
			{
				Tile tile = tiles[i];
				ItemData id = TileData.ItemTable[tile.ID & TileData.MaxItemValue];

				int checkZ = tile.Z;
				int checkTop = checkZ + id.CalcHeight;

				if ( checkTop == checkZ && !id.Surface )
					++checkTop;

				int zStart = checkZ - z;
				int zEnd = checkTop - z;

				if ( zStart >= 20 || zEnd < 0 )
					continue;

				if ( zStart < 0 )
					zStart = 0;

				if ( zEnd > 19 )
					zEnd = 19;

				int bitCount = zEnd - zStart;

				m_OpenSlots &= ~( ( ( 1 << bitCount ) - 1 ) << zStart );
			}

			for ( int i = 0; i < items.Count; ++i )
			{
				Item item = items[i];
				ItemData id = item.ItemData;

				int checkZ = item.Z;
				int checkTop = checkZ + id.CalcHeight;

				if ( checkTop == checkZ && !id.Surface )
					++checkTop;

				int zStart = checkZ - z;
				int zEnd = checkTop - z;

				if ( zStart >= 20 || zEnd < 0 )
					continue;

				if ( zStart < 0 )
					zStart = 0;

				if ( zEnd > 19 )
					zEnd = 19;

				int bitCount = zEnd - zStart;

				m_OpenSlots &= ~( ( ( 1 << bitCount ) - 1 ) << zStart );
			}

			int height = ItemData.Height;

			if ( height == 0 )
				++height;

			if ( height > 20 )
				height = 20;

			int match = ( 1 << height ) - 1;
			bool okay = false;

			for ( int i = 0; i < 20; ++i )
			{
				if ( ( i + height ) > 20 )
					match >>= 1;

				okay = ( ( m_OpenSlots >> i ) & match ) == match;

				if ( okay )
				{
					z += i;
					break;
				}
			}

			if ( !okay )
				return false;

			height = ItemData.Height;

			if ( height == 0 )
				++height;

			if ( height > 20 )
				height = 20;

			if ( landAvg > z && ( z + height ) > landZ )
				return false;
			else if ( ( landFlags & TileFlag.Impassable ) != 0 && landAvg > surfaceZ && ( z + height ) > landZ )
				return false;

			for ( int i = 0; i < tiles.Length; ++i )
			{
				Tile tile = tiles[i];
				ItemData id = TileData.ItemTable[tile.ID & TileData.MaxItemValue];

				int checkZ = tile.Z;
				int checkTop = checkZ + id.CalcHeight;

				if ( checkTop > z && ( z + height ) > checkZ )
					return false;
				else if ( ( id.Surface || id.Impassable ) && checkTop > surfaceZ && ( z + height ) > checkZ )
					return false;
			}

			for ( int i = 0; i < items.Count; ++i )
			{
				Item item = items[i];
				ItemData id = item.ItemData;

				if ( ( item.Z + id.CalcHeight ) > z && ( z + height ) > item.Z )
					return false;
			}

			p = new Point3D( x, y, z );

			if ( from.Location == p )
				return false;
			else if ( !from.InLOS( new Point3D( x, y, z + 1 ) ) )
				return false;
			else if ( !from.OnDroppedItemToWorld( this, p ) )
				return false;
			else if ( !OnDroppedToWorld( from, p ) )
				return false;

			int soundID = GetDropSound();

			MoveToWorld( p, from.Map );

			from.SendSound( soundID == -1 ? 0x42 : soundID, GetWorldLocation() );

			return true;
		}

		public void SendRemovePacket()
		{
			if ( !Deleted && m_Map != null )
			{
				Packet p = null;
				Point3D worldLoc = GetWorldLocation();

				foreach ( NetState state in m_Map.GetClientsInRange( worldLoc, GetMaxUpdateRange() ) )
				{
					Mobile m = state.Mobile;

					if ( m.InRange( worldLoc, GetUpdateRange( m ) ) )
					{
						if ( p == null )
							p = this.RemovePacket;

						state.Send( p );
					}
				}
			}
		}

		public virtual int GetDropSound()
		{
			return -1;
		}

		public Point3D GetWorldLocation()
		{
			object root = RootParent;

			if ( root == null )
				return m_Location;
			else
				return new Point3D( ( (IEntity) root ).Location );

			//return root == null ? m_Location : new Point3D( (IPoint3D) root );
		}

		public virtual bool BlocksFit => false;

		public Point3D GetSurfaceTop()
		{
			object root = RootParent;

			if ( root == null )
				return new Point3D( m_Location.X, m_Location.Y, m_Location.Z + ( ItemData.Surface ? ItemData.CalcHeight : 0 ) );
			else
				return new Point3D( ( (IEntity) root ).Location );
		}

		public Point3D GetWorldTop()
		{
			object root = RootParent;

			if ( root == null )
				return new Point3D( m_Location.X, m_Location.Y, m_Location.Z + ItemData.CalcHeight );
			else
				return new Point3D( ( (IEntity) root ).Location );
		}

		public void SendLocalizedMessageTo( Mobile to, int number, int hue = 0x3B2 )
		{
			if ( Deleted || !to.CanSee( this ) )
				return;

			to.Send( new MessageLocalized( Serial, ItemID, MessageType.Regular, hue, 3, number, "", "" ) );
		}

		public void SendLocalizedMessageTo( Mobile to, int number, string args )
		{
			if ( Deleted || !to.CanSee( this ) )
				return;

			to.Send( new MessageLocalized( Serial, ItemID, MessageType.Regular, 0x3B2, 3, number, "", args ) );
		}

		public void SendLocalizedMessageTo( Mobile to, int number, AffixType affixType, string affix, string args )
		{
			if ( Deleted || !to.CanSee( this ) )
				return;

			to.Send( new MessageLocalizedAffix( Serial, ItemID, MessageType.Regular, 0x3B2, 3, number, "", affixType, affix, args ) );
		}

		public void SendAsciiMessageTo( Mobile to, string text, int hue = 0x3B2 )
		{
			if ( Deleted || !to.CanSee( this ) )
				return;

			to.Send( new AsciiMessage( Serial, -1, MessageType.Regular, hue, 3, "", text ) );
		}

		public virtual bool CheckLOSOnUse => Parent == null;

		public virtual void OnDoubleClick( Mobile from )
		{
		}

		public virtual void OnDoubleClickOutOfRange( Mobile from )
		{
		}

		public virtual void OnDoubleClickCantSee( Mobile from )
		{
			from.LocalOverheadMessage( MessageType.Regular, 0x3B2, 1019045 ); // I can't reach that.
		}

		public virtual void OnDoubleClickDead( Mobile from )
		{
			from.LocalOverheadMessage( MessageType.Regular, 0x3B2, 1019048 ); // I am dead and cannot do that.
		}

		public virtual void OnDoubleClickNotAccessible( Mobile from )
		{
			from.SendLocalizedMessage( 500447 ); // That is not accessible.
		}

		public virtual void OnDoubleClickSecureTrade( Mobile from )
		{
			from.SendLocalizedMessage( 500447 ); // That is not accessible.
		}

		public virtual void OnSnoop( Mobile from )
		{
		}

		public bool InSecureTrade => ( GetSecureTradeCont() != null );

		public SecureTradeContainer GetSecureTradeCont()
		{
			object p = this;

			while ( p is Item )
			{
				if ( p is SecureTradeContainer )
					return (SecureTradeContainer) p;

				p = ( (Item) p ).m_Parent;
			}

			return null;
		}

		public virtual void OnItemAdded( Item item )
		{
			if ( m_Parent is Item )
				( (Item) m_Parent ).OnSubItemAdded( item );
			else if ( m_Parent is Mobile )
				( (Mobile) m_Parent ).OnSubItemAdded( item );
		}

		public virtual void OnItemRemoved( Item item )
		{
			if ( m_Parent is Item )
				( (Item) m_Parent ).OnSubItemRemoved( item );
			else if ( m_Parent is Mobile )
				( (Mobile) m_Parent ).OnSubItemRemoved( item );
		}

		public virtual void OnSubItemAdded( Item item )
		{
			if ( m_Parent is Item )
				( (Item) m_Parent ).OnSubItemAdded( item );
			else if ( m_Parent is Mobile )
				( (Mobile) m_Parent ).OnSubItemAdded( item );
		}

		public virtual void OnSubItemRemoved( Item item )
		{
			if ( m_Parent is Item )
				( (Item) m_Parent ).OnSubItemRemoved( item );
			else if ( m_Parent is Mobile )
				( (Mobile) m_Parent ).OnSubItemRemoved( item );
		}

		public virtual void OnItemBounceCleared( Item item )
		{
			if ( m_Parent is Item )
				( (Item) m_Parent ).OnSubItemBounceCleared( item );
			else if ( m_Parent is Mobile )
				( (Mobile) m_Parent ).OnSubItemBounceCleared( item );
		}

		public virtual void OnSubItemBounceCleared( Item item )
		{
			if ( m_Parent is Item )
				( (Item) m_Parent ).OnSubItemBounceCleared( item );
			else if ( m_Parent is Mobile )
				( (Mobile) m_Parent ).OnSubItemBounceCleared( item );
		}

		public virtual bool CheckTarget( Mobile from, Server.Targeting.Target targ, object targeted )
		{
			if ( m_Parent is Item )
				return ( (Item) m_Parent ).CheckTarget( from, targ, targeted );
			else if ( m_Parent is Mobile )
				return ( (Mobile) m_Parent ).CheckTarget( from, targ, targeted );

			return true;
		}

		public virtual bool IsAccessibleTo( Mobile check )
		{
			if ( m_Parent is Item )
				return ( (Item) m_Parent ).IsAccessibleTo( check );

			Region reg = Region.Find( GetWorldLocation(), m_Map );

			return reg.CheckAccessibility( this, check );
		}

		public bool IsChildOf( object o )
		{
			return IsChildOf( o, false );
		}

		public bool IsChildOf( object o, bool allowNull )
		{
			object p = m_Parent;

			if ( ( p == null || o == null ) && !allowNull )
				return false;

			if ( p == o )
				return true;

			while ( p is Item )
			{
				Item item = (Item) p;

				if ( item.m_Parent == null )
				{
					break;
				}
				else
				{
					p = item.m_Parent;

					if ( p == o )
						return true;
				}
			}

			return false;
		}

		public ItemData ItemData => TileData.ItemTable[m_ItemID & TileData.MaxItemValue];

		public virtual void OnItemUsed( Mobile from, Item item )
		{
			if ( m_Parent is Item )
				( (Item) m_Parent ).OnItemUsed( from, item );
			else if ( m_Parent is Mobile )
				( (Mobile) m_Parent ).OnItemUsed( from, item );
		}

		public virtual bool CheckItemUse( Mobile from, Item item )
		{
			if ( m_Parent is Item )
				return ( (Item) m_Parent ).CheckItemUse( from, item );
			else if ( m_Parent is Mobile )
				return ( (Mobile) m_Parent ).CheckItemUse( from, item );
			else
				return true;
		}

		public virtual void OnItemLifted( Mobile from, Item item )
		{
			if ( m_Parent is Item )
				( (Item) m_Parent ).OnItemLifted( from, item );
			else if ( m_Parent is Mobile )
				( (Mobile) m_Parent ).OnItemLifted( from, item );
		}

		public virtual bool CheckLift( Mobile from, Item item, ref LRReason reject )
		{
			if ( m_Parent is Item )
				return ( (Item) m_Parent ).CheckLift( from, item, ref reject );
			else if ( m_Parent is Mobile )
				return ( (Mobile) m_Parent ).CheckLift( from, item, ref reject );
			else
				return true;
		}

		public virtual bool CanInsure => true;

		public virtual bool CanTarget => true;

		public virtual bool DisplayLootType => true;

		public virtual void OnAosSingleClick( Mobile from )
		{
			ObjectPropertyListPacket opl = this.PropertyList;

			if ( opl.Header > 0 )
				from.Send( new MessageLocalized( Serial, m_ItemID, MessageType.Label, 0x3B2, 3, opl.Header, m_Name, opl.HeaderArgs ) );
		}

		public static bool ScissorCopyLootType { get; set; }

		public virtual void ScissorHelper( Mobile from, Item newItem, int amountPerOldItem )
		{
			ScissorHelper( from, newItem, amountPerOldItem, true );
		}

		public virtual void ScissorHelper( Mobile from, Item newItem, int amountPerOldItem, bool carryHue )
		{
			int amount = Amount;

			if ( amount > ( 60000 / amountPerOldItem ) ) // let's not go over 60000
				amount = ( 60000 / amountPerOldItem );

			Amount -= amount;

			int ourHue = Hue;
			Map thisMap = this.Map;
			Point3D worldLoc = this.GetWorldLocation();
			LootType type = this.LootType;

			if ( Amount == 0 )
				Delete();

			newItem.Amount = amount * amountPerOldItem;

			if ( carryHue )
				newItem.Hue = ourHue;

			if ( ScissorCopyLootType )
				newItem.LootType = type;

			if ( from.Backpack == null || !from.Backpack.TryDropItem( from, newItem, false ) )
				newItem.MoveToWorld( worldLoc, thisMap );
		}

		public virtual void Consume()
		{
			Consume( 1 );
		}

		public virtual void Consume( int amount )
		{
			this.Amount -= amount;

			if ( this.Amount <= 0 )
				this.Delete();
		}

		private Mobile m_BlessedFor;

		[CommandProperty( AccessLevel.Administrator )]
		public bool Insured
		{
			get { return GetFlag( ImplFlag.Insured ); }
			set { SetFlag( ImplFlag.Insured, value ); InvalidateProperties(); }
		}

		public bool PayedInsurance
		{
			get { return GetFlag( ImplFlag.PayedInsurance ); }
			set { SetFlag( ImplFlag.PayedInsurance, value ); }
		}

		public Mobile BlessedFor
		{
			get { return m_BlessedFor; }
			set { m_BlessedFor = value; InvalidateProperties(); }
		}

		public virtual bool CheckBlessed( object obj )
		{
			return CheckBlessed( obj as Mobile );
		}

		public virtual bool CheckBlessed( Mobile m )
		{
			if ( m_LootType == LootType.Blessed || ( Mobile.InsuranceEnabled && Insured ) )
				return true;

			return ( m != null && m == m_BlessedFor );
		}

		public virtual bool CheckNewbied()
		{
			return ( m_LootType == LootType.Newbied );
		}

		public virtual bool IsStandardLoot()
		{
			if ( Mobile.InsuranceEnabled && Insured )
				return false;

			if ( m_BlessedFor != null )
				return false;

			return ( m_LootType == LootType.Regular );
		}

		public override string ToString()
		{
			return String.Format( "0x{0:X} \"{1}\"", Serial.Value, GetType().Name );
		}

		internal int m_TypeRef;

		[Constructable]
		public Item( int itemID )
			: this()
		{
			m_ItemID = itemID;
		}

		public Item()
			: this( SerialGenerator.GetNewItemSerial() )
		{
			DefaultItemInit();

			World.AddItem( this );
		}

		/// <summary>
		/// Serialization constructor. Used when deserializing an existing item.
		/// </summary>
		public Item( Serial serial )
		{
			Serial = serial;

			RegisterType();
		}

		/// <summary>
		/// Initialize some fields when creating a fresh new Item. Otherwise these should be initialized by <see cref="Deserialize"/> method.
		/// </summary>
		private void DefaultItemInit()
		{
			Visible = true;
			Movable = true;
			Amount = 1;
			m_Map = Map.Internal;

			SetLastMoved();
		}

		private void RegisterType()
		{
			Type ourType = this.GetType();
			m_TypeRef = World.m_ItemTypes.IndexOf( ourType );

			if ( m_TypeRef == -1 )
			{
				World.m_ItemTypes.Add( ourType );
				m_TypeRef = World.m_ItemTypes.Count - 1;
			}
		}

		public virtual void OnSectorActivate()
		{
		}

		public virtual void OnSectorDeactivate()
		{
		}
	}
}
