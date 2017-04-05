using System;
using Server;
using Server.Network;

namespace Server.Targeting
{
	public abstract class MultiTarget : Target
	{
		public int MultiID { get; set; }

		public Point3D Offset { get; set; }

		public MultiTarget( int multiID, Point3D offset )
			: this( multiID, offset, 10, true, TargetFlags.None )
		{
		}

		public MultiTarget( int multiID, Point3D offset, int range, bool allowGround, TargetFlags flags )
			: base( range, allowGround, flags )
		{
			MultiID = multiID;
			Offset = offset;
		}

		public override Packet GetPacketFor( NetState ns )
		{
			return new MultiTargetReq( this );
		}
	}
}