using System;
using Server;
using Server.Items;
using Server.Engines.Loyalty;

namespace Server.Mobiles
{
	[CorpseName( "a green goblin scout corpse" )]
	public class GreenGoblinScout : BaseCreature
	{
		[Constructable]
		public GreenGoblinScout()
			: base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a green goblin scout";
			Body = 723;

			SetStr( 263, 309 );
			SetDex( 76, 78 );
			SetInt( 110, 140 );

			SetHits( 183, 207 );

			SetDamage( 5, 7 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 43, 47 );
			SetResistance( ResistanceType.Fire, 30, 40 );
			SetResistance( ResistanceType.Cold, 28, 34 );
			SetResistance( ResistanceType.Poison, 11, 18 );
			SetResistance( ResistanceType.Energy, 11, 19 );

			SetSkill( SkillName.Anatomy, 80.1, 85.1 );
			SetSkill( SkillName.MagicResist, 90.1, 100.0 );
			SetSkill( SkillName.Tactics, 85.1, 90.0 );
			SetSkill( SkillName.Wrestling, 105.1, 110.0 );

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

		public GreenGoblinScout( Serial serial )
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
