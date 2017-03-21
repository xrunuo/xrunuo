using System;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using Server.Accounting;
using Server;
using Server.Network;
using Server.Misc;
using System.IO;

namespace Scripts.Engines.Encryption
{
	// This class handles OSI client encryption for clients newer than 2.0.3. (not including 2.0.3)
	public class Encryption : IPacketEncoder
	{
		// Encryption state information
		private uint m_Seed;
		private bool m_Seeded;
		private ByteQueue m_Buffer;
		private IClientEncryption m_Encryption;
		private bool m_AlreadyRelayed;

		public Encryption()
		{
			m_AlreadyRelayed = false;
			m_Encryption = null;
			m_Buffer = new ByteQueue();
			m_Seeded = false;
			m_Seed = 0;
		}

		static public void Initialize()
		{
			// Only initialize our subsystem if we're enabled
			if ( Configuration.Enabled )
			{
				// Initialize static members and connect to the creation callback of a NetState.
				UOSocket.CreatedCallback = new NetStateCreatedCallback( NetStateCreated );

				// Overwrite the packet handler for the relay packet since we need to change the
				// encryption mode then.
				PacketHandlers.Instance.Register( 0xA0, 3, false, new OnPacketReceive( HookedPlayServer ) );
			}
		}

		public static void NetStateCreated( UOSocket state )
		{
			state.PacketEncoder = new Encryption();
		}

		public static void HookedPlayServer( NetState client, PacketReader pvSrc )
		{
			// Call the original handler
			PacketHandlers.Instance.PlayServer( client, pvSrc );

			// Now indicate, that the state has been relayed already. If it's used again,
			// it means we're entering a special encryption state
			Encryption context = (Encryption) ( client.UOSocket.PacketEncoder );
			context.m_AlreadyRelayed = true;
		}

		// Try to encrypt outgoing data.
		public void EncodeOutgoingPacket( UOSocket to, ref byte[] buffer, ref int length )
		{
			if ( m_Encryption != null )
			{
				m_Encryption.serverEncrypt( ref buffer, length );
				return;
			}
		}

		public void RejectNoEncryption( UOSocket ns )
		{
			// Log it on the console
			Console.WriteLine( "Client: {0}: Unencrypted client detected, disconnected", ns );

			// Send the client the typical "Bad communication" packet and also a sysmessage stating the error
			//ns.Send( new AsciiMessage( Server.Serial.MinusOne, -1, MessageType.Label, 0x35, 3, "System", "Unencrypted connections are not allowed on this server." ) );
			//ns.Send( new AccountLoginRej( ALRReason.BadComm ) );

			// Disconnect the client
			ns.Dispose( true );
		}

		// Try to decrypt incoming data.
		public void DecodeIncomingPacket( UOSocket from, ref byte[] buffer, ref int length )
		{
			if ( m_Encryption != null )
			{
				// If we're decrypting using LoginCrypt and we've already been relayed,
				// only decrypt a single packet using logincrypt and then disable it
				if ( m_AlreadyRelayed && m_Encryption is LoginEncryption )
				{
					uint newSeed = ( ( ( (LoginEncryption) m_Encryption ).Key1 + 1 ) ^ ( (LoginEncryption) m_Encryption ).Key2 );

					// Swap the seed
					newSeed = ( ( newSeed >> 24 ) & 0xFF ) | ( ( newSeed >> 8 ) & 0xFF00 ) | ( ( newSeed << 8 ) & 0xFF0000 ) | ( ( newSeed << 24 ) & 0xFF000000 );

					// XOR it with the old seed
					newSeed ^= m_Seed;

					IClientEncryption newEncryption = new GameEncryption( newSeed );

					// Game Encryption comes first
					newEncryption.clientDecrypt( ref buffer, length );

					// The login encryption is still used for this one packet
					m_Encryption.clientDecrypt( ref buffer, length );

					// Swap the encryption schemes
					m_Encryption = newEncryption;
					m_Seed = newSeed;

					return;
				}

				m_Encryption.clientDecrypt( ref buffer, length );
				return;
			}

			// If the client did not connect on the game server port,
			// it's not our business to handle encryption for it
			if ( ( (IPEndPoint) from.Socket.LocalEndPoint ).Port != Listener.Port )
			{
				m_Encryption = new NoEncryption();
				return;
			}

			// For simplicities sake, enqueue what we just received as long as we're not initialized
			m_Buffer.Enqueue( buffer, 0, length );
			// Clear the array
			length = 0;

			// If we didn't receive the seed yet, queue data until we can read the seed
			if ( !m_Seeded )
			{
				int seed_length = m_Buffer.GetSeedLength();

				// Now check if we have at least 4 bytes to get the seed
				if ( m_Buffer.Length >= seed_length )
				{
					byte[] m_Peek = new byte[m_Buffer.Length];
					m_Buffer.Dequeue( m_Peek, 0, seed_length ); // Dequeue the seed

					if ( seed_length == 4 )
						m_Seed = (uint) ( ( m_Peek[0] << 24 ) | ( m_Peek[1] << 16 ) | ( m_Peek[2] << 8 ) | m_Peek[3] );
					else if ( seed_length == 21 )
						m_Seed = (uint) ( ( m_Peek[1] << 24 ) | ( m_Peek[2] << 16 ) | ( m_Peek[3] << 8 ) | m_Peek[4] );

					m_Seeded = true;

					Buffer.BlockCopy( m_Peek, 0, buffer, 0, seed_length );

					length = seed_length;
				}
				else
				{
					return;
				}
			}

			// If the context isn't initialized yet, that means we haven't decided on an encryption method yet
			if ( m_Encryption == null )
			{
				int packetLength = m_Buffer.Length;
				int packetOffset = length;
				m_Buffer.Dequeue( buffer, length, packetLength ); // Dequeue everything
				length += packetLength;

				// This is special handling for the "special" UOG packet
				if ( packetLength >= 3 )
				{
					if ( buffer[packetOffset] == 0xf1 && buffer[packetOffset + 1] == ( ( packetLength >> 8 ) & 0xFF ) && buffer[packetOffset + 2] == ( packetLength & 0xFF ) )
					{
						m_Encryption = new NoEncryption();
						return;
					}
				}

				// Check if the current buffer contains a valid login packet (62 byte + 4 byte header)
				// Please note that the client sends these in two chunks. One 4 byte and one 62 byte.
				if ( packetLength == 62 )
				{
					// Check certain indices in the array to see if the given data is unencrypted
					if ( buffer[packetOffset] == 0x80 && buffer[packetOffset + 30] == 0x00 && buffer[packetOffset + 60] == 0x00 )
					{
						if ( Configuration.AllowUnencryptedClients )
						{
							m_Encryption = new NoEncryption();
						}
						else
						{
							RejectNoEncryption( from );
							from.Dispose();
							return;
						}
					}
					else
					{
						LoginEncryption encryption = new LoginEncryption();
						if ( encryption.init( m_Seed, buffer, packetOffset, packetLength ) )
						{
							Console.WriteLine( "Client: {0}: Encrypted client detected, using keys of client {1}", from, encryption.Name );
							m_Encryption = encryption;

							byte[] packet = new byte[packetLength];
							Buffer.BlockCopy( buffer, packetOffset, packet, 0, packetLength );
							encryption.clientDecrypt( ref packet, packet.Length );
							Buffer.BlockCopy( packet, 0, buffer, packetOffset, packetLength );
						}
						else
						{
							Console.WriteLine( "Detected an unknown client." );
						}
					}
				}
				else if ( packetLength == 65 )
				{
					// If its unencrypted, use the NoEncryption class
					if ( buffer[packetOffset] == '\x91' && buffer[packetOffset + 1] == ( ( m_Seed >> 24 ) & 0xFF ) && buffer[packetOffset + 2] == ( ( m_Seed >> 16 ) & 0xFF ) && buffer[packetOffset + 3] == ( ( m_Seed >> 8 ) & 0xFF ) && buffer[packetOffset + 4] == ( m_Seed & 0xFF ) )
					{
						if ( Configuration.AllowUnencryptedClients )
						{
							m_Encryption = new NoEncryption();
						}
						else
						{
							RejectNoEncryption( from );
							from.Dispose();
							return;
						}
					}
					else
					{
						// If it's not an unencrypted packet, simply assume it's encrypted with the seed
						m_Encryption = new GameEncryption( m_Seed );

						byte[] packet = new byte[packetLength];
						Buffer.BlockCopy( buffer, packetOffset, packet, 0, packetLength );
						m_Encryption.clientDecrypt( ref packet, packet.Length );
						Buffer.BlockCopy( packet, 0, buffer, packetOffset, packetLength );
					}
				}
				// Since we have not reversed UO:KR client encryption yet, we'll have to connect only with unencrypted clients
				else if ( packetLength == 23 )
				{
					m_Encryption = new NoEncryption();
					return;
				}

				// If it's still not initialized, copy the data back to the queue and wait for more
				if ( m_Encryption == null )
				{
					m_Buffer.Enqueue( buffer, packetOffset, packetLength );
					length -= packetLength;
					return;
				}
			}
		}
	}
}
