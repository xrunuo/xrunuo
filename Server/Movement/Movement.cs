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

namespace Server.Movement
{
	public class Movement
	{
		private static IMovementImpl m_Impl;

		public static IMovementImpl Impl
		{
			get { return m_Impl; }
			set { m_Impl = value; }
		}

		public static bool CheckMovement( Mobile m, Direction d, out int newZ )
		{
			if ( m_Impl != null )
				return m_Impl.CheckMovement( m, d, out newZ );

			newZ = m.Z;
			return false;
		}

		public static bool CheckMovement( Mobile m, Map map, Point3D loc, Direction d, out int newZ )
		{
			if ( m_Impl != null )
				return m_Impl.CheckMovement( m, map, loc, d, out newZ );

			newZ = m.Z;
			return false;
		}

		public static void Offset( Direction d, ref int x, ref int y )
		{
			switch ( d & Direction.Mask )
			{
				case Direction.North: --y; break;
				case Direction.South: ++y; break;
				case Direction.West: --x; break;
				case Direction.East: ++x; break;
				case Direction.Right: ++x; --y; break;
				case Direction.Left: --x; ++y; break;
				case Direction.Down: ++x; ++y; break;
				case Direction.Up: --x; --y; break;
			}
		}
	}

	public interface IMovementImpl
	{
		bool CheckMovement( Mobile m, Direction d, out int newZ );
		bool CheckMovement( Mobile m, Map map, Point3D loc, Direction d, out int newZ );
	}
}