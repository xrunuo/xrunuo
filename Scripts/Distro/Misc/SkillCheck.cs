using System;
using System.Collections.Generic;
using System.Linq;

using Server;
using Server.Engines.Quests;
using Server.Mobiles;
using Server.Items;

namespace Server.Misc
{
	public class SkillCheck
	{
		private static readonly bool AntiMacroCode = false; // Change this to false to disable anti-macro code

		public static TimeSpan AntiMacroExpire = TimeSpan.FromMinutes( 5.0 ); // How long do we remember targets/locations?
		public const int Allowance = 3; // How many times may we use the same location/target for gain
		private const int LocationSize = 5; // The size of eeach location, make this smaller so players dont have to move as far

		private static bool[] UseAntiMacro = new bool[] // true if this skill uses the anti-macro code, false if it does not
		{
			false, // Alchemy = 0,
			true, // Anatomy = 1,
			true, // AnimalLore = 2,
			true, // ItemID = 3,
			true, // ArmsLore = 4,
			false, // Parry = 5,
			true, // Begging = 6,
			false, // Blacksmith = 7,
			false, // Fletching = 8,
			true, // Peacemaking = 9,
			true, // Camping = 10,
			false, // Carpentry = 11,
			false, // Cartography = 12,
			false, // Cooking = 13,
			true, // DetectHidden = 14,
			true, // Discordance = 15,
			true, // EvalInt = 16,
			true, // Healing = 17,
			true, // Fishing = 18,
			true, // Forensics = 19,
			true, // Herding = 20,
			true, // Hiding = 21,
			true, // Provocation = 22,
			false, // Inscribe = 23,
			true, // Lockpicking = 24,
			true, // Magery = 25,
			true, // MagicResist = 26,
			false, // Tactics = 27,
			true, // Snooping = 28,
			true, // Musicianship = 29,
			true, // Poisoning = 30,
			false, // Archery = 31,
			true, // SpiritSpeak = 32,
			true, // Stealing = 33,
			false, // Tailoring = 34,
			true, // AnimalTaming = 35,
			true, // TasteID = 36,
			false, // Tinkering = 37,
			true, // Tracking = 38,
			true, // Veterinary = 39,
			false, // Swords = 40,
			false, // Macing = 41,
			false, // Fencing = 42,
			false, // Wrestling = 43,
			true, // Lumberjacking = 44,
			true, // Mining = 45,
			true, // Meditation = 46,
			true, // Stealth = 47,
			true, // RemoveTrap = 48,
			true, // Necromancy = 49,
			false, // Focus = 50,
			true, // Chivalry = 51
			true, // Bushido = 52
			true, // Ninjitsu = 53
			true, // Spellweaving = 54
			true, // Mysticism = 55
			false, // Imbuing = 56
			true, // Throwing = 57
		};

		public static void Initialize()
		{
			Mobile.SkillCheckLocationHandler = new SkillCheckLocationHandler( Mobile_SkillCheckLocation );
			Mobile.SkillCheckDirectLocationHandler = new SkillCheckDirectLocationHandler( Mobile_SkillCheckDirectLocation );

			Mobile.SkillCheckTargetHandler = new SkillCheckTargetHandler( Mobile_SkillCheckTarget );
			Mobile.SkillCheckDirectTargetHandler = new SkillCheckDirectTargetHandler( Mobile_SkillCheckDirectTarget );
		}

		public static bool Mobile_SkillCheckLocation( Mobile from, SkillName skillName, double minSkill, double maxSkill )
		{
			Skill skill = from.Skills[skillName];

			if ( skill == null )
				return false;

			double value = skill.Value;

			double chance = ( value - minSkill ) / ( maxSkill - minSkill );

			CrystalBallOfKnowledge.TellSkillDifficulty( from, skillName, chance );

			if ( chance < 0.0 )
				return false; // Too difficult
			else if ( chance >= ( skill.Cap / 100.0 ) )
				return true; // No challenge

			Point2D loc = new Point2D( from.Location.X / LocationSize, from.Location.Y / LocationSize );
			return CheckSkill( from, skill, loc, chance );
		}

		public static bool Mobile_SkillCheckDirectLocation( Mobile from, SkillName skillName, double chance )
		{
			Skill skill = from.Skills[skillName];

			if ( skill == null )
				return false;

			CrystalBallOfKnowledge.TellSkillDifficulty( from, skillName, chance );

			if ( chance < 0.0 )
				return false; // Too difficult
			else if ( chance >= ( skill.Cap / 100.0 ) )
				return true; // No challenge

			Point2D loc = new Point2D( from.Location.X / LocationSize, from.Location.Y / LocationSize );
			return CheckSkill( from, skill, loc, chance );
		}

		public static bool CheckSkill( Mobile from, Skill skill, object amObj, double chance )
		{
			if ( from.Skills.Cap == 0 )
				return false;

			bool success = ( chance >= Utility.RandomDouble() );
			double gc = (double) ( from.Skills.Cap - from.Skills.Total ) / from.Skills.Cap;
			gc += ( skill.Cap - skill.Base ) / skill.Cap;
			gc /= 3.0;

			gc += ( 1.0 - chance ) * ( success ? 0.5 : 0.2 );
			gc /= 3.0;

			gc *= skill.Info.GainFactor;

			if ( gc < 0.01 )
				gc = 0.01;

			if ( from is BaseCreature && ( (BaseCreature) from ).Controlled )
				gc *= 2;

			if ( from.Alive && ( ( gc >= Utility.RandomDouble() && AllowGain( from, skill, amObj ) ) || skill.Base < 10.0 ) )
				Gain( from, skill );

			if ( skill.Lock == SkillLock.Up )
				CheckStatGain( from, skill.Info.PrimaryStat, skill.Info.SecondaryStat );

			return success;
		}

		public static bool Mobile_SkillCheckTarget( Mobile from, SkillName skillName, object target, double minSkill, double maxSkill )
		{
			Skill skill = from.Skills[skillName];

			if ( skill == null )
				return false;

			double value = skill.Value;

			double chance = ( value - minSkill ) / ( maxSkill - minSkill );

			CrystalBallOfKnowledge.TellSkillDifficulty( from, skillName, chance );

			if ( chance < 0.0 )
				return false; // Too difficult
			else if ( chance >= 1.0 )
				return true; // No challenge

			return CheckSkill( from, skill, target, chance );
		}

		public static bool Mobile_SkillCheckDirectTarget( Mobile from, SkillName skillName, object target, double chance )
		{
			Skill skill = from.Skills[skillName];

			if ( skill == null )
				return false;

			CrystalBallOfKnowledge.TellSkillDifficulty( from, skillName, chance );

			if ( chance < 0.0 )
				return false; // Too difficult
			else if ( chance >= 1.0 )
				return true; // No challenge

			return CheckSkill( from, skill, target, chance );
		}

		private static bool AllowGain( Mobile from, Skill skill, object obj )
		{
			if ( from is PlayerMobile && AntiMacroCode && UseAntiMacro[skill.Info.SkillId] )
				return ( (PlayerMobile) from ).AntiMacroCheck( skill, obj );
			else
				return true;
		}

		public enum Stat
		{
			Str,
			Dex,
			Int
		}

		public static void Gain( Mobile from, Skill skill )
		{
			if ( from.Region.IsPartOf( typeof( Regions.Jail ) ) )
				return;

			if ( from is BaseCreature && ( (BaseCreature) from ).IsDeadPet )
				return;

			if ( skill.SkillName == SkillName.Focus && from is BaseCreature )
				return;

			if ( Factions.Faction.HasSkillLoss( from ) )
				return;

			if ( skill.Base < skill.Cap && skill.Lock == SkillLock.Up )
			{
				int toGain = 1;

				if ( skill.Base <= 10.0 )
					toGain = Utility.RandomMinMax( 1, 4 );
				else if ( HasAcceleratedSkillGain( from, skill ) )
					toGain = Utility.RandomMinMax( 2, 5 );

				Skills skills = from.Skills;

				if ( ( skills.Total / skills.Cap ) >= Utility.RandomDouble() ) //( skills.Total >= skills.Cap )
				{
					foreach ( Skill toLower in skills.Where( s => s != skill && s.Lock == SkillLock.Down ) )
					{
						if ( toLower.BaseFixedPoint >= toGain )
						{
							toLower.BaseFixedPoint -= toGain;
							break;
						}
					}
				}

				if ( ( skills.Total + toGain ) <= skills.Cap )
					skill.BaseFixedPoint += toGain;
			}

			#region Mondain's Legacy
			if ( from is PlayerMobile )
				Server.Engines.Quests.QuestHelper.CheckSkill( (PlayerMobile) from, skill );
			#endregion
		}

		public static bool HasAcceleratedSkillGain( Mobile from, Skill skill )
		{
			if ( ScrollOfAlacrity.HasAcceleratedSkillGain( from, skill ) )
				return true;

			if ( from is PlayerMobile && QuestHelper.EnhancedSkill( (PlayerMobile) from, skill ) )
				return true;

			return false;
		}

		public static bool HasAnyAcceleratedSkillGain( Mobile from )
		{
			if ( ScrollOfAlacrity.HasAnyAcceleratedSkillGain( from ) )
				return true;

			if ( from is PlayerMobile && ( (PlayerMobile) from ).Quests.Any( q => q.Objectives.OfType<ApprenticeObjective>().Any() ) )
				return true;

			return false;
		}

		public static bool CanLower( Mobile from, Stat stat )
		{
			switch ( stat )
			{
				case Stat.Str:
					return ( from.StrLock == StatLockType.Down && from.RawStr > 10 );
				case Stat.Dex:
					return ( from.DexLock == StatLockType.Down && from.RawDex > 10 );
				case Stat.Int:
					return ( from.IntLock == StatLockType.Down && from.RawInt > 10 );
			}

			return false;
		}

		public static bool CanRaise( Mobile from, Stat stat )
		{
			if ( !( from is BaseCreature && ( (BaseCreature) from ).Controlled ) )
			{
				if ( from.RawStatTotal >= from.StatCap )
					return false;
			}

			switch ( stat )
			{
				case Stat.Str:
					return ( from.StrLock == StatLockType.Up && from.RawStr < 125 );
				case Stat.Dex:
					return ( from.DexLock == StatLockType.Up && from.RawDex < 125 );
				case Stat.Int:
					return ( from.IntLock == StatLockType.Up && from.RawInt < 125 );
			}

			return false;
		}

		private static void CheckStatGain( Mobile from, StatType primaryStat, StatType secondaryStat )
		{
			if ( 1 == Utility.Random( 20 ) )
			{
				StatType statType;

				if ( 0.75 > Utility.RandomDouble() )
					statType = primaryStat;
				else
					statType = secondaryStat;

				switch ( statType )
				{
					case StatType.Str:
						{
							if ( from.StrLock == StatLockType.Up )
								GainStat( from, Stat.Str );

							break;
						}
					case StatType.Dex:
						{
							if ( from.DexLock == StatLockType.Up )
								GainStat( from, Stat.Dex );

							break;
						}
					case StatType.Int:
						{
							if ( from.IntLock == StatLockType.Up )
								GainStat( from, Stat.Int );

							break;
						}
				}
			}
		}

		private static void GainStat( Mobile from, Stat stat )
		{
			bool atrophy = ( from.RawStatTotal >= from.StatCap );

			switch ( stat )
			{
				case Stat.Str:
					{
						if ( atrophy )
						{
							if ( CanLower( from, Stat.Dex ) && ( from.RawDex < from.RawInt || !CanLower( from, Stat.Int ) ) )
								--from.RawDex;
							else if ( CanLower( from, Stat.Int ) )
								--from.RawInt;
						}

						if ( CanRaise( from, Stat.Str ) )
							++from.RawStr;

						break;
					}
				case Stat.Dex:
					{
						if ( atrophy )
						{
							if ( CanLower( from, Stat.Str ) && ( from.RawStr < from.RawInt || !CanLower( from, Stat.Int ) ) )
								--from.RawStr;
							else if ( CanLower( from, Stat.Int ) )
								--from.RawInt;
						}

						if ( CanRaise( from, Stat.Dex ) )
							++from.RawDex;

						break;
					}
				case Stat.Int:
					{
						if ( atrophy )
						{
							if ( CanLower( from, Stat.Str ) && ( from.RawStr < from.RawDex || !CanLower( from, Stat.Dex ) ) )
								--from.RawStr;
							else if ( CanLower( from, Stat.Dex ) )
								--from.RawDex;
						}

						if ( CanRaise( from, Stat.Int ) )
							++from.RawInt;

						break;
					}
			}
		}
	}
}