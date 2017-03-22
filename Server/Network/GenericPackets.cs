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
