using System;
using Server;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Items
{
	[FlipableAttribute( 0x315A, 0x315B )]
	public class PristineDreadHorn : Item
	{
		public override int LabelNumber { get { return 1032635; } } // Pristine Dread Horn

		[Constructable]
		public PristineDreadHorn()
			: base( 0x315A )
		{
			Weight = 1.0;

			Stackable = true;
		}

		public PristineDreadHorn( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();

			if ( !Stackable )
				Stackable = true;
		}
	}
}