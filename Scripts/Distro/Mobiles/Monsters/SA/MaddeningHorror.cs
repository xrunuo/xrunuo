using System;
using Server;
using Server.Engines.Loyalty;

namespace Server.Mobiles
{
	[CorpseName( "a maddening horror corpse" )]
	public class MaddeningHorror : BaseCreature
	{
		[Constructable]
		public MaddeningHorror()
			: base( AIType.AI_Necromancer, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a maddening horror";
			Body = 721;

			SetStr( 217, 255 );
			SetDex( 91, 122 );
			SetInt( 816, 989 );

			SetHits( 558, 664 );

			SetDamage( 15, 27 );

			SetDamageType( ResistanceType.Physical, 20 );
			SetDamageType( ResistanceType.Cold, 40 );
			SetDamageType( ResistanceType.Energy, 40 );

			SetResistance( ResistanceType.Physical, 55, 65 );
			SetResistance( ResistanceType.Fire, 25, 30 );
			SetResistance( ResistanceType.Cold, 50, 60 );
			SetResistance( ResistanceType.Poison, 45, 50 );
			SetResistance( ResistanceType.Energy, 50, 55 );

			SetSkill( SkillName.MagicResist, 190.0, 200.0 );
			SetSkill( SkillName.Tactics, 90.0, 95.0 );
			SetSkill( SkillName.Wrestling, 75.0, 90.0 );
			SetSkill( SkillName.EvalInt, 120.0, 130.0 );
			SetSkill( SkillName.Magery, 120.0, 130.0 );
			SetSkill( SkillName.Necromancy, 120.0, 130.0 );
			SetSkill( SkillName.SpiritSpeak, 120.0, 130.0 );
			SetSkill( SkillName.Meditation, 100.0, 105.0 );

			Fame = 23000;
			Karma = -23000;

			// TODO (SA): Special Drop "Vile Tentacles"
		}

		public override int GetAttackSound() { return 0x60E; }
		public override int GetDeathSound() { return 0x60F; }
		public override int GetHurtSound() { return 0x610; }
		public override int GetIdleSound() { return 0x611; }

		public override LoyaltyGroup LoyaltyGroupEnemy { get { return LoyaltyGroup.GargoyleQueen; } }
		public override int LoyaltyPointsAward { get { return 25; } }

		public override void GenerateLoot()
		{
			AddLoot( LootPack.UltraRich );
		}

		public MaddeningHorror( Serial serial )
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
