using System;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a unicorn corpse" )]
	public class Unicorn : BaseMount
	{
		public override bool AllowMaleRider { get { return false; } }
		public override bool AllowMaleTamer { get { return false; } }

		public override bool InitialInnocent { get { return true; } }

		public override void OnDisallowedRider( Mobile m )
		{
			m.SendLocalizedMessage( 1042318 ); // The unicorn refuses to allow you to ride it.
		}

		public DateTime m_NextAbilityTime;

		[Constructable]
		public Unicorn()
			: this( "a unicorn" )
		{
		}

		[Constructable]
		public Unicorn( string name )
			: base( name, 0x7A, 0x3EB4, AIType.AI_Mage, FightMode.Evil, 10, 1, 0.2, 0.4 )
		{
			BaseSoundID = 0x4BC;

			SetStr( 296, 325 );
			SetDex( 96, 115 );
			SetInt( 186, 225 );

			SetHits( 191, 210 );

			SetDamage( 16, 22 );

			SetDamageType( ResistanceType.Physical, 75 );
			SetDamageType( ResistanceType.Energy, 25 );

			SetResistance( ResistanceType.Physical, 55, 65 );
			SetResistance( ResistanceType.Fire, 25, 40 );
			SetResistance( ResistanceType.Cold, 25, 40 );
			SetResistance( ResistanceType.Poison, 55, 65 );
			SetResistance( ResistanceType.Energy, 25, 40 );

			SetSkill( SkillName.EvalInt, 80.1, 90.0 );
			SetSkill( SkillName.Magery, 60.2, 80.0 );
			SetSkill( SkillName.Meditation, 50.1, 60.0 );
			SetSkill( SkillName.MagicResist, 75.3, 90.0 );
			SetSkill( SkillName.Tactics, 20.1, 22.5 );
			SetSkill( SkillName.Wrestling, 80.5, 92.5 );

			Fame = 9000;
			Karma = 9000;

			Tamable = true;
			ControlSlots = 2;
			MinTameSkill = 95.1;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich );
			AddLoot( LootPack.LowScrolls );
			AddLoot( LootPack.Potions );
		}

		protected override void OnAfterDeath( Container c )
		{
			if ( 0.3 > Utility.RandomDouble() )
				c.DropItem( new UnicornRibs() );

			base.OnAfterDeath( c );
		}

		public override Poison PoisonImmune { get { return Poison.Lethal; } }
		public override int Meat { get { return 3; } }
		public override int Hides { get { return 10; } }
		public override HideType HideType { get { return HideType.Horned; } }
		public override FoodType FavoriteFood { get { return FoodType.FruitsAndVegies | FoodType.GrainsAndHay; } }

		public Unicorn( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version

			writer.Write( (DateTime) m_NextAbilityTime );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 1:
					{
						m_NextAbilityTime = reader.ReadDateTime();
						goto case 0;
					}
				case 0:
					{
						break;
					}
			}
		}
	}
}
