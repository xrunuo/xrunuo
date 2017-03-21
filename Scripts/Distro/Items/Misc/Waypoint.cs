using System;
using Server;
using Server.Commands;
using Server.Targeting;

namespace Server.Items
{
	[FlipableAttribute( 0x1f14, 0x1f15, 0x1f16, 0x1f17 )]
	public class WayPoint : Item
	{
		public static void Initialize()
		{
			CommandSystem.Register( "WayPointSeq", AccessLevel.GameMaster, new CommandEventHandler( WayPointSeq_OnCommand ) );
		}

		public static void WayPointSeq_OnCommand( CommandEventArgs arg )
		{
			arg.Mobile.SendMessage( "Target the position of the first way point." );
			arg.Mobile.Target = new WayPointSeqTarget( null );
		}

		private WayPoint m_Next;
		private int m_Range = 5;
		private bool m_UnlinkFromSpawner = false;

		[Constructable]
		public WayPoint()
			: base( 0x1f14 )
		{
			Hue = 0x498;
			Visible = false;
			Name = "AI Way Point";
		}

		public WayPoint( WayPoint prev )
			: this()
		{
			if ( prev != null )
			{
				prev.NextPoint = this;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public WayPoint NextPoint
		{
			get { return m_Next; }
			set
			{
				if ( m_Next != this )
				{
					m_Next = value;
				}
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int Range
		{
			get { return m_Range; }
			set { m_Range = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool UnlinkFromSpawner
		{
			get { return m_UnlinkFromSpawner; }
			set { m_UnlinkFromSpawner = value; }
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( from.AccessLevel >= AccessLevel.GameMaster )
			{
				from.SendMessage( "Target the next way point in the sequence." );

				from.Target = new NextPointTarget( this );
			}
		}

		public WayPoint( Serial serial )
			: base( serial )
		{
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 2:
					{
						m_UnlinkFromSpawner = reader.ReadBool();
						goto case 1;
					}
				case 1:
					{
						m_Range = reader.ReadInt();
						goto case 0;
					}
				case 0:
					{
						m_Next = reader.ReadItem() as WayPoint;
						break;
					}
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 2 );

			// Version 2
			writer.Write( (bool) m_UnlinkFromSpawner );

			// Version 1
			writer.Write( m_Range );

			// Version 0
			writer.Write( m_Next );
		}
	}

	public class NextPointTarget : Target
	{
		private WayPoint m_Point;

		public NextPointTarget( WayPoint pt )
			: base( -1, false, TargetFlags.None )
		{
			m_Point = pt;
		}

		protected override void OnTarget( Mobile from, object target )
		{
			if ( target is WayPoint && m_Point != null )
			{
				m_Point.NextPoint = (WayPoint) target;
			}
			else
			{
				from.SendMessage( "Target a way point." );
			}
		}
	}

	public class WayPointSeqTarget : Target
	{
		private WayPoint m_Last;

		public WayPointSeqTarget( WayPoint last )
			: base( -1, true, TargetFlags.None )
		{
			m_Last = last;
		}

		protected override void OnTarget( Mobile from, object targeted )
		{
			if ( targeted is WayPoint )
			{
				if ( m_Last != null )
				{
					m_Last.NextPoint = (WayPoint) targeted;
				}
			}
			else if ( targeted is IPoint3D )
			{
				Point3D p = new Point3D( (IPoint3D) targeted );

				WayPoint point = new WayPoint( m_Last );
				point.MoveToWorld( p, from.Map );

				from.Target = new WayPointSeqTarget( point );
				from.SendMessage( "Target the position of the next way point in the sequence, or target a way point link the newest way point to." );
			}
			else
			{
				from.SendMessage( "Target a position, or another way point." );
			}
		}
	}
}
