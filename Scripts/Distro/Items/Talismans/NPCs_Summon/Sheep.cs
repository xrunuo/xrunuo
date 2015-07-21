using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
	[CorpseName( "a sheep corpse" )]
	public class SummonSheep : BaseTalisNPCSummon
	{

		[Constructable]
		public SummonSheep()
		{
			Name = "a sheep";
			Body = 0xCF;

			SetStr( 19 );
			SetDex( 25 );
			SetInt( 5 );

			SetHits( 12 );
			SetMana( 0 );

			SetDamage( 1, 2 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 5, 10 );

			SetSkill( SkillName.MagicResist, 5.0 );
			SetSkill( SkillName.Tactics, 6.0 );
			SetSkill( SkillName.Wrestling, 5.0 );

			Fame = 300;
			Karma = 0;
		}

		public SummonSheep( Serial serial )
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