using System;
using Server;
using Server.Network;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.PuzzleBoard
{
	public class PuzzleBoardGump : Gump
	{
		public override int TypeID { get { return 0x2713; } }

		private PuzzleBoardItem m_Item;
		private int m_SelectedRow;

		public PuzzleBoardGump( PuzzleBoardItem item )
			: this( item, -1 )
		{
		}

		public PuzzleBoardGump( PuzzleBoardItem item, int selectedRow )
			: base( 5, 30 )
		{
			m_Item = item;
			m_SelectedRow = selectedRow;

			GameBoard board = item.Board;

			Panel player = board.GamePanel;
			Panel target = board.TargetPanel;

			AddBackground( 55, 45, 500, 200, 0x2422 );
			AddImage( 75, 83, 0x2423 );
			AddImage( 65, 118, 0x2423 );
			AddImage( 75, 153, 0x2423 );
			AddImage( 65, 188, 0x2423 );
			AddImage( 108, 55, 0x2427 );
			AddImage( 86, 65, 0x2427 );
			AddBackground( 75, 65, 86, 153, 0x2422 );
			AddBackground( 192, 65, 137, 153, 0x2422 );
			AddBackground( 397, 65, 137, 153, 0x2422 );
			AddBackground( 55, 270, 195, 110, 0x2422 );
			AddImage( 205, 77, 0x52 );
			AddImage( 205, 110, 0x52 );
			AddImage( 205, 143, 0x52 );
			AddImage( 410, 77, 0x52 );
			AddImage( 410, 110, 0x52 );
			AddImage( 410, 143, 0x52 );
			AddImage( 5, 5, 0x28C8 );

			AddButton( 160, 320, 0xF2, 0xF1, 8, GumpButtonType.Reply, 0 );		// Cancel
			AddButton( 80, 320, 0xEF, 0xF0, 7, GumpButtonType.Reply, 0 );		// Apply
			AddButton( 120, 345, 0x7DB, 0x7DB, 0, GumpButtonType.Reply, 0 );	// Log out

			AddHtml( 72, 285, 170, 20, "Command Functions: ", false, false );
			AddHtml( 200, 285, 100, 20, String.Format( "{0}/{1}", board.CurrentMovements, board.MaxMovements ), false, false );

			for ( int i = 0; i < 4; i++ )
			{
				bool button = m_SelectedRow != i;
				bool arrows = !button && !player.Rows[i].IsEmpty;

				if ( button )
					AddButton( 108, 82 + ( i * 33 ), 0xD1, 0xD0, i + 1, GumpButtonType.Reply, 0 );
				else
					AddImage( 108, 82 + ( i * 33 ), 0xD0 );

				if ( arrows )
				{
					AddButton( 88, 82 + ( i * 33 ), 0xA5A, 0xA5B, 5, GumpButtonType.Reply, 0 );
					AddButton( 128, 82 + ( i * 33 ), 0xA58, 0xA59, 6, GumpButtonType.Reply, 0 );
				}
				else
				{
					AddImage( 88, 82 + ( i * 33 ), 0xA95 );
					AddImage( 128, 82 + ( i * 33 ), 0xA95 );
				}
			}

			DrawPanel( player, 205, 76 );
			DrawPanel( target, 410, 76 );
		}

		private void DrawPanel( Panel panel, int xBase, int yBase )
		{
			for ( int i = 0; i < 4; i++ )
				DrawRow( panel.Rows[i], xBase, yBase + ( i * 33 ) );
		}

		private void DrawRow( Row row, int xBase, int yBase )
		{
			int graphic = GetGraphic( row );

			switch ( row.Pegs )
			{
				default:
				case 0:
					break;
				case 1: // 1 peg
					AddImage( xBase + 40, yBase, graphic );
					break;
				case 2: // 2 pegs
					AddImage( xBase + 2, yBase, graphic );
					AddImage( xBase + 80, yBase, graphic );
					break;
				case 3: // 3 pegs
					AddImage( xBase + 2, yBase, graphic );
					AddImage( xBase + 40, yBase, graphic );
					AddImage( xBase + 80, yBase, graphic );
					break;
				case 4: // bar
					AddImage( xBase + 1, yBase, graphic );
					break;
			}
		}

		private int GetGraphic( Row row )
		{
			bool bar = row.IsBar;

			switch ( row.Color )
			{
				default:
				case Color.Red: return bar ? 0x2A58 : 0x2A62;
				case Color.Green: return bar ? 0x2A44 : 0x2A4E;
				case Color.Blue: return bar ? 0x2A30 : 0x2A3A;
			}
		}

		public override void OnResponse( NetState sender, RelayInfo info )
		{
			if ( m_Item.Deleted )
				return;

			Mobile from = sender.Mobile;
			int id = info.ButtonID;
			GameBoard board = m_Item.Board;

			switch ( id )
			{
				case 5: // move up
					{
						if ( m_SelectedRow == -1 )
							return;

						board.GamePanel.MoveUp( m_SelectedRow );
						board.CurrentMovements++;

						from.PlaySound( 0xFA );
						from.SendGump( new PuzzleBoardGump( m_Item, m_SelectedRow ) );

						break;
					}
				case 6: // move down
					{
						if ( m_SelectedRow == -1 )
							return;

						board.GamePanel.MoveDown( m_SelectedRow );
						board.CurrentMovements++;

						from.PlaySound( 0xFA );
						from.SendGump( new PuzzleBoardGump( m_Item, m_SelectedRow ) );

						break;
					}
				case 7: // apply
					{
						if ( board.IsCorrect() )
						{
							from.LocalOverheadMessage( MessageType.Regular, 0x3B2, true, "Correct Code Entered. Crystal Lock Disengaged." );
							from.PlaySound( 0xF7 );

							m_Item.Delete();

							Item reward;

							switch ( Utility.Random( 10 ) )
							{
								default:
								case 0: reward = new VoidEssence( 30 ); break;
								case 1: reward = new SilverSerpentVenom( 30 ); break;
								case 2: reward = new ScouringToxin( 30 ); break;
								case 3: reward = new ToxicVenomSac( 30 ); break;
								case 4: reward = new KneadingBowl(); break;
								case 5: reward = new TotemPole(); break;
								case 6: reward = new DustyPillow(); break;
								case 7: reward = new Plinth(); break;
								case 8: reward = new FlouredBreadBoard(); break;
								case 9: reward = new LuckyCoin(); break;
							}

							from.PlaceInBackpack( reward );

							PlayerMobile pm = from as PlayerMobile;

							if ( pm != null && !Misc.TestCenter.Enabled )
								pm.NextPuzzleAttempt = DateTime.UtcNow + TimeSpan.FromDays( 1.0 );
						}
						else
						{
							m_Item.PublicOverheadMessage( MessageType.Regular, 0x3B2, true, "Incorrect Code Sequence. Access Denied" );
							from.PlaySound( 0xFD );
						}

						break;
					}
				case 8: // cancel
					{
						board.Reset();

						from.PlaySound( 0xFB );
						from.SendGump( new PuzzleBoardGump( m_Item, -1 ) );

						break;
					}
				default:
					{
						if ( id >= 1 && id <= 4 )
							from.SendGump( new PuzzleBoardGump( m_Item, id - 1 ) );

						break;
					}
			}
		}
	}
}