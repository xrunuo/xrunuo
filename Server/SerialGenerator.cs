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

namespace Server
{
	public class SerialGenerator
	{
		private static Serial m_LastMobile = 0x00000000;
		private static Serial m_LastItem = 0x40000000;

		public static Serial GetNewMobileSerial()
		{
			while ( World.Instance.FindMobile( ++m_LastMobile ) != null )
				;

			return m_LastMobile;
		}

		public static Serial GetNewItemSerial()
		{
			while ( World.Instance.FindItem( ++m_LastItem ) != null )
				;

			return m_LastItem;
		}
	}
}
