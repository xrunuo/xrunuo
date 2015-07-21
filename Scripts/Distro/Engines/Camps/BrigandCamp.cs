using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Multis
{
	public class BrigandCamp : BaseCamp
	{
		private Mobile m_Prisoner;

		[Constructable]
		public BrigandCamp()
			: base( -1 )
		{
		}

		public BrigandCamp( Serial serial )
			: base( serial )
		{
		}

		public override void AddComponents()
		{
			Item item = new Item( 0xFAC );
			item.Movable = false;
			AddItem( item, 0, 0, 0 ); // fire pit
			item = new Item( 0xDE3 );
			item.Movable = false;
			AddItem( item, 0, 0, 0 ); // camp fire
			item = new Item( 0x974 );
			item.Movable = false;
			AddItem( item, 0, 0, 1 ); // cauldron

			for ( int i = 0; i < 2; i++ )
			{
				LockableContainer cont = null;

				switch ( Utility.Random( 3 ) )
				{
					case 0:
						cont = new MetalChest();
						break;
					case 1:
						cont = new WoodenChest();
						break;
					case 2:
						cont = new SmallCrate();
						break;
				}

				cont.Movable = false;
				cont.Locked = true;

				cont.TrapType = TrapType.ExplosionTrap;
				cont.TrapPower = Utility.RandomMinMax( 30, 40 );
				cont.TrapLevel = 2;
				cont.TrapEnabled = true;

				cont.RequiredSkill = 76;
				cont.LockLevel = 66;
				cont.MaxLockLevel = 116;

				cont.DropItem( new Gold( Utility.RandomMinMax( 100, 400 ) ) );
				cont.DropItem( new Arrow( 10 ) );
				cont.DropItem( new Bolt( 10 ) );

				if ( Utility.RandomDouble() < 0.8 )
				{
					switch ( Utility.Random( 4 ) )
					{
						case 0:
							cont.DropItem( new LesserCurePotion() );
							break;
						case 1:
							cont.DropItem( new LesserExplosionPotion() );
							break;
						case 2:
							cont.DropItem( new LesserHealPotion() );
							break;
						case 3:
							cont.DropItem( new LesserPoisonPotion() );
							break;
					}
				}

				if ( Utility.RandomDouble() < 0.5 )
				{
					item = Loot.RandomArmorOrShieldOrWeapon();

					if ( item is BaseWeapon )
						BaseRunicTool.ApplyAttributesTo( (BaseWeapon) item, false, 0, Utility.RandomMinMax( 1, 5 ), 10, 100 );
					else if ( item is BaseArmor )
						BaseRunicTool.ApplyAttributesTo( (BaseArmor) item, false, 0, Utility.RandomMinMax( 1, 5 ), 10, 100 );

					cont.DropItem( item );
				}

				AddItem( cont, Utility.RandomMinMax( -5, 5 ), Utility.RandomMinMax( -5, 5 ), 0 );
			}

			m_Prisoner = new BaseEscort();

			for ( int i = 0; i < 4; i++ )
			{
				Point3D loc = GetRandomSpawnPoint( 5 );

				AddMobile( new Brigand(), 6, 0, 0, 0 );
			}

			AddMobile( m_Prisoner, 2, 2, 3, 0 );
		}

		public virtual Point3D GetRandomSpawnPoint( int range )
		{
			Map map = Map;

			if ( map == null )
				return Location;

			// Try 10 times to find a Spawnable location.
			for ( int i = 0; i < 10; i++ )
			{
				int x = Location.X + ( Utility.Random( ( range * 2 ) + 1 ) - range );
				int y = Location.Y + ( Utility.Random( ( range * 2 ) + 1 ) - range );
				int z = Map.GetAverageZ( x, y );

				if ( Map.CanSpawnMobile( new Point2D( x, y ), this.Z ) )
					return new Point3D( x, y, this.Z );
				else if ( Map.CanSpawnMobile( new Point2D( x, y ), z ) )
					return new Point3D( x, y, z );
			}

			return this.Location;
		}

		public override void OnEnter( Mobile m )
		{
			base.OnEnter( m );

			if ( m.IsPlayer && m_Prisoner != null )
			{
				m_Prisoner.Yell( Utility.RandomMinMax( 502261, 502268 ) );
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.Write( m_Prisoner );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			m_Prisoner = reader.ReadMobile();
		}
	}
}