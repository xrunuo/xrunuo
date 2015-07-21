using System;
using Server;
using Server.Items;
using Server.Engines.Loyalty;

namespace Server.Mobiles
{
	[CorpseName( "a pixie's corpse" )]
	public class PixieRenowned : BaseCreature, IRenowned
	{
		private static readonly Type[] m_Rewards = new Type[]
			{
				typeof( DemonHuntersStandard ),
				typeof( DragonJadeEarrings ),
				typeof( PillarOfStrength ),
				typeof( SwordOfShatteredHopes )
			};

		public Type[] Rewards { get { return m_Rewards; } }

		[Constructable]
		public PixieRenowned()
			: base( AIType.AI_Mage, FightMode.Evil, 10, 1, 0.2, 0.4 )
		{
			Name = "a pixie [Renowned]";
			Body = 176;
			BaseSoundID = 0x467;

			SetStr( 276, 345 );
			SetDex( 365, 595 );
			SetInt( 568, 735 );

			SetHits( 9134, 9155 );

			SetDamage( 27, 38 );

			SetDamageType( ResistanceType.Physical, 75 );
			SetDamageType( ResistanceType.Cold, 25 );

			SetResistance( ResistanceType.Physical, 70, 80 );
			SetResistance( ResistanceType.Fire, 60, 70 );
			SetResistance( ResistanceType.Cold, 70, 80 );
			SetResistance( ResistanceType.Poison, 60, 70 );
			SetResistance( ResistanceType.Energy, 55, 70 );

			SetSkill( SkillName.MagicResist, 109.0, 137.2 );
			SetSkill( SkillName.Tactics, 97.9, 122.4 );
			SetSkill( SkillName.Wrestling, 97.8, 122.2 );
			SetSkill( SkillName.EvalInt, 100.0 );
			SetSkill( SkillName.Magery, 97.7, 107.0 );
			SetSkill( SkillName.Meditation, 100 );

			Fame = 20000;
			Karma = 20000;

			PackItem( new FaeryDust() );
		}

		public override bool Unprovokable { get { return true; } }
		public override Poison PoisonImmune { get { return Poison.Regular; } }
		public override int Meat { get { return 1; } }

		public override LoyaltyGroup LoyaltyGroupEnemy { get { return LoyaltyGroup.GargoyleQueen; } }
		public override int LoyaltyPointsAward { get { return 50; } }

		public override void GenerateLoot()
		{
			AddLoot( LootPack.UltraRich, 2 );
			AddLoot( LootPack.RareGems );
		}

		public PixieRenowned( Serial serial )
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