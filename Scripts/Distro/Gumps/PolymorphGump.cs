using System;
using Server;
using Server.Network;
using Server.Targets;
using Server.Spells;
using Server.Spells.Seventh;

namespace Server.Gumps
{
	public class PolymorphGump : Gump
	{
		public override int TypeID { get { return 0x2336; } }

		private class PolymorphEntry
		{
			private int m_Art, m_Body, m_Num, m_Width, m_Height;

			public PolymorphEntry( int Art, int Body, int LocNum, int Width, int Height )
			{
				m_Art = Art;
				m_Body = Body;
				m_Num = LocNum;
				m_Width = Width;
				m_Height = Height;
			}

			public int ArtID { get { return m_Art; } }
			public int BodyID { get { return m_Body; } }
			public int LocNumber { get { return m_Num; } }
			public int Width { get { return m_Width; } }
			public int Height { get { return m_Height; } }
		}

		private static PolymorphEntry[] FirstPageEntries = new PolymorphEntry[]
			{
				new PolymorphEntry( 8401, 0xD0, 1015236, 15, 10 ), // Chicken
				new PolymorphEntry( 8405, 0xD9, 1015237, 17, 10 ), // Dog
				new PolymorphEntry( 8426, 0xE1, 1015238, 18, 10 ), // Wolf
				new PolymorphEntry( 8473, 0xD6, 1015239, 20, 14 ), // Panther
				new PolymorphEntry( 8437, 0x1D, 1015240, 23, 10 ), // Gorilla
				new PolymorphEntry( 8399, 0xD3, 1015241, 22, 10 ), // Black Bear
				new PolymorphEntry( 8411, 0xD4, 1015242, 22, 12 ), // Grizzly Bear
				new PolymorphEntry( 8417, 0xD5, 1015243, 26, 10 ), // Polar Bear
				new PolymorphEntry( 8397, 0x190, 1015244, 29, 8 ), // Human Male
				new PolymorphEntry( 8398, 0x191, 1015254, 29, 10 ) // Human Female
			};

		private static PolymorphEntry[] SecondPageEntries = new PolymorphEntry[]
			{
				new PolymorphEntry( 8424, 0x33, 1015246, 5, 10 ), // Slime
				new PolymorphEntry( 8416, 0x11, 1015247, 29, 10 ), // Orc
				new PolymorphEntry( 8414, 0x21, 1015248, 26, 10 ), // Lizard Man
				new PolymorphEntry( 8409, 0x04, 1015249, 22, 10 ), // Gargoyle
				new PolymorphEntry( 8415, 0x01, 1015250, 24, 9 ), // Orge
				new PolymorphEntry( 8425, 0x36, 1015251, 25, 9 ), // Troll
				new PolymorphEntry( 8408, 0x02, 1015252, 25, 8 ), // Ettin
				new PolymorphEntry( 8403, 0x09, 1015253, 25, 8 ) // Daemon
			};

		private Mobile m_Caster;
		private Item m_Scroll;

		public PolymorphGump( Mobile caster, Item scroll )
			: base( 60, 36 )
		{
			m_Caster = caster;
			m_Scroll = scroll;

			AddPage( 0 );

			AddBackground( 0, 0, 520, 404, 0x13BE );

			AddImageTiled( 10, 10, 500, 20, 0xA40 );
			AddImageTiled( 10, 40, 500, 324, 0xA40 );
			AddImageTiled( 10, 374, 500, 20, 0xA40 );

			AddAlphaRegion( 10, 10, 500, 384 );

			AddButton( 10, 374, 0xFB1, 0xFB2, 0, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 45, 376, 450, 20, 1060051, 0x7FFF, false, false ); // CANCEL

			AddHtmlLocalized( 14, 12, 500, 20, 1015234, 0x7FFF, false, false ); // <center>Polymorph Selection Menu</center>

			AddPage( 1 );

			for ( int i = 1; i <= FirstPageEntries.Length; i++ )
			{
				int offset = ( i % 2 == 0 ) ? i / 2 : ( i / 2 ) + 1;

				offset--;

				PolymorphEntry pe = FirstPageEntries[i - 1] as PolymorphEntry;

				AddButtonTileArt( ( i % 2 != 0 ) ? 14 : 264, 44 + offset * 64, 0x918, 0x919, GumpButtonType.Reply, 0, 100 + i, pe.ArtID, 0, pe.Width, pe.Height );
				AddHtmlLocalized( ( i % 2 != 0 ) ? 98 : 348, 44 + offset * 64, 250, 60, pe.LocNumber, 0x7FFF, false, false );
			}

			AddButton( 400, 374, 0xFA5, 0xFA7, 0, GumpButtonType.Page, 2 );
			AddHtmlLocalized( 440, 376, 60, 20, 1043353, 0x7FFF, false, false ); // Next

			AddPage( 2 );

			AddButton( 300, 374, 0xFAE, 0xFB0, 0, GumpButtonType.Page, 1 );
			AddHtmlLocalized( 340, 376, 60, 20, 1011393, 0x7FFF, false, false ); // Back

			for ( int i = 1; i <= SecondPageEntries.Length; i++ )
			{
				int offset = ( i % 2 == 0 ) ? i / 2 : ( i / 2 ) + 1;

				offset--;

				PolymorphEntry pe = SecondPageEntries[i - 1] as PolymorphEntry;

				AddButtonTileArt( ( i % 2 != 0 ) ? 14 : 264, 44 + offset * 64, 0x918, 0x919, GumpButtonType.Reply, 0, 110 + i, pe.ArtID, 0, pe.Width, pe.Height );
				AddHtmlLocalized( ( i % 2 != 0 ) ? 98 : 348, 44 + offset * 64, 250, 60, pe.LocNumber, 0x7FFF, false, false );
			}
		}

		public override void OnResponse( NetState state, RelayInfo info )
		{
			int buttonID = info.ButtonID;

			if ( buttonID < 100 )
				return;

			int page = ( buttonID > 110 ) ? 1 : 0;

			switch ( page )
			{
				case 1:
					{
						PolymorphEntry pe = SecondPageEntries[buttonID - 111] as PolymorphEntry;

						Spell spell = new PolymorphSpell( m_Caster, m_Scroll, pe.BodyID );
						spell.Cast();

						break;
					}
				case 0:
					{
						PolymorphEntry pe = FirstPageEntries[buttonID - 101] as PolymorphEntry;

						Spell spell = new PolymorphSpell( m_Caster, m_Scroll, pe.BodyID );
						spell.Cast();

						break;
					}
			}
		}
	}
}