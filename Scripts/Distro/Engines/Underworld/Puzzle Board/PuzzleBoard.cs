using System;
using Server;

namespace Server.Engines.PuzzleBoard
{
	public enum Color
	{
		Red,
		Green,
		Blue
	}

	public class GameBoard
	{
		#region Predefined Boards
		private static GameBoard[] m_PredefinedBoards = new GameBoard[]
		{
			new GameBoard(
				new Panel( new Row[] {
					new Row( 1, Color.Red ),
					new Row( 1, Color.Red ),
					new Row( 1, Color.Red ),
					new Row( 1, Color.Red ) } ),
				new Panel( new Row[] {
					new Row( 1, Color.Green ),
					new Row( 1, Color.Red ),
					new Row( 1, Color.Green ),
					new Row( 1, Color.Blue ) } ), 6 ),

			new GameBoard(
				new Panel( new Row[] {
					new Row( 4, Color.Blue ),
					new Row( 2, Color.Red ),
					new Row( 4, Color.Red ),
					new Row( 1, Color.Blue ) } ),
				new Panel( new Row[] {
					new Row( 1, Color.Red ),
					new Row( 4, Color.Green ),
					new Row( 0, Color.Green ),
					new Row( 4, Color.Blue ) } ), 6 ),

			new GameBoard(
				new Panel( new Row[] {
					new Row( 4, Color.Green ),
					new Row( 2, Color.Blue ),
					new Row( 3, Color.Red ),
					new Row( 4, Color.Red ) } ),
				new Panel( new Row[] {
					new Row( 0, Color.Red ),
					new Row( 0, Color.Red ),
					new Row( 4, Color.Red ),
					new Row( 3, Color.Red ) } ), 4 ),

			new GameBoard(
				new Panel( new Row[] {
					new Row( 1, Color.Red ),
					new Row( 2, Color.Green ),
					new Row( 3, Color.Blue ),
					new Row( 4, Color.Red ) } ),
				new Panel( new Row[] {
					new Row( 2, Color.Red ),
					new Row( 0, Color.Red ),
					new Row( 0, Color.Red ),
					new Row( 2, Color.Blue ) } ), 7 ),

			new GameBoard(
				new Panel( new Row[] {
					new Row( 1, Color.Green ),
					new Row( 4, Color.Red ),
					new Row( 2, Color.Red ),
					new Row( 1, Color.Green ) } ),
				new Panel( new Row[] {
					new Row( 1, Color.Blue ),
					new Row( 1, Color.Green ),
					new Row( 2, Color.Blue ),
					new Row( 4, Color.Blue ) } ), 7 ),

			new GameBoard(
				new Panel( new Row[] {
					new Row( 4, Color.Red ),
					new Row( 4, Color.Green ),
					new Row( 4, Color.Red ),
					new Row( 4, Color.Blue ) } ),
				new Panel( new Row[] {
					new Row( 2, Color.Blue ),
					new Row( 4, Color.Green ),
					new Row( 1, Color.Green ),
					new Row( 1, Color.Red ) } ), 8 ),

			new GameBoard(
				new Panel( new Row[] {
					new Row( 4, Color.Red ),
					new Row( 2, Color.Green ),
					new Row( 2, Color.Green ),
					new Row( 4, Color.Red ) } ),
				new Panel( new Row[] {
					new Row( 1, Color.Green ),
					new Row( 1, Color.Red ),
					new Row( 2, Color.Blue ),
					new Row( 4, Color.Blue ) } ), 5 ),

			new GameBoard(
				new Panel( new Row[] {
					new Row( 4, Color.Green ),
					new Row( 1, Color.Blue ),
					new Row( 3, Color.Blue ),
					new Row( 1, Color.Red ) } ),
				new Panel( new Row[] {
					new Row( 2, Color.Green ),
					new Row( 0, Color.Red ),
					new Row( 3, Color.Blue ),
					new Row( 0, Color.Red ) } ), 4 ),

			new GameBoard(
				new Panel( new Row[] {
					new Row( 4, Color.Green ),
					new Row( 0, Color.Red ),
					new Row( 3, Color.Green ),
					new Row( 1, Color.Green ) } ),
				new Panel( new Row[] {
					new Row( 1, Color.Green ),
					new Row( 4, Color.Green ),
					new Row( 2, Color.Red ),
					new Row( 1, Color.Red ) } ), 7 ),

			new GameBoard(
				new Panel( new Row[] {
					new Row( 4, Color.Red ),
					new Row( 4, Color.Red ),
					new Row( 4, Color.Red ),
					new Row( 4, Color.Red ) } ),
				new Panel( new Row[] {
					new Row( 1, Color.Blue ),
					new Row( 2, Color.Red ),
					new Row( 0, Color.Red ),
					new Row( 3, Color.Green ) } ), 6 ),

			new GameBoard(
				new Panel( new Row[] {
					new Row( 1, Color.Red ),
					new Row( 0, Color.Red ),
					new Row( 2, Color.Blue ),
					new Row( 4, Color.Red ) } ),
				new Panel( new Row[] {
					new Row( 1, Color.Blue ),
					new Row( 1, Color.Red ),
					new Row( 3, Color.Blue ),
					new Row( 0, Color.Red ) } ), 6 ),

			new GameBoard(
				new Panel( new Row[] {
					new Row( 2, Color.Red ),
					new Row( 2, Color.Green ),
					new Row( 2, Color.Red ),
					new Row( 2, Color.Blue ) } ),
				new Panel( new Row[] {
					new Row( 1, Color.Green ),
					new Row( 0, Color.Red ),
					new Row( 3, Color.Green ),
					new Row( 2, Color.Green ) } ), 5 ),

			new GameBoard(
				new Panel( new Row[] {
					new Row( 3, Color.Green ),
					new Row( 4, Color.Blue ),
					new Row( 3, Color.Green ),
					new Row( 4, Color.Red ) } ),
				new Panel( new Row[] {
					new Row( 3, Color.Red ),
					new Row( 0, Color.Red ),
					new Row( 0, Color.Red ),
					new Row( 3, Color.Red ) } ), 6 ),

			new GameBoard(
				new Panel( new Row[] {
					new Row( 1, Color.Red ),
					new Row( 4, Color.Green ),
					new Row( 4, Color.Green ),
					new Row( 1, Color.Red ) } ),
				new Panel( new Row[] {
					new Row( 0, Color.Red ),
					new Row( 3, Color.Green ),
					new Row( 1, Color.Green ),
					new Row( 2, Color.Blue ) } ), 7 ),

			new GameBoard(
				new Panel( new Row[] {
					new Row( 3, Color.Green ),
					new Row( 4, Color.Green ),
					new Row( 4, Color.Blue ),
					new Row( 1, Color.Green ) } ),
				new Panel( new Row[] {
					new Row( 3, Color.Red ),
					new Row( 0, Color.Red ),
					new Row( 3, Color.Green ),
					new Row( 0, Color.Red ) } ), 6 ),

			new GameBoard(
				new Panel( new Row[] {
					new Row( 3, Color.Blue ),
					new Row( 2, Color.Green ),
					new Row( 1, Color.Blue ),
					new Row( 2, Color.Red ) } ),
				new Panel( new Row[] {
					new Row( 1, Color.Red ),
					new Row( 2, Color.Blue ),
					new Row( 2, Color.Red ),
					new Row( 1, Color.Green ) } ), 6 )
		};
		#endregion

		private Panel m_GamePanel;
		private Panel m_StartPanel;
		private Panel m_TargetPanel;
		private int m_MaxMovements;
		private int m_CurrentMovements;

		public Panel GamePanel { get { return m_GamePanel; } }
		public Panel StartPanel { get { return m_StartPanel; } }
		public Panel TargetPanel { get { return m_TargetPanel; } }
		public int MaxMovements { get { return m_MaxMovements; } }
		public int CurrentMovements { get { return m_CurrentMovements; } set { m_CurrentMovements = value; } }

		public bool Initialized { get { return m_GamePanel != null; } }

		public void Reset()
		{
			m_GamePanel = m_StartPanel.Clone();
			m_CurrentMovements = 0;
		}

		public GameBoard( Panel startPanel, Panel targetPanel, int maxMovements )
		{
			m_StartPanel = startPanel;
			m_TargetPanel = targetPanel;
			m_MaxMovements = maxMovements;
		}

		public bool IsCorrect()
		{
			if ( m_CurrentMovements > m_MaxMovements )
				return false;

			return Panel.Equal( m_GamePanel, m_TargetPanel );
		}

		public static GameBoard MakeBoard()
		{
			return m_PredefinedBoards[Utility.Random( m_PredefinedBoards.Length )].Clone();
		}

		public GameBoard Clone()
		{
			return new GameBoard( m_StartPanel, m_TargetPanel, m_MaxMovements );
		}
	}

	public class Panel
	{
		private Row[] m_Rows;

		public Row[] Rows { get { return m_Rows; } }

		public Panel( Row[] rows )
		{
			m_Rows = rows;
		}

		public Panel Clone()
		{
			Row[] rows = new Row[m_Rows.Length];

			for ( int i = 0; i < m_Rows.Length; i++ )
				rows[i] = m_Rows[i].Clone();

			return new Panel( rows );
		}

		public static bool Equal( Panel p1, Panel p2 )
		{
			for ( int i = 0; i < 4; i++ )
			{
				if ( !Row.Equal( p1.Rows[i], p2.Rows[i] ) )
					return false;
			}

			return true;
		}

		#region Movements

		public void MoveUp( int rowIndex )
		{
			Row rowFrom = m_Rows[rowIndex];
			Row rowTo = m_Rows[( 4 + rowIndex - 1 ) % 4];

			DoMoveUp( rowFrom, rowTo );
		}

		public void MoveDown( int rowIndex )
		{
			Row rowFrom = m_Rows[rowIndex];
			Row rowTo = m_Rows[( rowIndex + 1 ) % 4];

			DoMoveDown( rowFrom, rowTo );
		}

		private static void DoMoveUp( Row rowFrom, Row rowTo )
		{
			if ( rowFrom.IsEmpty )
				return;

			if ( rowTo.IsBar )
			{
				if ( rowFrom.IsBar )
					ChangeColor( rowFrom, rowTo );
			}
			else if ( rowTo.IsEmpty )
			{
				if ( rowFrom.IsEven )
					Split( rowFrom, rowTo );
			}
			else if ( rowFrom.IsBar )
			{
				Swap( rowFrom, rowTo );
			}
			else
			{
				Add( rowFrom, rowTo );
			}
		}

		private static void DoMoveDown( Row rowFrom, Row rowTo )
		{
			if ( rowFrom.IsEmpty )
				return;

			if ( rowTo.IsEmpty )
			{
				if ( rowFrom.IsEven )
					Split( rowFrom, rowTo );
			}
			else
			{
				Sub( rowFrom, rowTo );
			}
		}

		#endregion

		#region Rules
		private static void ChangeColor( Row rowFrom, Row rowTo )
		{
			rowTo.Color = NextColor( rowFrom.Color, rowTo.Color );
		}

		private static void Swap( Row rowFrom, Row rowTo )
		{
			Color colorFrom = rowFrom.Color;
			Color colorTo = rowTo.Color;
			int pegsFrom = rowFrom.Pegs;
			int pegsTo = rowTo.Pegs;

			rowFrom.Color = colorTo;
			rowTo.Color = colorFrom;
			rowFrom.Pegs = pegsTo;
			rowTo.Pegs = pegsFrom;
		}

		private static void Add( Row rowFrom, Row rowTo )
		{
			Color colorFrom, colorTo;
			int totalPegs, restPegs;

			if ( rowFrom.Pegs == rowTo.Pegs )
			{
				colorTo = NextColor( rowFrom.Color, rowTo.Color );
				colorFrom = rowTo.Color;
			}
			else if ( rowFrom.Pegs > rowTo.Pegs )
			{
				colorTo = rowFrom.Color;
				colorFrom = rowTo.Color;
			}
			else
			{
				colorTo = rowTo.Color;
				colorFrom = rowFrom.Color;
			}

			totalPegs = rowFrom.Pegs + rowTo.Pegs;
			restPegs = Math.Max( totalPegs - 4, 0 );
			totalPegs = Math.Min( totalPegs, 4 );

			rowTo.Color = colorTo;
			rowTo.Pegs = totalPegs;
			rowFrom.Color = colorFrom;
			rowFrom.Pegs = restPegs;
		}

		private static void Sub( Row rowFrom, Row rowTo )
		{
			rowTo.Color = ( rowTo.Pegs > rowFrom.Pegs ? rowTo.Color : rowFrom.Color );
			rowTo.Pegs = Math.Abs( rowFrom.Pegs - rowTo.Pegs );

			rowFrom.Pegs = 0;
		}

		private static void Split( Row rowFrom, Row rowTo )
		{
			rowTo.Pegs = rowFrom.Pegs / 2;
			rowFrom.Pegs = rowTo.Pegs;

			rowFrom.Color = NextColor( rowFrom.Color );
			rowTo.Color = NextColor( rowFrom.Color );
		}
		#endregion

		#region Colors

		public static Color NextColor( Color c )
		{
			return NextColor( c, c );
		}

		public static Color NextColor( Color c1, Color c2 )
		{
			return m_NextColors[(int) c1, (int) c2];
		}

		private static Color[,] m_NextColors = new Color[3, 3]
		{
						  /* Red */		/* Green */		/* Blue */
			/* Red   */	{ Color.Green,	Color.Blue,		Color.Green	},
			/* Green */	{ Color.Blue,	Color.Blue,		Color.Red	},
			/* Blue  */	{ Color.Green,	Color.Red,		Color.Red	}
		};

		#endregion
	}

	public class Row
	{
		private int m_Pegs;
		private Color m_Color;

		public int Pegs { get { return m_Pegs; } set { m_Pegs = value; } }
		public Color Color { get { return m_Color; } set { m_Color = value; } }

		public bool IsBar { get { return m_Pegs == 4; } }
		public bool IsEmpty { get { return m_Pegs == 0; } }
		public bool IsEven { get { return ( m_Pegs % 2 ) == 0; } }

		public Row( int pegs, Color color )
		{
			m_Pegs = pegs;
			m_Color = color;
		}

		public Row Clone()
		{
			return new Row( m_Pegs, m_Color );
		}

		public static bool Equal( Row r1, Row r2 )
		{
			return ( r1.Pegs == r2.Pegs ) && ( r1.Pegs == 0 || r1.Color == r2.Color );
		}
	}
}