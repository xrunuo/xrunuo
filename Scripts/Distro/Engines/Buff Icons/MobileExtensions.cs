using System;
using System.Collections.Generic;
using System.Linq;
using Server;
using Server.Network;
using Server.Events;

namespace Server.Engines.BuffIcons
{
	public static class MobileExtensions
	{
		public static void Initialize()
		{
			EventSink.PlayerDeath += new PlayerDeathEventHandler( EventSink_PlayerDeath );
		}

		private static void EventSink_PlayerDeath( PlayerDeathEventArgs e )
		{
			Mobile m = e.Mobile;

			if ( m_BuffTables.ContainsKey( m ) )
			{
				var table = m_BuffTables[m];

				foreach ( BuffInfo buff in table.Values.ToArray() )
				{
					if ( !buff.RetainThroughDeath )
						m.RemoveBuff( buff );
				}
			}
		}

		private static Dictionary<Mobile, Dictionary<BuffIcon, BuffInfo>> m_BuffTables;

		static MobileExtensions()
		{
			m_BuffTables = new Dictionary<Mobile, Dictionary<BuffIcon, BuffInfo>>();
		}

		public static void ResendBuffs( this Mobile m )
		{
			if ( !BuffInfo.Enabled || !m_BuffTables.ContainsKey( m ) )
				return;

			NetState state = m.NetState;

			if ( state != null )
			{
				var table = m_BuffTables[m];

				foreach ( BuffInfo info in table.Values )
					state.Send( new AddBuffPacket( m, info ) );
			}
		}

		public static void AddBuff( this Mobile m, BuffInfo b )
		{
			if ( !BuffInfo.Enabled || b == null )
				return;

			m.RemoveBuff( b ); // Check & subsequently remove the old one.

			Dictionary<BuffIcon, BuffInfo> table;

			if ( m_BuffTables.ContainsKey( m ) )
				table = m_BuffTables[m];
			else
				table = m_BuffTables[m] = new Dictionary<BuffIcon, BuffInfo>();

			table[b.Id] = b;

			NetState state = m.NetState;

			if ( state != null )
				state.Send( new AddBuffPacket( m, b ) );
		}

		public static void RemoveBuff( this Mobile m, BuffInfo b )
		{
			if ( b == null )
				return;

			m.RemoveBuff( b.Id );
		}

		public static void RemoveBuff( this Mobile m, BuffIcon b )
		{
			if ( !m_BuffTables.ContainsKey( m ) )
				return;

			var table = m_BuffTables[m];

			if ( !table.ContainsKey( b ) )
				return;

			BuffInfo info = table[b];

			if ( info.Timer != null && info.Timer.Running )
				info.Timer.Stop();

			table.Remove( b );

			NetState state = m.NetState;

			if ( state != null )
				state.Send( new RemoveBuffPacket( m, b ) );

			if ( table.Count <= 0 )
				m_BuffTables.Remove( m );
		}
	}
}
