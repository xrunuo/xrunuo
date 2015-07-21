using System;
using Server;
using Server.Items;
using Server.Engines.Loyalty;

namespace Server.Mobiles
{
	[CorpseName( "a devourer of souls corpse" )]
	public class DevourerOfSoulsRenowned : BaseCreature, IRenowned
	{
		private static readonly Type[] m_Rewards = new Type[]
			{
				typeof( AnimatedLegsOfTheInsaneTinker ),
				typeof( PillarOfStrength ),
				typeof( StormCaller )
			};

		public Type[] Rewards { get { return m_Rewards; } }

		[Constructable]
		public DevourerOfSoulsRenowned()
			: base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Devourer of Souls [Renowned]";
			Body = 303;
			BaseSoundID = 357;

			SetStr( 891, 910 );
			SetDex( 132, 167 );
			SetInt( 214, 230 );

			SetHits( 1892, 2036 );

			SetDamage( 22, 26 );

			SetDamageType( ResistanceType.Physical, 60 );
			SetDamageType( ResistanceType.Cold, 20 );
			SetDamageType( ResistanceType.Energy, 20 );

			SetResistance( ResistanceType.Physical, 45, 55 );
			SetResistance( ResistanceType.Fire, 25, 30 );
			SetResistance( ResistanceType.Cold, 15, 20 );
			SetResistance( ResistanceType.Poison, 65, 70 );
			SetResistance( ResistanceType.Energy, 45, 50 );

			SetSkill( SkillName.MagicResist, 90.0, 100.0 );
			SetSkill( SkillName.Tactics, 75.0, 80.0 );
			SetSkill( SkillName.Wrestling, 90.0, 100.0 );
			SetSkill( SkillName.EvalInt, 90.0, 100.0 );
			SetSkill( SkillName.Magery, 90.0, 100.0 );
			SetSkill( SkillName.Meditation, 90.0, 100.0 );

			Fame = 18000;
			Karma = -18000;
		}

		public override LoyaltyGroup LoyaltyGroupEnemy { get { return LoyaltyGroup.GargoyleQueen; } }
		public override int LoyaltyPointsAward { get { return 50; } }

		public override void GenerateLoot()
		{
			AddLoot( LootPack.UltraRich );
			AddLoot( LootPack.RareGems );
			// TODO: dawn's music box rare gear
		}

		public DevourerOfSoulsRenowned( Serial serial )
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