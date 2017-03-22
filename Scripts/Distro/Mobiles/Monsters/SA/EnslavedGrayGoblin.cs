using System;
using Server;
using Server.Items;
using Server.Engines.Loyalty;

namespace Server.Mobiles
{
	[CorpseName( "a gray goblin corpse" )]
	public class EnslavedGrayGoblin : BaseCreature
	{
		[Constructable]
		public EnslavedGrayGoblin()
			: base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "enslaved gray goblin";
			Body = 723;

			Hue = 2301;

			SetStr( 270, 340 );
			SetDex( 60, 78 );
			SetInt( 103, 150 );

			SetHits( 158, 203 );

			SetDamage( 5, 7 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 40, 50 );
			SetResistance( ResistanceType.Fire, 30, 40 );
			SetResistance( ResistanceType.Cold, 25, 35 );
			SetResistance( ResistanceType.Poison, 10, 20 );
			SetResistance( ResistanceType.Energy, 10, 20 );

			SetSkill( SkillName.Anatomy, 80.8, 89.2 );
			SetSkill( SkillName.MagicResist, 121, 128.7 );
			SetSkill( SkillName.Tactics, 81.2, 87.5 );
			SetSkill( SkillName.Wrestling, 93.6, 104.3 );

			Fame = 10000;
			Karma = -10000;

			if ( 0.25 > Utility.RandomDouble() )
				PackItem( new GoblinBlood() );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich );
			AddLoot( LootPack.Gems, 1 );
		}

		public override LoyaltyGroup LoyaltyGroupEnemy { get { return LoyaltyGroup.GargoyleQueen; } }
		public override int LoyaltyPointsAward { get { return 2; } }
		public override SlayerName SlayerGroup { get { return SlayerName.Repond; } }
		public override bool CanRummageCorpses { get { return true; } }
		public override int Meat { get { return 1; } }
		public override FoodType FavoriteFood { get { return FoodType.Meat; } }

		public EnslavedGrayGoblin( Serial serial )
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
