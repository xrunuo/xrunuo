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

namespace Server.Targeting
{
	public class LandTarget : IPoint3D
	{
		private Point3D m_Location;
		private int m_TileID;

		public LandTarget( Point3D location, Map map )
		{
			m_Location = location;

			if ( map != null )
			{
				m_Location.Z = map.GetAverageZ( m_Location.X, m_Location.Y );
				m_TileID = map.Tiles.GetLandTile( m_Location.X, m_Location.Y ).ID & TileData.MaxLandValue;
			}
		}

		[CommandProperty( AccessLevel.Counselor )]
		public string Name
		{
			get
			{
				return TileData.LandTable[m_TileID].Name;
			}
		}

		[CommandProperty( AccessLevel.Counselor )]
		public TileFlag Flags
		{
			get
			{
				return TileData.LandTable[m_TileID].Flags;
			}
		}

		[CommandProperty( AccessLevel.Counselor )]
		public int TileID
		{
			get
			{
				return m_TileID;
			}
		}

		[CommandProperty( AccessLevel.Counselor )]
		public Point3D Location
		{
			get
			{
				return m_Location;
			}
		}

		[CommandProperty( AccessLevel.Counselor )]
		public int X
		{
			get
			{
				return m_Location.X;
			}
		}

		[CommandProperty( AccessLevel.Counselor )]
		public int Y
		{
			get
			{
				return m_Location.Y;
			}
		}

		[CommandProperty( AccessLevel.Counselor )]
		public int Z
		{
			get
			{
				return m_Location.Z;
			}
		}
	}
}