using System;
using Server;

namespace Server.Items
{
	public class QuotesMoonglow : BaseLibraryQuotes
	{
		public override int MsgMin { get { return 1073328; } }
		public override int MsgMax { get { return 1073336; } }

		public override int LabelNumber { get { return 1073327; } } // Library Friends - Quotes from the pen of Heigel of Moonglow

		[Constructable]
		public QuotesMoonglow()
		{
		}

		public QuotesMoonglow( Serial serial )
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