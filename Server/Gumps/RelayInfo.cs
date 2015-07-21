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

namespace Server.Gumps
{
	public class TextRelay
	{
		private int m_EntryID;
		private string m_Text;

		public TextRelay( int entryID, string text )
		{
			m_EntryID = entryID;
			m_Text = text;
		}

		public int EntryID
		{
			get
			{
				return m_EntryID;
			}
		}

		public string Text
		{
			get
			{
				return m_Text;
			}
		}
	}

	public class RelayInfo
	{
		private int m_ButtonID;
		private int[] m_Switches;
		private TextRelay[] m_TextEntries;

		public RelayInfo( int buttonID, int[] switches, TextRelay[] textEntries )
		{
			m_ButtonID = buttonID;
			m_Switches = switches;
			m_TextEntries = textEntries;
		}

		public int ButtonID
		{
			get
			{
				return m_ButtonID;
			}
		}

		public int[] Switches
		{
			get
			{
				return m_Switches;
			}
		}

		public TextRelay[] TextEntries
		{
			get
			{
				return m_TextEntries;
			}
		}

		public bool IsSwitched( int switchID )
		{
			for ( int i = 0; i < m_Switches.Length; ++i )
			{
				if ( m_Switches[i] == switchID )
				{
					return true;
				}
			}

			return false;
		}

		public TextRelay GetTextEntry( int entryID )
		{
			for ( int i = 0; i < m_TextEntries.Length; ++i )
			{
				if ( m_TextEntries[i].EntryID == entryID )
				{
					return m_TextEntries[i];
				}
			}

			return null;
		}
	}
}