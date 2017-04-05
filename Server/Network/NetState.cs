using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

using Server;
using Server.Accounting;
using Server.Items;
using Server.Gumps;
using Server.Menus;
using Server.HuePickers;

namespace Server.Network
{
	public class NetState
	{
		private static readonly ILog log = LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

		private readonly IList<Gump> m_Gumps;
		private readonly ISet<HuePicker> m_HuePickers;
		private readonly ISet<IMenu> m_Menus;
		private readonly ISet<SecureTrade> m_Trades;

		internal int m_Seed;
		internal int m_AuthID;

		#region Traffic
		public long Incoming => UOSocket.Incoming;

		public long Outgoing => UOSocket.Outgoing;
		#endregion

		public UOSocket UOSocket { get; }

		public IPAddress Address => UOSocket.Address;

		public IPAddress ClientAddress { get; set; }

		public bool SentFirstPacket { get; set; }

		public bool BlockAllPackets { get; set; }

		public int Flags { get; set; }

		public ClientVersion Version { get; set; }

		public IEnumerable<SecureTrade> Trades => m_Trades;

		public void ValidateAllTrades()
		{
			foreach ( var trade in m_Trades.ToArray() )
			{
				var from = trade.From.Mobile;
				var to = trade.To.Mobile;

				if ( from.Deleted || to.Deleted || !from.Alive || !to.Alive || !from.InRange( to, 2 ) || from.Map != to.Map )
					trade.Cancel();
			}
		}

		public void CancelAllTrades()
		{
			foreach ( var trade in m_Trades.ToArray() )
			{
				trade.Cancel();
			}
		}

		public void RemoveTrade( SecureTrade trade )
		{
			m_Trades.Remove( trade );
		}

		public SecureTrade FindTrade( Mobile m )
		{
			return m_Trades.Where( t => t.From.Mobile == m || t.To.Mobile == m ).FirstOrDefault();
		}

		public SecureTradeContainer FindTradeContainer( Mobile m )
		{
			foreach ( var trade in m_Trades )
			{
				var from = trade.From;
				var to = trade.To;

				if ( from.Mobile == Mobile && to.Mobile == m )
					return from.Container;
				else if ( from.Mobile == m && to.Mobile == Mobile )
					return to.Container;
			}

			return null;
		}

		public SecureTradeContainer AddTrade( NetState state )
		{
			var newTrade = new SecureTrade( Mobile, state.Mobile );

			m_Trades.Add( newTrade );
			state.m_Trades.Add( newTrade );

			return newTrade.From.Container;
		}

		public bool CompressionEnabled { get; set; }

		public int Sequence { get; set; }

		public IEnumerable<Gump> Gumps => m_Gumps;

		public IEnumerable<HuePicker> HuePickers => m_HuePickers;

		public IEnumerable<IMenu> Menus => m_Menus;

		public static int GumpCap { get; set; } = 512;

		public static int HuePickerCap { get; set; } = 512;

		public static int MenuCap { get; set; } = 512;

		public void AddMenu( IMenu menu )
		{
			if ( m_Menus == null )
				return;

			if ( m_Menus.Count >= MenuCap )
			{
				log.Warning( "Client: {0}: Exceeded menu cap, disconnecting...", this );
				Dispose();
			}
			else
			{
				m_Menus.Add( menu );
			}
		}

		public void RemoveMenu( IMenu menu )
		{
			if ( m_Menus == null )
				return;

			m_Menus.Remove( menu );
		}

		public void AddHuePicker( HuePicker huePicker )
		{
			if ( m_HuePickers == null )
				return;

			if ( m_HuePickers.Count >= HuePickerCap )
			{
				log.Warning( "Client: {0}: Exceeded hue picker cap, disconnecting...", this );
				Dispose();
			}
			else
			{
				m_HuePickers.Add( huePicker );
			}
		}

		public void RemoveHuePicker( HuePicker huePicker )
		{
			if ( m_HuePickers == null )
				return;

			m_HuePickers.Remove( huePicker );
		}

		public void AddGump( Gump g )
		{
			if ( m_Gumps == null )
				return;

			if ( m_Gumps.Count >= GumpCap )
			{
				log.Warning( "Client: {0}: Exceeded gump cap, disconnecting...", this );
				Dispose();
			}
			else
			{
				m_Gumps.Add( g );
			}
		}

		public void RemoveGump( Gump gump )
		{
			if ( m_Gumps != null )
				m_Gumps.Remove( gump );
		}

		public void ClearGumps()
		{
			if ( m_Gumps != null )
				m_Gumps.Clear();
		}

		public CityInfo[] CityInfo { get; set; }

		public Mobile Mobile { get; set; }

		public ServerInfo[] ServerInfo { get; set; }

		public IAccount Account { get; set; }

		public bool Running { get; private set; }

		public override string ToString()
		{
			return UOSocket.ToString();
		}

		public NetState( UOSocket uoSocket )
		{
			UOSocket = uoSocket;

			Seeded = false;
			m_Gumps = new List<Gump>();
			m_HuePickers = new HashSet<HuePicker>();
			m_Menus = new HashSet<IMenu>();
			m_Trades = new HashSet<SecureTrade>();

			// Ensure client version is NEVER null.
			Version = new ClientVersion( 0, 0, 0, 0, ClientType.Classic );

			Running = true;
		}

		public void Send( Packet p )
		{
			if ( BlockAllPackets )
			{
				p.OnSend();
				return;
			}

			var prof = PacketProfile.GetOutgoingProfile( (byte) p.PacketID );
			var start = ( prof == null ? DateTime.MinValue : DateTime.UtcNow );

			int length;
			var buffer = p.Compile( CompressionEnabled, out length );

			if ( buffer != null && buffer.Length > 0 && length > 0 )
			{
				UOSocket.Send( buffer, length );
			}

			if ( prof != null )
				prof.Record( length, DateTime.UtcNow - start );

			p.OnSend();
		}

		public void LaunchBrowser( string url )
		{
			Send( new MessageLocalized( Serial.MinusOne, -1, MessageType.Label, 0x35, 3, 501231, "", "" ) );
			Send( new LaunchBrowser( url ) );
		}

		public void Flush()
		{
			UOSocket.Flush();
		}

		public void Dispose()
		{
			Running = false;

			UOSocket.Dispose();
		}

		public void Clear()
		{
			Mobile = null;
			Account = null;
			ServerInfo = null;
			CityInfo = null;

			m_Gumps.Clear();
			m_Menus.Clear();
			m_HuePickers.Clear();
		}

		public bool Seeded { get; set; }

		public const int MaxNullTargets = 5;

		public int NullTargets { get; set; }
	}
}
