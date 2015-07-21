using System;
using Server;
using Server.Items;
using Server.Engines.Loyalty;

namespace Server.Mobiles
{
	[CorpseName( "a gray goblin mage corpse" )]
	public class EnslavedGoblinMage : BaseCreature
	{
		[Constructable]
		public EnslavedGoblinMage()
			: base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "enslaved goblin mage";
			Body = 723;

			Hue = 2301;

			SetStr( 241, 269 );
			SetDex( 74, 88 );
			SetInt( 466, 533 );

			SetHits( 135, 169 );

			SetDamage( 5, 7 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 20, 25 );
			SetResistance( ResistanceType.Fire, 35, 45 );
			SetResistance( ResistanceType.Cold, 30, 35 );
			SetResistance( ResistanceType.Poison, 35, 40 );
			SetResistance( ResistanceType.Energy, 10, 20 );

			SetSkill( SkillName.Anatomy, 82.2, 89.2 );
			SetSkill( SkillName.MagicResist, 142.3, 148.7 );
			SetSkill( SkillName.Tactics, 81.1, 85.9 );
			SetSkill( SkillName.Wrestling, 99.3, 106.1 );
			SetSkill( SkillName.EvalInt, 100.8, 102.2 );
			SetSkill( SkillName.Magery, 101.5, 109 );
			SetSkill( SkillName.Meditation, 94.4, 96.1 );

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
		public override int LoyaltyPointsAward { get { return 8; } }

		public override bool CanRummageCorpses { get { return true; } }
		public override int Meat { get { return 1; } }
		public override FoodType FavoriteFood { get { return FoodType.Meat; } }

		public EnslavedGoblinMage( Serial serial )
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
