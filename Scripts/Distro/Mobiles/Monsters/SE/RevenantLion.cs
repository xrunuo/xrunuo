using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a revenant lion corpse" )]
	public class RevenantLion : BaseCreature
	{
		public override WeaponAbility GetWeaponAbility()
		{
			return WeaponAbility.BleedAttack;
		}

		[Constructable]
		public RevenantLion()
			: base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a revenant lion";
			Body = 251;

			SetStr( 275, 320 );
			SetDex( 155, 175 );
			SetInt( 75, 105 );

			SetHits( 250, 280 );

			SetDamage( 19, 23 );

			SetDamageType( ResistanceType.Physical, 30 );
			SetDamageType( ResistanceType.Cold, 30 );
			SetDamageType( ResistanceType.Poison, 10 );
			SetDamageType( ResistanceType.Energy, 30 );

			SetResistance( ResistanceType.Physical, 40, 60 );
			SetResistance( ResistanceType.Fire, 20, 30 );
			SetResistance( ResistanceType.Cold, 50, 60 );
			SetResistance( ResistanceType.Poison, 55, 65 );
			SetResistance( ResistanceType.Energy, 40, 50 );

			SetSkill( SkillName.EvalInt, 80.1, 90.0 );
			SetSkill( SkillName.Magery, 80.1, 90.0 );
			SetSkill( SkillName.Meditation, 85.1, 95.0 );
			SetSkill( SkillName.MagicResist, 70.1, 90.0 );
			SetSkill( SkillName.Tactics, 60.1, 80.0 );
			SetSkill( SkillName.Wrestling, 80.1, 90.0 );

			Fame = 2500;
			Karma = -2500;

			Tamable = false;

			PackNecroReg( 6, 8 );
			PackItem( new Bone( Utility.RandomMinMax( 2, 3 ) ) );
			PackItem( BaseLootHelperSE.RandomBodyPart() );
		}

		public override int GetAngerSound()
		{
			return 0x519;
		}

		public override int GetIdleSound()
		{
			return 0x518;
		}

		public override int GetAttackSound()
		{
			return 0x517;
		}

		public override int GetHurtSound()
		{
			return 0x51A;
		}

		public override int GetDeathSound()
		{
			return 0x516;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich, 2 );
			AddLoot( LootPack.MedScrolls, 2 );
		}

		public override Poison PoisonImmune { get { return Poison.Greater; } }
		public override Poison HitPoison { get { return Poison.Greater; } }

		public RevenantLion( Serial serial )
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
