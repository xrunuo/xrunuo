using System;
using Server;
using Server.Engines.Loyalty;

namespace Server.Mobiles
{
	[CorpseName( "a skeletal drake corpse" )]
	public class SkeletalDrake : BaseCreature
	{
		[Constructable]
		public SkeletalDrake()
			: base( AIType.AI_Necromancer, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a skeletal drake";
			Body = 104;
			BaseSoundID = 0x488;

			Hue = 2101;

			SetStr( 642, 718 );
			SetDex( 52, 93 );
			SetInt( 293, 393 );

			SetHits( 360, 398 );

			SetDamage( 29, 35 );

			SetDamageType( ResistanceType.Physical, 75 );
			SetDamageType( ResistanceType.Fire, 25 );

			SetResistance( ResistanceType.Physical, 70, 80 );
			SetResistance( ResistanceType.Fire, 40, 60 );
			SetResistance( ResistanceType.Cold, 40, 60 );
			SetResistance( ResistanceType.Poison, 70, 80 );
			SetResistance( ResistanceType.Energy, 40, 60 );

			SetSkill( SkillName.MagicResist, 83.5, 87 );
			SetSkill( SkillName.Tactics, 77.7, 77.9 );
			SetSkill( SkillName.Wrestling, 67.8, 69.7 );
			SetSkill( SkillName.EvalInt, 60.8, 63 );
			SetSkill( SkillName.Magery, 53.5, 68.2 );
			SetSkill( SkillName.Necromancy, 66.9, 75.1 );
			SetSkill( SkillName.SpiritSpeak, 59.4, 67.5 );

			Fame = 22500;
			Karma = -22500;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.UltraRich, 2 );
		}

		public override bool HasBreath { get { return true; } }
		public override int BreathFireDamage { get { return 0; } }
		public override int BreathColdDamage { get { return 100; } }
		public override int BreathEffectHue { get { return 0x480; } }

		public override LoyaltyGroup LoyaltyGroupEnemy { get { return LoyaltyGroup.GargoyleQueen; } }
		public override int LoyaltyPointsAward { get { return 5; } }

		public override Poison PoisonImmune { get { return Poison.Lethal; } }
		public override bool BleedImmune { get { return true; } }

		public SkeletalDrake( Serial serial )
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
