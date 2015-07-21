using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a daemon corpse" )]
	public class Archdemon : BaseCreature
	{
		[Constructable]
		public Archdemon()
			: base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "archdemon";
			Body = 9;
			BaseSoundID = 357;

			Hue = 1106;

			SetStr( 476, 505 );
			SetDex( 76, 95 );
			SetInt( 301, 325 );

			SetHits( 386, 403 );

			SetDamage( 7, 14 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 45, 60 );
			SetResistance( ResistanceType.Fire, 50, 60 );
			SetResistance( ResistanceType.Cold, 30, 40 );
			SetResistance( ResistanceType.Poison, 20, 30 );
			SetResistance( ResistanceType.Energy, 30, 40 );

			SetSkill( SkillName.EvalInt, 97.1, 117.7 );
			SetSkill( SkillName.Magery, 107.2, 123.6 );
			SetSkill( SkillName.Meditation, 93.2, 104.0 );
			SetSkill( SkillName.MagicResist, 100.5, 110.7 );
			SetSkill( SkillName.Tactics, 120.5, 139.7 );
			SetSkill( SkillName.Wrestling, 73.1, 89.2 );

			Fame = 15000;
			Karma = -15000;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich );
			AddLoot( LootPack.Rich );
		}

		public override bool CanRummageCorpses { get { return true; } }
		public override Poison PoisonImmune { get { return Poison.Regular; } }
		public override int TreasureMapLevel { get { return 4; } }
		public override int Meat { get { return 1; } }

		public Archdemon( Serial serial )
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