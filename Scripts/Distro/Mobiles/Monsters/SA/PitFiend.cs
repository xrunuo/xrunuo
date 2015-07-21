using System;
using Server;

namespace Server.Mobiles
{
	[CorpseName( "a pit fiend's corpse" )]
	public class PitFiend : BaseCreature
	{
		[Constructable]
		public PitFiend()
			: base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a pit fiend";
			Body = 43;
			BaseSoundID = 357;

			Hue = 1136;

			SetStr( 378, 439 );
			SetDex( 176, 216 );
			SetInt( 201, 246 );

			SetHits( 230, 285 );

			SetDamage( 8, 19 );

			SetDamageType( ResistanceType.Physical, 20 );
			SetDamageType( ResistanceType.Cold, 80 );

			SetResistance( ResistanceType.Physical, 55, 65 );
			SetResistance( ResistanceType.Fire, 10, 20 );
			SetResistance( ResistanceType.Cold, 60, 70 );
			SetResistance( ResistanceType.Poison, 20, 30 );
			SetResistance( ResistanceType.Energy, 30, 40 );

			SetSkill( SkillName.MagicResist, 95.0, 115.0 );
			SetSkill( SkillName.Tactics, 95.0, 105.0 );
			SetSkill( SkillName.Wrestling, 80.0, 115.0 );
			SetSkill( SkillName.EvalInt, 95.0, 120.0 );
			SetSkill( SkillName.Magery, 105.0, 130.0 );

			Fame = 18000;
			Karma = -18000;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich );
			AddLoot( LootPack.Average, 3 );
			AddLoot( LootPack.MedScrolls, 2 );
		}

		public override bool CanRummageCorpses { get { return true; } }
		public override Poison PoisonImmune { get { return Poison.Regular; } }
		public override int TreasureMapLevel { get { return 4; } }
		public override int Meat { get { return 1; } }

		public PitFiend( Serial serial )
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