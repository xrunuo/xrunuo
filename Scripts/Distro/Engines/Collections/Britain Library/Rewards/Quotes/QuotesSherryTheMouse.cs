using System;
using Server;

namespace Server.Items
{
	public class QuotesSherryTheMouse : BaseLibraryQuotes
	{
		public override int MsgMin { get { return 1073301; } }
		public override int MsgMax { get { return 1073309; } }

		public override int LabelNumber { get { return 1073300; } } // Library Friends - Quotes from the pen of Sherry the Mouse

		[Constructable]
		public QuotesSherryTheMouse()
		{
		}

		public QuotesSherryTheMouse( Serial serial )
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