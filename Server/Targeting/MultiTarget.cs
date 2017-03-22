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