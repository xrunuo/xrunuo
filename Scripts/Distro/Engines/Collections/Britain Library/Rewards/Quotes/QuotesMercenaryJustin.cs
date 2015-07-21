using System;
using Server;

namespace Server.Items
{
	public class QuotesMercenaryJustin : BaseLibraryQuotes
	{
		public override int MsgMin { get { return 1073318; } }
		public override int MsgMax { get { return 1073326; } }

		public override int LabelNumber { get { return 1073317; } } // Library Friends - Quotes from the pen of Mercenary Justin

		[Constructable]
		public QuotesMercenaryJustin()
		{
		}

		public QuotesMercenaryJustin( Serial serial )
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