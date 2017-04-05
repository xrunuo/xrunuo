using System;
using System.Collections;
using System.IO;

namespace Server
{
	public class MultiData
	{
		private static readonly ILog log = LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

		private static readonly MultiComponentList[] m_Components;

		private static readonly FileStream m_Index;
		private static readonly FileStream m_Stream;
		private static readonly BinaryReader m_IndexReader;
		private static readonly BinaryReader m_StreamReader;

		public static MultiComponentList GetComponents( int multiID )
		{
			MultiComponentList mcl;

			if ( multiID >= 0 && multiID < m_Components.Length )
			{
				mcl = m_Components[multiID];

				if ( mcl == null )
					m_Components[multiID] = mcl = Load( multiID );
			}
			else
			{
				mcl = MultiComponentList.Empty;
			}

			return mcl;
		}

		public static MultiComponentList Load( int multiID )
		{
			try
			{
				m_IndexReader.BaseStream.Seek( multiID * 12, SeekOrigin.Begin );

				int lookup = m_IndexReader.ReadInt32();
				int length = m_IndexReader.ReadInt32();

				if ( lookup < 0 || length <= 0 )
					return MultiComponentList.Empty;

				m_StreamReader.BaseStream.Seek( lookup, SeekOrigin.Begin );

				return new MultiComponentList( m_StreamReader, length / 16 );
			}
			catch
			{
				return MultiComponentList.Empty;
			}
		}

		static MultiData()
		{
			string idxPath = Core.FindDataFile( "Multi.idx" );
			string mulPath = Core.FindDataFile( "Multi.mul" );

			if ( File.Exists( idxPath ) && File.Exists( mulPath ) )
			{
				m_Index = new FileStream( idxPath, FileMode.Open, FileAccess.Read, FileShare.Read );
				m_IndexReader = new BinaryReader( m_Index );

				m_Stream = new FileStream( mulPath, FileMode.Open, FileAccess.Read, FileShare.Read );
				m_StreamReader = new BinaryReader( m_Stream );

				m_Components = new MultiComponentList[(int) ( m_Index.Length / 12 )];

				string vdPath = Core.FindDataFile( "Verdata.mul" );

				if ( File.Exists( vdPath ) )
				{
					using ( FileStream fs = new FileStream( vdPath, FileMode.Open, FileAccess.Read, FileShare.Read ) )
					{
						BinaryReader bin = new BinaryReader( fs );

						int count = bin.ReadInt32();

						for ( int i = 0; i < count; ++i )
						{
							int file = bin.ReadInt32();
							int index = bin.ReadInt32();
							int lookup = bin.ReadInt32();
							int length = bin.ReadInt32();
							/*int extra = */
							bin.ReadInt32();

							if ( file == 14 && index >= 0 && index < m_Components.Length && lookup >= 0 && length > 0 )
							{
								bin.BaseStream.Seek( lookup, SeekOrigin.Begin );

								m_Components[index] = new MultiComponentList( bin, length / 12 );

								bin.BaseStream.Seek( 24 + ( i * 20 ), SeekOrigin.Begin );
							}
						}

						bin.Close();
					}
				}
			}
			else
			{
				log.Warning( "Multi data files not found" );

				m_Components = new MultiComponentList[0];
			}
		}
	}

	public struct MultiTileEntry
	{
		public ushort m_ItemID;
		public short m_OffsetX, m_OffsetY, m_OffsetZ;
		public int m_Flags;

		public MultiTileEntry( ushort itemID, short xOffset, short yOffset, short zOffset, int flags )
		{
			m_ItemID = itemID;
			m_OffsetX = xOffset;
			m_OffsetY = yOffset;
			m_OffsetZ = zOffset;
			m_Flags = flags;
		}
	}

	public sealed class MultiComponentList
	{
		private Point2D m_Min, m_Max;

		public static readonly MultiComponentList Empty = new MultiComponentList();

		public Point2D Min => m_Min;

		public Point2D Max => m_Max;

		public Point2D Center { get; }

		public int Width { get; private set; }

		public int Height { get; private set; }

		public Tile[][][] Tiles { get; private set; }

		public MultiTileEntry[] List { get; private set; }

		public void Add( int itemID, int x, int y, int z )
		{
			int vx = x + Center.X;
			int vy = y + Center.Y;

			if ( vx >= 0 && vx < Width && vy >= 0 && vy < Height )
			{
				Tile[] oldTiles = Tiles[vx][vy];

				for ( int i = oldTiles.Length - 1; i >= 0; --i )
				{
					ItemData data = TileData.ItemTable[itemID & TileData.MaxItemValue];

					if ( oldTiles[i].Z == z && ( oldTiles[i].Height > 0 == data.Height > 0 ) )
					{
						bool newIsRoof = ( data.Flags & TileFlag.Roof ) != 0;
						bool oldIsRoof = ( TileData.ItemTable[oldTiles[i].ID & TileData.MaxItemValue].Flags & TileFlag.Roof ) != 0;

						if ( newIsRoof == oldIsRoof )
							Remove( oldTiles[i].ID, x, y, z );
					}
				}

				oldTiles = Tiles[vx][vy];

				Tile[] newTiles = new Tile[oldTiles.Length + 1];

				for ( int i = 0; i < oldTiles.Length; ++i )
					newTiles[i] = oldTiles[i];

				newTiles[oldTiles.Length] = new Tile( (ushort) itemID, (sbyte) z );

				Tiles[vx][vy] = newTiles;

				MultiTileEntry[] oldList = List;
				MultiTileEntry[] newList = new MultiTileEntry[oldList.Length + 1];

				for ( int i = 0; i < oldList.Length; ++i )
					newList[i] = oldList[i];

				newList[oldList.Length] = new MultiTileEntry( (ushort) itemID, (short) x, (short) y, (short) z, 1 );

				List = newList;

				if ( x < m_Min.X )
					m_Min.X = x;

				if ( y < m_Min.Y )
					m_Min.Y = y;

				if ( x > m_Max.X )
					m_Max.X = x;

				if ( y > m_Max.Y )
					m_Max.Y = y;
			}
		}

		public void RemoveXYZH( int x, int y, int z, int minHeight )
		{
			int vx = x + Center.X;
			int vy = y + Center.Y;

			if ( vx >= 0 && vx < Width && vy >= 0 && vy < Height )
			{
				Tile[] oldTiles = Tiles[vx][vy];

				for ( int i = 0; i < oldTiles.Length; ++i )
				{
					Tile tile = oldTiles[i];

					if ( tile.Z == z && tile.Height >= minHeight )
					{
						Tile[] newTiles = new Tile[oldTiles.Length - 1];

						for ( int j = 0; j < i; ++j )
							newTiles[j] = oldTiles[j];

						for ( int j = i + 1; j < oldTiles.Length; ++j )
							newTiles[j - 1] = oldTiles[j];

						Tiles[vx][vy] = newTiles;

						break;
					}
				}

				MultiTileEntry[] oldList = List;

				for ( int i = 0; i < oldList.Length; ++i )
				{
					MultiTileEntry tile = oldList[i];

					if ( tile.m_OffsetX == (short) x && tile.m_OffsetY == (short) y && tile.m_OffsetZ == (short) z && TileData.ItemTable[tile.m_ItemID & TileData.MaxItemValue].Height >= minHeight )
					{
						MultiTileEntry[] newList = new MultiTileEntry[oldList.Length - 1];

						for ( int j = 0; j < i; ++j )
							newList[j] = oldList[j];

						for ( int j = i + 1; j < oldList.Length; ++j )
							newList[j - 1] = oldList[j];

						List = newList;

						break;
					}
				}
			}
		}

		public void Remove( int itemID, int x, int y, int z )
		{
			int vx = x + Center.X;
			int vy = y + Center.Y;

			if ( vx >= 0 && vx < Width && vy >= 0 && vy < Height )
			{
				Tile[] oldTiles = Tiles[vx][vy];

				for ( int i = 0; i < oldTiles.Length; ++i )
				{
					Tile tile = oldTiles[i];

					if ( ( tile.ID & TileData.MaxItemValue ) == ( itemID & TileData.MaxItemValue ) && tile.Z == z )
					{
						Tile[] newTiles = new Tile[oldTiles.Length - 1];

						for ( int j = 0; j < i; ++j )
							newTiles[j] = oldTiles[j];

						for ( int j = i + 1; j < oldTiles.Length; ++j )
							newTiles[j - 1] = oldTiles[j];

						Tiles[vx][vy] = newTiles;

						break;
					}
				}

				MultiTileEntry[] oldList = List;

				for ( int i = 0; i < oldList.Length; ++i )
				{
					MultiTileEntry tile = oldList[i];

					if ( ( tile.m_ItemID & TileData.MaxItemValue ) == (ushort) ( itemID & TileData.MaxItemValue ) && tile.m_OffsetX == (short) x && tile.m_OffsetY == (short) y && tile.m_OffsetZ == (short) z )
					{
						MultiTileEntry[] newList = new MultiTileEntry[oldList.Length - 1];

						for ( int j = 0; j < i; ++j )
							newList[j] = oldList[j];

						for ( int j = i + 1; j < oldList.Length; ++j )
							newList[j - 1] = oldList[j];

						List = newList;

						break;
					}
				}
			}
		}

		public void Resize( int newWidth, int newHeight )
		{
			int oldWidth = Width, oldHeight = Height;
			Tile[][][] oldTiles = Tiles;

			int totalLength = 0;

			Tile[][][] newTiles = new Tile[newWidth][][];

			for ( int x = 0; x < newWidth; ++x )
			{
				newTiles[x] = new Tile[newHeight][];

				for ( int y = 0; y < newHeight; ++y )
				{
					if ( x < oldWidth && y < oldHeight )
						newTiles[x][y] = oldTiles[x][y];
					else
						newTiles[x][y] = new Tile[0];

					totalLength += newTiles[x][y].Length;
				}
			}

			Tiles = newTiles;
			List = new MultiTileEntry[totalLength];
			Width = newWidth;
			Height = newHeight;

			m_Min = Point2D.Zero;
			m_Max = Point2D.Zero;

			int index = 0;

			for ( int x = 0; x < newWidth; ++x )
			{
				for ( int y = 0; y < newHeight; ++y )
				{
					Tile[] tiles = newTiles[x][y];

					for ( int i = 0; i < tiles.Length; ++i )
					{
						Tile tile = tiles[i];

						int vx = x - Center.X;
						int vy = y - Center.Y;

						if ( vx < m_Min.X )
							m_Min.X = vx;

						if ( vy < m_Min.Y )
							m_Min.Y = vy;

						if ( vx > m_Max.X )
							m_Max.X = vx;

						if ( vy > m_Max.Y )
							m_Max.Y = vy;

						List[index++] = new MultiTileEntry( (ushort) tile.ID, (short) vx, (short) vy, (short) tile.Z, 1 );
					}
				}
			}
		}

		public MultiComponentList( MultiComponentList toCopy )
		{
			m_Min = toCopy.m_Min;
			m_Max = toCopy.m_Max;

			Center = toCopy.Center;

			Width = toCopy.Width;
			Height = toCopy.Height;

			Tiles = new Tile[Width][][];

			for ( int x = 0; x < Width; ++x )
			{
				Tiles[x] = new Tile[Height][];

				for ( int y = 0; y < Height; ++y )
				{
					Tiles[x][y] = new Tile[toCopy.Tiles[x][y].Length];

					for ( int i = 0; i < Tiles[x][y].Length; ++i )
						Tiles[x][y][i] = toCopy.Tiles[x][y][i];
				}
			}

			List = new MultiTileEntry[toCopy.List.Length];

			for ( int i = 0; i < List.Length; ++i )
				List[i] = toCopy.List[i];
		}

		public void Serialize( GenericWriter writer )
		{
			writer.Write( (int) 1 ); // version;

			writer.Write( m_Min );
			writer.Write( m_Max );
			writer.Write( Center );

			writer.Write( (int) Width );
			writer.Write( (int) Height );

			writer.Write( (int) List.Length );

			for ( int i = 0; i < List.Length; ++i )
			{
				MultiTileEntry ent = List[i];

				writer.Write( (ushort) ent.m_ItemID );
				writer.Write( (short) ent.m_OffsetX );
				writer.Write( (short) ent.m_OffsetY );
				writer.Write( (short) ent.m_OffsetZ );
				writer.Write( (int) ent.m_Flags );
			}
		}

		public MultiComponentList( GenericReader reader )
		{
			int version = reader.ReadInt();

			switch ( version )
			{
				case 1:
				case 0:
					{
						m_Min = reader.ReadPoint2D();
						m_Max = reader.ReadPoint2D();
						Center = reader.ReadPoint2D();
						Width = reader.ReadInt();
						Height = reader.ReadInt();

						int length = reader.ReadInt();

						MultiTileEntry[] allTiles = List = new MultiTileEntry[length];

						for ( int i = 0; i < length; ++i )
						{
							ushort id = reader.ReadUShort();

							if ( version == 0 && id >= 0x8000 )
								id -= 0x8000;

							allTiles[i].m_ItemID = id;
							allTiles[i].m_OffsetX = reader.ReadShort();
							allTiles[i].m_OffsetY = reader.ReadShort();
							allTiles[i].m_OffsetZ = reader.ReadShort();
							allTiles[i].m_Flags = reader.ReadInt();
						}

						TileList[][] tiles = new TileList[Width][];
						Tiles = new Tile[Width][][];

						for ( int x = 0; x < Width; ++x )
						{
							tiles[x] = new TileList[Height];
							Tiles[x] = new Tile[Height][];

							for ( int y = 0; y < Height; ++y )
								tiles[x][y] = new TileList();
						}

						for ( int i = 0; i < allTiles.Length; ++i )
						{
							if ( i == 0 || allTiles[i].m_Flags != 0 )
							{
								int xOffset = allTiles[i].m_OffsetX + Center.X;
								int yOffset = allTiles[i].m_OffsetY + Center.Y;

								tiles[xOffset][yOffset].Add( (ushort) allTiles[i].m_ItemID, (sbyte) allTiles[i].m_OffsetZ );
							}
						}

						for ( int x = 0; x < Width; ++x )
							for ( int y = 0; y < Height; ++y )
								Tiles[x][y] = tiles[x][y].ToArray();

						break;
					}
			}
		}

		public MultiComponentList( BinaryReader reader, int count )
		{
			MultiTileEntry[] allTiles = List = new MultiTileEntry[count];

			for ( int i = 0; i < count; ++i )
			{
				allTiles[i].m_ItemID = reader.ReadUInt16();
				allTiles[i].m_OffsetX = reader.ReadInt16();
				allTiles[i].m_OffsetY = reader.ReadInt16();
				allTiles[i].m_OffsetZ = reader.ReadInt16();
				allTiles[i].m_Flags = reader.ReadInt32();
				reader.ReadInt32(); // ??

				MultiTileEntry e = allTiles[i];

				if ( i == 0 || e.m_Flags != 0 )
				{
					if ( e.m_OffsetX < m_Min.X )
						m_Min.X = e.m_OffsetX;

					if ( e.m_OffsetY < m_Min.Y )
						m_Min.Y = e.m_OffsetY;

					if ( e.m_OffsetX > m_Max.X )
						m_Max.X = e.m_OffsetX;

					if ( e.m_OffsetY > m_Max.Y )
						m_Max.Y = e.m_OffsetY;
				}
			}

			Center = new Point2D( -m_Min.X, -m_Min.Y );
			Width = ( m_Max.X - m_Min.X ) + 1;
			Height = ( m_Max.Y - m_Min.Y ) + 1;

			TileList[][] tiles = new TileList[Width][];
			Tiles = new Tile[Width][][];

			for ( int x = 0; x < Width; ++x )
			{
				tiles[x] = new TileList[Height];
				Tiles[x] = new Tile[Height][];

				for ( int y = 0; y < Height; ++y )
					tiles[x][y] = new TileList();
			}

			for ( int i = 0; i < allTiles.Length; ++i )
			{
				if ( i == 0 || allTiles[i].m_Flags != 0 )
				{
					int xOffset = allTiles[i].m_OffsetX + Center.X;
					int yOffset = allTiles[i].m_OffsetY + Center.Y;

					tiles[xOffset][yOffset].Add( (ushort) allTiles[i].m_ItemID, (sbyte) allTiles[i].m_OffsetZ );
				}
			}

			for ( int x = 0; x < Width; ++x )
				for ( int y = 0; y < Height; ++y )
					Tiles[x][y] = tiles[x][y].ToArray();
		}

		private MultiComponentList()
		{
			Tiles = new Tile[0][][];
			List = new MultiTileEntry[0];
		}
	}
}
