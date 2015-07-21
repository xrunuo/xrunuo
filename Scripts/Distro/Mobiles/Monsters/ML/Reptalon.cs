using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a reptalon corpse" )]
	public class Reptalon : BaseMount
	{
		public override bool StatLossAfterTame { get { return true; } }

		public override WeaponAbility GetWeaponAbility()
		{
			return WeaponAbility.ParalyzingBlow;
		}

		[Constructable]
		public Reptalon()
			: base( "a reptalon", 276, 0x3E90, AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			BaseSoundID = 362; // TODO: Verify

			SetStr( 1000, 1025 );
			SetDex( 150, 165 );
			SetInt( 250, 290 );

			SetHits( 830, 930 );

			SetDamage( 21, 28 );

			SetDamageType( ResistanceType.Physical, 0 );
			SetDamageType( ResistanceType.Poison, 25 );
			SetDamageType( ResistanceType.Energy, 75 );

			SetResistance( ResistanceType.Physical, 50, 65 );
			SetResistance( ResistanceType.Fire, 35, 45 );
			SetResistance( ResistanceType.Cold, 35, 45 );
			SetResistance( ResistanceType.Poison, 50, 65 );
			SetResistance( ResistanceType.Energy, 70, 85 );

			SetSkill( SkillName.Anatomy, 55.0, 60.0 );
			SetSkill( SkillName.MagicResist, 75.0, 90.0 );
			SetSkill( SkillName.Tactics, 100.0, 110.0 );
			SetSkill( SkillName.Wrestling, 100.0, 120.0 );

			Fame = 10000;
			Karma = -10000;

			Tamable = true;
			ControlSlots = 4;
			MinTameSkill = 101.1;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich );
		}

		public Reptalon( Serial serial )
			: base( serial )
		{
		}

		public override bool HasBreath { get { return true; } } // fire breath enabled

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */reader.ReadInt();
		}
	}
}