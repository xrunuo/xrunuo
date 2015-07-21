using System;
using System.IO;
using Server;
using System.Collections.Generic;
using Server.Accounting;

namespace Server.Engines.Chat
{
	public class ChatLogging
	{
		public static readonly bool Enabled = true;

		private static StreamWriter m_Output;
		//private static Dictionary<string, StreamWriter> m_OutputPerChannel;

		public static void Initialize()
		{
			if ( !Directory.Exists( "log" ) )
				Directory.CreateDirectory( "log" );

			string directory = "log/chat";

			if ( !Directory.Exists( directory ) )
				Directory.CreateDirectory( directory );

			//m_OutputPerChannel = new Dictionary<string, StreamWriter>();

			try
			{
				m_Output = new StreamWriter( Path.Combine( directory, String.Format( "{0}.log", DateTime.Now.ToLongDateString() ) ), true );

				m_Output.AutoFlush = true;

				m_Output.WriteLine( "##############################" );
				m_Output.WriteLine( "Log started on {0}", DateTime.Now );
				m_Output.WriteLine();
			}
			catch
			{
			}
		}

		public static void WriteLine( string channel, string format, params object[] args )
		{
			WriteLine( channel, String.Format( format, args ) );
		}

		public static void WriteLine( string channel, string text )
		{
			if ( !Enabled )
				return;

			try
			{
				m_Output.WriteLine( "{0}: [{1}] {2}", DateTime.Now, channel, text );

				//StreamWriter channelOutput;

				//if ( m_OutputPerChannel.ContainsKey( channel ) )
				//    channelOutput = m_OutputPerChannel[channel];
				//else
				//{
				//    string path = Environment.BaseDirectory;

				//    AppendPath( ref path, "log" );
				//    AppendPath( ref path, "chat" );
				//    AppendPath( ref path, "channels" );
				//    path = Path.Combine( path, String.Format( "{0}.log", channel ) );

				//    channelOutput = new StreamWriter( path, true );
				//    channelOutput.AutoFlush = true;

				//    m_OutputPerChannel[channel] = channelOutput;
				//}

				//channelOutput.WriteLine( "{0}: {1}", DateTime.Now, text );
			}
			catch
			{
			}
		}

		public static void AppendPath( ref string path, string toAppend )
		{
			path = Path.Combine( path, toAppend );

			if ( !Directory.Exists( path ) )
				Directory.CreateDirectory( path );
		}

		public static void LogMessage( string channel, string username, string message )
		{
			WriteLine( channel, "{0} says: {1}", username, message );
		}

		public static void LogCreateChannel( string channel )
		{
			WriteLine( channel, "************** Channel was created." );
		}

		public static void LogRemoveChannel( string channel )
		{
			WriteLine( channel, "************** Channel was removed." );
		}

		public static void LogJoin( string channel, string username )
		{
			WriteLine( channel, "{0} joined the channel.", username );
		}

		public static void LogLeave( string channel, string username )
		{
			WriteLine( channel, "{0} left the channel.", username );

			//if ( m_OutputPerChannel.ContainsKey( channel ) )
			//	m_OutputPerChannel[channel].Dispose();
		}

		public static void Log( string channel, string message )
		{
			WriteLine( channel, message );
		}
	}
}