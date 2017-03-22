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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

using Server;
using Server.Accounting;
using Server.Network;
using Server.Items;
using Server.Gumps;
using Server.Menus;
using Server.HuePickers;

namespace Server.Network
{
	public class NetState
	{
		private UOSocket m_UOSocket;
		private IPAddress m_ClientAddress;
		private bool m_Seeded;
		private ServerInfo[] m_ServerInfo;
		private IAccount m_Account;
		private Mobile m_Mobile;
		private CityInfo[] m_CityInfo;
		private IList<Gump> m_Gumps;
		private ISet<HuePicker> m_HuePickers;
		private ISet<IMenu> m_Menus;
		private ISet<SecureTrade> m_Trades;
		private int m_Sequence;
		private bool m_CompressionEnabled;
		private ClientVersion m_Version;
		private bool m_SentFirstPacket;
		private bool m_BlockAllPackets;
		private bool m_Running;

		internal int m_Seed;
		internal int m_AuthID;

		#region Traffic
		public long Incoming
		{
			get { return m_UOSocket.Incoming; }
		}

		public long Outgoing
		{
			get { return m_UOSocket.Outgoing; }
		}
		#endregion

		public UOSocket UOSocket
		{
			get { return m_UOSocket; }
		}

		public IPAddress Address
		{
			get { return m_UOSocket.Address; }
		}

		public IPAddress ClientAddress
		{
			get { return m_ClientAddress; }
			set { m_ClientAddress = value; }
		}

		private int m_Flags;

		public bool SentFirstPacket
		{
			get { return m_SentFirstPacket; }
			set { m_SentFirstPacket = value; }
		}

		public bool BlockAllPackets
		{
			get { return m_BlockAllPackets; }
			set { m_BlockAllPackets = value; }
		}

		public int Flags
		{
			get { return m_Flags; }
			set { m_Flags = value; }
		}

		public ClientVersion Version
		{
			get { return m_Version; }
			set { m_Version = value; }
		}

		public IEnumerable<SecureTrade> Trades
		{
			get { return m_Trades; }
		}

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
				SecureTradeInfo from = trade.From;
				SecureTradeInfo to = trade.To;

				if ( from.Mobile == m_Mobile && to.Mobile == m )
					return from.Container;
				else if ( from.Mobile == m && to.Mobile == m_Mobile )
					return to.Container;
			}

			return null;
		}

		public SecureTradeContainer AddTrade( NetState state )
		{
			SecureTrade newTrade = new SecureTrade( m_Mobile, state.m_Mobile );

			m_Trades.Add( newTrade );
			state.m_Trades.Add( newTrade );

			return newTrade.From.Container;
		}

		public bool CompressionEnabled
		{
			get { return m_CompressionEnabled; }
			set { m_CompressionEnabled = value; }
		}

		public int Sequence
		{
			get { return m_Sequence; }
			set { m_Sequence = value; }
		}

		public IEnumerable<Gump> Gumps
		{
			get { return m_Gumps; }
		}

		public IEnumerable<HuePicker> HuePickers
		{
			get { return m_HuePickers; }
		}

		public IEnumerable<IMenu> Menus
		{
			get { return m_Menus; }
		}

		private static int m_GumpCap = 512, m_HuePickerCap = 512, m_MenuCap = 512;

		public static int GumpCap
		{
			get { return m_GumpCap; }
			set { m_GumpCap = value; }
		}

		public static int HuePickerCap
		{
			get { return m_HuePickerCap; }
			set { m_HuePickerCap = value; }
		}

		public static int MenuCap
		{
			get { return m_MenuCap; }
			set { m_MenuCap = value; }
		}

		public void AddMenu( IMenu menu )
		{
			if ( m_Menus == null )
				return;

			if ( m_Menus.Count >= m_MenuCap )
			{
				Console.WriteLine( "Client: {0}: Exceeded menu cap, disconnecting...", this );
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

			if ( m_HuePickers.Count >= m_HuePickerCap )
			{
				Console.WriteLine( "Client: {0}: Exceeded hue picker cap, disconnecting...", this );
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

			if ( m_Gumps.Count >= m_GumpCap )
			{
				Console.WriteLine( "Client: {0}: Exceeded gump cap, disconnecting...", this );
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

		public CityInfo[] CityInfo
		{
			get { return m_CityInfo; }
			set { m_CityInfo = value; }
		}

		public Mobile Mobile
		{
			get { return m_Mobile; }
			set { m_Mobile = value; }
		}

		public ServerInfo[] ServerInfo
		{
			get { return m_ServerInfo; }
			set { m_ServerInfo = value; }
		}

		public IAccount Account
		{
			get { return m_Account; }
			set { m_Account = value; }
		}

		public bool Running
		{
			get { return m_Running; }
		}

		public override string ToString()
		{
			return m_UOSocket.ToString();
		}

		public NetState( UOSocket uoSocket )
		{
			m_UOSocket = uoSocket;

			m_Seeded = false;
			m_Gumps = new List<Gump>();
			m_HuePickers = new HashSet<HuePicker>();
			m_Menus = new HashSet<IMenu>();
			m_Trades = new HashSet<SecureTrade>();

			// Ensure client version is NEVER null.
			m_Version = new ClientVersion( 0, 0, 0, 0, ClientType.Classic );

			m_Running = true;
		}

		public void Send( Packet p )
		{
			if ( m_BlockAllPackets )
			{
				p.OnSend();
				return;
			}

			PacketProfile prof = PacketProfile.GetOutgoingProfile( (byte) p.PacketID );
			DateTime start = ( prof == null ? DateTime.MinValue : DateTime.UtcNow );

			int length;
			byte[] buffer = p.Compile( m_CompressionEnabled, out length );

			if ( buffer != null && buffer.Length > 0 && length > 0 )
			{
				m_UOSocket.Send( buffer, length );
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
			m_UOSocket.Flush();
		}

		public void Dispose()
		{
			m_Running = false;

			m_UOSocket.Dispose();
		}

		public void Clear()
		{
			m_Mobile = null;
			m_Account = null;
			m_ServerInfo = null;
			m_CityInfo = null;

			m_Gumps.Clear();
			m_Menus.Clear();
			m_HuePickers.Clear();
		}

		public bool Seeded
		{
			get { return m_Seeded; }
			set { m_Seeded = value; }
		}

		public const int MaxNullTargets = 5;

		private int m_NullTargets;

		public int NullTargets
		{
			get { return m_NullTargets; }
			set { m_NullTargets = value; }
		}
	}
}
