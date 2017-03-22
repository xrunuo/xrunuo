using System;
using Server;
using Server.Items;
using Server.Engines.Loyalty;

namespace Server.Mobiles
{
	[CorpseName( "a gray goblin keeper corpse" )]
	public class GrayGoblinKeeper : BaseCreature
	{
		[Constructable]
		public GrayGoblinKeeper()
			: base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a gray goblin keeper";
			Body = 723;

			Hue = 2301;

			SetStr( 323, 339 );
			SetDex( 68, 76 );
			SetInt( 122, 146 );

			SetHits( 171, 174 );

			SetDamage( 5, 7 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 40, 45 );
			SetResistance( ResistanceType.Fire, 30, 35 );
			SetResistance( ResistanceType.Cold, 25, 35 );
			SetResistance( ResistanceType.Poison, 10 );
			SetResistance( ResistanceType.Energy, 10, 15 );

			SetSkill( SkillName.Anatomy, 80.1, 90.0 );
			SetSkill( SkillName.MagicResist, 120.1, 125.0 );
			SetSkill( SkillName.Tactics, 85.1, 90.0 );
			SetSkill( SkillName.Wrestling, 100.1, 105.0 );

			Fame = 2500; // TODO: Verify
			Karma = -2500;

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
			AddLoot( LootPack.Gems, 1 );
		}

		public override LoyaltyGroup LoyaltyGroupEnemy { get { return LoyaltyGroup.GargoyleQueen; } }
		public override int LoyaltyPointsAward { get { return 10; } }
		public override SlayerName SlayerGroup { get { return SlayerName.Repond; } }
		public override bool CanRummageCorpses { get { return true; } }
		public override int Meat { get { return 1; } }
		public override FoodType FavoriteFood { get { return FoodType.Meat; } }

		public GrayGoblinKeeper( Serial serial )
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
