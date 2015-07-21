using System;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a vorpal bunny corpse" )]
	public class SummonVorpalBunny : BaseTalisNPCSummon
	{
		[Constructable]
		public SummonVorpalBunny()
		{
			Name = "a vorpal bunny";
			Body = 205;
			Hue = 0x480;

			SetStr( 15 );
			SetDex( 2000 );
			SetInt( 1000 );

			SetHits( 2000 );
			SetStam( 500 );
			SetMana( 0 );

			SetDamage( 1 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetSkill( SkillName.MagicResist, 200.0 );
			SetSkill( SkillName.Tactics, 5.0 );
			SetSkill( SkillName.Wrestling, 5.0 );

			Fame = 1000;
			Karma = 0;
		}

		public override bool BardImmune { get { return false; } }

		public SummonVorpalBunny( Serial serial )
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