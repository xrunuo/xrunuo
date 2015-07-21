using System;
using Server;
using Server.Engines.Loyalty;

namespace Server.Mobiles
{
	[CorpseName( "a skeletal lich corpse" )]
	public class SkeletalLich : BaseCreature
	{
		[Constructable]
		public SkeletalLich()
			: base( AIType.AI_Necromancer, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "A Skeletal Lich";
			Body = 308;
			BaseSoundID = 0x48D;

			Hue = 1109;

			SetStr( 450 );
			SetDex( 151, 163 );
			SetInt( 171, 202 );

			SetHits( 1500 );

			SetDamage( 34, 36 );

			SetDamageType( ResistanceType.Physical, 50 );
			SetDamageType( ResistanceType.Cold, 50 );

			SetResistance( ResistanceType.Physical, 35, 45 );
			SetResistance( ResistanceType.Fire, 25, 30 );
			SetResistance( ResistanceType.Cold, 55, 65 );
			SetResistance( ResistanceType.Energy, 40, 50 );

			SetSkill( SkillName.MagicResist, 50.0, 70.0 );
			SetSkill( SkillName.Tactics, 75 );
			SetSkill( SkillName.Wrestling, 90 );
			SetSkill( SkillName.EvalInt, 80.0, 90.0 );
			SetSkill( SkillName.Magery, 75.0, 85.0 );
			SetSkill( SkillName.Meditation, 100 );
			SetSkill( SkillName.SpiritSpeak, 90.0, 100.0 );
			SetSkill( SkillName.Necromancy, 85.0, 95.0 );

			Fame = 15000;
			Karma = -15000;
		}

		public override void GenerateLoot()
		{
			base.GenerateLoot();

			AddLoot( LootPack.UltraRich, 2 );
		}

		public override Poison PoisonImmune { get { return Poison.Lethal; } }

		public override LoyaltyGroup LoyaltyGroupEnemy { get { return LoyaltyGroup.GargoyleQueen; } }
		public override int LoyaltyPointsAward { get { return 8; } }

		public SkeletalLich( Serial serial )
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