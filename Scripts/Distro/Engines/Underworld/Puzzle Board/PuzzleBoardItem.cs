using System;
using Server;
using Server.Engines.PuzzleBoard;

namespace Server.Items
{
	public class PuzzleBoardItem : TransientItem
	{
		public override int LabelNumber { get { return 1113379; } } // Puzzle Board

		private GameBoard m_Board;
		public GameBoard Board { get { return m_Board; } }

		[Constructable]
		public PuzzleBoardItem()
			: base( 0x2AAA, TimeSpan.FromMinutes( 30.0 ) )
		{
			m_Board = GameBoard.MakeBoard();

			LootType = LootType.Blessed;
			Weight = 5.0;
			Hue = 0x281;
		}

		public override bool NonTransferable { get { return true; } }

		public override void HandleInvalidTransfer( Mobile from )
		{
			from.SendLocalizedMessage( 1076254 ); // That item cannot be dropped.
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( !m_Board.Initialized )
				m_Board.Reset();

			from.CloseGump<PuzzleBoardGump>();
			from.SendGump( new PuzzleBoardGump( this ) );
		}

		public PuzzleBoardItem( Serial serial )
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

			Delete();
		}
	}
}