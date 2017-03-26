using System;
using System.IO;
using System.IO.Compression;
using System.Net;

namespace Server
{
	public class Dependencies
	{
		private static readonly ILog log = LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

		public static string Fetch( Configuration.Library library )
		{
			string targetPath;

			log.Info( "Fetching library " + library.Name + " from " + library.Uri );

			using ( var client = new WebClient() )
			{
				var zipPath = Path.Combine( Core.BaseDirectory, $"deps/{library.Name}.zip" );

				if ( !File.Exists( zipPath ) )
				{
					client.DownloadFile( library.Uri, zipPath );
				}

				targetPath = $"Scripts/{library.Name}";

				if ( Directory.Exists( targetPath ) )
					Directory.Delete( targetPath, recursive: true );

				ZipFile.ExtractToDirectory( zipPath, targetPath );
			}

			return targetPath;
		}
	}
}
