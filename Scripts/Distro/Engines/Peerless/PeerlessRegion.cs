using System;
using System.Collections;

using Server;
using Server.Items;
using Server.Mobiles;
using Server.Events;

namespace Server.Regions
{
	public class PeerlessRegion : Region
	{
		private AltarPeerless m_Altar;

		public static void Initialize()
		{
			EventSink.Logout += EventSink_Logout;
		}

		public PeerlessRegion( string name, Map map, AltarPeerless altar, Rectangle2D[] regionBounds )
			: base( null, map, Region.Find( altar.Location, altar.Map ), regionBounds )
		{
			m_Altar = altar;

			Register();
		}

		public override TimeSpan GetLogoutDelay( Mobile m )
		{
			return TimeSpan.FromMinutes( 10 );
		}

		private static void EventSink_Logout( LogoutEventArgs e )
		{
			Mobile from = e.Mobile;

			PeerlessRegion reg = from.Region.GetRegion<PeerlessRegion>();

			if ( reg != null )
			{
				BaseCreature.TeleportPets( from, PeerlessEntry.GetExitPoint( reg.m_Altar.Peerless ), reg.m_Altar.Peerless == PeerlessList.Travesty ? Map.Tokuno : PeerlessEntry.GetMap( reg.m_Altar.Peerless ) );
				from.MoveToWorld( PeerlessEntry.GetExitPoint( reg.m_Altar.Peerless ), reg.m_Altar.Peerless == PeerlessList.Travesty ? Map.Tokuno : PeerlessEntry.GetMap( reg.m_Altar.Peerless ) );
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

			if ( players <= 1 && m is PlayerMobile && m_Altar != null && m.AccessLevel == AccessLevel.Player )
			{
				if ( !m_Altar.Deleted && m_Altar.Activated && m_Altar.Boss.CheckAlive() )
				{
					m_Altar.PeerlessT.Stop();

					if ( m_Altar.Boss != null && !m_Altar.Boss.Deleted )
						m_Altar.Boss.Delete();

					m_Altar.DeleteKeys( PeerlessEntry.GetLabelNum( m_Altar.Peerless ) );
					m_Altar.Clear();
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
				if ( to is PlayerMobile || ( (BaseCreature)to ).Controlled == true )
				{
					Effects.SendLocationParticles( EffectItem.Create( to.Location, to.Map, EffectItem.DefaultDuration ), 0x3728, 10, 10, 2023 );
					BaseCreature.TeleportPets( to, PeerlessEntry.GetExitPoint( Peerless ), Peerless == PeerlessList.Travesty ? Map.Tokuno : PeerlessEntry.GetMap( Peerless ) );
					to.MoveToWorld( PeerlessEntry.GetExitPoint( Peerless ), Peerless == PeerlessList.Travesty ? Map.Tokuno : PeerlessEntry.GetMap( Peerless ) );
				}
			}
		}
	}
}
