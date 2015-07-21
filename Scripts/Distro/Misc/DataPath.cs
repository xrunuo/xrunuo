using System;
using System.IO;
using Microsoft.Win32;
using Server;

namespace Server.Misc
{
	public class DataPath
	{
		/* The following is a list of files which a required for proper execution:
		 * 
		 * Multi.idx
		 * Multi.mul
		 * VerData.mul
		 * TileData.mul
		 * Map*.mul
		 * StaIdx*.mul
		 * Statics*.mul
		 * MapDif*.mul
		 * MapDifL*.mul
		 * StaDif*.mul
		 * StaDifL*.mul
		 * StaDifI*.mul
		 */

		public static void Configure()
		{
			string pathReg = GetExePath( "Ultima Online" );
			string pathTD = GetExePath( "Ultima Online Third Dawn" );

			if ( pathReg != null )
				Environment.DataDirectories.Add( pathReg );

			if ( pathTD != null )
				Environment.DataDirectories.Add( pathTD );

			if ( Environment.DataDirectories.Count == 0 )
			{
				Console.WriteLine( "Enter the Ultima Online directory:" );
				Console.Write( "> " );

				Environment.DataDirectories.Add( Console.ReadLine() );
			}
		}

		private static string GetExePath( string subName )
		{
			try
			{
				using ( RegistryKey key = Registry.LocalMachine.OpenSubKey( String.Format( @"SOFTWARE\Origin Worlds Online\{0}\1.0", subName ) ) )
				{
					if ( key == null )
						return null;

					string v = key.GetValue( "ExePath" ) as string;

					if ( v == null || v.Length <= 0 )
						return null;

					if ( !File.Exists( v ) )
						return null;

					v = Path.GetDirectoryName( v );

					if ( v == null )
						return null;

					return v;
				}
			}
			catch
			{
				return null;
			}
		}
	}
}