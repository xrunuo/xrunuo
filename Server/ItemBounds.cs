using System;
using System.IO;

namespace Server
{
	public class ItemBounds
	{
		private static readonly ILog log = LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

		public static Rectangle2D[] Table { get; }

		private static readonly string BoundsFilename = "Data/Binary/Bounds.bin";
		private static readonly int BoundsSize = 0x10000;

		static ItemBounds()
		{
			Table = new Rectangle2D[BoundsSize];

			if ( File.Exists( BoundsFilename ) )
			{
				using ( var fs = new FileStream( BoundsFilename, FileMode.Open, FileAccess.Read, FileShare.Read ) )
				{
					var bin = new BinaryReader( fs );

					for ( var i = 0; i < BoundsSize; ++i )
					{
						int xMin = bin.ReadInt16();
						int yMin = bin.ReadInt16();
						int xMax = bin.ReadInt16();
						int yMax = bin.ReadInt16();

						Table[i].Set( xMin, yMin, ( xMax - xMin ) + 1, ( yMax - yMin ) + 1 );
					}

					bin.Close();
				}
			}
			else
			{
				log.Warning( "{0} does not exist", BoundsFilename );
			}
		}
	}
}
