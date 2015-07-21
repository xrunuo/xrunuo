using System;
using Server;
using Server.Network;

namespace Server.Items
{
	public class GoldenCompass : TransientItem
	{
		public override int LabelNumber { get { return 1113578; } } // a golden compass

		[Constructable]
		public GoldenCompass()
			: base( 0x1CB, TimeSpan.FromHours( 2.0 ) )
		{
			Weight = 1.0;
			Hue = 0x499;
		}

		public override bool NonTransferable { get { return true; } }

		public override void HandleInvalidTransfer( Mobile from )
		{
			from.SendLocalizedMessage( 1076254 ); // That item cannot be dropped.
		}

		public override void OnDoubleClick( Mobile from )
		{
			// TODO: how this message work? it also says "Nothing Happens." sometimes

			from.LocalOverheadMessage( MessageType.Regular, 0x3B2, 1113585 ); // The compass' arrows flicker. You must be near the right location.
		}

		public GoldenCompass( Serial serial )
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
		}
	}
}