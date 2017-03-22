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
using Server.Network;

namespace Server
{
	public delegate TimeSpan SkillUseCallback( Mobile user );

	public enum SkillLock : byte
	{
		Up = 0,
		Down = 1,
		Locked = 2
	}

	public enum SkillName
	{
		Alchemy = 0,
		Anatomy = 1,
		AnimalLore = 2,
		ItemID = 3,
		ArmsLore = 4,
		Parry = 5,
		Begging = 6,
		Blacksmith = 7,
		Fletching = 8,
		Peacemaking = 9,
		Camping = 10,
		Carpentry = 11,
		Cartography = 12,
		Cooking = 13,
		DetectHidden = 14,
		Discordance = 15,
		EvalInt = 16,
		Healing = 17,
		Fishing = 18,
		Forensics = 19,
		Herding = 20,
		Hiding = 21,
		Provocation = 22,
		Inscribe = 23,
		Lockpicking = 24,
		Magery = 25,
		MagicResist = 26,
		Tactics = 27,
		Snooping = 28,
		Musicianship = 29,
		Poisoning = 30,
		Archery = 31,
		SpiritSpeak = 32,
		Stealing = 33,
		Tailoring = 34,
		AnimalTaming = 35,
		TasteID = 36,
		Tinkering = 37,
		Tracking = 38,
		Veterinary = 39,
		Swords = 40,
		Macing = 41,
		Fencing = 42,
		Wrestling = 43,
		Lumberjacking = 44,
		Mining = 45,
		Meditation = 46,
		Stealth = 47,
		RemoveTrap = 48,
		Necromancy = 49,
		Focus = 50,
		Chivalry = 51,
		Bushido = 52,
		Ninjitsu = 53,
		Spellweaving = 54,
		Mysticism = 55,
		Imbuing = 56,
		Throwing = 57
	}

	[PropertyObject]
	public class Skill
	{
		private readonly Skills m_Owner;
		private readonly SkillInfo m_Info;

		private ushort m_Base;
		private ushort m_Cap;
		private SkillLock m_Lock;

		public override string ToString()
		{
			return String.Format( "[{0}: {1}]", Name, Base );
		}

		public Skill( Skills owner, SkillInfo info, GenericReader reader )
		{
			m_Owner = owner;
			m_Info = info;

			int version = reader.ReadByte();

			switch ( version )
			{
				case 0:
					{
						m_Base = reader.ReadUShort();
						m_Cap = reader.ReadUShort();
						m_Lock = (SkillLock) reader.ReadByte();

						break;
					}
				case 0xFF:
					{
						m_Base = 0;
						m_Cap = 1000;
						m_Lock = SkillLock.Up;

						break;
					}
				default:
					{
						if ( ( version & 0xC0 ) == 0x00 )
						{
							if ( ( version & 0x1 ) != 0 )
								m_Base = reader.ReadUShort();

							if ( ( version & 0x2 ) != 0 )
								m_Cap = reader.ReadUShort();
							else
								m_Cap = 1000;

							if ( ( version & 0x4 ) != 0 )
								m_Lock = (SkillLock) reader.ReadByte();
						}

						break;
					}
			}
		}

		public Skill( Skills owner, SkillInfo info, int baseValue, int cap, SkillLock skillLock )
		{
			m_Owner = owner;
			m_Info = info;
			m_Base = (ushort) baseValue;
			m_Cap = (ushort) cap;
			m_Lock = skillLock;
		}

		public void SetLockNoRelay( SkillLock skillLock )
		{
			m_Lock = skillLock;
		}

		public void Serialize( GenericWriter writer )
		{
			if ( m_Base == 0 && m_Cap == 1000 && m_Lock == SkillLock.Up )
			{
				writer.Write( (byte) 0xFF ); // default
			}
			else
			{
				int flags = 0x0;

				if ( m_Base != 0 )
					flags |= 0x1;

				if ( m_Cap != 1000 )
					flags |= 0x2;

				if ( m_Lock != SkillLock.Up )
					flags |= 0x4;

				writer.Write( (byte) flags ); // version

				if ( m_Base != 0 )
					writer.Write( (short) m_Base );

				if ( m_Cap != 1000 )
					writer.Write( (short) m_Cap );

				if ( m_Lock != SkillLock.Up )
					writer.Write( (byte) m_Lock );
			}
		}

		public Skills Owner
		{
			get
			{
				return m_Owner;
			}
		}

		public SkillName SkillName
		{
			get
			{
				return (SkillName) m_Info.SkillId;
			}
		}

		public int SkillId
		{
			get
			{
				return m_Info.SkillId;
			}
		}

		[CommandProperty( AccessLevel.Counselor )]
		public string Name
		{
			get
			{
				return m_Info.Name;
			}
		}

		public SkillInfo Info
		{
			get
			{
				return m_Info;
			}
		}

		[CommandProperty( AccessLevel.Counselor )]
		public SkillLock Lock
		{
			get
			{
				return m_Lock;
			}
		}

		public int BaseFixedPoint
		{
			get
			{
				return m_Base;
			}
			set
			{
				if ( value < 0 )
					value = 0;
				else if ( value >= 0x10000 )
					value = 0xFFFF;

				ushort sv = (ushort) value;

				int oldBase = m_Base;

				if ( m_Base != sv )
				{
					m_Owner.Total = ( m_Owner.Total - m_Base ) + sv;

					m_Base = sv;

					m_Owner.OnSkillChange( this );

					Mobile m = m_Owner.Owner;

					if ( m != null )
						m.OnSkillChange( SkillName, (double) oldBase / 10 );
				}
			}
		}

		[CommandProperty( AccessLevel.Counselor, AccessLevel.GameMaster )]
		public double Base
		{
			get
			{
				return m_Base / 10.0;
			}
			set
			{
				BaseFixedPoint = (int) ( value * 10.0 );
			}
		}

		public int CapFixedPoint
		{
			get
			{
				return m_Cap;
			}
			set
			{
				if ( value < 0 )
					value = 0;
				else if ( value >= 0x10000 )
					value = 0xFFFF;

				ushort sv = (ushort) value;

				if ( m_Cap != sv )
				{
					m_Cap = sv;

					m_Owner.OnSkillChange( this );
				}
			}
		}

		[CommandProperty( AccessLevel.Counselor, AccessLevel.GameMaster )]
		public double Cap
		{
			get
			{
				return m_Cap / 10.0;
			}
			set
			{
				CapFixedPoint = (int) ( value * 10.0 );
			}
		}

		public static bool UseStatMods { get; set; }

		public int Fixed
		{
			get { return (int) ( Value * 10 ); }
		}

		[CommandProperty( AccessLevel.Counselor )]
		public double Value
		{
			get
			{
				double value = NonRacialValue;

				// Gargoyle Racial Ability: Mystic Insight
				if ( m_Owner.Owner.Race == Race.Gargoyle && SkillName == SkillName.Mysticism && value < 30.0 )
					value = 30.0;

				// Gargoyle Racial Ability: Deadly Aim
				if ( m_Owner.Owner.Race == Race.Gargoyle && SkillName == SkillName.Throwing && value < 20.0 )
					value = 20.0;

				// Human Racial Ability: Jack of all Trades
				if ( value < 20.0 && m_Owner.Owner.Race == Race.Human && m_Owner.Owner.Player )
					value = 20.0;

				return value;
			}
		}

		[CommandProperty( AccessLevel.Counselor )]
		public double NonRacialValue
		{
			get
			{
				double value = Base;

				m_Owner.Owner.ValidateSkillMods();

				double bonusObey = 0.0, bonusNotObey = 0.0;

				foreach ( SkillMod mod in m_Owner.Owner.GetSkillMods() )
				{
					if ( mod.Skill == (SkillName) m_Info.SkillId )
					{
						if ( mod.Relative )
						{
							if ( mod.ObeyCap )
								bonusObey += mod.Value;
							else
								bonusNotObey += mod.Value;
						}
						else
						{
							bonusObey = 0.0;
							bonusNotObey = 0.0;
							value = mod.Value;
						}
					}
				}

				value += bonusNotObey;

				if ( value < Cap )
				{
					value += bonusObey;

					if ( value > Cap )
						value = Cap;
				}

				return value;
			}
		}

		public void Update()
		{
			m_Owner.OnSkillChange( this );
		}
	}

	public class SkillInfo
	{
		private readonly int m_SkillId;

		public SkillInfo( int skillId, string name, string title, SkillUseCallback callback, StatType primaryStat, StatType secondaryStat, double gainFactor )
		{
			Name = name;
			Title = title;
			m_SkillId = skillId;
			Callback = callback;
			PrimaryStat = primaryStat;
			SecondaryStat = secondaryStat;
			GainFactor = gainFactor;
		}

		public int SkillId
		{
			get
			{
				return m_SkillId;
			}
		}

		public SkillUseCallback Callback { get; set; }
		public string Name { get; set; }
		public string Title { get; set; }
		public StatType PrimaryStat { get; set; }
		public StatType SecondaryStat { get; set; }
		public double GainFactor { get; set; }

		private static SkillInfo[] m_Table = new SkillInfo[58]
			{
				new SkillInfo(  0, "Alchemy",					"Alchemist",	null,	StatType.Int,	StatType.Dex,	1.0 ),
				new SkillInfo(  1, "Anatomy",					"Healer",		null,	StatType.Int,	StatType.Str,	1.0 ),
				new SkillInfo(  2, "Animal Lore",				"Ranger",		null,	StatType.Int,	StatType.Str,	1.0 ),
				new SkillInfo(  3, "Item Identification",		"Merchant",		null,	StatType.Int,	StatType.Dex,	1.0 ),
				new SkillInfo(  4, "Arms Lore",					"Warrior",		null,	StatType.Int,	StatType.Str,	1.0 ),
				new SkillInfo(  5, "Parrying",					"Warrior",		null,	StatType.Dex,	StatType.Str,	1.0 ),
				new SkillInfo(  6, "Begging",					"Beggar",		null,	StatType.Dex,	StatType.Int,	1.0 ),
				new SkillInfo(  7, "Blacksmithy",				"Smith",		null,	StatType.Str,	StatType.Dex,	1.0 ),
				new SkillInfo(  8, "Bowcraft/Fletching",		"Bowyer",		null,	StatType.Dex,	StatType.Str,	1.0 ),
				new SkillInfo(  9, "Peacemaking",				"Bard",			null,	StatType.Int,	StatType.Dex,	1.0 ),
				new SkillInfo( 10, "Camping",					"Ranger",		null,	StatType.Dex,	StatType.Int,	1.0 ),
				new SkillInfo( 11, "Carpentry",					"Carpenter",	null,	StatType.Str,	StatType.Dex,	1.0 ),
				new SkillInfo( 12, "Cartography",				"Cartographer",	null,	StatType.Int,	StatType.Dex,	1.0 ),
				new SkillInfo( 13, "Cooking",					"Chef",			null,	StatType.Int,	StatType.Dex,	1.0 ),
				new SkillInfo( 14, "Detecting Hidden",			"Scout",		null,	StatType.Int,	StatType.Dex,	1.0 ),
				new SkillInfo( 15, "Discordance",				"Bard",			null,	StatType.Int,	StatType.Dex,	1.0 ),
				new SkillInfo( 16, "Evaluating Intelligence",	"Scholar",		null,	StatType.Int,	StatType.Str,	1.0 ),
				new SkillInfo( 17, "Healing",					"Healer",		null,	StatType.Int,	StatType.Dex,	1.0 ),
				new SkillInfo( 18, "Fishing",					"Fisherman",	null,	StatType.Dex,	StatType.Str,	1.0 ),
				new SkillInfo( 19, "Forensic Evaluation",		"Detective",	null,	StatType.Int,	StatType.Dex,	1.0 ),
				new SkillInfo( 20, "Herding",					"Shepherd",		null,	StatType.Int,	StatType.Dex,	1.0 ),
				new SkillInfo( 21, "Hiding",					"Rogue",		null,	StatType.Dex,	StatType.Int,	1.0 ),
				new SkillInfo( 22, "Provocation",				"Bard",			null,	StatType.Int,	StatType.Dex,	1.0 ),
				new SkillInfo( 23, "Inscription",				"Scribe",		null,	StatType.Int,	StatType.Dex,	1.0 ),
				new SkillInfo( 24, "Lockpicking",				"Rogue",		null,	StatType.Dex,	StatType.Int,	1.0 ),
				new SkillInfo( 25, "Magery",					"Mage",			null,	StatType.Int,	StatType.Str,	1.0 ),
				new SkillInfo( 26, "Resisting Spells",			"Mage",			null,	StatType.Str,	StatType.Dex,	1.0 ),
				new SkillInfo( 27, "Tactics",					"Warrior",		null,	StatType.Str,	StatType.Dex,	1.0 ),
				new SkillInfo( 28, "Snooping",					"Pickpocket",	null,	StatType.Dex,	StatType.Int,	1.0 ),
				new SkillInfo( 29, "Musicianship",				"Bard",			null,	StatType.Dex,	StatType.Int,	1.0 ),
				new SkillInfo( 30, "Poisoning",					"Assassin",		null,	StatType.Int,	StatType.Dex,	1.0 ),
				new SkillInfo( 31, "Archery",					"Archer",		null,	StatType.Dex,	StatType.Str,	1.0 ),
				new SkillInfo( 32, "Spirit Speak",				"Medium",		null,	StatType.Int,	StatType.Str,	1.0 ),
				new SkillInfo( 33, "Stealing",					"Rogue",		null,	StatType.Dex,	StatType.Int,	1.0 ),
				new SkillInfo( 34, "Tailoring",					"Tailor",		null,	StatType.Dex,	StatType.Int,	1.0 ),
				new SkillInfo( 35, "Animal Taming",				"Tamer",		null,	StatType.Str,	StatType.Int,	1.0 ),
				new SkillInfo( 36, "Taste Identification",		"Chef",			null,	StatType.Int,	StatType.Str,	1.0 ),
				new SkillInfo( 37, "Tinkering",					"Tinker",		null,	StatType.Dex,	StatType.Int,	1.0 ),
				new SkillInfo( 38, "Tracking",					"Ranger",		null,	StatType.Int,	StatType.Dex,	1.0 ),
				new SkillInfo( 39, "Veterinary",				"Veterinarian",	null,	StatType.Int,	StatType.Dex,	1.0 ),
				new SkillInfo( 40, "Swordsmanship",				"Swordsman",	null,	StatType.Str,	StatType.Dex,	1.0 ),
				new SkillInfo( 41, "Mace Fighting",				"Armsman",		null,	StatType.Str,	StatType.Dex,	1.0 ),
				new SkillInfo( 42, "Fencing",					"Fencer",		null,	StatType.Dex,	StatType.Str,	1.0 ),
				new SkillInfo( 43, "Wrestling",					"Wrestler",		null,	StatType.Str,	StatType.Dex,	1.0 ),
				new SkillInfo( 44, "Lumberjacking",				"Lumberjack",	null,	StatType.Str,	StatType.Dex,	1.0 ),
				new SkillInfo( 45, "Mining",					"Miner",		null,	StatType.Str,	StatType.Dex,	1.0 ),
				new SkillInfo( 46, "Meditation",				"Stoic",		null,	StatType.Int,	StatType.Str,	1.0 ),
				new SkillInfo( 47, "Stealth",					"Rogue",		null,	StatType.Dex,	StatType.Int,	1.0 ),
				new SkillInfo( 48, "Remove Trap",				"Rogue",		null,	StatType.Dex,	StatType.Int,	1.0 ),
				new SkillInfo( 49, "Necromancy",				"Necromancer",	null,	StatType.Int,	StatType.Str,	1.0 ),
				new SkillInfo( 50, "Focus",						"Stoic",		null,	StatType.Dex,	StatType.Int,	1.0 ),
				new SkillInfo( 51, "Chivalry",					"Paladin",		null,	StatType.Str,	StatType.Int,	1.0 ),
				new SkillInfo( 52, "Bushido",					"Samurai",		null,	StatType.Str,	StatType.Int,	1.0 ),
				new SkillInfo( 53, "Ninjitsu",					"Ninja",		null,	StatType.Dex,	StatType.Int,	1.0 ),
				new SkillInfo( 54, "Spellweaving",				"Arcanist",		null,	StatType.Int,	StatType.Str,	1.0 ),
				new SkillInfo( 55, "Mysticism",					"Mystic",		null,	StatType.Int,	StatType.Str,	1.0 ),
				new SkillInfo( 56, "Imbuing",					"Artificer",	null,	StatType.Int,	StatType.Str,	1.0 ),
				new SkillInfo( 57, "Throwing",					"Bladeweaver",	null,	StatType.Dex,	StatType.Str,	1.0 ),
			};

		public static SkillInfo[] Table
		{
			get
			{
				return m_Table;
			}
			set
			{
				m_Table = value;
			}
		}
	}

	[PropertyObject]
	public class Skills : IEnumerable<Skill>
	{
		private readonly Mobile m_Owner;
		private readonly Skill[] m_Skills;

		private int m_Total, m_Cap;
		private Skill m_Highest;

		[CommandProperty( AccessLevel.Counselor )]
		public Skill Alchemy { get { return this[SkillName.Alchemy]; } set { } }

		[CommandProperty( AccessLevel.Counselor )]
		public Skill Anatomy { get { return this[SkillName.Anatomy]; } set { } }

		[CommandProperty( AccessLevel.Counselor )]
		public Skill AnimalLore { get { return this[SkillName.AnimalLore]; } set { } }

		[CommandProperty( AccessLevel.Counselor )]
		public Skill ItemID { get { return this[SkillName.ItemID]; } set { } }

		[CommandProperty( AccessLevel.Counselor )]
		public Skill ArmsLore { get { return this[SkillName.ArmsLore]; } set { } }

		[CommandProperty( AccessLevel.Counselor )]
		public Skill Parry { get { return this[SkillName.Parry]; } set { } }

		[CommandProperty( AccessLevel.Counselor )]
		public Skill Begging { get { return this[SkillName.Begging]; } set { } }

		[CommandProperty( AccessLevel.Counselor )]
		public Skill Blacksmith { get { return this[SkillName.Blacksmith]; } set { } }

		[CommandProperty( AccessLevel.Counselor )]
		public Skill Fletching { get { return this[SkillName.Fletching]; } set { } }

		[CommandProperty( AccessLevel.Counselor )]
		public Skill Peacemaking { get { return this[SkillName.Peacemaking]; } set { } }

		[CommandProperty( AccessLevel.Counselor )]
		public Skill Camping { get { return this[SkillName.Camping]; } set { } }

		[CommandProperty( AccessLevel.Counselor )]
		public Skill Carpentry { get { return this[SkillName.Carpentry]; } set { } }

		[CommandProperty( AccessLevel.Counselor )]
		public Skill Cartography { get { return this[SkillName.Cartography]; } set { } }

		[CommandProperty( AccessLevel.Counselor )]
		public Skill Cooking { get { return this[SkillName.Cooking]; } set { } }

		[CommandProperty( AccessLevel.Counselor )]
		public Skill DetectHidden { get { return this[SkillName.DetectHidden]; } set { } }

		[CommandProperty( AccessLevel.Counselor )]
		public Skill Discordance { get { return this[SkillName.Discordance]; } set { } }

		[CommandProperty( AccessLevel.Counselor )]
		public Skill EvalInt { get { return this[SkillName.EvalInt]; } set { } }

		[CommandProperty( AccessLevel.Counselor )]
		public Skill Healing { get { return this[SkillName.Healing]; } set { } }

		[CommandProperty( AccessLevel.Counselor )]
		public Skill Fishing { get { return this[SkillName.Fishing]; } set { } }

		[CommandProperty( AccessLevel.Counselor )]
		public Skill Forensics { get { return this[SkillName.Forensics]; } set { } }

		[CommandProperty( AccessLevel.Counselor )]
		public Skill Herding { get { return this[SkillName.Herding]; } set { } }

		[CommandProperty( AccessLevel.Counselor )]
		public Skill Hiding { get { return this[SkillName.Hiding]; } set { } }

		[CommandProperty( AccessLevel.Counselor )]
		public Skill Provocation { get { return this[SkillName.Provocation]; } set { } }

		[CommandProperty( AccessLevel.Counselor )]
		public Skill Inscribe { get { return this[SkillName.Inscribe]; } set { } }

		[CommandProperty( AccessLevel.Counselor )]
		public Skill Lockpicking { get { return this[SkillName.Lockpicking]; } set { } }

		[CommandProperty( AccessLevel.Counselor )]
		public Skill Magery { get { return this[SkillName.Magery]; } set { } }

		[CommandProperty( AccessLevel.Counselor )]
		public Skill MagicResist { get { return this[SkillName.MagicResist]; } set { } }

		[CommandProperty( AccessLevel.Counselor )]
		public Skill Tactics { get { return this[SkillName.Tactics]; } set { } }

		[CommandProperty( AccessLevel.Counselor )]
		public Skill Snooping { get { return this[SkillName.Snooping]; } set { } }

		[CommandProperty( AccessLevel.Counselor )]
		public Skill Musicianship { get { return this[SkillName.Musicianship]; } set { } }

		[CommandProperty( AccessLevel.Counselor )]
		public Skill Poisoning { get { return this[SkillName.Poisoning]; } set { } }

		[CommandProperty( AccessLevel.Counselor )]
		public Skill Archery { get { return this[SkillName.Archery]; } set { } }

		[CommandProperty( AccessLevel.Counselor )]
		public Skill SpiritSpeak { get { return this[SkillName.SpiritSpeak]; } set { } }

		[CommandProperty( AccessLevel.Counselor )]
		public Skill Stealing { get { return this[SkillName.Stealing]; } set { } }

		[CommandProperty( AccessLevel.Counselor )]
		public Skill Tailoring { get { return this[SkillName.Tailoring]; } set { } }

		[CommandProperty( AccessLevel.Counselor )]
		public Skill AnimalTaming { get { return this[SkillName.AnimalTaming]; } set { } }

		[CommandProperty( AccessLevel.Counselor )]
		public Skill TasteID { get { return this[SkillName.TasteID]; } set { } }

		[CommandProperty( AccessLevel.Counselor )]
		public Skill Tinkering { get { return this[SkillName.Tinkering]; } set { } }

		[CommandProperty( AccessLevel.Counselor )]
		public Skill Tracking { get { return this[SkillName.Tracking]; } set { } }

		[CommandProperty( AccessLevel.Counselor )]
		public Skill Veterinary { get { return this[SkillName.Veterinary]; } set { } }

		[CommandProperty( AccessLevel.Counselor )]
		public Skill Swords { get { return this[SkillName.Swords]; } set { } }

		[CommandProperty( AccessLevel.Counselor )]
		public Skill Macing { get { return this[SkillName.Macing]; } set { } }

		[CommandProperty( AccessLevel.Counselor )]
		public Skill Fencing { get { return this[SkillName.Fencing]; } set { } }

		[CommandProperty( AccessLevel.Counselor )]
		public Skill Wrestling { get { return this[SkillName.Wrestling]; } set { } }

		[CommandProperty( AccessLevel.Counselor )]
		public Skill Lumberjacking { get { return this[SkillName.Lumberjacking]; } set { } }

		[CommandProperty( AccessLevel.Counselor )]
		public Skill Mining { get { return this[SkillName.Mining]; } set { } }

		[CommandProperty( AccessLevel.Counselor )]
		public Skill Meditation { get { return this[SkillName.Meditation]; } set { } }

		[CommandProperty( AccessLevel.Counselor )]
		public Skill Stealth { get { return this[SkillName.Stealth]; } set { } }

		[CommandProperty( AccessLevel.Counselor )]
		public Skill RemoveTrap { get { return this[SkillName.RemoveTrap]; } set { } }

		[CommandProperty( AccessLevel.Counselor )]
		public Skill Necromancy { get { return this[SkillName.Necromancy]; } set { } }

		[CommandProperty( AccessLevel.Counselor )]
		public Skill Focus { get { return this[SkillName.Focus]; } set { } }

		[CommandProperty( AccessLevel.Counselor )]
		public Skill Chivalry { get { return this[SkillName.Chivalry]; } set { } }

		[CommandProperty( AccessLevel.Counselor )]
		public Skill Bushido { get { return this[SkillName.Bushido]; } set { } }

		[CommandProperty( AccessLevel.Counselor )]
		public Skill Ninjitsu { get { return this[SkillName.Ninjitsu]; } set { } }

		[CommandProperty( AccessLevel.Counselor )]
		public Skill Spellweaving { get { return this[SkillName.Spellweaving]; } set { } }

		[CommandProperty( AccessLevel.Counselor )]
		public Skill Mysticism { get { return this[SkillName.Mysticism]; } set { } }

		[CommandProperty( AccessLevel.Counselor )]
		public Skill Imbuing { get { return this[SkillName.Imbuing]; } set { } }

		[CommandProperty( AccessLevel.Counselor )]
		public Skill Throwing { get { return this[SkillName.Throwing]; } set { } }

		[CommandProperty( AccessLevel.Counselor, AccessLevel.GameMaster )]
		public int Cap
		{
			get { return m_Cap; }
			set { m_Cap = value; }
		}

		public int Total
		{
			get { return m_Total; }
			set { m_Total = value; }
		}

		public Mobile Owner
		{
			get { return m_Owner; }
		}

		public int Length
		{
			get { return m_Skills.Length; }
		}

		public Skill this[SkillName name]
		{
			get { return this[(int) name]; }
		}

		public Skill this[int skillId]
		{
			get
			{
				if ( skillId < 0 || skillId >= m_Skills.Length )
					return null;

				Skill sk = m_Skills[skillId];

				if ( sk == null )
					m_Skills[skillId] = sk = new Skill( this, SkillInfo.Table[skillId], 0, 1000, SkillLock.Up );

				return sk;
			}
		}

		public IEnumerator<Skill> GetEnumerator()
		{
			return m_Skills.AsEnumerable().Where( s => s != null ).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public override string ToString()
		{
			return "...";
		}

		public static bool UseSkill( Mobile from, SkillName name )
		{
			return UseSkill( from, (int) name );
		}

		public static bool UseSkill( Mobile from, int skillId )
		{
			if ( !from.CheckAlive() )
				return false;
			else if ( !from.Region.OnSkillUse( from, (SkillName) skillId ) )
				return false;
			else if ( !from.AllowSkillUse( (SkillName) skillId ) )
				return false;

			if ( skillId >= 0 && skillId < SkillInfo.Table.Length )
			{
				SkillInfo info = SkillInfo.Table[skillId];

				if ( info.Callback != null )
				{
					if ( from.NextSkillTime <= DateTime.UtcNow && from.Spell == null )
					{
						from.DisruptiveAction();

						from.NextSkillTime = DateTime.UtcNow + info.Callback( from );

						return true;
					}
					else
					{
						from.SendSkillMessage();
					}
				}
				else
				{
					from.SendLocalizedMessage( 500014 ); // That skill cannot be used directly.
				}
			}

			return false;
		}

		public Skill Highest
		{
			get
			{
				if ( m_Highest == null )
				{
					Skill highest = null;
					int value = int.MinValue;

					foreach ( Skill sk in m_Skills )
					{
						if ( sk != null && sk.BaseFixedPoint > value )
						{
							value = sk.BaseFixedPoint;
							highest = sk;
						}
					}

					if ( highest == null && m_Skills.Length > 0 )
						highest = this[0];

					m_Highest = highest;
				}

				return m_Highest;
			}
		}

		public void Serialize( GenericWriter writer )
		{
			m_Total = 0;

			writer.Write( (int) 3 ); // version

			writer.Write( (int) m_Cap );
			writer.Write( (int) m_Skills.Length );

			for ( int i = 0; i < m_Skills.Length; ++i )
			{
				Skill sk = m_Skills[i];

				if ( sk == null )
				{
					writer.Write( (byte) 0xFF );
				}
				else
				{
					sk.Serialize( writer );
					m_Total += sk.BaseFixedPoint;
				}
			}
		}

		public Skills( Mobile owner )
		{
			m_Owner = owner;
			m_Cap = 7200;

			SkillInfo[] info = SkillInfo.Table;

			m_Skills = new Skill[info.Length];
		}

		public Skills( Mobile owner, GenericReader reader )
		{
			m_Owner = owner;

			int version = reader.ReadInt();

			switch ( version )
			{
				case 3:
				case 2:
					{
						m_Cap = reader.ReadInt();

						goto case 1;
					}
				case 1:
					{
						if ( version < 2 )
							m_Cap = 7200;

						if ( version < 3 )
							/*m_Total = */
							reader.ReadInt();

						SkillInfo[] info = SkillInfo.Table;

						m_Skills = new Skill[info.Length];

						int count = reader.ReadInt();

						for ( int i = 0; i < count; ++i )
						{
							if ( i < info.Length )
							{
								Skill sk = new Skill( this, info[i], reader );

								if ( sk.BaseFixedPoint != 0 || sk.CapFixedPoint != 1000 || sk.Lock != SkillLock.Up )
								{
									m_Skills[i] = sk;
									m_Total += sk.BaseFixedPoint;
								}
							}
							else
							{
								new Skill( this, null, reader );
							}
						}

						break;
					}
				case 0:
					{
						reader.ReadInt();

						goto case 1;
					}
			}
		}

		public void OnSkillChange( Skill skill )
		{
			if ( skill == m_Highest ) // could be downgrading the skill, force a recalc
				m_Highest = null;
			else if ( m_Highest != null && skill.BaseFixedPoint > m_Highest.BaseFixedPoint )
				m_Highest = skill;

			m_Owner.OnSkillInvalidated( skill );

			NetState ns = m_Owner.NetState;

			if ( ns != null )
				ns.Send( new SkillChange( skill ) );
		}
	}
}