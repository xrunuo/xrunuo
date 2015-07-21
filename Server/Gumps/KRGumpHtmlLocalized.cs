//
//  X-RunUO - Ultima Online Server Emulator
//  Copyright (C) 2015 Pedro Pardal
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//

using System;
using Server.Network;

namespace Server.Gumps
{
	public enum KRGumpHtmlLocalizedType
	{
		Plain,
		Color
	}

	public class KRGumpHtmlLocalized : GumpEntry
	{
		private int m_X, m_Y;
		private int m_Width, m_Height;
		private int m_Number;
		private int m_Color;
		private bool m_Background, m_Scrollbar;

		private KRGumpHtmlLocalizedType m_Type;

		public int X
		{
			get
			{
				return m_X;
			}
			set
			{
				Delta( ref m_X, value );
			}
		}

		public int Y
		{
			get
			{
				return m_Y;
			}
			set
			{
				Delta( ref m_Y, value );
			}
		}

		public int Width
		{
			get
			{
				return m_Width;
			}
			set
			{
				Delta( ref m_Width, value );
			}
		}

		public int Height
		{
			get
			{
				return m_Height;
			}
			set
			{
				Delta( ref m_Height, value );
			}
		}

		public int Number
		{
			get
			{
				return m_Number;
			}
			set
			{
				Delta( ref m_Number, value );
			}
		}

		public int Color
		{
			get
			{
				return m_Color;
			}
			set
			{
				Delta( ref m_Color, value );
			}
		}

		public bool Background
		{
			get
			{
				return m_Background;
			}
			set
			{
				Delta( ref m_Background, value );
			}
		}

		public bool Scrollbar
		{
			get
			{
				return m_Scrollbar;
			}
			set
			{
				Delta( ref m_Scrollbar, value );
			}
		}

		public KRGumpHtmlLocalizedType Type
		{
			get
			{
				return m_Type;
			}
			set
			{
				if ( m_Type != value )
					m_Type = value;
			}
		}

		public KRGumpHtmlLocalized( int x, int y, int width, int height, int number, bool background, bool scrollbar )
			: this( x, y, width, height, number, 0, background, scrollbar )
		{
		}

		public KRGumpHtmlLocalized( int x, int y, int width, int height, int number, int color, bool background, bool scrollbar )
		{
			m_X = x;
			m_Y = y;
			m_Width = width;
			m_Height = height;
			m_Number = number;
			m_Background = background;
			m_Scrollbar = scrollbar;

			if ( color != 0 )
				m_Type = KRGumpHtmlLocalizedType.Color;
		}

		public override string Compile()
		{
			switch ( m_Type )
			{
				default:
				case KRGumpHtmlLocalizedType.Plain:
					return String.Format( "{{ kr_xmfhtmlgump {0} {1} {2} {3} {4} {5} {6} }}", m_X, m_Y, m_Width, m_Height, m_Number, m_Background ? 1 : 0, m_Scrollbar ? 1 : 0 );
				case KRGumpHtmlLocalizedType.Color:
					return String.Format( "{{ kr_xmfhtmlgumpcolor {0} {1} {2} {3} {4} {5} {6} {7} }}", m_X, m_Y, m_Width, m_Height, m_Number, m_Background ? 1 : 0, m_Scrollbar ? 1 : 0, m_Color );
			}
		}

		private static byte[] m_LayoutNamePlain = Gump.StringToBuffer( "kr_xmfhtmlgump" );
		private static byte[] m_LayoutNameColor = Gump.StringToBuffer( "kr_xmfhtmlgumpcolor" );

		public override void AppendTo( IGumpWriter disp )
		{
			switch ( m_Type )
			{
				case KRGumpHtmlLocalizedType.Plain:
					{
						disp.AppendLayout( m_LayoutNamePlain );
						disp.AppendLayout( m_X );
						disp.AppendLayout( m_Y );
						disp.AppendLayout( m_Width );
						disp.AppendLayout( m_Height );
						disp.AppendLayout( m_Number );
						disp.AppendLayout( m_Background );
						disp.AppendLayout( m_Scrollbar );

						break;
					}
				case KRGumpHtmlLocalizedType.Color:
					{
						disp.AppendLayout( m_LayoutNameColor );
						disp.AppendLayout( m_X );
						disp.AppendLayout( m_Y );
						disp.AppendLayout( m_Width );
						disp.AppendLayout( m_Height );
						disp.AppendLayout( m_Number );
						disp.AppendLayout( m_Background );
						disp.AppendLayout( m_Scrollbar );
						disp.AppendLayout( m_Color );

						break;
					}
			}
		}
	}
}