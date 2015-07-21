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
using System.Collections;
using Server;
using Server.Guilds;
using Server.Items;

namespace Server
{
	public delegate int NotorietyHandler( Mobile source, Mobile target );

	public class Notoriety
	{
		public const int Innocent = 1;
		public const int Ally = 2;
		public const int CanBeAttacked = 3;
		public const int Criminal = 4;
		public const int Enemy = 5;
		public const int Murderer = 6;
		public const int Invulnerable = 7;

		private static NotorietyHandler m_Handler;

		public static NotorietyHandler Handler
		{
			get { return m_Handler; }
			set { m_Handler = value; }
		}

		private static int[] m_Hues = new int[]
			{
				0x000,
				0x059,
				0x03F,
				0x3B2,
				0x3B2,
				0x090,
				0x022,
				0x035
			};

		public static int[] Hues
		{
			get { return m_Hues; }
			set { m_Hues = value; }
		}

		public static int GetHue( int noto )
		{
			if ( noto < 0 || noto >= m_Hues.Length )
				return 0;

			return m_Hues[noto];
		}

		public static int Compute( Mobile source, Mobile target )
		{
			return m_Handler == null ? CanBeAttacked : m_Handler( source, target );
		}
	}
}