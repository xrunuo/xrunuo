using System;
using Server.Network;

namespace Server.Targeting
{
	public abstract class Target
	{
		private static int m_NextTargetID;

		public DateTime TimeoutTime { get; private set; }

		public Target( int range, bool allowGround, TargetFlags flags )
		{
			TargetID = ++m_NextTargetID;
			Range = range;
			AllowGround = allowGround;
			Flags = flags;

			CheckLOS = true;
		}

		public static void Cancel( Mobile m )
		{
			NetState ns = m.NetState;

			if ( ns != null )
				ns.Send( CancelTarget.Instance );

			Target targ = m.Target;

			if ( targ != null )
				targ.OnTargetCancel( m, TargetCancelType.Canceled );
		}

		private Timer m_TimeoutTimer;

		public void BeginTimeout( Mobile from, TimeSpan delay )
		{
			TimeoutTime = DateTime.UtcNow + delay;

			if ( m_TimeoutTimer != null )
				m_TimeoutTimer.Stop();

			m_TimeoutTimer = new TimeoutTimer( this, from, delay );
			m_TimeoutTimer.Start();
		}

		public void CancelTimeout()
		{
			if ( m_TimeoutTimer != null )
				m_TimeoutTimer.Stop();

			m_TimeoutTimer = null;
		}

		public void Timeout( Mobile from )
		{
			CancelTimeout();
			from.ClearTarget();

			Cancel( from );

			OnTargetCancel( from, TargetCancelType.Timeout );
			OnTargetFinish( from );
		}

		private class TimeoutTimer : Timer
		{
			private readonly Target m_Target;
			private readonly Mobile m_Mobile;

			public TimeoutTimer( Target target, Mobile m, TimeSpan delay )
				: base( delay )
			{
				m_Target = target;
				m_Mobile = m;
			}

			protected override void OnTick()
			{
				if ( m_Mobile.Target == m_Target )
					m_Target.Timeout( m_Mobile );
			}
		}

		public bool CheckLOS { get; set; }

		public bool DisallowMultis { get; set; }

		public bool AllowNonlocal { get; set; }

		public int TargetID { get; }

		public virtual Packet GetPacketFor( NetState ns )
		{
			return new TargetReq( this );
		}

		public void Cancel( Mobile from, TargetCancelType type )
		{
			CancelTimeout();
			from.ClearTarget();

			OnTargetCancel( from, type );
			OnTargetFinish( from );
		}

		public void Invoke( Mobile from, object targeted )
		{
			CancelTimeout();
			from.ClearTarget();

			if ( from.Deleted )
			{
				OnTargetCancel( from, TargetCancelType.Canceled );
				OnTargetFinish( from );
				return;
			}

			IPoint3D loc;
			Map map;

			if ( targeted is LandTarget )
			{
				loc = ( (LandTarget) targeted ).Location;
				map = from.Map;
			}
			else if ( targeted is StaticTarget )
			{
				loc = ( (StaticTarget) targeted ).Location;
				map = from.Map;
			}
			else if ( targeted is Mobile )
			{
				if ( ( (Mobile) targeted ).Deleted )
				{
					OnTargetDeleted( from, targeted );
					OnTargetFinish( from );
					return;
				}
				else if ( !( (Mobile) targeted ).CanTarget )
				{
					OnTargetUntargetable( from, targeted );
					OnTargetFinish( from );
					return;
				}

				loc = ( (Mobile) targeted ).Location;
				map = ( (Mobile) targeted ).Map;
			}
			else if ( targeted is Item )
			{
				Item item = (Item) targeted;

				if ( item.Deleted )
				{
					OnTargetDeleted( from, targeted );
					OnTargetFinish( from );
					return;
				}
				else if ( !item.CanTarget )
				{
					OnTargetUntargetable( from, targeted );
					OnTargetFinish( from );
					return;
				}

				object root = item.RootParent;

				if ( !AllowNonlocal && root is Mobile && root != from && from.AccessLevel == AccessLevel.Player )
				{
					OnNonlocalTarget( from, targeted );
					OnTargetFinish( from );
					return;
				}

				loc = item.GetWorldLocation();
				map = item.Map;
			}
			else
			{
				OnTargetCancel( from, TargetCancelType.Canceled );
				OnTargetFinish( from );
				return;
			}

			if ( map == null || map != from.Map || ( Range != -1 && (int) from.GetDistanceToSqrt( loc ) > Range ) )
			{
				OnTargetOutOfRange( from, targeted );
			}
			else
			{
				if ( !from.CanSee( targeted ) )
					OnCantSeeTarget( from, targeted );
				else if ( CheckLOS && !from.InLOS( targeted ) )
					OnTargetOutOfLOS( from, targeted );
				else if ( targeted is Item && ( (Item) targeted ).InSecureTrade )
					OnTargetInSecureTrade( from, targeted );
				else if ( targeted is Item && !( (Item) targeted ).IsAccessibleTo( from ) )
					OnTargetNotAccessible( from, targeted );
				else if ( targeted is Item && !( (Item) targeted ).CheckTarget( from, this, targeted ) )
					OnTargetUntargetable( from, targeted );
				else if ( targeted is Mobile && !( (Mobile) targeted ).CheckTarget( from, this, targeted ) )
					OnTargetUntargetable( from, targeted );
				else if ( from.Region.OnTarget( from, this, targeted ) )
					OnTarget( from, targeted );
			}

			OnTargetFinish( from );
		}

		protected virtual void OnTarget( Mobile from, object targeted )
		{
		}

		protected virtual void OnTargetNotAccessible( Mobile from, object targeted )
		{
			from.SendLocalizedMessage( 500447 ); // That is not accessible.
		}

		protected virtual void OnTargetInSecureTrade( Mobile from, object targeted )
		{
			from.SendLocalizedMessage( 500447 ); // That is not accessible.
		}

		protected virtual void OnNonlocalTarget( Mobile from, object targeted )
		{
			from.SendLocalizedMessage( 500447 ); // That is not accessible.
		}

		protected virtual void OnCantSeeTarget( Mobile from, object targeted )
		{
			from.SendLocalizedMessage( 500237 ); // Target can not be seen.
		}

		protected virtual void OnTargetOutOfLOS( Mobile from, object targeted )
		{
			from.SendLocalizedMessage( 500237 ); // Target can not be seen.
		}

		protected virtual void OnTargetOutOfRange( Mobile from, object targeted )
		{
			from.SendLocalizedMessage( 500446 ); // That is too far away.
		}

		protected virtual void OnTargetDeleted( Mobile from, object targeted )
		{
		}

		protected virtual void OnTargetUntargetable( Mobile from, object targeted )
		{
			from.SendLocalizedMessage( 500447 ); // That is not accessible.
		}

		protected virtual void OnTargetCancel( Mobile from, TargetCancelType cancelType )
		{
		}

		protected virtual void OnTargetFinish( Mobile from )
		{
		}

		public int Range { get; set; }

		public bool AllowGround { get; set; }

		public TargetFlags Flags { get; set; }
	}
}