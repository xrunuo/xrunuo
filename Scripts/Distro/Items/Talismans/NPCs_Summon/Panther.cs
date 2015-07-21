using System;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName( "a panther corpse" )]
	public class SummonPanther : BaseTalisNPCSummon
	{
		[Constructable]
		public SummonPanther()
		{
			Name = "a panther";
			Body = 0xD6;
			Hue = 0x901;

			SetStr( 61, 85 );
			SetDex( 86, 105 );
			SetInt( 26, 50 );

			SetHits( 37, 51 );
			SetMana( 0 );

			SetDamage( 4, 12 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 20, 25 );
			SetResistance( ResistanceType.Fire, 5, 10 );
			SetResistance( ResistanceType.Cold, 10, 15 );
			SetResistance( ResistanceType.Poison, 5, 10 );

			SetSkill( SkillName.MagicResist, 15.1, 30.0 );
			SetSkill( SkillName.Tactics, 50.1, 65.0 );
			SetSkill( SkillName.Wrestling, 50.1, 65.0 );

			Fame = 450;
			Karma = 0;
		}

		public SummonPanther( Serial serial )
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