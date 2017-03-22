﻿using System;
using System.IO;

namespace Server
{
	public class Logger
	{
		public static void Error( string format, params object[] args )
		{
			string log = String.Format( format, args );

			try
			{
				using ( StreamWriter op = new StreamWriter( Path.Combine( Environment.Config.LogDirectory, "Exceptions.log" ), true ) )
				{
					op.WriteLine( "{0}, {1}", DateTime.UtcNow, log );
					op.WriteLine();
				}
			}
			catch
			{
			}

			Console.WriteLine( log );
		}

		public static void Debug( string format, params object[] args )
		{
			string log = String.Format( format, args );

			try
			{
				using ( StreamWriter op = new StreamWriter( Path.Combine( Environment.Config.LogDirectory, "Debug.log" ), true ) )
				{
					op.WriteLine( "{0}, {1}", DateTime.UtcNow, log );
				}
			}
			catch
			{
			}

			Console.WriteLine( log );
		}
	}
}
