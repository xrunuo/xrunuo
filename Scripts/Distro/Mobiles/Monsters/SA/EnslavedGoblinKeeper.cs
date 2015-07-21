using System;
using Server;
using Server.Items;
using Server.Engines.Loyalty;

namespace Server.Mobiles
{
	[CorpseName( "a gray goblin's corpse" )]
	public class EnslavedGoblinKeeper : BaseCreature
	{
		[Constructable]
		public EnslavedGoblinKeeper()
			: base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "enslaved goblin keeper";
			Body = 723;

			Hue = 2301;

			SetStr( 274, 297 );
			SetDex( 66, 77 );
			SetInt( 117, 120 );

			SetHits( 157, 186 );

			SetDamage( 5, 7 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 40, 50 );
			SetResistance( ResistanceType.Fire, 30, 40 );
			SetResistance( ResistanceType.Cold, 25, 35 );
			SetResistance( ResistanceType.Poison, 10, 20 );
			SetResistance( ResistanceType.Energy, 10, 20 );

			SetSkill( SkillName.Anatomy, 80.9, 89.1 );
			SetSkill( SkillName.MagicResist, 121, 127 );
			SetSkill( SkillName.Tactics, 84.4, 88.6 );
			SetSkill( SkillName.Wrestling, 99.8, 105.7 );

			Fame = 15000;
			Karma = -15000;

			PackItem( new BolaBall() );
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

		public override bool CanRummageCorpses { get { return true; } }
		public override int Meat { get { return 1; } }
		public override FoodType FavoriteFood { get { return FoodType.Meat; } }

		public EnslavedGoblinKeeper( Serial serial )
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
