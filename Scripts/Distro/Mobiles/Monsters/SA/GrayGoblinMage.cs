using System;
using Server;
using Server.Items;
using Server.Engines.Loyalty;

namespace Server.Mobiles
{
	[CorpseName( "a gray goblin mage corpse" )]
	public class GrayGoblinMage : BaseCreature
	{
		[Constructable]
		public GrayGoblinMage()
			: base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.25, 0.5 )
		{
			Name = "a gray goblin mage";
			Body = 723;

			Hue = 2301;

			SetStr( 212, 314 );
			SetDex( 70, 96 );
			SetInt( 457, 548 );

			SetHits( 131, 180 );

			SetDamage( 5, 7 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 20, 30 );
			SetResistance( ResistanceType.Fire, 35, 45 );
			SetResistance( ResistanceType.Cold, 30, 40 );
			SetResistance( ResistanceType.Poison, 35, 45 );
			SetResistance( ResistanceType.Energy, 10, 20 );

			SetSkill( SkillName.Anatomy, 80.1, 90.0 );
			SetSkill( SkillName.MagicResist, 140.1, 150.0 );
			SetSkill( SkillName.Tactics, 80.1, 90.0 );
			SetSkill( SkillName.Wrestling, 90.1, 110.0 );
			SetSkill( SkillName.EvalInt, 90.1, 110.0 );
			SetSkill( SkillName.Magery, 100.1, 120.0 );
			SetSkill( SkillName.Meditation, 90.1, 100.0 );

			Fame = 15000;
			Karma = -15000;

			PackReg( 8 );

			if ( 0.25 > Utility.RandomDouble() )
				PackItem( new GoblinBlood() );
		}

		public override int GetAngerSound() { return 0x600; }
		public override int GetAttackSound() { return 0x5FD; }
		public override int GetHurtSound() { return 0x5FF; }
		public override int GetDeathSound() { return 0x5FE; }

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich );
			AddLoot( LootPack.MedScrolls );
			AddLoot( LootPack.Gems, 1 );
		}

		public override LoyaltyGroup LoyaltyGroupEnemy { get { return LoyaltyGroup.GargoyleQueen; } }
		public override int LoyaltyPointsAward { get { return 10; } }

		public override bool CanRummageCorpses { get { return true; } }
		public override int Meat { get { return 1; } }
		public override FoodType FavoriteFood { get { return FoodType.Meat; } }

		public GrayGoblinMage( Serial serial )
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
