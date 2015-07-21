using System;
using System.Collections;
using Server.Misc;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "Chiikkaha corpse" )]
	public class Chiikkaha : RatmanMage
	{
		[Constructable]
		public Chiikkaha()
		{
			Name = "Chiikkaha";
			Title = "the Toothed";

			SetStr( 450, 500 );
			SetDex( 150, 200 );
			SetInt( 250, 300 );

			SetHits( 400, 120 );

			SetResistance( ResistanceType.Energy, 100 );

			SetSkill( SkillName.EvalInt, 70.1, 80.0 );
			SetSkill( SkillName.Magery, 80.1, 90.0 );
			SetSkill( SkillName.MagicResist, 75.1, 95.0 );
			SetSkill( SkillName.Tactics, 50.1, 75.0 );
			SetSkill( SkillName.Wrestling, 50.1, 75.0 );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich, 2 );
			AddLoot( LootPack.LowScrolls );
		}

		public Chiikkaha( Serial serial )
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