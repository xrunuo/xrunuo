using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.Network
{
	public class GameServer
	{
		private static readonly ILog log = LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

		public static GameServer Instance { get; set; }

		private readonly NetServer m_Server;

		public GameServer( NetServer server )
		{
			m_Server = server;
		}

		public void Initialize()
		{
			m_Server.Connected += OnConnected;
			m_Server.Disconnected += OnDisconnected;
			m_Server.Received += OnReceived;
		}

		private void OnConnected( UOSocket state )
		{
			m_Clients.Add( state, new NetState( state ) );

			log.Info( "Client: {0}: Connected. [{1} Online]", state, m_Clients.Count );
		}

		private void OnDisconnected( UOSocket state )
		{
			NetState client;

			if ( !m_Clients.TryGetValue( state, out client ) )
			{
				var trace = new System.Diagnostics.StackTrace();
				log.Error( "Inconsistent game server state: game client for {0} not found: {1}", state, trace );

				return;
			}

			m_Clients.Remove( state );

			var mob = client.Mobile;

			if ( mob != null )
				mob.NetState = null;

			var account = client.Account;

			if ( account == null )
				log.Info( "Client: {0}: Disconnected. [{1} Online]", client, m_Clients.Count );
			else
				log.Info( "Client: {0}: Disconnected. [{1} Online] [{2}]", client, m_Clients.Count, account );

			client.Clear();
		}

		private const int BufferSize = 4096;
		private readonly BufferPool m_Buffers = new BufferPool( "Processor", 4, BufferSize );

		private void OnReceived( UOSocket state, ByteQueue buffer, out bool throttle )
		{
			throttle = false;
			NetState client;

			if ( !m_Clients.TryGetValue( state, out client ) )
			{
				log.Error( "Inconsistent game server state: game client for {0} not found", state );
				return;
			}

			if ( !client.Seeded )
			{
				if ( buffer.GetPacketID() == 0xEF )
				{
					client.Seeded = true;
				}
				else if ( buffer.Length >= 4 )
				{
					var peek = new byte[4];
					buffer.Dequeue( peek, 0, 4 );

					var seed = ( peek[0] << 24 ) | ( peek[1] << 16 ) | ( peek[2] << 8 ) | peek[3];

					if ( seed == 0 )
					{
						log.Info( "Login: {0}: Invalid client detected, disconnecting", client );
						client.Dispose();

						return;
					}

					client.m_Seed = seed;
					client.Seeded = true;
				}
				else
				{
					return; // Need at least 4 bytes for the seed
				}
			}

			var length = buffer.Length;

			while ( length > 0 && buffer.Length > 0 )
			{
				int packetID = buffer.GetPacketID();

				if ( !client.SentFirstPacket && packetID != 0xF1 && packetID != 0xCF && packetID != 0x80 && packetID != 0x91 && packetID != 0xA4 && packetID != 0xEF )
				{
					log.Info( "Client: {0}: Encrypted client detected, disconnecting", client );
					client.Dispose();
					return;
				}

				var handler = PacketHandlers.GetHandler( packetID );

				if ( handler == null )
				{
					var data = new byte[length];
					length = buffer.Dequeue( data, 0, length );

					if ( Core.Logging )
					{
						var reader = PacketReader.CreateInstance( data, length, false );
						reader.Trace( client );
						PacketReader.ReleaseInstance( reader );
					}

					return;
				}

				var packetLength = handler.Length;

				if ( packetLength == 0 )
				{
					// Dynamic length packet. Need at leaset 3 bytes (1 packet cmd + 2 length)

					if ( length >= 3 )
					{
						packetLength = buffer.GetPacketLength();

						if ( packetLength < 3 )
						{
							client.Dispose();
							return;
						}
					}
					else
					{
						break;
					}
				}

				if ( length >= packetLength )
				{
					if ( handler.Ingame && client.Mobile == null )
					{
						log.Info( "Client: {0}: Sent ingame packet (0x{1:X2}) before having been attached to a mobile", client, packetID );
						client.Dispose();
						return;
					}
					else if ( handler.Ingame && client.Mobile.Deleted )
					{
						client.Dispose();
						return;
					}
					else
					{
						var throttler = handler.ThrottleCallback;

						if ( throttler != null && !throttler( client ) )
						{
							throttle = true;
							return;
						}

						var profile = PacketProfile.GetIncomingProfile( packetID );
						var start = ( profile == null ? DateTime.MinValue : DateTime.UtcNow );

						byte[] packetBuffer;

						if ( BufferSize >= packetLength )
							packetBuffer = m_Buffers.AcquireBuffer();
						else
							packetBuffer = new byte[packetLength];

						packetLength = buffer.Dequeue( packetBuffer, 0, packetLength );

						var reader = PacketReader.CreateInstance( packetBuffer, packetLength, handler.Length != 0 );

						try
						{
							handler.OnReceive( client, reader );
						}
						catch ( Exception e )
						{
							log.Error( "Exception disarmed in HandleReceive from {0}: {1}", client.Address, e );
						}

						PacketReader.ReleaseInstance( reader );

						length = buffer.Length;

						if ( BufferSize >= packetLength )
							m_Buffers.ReleaseBuffer( packetBuffer );

						if ( profile != null )
							profile.Record( packetLength, DateTime.UtcNow - start );
					}
				}
				else
				{
					break;
				}
			}
		}

		private readonly Dictionary<UOSocket, NetState> m_Clients = new Dictionary<UOSocket, NetState>();

		public IEnumerable<NetState> Clients => m_Clients.Values;

		public int ClientCount => m_Clients.Count;

		public IEnumerable<Mobile> OnlinePlayers
		{
			get { return m_Clients.Values.Select( c => c.Mobile ).Where( m => m != null ); }
		}

		public void ProcessDisposedQueue()
		{
			m_Server.ProcessDisposedQueue();
		}

		public long Incoming => m_Server.Incoming;

		public long Outgoing => m_Server.Outgoing;
	}
}
