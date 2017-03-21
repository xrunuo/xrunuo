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
using Server;
using Server.Network;

namespace Server.Targeting
{
	public abstract class MultiTarget : Target
	{
		private int m_MultiID;
		private Point3D m_Offset;

		public int MultiID
		{
			get { return m_MultiID; }
			set { m_MultiID = value; }
		}

		public Point3D Offset
		{
			get { return m_Offset; }
			set { m_Offset = value; }
		}

		public MultiTarget( int multiID, Point3D offset )
			: this( multiID, offset, 10, true, TargetFlags.None )
		{
		}

		public MultiTarget( int multiID, Point3D offset, int range, bool allowGround, TargetFlags flags )
			: base( range, allowGround, flags )
		{
			m_MultiID = multiID;
			m_Offset = offset;
		}

		public override Packet GetPacketFor( NetState ns )
		{
			return new MultiTargetReq( this );
		}
	}
}