using System;
using Server;
using Server.Items;
using Server.Engines.Loyalty;

namespace Server.Mobiles
{
	[CorpseName( "a wolf spider corpse" )]
	public class WolfSpider : BaseCreature
	{
		[Constructable]
		public WolfSpider()
			: base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a wolf spider";
			Body = 736;

			SetStr( 227, 270 );
			SetDex( 145, 165 );
			SetInt( 285, 307 );

			SetHits( 156, 200 );

			SetDamage( 15, 18 );

			SetDamageType( ResistanceType.Physical, 70 );
			SetDamageType( ResistanceType.Poison, 30 );

			SetResistance( ResistanceType.Physical, 30, 35 );
			SetResistance( ResistanceType.Fire, 20, 30 );
			SetResistance( ResistanceType.Cold, 25, 35 );
			SetResistance( ResistanceType.Poison, 100 );
			SetResistance( ResistanceType.Energy, 25, 35 );

			SetSkill( SkillName.Anatomy, 80.1, 90 );
			SetSkill( SkillName.Poisoning, 60.1, 75.0 );
			SetSkill( SkillName.MagicResist, 60.1, 75.0 );
			SetSkill( SkillName.Tactics, 80.0, 100.0 );
			SetSkill( SkillName.Wrestling, 80.1, 90.0 );

			Fame = 5000;
			Karma = -5000;

			Tamable = true;
			MinTameSkill = 59.1;
			ControlSlots = 2;

			PackItem( new SulfurousAsh( 8 ) );

			if ( 0.15 > Utility.RandomDouble() )
				PackItem( new BottleOfIchor() );
		}

		public override int GetAngerSound() { return 0x604; }
		public override int GetAttackSound() { return 0x601; }
		public override int GetHurtSound() { return 0x603; }
		public override int GetDeathSound() { return 0x602; }

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich );
		}

		public override LoyaltyGroup LoyaltyGroupEnemy { get { return LoyaltyGroup.GargoyleQueen; } }
		public override int LoyaltyPointsAward { get { return 5; } }

		public override FoodType FavoriteFood { get { return FoodType.Meat; } }
		public override PackInstinct PackInstinct { get { return PackInstinct.Arachnid; } }
		public override Poison PoisonImmune { get { return Poison.Deadly; } }
		public override Poison HitPoison { get { return ( 0.8 >= Utility.RandomDouble() ? Poison.Greater : Poison.Deadly ); } }

		public WolfSpider( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}
}
