using System;
using Server;
using Server.Items;
using Server.Engines.Loyalty;

namespace Server.Mobiles
{
	[CorpseName( "a fire daemon corpse" )]
	public class FireDaemonRenowned : BaseCreature, IRenowned
	{
		private static readonly Type[] m_Rewards = new Type[]
			{
				typeof( MantleOfTheFallen ),
				typeof( ResonantStaffOfEnlightenment ),
				typeof( LegacyOfDespair )
			};

		public Type[] Rewards { get { return m_Rewards; } }

		[Constructable]
		public FireDaemonRenowned()
			: base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "fire daemon [Renowned]";
			Body = 10;
			BaseSoundID = 357;

			Hue = 1359;

			SetStr( 1119, 1180 );
			SetDex( 229, 239 );
			SetInt( 160, 226 );

			SetHits( 1186, 1358 );

			SetDamage( 22, 29 );

			SetDamageType( ResistanceType.Physical, 50 );
			SetDamageType( ResistanceType.Fire, 25 );
			SetDamageType( ResistanceType.Energy, 25 );

			SetResistance( ResistanceType.Physical, 70, 75 );
			SetResistance( ResistanceType.Fire, 70, 75 );
			SetResistance( ResistanceType.Cold, 50, 60 );
			SetResistance( ResistanceType.Poison, 100 );
			SetResistance( ResistanceType.Energy, 40, 50 );

			SetSkill( SkillName.Anatomy, 45.0, 50.0 );
			SetSkill( SkillName.MagicResist, 120.0, 125.0 );
			SetSkill( SkillName.Tactics, 95.0, 100.0 );
			SetSkill( SkillName.Wrestling, 90.0, 100.0 );
			SetSkill( SkillName.EvalInt, 95.0, 100.0 );
			SetSkill( SkillName.Magery, 95.0, 100.0 );
			SetSkill( SkillName.Meditation, 30.0, 50.0 );

			Fame = 24000;
			Karma = -24000;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.UltraRich );
			AddLoot( LootPack.FilthyRich );
			AddLoot( LootPack.RareGems );
		}

		public override LoyaltyGroup LoyaltyGroupEnemy { get { return LoyaltyGroup.GargoyleQueen; } }
		public override int LoyaltyPointsAward { get { return 50; } }

		public override int Meat { get { return 1; } }

		public FireDaemonRenowned( Serial serial )
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
