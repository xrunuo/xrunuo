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
using System.IO;

namespace Server
{
	public class ItemBounds
	{
		private static Rectangle2D[] m_Bounds;

		public static Rectangle2D[] Table
		{
			get
			{
				return m_Bounds;
			}
		}

		static ItemBounds()
		{
			if ( File.Exists( "Data/Binary/Bounds.bin" ) )
			{
				using ( FileStream fs = new FileStream( "Data/Binary/Bounds.bin", FileMode.Open, FileAccess.Read, FileShare.Read ) )
				{
					BinaryReader bin = new BinaryReader( fs );

					m_Bounds = new Rectangle2D[0x10000];

					for ( int i = 0; i < 0x10000; ++i )
					{
						int xMin = bin.ReadInt16();
						int yMin = bin.ReadInt16();
						int xMax = bin.ReadInt16();
						int yMax = bin.ReadInt16();

						m_Bounds[i].Set( xMin, yMin, ( xMax - xMin ) + 1, ( yMax - yMin ) + 1 );
					}

					bin.Close();
				}
			}
			else
			{
				Console.WriteLine( "Warning: Data/Binary/Bounds.bin does not exist" );

				m_Bounds = new Rectangle2D[0x10000];
			}
		}
	}
}