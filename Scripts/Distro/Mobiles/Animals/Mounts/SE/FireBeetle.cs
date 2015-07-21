using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName( "a fire beetle corpse" )]
	public class FireBeetle : BaseMount
	{
		[Constructable]
		public FireBeetle()
			: this( "a fire beetle" )
		{
		}

		public override bool SubdueBeforeTame { get { return true; } }

		[Constructable]
		public FireBeetle( string name )
			: base( name, 0xA9, 0x3E95, AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Hue = 0x489;

			SetStr( 300 );
			SetDex( 100 );
			SetInt( 500 );

			SetHits( 200 );

			SetDamage( 7, 20 );

			SetDamageType( ResistanceType.Fire, 100 );

			SetResistance( ResistanceType.Physical, 40 );
			SetResistance( ResistanceType.Fire, 70, 75 );
			SetResistance( ResistanceType.Cold, 10 );
			SetResistance( ResistanceType.Poison, 30 );
			SetResistance( ResistanceType.Energy, 30 );

			SetSkill( SkillName.MagicResist, 90.0 );
			SetSkill( SkillName.Tactics, 100.0 );
			SetSkill( SkillName.Wrestling, 100.0 );

			Fame = 4000;
			Karma = -4000;

			Tamable = true;
			ControlSlots = 3;
			MinTameSkill = 93.9;

			PackItem( new IronIngot( 2 ) );
			PackItem( new SulfurousAsh( 15 ) );
		}

		public override int GetAngerSound()
		{
			return 0x21D;
		}

		public override int GetIdleSound()
		{
			return 0x21D;
		}

		public override int GetAttackSound()
		{
			return 0x162;
		}

		public override int GetHurtSound()
		{
			return 0x163;
		}

		public override int GetDeathSound()
		{
			return 0x21D;
		}

		public override double GetControlChance( Mobile m )
		{
			return 1.0;
		}

		public override FoodType FavoriteFood { get { return FoodType.Meat; } }

		public FireBeetle( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			if ( version < 1 )
			{
				Hue = 0x489;
				Body = 0xA9;
				ItemID = 0x3E95;
			}
		}
	}
}