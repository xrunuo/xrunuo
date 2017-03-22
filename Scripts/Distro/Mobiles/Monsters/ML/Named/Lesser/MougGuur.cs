using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "Moug-Guur corpse" )]
	public class MougGuur : Ettin
	{
		[Constructable]
		public MougGuur()
		{
			Name = "Moug-Guur";

			SetStr( 556, 560 );
			SetDex( 84, 88 );
			SetInt( 61, 73 );

			SetHits( 409, 411 );

			SetDamage( 7, 17 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 60, 65 );
			SetResistance( ResistanceType.Fire, 15, 20 );
			SetResistance( ResistanceType.Cold, 40, 50 );
			SetResistance( ResistanceType.Poison, 15, 25 );
			SetResistance( ResistanceType.Energy, 15, 25 );

			SetSkill( SkillName.MagicResist, 70.1, 75.0 );
			SetSkill( SkillName.Tactics, 80.1, 85.0 );
			SetSkill( SkillName.Wrestling, 90.1, 100.0 );

			Fame = 5000;
			Karma = -5000;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Meager );
			AddLoot( LootPack.Average, 2 );
			AddLoot( LootPack.Potions );
		}

		public override bool CanRummageCorpses { get { return true; } }
		public override int TreasureMapLevel { get { return 1; } }
		public override int Meat { get { return 4; } }
		public override SlayerName SlayerGroup { get { return SlayerName.Repond; } }

		public MougGuur( Serial serial )
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
