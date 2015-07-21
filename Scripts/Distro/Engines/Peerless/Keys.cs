using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Items
{

	public class PeerlessEntry
	{
		private static PeerlessEntry[] m_Table = new PeerlessEntry[]
		{
			new PeerlessEntry( PeerlessList.DreadHorn, 6, typeof(DreadHornActivation), "Essence Of The Wind", typeof( DreadHorn ), Map.Ilshenar, new Point3D(2138, 1248, -60), new Point3D(1311, 928, 6), typeof( BlightedCotton ), typeof( GnawsFang ), typeof( IrksBrain ), typeof( LissithsSilk ), typeof( SabrixsEye ), typeof( ThornyBriar ) ),
			new PeerlessEntry( PeerlessList.MelisandeTrammel, 1, typeof(MelisandeActivationTrammel), "Dryad's Curse", typeof( LadyMelisande ), Map.Trammel, new Point3D(6492, 946, 16), new Point3D(577, 1640, 0), typeof( DryadsBlessing ) ),
			new PeerlessEntry( PeerlessList.MelisandeFelucca, 1, typeof(MelisandeActivationFelucca), "Dryad's Curse", typeof( LadyMelisande ), Map.Felucca, new Point3D(6492, 946, 16), new Point3D(577, 1640, 0), typeof( DryadsBlessing ) ),
			new PeerlessEntry( PeerlessList.Travesty, 3, typeof(BlackOrderKey), "Black Order Key", typeof( Travesty ), Map.Malas, new Point3D(87, 1955, 0), new Point3D(1355, 779, 17), typeof( DragonFlameKey ), typeof(SerpentFangKey), typeof(TigerClawKey) ),
			new PeerlessEntry( PeerlessList.InterredGrizzle, 1, typeof(MasterKey), "Master Key", typeof( MonstrousInterredGrizzle ), Map.Malas, new Point3D(178, 1734, 90), new Point3D(2068, 1372, -75), typeof( LibrariansKey ) ),
			new PeerlessEntry( PeerlessList.ParoxysmusTrammel, 4, typeof(SlimyOintmentTrammel), "Slimy Ointment", typeof( ChiefParoxysmus ), Map.Trammel, new Point3D(6517, 357, 0), new Point3D(5623, 3038, 15), typeof( CoagulatedLegs ), typeof( GelatanousSkull ), typeof( PartiallyDigestedTorso ), typeof( SpleenOfThePutrefier ) ),
			new PeerlessEntry( PeerlessList.ParoxysmusFelucca, 4, typeof(SlimyOintmentFelucca), "Slimy Ointment", typeof( ChiefParoxysmus ), Map.Felucca, new Point3D(6517, 357, 0), new Point3D(5623, 3038, 15), typeof( CoagulatedLegs ), typeof( GelatanousSkull ), typeof( PartiallyDigestedTorso ), typeof( SpleenOfThePutrefier ) ),
            new PeerlessEntry( PeerlessList.ShimmeringEffusionTrammel, 6, typeof(ShimmeringEffusionActivationTrammel), "Master Key", typeof( ShimmeringEffusion ), Map.Trammel, new Point3D(6511, 117, -20), new Point3D(3785, 1107, 20), typeof( BrokenCrystals ), typeof( CrushedCrystals ), typeof ( JaggedCrystals ), typeof( PiecesOfCrystal ), typeof( ScatteredCrystals ), typeof( ShatteredCrystals ) ),
			new PeerlessEntry( PeerlessList.ShimmeringEffusionFelucca, 6, typeof(ShimmeringEffusionActivationFelucca), "Master Key", typeof( ShimmeringEffusion ), Map.Felucca, new Point3D(6511, 117, -20), new Point3D(3785, 1107, 20), typeof( BrokenCrystals ), typeof( CrushedCrystals ), typeof ( JaggedCrystals ), typeof( PiecesOfCrystal ), typeof( ScatteredCrystals ), typeof( ShatteredCrystals ) ),
			

			new PeerlessEntry( PeerlessList.Dummy, 1, typeof(DreadHornActivation), "Dryad's Curse", typeof( Dragon ), Map.Felucca, new Point3D(2138, 1248, -60), new Point3D(2174, 1261, -60), typeof( Robe ) ),
		};

		public static PeerlessEntry[] PeerlessTable { get { return m_Table; } set { m_Table = value; } }

		private PeerlessList m_Name;
		public PeerlessList Name { get { return m_Name; } set { m_Name = value; } }

		private int m_TotalKeys;
		public int TotalKeys { get { return m_TotalKeys; } set { m_TotalKeys = value; } }

		private Type m_Key;
		public Type Key { get { return m_Key; } set { m_Key = value; } }

		private String m_LabelNum;
		public String LabelNum { get { return m_LabelNum; } set { m_LabelNum = value; } }

		private Type m_Boss;
		public Type Boss { get { return m_Boss; } set { m_Boss = value; } }

		private Map m_Map;
		public Map SpawnMap { get { return m_Map; } set { m_Map = value; } }

		private Point3D m_SpawnPoint;
		public Point3D SpawnPoint { get { return m_SpawnPoint; } set { m_SpawnPoint = value; } }

		private Point3D m_ExitPoint;
		public Point3D ExitPoint { get { return m_ExitPoint; } set { m_ExitPoint = value; } }

		private Type[] m_KEYs;
		public Type[] KEYs { get { return m_KEYs; } set { m_KEYs = value; } }

		public PeerlessEntry( PeerlessList name, int totalkeys, Type key, String labelnum, Type boss, Map map, Point3D spawnpoint, Point3D exitpoint, params Type[] keys )
		{
			m_Name = name;
			m_TotalKeys = totalkeys;
			m_Key = key;
			m_LabelNum = labelnum;
			m_Boss = boss;
			m_Map = map;
			m_SpawnPoint = spawnpoint;
			m_ExitPoint = exitpoint;
			m_KEYs = keys;
		}

		public static bool IsPeerlessKey( PeerlessList altar, Item key )
		{
			if ( altar == PeerlessList.None )
				return false;

			int v = (int) altar - 1;

			if ( v >= 0 && v < m_Table.Length )
			{
				Type t = key.GetType();

				for ( int i = 0; i < m_Table[v].m_KEYs.Length; ++i )
				{
					if ( m_Table[v].m_KEYs[i] == t )
						return true;
				}
			}
			return false;
		}

		public static int GetAltarKeys( PeerlessList altar )
		{
			if ( altar == PeerlessList.None )
				return 0;

			int v = (int) altar - 1;

			if ( v >= 0 && v < m_Table.Length )
				return m_Table[v].m_TotalKeys;
			return 0;
		}

		public static String GetLabelNum( PeerlessList altar )
		{
			if ( altar == PeerlessList.None )
				return null;

			int v = (int) altar - 1;

			if ( v >= 0 && v < m_Table.Length )
				return m_Table[v].m_LabelNum;
			return null;
		}

		public static Type GetKey( PeerlessList altar )
		{
			if ( altar == PeerlessList.None )
				return null;

			int v = (int) altar - 1;

			if ( v >= 0 && v < m_Table.Length )
				return m_Table[v].m_Key;
			return null;
		}

		public static Point3D GetSpawnPoint( PeerlessList altar )
		{
			if ( altar == PeerlessList.None )
				return new Point3D( 2170, 1254, -60 );

			int v = (int) altar - 1;

			if ( v >= 0 && v < m_Table.Length )
				return m_Table[v].m_SpawnPoint;

			return new Point3D( 2170, 1254, -60 );
		}

		public static Point3D GetExitPoint( PeerlessList altar )
		{
			if ( altar == PeerlessList.None )
				return new Point3D( 2170, 1254, -60 );

			int v = (int) altar - 1;

			if ( v >= 0 && v < m_Table.Length )
				return m_Table[v].m_ExitPoint;

			return new Point3D( 2170, 1254, -60 );
		}

		public static Map GetMap( PeerlessList altar )
		{
			if ( altar == PeerlessList.None )
				return Map.Ilshenar;

			int v = (int) altar - 1;

			if ( v >= 0 && v < m_Table.Length )
				return m_Table[v].m_Map;

			return Map.Ilshenar;
		}
		public static Type GetBoss( PeerlessList altar )
		{
			if ( altar == PeerlessList.None )
				return null;

			int v = (int) altar - 1;

			if ( v >= 0 && v < m_Table.Length )
				return m_Table[v].m_Boss;

			return null;
		}
	}
}
