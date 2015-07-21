using System;
using System.Collections;
using Server;

namespace Server.Items
{
	public class DungeonChestsSpawner : Item
	{
		public class ChestEntry
		{
			private Map m_Map;
			private Point3D m_Location;
			private int m_MinDelay;
			private int m_MaxDelay;
			private Type m_Type;
			private int m_Range;

			public Map Map { get { return m_Map; } }
			public Point3D Location { get { return m_Location; } }
			public int MinDelay { get { return m_MinDelay; } }
			public int MaxDelay { get { return m_MaxDelay; } }
			public Type Type { get { return m_Type; } }
			public int Range { get { return m_Range; } }

			public ChestEntry( Map map, Point3D location, int minDelay, int maxDelay, Type type, int range )
			{
				m_Map = map;
				m_Location = location;
				m_MinDelay = minDelay;
				m_MaxDelay = maxDelay;
				m_Type = type;
				m_Range = range;
			}

			public Point3D GetSpawnPosition()
			{
				Map map = Map;

				if ( map == null )
					return Location;

				// Try 10 times to find a Spawnable location.
				for ( int i = 0; i < 10; i++ )
				{
					int x = Location.X + ( Utility.Random( ( m_Range * 2 ) + 1 ) - m_Range );
					int y = Location.Y + ( Utility.Random( ( m_Range * 2 ) + 1 ) - m_Range );
					int z = Map.GetAverageZ( x, y );

					if ( Map.CanSpawnMobile( new Point2D( x, y ), Location.Z ) )
						return new Point3D( x, y, Location.Z );
					else if ( Map.CanSpawnMobile( new Point2D( x, y ), z ) )
						return new Point3D( x, y, z );
				}

				return Location;
			}

			public Item CreateInstance()
			{
				Item item = (Item) Activator.CreateInstance( m_Type );

				item.Movable = false;

				item.MoveToWorld( GetSpawnPosition(), this.Map );

				return item;
			}
		}

		private static ChestEntry[] m_Entries = new ChestEntry[]
			{
				// Covetous lvl 1 Felucca
				new ChestEntry( Map.Felucca, new Point3D( 5452, 1891, 0 ), 60, 120, typeof( DungeonTreasureChestFirst ), 2 ),
				new ChestEntry( Map.Felucca, new Point3D( 5452, 1891, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 2 ),
				new ChestEntry( Map.Felucca, new Point3D( 5452, 1891, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 2 ),
				new ChestEntry( Map.Felucca, new Point3D( 5400, 1860, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5400, 1860, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5400, 1860, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ), 
				new ChestEntry( Map.Felucca, new Point3D( 5387, 1911, 0 ), 60, 120, typeof( DungeonTreasureChestFirst ), 2 ),
				new ChestEntry( Map.Felucca, new Point3D( 5387, 1911, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 2 ),
				new ChestEntry( Map.Felucca, new Point3D( 5387, 1911, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 2 ),
				new ChestEntry( Map.Felucca, new Point3D( 5410, 1933, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5410, 1933, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5410, 1933, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ), 

				// Covetous lvl 1 Trammel
				new ChestEntry( Map.Trammel, new Point3D( 5452, 1891, 0 ), 60, 120, typeof( DungeonTreasureChestFirst ), 2 ),
				new ChestEntry( Map.Trammel, new Point3D( 5452, 1891, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 2 ),
				new ChestEntry( Map.Trammel, new Point3D( 5452, 1891, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 2 ),
				new ChestEntry( Map.Trammel, new Point3D( 5400, 1860, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5400, 1860, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5400, 1860, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ), 
				new ChestEntry( Map.Trammel, new Point3D( 5387, 1911, 0 ), 60, 120, typeof( DungeonTreasureChestFirst ), 2 ),
				new ChestEntry( Map.Trammel, new Point3D( 5387, 1911, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 2 ),
				new ChestEntry( Map.Trammel, new Point3D( 5387, 1911, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 2 ),
				new ChestEntry( Map.Trammel, new Point3D( 5410, 1933, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5410, 1933, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5410, 1933, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ), 

				// Covetous lvl 2 Felucca
				new ChestEntry( Map.Felucca, new Point3D( 5498, 2002, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5498, 2002, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5498, 2002, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ), 
				new ChestEntry( Map.Felucca, new Point3D( 5448, 2026, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5448, 2026, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5448, 2026, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5417, 2000, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5457, 1973, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5457, 1973, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5457, 1973, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ), 

				// Covetous lvl 2 Trammel
				new ChestEntry( Map.Trammel, new Point3D( 5498, 2002, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5498, 2002, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5498, 2002, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ), 
				new ChestEntry( Map.Trammel, new Point3D( 5448, 2026, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5448, 2026, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5448, 2026, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5417, 2000, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5457, 1973, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5457, 1973, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5457, 1973, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ), 

				// Covetous lvl 3 Felucca
				new ChestEntry( Map.Felucca, new Point3D( 5616, 1839, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5616, 1839, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5616, 1839, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),

				// Covetous lvl 3 Trammel
				new ChestEntry( Map.Trammel, new Point3D( 5616, 1839, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5616, 1839, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5616, 1839, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),

				// Covetous undead prison Felucca
				new ChestEntry( Map.Felucca, new Point3D( 5505, 1808, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5505, 1808, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5505, 1808, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),

				// Covetous undead prison Trammel
				new ChestEntry( Map.Trammel, new Point3D( 5505, 1808, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5505, 1808, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5505, 1808, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),

				// Covetous dragon's lair Felucca
				new ChestEntry( Map.Felucca, new Point3D( 5460, 1810, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5460, 1810, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5460, 1810, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),

				// Covetous dragon's lair Trammel
				new ChestEntry( Map.Trammel, new Point3D( 5460, 1810, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5460, 1810, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5460, 1810, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),

				// Destard lvl 1 Felucca
				new ChestEntry( Map.Felucca, new Point3D( 5242, 941, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5242, 941, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5242, 941, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5318, 981, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5318, 981, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5318, 981, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5145, 929, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 15 ),
				new ChestEntry( Map.Felucca, new Point3D( 5145, 929, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 15 ),
				new ChestEntry( Map.Felucca, new Point3D( 5145, 929, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 15 ),
				new ChestEntry( Map.Felucca, new Point3D( 5199, 779, 0 ), 1200, 1800, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5232, 834, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5232, 834, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5232, 834, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),

				// Destard lvl 1 Trammel
				new ChestEntry( Map.Trammel, new Point3D( 5242, 941, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5242, 941, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5242, 941, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5318, 981, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5318, 981, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5318, 981, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5145, 929, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 15 ),
				new ChestEntry( Map.Trammel, new Point3D( 5145, 929, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 15 ),
				new ChestEntry( Map.Trammel, new Point3D( 5145, 929, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 15 ),
				new ChestEntry( Map.Trammel, new Point3D( 5199, 779, 0 ), 1200, 1800, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5232, 834, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5232, 834, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5232, 834, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),

				// Destard lvl 2 Felucca
				new ChestEntry( Map.Felucca, new Point3D( 5152, 834, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 30 ),
				new ChestEntry( Map.Felucca, new Point3D( 5152, 834, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 30 ),

				// Destard lvl 2 Trammel
				new ChestEntry( Map.Trammel, new Point3D( 5152, 834, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 30 ),
				new ChestEntry( Map.Trammel, new Point3D( 5152, 834, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 30 ),

				// Destard lvl 3 Felucca
				new ChestEntry( Map.Felucca, new Point3D( 5145, 988, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5145, 988, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5145, 988, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5185, 1009, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5185, 1009, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5185, 1009, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),

				// Destard lvl 3 Trammel
				new ChestEntry( Map.Trammel, new Point3D( 5145, 988, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5145, 988, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5145, 988, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5185, 1009, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5185, 1009, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5185, 1009, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),

				// Despise lvl 1 Felucca
				new ChestEntry( Map.Felucca, new Point3D( 5503, 530, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5503, 530, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5503, 530, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5390, 525, 0 ), 60, 120, typeof( DungeonTreasureChestFirst ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5390, 525, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5390, 525, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5406, 724, 0 ), 60, 120, typeof( DungeonTreasureChestFirst ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5406, 724, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5406, 724, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5392, 608, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5392, 608, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5392, 608, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),

				// Despise lvl 1 Trammel
				new ChestEntry( Map.Trammel, new Point3D( 5503, 530, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5503, 530, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5503, 530, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5390, 525, 0 ), 60, 120, typeof( DungeonTreasureChestFirst ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5390, 525, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5390, 525, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5406, 724, 0 ), 60, 120, typeof( DungeonTreasureChestFirst ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5406, 724, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5406, 724, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5392, 608, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5392, 608, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5392, 608, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),

				// Despise lvl 2 Felucca
				new ChestEntry( Map.Felucca, new Point3D( 5508, 654, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5508, 654, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5508, 654, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5388, 701, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5388, 701, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5388, 701, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),

				// Despise lvl 2 Trammel
				new ChestEntry( Map.Trammel, new Point3D( 5508, 654, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5508, 654, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5508, 654, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5388, 701, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5388, 701, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5388, 701, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),

				// Despise lvl 3 Felucca
				new ChestEntry( Map.Felucca, new Point3D( 5395, 978, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 7 ),
				new ChestEntry( Map.Felucca, new Point3D( 5395, 978, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 7 ),
				new ChestEntry( Map.Felucca, new Point3D( 5395, 978, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 7 ),
				new ChestEntry( Map.Felucca, new Point3D( 5549, 894, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5549, 894, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5549, 894, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ), 
				new ChestEntry( Map.Felucca, new Point3D( 5558, 824, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5558, 824, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5558, 824, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5599, 803, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5599, 803, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5599, 803, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),

				// Despise lvl 3 Trammel
				new ChestEntry( Map.Trammel, new Point3D( 5395, 978, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 7 ),
				new ChestEntry( Map.Trammel, new Point3D( 5395, 978, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 7 ),
				new ChestEntry( Map.Trammel, new Point3D( 5395, 978, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 7 ),
				new ChestEntry( Map.Trammel, new Point3D( 5549, 894, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5549, 894, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5549, 894, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ), 
				new ChestEntry( Map.Trammel, new Point3D( 5558, 824, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5558, 824, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5558, 824, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5599, 803, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5599, 803, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5599, 803, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),

				// Hythloth lvl 1 Felucca
				new ChestEntry( Map.Felucca, new Point3D( 5910,  55, 0 ), 60, 120, typeof( DungeonTreasureChestHybrid ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5910,  55, 0 ), 60, 120, typeof( DungeonTreasureChestHybrid ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5910,  55, 0 ), 60, 120, typeof( DungeonTreasureChestHybrid ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5910,  55, 0 ), 60, 120, typeof( DungeonTreasureChestHybrid ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5910,  55, 0 ), 60, 120, typeof( DungeonTreasureChestHybrid ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5910,  55, 0 ), 60, 120, typeof( DungeonTreasureChestHybrid ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5930, 81, 25 ), 60, 120, typeof( DungeonTreasureChestFirst ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5911, 103, 25 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5968, 80, 25 ), 60, 120, typeof( DungeonTreasureChestFirst ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5983, 29, 0 ), 60, 120, typeof( DungeonTreasureChestHybrid ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5983, 29, 0 ), 60, 120, typeof( DungeonTreasureChestHybrid ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5983, 29, 0 ), 60, 120, typeof( DungeonTreasureChestHybrid ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5983, 29, 0 ), 60, 120, typeof( DungeonTreasureChestHybrid ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5983, 29, 0 ), 60, 120, typeof( DungeonTreasureChestHybrid ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5983, 29, 0 ), 60, 120, typeof( DungeonTreasureChestHybrid ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5983, 29, 0 ), 60, 120, typeof( DungeonTreasureChestHybrid ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5983, 29, 0 ), 60, 120, typeof( DungeonTreasureChestHybrid ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5983, 29, 0 ), 60, 120, typeof( DungeonTreasureChestHybrid ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5983, 29, 0 ), 60, 120, typeof( DungeonTreasureChestHybrid ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5983, 29, 0 ), 60, 120, typeof( DungeonTreasureChestHybrid ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5983, 29, 0 ), 60, 120, typeof( DungeonTreasureChestHybrid ), 3 ),

				// Hythloth lvl 1 Trammel
				new ChestEntry( Map.Trammel, new Point3D( 5910,  55, 6 ), 60, 120, typeof( DungeonTreasureChestHybrid ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5910,  55, 0 ), 60, 120, typeof( DungeonTreasureChestHybrid ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5910,  55, 0 ), 60, 120, typeof( DungeonTreasureChestHybrid ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5910,  55, 0 ), 60, 120, typeof( DungeonTreasureChestHybrid ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5910,  55, 0 ), 60, 120, typeof( DungeonTreasureChestHybrid ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5910,  55, 0 ), 60, 120, typeof( DungeonTreasureChestHybrid ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5930, 81, 25 ), 60, 120, typeof( DungeonTreasureChestFirst ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5911, 103, 25 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5968, 80, 25 ), 60, 120, typeof( DungeonTreasureChestFirst ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5983, 29, 12 ), 60, 120, typeof( DungeonTreasureChestHybrid ), 3 ),				
				new ChestEntry( Map.Trammel, new Point3D( 5983, 29, 0 ), 60, 120, typeof( DungeonTreasureChestHybrid ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5983, 29, 0 ), 60, 120, typeof( DungeonTreasureChestHybrid ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5983, 29, 0 ), 60, 120, typeof( DungeonTreasureChestHybrid ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5983, 29, 0 ), 60, 120, typeof( DungeonTreasureChestHybrid ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5983, 29, 0 ), 60, 120, typeof( DungeonTreasureChestHybrid ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5983, 29, 0 ), 60, 120, typeof( DungeonTreasureChestHybrid ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5983, 29, 0 ), 60, 120, typeof( DungeonTreasureChestHybrid ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5983, 29, 0 ), 60, 120, typeof( DungeonTreasureChestHybrid ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5983, 29, 0 ), 60, 120, typeof( DungeonTreasureChestHybrid ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5983, 29, 0 ), 60, 120, typeof( DungeonTreasureChestHybrid ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5983, 29, 0 ), 60, 120, typeof( DungeonTreasureChestHybrid ), 3 ),

				// Hythloth lvl 2 Felucca
				new ChestEntry( Map.Felucca, new Point3D( 5973, 169, 0 ), 60, 120, typeof( DungeonTreasureChestFirst ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5973, 169, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5973, 169, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5991, 150, 0 ), 60, 120, typeof( DungeonTreasureChestFirst ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5991, 150, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5991, 150, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5955, 228, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5955, 228, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5955, 228, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5981, 194, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5981, 194, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5981, 194, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5919, 226, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5919, 226, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5919, 226, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),

				// Hythloth lvl 2 Trammel
				new ChestEntry( Map.Trammel, new Point3D( 5973, 169, 0 ), 60, 120, typeof( DungeonTreasureChestFirst ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5973, 169, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5973, 169, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5991, 150, 0 ), 60, 120, typeof( DungeonTreasureChestFirst ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5991, 150, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5991, 150, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5955, 228, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5955, 228, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5955, 228, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5981, 194, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5981, 194, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5981, 194, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5919, 226, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5919, 226, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5919, 226, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),

				// Hythloth lvl 3 Felucca
				new ChestEntry( Map.Felucca, new Point3D( 6090, 181, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 6090, 181, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 6090, 181, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 6054, 159, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 6054, 159, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 6054, 159, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 6039, 199, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 6039, 199, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 6039, 199, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 6124, 163, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 6124, 163, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 6124, 163, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 6120, 220, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 6120, 220, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 6120, 220, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 6048, 222, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 6048, 222, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 6048, 222, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),

				// Hythloth lvl 3 Trammel
				new ChestEntry( Map.Trammel, new Point3D( 6090, 181, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 6090, 181, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 6090, 181, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 6054, 159, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 6054, 159, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 6054, 159, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 6039, 199, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 6039, 199, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 6039, 199, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 6124, 163, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 6124, 163, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 6124, 163, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 6120, 220, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 6120, 220, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 6120, 220, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 6048, 222, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 6048, 222, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 6048, 222, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),

				// Hythloth lvl 4 Felucca
				new ChestEntry( Map.Felucca, new Point3D( 6063, 95, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 6063, 95, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 6063, 95, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 6108, 68, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 6108, 68, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 6108, 68, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 6112, 88, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 6112, 88, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 6112, 88, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 6083, 43, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 6083, 43, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 6083, 43, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 6103, 39, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 6103, 39, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 6103, 39, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),

				// Hythloth lvl 4 Trammel
				new ChestEntry( Map.Trammel, new Point3D( 6063, 95, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 6063, 95, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 6063, 95, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 6108, 68, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 6108, 68, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 6108, 68, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 6112, 88, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 6112, 88, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 6112, 88, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 6083, 43, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 6083, 43, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 6083, 43, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 6103, 39, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 6103, 39, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 6103, 39, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),

				// Ice Felucca
				new ChestEntry( Map.Felucca, new Point3D( 5757, 143, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5757, 143, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5757, 143, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5719, 147, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5719, 147, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5719, 147, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5674, 314, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5674, 314, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5674, 314, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5674, 314, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5756, 207, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5756, 207, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5756, 207, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5768, 187, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5768, 187, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5768, 187, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5835, 243, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5835, 243, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5835, 243, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5851, 233, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5851, 233, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5851, 233, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5830, 357, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5830, 357, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5830, 357, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),

				// Ice Trammel
				new ChestEntry( Map.Trammel, new Point3D( 5757, 143, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5757, 143, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5757, 143, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5719, 147, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5719, 147, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5719, 147, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5674, 314, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5674, 314, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5674, 314, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5674, 314, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5756, 207, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5756, 207, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5756, 207, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5768, 187, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5768, 187, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5768, 187, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5835, 243, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5835, 243, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5835, 243, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5851, 233, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5851, 233, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5851, 233, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5830, 357, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5830, 357, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5830, 357, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),

				// Deceit lvl 1 Felucca
				new ChestEntry( Map.Felucca, new Point3D( 5146, 616, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5146, 616, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5146, 616, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),

				// Deceit lvl 1 Trammel
				new ChestEntry( Map.Trammel, new Point3D( 5146, 616, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5146, 616, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5146, 616, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),

				// Deceit lvl 2 Felucca
				new ChestEntry( Map.Felucca, new Point3D( 5323, 570, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5323, 570, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5323, 570, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),

				// Deceit lvl 2 Trammel
				new ChestEntry( Map.Trammel, new Point3D( 5323, 570, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5323, 570, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5323, 570, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),

				// Deceit lvl 3 Felucca
				new ChestEntry( Map.Felucca, new Point3D( 5151, 742, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5151, 742, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5151, 742, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),

				// Deceit lvl 3 Trammel
				new ChestEntry( Map.Trammel, new Point3D( 5151, 742, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5151, 742, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5151, 742, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),

				// Deceit lvl 4 Felucca
				new ChestEntry( Map.Felucca, new Point3D( 5315, 748, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5315, 748, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5315, 748, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5267, 691, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5267, 691, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5267, 691, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),

				// Deceit lvl 4 Trammel
				new ChestEntry( Map.Trammel, new Point3D( 5315, 748, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5315, 748, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5315, 748, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5267, 691, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5267, 691, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5267, 691, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),

				// Fire lvl 1 Felucca
				new ChestEntry( Map.Felucca, new Point3D( 5678, 1439, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5678, 1439, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5678, 1439, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5712, 1464, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5712, 1464, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5712, 1464, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),

				// Fire lvl 1 Trammel
				new ChestEntry( Map.Trammel, new Point3D( 5678, 1439, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5678, 1439, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5678, 1439, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5712, 1464, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5712, 1464, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5712, 1464, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),

				// Fire lvl 2 Felucca
				new ChestEntry( Map.Felucca, new Point3D( 5723, 1388, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5723, 1388, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5723, 1388, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5647, 1435, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5647, 1435, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5647, 1435, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),

				// Fire lvl 2 Trammel
				new ChestEntry( Map.Trammel, new Point3D( 5723, 1388, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5723, 1388, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5723, 1388, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5647, 1435, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5647, 1435, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5647, 1435, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),

				// Shame lvl 2 Felucca
				new ChestEntry( Map.Felucca, new Point3D( 5608, 19, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5608, 19, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5608, 19, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5543, 115, 0 ), 60, 120, typeof( DungeonTreasureChestFirst ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5543, 115, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5543, 115, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5543, 115, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5543, 115, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5543, 115, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5566, 115, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5566, 115, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5566, 115, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),

				// Shame lvl 2 Trammel
				new ChestEntry( Map.Trammel, new Point3D( 5608, 19, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5608, 19, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5608, 19, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5543, 115, 0 ), 60, 120, typeof( DungeonTreasureChestFirst ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5543, 115, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5543, 115, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5543, 115, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5543, 115, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5543, 115, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5566, 115, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5566, 115, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5566, 115, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),

				// Shame lvl 3 Felucca
				new ChestEntry( Map.Felucca, new Point3D( 5394, 145, 0 ), 60, 120, typeof( DungeonTreasureChestFirst ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5394, 145, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5394, 145, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5593, 140, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5593, 140, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5593, 140, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5477, 185, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5477, 185, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5477, 185, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
																											  															 
				// Shame lvl 3 Trammel
				new ChestEntry( Map.Trammel, new Point3D( 5394, 145, 0 ), 60, 120, typeof( DungeonTreasureChestFirst ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5394, 145, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5394, 145, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5593, 140, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5593, 140, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5593, 140, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5477, 185, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5477, 185, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5477, 185, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),

				// Shame lvl 4 Felucca
				new ChestEntry( Map.Felucca, new Point3D( 5731, 91, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5731, 91, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5731, 91, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5731, 91, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5731, 91, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5731, 91, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5723, 75, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5723, 75, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5723, 75, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5715, 59, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5715, 59, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5715, 59, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5715, 59, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5715, 59, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5715, 59, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5715, 59, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5699, 59, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5699, 59, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5699, 59, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5699, 59, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5699, 59, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5699, 59, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5818, 80, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5818, 80, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5818, 80, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),

				// Shame lvl 4 Trammel
				new ChestEntry( Map.Trammel, new Point3D( 5731, 91, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5731, 91, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5731, 91, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5731, 91, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5731, 91, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5731, 91, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5723, 75, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5723, 75, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5723, 75, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5715, 59, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5715, 59, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5715, 59, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5715, 59, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5715, 59, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5715, 59, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5715, 59, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5699, 59, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5699, 59, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5699, 59, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5699, 59, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5699, 59, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5699, 59, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5818, 80, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5818, 80, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5818, 80, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),

				// Yew Crypts Felucca
				new ChestEntry( Map.Felucca, new Point3D( 1017, 825, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 1017, 825, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 1017, 825, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 1015, 705, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 1015, 705, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 1015, 705, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),

				// Yew Crypts Trammel
				new ChestEntry( Map.Trammel, new Point3D( 1017, 825, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 1017, 825, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 1017, 825, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 1015, 705, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 1015, 705, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 1015, 705, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),

				// Vesper Bank Felucca
				new ChestEntry( Map.Felucca, new Point3D( 2874, 691, 0 ), 60, 120, typeof( DungeonTreasureChestFirst ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 2874, 691, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 2874, 691, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 2883, 691, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 2883, 691, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 2883, 691, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 2875, 675, 0 ), 60, 120, typeof( DungeonTreasureChestHybrid ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 2875, 675, 0 ), 60, 120, typeof( DungeonTreasureChestHybrid ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 2875, 675, 0 ), 60, 120, typeof( DungeonTreasureChestHybrid ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 2875, 675, 0 ), 60, 120, typeof( DungeonTreasureChestHybrid ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 2875, 675, 0 ), 60, 120, typeof( DungeonTreasureChestHybrid ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 2875, 675, 0 ), 60, 120, typeof( DungeonTreasureChestHybrid ), 3 ),

				// Vesper Bank Trammel
				new ChestEntry( Map.Trammel, new Point3D( 2874, 691, 0 ), 60, 120, typeof( DungeonTreasureChestFirst ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 2874, 691, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 2874, 691, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 2883, 691, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 2883, 691, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 2883, 691, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 2875, 675, 0 ), 60, 120, typeof( DungeonTreasureChestHybrid ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 2875, 675, 0 ), 60, 120, typeof( DungeonTreasureChestHybrid ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 2875, 675, 0 ), 60, 120, typeof( DungeonTreasureChestHybrid ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 2875, 675, 0 ), 60, 120, typeof( DungeonTreasureChestHybrid ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 2875, 675, 0 ), 60, 120, typeof( DungeonTreasureChestHybrid ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 2875, 675, 0 ), 60, 120, typeof( DungeonTreasureChestHybrid ), 3 ),

				// Trinsic Passage Felucca
				new ChestEntry( Map.Felucca, new Point3D( 5913, 1303, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5913, 1303, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5913, 1303, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),

				// Trinsic Passage Trammel
				new ChestEntry( Map.Trammel, new Point3D( 5913, 1303, 0 ), 60, 120, typeof( DungeonTreasureChestSecond ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5913, 1303, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5913, 1303, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),

				// Avatar Temple Felucca
				new ChestEntry( Map.Felucca, new Point3D( 4595, 3572, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 4595, 3572, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 4595, 3572, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 4595, 3572, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 4595, 3572, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 4595, 3572, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 4595, 3572, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),

				// Avatar Temple Trammel
				new ChestEntry( Map.Trammel, new Point3D( 4595, 3572, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 4595, 3572, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 4595, 3572, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 4595, 3572, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 4595, 3572, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 4595, 3572, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 4595, 3572, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),

				// Terathan Keep Felucca
				new ChestEntry( Map.Felucca, new Point3D( 5344, 1550, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5344, 1550, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5344, 1550, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5361, 1713, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5361, 1713, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Felucca, new Point3D( 5361, 1713, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),

				// Terathan Keep Trammel
				new ChestEntry( Map.Trammel, new Point3D( 5344, 1550, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5344, 1550, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5344, 1550, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5361, 1713, 0 ), 60, 120, typeof( DungeonTreasureChestThird ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5361, 1713, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
				new ChestEntry( Map.Trammel, new Point3D( 5361, 1713, 0 ), 60, 120, typeof( DungeonTreasureChestFourth ), 3 ),
			};

		public static ChestEntry[] Entries { get { return m_Entries; } }

		private static DungeonChestsSpawner m_Instance;

		public static DungeonChestsSpawner Instance { get { return m_Instance; } }

		public static void Initialize()
		{
			CommandSystem.Register( "ChestsGenerate", AccessLevel.Developer, new CommandEventHandler( ChestsGenerate_OnCommand ) );
			CommandSystem.Register( "ChestsDelete", AccessLevel.Developer, new CommandEventHandler( ChestsDelete_OnCommand ) );
		}

		[Usage( "ChestsGenerate" )]
		[Description( "Generates the dungeon treasure chests spawner." )]
		private static void ChestsGenerate_OnCommand( CommandEventArgs args )
		{
			Mobile from = args.Mobile;

			if ( Create() )
				from.SendMessage( "Dungeon treasure chests spawner generated." );
			else
				from.SendMessage( "Dungeon treasure chests spawner already present." );
		}

		[Usage( "ChestsDelete" )]
		[Description( "Removes the dungeon treasure chests spawner." )]
		private static void ChestsDelete_OnCommand( CommandEventArgs args )
		{
			Mobile from = args.Mobile;

			if ( Remove() )
				from.SendMessage( "Dungeon treasure chests spawner removed." );
			else
				from.SendMessage( "Dungeon treasure chests spawner not present." );
		}

		public static bool Create()
		{
			if ( m_Instance != null && !m_Instance.Deleted )
				return false;

			m_Instance = new DungeonChestsSpawner();
			return true;
		}

		public static bool Remove()
		{
			if ( m_Instance == null )
				return false;

			m_Instance.Delete();
			m_Instance = null;
			return true;
		}

		public static ChestInstance GetChestInstance( Item item )
		{
			if ( Instance == null )
				return null;

			return (ChestInstance) Instance.m_Table[item];
		}

		public class ChestInstance
		{
			private ChestEntry m_Entry;
			private Item m_Item;
			private DateTime m_NextRespawn;

			public ChestEntry Entry { get { return m_Entry; } }

			public Item Item
			{
				get { return m_Item; }
				set
				{
					if ( m_Item != null && value == null )
					{
						int delay = Utility.RandomMinMax( this.Entry.MinDelay * 2, this.Entry.MaxDelay * 2 );
						this.NextRespawn = DateTime.Now + TimeSpan.FromSeconds( delay );
					}

					if ( Instance != null )
					{
						if ( m_Item != null )
							Instance.m_Table.Remove( m_Item );

						if ( value != null )
							Instance.m_Table[value] = this;
					}

					m_Item = value;
				}
			}

			public DateTime NextRespawn { get { return m_NextRespawn; } set { m_NextRespawn = value; } }

			public ChestInstance( ChestEntry entry )
				: this( entry, null, DateTime.Now )
			{
			}

			public ChestInstance( ChestEntry entry, Item item, DateTime nextRespawn )
			{
				m_Item = item;
				m_NextRespawn = nextRespawn;
				m_Entry = entry;
			}

			public void CheckRespawn()
			{
				if ( DateTime.Now >= this.NextRespawn )
				{
					if ( this.Item != null )
						this.Item.Delete();

					this.Item = this.Entry.CreateInstance();
				}
			}
		}

		private Timer m_RespawnTimer;
		private ChestInstance[] m_Artifacts;
		private Hashtable m_Table;

		private DungeonChestsSpawner()
			: base( 1 )
		{
			Name = "Dungeon Treasure Chests Spawner - Internal";
			Movable = false;

			m_Artifacts = new ChestInstance[m_Entries.Length];
			m_Table = new Hashtable( m_Entries.Length );

			for ( int i = 0; i < m_Entries.Length; i++ )
				m_Artifacts[i] = new ChestInstance( m_Entries[i] );

			m_RespawnTimer = Timer.DelayCall( TimeSpan.Zero, TimeSpan.FromMinutes( 7.0 ), new TimerCallback( CheckRespawn ) );
		}

		public override void OnDelete()
		{
			base.OnDelete();

			if ( m_RespawnTimer != null )
			{
				m_RespawnTimer.Stop();
				m_RespawnTimer = null;
			}

			foreach ( ChestInstance ci in m_Artifacts )
			{
				if ( ci.Item != null )
					ci.Item.Delete();
			}

			m_Instance = null;
		}

		public void CheckRespawn()
		{
			foreach ( ChestInstance ci in m_Artifacts )
				ci.CheckRespawn();
		}

		public DungeonChestsSpawner( Serial serial )
			: base( serial )
		{
			m_Instance = this;
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version

			writer.WriteEncodedInt( m_Artifacts.Length );

			for ( int i = 0; i < m_Artifacts.Length; i++ )
			{
				ChestInstance ci = m_Artifacts[i];

				writer.Write( (Item) ci.Item );
				writer.WriteDeltaTime( (DateTime) ci.NextRespawn );
			}
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadEncodedInt();

			m_Artifacts = new ChestInstance[m_Entries.Length];
			m_Table = new Hashtable( m_Entries.Length );

			int length = reader.ReadEncodedInt();

			for ( int i = 0; i < length; i++ )
			{
				Item item = reader.ReadItem();
				DateTime nextRespawn = reader.ReadDeltaTime();

				if ( i < m_Artifacts.Length )
				{
					ChestInstance ci = new ChestInstance( m_Entries[i], item, nextRespawn );
					m_Artifacts[i] = ci;

					if ( ci.Item != null )
						m_Table[ci.Item] = ci;
				}
			}

			for ( int i = length; i < m_Entries.Length; i++ )
				m_Artifacts[i] = new ChestInstance( m_Entries[i] );

			m_RespawnTimer = Timer.DelayCall( TimeSpan.Zero, TimeSpan.FromMinutes( 7.0 ), new TimerCallback( CheckRespawn ) );
		}
	}
}