using System;
using Server;
using Server.Items;
using Server.Mobiles;
using System.Collections;
using Server.Events;

namespace Server.Regions
{
	public class PeerlessRegion : Region
	{
		private AltarPeerless m_altar;

		public static void Initialize()
		{
			EventSink.Instance.Logout += new LogoutEventHandler( EventSink_Logout );
		}

		public PeerlessRegion( string name, Map map, AltarPeerless altar, Rectangle2D[] regionBounds )
			: base( null, map, Region.Find( altar.Location, altar.Map ), regionBounds )
		{
			m_altar = altar;

			Register();
		}

		public override TimeSpan GetLogoutDelay( Mobile m )
		{
			return TimeSpan.FromMinutes( 10 );
		}

		private static void EventSink_Logout( LogoutEventArgs e )
		{
			Mobile from = e.Mobile;

			PeerlessRegion reg = (PeerlessRegion) from.Region.GetRegion( typeof( PeerlessRegion ) );

			if ( reg != null )
			{
				BaseCreature.TeleportPets( from, PeerlessEntry.GetExitPoint( reg.m_altar.Peerless ), reg.m_altar.Peerless == PeerlessList.Travesty ? Map.Tokuno : PeerlessEntry.GetMap( reg.m_altar.Peerless ) );
				from.MoveToWorld( PeerlessEntry.GetExitPoint( reg.m_altar.Peerless ), reg.m_altar.Peerless == PeerlessList.Travesty ? Map.Tokuno : PeerlessEntry.GetMap( reg.m_altar.Peerless ) );
			}
		}

		public override void OnEnter( Mobile m )
		{
		}
		public override void OnExit( Mobile m )
		{
			int players = 0;
			foreach ( Mobile mob in GetPlayers() )
				if ( mob.AccessLevel == AccessLevel.Player )
					players++;

			if ( players <= 1 && m is PlayerMobile && m_altar != null && m.AccessLevel == AccessLevel.Player )
			{
				if ( !m_altar.Deleted && m_altar.actived && m_altar.Boss.CheckAlive() )
				{
					m_altar.PeerlessT.Stop();
					if ( m_altar.Boss != null && !m_altar.Boss.Deleted )
						m_altar.Boss.Delete();
					m_altar.DeleteKeys( PeerlessEntry.GetLabelNum( m_altar.Peerless ) );
					m_altar.Clear();
				}
			}
		}
		public void BroadcastLocalizedMessage( int num )
		{
			foreach ( Mobile m in GetPlayers() )
				m.SendLocalizedMessage( num );
		}
		public void KickAll( PeerlessList Peerless )
		{
			ArrayList list = new ArrayList();

			foreach ( Mobile m in GetMobiles() )
			{
				list.Add( m );
			}

			for ( int i = 0; i < list.Count; i++ )
			{
				Mobile to = list[i] as Mobile;
				if ( to is PlayerMobile || ( (BaseCreature) to ).Controlled == true )
				{
					Effects.SendLocationParticles( EffectItem.Create( to.Location, to.Map, EffectItem.DefaultDuration ), 0x3728, 10, 10, 2023 );
					BaseCreature.TeleportPets( to, PeerlessEntry.GetExitPoint( Peerless ), Peerless == PeerlessList.Travesty ? Map.Tokuno : PeerlessEntry.GetMap( Peerless ) );
					to.MoveToWorld( PeerlessEntry.GetExitPoint( Peerless ), Peerless == PeerlessList.Travesty ? Map.Tokuno : PeerlessEntry.GetMap( Peerless ) );
				}

			}
		}
	}
}
