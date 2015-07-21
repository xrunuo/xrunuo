using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Items
{
	public enum NPC_Name
	{
		None,
		Orc,
		OrcCaptain,
		OrcBomber,
		OrcBrute,
		Rat,
		GiantRat,
		Ratman,
		RatmanMage,
		RatmanArcher,
		GiantSpider,
		FrostSpider,
		GiantBlackWidow,
		DreadSpider,
		SilverSerpent,
		DeepSeaSerpent,
		GiantSerpent,
		Snake,
		IceSnake,
		GiantIceSerpent,
		LavaSerpent,
		LavaSnake,
		Yamandon,
		Mongbat,
		VampireBat,
		Lich,
		EvilMage,
		LichLord,
		EvilMageLord,
		SkeletalMage,
		AncientLich,
		JukaMage,
		GiantBeetle,
		DeathwatchBeetle,
		RuneBeetle,
		FireBeetle,
		DeathwatchBeetleHatchling,
		Phoenix,
		FrenziedOstard,
		FrostTroll,
		IceElemental,
		SnowElemental,
		GiantIceWorm,
		FireElemental,
		HellHound,
		HellCat,
		LavaLizard,
		Dragon,
		Drake,
		Daemon,
		IceFiend,
		Balron,
		BloodElemental,
		AcidElemental,
		PoisonElemental,
		Succubus,
		Wyvern,
		AncientWyrm,
		WhiteWyrm,
		ShadowWyrm,
		FireGargoyle,
		Gargoyle,
		GargoyleDestroyer,
		GargoyleEnforcer,
		EnslavedGargoyle,
		OphidianArchmage,
		OphidianKnight,
		OphidianMage,
		OphidianMatriarch,
		OphidianWarrior,
		TerathanAvenger,
		TerathanDrone,
		TerathanMatriarch,
		TerathanWarrior,
		Cyclops,
		Ettin,
		MeerCaptain,
		MeerEternal,
		MeerMage,
		MeerWarrior,
		Ogre,
		OgreLord,
		Titan,
		Troll,
		BoneKnight,
		BoneMagi,
		LadyOfTheSnow,
		Hiryu,
		LesserHiryu,
		Mummy,
		RottingCorpse,
		AirElemental,
		CrystalElemental,
		EarthElemental,
		Efreet,
		KazeKemono,
		Devourer,
		FanDancer,
		Imp,
		Oni,
		StoneGargoyle,
		Scorpion,
		SkeletalDragon,
		SerpentineDragon
	}

	public class ProtectionKillerEntry
	{
		private static ProtectionKillerEntry[] m_Table = new ProtectionKillerEntry[]
		{
			new ProtectionKillerEntry( NPC_Name.Orc, "Orc", typeof( Orc ) ),
			new ProtectionKillerEntry( NPC_Name.OrcCaptain, "Orc Captain", typeof( OrcCaptain ) ),
			new ProtectionKillerEntry( NPC_Name.OrcBomber, "Orc Bomber", typeof( OrcBomber ) ),
			new ProtectionKillerEntry( NPC_Name.OrcBrute, "Orc Brute", typeof( OrcBrute ) ),
			new ProtectionKillerEntry( NPC_Name.Rat, "Rat", typeof( Rat ) ),
			new ProtectionKillerEntry( NPC_Name.GiantRat, "Giant Rat", typeof( GiantRat ) ),
			new ProtectionKillerEntry( NPC_Name.Ratman, "Ratman", typeof( Ratman ) ),
			new ProtectionKillerEntry( NPC_Name.RatmanMage, "Ratman Mage", typeof( RatmanMage ) ),
			new ProtectionKillerEntry( NPC_Name.RatmanArcher, "Ratman Archer", typeof( RatmanArcher ) ),
			new ProtectionKillerEntry( NPC_Name.GiantSpider, "Giant Spider", typeof( GiantSpider ) ),
			new ProtectionKillerEntry( NPC_Name.FrostSpider, "Frost Spider", typeof( FrostSpider ) ),
			new ProtectionKillerEntry( NPC_Name.GiantBlackWidow, "Giant Black Widow", typeof( GiantBlackWidow ) ),
			new ProtectionKillerEntry( NPC_Name.DreadSpider, "Dread Spider", typeof( DreadSpider ) ),
			new ProtectionKillerEntry( NPC_Name.SilverSerpent, "Silver Serpent", typeof( SilverSerpent ) ),
			new ProtectionKillerEntry( NPC_Name.DeepSeaSerpent, "Deep Sea Serpent", typeof( DeepSeaSerpent ) ),
			new ProtectionKillerEntry( NPC_Name.GiantSerpent, "Giant Serpent", typeof( GiantSerpent ) ),
			new ProtectionKillerEntry( NPC_Name.Snake, "Snake", typeof( Snake ) ),
			new ProtectionKillerEntry( NPC_Name.IceSnake, "Ice Snake", typeof( IceSnake ) ),
			new ProtectionKillerEntry( NPC_Name.GiantIceSerpent, "Giant Ice Serpent", typeof( IceSerpent ) ),
			new ProtectionKillerEntry( NPC_Name.LavaSerpent, "Lava Serpent", typeof( LavaSerpent ) ),
			new ProtectionKillerEntry( NPC_Name.LavaSnake, "Lava Snake", typeof( LavaSnake ) ),
			new ProtectionKillerEntry( NPC_Name.Yamandon, "Yamandon", typeof( Yamandon ) ),
			new ProtectionKillerEntry( NPC_Name.Mongbat, "Mongbat", typeof( Mongbat ) ),
			new ProtectionKillerEntry( NPC_Name.VampireBat, "Vampire Bat", typeof( VampireBat ) ),
			new ProtectionKillerEntry( NPC_Name.Lich, "Lich", typeof( Lich ) ),
			new ProtectionKillerEntry( NPC_Name.EvilMage, "Evil Mage", typeof( EvilMage ) ),
			new ProtectionKillerEntry( NPC_Name.LichLord, "Lich Lord", typeof( LichLord ) ),
			new ProtectionKillerEntry( NPC_Name.EvilMageLord, "Evil Mage Lord", typeof( EvilMageLord ) ),
			new ProtectionKillerEntry( NPC_Name.SkeletalMage, "Skeletal Mage", typeof( SkeletalMage ) ),
			new ProtectionKillerEntry( NPC_Name.AncientLich, "Ancient Lich", typeof( AncientLich ) ),
			new ProtectionKillerEntry( NPC_Name.JukaMage, "Juka Mage", typeof( JukaMage ) ),
			new ProtectionKillerEntry( NPC_Name.GiantBeetle, "Giant Beetle", typeof( Beetle ) ),
			new ProtectionKillerEntry( NPC_Name.DeathwatchBeetle, "Deathwatch Beetle", typeof( DeathWatchBeetle ) ),
			new ProtectionKillerEntry( NPC_Name.RuneBeetle, "Rune Beetle", typeof( RuneBeetle ) ),
			new ProtectionKillerEntry( NPC_Name.FireBeetle, "Fire Beetle", typeof( FireBeetle ) ),
			new ProtectionKillerEntry( NPC_Name.DeathwatchBeetleHatchling, "Deathwatch Beetle Hatchling", typeof( DeathWatchBeetleHatchling ) ),
			new ProtectionKillerEntry( NPC_Name.Phoenix, "Phoenix", typeof( Phoenix ) ),
			new ProtectionKillerEntry( NPC_Name.FrenziedOstard, "Frenzied Ostard", typeof( FrenziedOstard ) ),
			new ProtectionKillerEntry( NPC_Name.FrostTroll, "Frost Troll", typeof( FrostTroll ) ),
			new ProtectionKillerEntry( NPC_Name.IceElemental, "Ice Elemental", typeof( IceElemental ) ),
			new ProtectionKillerEntry( NPC_Name.SnowElemental, "Snow Elemental", typeof( SnowElemental ) ),
			new ProtectionKillerEntry( NPC_Name.GiantIceWorm, "Giant IceWorm", typeof( GiantIceWorm ) ),
			new ProtectionKillerEntry( NPC_Name.FireElemental, "Fire Elemental", typeof( FireElemental ) ),
			new ProtectionKillerEntry( NPC_Name.HellHound, "Hell Hound", typeof( HellHound ) ),
			new ProtectionKillerEntry( NPC_Name.HellCat, "Hell Cat", typeof( HellCat ) ),
			new ProtectionKillerEntry( NPC_Name.LavaLizard, "Lava Lizard", typeof( LavaLizard ) ),
			new ProtectionKillerEntry( NPC_Name.Dragon, "Dragon", typeof( Dragon ) ),
			new ProtectionKillerEntry( NPC_Name.Drake, "Drake", typeof( Drake ) ),
			new ProtectionKillerEntry( NPC_Name.Daemon, "Daemon", typeof( Daemon ) ),
			new ProtectionKillerEntry( NPC_Name.IceFiend, "Ice Fiend", typeof( IceFiend ) ),
			new ProtectionKillerEntry( NPC_Name.Balron, "Balron", typeof( Balron ) ),
			new ProtectionKillerEntry( NPC_Name.BloodElemental, "Blood Elemental", typeof( BloodElemental ) ),
			new ProtectionKillerEntry( NPC_Name.AcidElemental, "Acid Elemental", typeof( ToxicElemental ) ),
			new ProtectionKillerEntry( NPC_Name.PoisonElemental, "Poison Elemental", typeof( PoisonElemental ) ),
			new ProtectionKillerEntry( NPC_Name.Succubus, "Succubus", typeof( Succubus ) ),
			new ProtectionKillerEntry( NPC_Name.Wyvern, "Wyvern", typeof( Wyvern ) ),
			new ProtectionKillerEntry( NPC_Name.AncientWyrm, "Ancient Wyrm", typeof( AncientWyrm ) ),
			new ProtectionKillerEntry( NPC_Name.WhiteWyrm, "White Wyrm", typeof( WhiteWyrm ) ),
			new ProtectionKillerEntry( NPC_Name.ShadowWyrm, "Shadow Wyrm", typeof( ShadowWyrm ) ),
			new ProtectionKillerEntry( NPC_Name.FireGargoyle, "Fire Gargoyle", typeof( FireGargoyle ) ),
			new ProtectionKillerEntry( NPC_Name.Gargoyle, "Gargoyle", typeof( Gargoyle ) ),
			new ProtectionKillerEntry( NPC_Name.GargoyleDestroyer, "Gargoyle Destroyer", typeof( GargoyleDestroyer ) ),
			new ProtectionKillerEntry( NPC_Name.GargoyleEnforcer, "Gargoyle Enforcer", typeof( GargoyleEnforcer ) ),
			new ProtectionKillerEntry( NPC_Name.EnslavedGargoyle, "Enslaved Gargoyle", typeof( EnslavedGargoyle ) ),
			new ProtectionKillerEntry( NPC_Name.OphidianArchmage, "Ophidian Archmage", typeof( OphidianArchmage ) ),
			new ProtectionKillerEntry( NPC_Name.OphidianKnight, "Ophidian Knight", typeof( OphidianKnight ) ),
			new ProtectionKillerEntry( NPC_Name.OphidianMage, "Ophidian Mage", typeof( OphidianMage ) ),
			new ProtectionKillerEntry( NPC_Name.OphidianMatriarch, "Ophidian Matriarch", typeof( OphidianMatriarch ) ),
			new ProtectionKillerEntry( NPC_Name.OphidianWarrior, "Ophidian Warrior", typeof( OphidianWarrior ) ),
			new ProtectionKillerEntry( NPC_Name.TerathanAvenger, "Terathan Avenger", typeof( TerathanAvenger ) ),
			new ProtectionKillerEntry( NPC_Name.TerathanDrone, "Terathan Drone", typeof( TerathanDrone ) ),
			new ProtectionKillerEntry( NPC_Name.TerathanMatriarch, "Terathan Matriarch", typeof( TerathanMatriarch ) ),
			new ProtectionKillerEntry( NPC_Name.TerathanWarrior, "Terathan Warrior", typeof( TerathanWarrior ) ),
			new ProtectionKillerEntry( NPC_Name.Cyclops, "Cyclops", typeof( Cyclops ) ),
			new ProtectionKillerEntry( NPC_Name.Ettin, "Ettin", typeof( Ettin ) ),
			new ProtectionKillerEntry( NPC_Name.MeerCaptain, "Meer Captain", typeof( MeerCaptain ) ),
			new ProtectionKillerEntry( NPC_Name.MeerEternal, "Meer Eternal", typeof( MeerEternal ) ),
			new ProtectionKillerEntry( NPC_Name.MeerMage, "Meer Mage", typeof( MeerMage ) ),
			new ProtectionKillerEntry( NPC_Name.MeerWarrior, "Meer Warrior", typeof( MeerWarrior ) ),
			new ProtectionKillerEntry( NPC_Name.Ogre, "Ogre", typeof( Ogre ) ),
			new ProtectionKillerEntry( NPC_Name.OgreLord, "Ogre Lord", typeof( OgreLord ) ),
			new ProtectionKillerEntry( NPC_Name.Titan, "Titan", typeof( Titan ) ),
			new ProtectionKillerEntry( NPC_Name.Troll, "Troll", typeof( Troll ) ),
			new ProtectionKillerEntry( NPC_Name.BoneKnight, "Bone Knight", typeof( BoneKnight ) ),
			new ProtectionKillerEntry( NPC_Name.BoneMagi, "Bone Magi", typeof( BoneMagi ) ),
			new ProtectionKillerEntry( NPC_Name.LadyOfTheSnow, "Lady Of The Snow", typeof( LadyOfTheSnow ) ),
			new ProtectionKillerEntry( NPC_Name.Hiryu, "Hiryu", typeof( Hiryu ) ),
			new ProtectionKillerEntry( NPC_Name.LesserHiryu, "Lesser Hiryu", typeof( LesserHiryu ) ),
			new ProtectionKillerEntry( NPC_Name.Mummy, "Mummy", typeof( Mummy ) ),
			new ProtectionKillerEntry( NPC_Name.RottingCorpse, "Rotting Corpse", typeof( RottingCorpse ) ),
			new ProtectionKillerEntry( NPC_Name.AirElemental, "Air Elemental", typeof( AirElemental ) ),
			new ProtectionKillerEntry( NPC_Name.CrystalElemental, "Crystal Elemental", typeof( CrystalElemental ) ),
			new ProtectionKillerEntry( NPC_Name.EarthElemental, "Earth Elemental", typeof( EarthElemental ) ),
			new ProtectionKillerEntry( NPC_Name.Efreet, "Efreet", typeof( Efreet ) ),
			new ProtectionKillerEntry( NPC_Name.KazeKemono, "KazeKemono", typeof( KazeKemono ) ),
			new ProtectionKillerEntry( NPC_Name.Devourer, "Devourer", typeof( Devourer ) ),
			new ProtectionKillerEntry( NPC_Name.FanDancer, "Fan Dancer", typeof( FanDancer ) ),
			new ProtectionKillerEntry( NPC_Name.Imp, "Imp", typeof( Imp ) ),
			new ProtectionKillerEntry( NPC_Name.Oni, "Oni", typeof( Oni ) ),
			new ProtectionKillerEntry( NPC_Name.StoneGargoyle, "Stone Gargoyle", typeof( StoneGargoyle ) ),
			new ProtectionKillerEntry( NPC_Name.Scorpion, "Scorpion", typeof( Scorpion ) ),
			new ProtectionKillerEntry( NPC_Name.SkeletalDragon, "Skeletal Dragon", typeof( SkeletalDragon ) ),
			new ProtectionKillerEntry( NPC_Name.SerpentineDragon, "Serpentine Dragon", typeof( SerpentineDragon ) )
		};

		public static ProtectionKillerEntry[] ProtectionKillerTable { get { return m_Table; } set { m_Table = value; } }

		private NPC_Name m_Name;
		public NPC_Name Name { get { return m_Name; } set { m_Name = value; } }

		private string m_Title;
		public string Title { get { return m_Title; } set { m_Title = value; } }

		private Type m_NPC;
		public Type NPC { get { return m_NPC; } set { m_NPC = value; } }

		public ProtectionKillerEntry( NPC_Name name, string title, Type npc )
		{
			m_Name = name;
			m_Title = title;
			m_NPC = npc;
		}

		public static bool IsProtectionKiller( NPC_Name npcname, Mobile m )
		{
			if ( npcname == NPC_Name.None )
				return false;

			int v = (int) npcname - 1;

			if ( v >= 0 && v < m_Table.Length )
			{
				Type t = m.GetType();

				if ( m_Table[v].m_NPC == t )
					return true;

				return false;
			}
			return false;
		}
		public static String GetProtectionKillerTitle( NPC_Name npcname )
		{
			if ( npcname == NPC_Name.None )
				return null;

			int v = (int) npcname - 1;

			if ( v >= 0 && v < m_Table.Length )
				return m_Table[v].m_Title;

			return null;
		}

		public static NPC_Name GetRandom()
		{
			int index = Utility.Random( m_Table.Length );
			return m_Table[index].Name;
		}
	}
}