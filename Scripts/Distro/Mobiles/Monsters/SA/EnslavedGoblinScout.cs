using System;
using Server;
using Server.Items;
using Server.Engines.Loyalty;

namespace Server.Mobiles
{
	[CorpseName( "a green goblin scout corpse" )]
	public class EnslavedGoblinScout : BaseCreature
	{
		[Constructable]
		public EnslavedGoblinScout()
			: base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "enslaved goblin scout";
			Body = 723;

			SetStr( 290, 340 );
			SetDex( 69, 79 );
			SetInt( 116, 143 );

			SetHits( 168, 199 );

			SetDamage( 5, 7 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 40, 50 );
			SetResistance( ResistanceType.Fire, 30, 40 );
			SetResistance( ResistanceType.Cold, 25, 35 );
			SetResistance( ResistanceType.Poison, 10, 20 );
			SetResistance( ResistanceType.Energy, 10, 20 );

			SetSkill( SkillName.Anatomy, 83, 87.3 );
			SetSkill( SkillName.MagicResist, 90.6, 98.5 );
			SetSkill( SkillName.Tactics, 80.5, 88.5 );
			SetSkill( SkillName.Wrestling, 110.2, 116.5 );

			Fame = 15000;
			Karma = -15000;

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
		public override int LoyaltyPointsAward { get { return 5; } }
		public override SlayerName SlayerGroup { get { return SlayerName.Repond; } }
		public override bool CanRummageCorpses { get { return true; } }
		public override int Meat { get { return 1; } }
		public override FoodType FavoriteFood { get { return FoodType.Meat; } }

		public EnslavedGoblinScout( Serial serial )
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
