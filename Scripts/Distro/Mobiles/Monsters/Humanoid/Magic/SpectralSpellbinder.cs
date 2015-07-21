using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a ghostly corpse" )]
	public class SpectralSpellbinder : BaseCreature
	{
		[Constructable]
		public SpectralSpellbinder()
			: base( AIType.AI_WeakMage, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			Name = "a spectral spellbinder";
			Body = 153;
			BaseSoundID = 0x482;

			SetStr( 46, 70 );
			SetDex( 47, 65 );
			SetInt( 187, 210 );

			SetHits( 36, 50 );

			SetDamage( 3, 6 );

			SetDamageType( ResistanceType.Physical, 50 );
			SetDamageType( ResistanceType.Cold, 50 );

			SetResistance( ResistanceType.Physical, 25, 30 );
			SetResistance( ResistanceType.Cold, 15, 25 );
			SetResistance( ResistanceType.Poison, 10, 20 );

			SetSkill( SkillName.MagicResist, 35.1, 50.0 );
			SetSkill( SkillName.Tactics, 35.1, 45.0 );
			SetSkill( SkillName.Wrestling, 35.1, 45.0 );
			SetSkill( SkillName.Necromancy, 35.0, 45.0 );
			SetSkill( SkillName.Magery, 40.1, 50.0 );
			SetSkill( SkillName.EvalInt, 30.0, 40.0 );
			SetSkill( SkillName.SpiritSpeak, 45.0, 55.0 );

			Fame = 2500;
			Karma = -2500;

			if ( Utility.RandomBool() )
			{
				PackReg( 10 );
			}
			else
			{
				PackNecroReg( 10 );
			}
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Meager );
		}

		public override bool BleedImmune { get { return true; } }
		public override Poison PoisonImmune { get { return Poison.Regular; } }

		public SpectralSpellbinder( Serial serial )
			: base( serial )
		{
		}

		public override OppositionGroup OppositionGroup
		{
			get { return OppositionGroup.FeyAndUndead; }
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