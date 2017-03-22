using System;
using System.Collections;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a minotaur scout corpse" )]
	public class MinotaurScout : BaseCreature
	{
		public override WeaponAbility GetWeaponAbility()
		{
			return WeaponAbility.ParalyzingBlow;
		}

		//private DateTime m_Delay = DateTime.UtcNow;

		public override int GetDeathSound()
		{
			return 0x596;
		}
		public override int GetAttackSound()
		{
			return 0x597;
		}
		public override int GetIdleSound()
		{
			return 0x598;
		}
		public override int GetAngerSound()
		{
			return 0x599;
		}
		public override int GetHurtSound()
		{
			return 0x59A;
		}

		[Constructable]
		public MinotaurScout()
			: base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a minotaur scout";
			Body = 281;

			SetStr( 350, 375 );
			SetDex( 110, 130 );
			SetInt( 30, 50 );

			SetHits( 350, 400 );

			SetDamage( 11, 20 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 55, 65 );
			SetResistance( ResistanceType.Fire, 25, 35 );
			SetResistance( ResistanceType.Cold, 30, 40 );
			SetResistance( ResistanceType.Poison, 30, 40 );
			SetResistance( ResistanceType.Energy, 30, 40 );

			SetSkill( SkillName.MagicResist, 60, 70 );
			SetSkill( SkillName.Tactics, 85, 105 );
			SetSkill( SkillName.Wrestling, 85, 105 );
			SetSkill( SkillName.DetectHidden, 100 );

			Fame = 10000;
			Karma = -10000;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich );
		}// 479,3

		public override int Meat { get { return 2; } }

		public MinotaurScout( Serial serial )
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

			/*int version = */reader.ReadInt();
		}
	}
}
