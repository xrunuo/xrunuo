using System;
using System.Linq;
using System.IO;

namespace Server.Network
{
	public static class Extensions
	{
		public static void Trace( this PacketReader reader, NetState client )
		{
			try
			{
				using ( StreamWriter sw = new StreamWriter( Path.Combine( Environment.Config.LogDirectory, "Packets.log" ), true ) )
				{
					byte[] buffer = reader.Buffer;

					if ( buffer.Length > 0 )
						sw.WriteLine( "Client: {0}: Unhandled packet 0x{1:X2}", client, buffer[0] );

					using ( MemoryStream ms = new MemoryStream( buffer ) )
						Utility.FormatBuffer( sw, ms, buffer.Length );

					sw.WriteLine();
					sw.WriteLine();
				}
			}
			catch
			{
			}
		}
	}
}
