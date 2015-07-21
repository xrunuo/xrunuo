//
//  X-RunUO - Ultima Online Server Emulator
//  Copyright (C) 2015 Pedro Pardal
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Server.Items;
using Server.Network;
using Server.Targeting;

namespace Server
{
	[Flags]
	public enum MapRules
	{
		None = 0x0000,
		Internal = 0x0001, // Internal map (used for dragging, commodity deeds, etc)
		FreeMovement = 0x0002, // Anyone can move over anyone else without taking stamina loss
		BeneficialRestrictions = 0x0004, // Disallow performing beneficial actions on criminals/murderers
		HarmfulRestrictions = 0x0008, // Disallow performing harmful actions on innocents
		TrammelRules = FreeMovement | BeneficialRestrictions | HarmfulRestrictions,
		FeluccaRules = None
	}

	[Parsable]
	public sealed class Map : IMap, IComparable, IComparable<Map>
	{
		public const int SectorSize = 16;
		public const int SectorShift = 4;
		public static int SectorActiveRange = 2;

		private static Map[] m_Maps = new Map[0x100];

		public static Map[] Maps { get { return m_Maps; } }

		public static Map Felucca { get { return m_Maps[0]; } }
		public static Map Trammel { get { return m_Maps[1]; } }
		public static Map Ilshenar { get { return m_Maps[2]; } }
		public static Map Malas { get { return m_Maps[3]; } }
		public static Map Tokuno { get { return m_Maps[4]; } }
		public static Map TerMur { get { return m_Maps[5]; } }
		public static Map Internal { get { return m_Maps[0x7F]; } }

		private static List<Map> m_AllMaps = new List<Map>();

		public static List<Map> AllMaps { get { return m_AllMaps; } }

		private int m_MapID, m_MapIndex, m_FileIndex;

		private int m_Width, m_Height;
		private int m_SectorsWidth, m_SectorsHeight;
		private int m_Season;
		private Dictionary<string, Region> m_Regions;
		private Region m_DefaultRegion;

		public int Season { get { return m_Season; } }

		private string m_Name;
		private MapRules m_Rules;
		private Sector[][] m_Sectors;
		private Sector m_InvalidSector;

		private TileMatrix m_Tiles;

		private static string[] m_MapNames;
		private static Map[] m_MapValues;

		public static string[] GetMapNames()
		{
			CheckNamesAndValues();
			return m_MapNames;
		}

		public static Map[] GetMapValues()
		{
			CheckNamesAndValues();
			return m_MapValues;
		}

		public static Map Parse( string value )
		{
			CheckNamesAndValues();

			for ( int i = 0; i < m_MapNames.Length; ++i )
			{
				if ( Insensitive.Equals( m_MapNames[i], value ) )
					return m_MapValues[i];
			}

			int index;

			if ( int.TryParse( value, out index ) )
			{
				if ( index >= 0 && index < m_Maps.Length && m_Maps[index] != null )
					return m_Maps[index];
			}

			throw new Exception( "Invalid map name" );
		}

		private static void CheckNamesAndValues()
		{
			if ( m_MapNames != null && m_MapNames.Length == m_AllMaps.Count )
				return;

			m_MapNames = new string[m_AllMaps.Count];
			m_MapValues = new Map[m_AllMaps.Count];

			for ( int i = 0; i < m_AllMaps.Count; ++i )
			{
				Map map = m_AllMaps[i];

				m_MapNames[i] = map.Name;
				m_MapValues[i] = map;
			}
		}

		public override string ToString()
		{
			return m_Name;
		}

		public int GetAverageZ( int x, int y )
		{
			int z = 0, avg = 0, top = 0;

			GetAverageZ( x, y, ref z, ref avg, ref top );

			return avg;
		}

		public void GetAverageZ( int x, int y, ref int z, ref int avg, ref int top )
		{
			int zTop = Tiles.GetLandTile( x, y ).Z;
			int zLeft = Tiles.GetLandTile( x, y + 1 ).Z;
			int zRight = Tiles.GetLandTile( x + 1, y ).Z;
			int zBottom = Tiles.GetLandTile( x + 1, y + 1 ).Z;

			z = zTop;
			if ( zLeft < z )
				z = zLeft;
			if ( zRight < z )
				z = zRight;
			if ( zBottom < z )
				z = zBottom;

			top = zTop;
			if ( zLeft > top )
				top = zLeft;
			if ( zRight > top )
				top = zRight;
			if ( zBottom > top )
				top = zBottom;

			if ( Math.Abs( zTop - zBottom ) > Math.Abs( zLeft - zRight ) )
				avg = FloorAverage( zLeft, zRight );
			else
				avg = FloorAverage( zTop, zBottom );
		}

		private static int FloorAverage( int a, int b )
		{
			int v = a + b;

			if ( v < 0 )
				--v;

			return ( v / 2 );
		}

		#region Get*InRange/Bounds/AtWorldPoint

		public IEnumerable<object> GetObjectsInRange( IPoint2D p, int range = 18 )
		{
			return GetObjectsInBounds( new Rectangle2D( p.X - range, p.Y - range, range * 2 + 1, range * 2 + 1 ) );
		}

		public IEnumerable<object> GetObjectsInBounds( Rectangle2D bounds )
		{
			if ( this == Map.Internal )
				return Enumerable.Empty<object>();

			var mobiles = GetMobilesInBounds( bounds ).Cast<object>();
			var items = GetItemsInBounds( bounds ).Cast<object>();

			return mobiles.Union( items );
		}

		public IEnumerable<object> GetObjectsAtWorldPoint( IPoint2D p )
		{
			return GetObjectsInRange( p, 0 );
		}

		public IEnumerable<GameClient> GetClientsInRange( IPoint2D p, int range = 18 )
		{
			return GetClientsInBounds( new Rectangle2D( p.X - range, p.Y - range, range * 2 + 1, range * 2 + 1 ) );
		}

		public IEnumerable<GameClient> GetClientsInBounds( Rectangle2D bounds )
		{
			if ( this == Map.Internal )
				return Enumerable.Empty<GameClient>();

			return GetSectors( bounds ).SelectMany( sector => sector.Clients )
				.Where( state => state.Mobile != null && bounds.Contains( state.Mobile ) );
		}

		public IEnumerable<GameClient> GetClientsAtWorldPoint( IPoint2D p )
		{
			return GetClientsInRange( p, 0 );
		}

		public IEnumerable<Item> GetItemsInRange( IPoint2D p, int range = 18 )
		{
			return GetItemsInBounds( new Rectangle2D( p.X - range, p.Y - range, range * 2 + 1, range * 2 + 1 ) );
		}

		public IEnumerable<Item> GetItemsInBounds( Rectangle2D bounds )
		{
			if ( this == Map.Internal )
				return Enumerable.Empty<Item>();

			return GetSectors( bounds ).SelectMany( sector => sector.Items )
				.Where( item => item.Parent == null && bounds.Contains( item ) );
		}

		public IEnumerable<Item> GetItemsAtWorldPoint( IPoint2D p )
		{
			return GetItemsInRange( p, 0 );
		}

		public IEnumerable<Mobile> GetMobilesInRange( IPoint2D p, int range = 18 )
		{
			return GetMobilesInBounds( new Rectangle2D( p.X - range, p.Y - range, range * 2 + 1, range * 2 + 1 ) );
		}

		public IEnumerable<Mobile> GetMobilesInBounds( Rectangle2D bounds )
		{
			if ( this == Map.Internal )
				return Enumerable.Empty<Mobile>();

			return GetSectors( bounds ).SelectMany( sector => sector.Mobiles )
				.Where( mob => bounds.Contains( mob ) );
		}

		public IEnumerable<Mobile> GetMobilesAtWorldPoint( IPoint2D p )
		{
			return GetMobilesInRange( p, 0 );
		}
		#endregion

		public IEnumerable<Tile[]> GetMultiTilesAt( int x, int y )
		{
			if ( this == Map.Internal )
				return Enumerable.Empty<Tile[]>();

			return InternalGetMultiTilesAt( x, y );
		}

		private IEnumerable<Tile[]> InternalGetMultiTilesAt( int x, int y )
		{
			foreach ( BaseMulti multi in GetSector( x, y ).Multis )
			{
				if ( multi != null && !multi.Deleted )
				{
					MultiComponentList list = multi.Components;

					int xOffset = x - ( multi.Location.X + list.Min.X );
					int yOffset = y - ( multi.Location.Y + list.Min.Y );

					if ( xOffset >= 0 && xOffset < list.Width && yOffset >= 0 && yOffset < list.Height )
					{
						Tile[] tiles = list.Tiles[xOffset][yOffset];

						if ( tiles.Length > 0 )
						{
							// TODO: How to avoid this copy?
							Tile[] copy = new Tile[tiles.Length];

							for ( int i = 0; i < copy.Length; ++i )
							{
								copy[i] = tiles[i];
								copy[i].Z += multi.Z;
							}

							yield return copy;
						}
					}
				}
			}
		}

		#region CanFit
		public bool CanFit( Point3D p, int height, bool checkBlocksFit )
		{
			return CanFit( p.X, p.Y, p.Z, height, checkBlocksFit, true, true );
		}

		public bool CanFit( Point3D p, int height, bool checkBlocksFit, bool checkMobiles )
		{
			return CanFit( p.X, p.Y, p.Z, height, checkBlocksFit, checkMobiles, true );
		}

		public bool CanFit( Point2D p, int z, int height, bool checkBlocksFit )
		{
			return CanFit( p.X, p.Y, z, height, checkBlocksFit, true, true );
		}

		public bool CanFit( Point3D p, int height )
		{
			return CanFit( p.X, p.Y, p.Z, height, false, true, true );
		}

		public bool CanFit( Point2D p, int z, int height )
		{
			return CanFit( p.X, p.Y, z, height, false, true, true );
		}

		public bool CanFit( int x, int y, int z, int height )
		{
			return CanFit( x, y, z, height, false, true, true );
		}

		public bool CanFit( int x, int y, int z, int height, bool checksBlocksFit )
		{
			return CanFit( x, y, z, height, checksBlocksFit, true, true );
		}

		public bool CanFit( int x, int y, int z, int height, bool checkBlocksFit, bool checkMobiles )
		{
			return CanFit( x, y, z, height, checkBlocksFit, checkMobiles, true );
		}

		public bool CanFit( int x, int y, int z, int height, bool checkBlocksFit, bool checkMobiles, bool requireSurface )
		{
			if ( this == Map.Internal )
				return false;

			if ( x < 0 || y < 0 || x >= m_Width || y >= m_Height )
				return false;

			bool hasSurface = false;

			Tile lt = Tiles.GetLandTile( x, y );
			int lowZ = 0, avgZ = 0, topZ = 0;

			GetAverageZ( x, y, ref lowZ, ref avgZ, ref topZ );
			TileFlag landFlags = TileData.LandTable[lt.ID & TileData.MaxLandValue].Flags;

			if ( ( landFlags & TileFlag.Impassable ) != 0 && avgZ > z && ( z + height ) > lowZ )
				return false;
			else if ( ( landFlags & TileFlag.Impassable ) == 0 && z == avgZ && !lt.Ignored )
				hasSurface = true;

			Tile[] staticTiles = Tiles.GetStaticTiles( x, y, true );

			bool surface, impassable;

			for ( int i = 0; i < staticTiles.Length; ++i )
			{
				ItemData id = TileData.ItemTable[staticTiles[i].ID & TileData.MaxItemValue];
				surface = id.Surface;
				impassable = id.Impassable;

				if ( ( surface || impassable ) && ( staticTiles[i].Z + id.CalcHeight ) > z && ( z + height ) > staticTiles[i].Z )
					return false;
				else if ( surface && !impassable && z == ( staticTiles[i].Z + id.CalcHeight ) )
					hasSurface = true;
			}

			Sector sector = GetSector( x, y );
			List<Item> items = sector.Items;
			List<Mobile> mobs = sector.Mobiles;

			for ( int i = 0; i < items.Count; ++i )
			{
				Item item = items[i];

				if ( !( item is BaseMulti ) && item.ItemID <= TileData.MaxItemValue && item.AtWorldPoint( x, y ) )
				{
					ItemData id = item.ItemData;
					surface = id.Surface;
					impassable = id.Impassable;

					if ( ( surface || impassable || ( checkBlocksFit && item.BlocksFit ) ) && ( item.Z + id.CalcHeight ) > z && ( z + height ) > item.Z )
						return false;
					else if ( surface && !impassable && !item.Movable && z == ( item.Z + id.CalcHeight ) )
						hasSurface = true;
				}
			}

			if ( checkMobiles )
			{
				for ( int i = 0; i < mobs.Count; ++i )
				{
					Mobile m = mobs[i];

					if ( m.Location.X == x && m.Location.Y == y && m.Alive && ( m.AccessLevel == AccessLevel.Player || !m.Hidden ) )
						if ( ( m.Z + 16 ) > z && ( z + height ) > m.Z )
							return false;
				}
			}

			return !requireSurface || hasSurface;
		}

		#endregion

		#region CanSpawnMobile
		public bool CanSpawnMobile( Point3D p )
		{
			return CanSpawnMobile( p.X, p.Y, p.Z );
		}

		public bool CanSpawnMobile( Point2D p, int z )
		{
			return CanSpawnMobile( p.X, p.Y, z );
		}

		public bool CanSpawnMobile( int x, int y, int z )
		{
			if ( !Region.Find( new Point3D( x, y, z ), this ).AllowSpawn() )
				return false;

			foreach ( Item item in this.GetItemsInRange( new Point3D( x, y, z ), 5 ) )
				if ( item is SpawnBlocker )
					return false;

			return CanFit( x, y, z, 16 );
		}
		#endregion

		public Point3D GetSpawnPosition( Point3D center, int range )
		{
			for ( int i = 0; i < 10; i++ )
			{
				int x = center.X + ( Utility.Random( ( range * 2 ) + 1 ) - range );
				int y = center.Y + ( Utility.Random( ( range * 2 ) + 1 ) - range );
				int z = GetAverageZ( x, y );

				if ( CanSpawnMobile( new Point2D( x, y ), center.Z ) )
					return new Point3D( x, y, center.Z );
				else if ( CanSpawnMobile( new Point2D( x, y ), z ) )
					return new Point3D( x, y, z );
			}

			return center;
		}

		private class ZComparer : IComparer<Item>
		{
			public static readonly ZComparer Default = new ZComparer();

			public int Compare( Item x, Item y )
			{
				return x.Z.CompareTo( y.Z );
			}
		}

		public void FixColumn( int x, int y )
		{
			Tile landTile = Tiles.GetLandTile( x, y );

			int landZ = 0, landAvg = 0, landTop = 0;
			GetAverageZ( x, y, ref landZ, ref landAvg, ref landTop );

			Tile[] tiles = Tiles.GetStaticTiles( x, y, true );

			List<Item> items = new List<Item>();

			foreach ( Item item in GetItemsInRange( new Point3D( x, y, 0 ), 0 ) )
			{
				if ( !( item is BaseMulti ) && item.ItemID <= TileData.MaxItemValue )
				{
					items.Add( item );

					if ( items.Count > 100 )
						break;
				}
			}

			if ( items.Count > 100 )
				return;

			items.Sort( ZComparer.Default );

			for ( int i = 0; i < items.Count; ++i )
			{
				Item toFix = items[i];

				if ( !toFix.Movable )
					continue;

				int z = int.MinValue;
				int currentZ = toFix.Z;

				if ( !landTile.Ignored && landAvg <= currentZ )
					z = landAvg;

				for ( int j = 0; j < tiles.Length; ++j )
				{
					Tile tile = tiles[j];
					ItemData id = TileData.ItemTable[tile.ID & TileData.MaxItemValue];

					int checkZ = tile.Z;
					int checkTop = checkZ + id.CalcHeight;

					if ( checkTop == checkZ && !id.Surface )
						++checkTop;

					if ( checkTop > z && checkTop <= currentZ )
						z = checkTop;
				}

				for ( int j = 0; j < items.Count; ++j )
				{
					if ( j == i )
						continue;

					Item item = items[j];
					ItemData id = item.ItemData;

					int checkZ = item.Z;
					int checkTop = checkZ + id.CalcHeight;

					if ( checkTop == checkZ && !id.Surface )
						++checkTop;

					if ( checkTop > z && checkTop <= currentZ )
						z = checkTop;
				}

				if ( z != int.MinValue )
					toFix.Location = new Point3D( toFix.X, toFix.Y, z );
			}
		}

		public List<Tile> GetTilesAt( Point2D p, bool items, bool land, bool statics )
		{
			List<Tile> list = new List<Tile>();

			if ( this == Map.Internal )
				return list;

			if ( land )
				list.Add( Tiles.GetLandTile( p.X, p.Y ) );

			if ( statics )
				list.AddRange( Tiles.GetStaticTiles( p.X, p.Y, true ) );

			if ( items )
			{
				Sector sector = GetSector( p );

				foreach ( Item item in sector.Items )
					if ( item.AtWorldPoint( p.X, p.Y ) )
						list.Add( new Tile( (ushort) ( ( item.ItemID & 0x7FFF ) | 0x8000 ), (sbyte) item.Z ) );
			}

			return list;
		}

		/// <summary>
		/// Gets the highest surface that is lower than <paramref name="p"/>.
		/// </summary>
		/// <param name="p">The reference point.</param>
		/// <returns>A surface <typeparamref name="Tile"/> or <typeparamref name="Item"/>.</returns>
		public object GetTopSurface( Point3D p )
		{
			if ( this == Map.Internal )
				return null;

			object surface = null;
			int surfaceZ = int.MinValue;


			Tile lt = Tiles.GetLandTile( p.X, p.Y );

			if ( !lt.Ignored )
			{
				int avgZ = GetAverageZ( p.X, p.Y );

				if ( avgZ <= p.Z )
				{
					surface = lt;
					surfaceZ = avgZ;

					if ( surfaceZ == p.Z )
						return surface;
				}
			}


			Tile[] staticTiles = Tiles.GetStaticTiles( p.X, p.Y, true );

			for ( int i = 0; i < staticTiles.Length; i++ )
			{
				Tile tile = staticTiles[i];
				ItemData id = TileData.ItemTable[tile.ID & TileData.MaxItemValue];

				if ( id.Surface || ( id.Flags & TileFlag.Wet ) != 0 )
				{
					int tileZ = tile.Z + id.CalcHeight;

					if ( tileZ > surfaceZ && tileZ <= p.Z )
					{
						surface = tile;
						surfaceZ = tileZ;

						if ( surfaceZ == p.Z )
							return surface;
					}
				}
			}


			Sector sector = GetSector( p.X, p.Y );

			for ( int i = 0; i < sector.Items.Count; i++ )
			{
				Item item = sector.Items[i];

				if ( !( item is BaseMulti ) && item.ItemID <= TileData.MaxItemValue && item.AtWorldPoint( p.X, p.Y ) && !item.Movable )
				{
					ItemData id = item.ItemData;

					if ( id.Surface || ( id.Flags & TileFlag.Wet ) != 0 )
					{
						int itemZ = item.Z + id.CalcHeight;

						if ( itemZ > surfaceZ && itemZ <= p.Z )
						{
							surface = item;
							surfaceZ = itemZ;

							if ( surfaceZ == p.Z )
								return surface;
						}
					}
				}
			}


			return surface;
		}

		public void Bound( int x, int y, out int newX, out int newY )
		{
			if ( x < 0 )
				newX = 0;
			else if ( x >= m_Width )
				newX = m_Width - 1;
			else
				newX = x;

			if ( y < 0 )
				newY = 0;
			else if ( y >= m_Height )
				newY = m_Height - 1;
			else
				newY = y;
		}

		public Point2D Bound( Point2D p )
		{
			int x = p.X, y = p.Y;

			if ( x < 0 )
				x = 0;
			else if ( x >= m_Width )
				x = m_Width - 1;

			if ( y < 0 )
				y = 0;
			else if ( y >= m_Height )
				y = m_Height - 1;

			return new Point2D( x, y );
		}

		public Map( int mapID, int mapIndex, int fileIndex, int width, int height, int season, string name, MapRules rules )
		{
			m_MapID = mapID;
			m_MapIndex = mapIndex;
			m_FileIndex = fileIndex;
			m_Width = width;
			m_Height = height;
			m_Season = season;
			m_Name = name;
			m_Rules = rules;
			m_Regions = new Dictionary<string, Region>( StringComparer.OrdinalIgnoreCase );
			m_InvalidSector = new Sector( 0, 0, this );
			m_SectorsWidth = width >> SectorShift;
			m_SectorsHeight = height >> SectorShift;
			m_Sectors = new Sector[m_SectorsWidth][];
		}

		#region GetSector
		public Sector GetSector( Point3D p )
		{
			return InternalGetSector( p.X >> SectorShift, p.Y >> SectorShift );
		}

		public Sector GetSector( Point2D p )
		{
			return InternalGetSector( p.X >> SectorShift, p.Y >> SectorShift );
		}

		public Sector GetSector( IPoint2D p )
		{
			return InternalGetSector( p.X >> SectorShift, p.Y >> SectorShift );
		}

		public Sector GetSector( int x, int y )
		{
			return InternalGetSector( x >> SectorShift, y >> SectorShift );
		}

		public Sector GetRealSector( int x, int y )
		{
			return InternalGetSector( x, y );
		}

		private Sector InternalGetSector( int x, int y )
		{
			if ( x >= 0 && x < m_SectorsWidth && y >= 0 && y < m_SectorsHeight )
			{
				Sector[] xSectors = m_Sectors[x];

				if ( xSectors == null )
					m_Sectors[x] = xSectors = new Sector[m_SectorsHeight];

				Sector sec = xSectors[y];

				if ( sec == null )
					xSectors[y] = sec = new Sector( x, y, this );

				return sec;
			}
			else
			{
				return m_InvalidSector;
			}
		}
		#endregion

		public void ActivateSectors( int cx, int cy )
		{
			for ( int x = cx - SectorActiveRange; x <= cx + SectorActiveRange; ++x )
			{
				for ( int y = cy - SectorActiveRange; y <= cy + SectorActiveRange; ++y )
				{
					Sector sect = GetRealSector( x, y );
					if ( sect != m_InvalidSector )
						sect.Activate();
				}
			}
		}

		public void DeactivateSectors( int cx, int cy )
		{
			for ( int x = cx - SectorActiveRange; x <= cx + SectorActiveRange; ++x )
			{
				for ( int y = cy - SectorActiveRange; y <= cy + SectorActiveRange; ++y )
				{
					Sector sect = GetRealSector( x, y );
					if ( sect != m_InvalidSector && !PlayersInRange( sect, SectorActiveRange ) )
						sect.Deactivate();
				}
			}
		}

		private bool PlayersInRange( Sector sect, int range )
		{
			for ( int x = sect.X - range; x <= sect.X + range; ++x )
			{
				for ( int y = sect.Y - range; y <= sect.Y + range; ++y )
				{
					Sector check = GetRealSector( x, y );
					if ( check != m_InvalidSector && check.Players.Count > 0 )
						return true;
				}
			}

			return false;
		}

		public void OnClientChange( GameClient oldState, GameClient newState, Mobile m )
		{
			if ( this == Map.Internal )
				return;

			GetSector( m ).OnClientChange( oldState, newState );
		}

		public void OnEnter( Mobile m )
		{
			if ( this == Map.Internal )
				return;

			Sector sector = GetSector( m );

			sector.OnEnter( m );
		}

		public void OnEnter( Item item )
		{
			if ( this == Map.Internal )
				return;

			GetSector( item ).OnEnter( item );

			if ( item is BaseMulti )
			{
				BaseMulti m = (BaseMulti) item;
				MultiComponentList mcl = m.Components;

				Sector start = GetMultiMinSector( item.Location, mcl );
				Sector end = GetMultiMaxSector( item.Location, mcl );

				AddMulti( m, start, end );
			}
		}

		public void OnLeave( Mobile m )
		{
			if ( this == Map.Internal )
				return;

			Sector sector = GetSector( m );

			sector.OnLeave( m );
		}

		public void OnLeave( Item item )
		{
			if ( this == Map.Internal )
				return;

			GetSector( item ).OnLeave( item );

			if ( item is BaseMulti )
			{
				BaseMulti m = (BaseMulti) item;
				MultiComponentList mcl = m.Components;

				Sector start = GetMultiMinSector( item.Location, mcl );
				Sector end = GetMultiMaxSector( item.Location, mcl );

				RemoveMulti( m, start, end );
			}
		}

		public void RemoveMulti( BaseMulti m, Sector start, Sector end )
		{
			if ( this == Map.Internal )
				return;

			for ( int x = start.X; x <= end.X; ++x )
				for ( int y = start.Y; y <= end.Y; ++y )
					InternalGetSector( x, y ).OnMultiLeave( m );
		}

		public void AddMulti( BaseMulti m, Sector start, Sector end )
		{
			if ( this == Map.Internal )
				return;

			for ( int x = start.X; x <= end.X; ++x )
				for ( int y = start.Y; y <= end.Y; ++y )
					InternalGetSector( x, y ).OnMultiEnter( m );
		}

		public Sector GetMultiMinSector( Point3D loc, MultiComponentList mcl )
		{
			return GetSector( Bound( new Point2D( loc.X + mcl.Min.X, loc.Y + mcl.Min.Y ) ) );
		}

		public Sector GetMultiMaxSector( Point3D loc, MultiComponentList mcl )
		{
			return GetSector( Bound( new Point2D( loc.X + mcl.Max.X, loc.Y + mcl.Max.Y ) ) );
		}

		public void OnMove( Point3D oldLocation, Mobile m )
		{
			if ( this == Map.Internal )
				return;

			Sector oldSector = GetSector( oldLocation );
			Sector newSector = GetSector( m.Location );

			if ( oldSector != newSector )
			{
				oldSector.OnLeave( m );
				newSector.OnEnter( m );
			}
		}

		public void OnMove( Point3D oldLocation, Item item )
		{
			if ( this == Map.Internal )
				return;

			Sector oldSector = GetSector( oldLocation );
			Sector newSector = GetSector( item.Location );

			if ( oldSector != newSector )
			{
				oldSector.OnLeave( item );
				newSector.OnEnter( item );
			}

			if ( item is BaseMulti )
			{
				BaseMulti m = (BaseMulti) item;
				MultiComponentList mcl = m.Components;

				Sector start = GetMultiMinSector( item.Location, mcl );
				Sector end = GetMultiMaxSector( item.Location, mcl );

				Sector oldStart = GetMultiMinSector( oldLocation, mcl );
				Sector oldEnd = GetMultiMaxSector( oldLocation, mcl );

				if ( oldStart != start || oldEnd != end )
				{
					RemoveMulti( m, oldStart, oldEnd );
					AddMulti( m, start, end );
				}
			}
		}

		public TileMatrix Tiles
		{
			get
			{
				if ( m_Tiles == null )
					m_Tiles = new TileMatrix( this, m_FileIndex, m_MapID, m_Width, m_Height );

				return m_Tiles;
			}
		}

		public int MapID
		{
			get
			{
				return m_MapID;
			}
		}

		public int MapIndex
		{
			get
			{
				return m_MapIndex;
			}
		}

		public int Width
		{
			get
			{
				return m_Width;
			}
		}

		public int Height
		{
			get
			{
				return m_Height;
			}
		}

		public Dictionary<string, Region> Regions
		{
			get
			{
				return m_Regions;
			}
		}

		public void RegisterRegion( Region reg )
		{
			string regName = reg.Name;

			if ( regName != null )
			{
				if ( m_Regions.ContainsKey( regName ) )
					Console.WriteLine( "Warning: Duplicate region name '{0}' for map '{1}'", regName, this.Name );
				else
					m_Regions[regName] = reg;
			}
		}

		public void UnregisterRegion( Region reg )
		{
			string regName = reg.Name;

			if ( regName != null )
				m_Regions.Remove( regName );
		}

		public Region DefaultRegion
		{
			get
			{
				if ( m_DefaultRegion == null )
					m_DefaultRegion = new Region( null, this, 0, new Rectangle3D[0] );

				return m_DefaultRegion;
			}
			set
			{
				m_DefaultRegion = value;
			}
		}

		public MapRules Rules
		{
			get
			{
				return m_Rules;
			}
			set
			{
				m_Rules = value;
			}
		}

		public Sector InvalidSector
		{
			get
			{
				return m_InvalidSector;
			}
		}

		public string Name
		{
			get
			{
				return m_Name;
			}
			set
			{
				m_Name = value;
			}
		}

		public IEnumerable<Sector> GetSectors( Rectangle2D bounds )
		{
			int xStart, yStart, xEnd, yEnd;

			Bound( bounds.Start.X, bounds.Start.Y, out xStart, out yStart );
			Bound( bounds.End.X - 1, bounds.End.Y - 1, out xEnd, out yEnd );

			int xSectorStart = xStart >>= Map.SectorShift;
			int ySectorStart = yStart >>= Map.SectorShift;
			int xSectorEnd = xEnd >> Map.SectorShift;
			int ySectorEnd = yEnd >> Map.SectorShift;

			for ( int xSector = xSectorStart; xSector <= xSectorEnd; xSector++ )
			{
				for ( int ySector = ySectorStart; ySector <= ySectorEnd; ySector++ )
				{
					yield return InternalGetSector( xSector, ySector );
				}
			}
		}

		public Point3D GetPoint( object o )
		{
			Point3D p;

			if ( o is Mobile )
			{
				p = ( (Mobile) o ).Location;
				p.Z += 14;
			}
			else if ( o is Item )
			{
				p = ( (Item) o ).GetWorldLocation();
				p.Z += ( ( (Item) o ).ItemData.Height / 2 ) + 1;
			}
			else if ( o is Point3D )
			{
				p = (Point3D) o;
			}
			else if ( o is LandTarget )
			{
				p = ( (LandTarget) o ).Location;

				int low = 0, avg = 0, top = 0;
				GetAverageZ( p.X, p.Y, ref low, ref avg, ref top );

				p.Z = top + 1;
			}
			else if ( o is StaticTarget )
			{
				StaticTarget st = (StaticTarget) o;
				ItemData id = TileData.ItemTable[st.ItemID & TileData.MaxItemValue];

				p = new Point3D( st.X, st.Y, st.Z - id.CalcHeight + ( id.Height / 2 ) + 1 );
			}
			else if ( o is IPoint3D )
			{
				p = new Point3D( (IPoint3D) o );
			}
			else
			{
				Console.WriteLine( "Warning: Invalid object ({0}) in line of sight", o );
				p = Point3D.Zero;
			}

			return p;
		}

		#region Line Of Sight
		private static int m_MaxLOSDistance = 25;

		public static int MaxLOSDistance
		{
			get { return m_MaxLOSDistance; }
			set { m_MaxLOSDistance = value; }
		}

		public bool LineOfSight( Point3D org, Point3D dest )
		{
			if ( this == Map.Internal )
				return false;

			if ( !org.InRange( dest, m_MaxLOSDistance ) )
				return false;

			Point3D end = dest;

			if ( org.X > dest.X || ( org.X == dest.X && org.Y > dest.Y ) || ( org.X == dest.X && org.Y == dest.Y && org.Z > dest.Z ) )
			{
				Point3D swap = org;
				org = dest;
				dest = swap;
			}

			double rise, run, zslp;
			double sq3d;
			double x, y, z;
			int xd, yd, zd;
			int ix, iy, iz;
			int height;
			bool found;
			Point3D p;
			Point3DList path = m_PathList;
			TileFlag flags;

			if ( org == dest )
				return true;

			if ( path.Count > 0 )
				path.Clear();

			xd = dest.X - org.X;
			yd = dest.Y - org.Y;
			zd = dest.Z - org.Z;
			zslp = Math.Sqrt( xd * xd + yd * yd );
			if ( zd != 0 )
				sq3d = Math.Sqrt( zslp * zslp + zd * zd );
			else
				sq3d = zslp;

			rise = ( (float) yd ) / sq3d;
			run = ( (float) xd ) / sq3d;
			zslp = ( (float) zd ) / sq3d;

			y = org.Y;
			z = org.Z;
			x = org.X;
			while ( Utility.NumberBetween( x, dest.X, org.X, 0.5 ) && Utility.NumberBetween( y, dest.Y, org.Y, 0.5 ) && Utility.NumberBetween( z, dest.Z, org.Z, 0.5 ) )
			{
				ix = (int) Math.Round( x );
				iy = (int) Math.Round( y );
				iz = (int) Math.Round( z );
				if ( path.Count > 0 )
				{
					p = path.Last;

					if ( p.X != ix || p.Y != iy || p.Z != iz )
						path.Add( ix, iy, iz );
				}
				else
				{
					path.Add( ix, iy, iz );
				}
				x += run;
				y += rise;
				z += zslp;
			}

			if ( path.Count == 0 )
				return true; // should never happen, but to be safe.

			p = path.Last;

			if ( p != dest )
				path.Add( dest );

			Point3D pTop = org, pBottom = dest;
			Utility.FixPoints( ref pTop, ref pBottom );

			int pathCount = path.Count;

			for ( int i = 0; i < pathCount; ++i )
			{
				Point3D point = path[i];

				Tile landTile = Tiles.GetLandTile( point.X, point.Y );
				int landZ = 0, landAvg = 0, landTop = 0;
				GetAverageZ( point.X, point.Y, ref landZ, ref landAvg, ref landTop );

				if ( landZ <= point.Z && landTop >= point.Z && ( point.X != end.X || point.Y != end.Y || landZ > end.Z || landTop < end.Z ) && !landTile.Ignored )
					return false;

				Tile[] statics = Tiles.GetStaticTiles( point.X, point.Y, true );

				bool contains = false;
				int ltID = landTile.ID;

				for ( int j = 0; !contains && j < m_InvalidLandTiles.Length; ++j )
					contains = ( ltID == m_InvalidLandTiles[j] );

				if ( contains && statics.Length == 0 )
				{
					foreach ( Item item in GetItemsInRange( point, 0 ) )
					{
						if ( item.Visible )
							contains = false;

						if ( !contains )
							break;
					}

					if ( contains )
						return false;
				}

				for ( int j = 0; j < statics.Length; ++j )
				{
					Tile t = statics[j];

					ItemData id = TileData.ItemTable[t.ID & TileData.MaxItemValue];

					flags = id.Flags;
					height = id.CalcHeight;

					if ( t.Z <= point.Z && t.Z + height >= point.Z && ( flags & ( TileFlag.Window | TileFlag.NoShoot ) ) != 0 )
					{
						if ( point.X == end.X && point.Y == end.Y && t.Z <= end.Z && t.Z + height >= end.Z )
							continue;

						return false;
					}
				}
			}

			Rectangle2D rect = new Rectangle2D( pTop.X, pTop.Y, ( pBottom.X - pTop.X ) + 1, ( pBottom.Y - pTop.Y ) + 1 );

			foreach ( Item i in GetItemsInBounds( rect ) )
			{
				if ( !i.Visible )
					continue;

				if ( i is BaseMulti || i.ItemID > TileData.MaxItemValue )
					continue;

				ItemData id = i.ItemData;
				flags = id.Flags;

				if ( ( flags & ( TileFlag.Window | TileFlag.NoShoot ) ) == 0 )
					continue;

				height = id.CalcHeight;

				found = false;

				int count = path.Count;

				for ( int j = 0; j < count; ++j )
				{
					Point3D point = path[j];
					Point3D loc = i.Location;

					if ( loc.X == point.X && loc.Y == point.Y &&
						loc.Z <= point.Z && loc.Z + height >= point.Z )
					{
						if ( loc.X == end.X && loc.Y == end.Y && loc.Z <= end.Z && loc.Z + height >= end.Z )
							continue;

						found = true;
						break;
					}
				}

				if ( !found )
					continue;

				return false;
			}

			return true;
		}

		public bool LineOfSight( object from, object dest )
		{
			if ( from == dest || ( from is Mobile && ( (Mobile) from ).AccessLevel > AccessLevel.Player ) )
				return true;
			else if ( dest is Item && from is Mobile && ( (Item) dest ).RootParent == from )
				return true;

			return LineOfSight( GetPoint( from ), GetPoint( dest ) );
		}

		public bool LineOfSight( Mobile from, Point3D target )
		{
			if ( from.AccessLevel > AccessLevel.Player )
				return true;

			Point3D eye = from.Location;

			eye.Z += 14;

			return LineOfSight( eye, target );
		}

		public bool LineOfSight( Mobile from, Mobile to )
		{
			if ( from == to || from.AccessLevel > AccessLevel.Player )
				return true;

			Point3D eye = from.Location;
			Point3D target = to.Location;

			eye.Z += 14;
			target.Z += 14;//10;

			return LineOfSight( eye, target );
		}
		#endregion

		private static int[] m_InvalidLandTiles = new int[] { 0x244 };

		public static int[] InvalidLandTiles
		{
			get { return m_InvalidLandTiles; }
			set { m_InvalidLandTiles = value; }
		}

		private static Point3DList m_PathList = new Point3DList();
		public int CompareTo( Map other )
		{
			if ( other == null )
				return -1;

			return m_MapID.CompareTo( other.m_MapID );
		}

		public int CompareTo( object other )
		{
			if ( other == null || other is Map )
				return this.CompareTo( other );

			throw new ArgumentException();
		}
	}

	public static class MobileMapExtensions
	{
		public static IEnumerable<Item> GetItemsInRange( this Mobile m, int range = 18 )
		{
			if ( m.Map == null )
				return Enumerable.Empty<Item>();

			return m.Map.GetItemsInRange( m.Location, range );
		}

		public static IEnumerable<object> GetObjectsInRange( this Mobile m, int range = 18 )
		{
			if ( m.Map == null )
				return Enumerable.Empty<object>();

			return m.Map.GetObjectsInRange( m.Location, range );
		}

		public static IEnumerable<Mobile> GetMobilesInRange( this Mobile m, int range = 18 )
		{
			if ( m.Map == null )
				return Enumerable.Empty<Mobile>();

			return m.Map.GetMobilesInRange( m.Location, range );
		}

		public static IEnumerable<GameClient> GetClientsInRange( this Mobile m, int range = 18 )
		{
			if ( m.Map == null )
				return Enumerable.Empty<GameClient>();

			return m.Map.GetClientsInRange( m.Location, range );
		}
	}
}