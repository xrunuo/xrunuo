using System;
using Server;

namespace Server.Items
{
	public class QuotesHoraceTrader : BaseLibraryQuotes
	{
		public override int MsgMin { get { return 1073332; } }
		public override int MsgMax { get { return 1073337; } }

		public override int LabelNumber { get { return 1073338; } } // Library Friends - Quotes from the pen of Horace, Trader

		[Constructable]
		public QuotesHoraceTrader()
		{
		}

		public QuotesHoraceTrader( Serial serial )
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