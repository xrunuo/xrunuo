using System;
using Server;
using Server.Items;
using Server.Engines.Loyalty;

namespace Server.Mobiles
{
	[CorpseName( "a skeletal dragon corpse" )]
	public class SkeletalDragonRenowned : BaseCreature, IRenowned
	{
		private static readonly Type[] m_Rewards = new Type[]
			{
				typeof( AxeOfAbandon ),
				typeof( DemonBridleRing ),
				typeof( VoidInfusedKilt ),
				typeof( BladeOfBattle )
			};

		public Type[] Rewards { get { return m_Rewards; } }

		[Constructable]
		public SkeletalDragonRenowned()
			: base( AIType.AI_Necromancer, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a skeletal dragon [Renowned]";
			Body = 104;
			BaseSoundID = 0x488;

			Hue = 2101;

			SetStr( 936, 980 );
			SetDex( 123, 145 );
			SetInt( 517, 546 );

			SetHits( 1120, 1198 );

			SetDamage( 29, 35 );

			SetDamageType( ResistanceType.Physical, 75 );
			SetDamageType( ResistanceType.Fire, 25 );

			SetResistance( ResistanceType.Physical, 75, 80 );
			SetResistance( ResistanceType.Fire, 40, 60 );
			SetResistance( ResistanceType.Cold, 40, 60 );
			SetResistance( ResistanceType.Poison, 70, 80 );
			SetResistance( ResistanceType.Energy, 40, 60 );

			SetSkill( SkillName.MagicResist, 110.6, 125.7 );
			SetSkill( SkillName.Tactics, 97.9, 99.9 );
			SetSkill( SkillName.Wrestling, 97.7, 99.9 );
			SetSkill( SkillName.EvalInt, 83.6, 99.2 );
			SetSkill( SkillName.Magery, 81.7, 93.4 );
			SetSkill( SkillName.SpiritSpeak, 87.5, 101.1 );
			SetSkill( SkillName.Necromancy, 80.2, 98.2 );

			Fame = 22500;
			Karma = -22500;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.UltraRich, 2 );
			AddLoot( LootPack.RareGems );
		}

		public override bool HasBreath { get { return true; } }
		public override int BreathFireDamage { get { return 0; } }
		public override int BreathColdDamage { get { return 100; } }
		public override int BreathEffectHue { get { return 0x480; } }

		public override Poison PoisonImmune { get { return Poison.Lethal; } }
		public override bool BleedImmune { get { return true; } }

		public override LoyaltyGroup LoyaltyGroupEnemy { get { return LoyaltyGroup.GargoyleQueen; } }
		public override int LoyaltyPointsAward { get { return 50; } }

		public SkeletalDragonRenowned( Serial serial )
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
