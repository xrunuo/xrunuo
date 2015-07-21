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
using System.Collections.Generic;
using System.Text;

namespace Server.Network
{
	public static class GenericPackets
	{
		public static GenericPacket PlaySound( int soundID, IPoint3D target )
		{
			GenericPacket p = GenericPacket.Instantiate( 0x54, 12 );

			p.Stream.Write( (byte) 1 ); // flags
			p.Stream.Write( (short) soundID );
			p.Stream.Write( (short) 0 ); // volume
			p.Stream.Write( (short) target.X );
			p.Stream.Write( (short) target.Y );
			p.Stream.Write( (short) target.Z );

			return p;
		}

		public static GenericPacket MobileMoving( Mobile m, int noto )
		{
			GenericPacket p = GenericPacket.Instantiate( 0x77, 17 );

			Point3D loc = m.Location;

			int hue = m.Hue;

			if ( m.SolidHueOverride >= 0 )
				hue = m.SolidHueOverride;

			p.Stream.Write( (int) m.Serial );
			p.Stream.Write( (short) ( (int) m.Body ) );
			p.Stream.Write( (short) loc.X );
			p.Stream.Write( (short) loc.Y );
			p.Stream.Write( (sbyte) loc.Z );
			p.Stream.Write( (byte) m.Direction );
			p.Stream.Write( (short) hue );
			p.Stream.Write( (byte) m.GetPacketFlags() );
			p.Stream.Write( (byte) noto );

			return p;
		}
	}
}
