using System;
using Server;
using Server.Items;
using Server.Engines.Loyalty;

namespace Server.Mobiles
{
	[CorpseName( "an acid elemental corpse" )]
	public class AcidElementalRenowned : BaseCreature, IRenowned
	{
		private static readonly Type[] m_Rewards = new Type[]
			{
				typeof( BreastplateOfTheBerserker ),
				typeof( MysticsGarb )
			};

		public Type[] Rewards { get { return m_Rewards; } }

		[Constructable]
		public AcidElementalRenowned()
			: base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "acid elemental [Renowned]";
			Body = 158;
			BaseSoundID = 278;

			Hue = 1167;

			SetStr( 537, 553 );
			SetDex( 134, 138 );
			SetInt( 379, 394 );

			SetHits( 2212, 2226 );

			SetDamage( 9, 15 );

			SetDamageType( ResistanceType.Physical, 25 );
			SetDamageType( ResistanceType.Fire, 50 );
			SetDamageType( ResistanceType.Energy, 25 );

			SetResistance( ResistanceType.Physical, 45, 50 );
			SetResistance( ResistanceType.Fire, 40, 45 );
			SetResistance( ResistanceType.Cold, 25, 30 );
			SetResistance( ResistanceType.Poison, 15, 20 );
			SetResistance( ResistanceType.Energy, 30, 35 );

			SetSkill( SkillName.Anatomy, 65.0, 75.0 );
			SetSkill( SkillName.MagicResist, 70.0, 80.0 );
			SetSkill( SkillName.Tactics, 90.0, 100.0 );
			SetSkill( SkillName.Wrestling, 90.0, 100.0 );
			SetSkill( SkillName.EvalInt, 85.0, 95.0 );
			SetSkill( SkillName.Magery, 85.0, 95.0 );

			Fame = 12500;
			Karma = -12500;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich, 2 );
			AddLoot( LootPack.RareGems );
		}

		public override LoyaltyGroup LoyaltyGroupEnemy { get { return LoyaltyGroup.GargoyleQueen; } }
		public override int LoyaltyPointsAward { get { return 50; } }

		public override bool BleedImmune { get { return true; } }
		public override Poison HitPoison { get { return Poison.Lethal; } }
		public override double HitPoisonChance { get { return 0.6; } }

		public AcidElementalRenowned( Serial serial )
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
