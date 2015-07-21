using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Items
{
	public enum TalismanType
	{
		None,
		CurseRemoval,
		DamageRemoval,
		WardRemoval,
		WildfireRemoval,
		SummonBandage,
		SummonBoard,
		SummonIngot,
		SummonRandom,
		SummonAntLion,
		SummonCow,
		SummonLavaSerpent,
		SummonOrcBrute,
		SummonFrostSpider,
		SummonPanther,
		SummonDoppleganger,
		SummonGreatHart,
		SummonBullfrog,
		SummonArcticOgreLord,
		SummonBogling,
		SummonBakeKitsune,
		SummonSheep,
		SummonSkeletalKnight,
		SummonWailingBanshee,
		SummonChicken,
		SummonVorpalBunny,
		ManaPhase
	}

	public class SummonEntry
	{
		private static SummonEntry[] m_Table = new SummonEntry[]
		{
			new SummonEntry( TalismanType.SummonAntLion, typeof( SummonAntLion ) ),
			new SummonEntry( TalismanType.SummonCow, typeof( SummonCow ) ),
			new SummonEntry( TalismanType.SummonLavaSerpent, typeof( SummonLavaSerpent ) ),
			new SummonEntry( TalismanType.SummonOrcBrute, typeof( SummonOrcBrute ) ),
			new SummonEntry( TalismanType.SummonFrostSpider, typeof( SummonFrostSpider ) ),
			new SummonEntry( TalismanType.SummonPanther, typeof( SummonPanther ) ),
			new SummonEntry( TalismanType.SummonDoppleganger, typeof( SummonDoppleganger ) ),
			new SummonEntry( TalismanType.SummonGreatHart, typeof( SummonGreatHart ) ),
			new SummonEntry( TalismanType.SummonBullfrog, typeof( SummonBullFrog ) ),
			new SummonEntry( TalismanType.SummonArcticOgreLord, typeof( SummonArcticOgreLord ) ),
			new SummonEntry( TalismanType.SummonBogling, typeof( SummonBogling ) ),
			new SummonEntry( TalismanType.SummonBakeKitsune, typeof( SummonBakeKitsune ) ),
			new SummonEntry( TalismanType.SummonSheep, typeof( SummonSheep ) ),
			new SummonEntry( TalismanType.SummonSkeletalKnight, typeof( SummonSkeletalKnight ) ),
			new SummonEntry( TalismanType.SummonWailingBanshee, typeof( SummonWailingBanshee ) ),
			new SummonEntry( TalismanType.SummonChicken, typeof( SummonChicken ) ),
			new SummonEntry( TalismanType.SummonVorpalBunny, typeof( SummonVorpalBunny ) )
		};

		public static SummonEntry[] SummonTable { get { return m_Table; } set { m_Table = value; } }

		private TalismanType m_Name;
		public TalismanType Name { get { return m_Name; } set { m_Name = value; } }

		private Type m_NPC;
		public Type NPC { get { return m_NPC; } set { m_NPC = value; } }

		public SummonEntry( TalismanType name, Type npc )
		{
			m_Name = name;
			m_NPC = npc;
		}

		public static TalismanType GetRandom()
		{
			int index = Utility.Random( m_Table.Length );
			return m_Table[index].Name;
		}

		public static Type GetNPC( TalismanType name )
		{
			return m_Table[( (int) name - 9 )].m_NPC;
		}
	}
}