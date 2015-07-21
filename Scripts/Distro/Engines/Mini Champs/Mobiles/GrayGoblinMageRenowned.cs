using System;
using Server;
using Server.Items;
using Server.Engines.Loyalty;

namespace Server.Mobiles
{
	[CorpseName( "a gray goblin mage corpse" )]
	public class GrayGoblinMageRenowned : BaseCreature, IRenowned
	{
		private static readonly Type[] m_Rewards = new Type[]
			{
				typeof( CavalrysFolly ),
				typeof( StormCaller ),
				typeof( TorcOfTheGuardians ),
				typeof( GiantSteps )
			};

		public Type[] Rewards { get { return m_Rewards; } }

		[Constructable]
		public GrayGoblinMageRenowned()
			: base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "gray goblin mage [Renowned]";
			Body = 723;

			Hue = 2301;

			SetStr( 547, 566 );
			SetDex( 77, 89 );
			SetInt( 551, 592 );

			SetHits( 1066, 1130 );

			SetDamage( 5, 7 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 30, 40 );
			SetResistance( ResistanceType.Fire, 45, 55 );
			SetResistance( ResistanceType.Cold, 40, 50 );
			SetResistance( ResistanceType.Poison, 45, 55 );
			SetResistance( ResistanceType.Energy, 20, 30 );

			SetSkill( SkillName.Anatomy, 94.1, 96.4 );
			SetSkill( SkillName.MagicResist, 140.6, 147.2 );
			SetSkill( SkillName.Tactics, 91.2, 96.3 );
			SetSkill( SkillName.Wrestling, 93.5, 101.1 );
			SetSkill( SkillName.EvalInt, 102.8, 115.8 );
			SetSkill( SkillName.Magery, 111.2, 115.3 );
			SetSkill( SkillName.Meditation, 105.4, 107.1 );

			Fame = 25000;
			Karma = -25000;

			PackItem( new GoblinBlood() );
			PackItem( new BolaBall() );
		}

		public override int GetAngerSound() { return 0x600; }
		public override int GetAttackSound() { return 0x5FD; }
		public override int GetHurtSound() { return 0x5FF; }
		public override int GetDeathSound() { return 0x5FE; }

		public override LoyaltyGroup LoyaltyGroupEnemy { get { return LoyaltyGroup.GargoyleQueen; } }
		public override int LoyaltyPointsAward { get { return 50; } }

		public override void GenerateLoot()
		{
			AddLoot( LootPack.UltraRich );
			AddLoot( LootPack.Gems );
			AddLoot( LootPack.RareGems );
		}

		public GrayGoblinMageRenowned( Serial serial )
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
