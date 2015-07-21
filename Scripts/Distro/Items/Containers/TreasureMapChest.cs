using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Server;
using Server.ContextMenus;
using Server.Engines.PartySystem;
using Server.Gumps;
using Server.Misc;
using Server.Multis;
using Server.Mobiles;
using Server.Network;

namespace Server.Items
{
	public class TreasureMapChest : LockableContainer
	{
		public override int LabelNumber { get { return 3000541; } }

		public static Type[] m_Artifacts = new Type[]
		{
			typeof( CandelabraOfSouls ), typeof( GoldBricks ), typeof( PhillipsWoodenSteed ),
			typeof( ArcticDeathDealer ), typeof( BlazeOfDeath ), typeof( BurglarsBandana ),
			typeof( CavortingClub ), typeof( DreadPirateHat ),
			typeof( EnchantedTitanLegBone ), typeof( GwennosHarp ), typeof( IolosLute ),
			typeof( LunaLance ), typeof( NightsKiss ), typeof( NoxRangersHeavyCrossbow ),
			typeof( PolarBearMask ), typeof( VioletCourage ), typeof( HeartOfTheLion ),
			typeof( ColdBlood ), typeof( AlchemistsBauble ),
			typeof( ShipModelOfTheHMSCape ), typeof( AdmiralsHeartyRum )
		};

		private int m_Level;
		private DateTime m_DeleteTime;
		private Timer m_Timer;
		private Mobile m_Owner;
		private bool m_Temporary;

		private List<Mobile> m_Guardians;

		[CommandProperty( AccessLevel.GameMaster )]
		public int Level { get { return m_Level; } set { m_Level = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Owner { get { return m_Owner; } set { m_Owner = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public DateTime DeleteTime { get { return m_DeleteTime; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Temporary { get { return m_Temporary; } set { m_Temporary = value; } }

		public List<Mobile> Guardians { get { return m_Guardians; } }

		[Constructable]
		public TreasureMapChest( int level, Map map )
			: this( null, level, false, map )
		{
		}

		public TreasureMapChest( Mobile owner, int level, bool temporary, Map map )
			: base( 0xE40 )
		{
			m_Owner = owner;
			m_Level = level;
			m_DeleteTime = DateTime.Now + TimeSpan.FromHours( 3.0 );

			m_Temporary = temporary;
			m_Guardians = new List<Mobile>();

			m_Timer = new DeleteTimer( this, m_DeleteTime );
			m_Timer.Start();

			Fill( this, level, map );
		}

		private static void GetRandomAOSStats( out int attributeCount, out int min, out int max )
		{
			int rnd = Utility.Random( 15 );

			if ( rnd < 1 )
			{
				attributeCount = Utility.RandomMinMax( 2, 6 );
				min = 20;
				max = 70;
			}
			else if ( rnd < 3 )
			{
				attributeCount = Utility.RandomMinMax( 2, 4 );
				min = 20;
				max = 50;
			}
			else if ( rnd < 6 )
			{
				attributeCount = Utility.RandomMinMax( 2, 3 );
				min = 20;
				max = 40;
			}
			else if ( rnd < 10 )
			{
				attributeCount = Utility.RandomMinMax( 1, 2 );
				min = 10;
				max = 30;
			}
			else
			{
				attributeCount = 1;
				min = 10;
				max = 20;
			}
		}

		public static void Fill( LockableContainer cont, int level, Map map )
		{
			cont.Movable = false;
			cont.Locked = true;

			#region Lock & Trap
			cont.TrapType = TrapType.ExplosionTrap;
			cont.TrapPower = level * 25;
			cont.TrapLevel = level;
			cont.TrapEnabled = true;

			switch ( level )
			{
				case 1:
					cont.RequiredSkill = 36;
					break;
				case 2:
					cont.RequiredSkill = 76;
					break;
				case 3:
					cont.RequiredSkill = 84;
					break;
				case 4:
					cont.RequiredSkill = 92;
					break;
				case 5:
					cont.RequiredSkill = 100;
					break;
				case 6:
					cont.RequiredSkill = 100;
					break;
			}

			cont.LockLevel = cont.RequiredSkill - 10;
			cont.MaxLockLevel = cont.RequiredSkill + 40;
			#endregion

			#region Gold
			cont.DropItem( new Gold( level * 5000 ) );
			#endregion

			#region Reagents
			int reagentStackCount = level + 3;

			for ( int i = 0; i < reagentStackCount; i++ )
			{
				Item item = Loot.RandomPossibleReagent();
				item.Amount = Utility.RandomMinMax( 40, 60 );
				cont.DropItem( item );
			}
			#endregion

			#region Magic Items
			int magicItemCount = 24 + ( 8 * level );

			for ( int i = 0; i < magicItemCount; ++i )
			{
				Item item = Loot.RandomArmorOrShieldOrWeaponOrJewelry();

				if ( item is BaseWeapon )
				{
					BaseWeapon weapon = (BaseWeapon) item;

					int attributeCount;
					int min, max;

					GetRandomAOSStats( out attributeCount, out min, out max );

					BaseRunicTool.ApplyAttributesTo( weapon, attributeCount, min, max );

					cont.DropItem( item );
				}
				else if ( item is BaseArmor )
				{
					BaseArmor armor = (BaseArmor) item;

					int attributeCount;
					int min, max;

					GetRandomAOSStats( out attributeCount, out min, out max );

					BaseRunicTool.ApplyAttributesTo( armor, attributeCount, min, max );

					cont.DropItem( item );
				}
				else if ( item is BaseJewel )
				{
					int attributeCount;
					int min, max;

					GetRandomAOSStats( out attributeCount, out min, out max );

					BaseRunicTool.ApplyAttributesTo( (BaseJewel) item, attributeCount, min, max );

					cont.DropItem( item );
				}
			}
			#endregion

			#region Gems
			int gemCount = level * 3;

			for ( int i = 0; i < gemCount; i++ )
			{
				Item item = Loot.RandomGem();
				cont.DropItem( item );
			}
			#endregion

			#region Essences
			if ( level >= 2 )
			{
				int essenceCount = level;

				for ( int i = 0; i < essenceCount; i++ )
				{
					Item item = Loot.RandomEssence();
					cont.DropItem( item );
				}
			}
			#endregion

			#region Special loot
			if ( map == Map.Felucca && ( level * 0.15 ) > Utility.RandomDouble() )
			{
				Item item = ScrollOfTranscendence.CreateRandom( 5 );
				cont.DropItem( item );
			}

			if ( ( level * 0.1 ) > Utility.RandomDouble() )
				cont.DropItem( new TastyTreat() );

			if ( ( level * 0.05 ) > Utility.RandomDouble() )
				cont.DropItem( new CreepingVine() );

			if ( ( level * 0.05 ) > Utility.RandomDouble() )
				cont.DropItem( Server.Engines.Quests.BaseReward.RandomRecipe() );

			if ( 0.5 > Utility.RandomDouble() )
				cont.DropItem( new TreasureMap( level < 6 && 0.25 > Utility.RandomDouble() ? level + 1 : level ) );

			if ( 0.25 > Utility.RandomDouble() )
				cont.DropItem( new SkeletonKey() );

			if ( 0.2 > Utility.RandomDouble() )
				cont.DropItem( ScrollOfAlacrity.CreateRandom() );

			if ( 0.2 > Utility.RandomDouble() )
				cont.DropItem( new MessageInABottle() );

			if ( level >= 5 )
			{
				if ( 0.1 > Utility.RandomDouble() )
					cont.DropItem( new ForgedPardon() );

				if ( 0.09 > Utility.RandomDouble() )
					cont.DropItem( new ManaPhasingOrb() );

				if ( 0.09 > Utility.RandomDouble() )
					cont.DropItem( new RunedSashOfWarding() );

				if ( 0.09 > Utility.RandomDouble() )
					cont.DropItem( map == Map.TerMur ? new GargishSurgeShield() : new SurgeShield() );
			}
			#endregion

			#region Artifacts
			if ( level >= 6 )
			{
				Item item = (Item) Activator.CreateInstance( m_Artifacts[Utility.Random( m_Artifacts.Length )] );
				cont.DropItem( item );
			}
			#endregion
		}

		public static void FillOld( LockableContainer cont, int level )
		{
			cont.Movable = false;
			cont.Locked = true;

			cont.TrapType = TrapType.ExplosionTrap;
			cont.TrapPower = level * 25;
			cont.TrapLevel = level;
			cont.TrapEnabled = true;

			switch ( level )
			{
				case 1:
					cont.RequiredSkill = 36;
					break;
				case 2:
					cont.RequiredSkill = 76;
					break;
				case 3:
					cont.RequiredSkill = 84;
					break;
				case 4:
					cont.RequiredSkill = 92;
					break;
				case 5:
					cont.RequiredSkill = 100;
					break;
				case 6:
					cont.RequiredSkill = 100;
					break;
			}

			cont.LockLevel = cont.RequiredSkill - 10;
			cont.MaxLockLevel = cont.RequiredSkill + 40;

			cont.DropItem( new Gold( level * 1000 ) );

			for ( int i = 0; i < level * 5; ++i )
				cont.DropItem( Loot.RandomScroll( 0, 63, SpellbookType.Regular ) );

			for ( int i = 0; i < level * 6; ++i )
			{
				Item item = Loot.RandomArmorOrShieldOrWeaponOrJewelry();

				if ( item is BaseWeapon )
				{
					BaseWeapon weapon = (BaseWeapon) item;

					int attributeCount;
					int min, max;

					GetRandomAOSStats( out attributeCount, out min, out max );

					BaseRunicTool.ApplyAttributesTo( weapon, attributeCount, min, max );

					cont.DropItem( item );
				}
				else if ( item is BaseArmor )
				{
					BaseArmor armor = (BaseArmor) item;

					int attributeCount;
					int min, max;

					GetRandomAOSStats( out attributeCount, out min, out max );

					BaseRunicTool.ApplyAttributesTo( armor, attributeCount, min, max );

					cont.DropItem( item );
				}
				else if ( item is BaseJewel )
				{
					int attributeCount;
					int min, max;

					GetRandomAOSStats( out attributeCount, out min, out max );

					BaseRunicTool.ApplyAttributesTo( (BaseJewel) item, attributeCount, min, max );

					cont.DropItem( item );
				}
			}

			int reagents;
			if ( level == 0 )
				reagents = 12;
			else
				reagents = level * 3;

			for ( int i = 0; i < reagents; i++ )
			{
				Item item = Loot.RandomPossibleReagent();
				item.Amount = Utility.RandomMinMax( 40, 60 );
				cont.DropItem( item );
			}

			int gems;
			if ( level == 0 )
				gems = 2;
			else
				gems = level * 3;

			for ( int i = 0; i < gems; i++ )
			{
				Item item = Loot.RandomGem();
				cont.DropItem( item );
			}

			if ( level == 6 )
			{
				Item item = (Item) Activator.CreateInstance( m_Artifacts[Utility.Random( m_Artifacts.Length )] );
				cont.DropItem( item );
			}
		}

		public override bool CheckAccess( Mobile from )
		{
			foreach ( Mobile m in this.Guardians )
			{
				if ( m.Alive )
				{
					from.SendLocalizedMessage( 1116233 ); // You must defeat the guardians of the chest before you can open it.
					return false;
				}
			}

			return base.CheckAccess( from );
		}

		private List<Item> m_Lifted = new List<Item>();

		private bool CheckLoot( Mobile m, bool criminalAction )
		{
			if ( m_Temporary )
				return false;

			if ( m.AccessLevel >= AccessLevel.GameMaster || m_Owner == null || m == m_Owner )
				return true;

			Party p = Party.Get( m_Owner );

			if ( p != null && p.Contains( m ) )
				return true;

			Map map = this.Map;

			if ( map != null && ( map.Rules & MapRules.HarmfulRestrictions ) == 0 )
			{
				if ( criminalAction )
					m.CriminalAction( true );
				else
					m.SendLocalizedMessage( 1010630 ); // Taking someone else's treasure is a criminal offense!

				return true;
			}

			m.SendLocalizedMessage( 1010631 ); // You did not discover this chest!
			return false;
		}

		public override bool IsDecoContainer
		{
			get { return false; }
		}

		public override bool CheckItemUse( Mobile from, Item item )
		{
			return CheckLoot( from, item != this ) && base.CheckItemUse( from, item );
		}

		public override bool CheckLift( Mobile from, Item item, ref LRReason reject )
		{
			return CheckLoot( from, true ) && base.CheckLift( from, item, ref reject );
		}

		public override void OnItemLifted( Mobile from, Item item )
		{
			bool notYetLifted = !m_Lifted.Contains( item );

			from.RevealingAction();

			if ( notYetLifted )
			{
				m_Lifted.Add( item );

				if ( 0.1 >= Utility.RandomDouble() ) // 10% chance to spawn a new monster
					TreasureMap.Spawn( m_Level, GetWorldLocation(), Map, from );
			}

			base.OnItemLifted( from, item );
		}

		public override bool CheckHold( Mobile m, Item item, bool message, bool checkItems, int plusItems, int plusWeight )
		{
			if ( m.AccessLevel < AccessLevel.GameMaster )
			{
				m.SendLocalizedMessage( 1048122, "", 0x8A5 ); // The chest refuses to be filled with treasure again.
				return false;
			}

			return base.CheckHold( m, item, message, checkItems, plusItems, plusWeight );
		}

		public override int LockPick( Mobile from )
		{
			if ( Picker == null && 0.05 > Utility.RandomDouble() )
			{
				Item item = PickRandomItemFromChest();

				if ( item != null )
				{
					Grubber grubber = new Grubber();
					grubber.PackItem( item );

					grubber.MoveToWorld( TreasureMap.GetRandomSpawnLocation( Location, Map ), Map );

					grubber.PublicOverheadMessage( Network.MessageType.Regular, 0x3B2, false, "*a grubber appears and ganks a piece of your loot*" );
				}
			}

			return base.LockPick( from );
		}

		public override int FailLockPick( Mobile from )
		{
			if ( 0.3 > Utility.RandomDouble() )
			{
				Item destroy = PickRandomItemFromChest();

				if ( destroy != null )
				{
					destroy.Delete();

					DropItem( new DustPile() );
				}

				Effects.PlaySound( Location, Map, 0x1DE );
				PublicOverheadMessage( MessageType.Regular, 0x3B2, false, "The sound of gas escaping is heard from the chest." );
			}

			return base.FailLockPick( from );
		}

		private Item PickRandomItemFromChest()
		{
			var items = Items.Where( i => ( !( i is Gold ) && !( i is DustPile ) ) ).ToArray();
			return items[Utility.Random( items.Length )];
		}

		public TreasureMapChest( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 3 ); // version

			writer.WriteMobileList( m_Guardians, true );
			writer.Write( (bool) m_Temporary );

			writer.Write( m_Owner );

			writer.Write( (int) m_Level );
			writer.WriteDeltaTime( m_DeleteTime );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 3:
				case 2:
					{
						m_Guardians = reader.ReadStrongMobileList();
						m_Temporary = reader.ReadBool();

						goto case 1;
					}
				case 1:
					{
						m_Owner = reader.ReadMobile();

						goto case 0;
					}
				case 0:
					{
						m_Level = reader.ReadInt();
						m_DeleteTime = reader.ReadDeltaTime();

						if ( version < 3 )
							reader.ReadItemList();

						if ( version < 2 )
							m_Guardians = new List<Mobile>();

						break;
					}
			}

			if ( !m_Temporary )
			{
				m_Timer = new DeleteTimer( this, m_DeleteTime );
				m_Timer.Start();
			}
			else
			{
				Delete();
			}
		}

		public override void OnAfterDelete()
		{
			if ( m_Timer != null )
			{
				m_Timer.Stop();
				m_Timer = null;
			}

			Guardians.Each( g => g.Delete() );

			base.OnAfterDelete();
		}

		public override void GetContextMenuEntries( Mobile from, List<ContextMenuEntry> list )
		{
			base.GetContextMenuEntries( from, list );

			if ( from.Alive )
				list.Add( new RemoveEntry( from, this ) );
		}

		public void BeginRemove( Mobile from )
		{
			if ( !from.Alive )
				return;

			from.CloseGump( typeof( RemoveGump ) );
			from.SendGump( new RemoveGump( from, this ) );
		}

		public void EndRemove( Mobile from )
		{
			if ( Deleted || from != m_Owner || !from.InRange( GetWorldLocation(), 3 ) )
				return;

			from.SendLocalizedMessage( 1048124, "", 0x8A5 ); // The old, rusted chest crumbles when you hit it.
			this.Delete();
		}

		private class RemoveGump : Gump
		{
			private Mobile m_From;
			private TreasureMapChest m_Chest;

			public RemoveGump( Mobile from, TreasureMapChest chest )
				: base( 15, 15 )
			{
				m_From = from;
				m_Chest = chest;

				Closable = false;
				Disposable = false;

				AddPage( 0 );

				AddBackground( 30, 0, 240, 240, 2620 );

				AddHtmlLocalized( 45, 15, 200, 80, 1048125, 0xFFFFFF, false, false ); // When this treasure chest is removed, any items still inside of it will be lost.
				AddHtmlLocalized( 45, 95, 200, 60, 1048126, 0xFFFFFF, false, false ); // Are you certain you're ready to remove this chest?

				AddButton( 40, 153, 4005, 4007, 1, GumpButtonType.Reply, 0 );
				AddHtmlLocalized( 75, 155, 180, 40, 1048127, 0xFFFFFF, false, false ); // Remove the Treasure Chest

				AddButton( 40, 195, 4005, 4007, 2, GumpButtonType.Reply, 0 );
				AddHtmlLocalized( 75, 197, 180, 35, 1006045, 0xFFFFFF, false, false ); // Cancel
			}

			public override void OnResponse( GameClient sender, RelayInfo info )
			{
				if ( info.ButtonID == 1 )
					m_Chest.EndRemove( m_From );
			}
		}

		private class RemoveEntry : ContextMenuEntry
		{
			private Mobile m_From;
			private TreasureMapChest m_Chest;

			public RemoveEntry( Mobile from, TreasureMapChest chest )
				: base( 6149, 3 )
			{
				m_From = from;
				m_Chest = chest;

				Enabled = ( from == chest.Owner );
			}

			public override void OnClick()
			{
				if ( m_Chest.Deleted || m_From != m_Chest.Owner || !m_From.CheckAlive() )
					return;

				m_Chest.BeginRemove( m_From );
			}
		}

		private class DeleteTimer : Timer
		{
			private Item m_Item;

			public DeleteTimer( Item item, DateTime time )
				: base( time - DateTime.Now )
			{
				m_Item = item;
			}

			protected override void OnTick()
			{
				m_Item.Delete();
			}
		}
	}
}