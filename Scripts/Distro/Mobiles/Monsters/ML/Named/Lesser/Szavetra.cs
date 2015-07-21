using System;
using System.Collections;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "Szavetra corpse" )]
	public class Szavetra : Succubus
	{
		[Constructable]
		public Szavetra()
		{
			Name = "Szavetra";

			SetStr( 600, 650 );
			SetDex( 150, 200 );
			SetInt( 550, 600 );

			SetHits( 400, 450 );
		}

		public Szavetra( Serial serial )
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