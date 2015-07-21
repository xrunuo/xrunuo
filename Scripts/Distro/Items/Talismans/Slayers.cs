using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Items
{
	public enum TalisSlayerName
	{
		None,
		Bat,
		Bear,
		Beetle,
		Bird,
		Bovine,
		Flame,
		Ice,
		Mage,
		Vermin,
		Wolf,
		Goblin
	}

	public class TalisSlayerEntry
	{
		private static TalisSlayerEntry[] m_Table = new TalisSlayerEntry[]
		{
			new TalisSlayerEntry( TalisSlayerName.Bat, 1072506, typeof( Mongbat ), typeof( StrongMongbat ), typeof( VampireBat ) ),
			new TalisSlayerEntry( TalisSlayerName.Bear, 1072504, typeof( BlackBear ), typeof( BrownBear ), typeof( GrizzlyBear ), typeof( PolarBear ), typeof( Grobu ) ),
			new TalisSlayerEntry( TalisSlayerName.Beetle, 1072508, typeof( DeathWatchBeetle ), typeof( DeathWatchBeetleHatchling ), typeof( FireBeetle ), typeof( Beetle ), typeof( RuneBeetle ) ),
			new TalisSlayerEntry( TalisSlayerName.Bird, 1072509, typeof( Bird ), typeof( Chicken ), typeof( Crane ), typeof( DesertOstard ), typeof( ForestOstard ), typeof( FrenziedOstard ), typeof( Eagle ), typeof( Phoenix ), typeof( Swoop ), typeof( Pyre ) ),
			new TalisSlayerEntry( TalisSlayerName.Bovine, 1072512, typeof( Bull ), typeof( Cow ), typeof( Gaman ), typeof( MinotaurScout ), typeof( MinotaurCaptain ), typeof( Minotaur ), typeof( LowlandBoura ), typeof( RuddyBoura ), typeof( HighPlainsBoura ), typeof( TormentedMinotaur ), typeof( Meraktus ) ), 
			new TalisSlayerEntry( TalisSlayerName.Flame, 1072511, typeof( FireBeetle ), typeof( FireElemental ), typeof( FireSteed ), typeof( HellHound ), typeof( HellCat ), typeof( PredatorHellCat ), typeof( LavaLizard ), typeof( LavaSerpent ), typeof( LavaSnake ), typeof( FireRabbit ), typeof( Pyre )),
			new TalisSlayerEntry( TalisSlayerName.Ice, 1072510, typeof( FrostOoze ), typeof( FrostSpider ), typeof( FrostTroll ), typeof( GiantIceWorm ), typeof( IceSerpent ), typeof( IceElemental ), typeof( IceFiend ), typeof( IceSnake ), typeof( LadyOfTheSnow ), typeof( PolarBear ), typeof( SnowElemental ), typeof( SnowLeopard ) ),
			new TalisSlayerEntry( TalisSlayerName.Mage, 1072507, typeof( AncientLich ), typeof( BoneMagi ), typeof( EvilMage ), typeof( EvilMageLord ), typeof( JukaMage ), typeof( Lich ), typeof( LichLord ), typeof( OrcishMage ), typeof( SkeletalMage ), typeof( KhaldunSummoner ), typeof( MasterTheophilus ), typeof( CursedMetallicMage ), typeof( GrayGoblinMage ) ),
			new TalisSlayerEntry( TalisSlayerName.Vermin, 1072505, typeof( Rat ), typeof( GiantRat ), typeof( Ratman ), typeof( RatmanArcher ), typeof( RatmanMage ), typeof( Sewerrat ), typeof( Barracoon ), typeof( Chiikkaha ), typeof( ClanRibbonPlagueRat ), typeof( ClanRibbonSupplicant ), typeof( ClanRibbonCourtier ), typeof( VitaviRenowned ), typeof( ClanChitterAssistant ), typeof( ClanChitterTinkerer ), typeof( RakktaviRenowned ), typeof( ClanScratchHenchrat ), typeof( ClanScratchScrounger ), typeof( TikitaviRenowned ) ),
			new TalisSlayerEntry( TalisSlayerName.Wolf, 1075462, typeof( BakeKitsune ), typeof( DarkWolfFamiliar ), typeof( DireWolf ), typeof( GreyWolf ), typeof( TimberWolf ), typeof( TsukiWolf ), typeof( WhiteWolf ) ),
			new TalisSlayerEntry( TalisSlayerName.Goblin, 1095010, typeof( GrayGoblin ), typeof( GrayGoblinKeeper ), typeof( GrayGoblinMage ), typeof( GreenGoblin ), typeof( GreenGoblinAlchemist ), typeof( GreenGoblinScout ), typeof( EnslavedGoblinAlchemist ), typeof( EnslavedGoblinKeeper ), typeof( EnslavedGoblinMage ), typeof( EnslavedGoblinScout ), typeof( EnslavedGrayGoblin ), typeof( EnslavedGreenGoblin ), typeof( GrayGoblinMageRenowned ), typeof( GreenGoblinAlchemistRenowned ) )
		};

		public static TalisSlayerEntry[] TalisSlayerTable { get { return m_Table; } set { m_Table = value; } }

		private TalisSlayerName m_Name;
		public TalisSlayerName Name { get { return m_Name; } set { m_Name = value; } }

		private int m_Title;
		public int Title { get { return m_Title; } set { m_Title = value; } }

		private Type[] m_NPCs;
		public Type[] NPCs { get { return m_NPCs; } set { m_NPCs = value; } }

		public TalisSlayerEntry( TalisSlayerName name, int title, params Type[] npcs )
		{
			m_Name = name;
			m_Title = title;
			m_NPCs = npcs;
		}

		public static bool IsSlayer( TalisSlayerName npcname, Mobile m )
		{
			if ( npcname == TalisSlayerName.None )
				return false;

			int v = (int) npcname - 1;

			if ( v >= 0 && v < m_Table.Length )
			{
				Type t = m.GetType();

				for ( int i = 0; i < m_Table[v].m_NPCs.Length; ++i )
				{
					if ( m_Table[v].m_NPCs[i] == t )
						return true;
				}
			}

			return false;
		}

		public static int GetSlayerTitle( TalisSlayerName npcname )
		{
			if ( npcname == TalisSlayerName.None )
				return 0;

			int v = (int) npcname - 1;

			if ( v >= 0 && v < m_Table.Length )
				return m_Table[v].m_Title;

			return 0;
		}

		public static TalisSlayerName GetRandom()
		{
			int index = Utility.Random( m_Table.Length - 1 );
			return m_Table[index].Name;
		}
	}
}