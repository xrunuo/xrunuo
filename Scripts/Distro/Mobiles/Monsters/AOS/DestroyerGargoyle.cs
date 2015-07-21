using System;
using System.Collections;
using Server.Items;
using Server.Targeting;
using Server.Misc;
using Server.Spells;
using Server.Spells.Fourth;

namespace Server.Mobiles
{
	[CorpseName( "a Gargoyle Destroyer Corpse" )]
	public class DestroyerGargoyle : BaseCreature
	{

		[Constructable]
		public DestroyerGargoyle()
			: base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a gargoyle destroyer";
			Body = 755;
			BaseSoundID = 372;

			SetStr( 760, 850 );
			SetDex( 102, 150 );
			SetInt( 152, 200 );

			SetHits( 482, 485 );

			SetDamage( 7, 14 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 40, 60 );
			SetResistance( ResistanceType.Fire, 60, 70 );
			SetResistance( ResistanceType.Cold, 15, 25 );
			SetResistance( ResistanceType.Poison, 15, 25 );
			SetResistance( ResistanceType.Energy, 15, 25 );

			SetSkill( SkillName.EvalInt, 91.1, 100.0 );
			SetSkill( SkillName.Magery, 91.1, 100.0 );
			SetSkill( SkillName.Meditation, 91.1, 100.0 );
			SetSkill( SkillName.MagicResist, 120.4, 160.0 );
			SetSkill( SkillName.Tactics, 90.1, 100.0 );
			SetSkill( SkillName.Wrestling, 90.1, 100.0 );
			SetSkill( SkillName.Swords, 90.1, 100.0 );
			SetSkill( SkillName.Anatomy, 50.1, 100.0 );

			SetFameLevel( 5 );
			SetKarmaLevel( 5 );

			PackGold( 180, 240 );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich, 1 );
			AddLoot( LootPack.MedScrolls, Utility.RandomMinMax( 1, 3 ) );
			AddLoot( LootPack.Gems, 2 );
		}

		public override bool BardImmune { get { return true; } }
		public override int Meat { get { return 1; } }

		public DestroyerGargoyle( Serial serial )
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
