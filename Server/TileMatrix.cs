using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Runtime.InteropServices;

namespace Server
{
	public class TileMatrix
	{
		private static readonly ILog log = LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

		private readonly Tile[][][][][] m_StaticTiles;
		private readonly Tile[][][] m_LandTiles;

		private readonly Tile[] m_InvalidLandBlock;

		private readonly UopIndex m_MapIndex;

		private readonly int m_FileIndex;

		private readonly int[][] m_StaticPatches;
		private readonly int[][] m_LandPatches;

		public Map Owner { get; }

		public int BlockWidth { get; }

		public int BlockHeight { get; }

		public int Width { get; }

		public int Height { get; }

		public FileStream MapStream { get; set; }

		public bool MapUOPPacked => ( m_MapIndex != null );

		public FileStream IndexStream { get; set; }

		public FileStream DataStream { get; set; }

		public BinaryReader IndexReader { get; set; }

		public bool Exists => ( MapStream != null && IndexStream != null && DataStream != null );

		private static readonly List<TileMatrix> m_Instances = new List<TileMatrix>();
		private readonly List<TileMatrix> m_FileShare;

		public TileMatrix( Map owner, int fileIndex, int mapID, int width, int height )
		{
			m_FileShare = new List<TileMatrix>();

			for ( int i = 0; i < m_Instances.Count; ++i )
			{
				TileMatrix tm = m_Instances[i];

				if ( tm.m_FileIndex == fileIndex )
				{
					tm.m_FileShare.Add( this );
					m_FileShare.Add( tm );
				}
			}

			m_Instances.Add( this );
			m_FileIndex = fileIndex;
			Width = width;
			Height = height;
			BlockWidth = width >> 3;
			BlockHeight = height >> 3;

			Owner = owner;

			if ( fileIndex != 0x7F )
			{
				string mapPath = Core.FindDataFile( "map{0}.mul", fileIndex );

				if ( File.Exists( mapPath ) )
				{
					MapStream = new FileStream( mapPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite );
				}
				else
				{
					mapPath = Core.FindDataFile( "map{0}LegacyMUL.uop", fileIndex );

					if ( File.Exists( mapPath ) )
					{
						MapStream = new FileStream( mapPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite );
						m_MapIndex = new UopIndex( MapStream );
					}
				}

				string indexPath = Core.FindDataFile( "staidx{0}.mul", fileIndex );

				if ( File.Exists( indexPath ) )
				{
					IndexStream = new FileStream( indexPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite );
					IndexReader = new BinaryReader( IndexStream );
				}

				string staticsPath = Core.FindDataFile( "statics{0}.mul", fileIndex );

				if ( File.Exists( staticsPath ) )
					DataStream = new FileStream( staticsPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite );
			}

			EmptyStaticBlock = new Tile[8][][];

			for ( int i = 0; i < 8; ++i )
			{
				EmptyStaticBlock[i] = new Tile[8][];

				for ( int j = 0; j < 8; ++j )
					EmptyStaticBlock[i][j] = new Tile[0];
			}

			m_InvalidLandBlock = new Tile[196];

			m_LandTiles = new Tile[BlockWidth][][];
			m_StaticTiles = new Tile[BlockWidth][][][][];
			m_StaticPatches = new int[BlockWidth][];
			m_LandPatches = new int[BlockWidth][];
		}

		public Tile[][][] EmptyStaticBlock { get; }

		public void SetStaticBlock( int x, int y, Tile[][][] value )
		{
			if ( x < 0 || y < 0 || x >= BlockWidth || y >= BlockHeight )
				return;

			if ( m_StaticTiles[x] == null )
				m_StaticTiles[x] = new Tile[BlockHeight][][][];

			m_StaticTiles[x][y] = value;

			if ( m_StaticPatches[x] == null )
				m_StaticPatches[x] = new int[( BlockHeight + 31 ) >> 5];

			m_StaticPatches[x][y >> 5] |= 1 << ( y & 0x1F );
		}

		public Tile[][][] GetStaticBlock( int x, int y )
		{
			if ( x < 0 || y < 0 || x >= BlockWidth || y >= BlockHeight || DataStream == null || IndexStream == null )
				return EmptyStaticBlock;

			if ( m_StaticTiles[x] == null )
				m_StaticTiles[x] = new Tile[BlockHeight][][][];

			Tile[][][] tiles = m_StaticTiles[x][y];

			if ( tiles == null )
			{
				for ( int i = 0; tiles == null && i < m_FileShare.Count; ++i )
				{
					TileMatrix shared = m_FileShare[i];

					if ( x >= 0 && x < shared.BlockWidth && y >= 0 && y < shared.BlockHeight )
					{
						Tile[][][][] theirTiles = shared.m_StaticTiles[x];

						if ( theirTiles != null )
							tiles = theirTiles[y];

						if ( tiles != null )
						{
							int[] theirBits = shared.m_StaticPatches[x];

							if ( theirBits != null && ( theirBits[y >> 5] & ( 1 << ( y & 0x1F ) ) ) != 0 )
								tiles = null;
						}
					}
				}

				if ( tiles == null )
					tiles = ReadStaticBlock( x, y );

				m_StaticTiles[x][y] = tiles;
			}

			return tiles;
		}

		public Tile[] GetStaticTiles( int x, int y )
		{
			Tile[][][] tiles = GetStaticBlock( x >> 3, y >> 3 );

			return Season.PatchTiles( tiles[x & 0x7][y & 0x7], Owner.Season );
		}

		private static readonly TileList m_TilesList = new TileList();

		public Tile[] GetStaticTiles( int x, int y, bool multis )
		{
			if ( !multis )
				return GetStaticTiles( x, y );

			Tile[][][] tiles = GetStaticBlock( x >> 3, y >> 3 );

			var eable = Owner.GetMultiTilesAt( x, y );

			if ( !eable.Any() )
				return Season.PatchTiles( tiles[x & 0x7][y & 0x7], Owner.Season );

			foreach ( Tile[] multiTiles in eable )
			{
				m_TilesList.AddRange( multiTiles );
			}

			m_TilesList.AddRange( Season.PatchTiles( tiles[x & 0x7][y & 0x7], Owner.Season ) );

			return m_TilesList.ToArray();
		}

		public void SetLandBlock( int x, int y, Tile[] value )
		{
			if ( x < 0 || y < 0 || x >= BlockWidth || y >= BlockHeight )
				return;

			if ( m_LandTiles[x] == null )
				m_LandTiles[x] = new Tile[BlockHeight][];

			m_LandTiles[x][y] = value;

			if ( m_LandPatches[x] == null )
				m_LandPatches[x] = new int[( BlockHeight + 31 ) >> 5];

			m_LandPatches[x][y >> 5] |= 1 << ( y & 0x1F );
		}

		public Tile[] GetLandBlock( int x, int y )
		{
			if ( x < 0 || y < 0 || x >= BlockWidth || y >= BlockHeight || MapStream == null )
				return m_InvalidLandBlock;

			if ( m_LandTiles[x] == null )
				m_LandTiles[x] = new Tile[BlockHeight][];

			Tile[] tiles = m_LandTiles[x][y];

			if ( tiles == null )
			{
				for ( int i = 0; tiles == null && i < m_FileShare.Count; ++i )
				{
					TileMatrix shared = m_FileShare[i];

					if ( x >= 0 && x < shared.BlockWidth && y >= 0 && y < shared.BlockHeight )
					{
						Tile[][] theirTiles = shared.m_LandTiles[x];

						if ( theirTiles != null )
							tiles = theirTiles[y];

						if ( tiles != null )
						{
							int[] theirBits = shared.m_LandPatches[x];

							if ( theirBits != null && ( theirBits[y >> 5] & ( 1 << ( y & 0x1F ) ) ) != 0 )
								tiles = null;
						}
					}
				}

				if ( tiles == null )
					tiles = ReadLandBlock( x, y );

				m_LandTiles[x][y] = tiles;
			}

			return tiles;
		}

		public Tile GetLandTile( int x, int y )
		{
			Tile[] tiles = GetLandBlock( x >> 3, y >> 3 );

			return tiles[( ( y & 0x7 ) << 3 ) + ( x & 0x7 )];
		}

		private static TileList[][] m_Lists;

		private static byte[] m_Buffer;

		private static StaticTile[] m_TileBuffer = new StaticTile[128];

		private unsafe Tile[][][] ReadStaticBlock( int x, int y )
		{
			try
			{
				IndexReader.BaseStream.Seek( ( ( x * BlockHeight ) + y ) * 12, SeekOrigin.Begin );

				int lookup = IndexReader.ReadInt32();
				int length = IndexReader.ReadInt32();

				if ( lookup < 0 || length <= 0 )
				{
					return EmptyStaticBlock;
				}
				else
				{
					int count = length / 7;

					DataStream.Seek( lookup, SeekOrigin.Begin );

					if ( m_TileBuffer.Length < count )
						m_TileBuffer = new StaticTile[count];

					StaticTile[] staTiles = m_TileBuffer; // new StaticTile[tileCount];

					fixed ( StaticTile* pTiles = staTiles )
					{
						if ( m_Buffer == null || length > m_Buffer.Length )
							m_Buffer = new byte[length];

						DataStream.Read( m_Buffer, 0, length );

						Marshal.Copy( m_Buffer, 0, new IntPtr( pTiles ), length );

						if ( m_Lists == null )
						{
							m_Lists = new TileList[8][];

							for ( int i = 0; i < 8; ++i )
							{
								m_Lists[i] = new TileList[8];

								for ( int j = 0; j < 8; ++j )
									m_Lists[i][j] = new TileList();
							}
						}

						TileList[][] lists = m_Lists;

						for ( int i = 0; i < count; i++ )
						{
							StaticTile* pCur = pTiles + i;
							lists[pCur->m_X & 0x7][pCur->m_Y & 0x7].Add( pCur->m_ID, pCur->m_Z );
						}

						Tile[][][] tiles = new Tile[8][][];

						for ( int i = 0; i < 8; ++i )
						{
							tiles[i] = new Tile[8][];

							for ( int j = 0; j < 8; ++j )
								tiles[i][j] = lists[i][j].ToArray();
						}

						return tiles;
					}
				}
			}
			catch ( EndOfStreamException )
			{
				if ( DateTime.UtcNow >= m_NextStaticWarning )
				{
					log.Warning( "Static EOS for {0} ({1}, {2})", Owner, x, y );
					m_NextStaticWarning = DateTime.UtcNow + TimeSpan.FromMinutes( 1.0 );
				}

				return EmptyStaticBlock;
			}
		}

		private DateTime m_NextStaticWarning;
		private DateTime m_NextLandWarning;

		private unsafe Tile[] ReadLandBlock( int x, int y )
		{
			try
			{
				int offset = ( ( x * BlockHeight ) + y ) * 196 + 4;

				if ( m_MapIndex != null )
					offset = m_MapIndex.Lookup( offset );

				MapStream.Seek( offset, SeekOrigin.Begin );

				Tile[] tiles = new Tile[64];

				fixed ( Tile* pTiles = tiles )
				{
					if ( m_Buffer == null || 192 > m_Buffer.Length )
						m_Buffer = new byte[192];

					MapStream.Read( m_Buffer, 0, 192 );

					Marshal.Copy( m_Buffer, 0, new IntPtr( pTiles ), 192 );
				}

				return tiles;
			}
			catch
			{
				if ( DateTime.UtcNow >= m_NextLandWarning )
				{
					log.Warning( "Land EOS for {0} ({1}, {2})", Owner, x, y );
					m_NextLandWarning = DateTime.UtcNow + TimeSpan.FromMinutes( 1.0 );
				}

				return m_InvalidLandBlock;
			}
		}

		public void Dispose()
		{
			if ( MapStream != null )
				MapStream.Close();

			if ( m_MapIndex != null )
				m_MapIndex.Close();

			if ( DataStream != null )
				DataStream.Close();

			if ( IndexReader != null )
				IndexReader.Close();
		}
	}

	[System.Runtime.InteropServices.StructLayout( System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 1 )]
	public struct StaticTile
	{
		public ushort m_ID;
		public byte m_X;
		public byte m_Y;
		public sbyte m_Z;
		public short m_Hue;
	}

	[System.Runtime.InteropServices.StructLayout( System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 1 )]
	public struct Tile
	{
		internal ushort m_ID;
		internal sbyte m_Z;

		public int ID
		{
			get { return m_ID; }
			set { m_ID = (ushort)value; }
		}

		public int Z
		{
			get { return m_Z; }
			set { m_Z = (sbyte)value; }
		}

		public int Height => TileData.ItemTable[m_ID & TileData.MaxItemValue].Height;

		public bool Ignored => ( m_ID == 2 || m_ID == 0x1DB || ( m_ID >= 0x1AE && m_ID <= 0x1B5 ) );

		public Tile( ushort id, sbyte z )
		{
			m_ID = id;
			m_Z = z;
		}

		public void Set( ushort id, sbyte z )
		{
			m_ID = id;
			m_Z = z;
		}
	}
}
