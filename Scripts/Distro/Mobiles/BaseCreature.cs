using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Server.ContextMenus;
using Server.Engines.CannedEvil;
using Server.Engines.Housing.Regions;
using Server.Engines.Loyalty;
using Server.Engines.MiniChamps;
using Server.Engines.Quests;
using Server.Events;
using Server.Factions;
using Server.Items;
using Server.Misc;
using Server.Network;
using Server.Prompts;
using Server.Regions;
using Server.Spells;
using Server.Spells.Bushido;
using Server.Spells.Spellweaving;
using Server.Targeting;

namespace Server.Mobiles
{
	#region Enums
	/// <summary>
	/// Summary description for MobileAI.
	/// </summary>
	/// 
	public enum FightMode
	{
		None,			// Never focus on others
		Aggressor,		// Only attack aggressors
		Strongest,		// Attack the strongest
		Weakest,		// Attack the weakest
		Closest, 		// Attack the closest
		Evil,			// Only attack aggressor -or- negative karma
		Good			// Only attack aggressor -or- positive karma
	}

	public enum OrderType
	{
		None,			// When no order, let's roam

		Come,			// "(All/Name) come"		Summons all or one pet to your location.  
		Drop,			// "(Name) drop"			Drops its loot to the ground (if it carries any).  
		Follow,			// "(Name) follow"			Follows targeted being.  
		// "(All/Name) follow me"	Makes all or one pet follow you.  
		Friend,			// "(Name) friend"			Allows targeted player to confirm resurrection. 
		Unfriend,		// Remove a friend
		Guard,			// "(Name) guard"			Makes the specified pet guard you. Pets can only guard their owner. 
		// "(All/Name) guard me"	Makes all or one pet guard you.  
		Attack,			// "(All/Name) kill", 
		// "(All/Name) attack"		All or the specified pet(s) currently under your control attack the target. 
		Patrol,			// "(Name) patrol"			Roves between two or more guarded targets.  
		Release,		// "(Name) release"			Releases pet back into the wild (removes "tame" status). 
		Stay,			// "(All/Name) stay"		All or the specified pet(s) will stop and stay in current spot. 
		Stop,			// "(All/Name) stop"		Cancels any current orders to attack, guard or follow.  
		Transfer		// "(Name) transfer"		Transfers complete ownership to targeted player. 
	}

	[Flags]
	public enum FoodType
	{
		Meat = 0x0001,
		FruitsAndVegies = 0x0002,
		GrainsAndHay = 0x0004,
		Fish = 0x0008,
		Eggs = 0x0010,
		Gold = 0x0020
	}

	[Flags]
	public enum PackInstinct
	{
		None = 0x0000,
		Canine = 0x0001,
		Ostard = 0x0002,
		Feline = 0x0004,
		Arachnid = 0x0008,
		Daemon = 0x0010,
		Bear = 0x0020,
		Equine = 0x0040,
		Bull = 0x0080,
		Boura = 0x0100,
		Raptor = 0x0200
	}

	public enum ScaleType
	{
		Red,
		Yellow,
		Black,
		Green,
		White,
		Blue,
		All
	}

	public enum MeatType
	{
		Ribs,
		Bird,
		LambLeg,
		Rotworm
	}

	public enum HideType
	{
		Regular,
		Spined,
		Horned,
		Barbed
	}

	public enum FurType
	{
		Green,
		Red,
		Yellow,
		DarkGreen
	}

	public enum PetLoyalty
	{
		None,
		Confused,
		ExtremelyUnhappy,
		RatherUnhappy,
		Unhappy,
		SomewhatContent,
		Content,
		Happy,
		RatherHappy,
		VeryHappy,
		ExtremelyHappy,
		WonderfullyHappy
	}
	#endregion

	#region Events
	public delegate void LootGeneratedEventHandler( BaseCreature bc, LootGeneratedEventArgs args );

	public class LootGeneratedEventArgs : EventArgs
	{
		public LootGeneratedEventArgs()
		{
		}
	}
	#endregion

	public class DamageStore : IComparable
	{
		public Mobile Mobile;
		public int Damage;
		public bool HasRight;
		public double DamagePercent;

		public DamageStore( Mobile m, int damage )
		{
			Mobile = m;
			Damage = damage;
		}

		public int CompareTo( object obj )
		{
			DamageStore ds = (DamageStore) obj;

			return ds.Damage - Damage;
		}
	}

	public class BaseCreature : Mobile, IHonorTarget
	{
		#region Var declarations
		private BaseAI m_AI;				// THE AI

		private AIType m_CurrentAI;			// The current AI
		private AIType m_DefaultAI;			// The default AI

		private Mobile m_FocusMob;			// Use focus mob instead of combatant, maybe we don't whan to fight
		private FightMode m_FightMode;		// The style the mob uses

		private int m_iRangePerception;		// The view area
		private int m_iRangeFight;			// The fight distance

		private bool m_bDebugAI;			// Show debug AI messages

		private int m_iTeam;				// Monster Team

		private double m_dActiveSpeed;		// Timer speed when active
		private double m_dPassiveSpeed;		// Timer speed when not active
		private double m_dCurrentSpeed;		// The current speed, lets say it could be changed by something;

		private Point3D m_pHome;			// The home position of the creature, used by some AI
		private int m_iRangeHome = 10;		// The home range of the creature

		private bool m_bControlled;			// Is controlled
		private Mobile m_ControlMaster;		// My master
		private Mobile m_ControlTarget;		// My target mobile
		private Point3D m_ControlDest;		// My target destination (patrol)
		private OrderType m_ControlOrder;	// My order

		private PetLoyalty m_Loyalty;

		private double m_dMinTameSkill;
		private bool m_bTamable;

		private bool m_bSummoned = false;
		private DateTime m_SummonEnd;
		private int m_iControlSlots = 1;

		private bool m_bBardProvoked = false;
		private bool m_bBardPacified = false;
		private Mobile m_bBardMaster = null;
		private Mobile m_bBardTarget = null;
		private DateTime m_timeBardEnd;
		private WayPoint m_CurrentWayPoint = null;
		private Point2D m_TargetLocation = Point2D.Zero;

		private Mobile m_SummonMaster;

		private int m_HitsMax = -1;
		private int m_StamMax = -1;
		private int m_ManaMax = -1;
		private int m_DamageMin = -1;
		private int m_DamageMax = -1;

		private int m_PhysicalResistance, m_PhysicalDamage = 100;
		private int m_FireResistance, m_FireDamage;
		private int m_ColdResistance, m_ColdDamage;
		private int m_PoisonResistance, m_PoisonDamage;
		private int m_EnergyResistance, m_EnergyDamage;

		private List<Mobile> m_Owners;
		private List<Mobile> m_Friends;

		private bool m_IsStabled;

		private bool m_HasGeneratedLoot; // have we generated our loot yet?

		private bool m_Paragon;

		private bool m_IsChampionMonster;
		private int m_SpawnLevel;
		private ChampionSpawnType m_ChampionType;

		private bool m_IsMinichampMonster;
		private MiniChampType m_MinichampType;

		private bool m_StolenFrom;

		private DateTime m_NextTastyTreat;
		private DateTime m_NextArmorEssence;
		#endregion

		public virtual InhumanSpeech SpeechType { get { return null; } }

		public bool IsStabled
		{
			get { return m_IsStabled; }
			set { m_IsStabled = value; }
		}

		protected DateTime SummonEnd
		{
			get { return m_SummonEnd; }
			set { m_SummonEnd = value; }
		}

		public virtual Faction FactionAllegiance { get { return null; } }
		public virtual int FactionSilverWorth { get { return 30; } }

		#region Bonding
		public const bool BondingEnabled = true;

		public virtual bool IsBondable { get { return ( BondingEnabled && !Summoned && !IsGolem ); } }
		public virtual TimeSpan BondingDelay { get { return TimeSpan.FromDays( 7.0 ); } }
		public virtual TimeSpan BondingAbandonDelay { get { return TimeSpan.FromDays( 1.0 ); } }

		public override bool CanRegenHits { get { return !m_IsDeadPet && base.CanRegenHits; } }
		public override bool CanRegenStam { get { return !m_IsDeadPet && base.CanRegenStam; } }
		public override bool CanRegenMana { get { return !m_IsDeadPet && base.CanRegenMana; } }

		public override bool IsDeadBondedPet { get { return m_IsDeadPet; } }

		private DateTime m_NextPBlow;

		public DateTime NextPBlow
		{
			get { return m_NextPBlow; }
			set { m_NextPBlow = value; }
		}

		private DateTime m_NextDisarm;

		public DateTime NextDisarm
		{
			get { return m_NextDisarm; }
			set { m_NextDisarm = value; }
		}

		private bool m_IsBonded;
		private bool m_IsDeadPet;
		private DateTime m_BondingBegin;
		private DateTime m_OwnerAbandonTime;

		private bool m_IsGolem;

		[CommandProperty( AccessLevel.GameMaster )]
		public bool IsGolem
		{
			get { return m_IsGolem; }
			set { m_IsGolem = value; InvalidateProperties(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool IsBonded
		{
			get { return m_IsBonded; }
			set { m_IsBonded = value; InvalidateProperties(); }
		}

		public bool IsDeadPet
		{
			get { return m_IsDeadPet; }
			set { m_IsDeadPet = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public DateTime BondingBegin
		{
			get { return m_BondingBegin; }
			set { m_BondingBegin = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public DateTime OwnerAbandonTime
		{
			get { return m_OwnerAbandonTime; }
			set { m_OwnerAbandonTime = value; }
		}
		#endregion

		public virtual double WeaponAbilityChance { get { return 0.4; } }

		public virtual WeaponAbility GetWeaponAbility()
		{
			return null;
		}

		#region Elemental Resistance/Damage

		public override int BasePhysicalResistance { get { return m_PhysicalResistance; } }
		public override int BaseFireResistance { get { return m_FireResistance; } }
		public override int BaseColdResistance { get { return m_ColdResistance; } }
		public override int BasePoisonResistance { get { return m_PoisonResistance; } }
		public override int BaseEnergyResistance { get { return m_EnergyResistance; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int PhysicalResistanceSeed { get { return m_PhysicalResistance; } set { m_PhysicalResistance = value; UpdateResistances(); } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int FireResistSeed { get { return m_FireResistance; } set { m_FireResistance = value; UpdateResistances(); } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int ColdResistSeed { get { return m_ColdResistance; } set { m_ColdResistance = value; UpdateResistances(); } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int PoisonResistSeed { get { return m_PoisonResistance; } set { m_PoisonResistance = value; UpdateResistances(); } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int EnergyResistSeed { get { return m_EnergyResistance; } set { m_EnergyResistance = value; UpdateResistances(); } }


		[CommandProperty( AccessLevel.GameMaster )]
		public int PhysicalDamage { get { return m_PhysicalDamage; } set { m_PhysicalDamage = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int FireDamage { get { return m_FireDamage; } set { m_FireDamage = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int ColdDamage { get { return m_ColdDamage; } set { m_ColdDamage = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int PoisonDamage { get { return m_PoisonDamage; } set { m_PoisonDamage = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int EnergyDamage { get { return m_EnergyDamage; } set { m_EnergyDamage = value; } }

		#endregion

		[CommandProperty( AccessLevel.GameMaster )]
		public bool IsParagon
		{
			get { return m_Paragon; }
			set
			{
				if ( m_Paragon == value )
					return;
				else if ( value )
					Paragon.Convert( this );
				else
					Paragon.UnConvert( this );

				m_Paragon = value;

				InvalidateProperties();
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool IsChampionMonster { get { return m_IsChampionMonster; } set { m_IsChampionMonster = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int SpawnLevel { get { return m_SpawnLevel; } set { m_SpawnLevel = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public ChampionSpawnType ChampionType { get { return m_ChampionType; } set { m_ChampionType = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public bool IsMinichampMonster { get { return m_IsMinichampMonster; } set { m_IsMinichampMonster = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public MiniChampType MinichampType { get { return m_MinichampType; } set { m_MinichampType = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public bool StolenFrom { get { return m_StolenFrom; } set { m_StolenFrom = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public DateTime NextTastyTreat { get { return m_NextTastyTreat; } set { m_NextTastyTreat = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public DateTime NextArmorEssence { get { return m_NextArmorEssence; } set { m_NextArmorEssence = value; } }

		public virtual FoodType FavoriteFood { get { return FoodType.Meat; } }
		public virtual PackInstinct PackInstinct { get { return PackInstinct.None; } }

		public List<Mobile> Owners { get { return m_Owners; } }

		public virtual bool AllowMaleTamer { get { return true; } }
		public virtual bool AllowFemaleTamer { get { return true; } }
		public virtual bool SubdueBeforeTame { get { return false; } }
		public virtual bool StatLossAfterTame { get { return SubdueBeforeTame; } }

		public virtual bool Commandable { get { return true; } }

		public virtual Poison HitPoison { get { return null; } }
		public virtual double HitPoisonChance { get { return 0.5; } }
		public virtual Poison PoisonImmune { get { return null; } }

		public virtual bool BardImmune { get { return false; } }
		public virtual bool Unprovokable { get { return BardImmune || m_IsDeadPet; } }
		public virtual bool Uncalmable { get { return BardImmune || m_IsDeadPet; } }

		public virtual bool BleedImmune { get { return false; } }
		public virtual bool DeathAdderCharmable { get { return false; } }

		public virtual double DispelDifficulty { get { return 0.0; } } // at this skill level we dispel 50% chance
		public virtual double DispelFocus { get { return 20.0; } } // at difficulty - focus we have 0%, at difficulty + focus we have 100%

		public virtual LoyaltyGroup LoyaltyGroupEnemy { get { return LoyaltyGroup.GargoyleQueen; } }
		public virtual int LoyaltyPointsAward { get { return 0; } }

		public virtual bool InstantCast { get { return false; } }
		public virtual double ChangeCombatantChance { get { return 0.0; } }

		#region Breath ability, like dragon fire breath
		private DateTime m_NextBreathTime;

		// Must be overriden in subclass to enable
		public virtual bool HasBreath { get { return false; } }

		// Base damage given is: CurrentHitPoints * BreathDamageScalar
		public virtual double BreathDamageScalar { get { return 0.16; } }

		// Min/max seconds until next breath
		public virtual double BreathMinDelay { get { return 10.0; } }
		public virtual double BreathMaxDelay { get { return 15.0; } }

		// Creature stops moving for 1.0 seconds while breathing
		public virtual double BreathStallTime { get { return 1.0; } }

		// Effect is sent 1.3 seconds after BreathAngerSound and BreathAngerAnimation is played
		public virtual double BreathEffectDelay { get { return 1.3; } }

		// Damage is given 1.0 seconds after effect is sent
		public virtual double BreathDamageDelay { get { return 1.0; } }

		public virtual int BreathRange { get { return RangePerception; } }

		// Damage types
		public virtual int BreathPhysicalDamage { get { return 0; } }
		public virtual int BreathFireDamage { get { return 100; } }
		public virtual int BreathColdDamage { get { return 0; } }
		public virtual int BreathPoisonDamage { get { return 0; } }
		public virtual int BreathEnergyDamage { get { return 0; } }

		// Effect details and sound
		public virtual int BreathEffectItemID { get { return 0x36D4; } }
		public virtual int BreathEffectSpeed { get { return 5; } }
		public virtual int BreathEffectDuration { get { return 0; } }
		public virtual bool BreathEffectExplodes { get { return false; } }
		public virtual bool BreathEffectFixedDir { get { return false; } }
		public virtual int BreathEffectHue { get { return 0; } }
		public virtual int BreathEffectRenderMode { get { return 0; } }

		public virtual int BreathEffectSound { get { return 0x227; } }

		// Anger sound/animations
		public virtual int BreathAngerSound { get { return GetAngerSound(); } }
		public virtual int BreathAngerAnimation { get { return 12; } }

		public virtual void BreathStart( Mobile target )
		{
			BreathStallMovement();
			BreathPlayAngerSound();
			BreathPlayAngerAnimation();

			this.Direction = this.GetDirectionTo( target );

			Timer.DelayCall( TimeSpan.FromSeconds( BreathEffectDelay ), new TimerStateCallback( BreathEffect_Callback ), target );
		}

		public virtual void BreathStallMovement()
		{
			if ( m_AI != null )
				m_AI.NextMove = DateTime.Now + TimeSpan.FromSeconds( BreathStallTime );
		}

		public virtual void BreathPlayAngerSound()
		{
			PlaySound( BreathAngerSound );
		}

		public virtual void BreathPlayAngerAnimation()
		{
			Animate( BreathAngerAnimation, 5, 1, true, false, 0 );
		}

		public virtual void BreathEffect_Callback( object state )
		{
			Mobile target = (Mobile) state;

			if ( !target.Alive || !CanBeHarmful( target ) )
				return;

			BreathPlayEffectSound();
			BreathPlayEffect( target );

			Timer.DelayCall( TimeSpan.FromSeconds( BreathDamageDelay ), new TimerStateCallback( BreathDamage_Callback ), target );
		}

		public virtual void BreathPlayEffectSound()
		{
			PlaySound( BreathEffectSound );
		}

		public virtual void BreathPlayEffect( Mobile target )
		{
			Effects.SendMovingEffect( this, target, BreathEffectItemID,
				BreathEffectSpeed, BreathEffectDuration, BreathEffectFixedDir,
				BreathEffectExplodes, BreathEffectHue, BreathEffectRenderMode );
		}

		public virtual void BreathDamage_Callback( object state )
		{
			Mobile target = (Mobile) state;

			if ( CanBeHarmful( target ) )
			{
				DoHarmful( target );
				BreathDealDamage( target );
			}
		}

		public virtual void BreathDealDamage( Mobile target )
		{
			if ( Spells.Bushido.Evasion.IsEvading( target ) )
				return;

			int physDamage = BreathPhysicalDamage;
			int fireDamage = BreathFireDamage;
			int coldDamage = BreathColdDamage;
			int poisDamage = BreathPoisonDamage;
			int nrgyDamage = BreathEnergyDamage;

			if ( physDamage == 0 && fireDamage == 0 && coldDamage == 0 && poisDamage == 0 && nrgyDamage == 0 )
			{ // Unresistable damage even in AOS
				target.Damage( BreathComputeDamage(), this );
			}
			else
			{
				AOS.Damage( target, this, BreathComputeDamage(), physDamage, fireDamage, coldDamage, poisDamage, nrgyDamage );
			}
		}

		public virtual int BreathComputeDamage()
		{
			int damage = (int) ( Hits * BreathDamageScalar );

			if ( IsParagon )
				damage = (int) ( damage / Paragon.HitsBuff );

			return damage;
		}
		#endregion

		#region Spill Acid

		public void SpillAcid( int amount )
		{
			SpillAcid( null, amount );
		}

		public void SpillAcid( Mobile target, int amount )
		{
			if ( ( target != null && target.Map == null ) || this.Map == null )
				return;

			for ( int i = 0; i < amount; ++i )
			{
				Point3D loc = this.Location;
				Map map = this.Map;
				Item acid = NewHarmfulItem();

				if ( target != null && target.Map != null && amount == 1 )
				{
					loc = target.Location;
					map = target.Map;
				}
				else
				{
					bool validLocation = false;
					for ( int j = 0; !validLocation && j < 10; ++j )
					{
						loc = new Point3D(
							loc.X + ( Utility.Random( 0, 3 ) - 2 ),
							loc.Y + ( Utility.Random( 0, 3 ) - 2 ),
							loc.Z );
						loc.Z = map.GetAverageZ( loc.X, loc.Y );
						validLocation = map.CanFit( loc, 16, false, false );
					}
				}

				acid.MoveToWorld( loc, map );
			}
		}

		/// <summary>
		/// Solen Style, override me for other mobiles/items:
		/// kappa+acidslime, grizzles+whatever, etc.
		/// </summary>
		public virtual Item NewHarmfulItem()
		{
			return new PoolOfAcid( TimeSpan.FromSeconds( 10 ), 30, 30 );
		}

		#endregion

		#region Flee!!!
		public virtual bool CanFlee { get { return !m_Paragon; } }

		private DateTime m_EndFlee;

		public DateTime EndFleeTime
		{
			get { return m_EndFlee; }
			set { m_EndFlee = value; }
		}

		public virtual void StopFlee()
		{
			m_EndFlee = DateTime.MinValue;
		}

		public virtual bool CheckFlee()
		{
			if ( m_EndFlee == DateTime.MinValue )
				return false;

			if ( DateTime.Now >= m_EndFlee )
			{
				StopFlee();
				return false;
			}

			return true;
		}

		public virtual void BeginFlee( TimeSpan maxDuration )
		{
			m_EndFlee = DateTime.Now + maxDuration;
		}
		#endregion

		public BaseAI AIObject { get { return m_AI; } }

		public const int MaxOwners = 5;

		public virtual OppositionGroup OppositionGroup
		{
			get { return null; }
		}

		#region Friends
		public List<Mobile> Friends { get { return m_Friends; } }

		public virtual bool AllowNewPetFriend
		{
			get { return ( m_Friends == null || m_Friends.Count < 5 ); }
		}

		public virtual bool IsPetFriend( Mobile m )
		{
			return ( m_Friends != null && m_Friends.Contains( m ) );
		}

		public virtual void AddPetFriend( Mobile m )
		{
			if ( m_Friends == null )
				m_Friends = new List<Mobile>( 8 );

			m_Friends.Add( m );
		}

		public virtual void RemovePetFriend( Mobile m )
		{
			if ( m_Friends != null )
				m_Friends.Remove( m );
		}

		public virtual bool IsFriend( Mobile m )
		{
			OppositionGroup g = this.OppositionGroup;

			if ( g != null && g.IsEnemy( this, m ) )
				return false;

			if ( !( m is BaseCreature ) )
				return false;

			BaseCreature c = (BaseCreature) m;

			return ( m_iTeam == c.m_iTeam && ( ( m_bSummoned || m_bControlled ) == ( c.m_bSummoned || c.m_bControlled ) ) );
		}
		#endregion

		public virtual bool IsEnemy( Mobile m )
		{
			OppositionGroup g = this.OppositionGroup;

			if ( g != null && g.IsEnemy( this, m ) )
				return true;

			if ( m is BaseGuard )
				return false;

			Faction ourFaction = FactionAllegiance;

			if ( ourFaction != null && Map == Faction.Facet )
			{
				Faction theirFaction = Faction.Find( m, true );

				if ( theirFaction == ourFaction )
					return false;
			}

			if ( m is PlayerMobile )
			{
				PlayerMobile pm = (PlayerMobile) m;

				if ( pm.Combatant != null && pm.KRStartingQuestStep == 24 )
					return true;

				if ( pm.KRStartingQuestStep > 0 )
					return false;
			}

			if ( !Controlled && BaseMaskOfDeathPotion.UnderEffect( m ) )
				return false;

			if ( Server.Spells.Spellweaving.EtherealVoyageSpell.UnderEffect( m ) )
				return false;

			if ( !( m is BaseCreature ) || m is Server.Engines.Quests.Haven.MilitiaFighter )
				return true;

			if ( m is PlayerMobile && ( (PlayerMobile) m ).HonorActive )
				return false;

			BaseCreature c = (BaseCreature) m;

			return ( m_iTeam != c.m_iTeam || ( ( m_bSummoned || m_bControlled ) != ( c.m_bSummoned || c.m_bControlled ) ) );
		}

		public override string ApplyNameSuffix( string suffix )
		{
			if ( IsParagon )
			{
				if ( suffix.Length == 0 )
					suffix = "(Paragon)";
				else
					suffix = String.Concat( suffix, " (Paragon)" );
			}

			#region Factions
			if ( this.Map == Faction.Facet )
			{
				Faction faction = Faction.Find( this, inherit: true, creatureAllegiances: true );

				if ( faction != null )
				{
					string adjunct = String.Format( "[{0}]", faction.Definition.Abbreviation );
					if ( suffix.Length == 0 )
						suffix = adjunct;
					else
						suffix = String.Concat( suffix, " ", adjunct );
				}
			}
			#endregion

			return base.ApplyNameSuffix( suffix );
		}

		public virtual bool CheckControlChance( Mobile m )
		{
			return CheckControlChance( m, 0.0 );
		}

		public virtual bool CheckControlChance( Mobile m, double offset )
		{
			double v = GetControlChance( m ) + offset;

			if ( v > Utility.RandomDouble() )
				return true;

			if ( Body.IsAnimal )
				Animate( 10, 5, 1, true, false, 0 );
			else if ( Body.IsMonster )
				Animate( 18, 5, 1, true, false, 0 );

			if ( ( IsBonded ? 0.25 : 0.5 ) > Utility.RandomDouble() )
				--Loyalty;

			if ( Loyalty == PetLoyalty.Confused )
			{
				Say( 1043270, Name ); // * ~1_NAME~ looks around desperately *
				PlaySound( GetIdleSound() );
			}
			else if ( Loyalty == PetLoyalty.None )
			{
				Say( 1043255, Name ); // ~1_NAME~ appears to have decided that is better off without a master!
				Loyalty = PetLoyalty.WonderfullyHappy;
				IsBonded = false;
				BondingBegin = DateTime.MinValue;
				OwnerAbandonTime = DateTime.MinValue;
				ControlTarget = null;
				AIObject.DoOrderRelease(); // this will prevent no release of creatures left alone with AI disabled (and consequent bug of Followers)
				RemoveOnSave = true;
			}
			else
			{
				PlaySound( GetAngerSound() );
			}

			return false;
		}

		public virtual bool CanBeControlledBy( Mobile m )
		{
			return ( GetControlChance( m ) > 0.0 );
		}

		public virtual double GetControlChance( Mobile m )
		{
			if ( m_dMinTameSkill <= 29.1 || m_bSummoned || m.AccessLevel >= AccessLevel.GameMaster )
				return 1.0;

			double dMinTameSkill = m_dMinTameSkill;

			if ( dMinTameSkill > -24.9 && Server.SkillHandlers.AnimalTaming.CheckMastery( m, this ) )
				dMinTameSkill = -24.9;

			int tamingModifier = 6;
			int loreModifier = 6;

			double tamingBonus, loreBonus;

			if ( ( tamingBonus = ( m.Skills[SkillName.AnimalTaming].Value - dMinTameSkill ) ) < 0 )
				tamingModifier = 28;

			if ( ( loreBonus = ( m.Skills[SkillName.AnimalLore].Value - dMinTameSkill ) ) < 0 )
				loreModifier = 14;

			tamingBonus *= tamingModifier;
			loreBonus *= loreModifier;

			double skillBonus = ( tamingBonus + loreBonus ) / 2;

			double chance = ( 70 + skillBonus ) / 100;

			if ( chance >= 0 && chance < 0.2 )
				chance = 0.2;
			else if ( chance > 0.99 )
				chance = 0.99;

			return chance;
		}

		private static Type[] m_AnimateDeadTypes = new Type[]
			{
				typeof( MoundOfMaggots ),	typeof( HellSteed ),	typeof( SkeletalMount ),
				typeof( WailingBanshee ),	typeof( Wraith ),		typeof( SkeletalDragon ),
				typeof( LichLord ),			typeof( FleshGolem ),	typeof( Lich ),
				typeof( SkeletalKnight ),	typeof( BoneKnight ),	typeof( Mummy ),
				typeof( SkeletalMage ),		typeof( BoneMagi ),		typeof( PatchworkSkeleton )
			};

		public virtual bool IsAnimatedDead
		{
			get
			{
				if ( !Summoned )
					return false;

				Type type = this.GetType();

				bool contains = false;

				for ( int i = 0; !contains && i < m_AnimateDeadTypes.Length; ++i )
					contains = ( type == m_AnimateDeadTypes[i] );

				return contains;
			}
		}

		public override void Damage( int amount, Mobile from )
		{
			int oldHits = this.Hits;

			base.Damage( amount, from );

			if ( SubdueBeforeTame && !Controlled )
			{
				if ( ( oldHits > ( this.HitsMax / 10 ) ) && ( this.Hits <= ( this.HitsMax / 10 ) ) )
					PublicOverheadMessage( MessageType.Regular, 0x3B2, true, "* The creature has been beaten into subjugation! *" );
			}
		}

		public virtual bool DeleteCorpseOnDeath { get { return false; } }

		public override void SetLocation( Point3D newLocation, bool isTeleport )
		{
			base.SetLocation( newLocation, isTeleport );

			if ( isTeleport && m_AI != null )
				m_AI.OnTeleported();
		}

		public void CheckParagon()
		{
			if ( Paragon.CheckConvert( this ) )
				IsParagon = true;
		}

		public override ApplyPoisonResult ApplyPoison( Mobile from, Poison poison )
		{
			if ( !Alive || IsDeadPet )
				return ApplyPoisonResult.Immune;

			if ( Spells.Necromancy.EvilOmenSpell.CheckEffect( this ) )
				return base.ApplyPoison( from, PoisonImpl.IncreaseLevel( poison ) );

			return base.ApplyPoison( from, poison );
		}

		public override bool CheckPoisonImmunity( Mobile from, Poison poison )
		{
			if ( base.CheckPoisonImmunity( from, poison ) )
				return true;

			Poison p = this.PoisonImmune;

			return ( p != null && p.Level >= poison.Level );
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public PetLoyalty Loyalty
		{
			get
			{
				return m_Loyalty;
			}
			set
			{
				m_Loyalty = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public WayPoint CurrentWayPoint
		{
			get
			{
				return m_CurrentWayPoint;
			}
			set
			{
				m_CurrentWayPoint = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Point2D TargetLocation
		{
			get
			{
				return m_TargetLocation;
			}
			set
			{
				m_TargetLocation = value;
			}
		}

		public virtual Mobile ConstantFocus { get { return null; } }

		public virtual bool DisallowAllMoves
		{
			get
			{
				return false;
			}
		}

		public virtual bool InitialInnocent
		{
			get
			{
				return false;
			}
		}

		public virtual bool AlwaysMurderer
		{
			get
			{
				return false;
			}
		}

		public virtual bool AlwaysAttackable
		{
			get
			{
				return false;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public virtual int DamageMin { get { return m_DamageMin; } set { m_DamageMin = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public virtual int DamageMax { get { return m_DamageMax; } set { m_DamageMax = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public override int HitsMax
		{
			get
			{
				if ( m_HitsMax >= 0 )
					return m_HitsMax;

				return Str;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int HitsMaxSeed
		{
			get { return m_HitsMax; }
			set { m_HitsMax = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public override int StamMax
		{
			get
			{
				if ( m_StamMax >= 0 )
					return m_StamMax;

				return Dex;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int StamMaxSeed
		{
			get { return m_StamMax; }
			set { m_StamMax = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public override int ManaMax
		{
			get
			{
				if ( m_ManaMax >= 0 )
					return m_ManaMax;

				return Int;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int ManaMaxSeed
		{
			get { return m_ManaMax; }
			set { m_ManaMax = value; }
		}

		public virtual bool CanOpenDoors
		{
			get
			{
				return !this.Body.IsAnimal && !this.Body.IsSea;
			}
		}

		public virtual bool CanMoveOverObstacles { get { return true; } }

		public virtual bool CanDestroyObstacles
		{
			get
			{
				// to enable breaking of furniture, 'return CanMoveOverObstacles;'
				return false;
			}
		}

		public void Unpacify()
		{
			BardEndTime = DateTime.Now;
			BardPacified = false;
		}

		private HonorContext m_ReceivedHonorContext;

		public HonorContext ReceivedHonorContext { get { return m_ReceivedHonorContext; } set { m_ReceivedHonorContext = value; } }

		public override void OnDamage( int amount, Mobile from, bool willKill )
		{
			if ( BardPacified && ( HitsMax - Hits ) * 0.001 > Utility.RandomDouble() )
				Unpacify();

			if ( BaseExplodingTarPotion.IsSlept( this ) )
				BaseExplodingTarPotion.RemoveEffect( this );

			int disruptThreshold;

			if ( from != null && from.IsPlayer )
				disruptThreshold = 18;
			else
				disruptThreshold = 25;

			if ( amount > disruptThreshold )
			{
				BandageContext c = BandageContext.GetContext( this );

				if ( c != null )
					c.Slip();
			}

			if ( Confidence.IsRegenerating( this ) )
				Confidence.StopRegenerating( this );

			WeightOverloading.FatigueOnDamage( this, amount );

			InhumanSpeech speechType = this.SpeechType;

			if ( speechType != null && !willKill )
				speechType.OnDamage( this, amount );

			if ( m_ReceivedHonorContext != null )
				m_ReceivedHonorContext.OnTargetDamaged( from, amount );

			base.OnDamage( amount, from, willKill );
		}

		public virtual void OnDamagedBySpell( Mobile from )
		{
		}

		#region Alter[...]Damage From/To

		public virtual void AlterDamageScalarFrom( Mobile caster, ref double scalar )
		{
		}

		public virtual void AlterDamageScalarTo( Mobile target, ref double scalar )
		{
		}

		public virtual void AlterSpellDamageFrom( Mobile from, ref int damage )
		{
		}

		public virtual void AlterSpellDamageTo( Mobile to, ref int damage )
		{
		}

		public virtual void AlterMeleeDamageFrom( Mobile from, ref int damage )
		{
		}

		public virtual void AlterMeleeDamageTo( Mobile to, ref int damage )
		{
		}
		#endregion


		public virtual void CheckReflect( Mobile caster, ref bool reflect )
		{
		}

		public virtual void OnCarve( Mobile from, Corpse corpse, bool butcher )
		{
			int feathers = Feathers;
			int wool = Wool;
			int meat = Meat;
			int hides = Hides;
			int scales = Scales;
			int blood = Blood;
			int fur = Fur;

			if ( ( feathers == 0 && wool == 0 && meat == 0 && hides == 0 && scales == 0 && blood == 0 && fur == 0 ) || Summoned || IsBonded )
			{
				from.SendLocalizedMessage( 500485 ); // You see nothing useful to carve from the corpse.
			}
			else
			{
				if ( from.Race == Race.Human )
					hides = (int) Math.Ceiling( hides * 1.1 ); // 10% Bonus Only applies to Hides, Ore & Logs

				if ( corpse.Map == Map.Felucca )
				{
					feathers *= 2;
					wool *= 2;
					meat *= 2;
					hides *= 2;
					scales *= 2;
					blood *= 2;
					fur *= 2;
				}

				new Blood( 0x122D ).MoveToWorld( corpse.Location, corpse.Map );

				if ( feathers != 0 )
				{
					corpse.AddCarvedItem( new Feather( feathers ), from );
					from.SendLocalizedMessage( 500479 ); // You pluck the bird. The feathers are now on the corpse.
				}

				if ( wool != 0 )
				{
					corpse.AddCarvedItem( new Wool( wool ), from );
					from.SendLocalizedMessage( 500483 ); // You shear it, and the wool is now on the corpse.
				}

				if ( meat != 0 )
				{
					if ( MeatType == MeatType.Ribs )
						corpse.AddCarvedItem( new RawRibs( meat ), from );
					else if ( MeatType == MeatType.Bird )
						corpse.AddCarvedItem( new RawBird( meat ), from );
					else if ( MeatType == MeatType.LambLeg )
						corpse.AddCarvedItem( new RawLambLeg( meat ), from );
					else if ( MeatType == MeatType.Rotworm )
						corpse.AddCarvedItem( new RawRotwormMeat( meat ), from );

					from.SendLocalizedMessage( 500467 ); // You carve some meat, which remains on the corpse.
				}

				if ( hides != 0 )
				{
					if ( butcher )
					{
						Item item = null;

						if ( HideType == HideType.Regular )
							item = new Leather( hides );
						else if ( HideType == HideType.Spined )
							item = new SpinedLeather( hides );
						else if ( HideType == HideType.Horned )
							item = new HornedLeather( hides );
						else if ( HideType == HideType.Barbed )
							item = new BarbedLeather( hides );

						if ( item != null )
						{
							if ( from.AddToBackpack( item ) )
							{
								from.SendLocalizedMessage( 1073555 ); // You skin it and place the cut-up hides in your backpack.
							}
							else
							{
								corpse.AddCarvedItem( item, from );
								from.SendLocalizedMessage( 500471 ); // You skin it, and the hides are now in the corpse.
							}
						}
					}
					else
					{
						if ( HideType == HideType.Regular )
							corpse.AddCarvedItem( new Hides( hides ), from );
						else if ( HideType == HideType.Spined )
							corpse.AddCarvedItem( new SpinedHides( hides ), from );
						else if ( HideType == HideType.Horned )
							corpse.AddCarvedItem( new HornedHides( hides ), from );
						else if ( HideType == HideType.Barbed )
							corpse.AddCarvedItem( new BarbedHides( hides ), from );

						from.SendLocalizedMessage( 500471 ); // You skin it, and the hides are now in the corpse.
					}
				}

				if ( scales != 0 )
				{
					ScaleType sc = this.ScaleType;

					switch ( sc )
					{
						case ScaleType.Red:
							corpse.AddCarvedItem( new RedScales( scales ), from );
							break;
						case ScaleType.Yellow:
							corpse.AddCarvedItem( new YellowScales( scales ), from );
							break;
						case ScaleType.Black:
							corpse.AddCarvedItem( new BlackScales( scales ), from );
							break;
						case ScaleType.Green:
							corpse.AddCarvedItem( new GreenScales( scales ), from );
							break;
						case ScaleType.White:
							corpse.AddCarvedItem( new WhiteScales( scales ), from );
							break;
						case ScaleType.Blue:
							corpse.AddCarvedItem( new BlueScales( scales ), from );
							break;
						case ScaleType.All:
							{
								corpse.AddCarvedItem( new RedScales( scales ), from );
								corpse.AddCarvedItem( new YellowScales( scales ), from );
								corpse.AddCarvedItem( new BlackScales( scales ), from );
								corpse.AddCarvedItem( new GreenScales( scales ), from );
								corpse.AddCarvedItem( new WhiteScales( scales ), from );
								corpse.AddCarvedItem( new BlueScales( scales ), from );
								break;
							}
					}

					from.SendLocalizedMessage( 1079284 ); // You cut away some scales, but they remain on the corpse.
				}

				if ( blood != 0 )
				{
					corpse.AddCarvedItem( new DragonsBlood( blood ), from );
					from.SendLocalizedMessage( 1094946 ); // Some blood is left on the corpse.
				}

				if ( fur != 0 )
				{
					if ( FurType == FurType.Green )
						corpse.AddCarvedItem( new GreenFur( fur ), from );
					else if ( FurType == FurType.Red )
						corpse.AddCarvedItem( new RedFur( fur ), from );
					else if ( FurType == FurType.Yellow )
						corpse.AddCarvedItem( new YellowFur( fur ), from );
					else if ( FurType == FurType.DarkGreen )
						corpse.AddCarvedItem( new DarkGreenFur( fur ), from );

					from.SendLocalizedMessage( 1112765 ); // You shear it, and the fur is now on the corpse.
				}

				corpse.Carved = true;

				if ( corpse.IsCriminalAction( from ) )
					from.CriminalAction( true );
			}
		}

		public const int DefaultRangePerception = 16;
		public const int OldRangePerception = 10;

		public BaseCreature( AIType ai,
			FightMode mode,
			int iRangePerception,
			int iRangeFight,
			double dActiveSpeed,
			double dPassiveSpeed )
		{
			if ( iRangePerception == OldRangePerception )
				iRangePerception = DefaultRangePerception;

			m_Loyalty = PetLoyalty.WonderfullyHappy;

			m_CurrentAI = ai;
			m_DefaultAI = ai;

			m_iRangePerception = iRangePerception;
			m_iRangeFight = iRangeFight;

			m_FightMode = mode;

			m_iTeam = 0;

			SpeedInfo.GetSpeeds( this, ref dActiveSpeed, ref dPassiveSpeed );

			m_dActiveSpeed = dActiveSpeed;
			m_dPassiveSpeed = dPassiveSpeed;
			m_dCurrentSpeed = dPassiveSpeed;

			m_bDebugAI = false;

			m_bControlled = false;
			m_ControlMaster = null;
			m_ControlTarget = null;
			m_ControlOrder = OrderType.None;

			m_bTamable = false;

			m_Owners = new List<Mobile>();

			m_NextReacquireTime = DateTime.Now + ReacquireDelay;

			ChangeAIType( AI );

			InhumanSpeech speechType = this.SpeechType;

			if ( speechType != null )
				speechType.OnConstruct( this );

			GenerateLoot( true );

			Timer.DelayCall( TimeSpan.FromSeconds( 0.01 ), new TimerCallback( CheckParagon ) );
		}

		public BaseCreature( Serial serial )
			: base( serial )
		{
			m_bDebugAI = false;
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 25 ); // version

			writer.Write( (int) m_CurrentAI );
			writer.Write( (int) m_DefaultAI );

			writer.Write( (int) m_iRangePerception );
			writer.Write( (int) m_iRangeFight );

			writer.Write( (int) m_iTeam );

			writer.Write( (double) m_dActiveSpeed );
			writer.Write( (double) m_dPassiveSpeed );
			writer.Write( (double) m_dCurrentSpeed );

			writer.Write( (int) m_pHome.X );
			writer.Write( (int) m_pHome.Y );
			writer.Write( (int) m_pHome.Z );

			// Version 1
			writer.Write( (int) m_iRangeHome );

			// Version 2
			writer.Write( (int) m_FightMode );

			writer.Write( (bool) m_bControlled );
			writer.Write( (Mobile) m_ControlMaster );
			writer.Write( (Mobile) m_ControlTarget );
			writer.Write( (Point3D) m_ControlDest );
			writer.Write( (int) m_ControlOrder );
			writer.Write( (double) m_dMinTameSkill );
			writer.Write( (bool) m_bTamable );
			writer.Write( (bool) m_bSummoned );

			if ( m_bSummoned )
				writer.WriteDeltaTime( m_SummonEnd );

			writer.Write( (int) m_iControlSlots );

			// Version 3
			writer.Write( (int) m_Loyalty );

			// Version 4 
			writer.Write( m_CurrentWayPoint );

			// Verison 5
			writer.Write( m_SummonMaster );

			// Version 6
			writer.Write( (int) m_HitsMax );
			writer.Write( (int) m_StamMax );
			writer.Write( (int) m_ManaMax );
			writer.Write( (int) m_DamageMin );
			writer.Write( (int) m_DamageMax );

			// Version 7
			writer.Write( (int) m_PhysicalResistance );
			writer.Write( (int) m_PhysicalDamage );

			writer.Write( (int) m_FireResistance );
			writer.Write( (int) m_FireDamage );

			writer.Write( (int) m_ColdResistance );
			writer.Write( (int) m_ColdDamage );

			writer.Write( (int) m_PoisonResistance );
			writer.Write( (int) m_PoisonDamage );

			writer.Write( (int) m_EnergyResistance );
			writer.Write( (int) m_EnergyDamage );

			// Version 8
			writer.WriteMobileList( m_Owners, true );

			// Version 10
			writer.Write( (bool) m_IsDeadPet );
			writer.Write( (bool) m_IsBonded );
			writer.Write( (DateTime) m_BondingBegin );
			writer.Write( (DateTime) m_OwnerAbandonTime );

			// Version 11
			writer.Write( (bool) m_HasGeneratedLoot );

			// Version 12
			writer.Write( (bool) m_Paragon );
			writer.Write( (bool) m_IsChampionMonster );

			// Version 13
			writer.Write( (bool) ( m_Friends != null && m_Friends.Count > 0 ) );

			if ( m_Friends != null && m_Friends.Count > 0 )
				writer.WriteMobileList( m_Friends, true );

			// Version 14
			writer.Write( (int) m_SpawnLevel );

			// Version 15
			writer.Write( (int) m_ChampionType );

			// Version 17
			writer.Write( (bool) m_StolenFrom );

			// Version 18
			writer.Write( (int) m_RummagedItems.Count );

			foreach ( KeyValuePair<Item, Mobile> kvp in m_RummagedItems )
			{
				writer.Write( kvp.Key );
				writer.Write( kvp.Value );
			}

			// Version 20
			writer.Write( (bool) m_IsMinichampMonster );
			writer.Write( (int) m_MinichampType );

			// Version 21
			writer.Write( (DateTime) m_NextTastyTreat );

			// Version 22
			writer.Write( (bool) m_Petrified );

			if ( m_Petrified )
			{
				writer.Write( (int) m_StatueAnimation );
				writer.Write( (int) m_StatueFrames );
			}

			// Version 23
			writer.Write( (DateTime) m_NextArmorEssence );

			// Version 24
			writer.Write( (bool) m_IsGolem );
		}

		private static double[] m_StandardActiveSpeeds = new double[]
			{
				0.175, 0.1, 0.15, 0.2, 0.25, 0.3, 0.4, 0.5, 0.6, 0.8
			};

		private static double[] m_StandardPassiveSpeeds = new double[]
			{
				0.350, 0.2, 0.4, 0.5, 0.6, 0.8, 1.0, 1.2, 1.6, 2.0
			};

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			if ( version >= 16 && version <= 18 )
				reader.ReadItem(); // spawner

			m_CurrentAI = (AIType) reader.ReadInt();
			m_DefaultAI = (AIType) reader.ReadInt();

			m_iRangePerception = reader.ReadInt();
			m_iRangeFight = reader.ReadInt();

			m_iTeam = reader.ReadInt();

			m_dActiveSpeed = reader.ReadDouble();
			m_dPassiveSpeed = reader.ReadDouble();
			m_dCurrentSpeed = reader.ReadDouble();

			double activeSpeed = m_dActiveSpeed;
			double passiveSpeed = m_dPassiveSpeed;

			SpeedInfo.GetSpeeds( this, ref activeSpeed, ref passiveSpeed );

			bool isStandardActive = false;
			for ( int i = 0; !isStandardActive && i < m_StandardActiveSpeeds.Length; ++i )
			{
				isStandardActive = ( m_dActiveSpeed == m_StandardActiveSpeeds[i] );
			}

			bool isStandardPassive = false;
			for ( int i = 0; !isStandardPassive && i < m_StandardPassiveSpeeds.Length; ++i )
			{
				isStandardPassive = ( m_dPassiveSpeed == m_StandardPassiveSpeeds[i] );
			}

			if ( isStandardActive && m_dCurrentSpeed == m_dActiveSpeed )
			{
				m_dCurrentSpeed = activeSpeed;
			}
			else if ( isStandardPassive && m_dCurrentSpeed == m_dPassiveSpeed )
			{
				m_dCurrentSpeed = passiveSpeed;
			}

			if ( isStandardActive )
			{
				m_dActiveSpeed = activeSpeed;
			}

			if ( isStandardPassive )
			{
				m_dPassiveSpeed = passiveSpeed;
			}

			if ( m_iRangePerception == OldRangePerception )
			{
				m_iRangePerception = DefaultRangePerception;
			}

			m_pHome.X = reader.ReadInt();
			m_pHome.Y = reader.ReadInt();
			m_pHome.Z = reader.ReadInt();

			if ( version >= 1 )
			{
				m_iRangeHome = reader.ReadInt();

				if ( version < 25 )
				{
					int i, iCount;

					iCount = reader.ReadInt();
					for ( i = 0; i < iCount; i++ )
						reader.ReadString();

					iCount = reader.ReadInt();
					for ( i = 0; i < iCount; i++ )
						reader.ReadString();
				}
			}
			else
			{
				m_iRangeHome = 0;
			}

			if ( version >= 2 )
			{
				m_FightMode = (FightMode) reader.ReadInt();

				m_bControlled = reader.ReadBool();
				m_ControlMaster = reader.ReadMobile();
				m_ControlTarget = reader.ReadMobile();
				m_ControlDest = reader.ReadPoint3D();
				m_ControlOrder = (OrderType) reader.ReadInt();

				m_dMinTameSkill = reader.ReadDouble();

				if ( version < 9 )
					reader.ReadDouble();

				m_bTamable = reader.ReadBool();
				m_bSummoned = reader.ReadBool();

				if ( m_bSummoned )
				{
					m_SummonEnd = reader.ReadDeltaTime();
					new UnsummonTimer( m_ControlMaster, this, m_SummonEnd - DateTime.Now ).Start();
				}

				m_iControlSlots = reader.ReadInt();
			}
			else
			{
				m_FightMode = FightMode.Closest;

				m_bControlled = false;
				m_ControlMaster = null;
				m_ControlTarget = null;
				m_ControlOrder = OrderType.None;
			}

			if ( version >= 3 )
				m_Loyalty = (PetLoyalty) reader.ReadInt();
			else
				m_Loyalty = PetLoyalty.WonderfullyHappy;

			if ( version >= 4 )
				m_CurrentWayPoint = reader.ReadItem() as WayPoint;

			if ( version >= 5 )
				m_SummonMaster = reader.ReadMobile();

			if ( version >= 6 )
			{
				m_HitsMax = reader.ReadInt();
				m_StamMax = reader.ReadInt();
				m_ManaMax = reader.ReadInt();
				m_DamageMin = reader.ReadInt();
				m_DamageMax = reader.ReadInt();
			}

			if ( version >= 7 )
			{
				m_PhysicalResistance = reader.ReadInt();
				m_PhysicalDamage = reader.ReadInt();

				m_FireResistance = reader.ReadInt();
				m_FireDamage = reader.ReadInt();

				m_ColdResistance = reader.ReadInt();
				m_ColdDamage = reader.ReadInt();

				m_PoisonResistance = reader.ReadInt();
				m_PoisonDamage = reader.ReadInt();

				m_EnergyResistance = reader.ReadInt();
				m_EnergyDamage = reader.ReadInt();
			}

			if ( version >= 8 )
				m_Owners = reader.ReadStrongMobileList();
			else
				m_Owners = new List<Mobile>();

			if ( version >= 10 )
			{
				m_IsDeadPet = reader.ReadBool();
				m_IsBonded = reader.ReadBool();
				m_BondingBegin = reader.ReadDateTime();
				m_OwnerAbandonTime = reader.ReadDateTime();
			}

			if ( version >= 11 )
				m_HasGeneratedLoot = reader.ReadBool();
			else
				m_HasGeneratedLoot = true;

			if ( version >= 12 )
			{
				m_Paragon = reader.ReadBool();
				m_IsChampionMonster = reader.ReadBool();
			}
			else
			{
				m_Paragon = false;
				m_IsChampionMonster = false;
			}

			if ( version >= 13 && reader.ReadBool() )
				m_Friends = reader.ReadStrongMobileList();
			else if ( version < 13 && m_ControlOrder >= OrderType.Unfriend )
				++m_ControlOrder;

			if ( version >= 14 )
				m_SpawnLevel = reader.ReadInt();
			else
				m_SpawnLevel = 0;

			if ( version >= 15 )
				m_ChampionType = (ChampionSpawnType) reader.ReadInt();

			if ( version >= 17 )
				m_StolenFrom = reader.ReadBool();

			if ( version >= 18 )
			{
				int count = reader.ReadInt();

				for ( int i = 0; i < count; i++ )
				{
					Item rummaged = reader.ReadItem();
					Mobile owner = reader.ReadMobile();

					if ( rummaged != null && owner != null )
						m_RummagedItems.Add( rummaged, owner );
				}
			}

			if ( version >= 20 )
			{
				m_IsMinichampMonster = reader.ReadBool();
				m_MinichampType = (MiniChampType) reader.ReadInt();
			}

			if ( version >= 21 )
			{
				m_NextTastyTreat = reader.ReadDateTime();
			}

			if ( version >= 22 )
			{
				m_Petrified = reader.ReadBool();

				if ( m_Petrified )
				{
					m_StatueAnimation = reader.ReadInt();
					m_StatueFrames = reader.ReadInt();

					Frozen = true;
					HueMod = 2401;
				}
			}

			if ( version >= 23 )
			{
				m_NextArmorEssence = reader.ReadDateTime();
			}

			if ( version >= 24 )
			{
				m_IsGolem = reader.ReadBool();
			}
			else
			{
				m_IsGolem = this is Golem;
			}

			CheckStatTimers();

			ChangeAIType( m_CurrentAI );

			AddFollowers();

			if ( IsAnimatedDead )
				Spells.Necromancy.AnimateDeadSpell.Register( m_SummonMaster, this );

			if ( Controlled )
				Skills.Cap = 15000;
		}

		public virtual bool IsHumanInTown()
		{
			return ( Body.IsHuman && Region.IsPartOf( typeof( Regions.GuardedRegion ) ) );
		}

		public virtual bool CheckGold( Mobile from, Item dropped )
		{
			if ( dropped is Gold )
				return OnGoldGiven( from, (Gold) dropped );

			return false;
		}

		public virtual bool OnGoldGiven( Mobile from, Gold dropped )
		{
			if ( CheckTeachingMatch( from ) )
			{
				if ( Teach( m_Teaching, from, dropped.Amount, true ) )
				{
					dropped.Delete();
					return true;
				}
			}
			else if ( IsHumanInTown() )
			{
				Direction = this.GetDirectionTo( from );

				int oldSpeechHue = this.SpeechHue;

				this.SpeechHue = 0x23F;
				SayTo( from, "Thou art giving me gold?" );

				if ( dropped.Amount >= 400 )
					SayTo( from, "'Tis a noble gift." );
				else
					SayTo( from, "Money is always welcome." );

				this.SpeechHue = 0x3B2;
				SayTo( from, 501548 ); // I thank thee.

				this.SpeechHue = oldSpeechHue;

				dropped.Delete();
				return true;
			}

			return false;
		}

		public override bool ShouldCheckStatTimers { get { return false; } }

		#region Food
		private static Type[] m_Eggs = new Type[]
			{
				typeof( FriedEggs ), typeof( Eggs )
			};

		private static Type[] m_Fish = new Type[]
			{
				typeof( FishSteak ), typeof( RawFishSteak )
			};

		private static Type[] m_GrainsAndHay = new Type[]
			{
				typeof( BreadLoaf ), typeof( FrenchBread ), typeof( SheafOfHay )
			};

		private static Type[] m_Meat = new Type[]
			{
				/* Cooked */
				typeof( Bacon ), typeof( CookedBird ), typeof( Sausage ),
				typeof( Ham ), typeof( Ribs ), typeof( LambLeg ),
				typeof( ChickenLeg ),

				/* Uncooked */
				typeof( RawBird ), typeof( RawRibs ), typeof( RawLambLeg ),
				typeof( RawChickenLeg ),

				/* Body Parts */
				typeof( Head ), typeof( LeftArm ), typeof( LeftLeg ),
				typeof( Torso ), typeof( RightArm ), typeof( RightLeg )
			};

		private static Type[] m_FruitsAndVegies = new Type[]
			{
				typeof( HoneydewMelon ), typeof( YellowGourd ), typeof( GreenGourd ),
				typeof( Banana ), typeof( Bananas ), typeof( Lemon ), typeof( Lime ),
				typeof( Dates ), typeof( Grapes ), typeof( Peach ), typeof( Pear ),
				typeof( Apple ), typeof( Watermelon ), typeof( Squash ),
				typeof( Cantaloupe ), typeof( Carrot ), typeof( Cabbage ),
				typeof( Onion ), typeof( Lettuce ), typeof( Pumpkin )
			};

		private static Type[] m_Gold = new Type[]
			{
				// white wyrms eat gold..
				typeof( Gold )
			};

		public virtual bool CheckFoodPreference( Item f )
		{
			if ( CheckFoodPreference( f, FoodType.Eggs, m_Eggs ) )
				return true;

			if ( CheckFoodPreference( f, FoodType.Fish, m_Fish ) )
				return true;

			if ( CheckFoodPreference( f, FoodType.GrainsAndHay, m_GrainsAndHay ) )
				return true;

			if ( CheckFoodPreference( f, FoodType.Meat, m_Meat ) )
				return true;

			if ( CheckFoodPreference( f, FoodType.FruitsAndVegies, m_FruitsAndVegies ) )
				return true;

			if ( CheckFoodPreference( f, FoodType.Gold, m_Gold ) )
				return true;

			return false;
		}

		public virtual bool CheckFoodPreference( Item fed, FoodType type, Type[] types )
		{
			if ( ( FavoriteFood & type ) == 0 )
				return false;

			Type fedType = fed.GetType();
			bool contains = false;

			for ( int i = 0; !contains && i < types.Length; ++i )
				contains = ( fedType == types[i] );

			return contains;
		}

		public virtual bool CheckFeed( Mobile from, Item dropped )
		{
			if ( !IsDeadPet && Controlled && ( ControlMaster == from || IsPetFriend( from ) ) && ( dropped is Food || dropped is Gold || dropped is CookableFood || dropped is Head || dropped is LeftArm || dropped is LeftLeg || dropped is Torso || dropped is RightArm || dropped is RightLeg || dropped is IPetBooster ) )
			{
				Item f = dropped;

				if ( dropped is IPetBooster )
				{
					IPetBooster pb = dropped as IPetBooster;

					return pb.OnUsed( from, this );
				}
				else if ( CheckFoodPreference( f ) )
				{
					int amount = f.Amount;

					if ( amount > 0 )
					{
						bool happier = false;

						int stamGain;

						if ( f is Gold )
							stamGain = amount - 50;
						else
							stamGain = ( amount * 15 ) - 50;

						if ( stamGain > 0 )
							Stam += stamGain;

						if ( m_Loyalty != PetLoyalty.WonderfullyHappy )
						{
							m_Loyalty = PetLoyalty.WonderfullyHappy;
							happier = true;
						}

						#region PetBondingGate
						if ( TestCenter.Enabled )
						{
							if ( !IsBonded )
							{
								var overPetBondingGate = from.GetItemsInRange( 5 ).OfType<PetBondingGate>()
									.Any( pbg => pbg.Location == from.Location && pbg.Location == Location );

								if ( overPetBondingGate )
								{
									IsBonded = true;
									BondingBegin = DateTime.MinValue;
									from.SendLocalizedMessage( 1049666 ); // Your pet has bonded with you!	
								}
							}
						}
						#endregion

						if ( happier )
							SayTo( from, 502060 ); // Your pet looks happier.

						if ( Body.IsAnimal )
							Animate( 3, 5, 1, true, false, 0 );
						else if ( Body.IsMonster )
							Animate( 17, 5, 1, true, false, 0 );

						if ( IsBondable && !IsBonded )
						{
							Mobile master = m_ControlMaster;

							if ( master != null && master == from )	//So friends can't start the bonding process
							{
								if ( m_dMinTameSkill <= 29.1 || master.Skills[SkillName.AnimalTaming].Base >= m_dMinTameSkill || this is SwampDragon || this is Ridgeback || this is SavageRidgeback || this is FireBeetle || this is LesserHiryu || this is IronBeetle )
								{
									if ( BondingBegin == DateTime.MinValue )
									{
										BondingBegin = DateTime.Now;
									}
									else if ( ( BondingBegin + BondingDelay ) <= DateTime.Now )
									{
										IsBonded = true;
										BondingBegin = DateTime.MinValue;
										from.SendLocalizedMessage( 1049666 ); // Your pet has bonded with you!
									}
								}
								else
								{
									from.SendLocalizedMessage( 1075268 ); // Your pet cannot form a bond with you until your animal taming ability has risen.
								}
							}
						}

						dropped.Delete();
						return true;
					}
				}
				else if ( !( dropped is Gold ) )
				{
					SayTo( from, 1043257 ); // The animal shies away.
				}
			}

			return false;
		}

		#endregion

		public virtual bool CanAngerOnTame { get { return false; } }

		#region OnAction[...]
		public virtual void OnActionWander()
		{
		}

		public virtual void OnActionCombat()
		{
		}

		public virtual void OnActionGuard()
		{
		}

		public virtual void OnActionFlee()
		{
		}

		public virtual void OnActionInteract()
		{
		}

		public virtual void OnActionBackoff()
		{
		}
		#endregion

		public override bool OnDragDrop( Mobile from, Item dropped )
		{
			if ( CheckFeed( from, dropped ) )
				return true;
			else if ( CheckGold( from, dropped ) )
				return true;

			return base.OnDragDrop( from, dropped );
		}

		protected virtual BaseAI ForcedAI { get { return null; } }

		public virtual void ChangeAIType( AIType NewAI )
		{
			if ( m_AI != null )
				m_AI.m_Timer.Stop();

			if ( ForcedAI != null )
			{
				m_AI = ForcedAI;
				return;
			}

			m_AI = null;

			if ( this is BaseFactionGuard )
			{
				m_AI = new FactionGuardAI( (BaseFactionGuard) this );
				return;
			}

			switch ( NewAI )
			{
				case AIType.AI_Melee:
					m_AI = new MeleeAI( this );
					break;
				case AIType.AI_Animal:
					m_AI = new AnimalAI( this );
					break;
				case AIType.AI_Berserk:
					m_AI = new BerserkAI( this );
					break;
				case AIType.AI_Archer:
					m_AI = new ArcherAI( this );
					break;
				case AIType.AI_Healer:
					m_AI = new HealerAI( this );
					break;
				case AIType.AI_Vendor:
					m_AI = new VendorAI( this );
					break;
				case AIType.AI_Mage:
					m_AI = new MageAI( this );
					break;
				case AIType.AI_Arcanist:
					m_AI = new ArcanistAI( this );
					break;
				case AIType.AI_Predator:
					m_AI = new MeleeAI( this );
					break;
				case AIType.AI_Thief:
					m_AI = new ThiefAI( this );
					break;
				case AIType.AI_Ninja:
					m_AI = new NinjaAI( this );
					break;
				case AIType.AI_Samurai:
					m_AI = new SamuraiAI( this );
					break;
				case AIType.AI_ChampionMelee:
					m_AI = new ChampionMeleeAI( this );
					break;
				case AIType.AI_BoneDemon:
					m_AI = new BoneDemonAI( this );
					break;
				case AIType.AI_BossMelee:
					m_AI = new BossMeleeAI( this );
					break;
				case AIType.AI_Necromancer:
					m_AI = new NecromancerAI( this );
					break;
				case AIType.AI_Mephitis:
					m_AI = new MephitisAI( this );
					break;
				case AIType.AI_OrcScout:
					m_AI = new OrcScoutAI( this );
					break;
				case AIType.AI_Mystic:
					m_AI = new MysticAI( this );
					break;
				case AIType.AI_Ambusher:
					m_AI = new AmbusherAI( this );
					break;
				case AIType.AI_WeakMage:
					m_AI = new WeakMageAI( this );
					break;
			}
		}

		public virtual void ChangeAIToDefault()
		{
			ChangeAIType( m_DefaultAI );
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public AIType AI
		{
			get
			{
				return m_CurrentAI;
			}
			set
			{
				m_CurrentAI = value;

				if ( m_CurrentAI == AIType.AI_Use_Default )
					m_CurrentAI = m_DefaultAI;

				ChangeAIType( m_CurrentAI );
			}
		}

		[CommandProperty( AccessLevel.Administrator )]
		public bool Debug
		{
			get
			{
				return m_bDebugAI;
			}
			set
			{
				m_bDebugAI = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int Team
		{
			get
			{
				return m_iTeam;
			}
			set
			{
				m_iTeam = value;

				OnTeamChange();
			}
		}

		public virtual void OnTeamChange()
		{
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile FocusMob
		{
			get
			{
				return m_FocusMob;
			}
			set
			{
				m_FocusMob = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public FightMode FightMode
		{
			get
			{
				return m_FightMode;
			}
			set
			{
				m_FightMode = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int RangePerception
		{
			get
			{
				return m_iRangePerception;
			}
			set
			{
				m_iRangePerception = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int RangeFight
		{
			get
			{
				return m_iRangeFight;
			}
			set
			{
				m_iRangeFight = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int RangeHome
		{
			get
			{
				return m_iRangeHome;
			}
			set
			{
				m_iRangeHome = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public double ActiveSpeed
		{
			get
			{
				return m_dActiveSpeed;
			}
			set
			{
				m_dActiveSpeed = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public double PassiveSpeed
		{
			get
			{
				return m_dPassiveSpeed;
			}
			set
			{
				m_dPassiveSpeed = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public double CurrentSpeed
		{
			get
			{
				return m_dCurrentSpeed;
			}
			set
			{
				if ( m_dCurrentSpeed != value )
				{
					m_dCurrentSpeed = value;

					if ( m_AI != null )
						m_AI.OnCurrentSpeedChanged();
				}
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Point3D Home
		{
			get
			{
				return m_pHome;
			}
			set
			{
				m_pHome = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Controlled
		{
			get
			{
				return m_bControlled;
			}
			set
			{
				if ( m_bControlled == value )
					return;

				m_bControlled = value;
				Delta( MobileDelta.Noto );

				InvalidateProperties();
			}
		}

		#region Snake Charming
		private Mobile m_CharmMaster;
		private Point2D m_CharmTarget;
		private Timer m_CharmTimer;

		public void BeginCharm( Mobile master, Point2D target )
		{
			m_CharmMaster = master;
			m_CharmTarget = target;

			m_CharmTimer = new CharmTimer( this );
			m_CharmTimer.Start();
		}

		public void EndCharm()
		{
			if ( !Deleted && m_CharmMaster != null )
			{
				// The charm seems to wear off.
				PrivateOverheadMessage( MessageType.Regular, 0x3B2, 1112181, m_CharmMaster.NetState );

				Frozen = false;

				m_CharmMaster = null;
				m_CharmTarget = Point2D.Zero;

				if ( m_CharmTimer != null )
				{
					m_CharmTimer.Stop();
					m_CharmTimer = null;
				}
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile CharmMaster { get { return m_CharmMaster; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public Point2D CharmTarget { get { return m_CharmTarget; } }

		private class CharmTimer : Timer
		{
			private BaseCreature m_Owner;
			private int m_Count;

			public CharmTimer( BaseCreature owner )
				: base( TimeSpan.Zero, TimeSpan.FromSeconds( 2.0 ) )
			{
				m_Owner = owner;
				m_Count = 10;
			}

			protected override void OnTick()
			{
				if ( m_Count == 0 || m_Owner.CharmMaster == null || !m_Owner.CharmMaster.InRange( m_Owner.Location, 10 ) )
				{
					Stop();
					m_Owner.EndCharm();
				}
				else
				{
					m_Owner.FixedParticles( 0x376A, 9, 32, 5030, EffectLayer.Waist );
					m_Count--;
				}
			}
		}
		#endregion

		public override void RevealingAction()
		{
			Spells.Sixth.InvisibilitySpell.RemoveTimer( this );

			base.RevealingAction();
		}

		public void RemoveFollowers()
		{
			if ( m_ControlMaster != null )
				m_ControlMaster.Followers -= ControlSlots;
			else if ( m_SummonMaster != null )
				m_SummonMaster.Followers -= ControlSlots;

			if ( m_ControlMaster != null && m_ControlMaster.Followers < 0 )
				m_ControlMaster.Followers = 0;

			if ( m_SummonMaster != null && m_SummonMaster.Followers < 0 )
				m_SummonMaster.Followers = 0;
		}

		public void AddFollowers()
		{
			if ( m_ControlMaster != null )
				m_ControlMaster.Followers += ControlSlots;
			else if ( m_SummonMaster != null )
				m_SummonMaster.Followers += ControlSlots;
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile ControlMaster
		{
			get
			{
				return m_ControlMaster;
			}
			set
			{
				if ( m_ControlMaster == value )
					return;

				RemoveFollowers();
				m_ControlMaster = value;
				AddFollowers();

				Delta( MobileDelta.Noto );
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile SummonMaster
		{
			get
			{
				return m_SummonMaster;
			}
			set
			{
				if ( m_SummonMaster == value )
					return;

				RemoveFollowers();
				m_SummonMaster = value;
				AddFollowers();

				Delta( MobileDelta.Noto );
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile ControlTarget
		{
			get
			{
				return m_ControlTarget;
			}
			set
			{
				m_ControlTarget = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Point3D ControlDest
		{
			get
			{
				return m_ControlDest;
			}
			set
			{
				m_ControlDest = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public OrderType ControlOrder
		{
			get
			{
				return m_ControlOrder;
			}
			set
			{
				m_ControlOrder = value;

				InvalidateProperties();

				if ( m_AI != null )
					m_AI.OnCurrentOrderChanged();
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool BardProvoked
		{
			get
			{
				return m_bBardProvoked;
			}
			set
			{
				m_bBardProvoked = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool BardPacified
		{
			get
			{
				return m_bBardPacified;
			}
			set
			{
				m_bBardPacified = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile BardMaster
		{
			get
			{
				return m_bBardMaster;
			}
			set
			{
				m_bBardMaster = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile BardTarget
		{
			get
			{
				return m_bBardTarget;
			}
			set
			{
				m_bBardTarget = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public DateTime BardEndTime
		{
			get
			{
				return m_timeBardEnd;
			}
			set
			{
				m_timeBardEnd = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public double MinTameSkill
		{
			get
			{
				return m_dMinTameSkill;
			}
			set
			{
				m_dMinTameSkill = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Tamable
		{
			get
			{
				return m_bTamable && !m_Paragon;
			}
			set
			{
				m_bTamable = value;
			}
		}

		[CommandProperty( AccessLevel.Administrator )]
		public bool Summoned
		{
			get
			{
				return m_bSummoned;
			}
			set
			{
				if ( m_bSummoned == value )
					return;

				m_NextReacquireTime = DateTime.Now;

				m_bSummoned = value;
				Delta( MobileDelta.Noto );

				InvalidateProperties();
			}
		}

		[CommandProperty( AccessLevel.Administrator )]
		public int ControlSlots
		{
			get
			{
				return m_iControlSlots;
			}
			set
			{
				m_iControlSlots = value;
			}
		}

		public virtual bool NoHouseRestrictions { get { return false; } }
		public virtual bool IsHouseSummonable { get { return false; } }

		#region Corpse Resources
		public virtual int Feathers { get { return 0; } }
		public virtual int Wool { get { return 0; } }

		public virtual int Meat { get { return 0; } }
		public virtual MeatType MeatType { get { return MeatType.Ribs; } }

		public virtual int Hides { get { return 0; } }
		public virtual HideType HideType { get { return HideType.Regular; } }

		public virtual int Scales { get { return 0; } }
		public virtual ScaleType ScaleType { get { return ScaleType.Red; } }

		public virtual int Blood { get { return 0; } }

		public virtual int Fur { get { return 0; } }
		public virtual FurType FurType { get { return FurType.Green; } }
		#endregion

		public virtual bool AutoDispel { get { return false; } }
		public virtual double AutoDispelChance { get { return 0.1; } }

		public virtual bool IsScaryToPets { get { return false; } }
		public virtual bool IsScaredOfScaryThings { get { return true; } }

		public virtual bool CanRummageCorpses { get { return false; } }

		public virtual void OnGotMeleeAttack( Mobile attacker )
		{
			if ( AutoDispel && attacker is BaseCreature && ( (BaseCreature) attacker ).IsDispellable && AutoDispelChance > Utility.RandomDouble() )
				Dispel( attacker );
		}

		public virtual void Dispel( Mobile m )
		{
			Effects.SendLocationParticles( EffectItem.Create( m.Location, m.Map, EffectItem.DefaultDuration ), 0x3728, 8, 20, 5042 );
			Effects.PlaySound( m, m.Map, 0x201 );

			m.Delete();
		}

		public virtual bool DeleteOnRelease { get { return m_bSummoned; } }

		public virtual void OnGaveMeleeAttack( Mobile defender )
		{
			Poison p = HitPoison;

			if ( m_Paragon )
				p = PoisonImpl.IncreaseLevel( p );

			if ( p != null && HitPoisonChance >= Utility.RandomDouble() )
				defender.ApplyPoison( this, p );

			if ( AutoDispel && defender is BaseCreature && ( (BaseCreature) defender ).Summoned && !( (BaseCreature) defender ).IsAnimatedDead )
				Dispel( defender );
		}

		public override void OnAfterDelete()
		{
			if ( m_AI != null )
			{
				if ( m_AI.m_Timer != null )
					m_AI.m_Timer.Stop();

				m_AI = null;
			}

			FocusMob = null;

			if ( IsAnimatedDead )
				Spells.Necromancy.AnimateDeadSpell.Unregister( m_SummonMaster, this );

			base.OnAfterDelete();
		}

		public bool IsInvolvedInTrade()
		{
			return m_AI != null && m_AI.IsInvolvedInTrade();
		}

		public void DebugSay( string text )
		{
			if ( m_bDebugAI )
				this.PublicOverheadMessage( MessageType.Regular, 41, false, text );
		}

		public void DebugSay( string format, params object[] args )
		{
			if ( m_bDebugAI )
				this.PublicOverheadMessage( MessageType.Regular, 41, false, String.Format( format, args ) );
		}

		/*
		 * Will need to be givent a better name
		 * 
		 * This function can be overriden.. so a "Strongest" mobile, can have a different definition depending
		 * on who check for value
		 * -Could add a FightMode.Prefered
		 * 
		 */
		public virtual double GetValueFrom( Mobile m, FightMode acqType, bool bPlayerOnly )
		{
			if ( ( bPlayerOnly && m.IsPlayer ) || !bPlayerOnly )
			{
				switch ( acqType )
				{
					case FightMode.Strongest:
						return ( m.Skills[SkillName.Tactics].Value + m.Str ); //returns strongest mobile

					case FightMode.Weakest:
						return -m.Hits; // returns weakest mobile

					default:
						return -this.GetDistanceToSqrt( m ); // returns closest mobile
				}
			}
			else
			{
				return double.MinValue;
			}
		}

		// Turn, - for let, + for right
		// Basic for now, needs work
		public virtual void Turn( int iTurnSteps )
		{
			int v = (int) Direction;

			Direction = (Direction) ( ( ( ( v & 0x7 ) + iTurnSteps ) & 0x7 ) | ( v & 0x80 ) );
		}

		public virtual void TurnInternal( int iTurnSteps )
		{
			int v = (int) Direction;

			SetDirection( (Direction) ( ( ( ( v & 0x7 ) + iTurnSteps ) & 0x7 ) | ( v & 0x80 ) ) );
		}

		public bool IsHurt()
		{
			return ( Hits != HitsMax );
		}

		public double GetHomeDistance()
		{
			return this.GetDistanceToSqrt( m_pHome );
		}

		public virtual int GetTeamSize( int iRange )
		{
			int iCount = 0;

			foreach ( Mobile m in this.GetMobilesInRange( iRange ) )
			{
				if ( m is BaseCreature )
				{
					if ( ( (BaseCreature) m ).Team == Team )
					{
						if ( !m.Deleted )
						{
							if ( m != this )
							{
								if ( CanSee( m ) )
								{
									iCount++;
								}
							}
						}
					}
				}
			}

			return iCount;
		}

		// Do my combatant is attaking me??
		public bool IsCombatantAnAggressor()
		{
			if ( Combatant != null )
			{
				if ( Combatant.Combatant == this )
					return true;
			}
			return false;
		}

		private class TameEntry : ContextMenuEntry
		{
			private BaseCreature m_Mobile;

			public TameEntry( Mobile from, BaseCreature creature )
				: base( 6130, 6 )
			{
				m_Mobile = creature;

				Enabled = Enabled && ( from.Female ? creature.AllowFemaleTamer : creature.AllowMaleTamer );
			}

			public override void OnClick()
			{
				if ( !Owner.From.CheckAlive() )
					return;

				Owner.From.TargetLocked = true;
				SkillHandlers.AnimalTaming.DisableMessage = true;

				if ( Owner.From.UseSkill( SkillName.AnimalTaming ) )
					Owner.From.Target.Invoke( Owner.From, m_Mobile );

				SkillHandlers.AnimalTaming.DisableMessage = false;
				Owner.From.TargetLocked = false;
			}
		}

		private class RenameEntry : ContextMenuEntry
		{
			private Mobile m_From;
			private BaseCreature m_Creature;

			public RenameEntry( Mobile from, BaseCreature creature )
				: base( 1111680, 6 )
			{
				m_From = from;
				m_Creature = creature;
			}

			public override void OnClick()
			{
				if ( !m_Creature.Deleted && m_Creature.Controlled && m_Creature.ControlMaster == m_From )
					m_From.Prompt = new RenamePrompt( m_Creature );
			}
		}

		public class RenamePrompt : Prompt
		{
			// Enter a new name for your pet.
			public override int MessageCliloc { get { return 1115558; } }

			private BaseCreature m_Creature;

			public RenamePrompt( BaseCreature creature )
				: base( creature )
			{
				m_Creature = creature;
			}

			public override void OnCancel( Mobile from )
			{
				from.SendLocalizedMessage( 501806 ); // Request cancelled.
			}

			public override void OnResponse( Mobile from, string text )
			{
				if ( !m_Creature.Deleted && m_Creature.Controlled && m_Creature.ControlMaster == from )
				{
					if ( Utility.IsAlpha( text ) )
					{
						m_Creature.Name = text;
						from.SendLocalizedMessage( 1115559 ); // Pet name changed.
					}
					else
					{
						from.SendLocalizedMessage( 1075246 ); // That name is not valid.
					}
				}
			}
		}

		#region Teaching
		public virtual bool CanTeach { get { return false; } }

		public virtual bool CheckTeach( SkillName skill, Mobile from )
		{
			if ( !CanTeach )
				return false;

			if ( skill == SkillName.Stealth && from.Skills[SkillName.Hiding].Base < 30.0 )
				return false;

			if ( skill == SkillName.RemoveTrap && ( from.Skills[SkillName.Lockpicking].Base < 50.0 || from.Skills[SkillName.DetectHidden].Base < 50.0 ) )
				return false;

			return true;
		}

		public enum TeachResult
		{
			Success,
			Failure,
			KnowsMoreThanMe,
			KnowsWhatIKnow,
			SkillNotRaisable,
			NotEnoughFreePoints
		}

		public virtual TeachResult CheckTeachSkills( SkillName skill, Mobile m, int maxPointsToLearn, ref int pointsToLearn, bool doTeach )
		{
			if ( !CheckTeach( skill, m ) || !m.CheckAlive() )
				return TeachResult.Failure;

			Skill ourSkill = Skills[skill];
			Skill theirSkill = m.Skills[skill];

			if ( ourSkill == null || theirSkill == null )
				return TeachResult.Failure;

			int baseToSet = ourSkill.BaseFixedPoint / 3;

			if ( baseToSet > 420 )
				baseToSet = 420;
			else if ( baseToSet < 200 )
				return TeachResult.Failure;

			if ( baseToSet > theirSkill.CapFixedPoint )
				baseToSet = theirSkill.CapFixedPoint;

			pointsToLearn = baseToSet - theirSkill.BaseFixedPoint;

			if ( maxPointsToLearn > 0 && pointsToLearn > maxPointsToLearn )
			{
				pointsToLearn = maxPointsToLearn;
				baseToSet = theirSkill.BaseFixedPoint + pointsToLearn;
			}

			if ( pointsToLearn < 0 )
				return TeachResult.KnowsMoreThanMe;

			if ( pointsToLearn == 0 )
				return TeachResult.KnowsWhatIKnow;

			if ( theirSkill.Lock != SkillLock.Up )
				return TeachResult.SkillNotRaisable;

			int freePoints = m.Skills.Cap - m.Skills.Total;
			int freeablePoints = 0;

			if ( freePoints < 0 )
				freePoints = 0;

			for ( int i = 0; ( freePoints + freeablePoints ) < pointsToLearn && i < m.Skills.Length; ++i )
			{
				Skill sk = m.Skills[i];

				if ( sk == theirSkill || sk.Lock != SkillLock.Down )
					continue;

				freeablePoints += sk.BaseFixedPoint;
			}

			if ( ( freePoints + freeablePoints ) == 0 )
				return TeachResult.NotEnoughFreePoints;

			if ( ( freePoints + freeablePoints ) < pointsToLearn )
			{
				pointsToLearn = freePoints + freeablePoints;
				baseToSet = theirSkill.BaseFixedPoint + pointsToLearn;
			}

			if ( doTeach )
			{
				int need = pointsToLearn - freePoints;

				for ( int i = 0; need > 0 && i < m.Skills.Length; ++i )
				{
					Skill sk = m.Skills[i];

					if ( sk == theirSkill || sk.Lock != SkillLock.Down )
						continue;

					if ( sk.BaseFixedPoint < need )
					{
						need -= sk.BaseFixedPoint;
						sk.BaseFixedPoint = 0;
					}
					else
					{
						sk.BaseFixedPoint -= need;
						need = 0;
					}
				}

				/* Sanity check */
				if ( baseToSet > theirSkill.CapFixedPoint || ( m.Skills.Total - theirSkill.BaseFixedPoint + baseToSet ) > m.Skills.Cap )
					return TeachResult.NotEnoughFreePoints;

				theirSkill.BaseFixedPoint = baseToSet;
			}

			return TeachResult.Success;
		}

		public virtual bool CheckTeachingMatch( Mobile m )
		{
			if ( m_Teaching == (SkillName) ( -1 ) )
				return false;

			if ( m is PlayerMobile )
				return ( ( (PlayerMobile) m ).Learning == m_Teaching );

			return true;
		}

		private SkillName m_Teaching = (SkillName) ( -1 );

		public virtual bool Teach( SkillName skill, Mobile m, int maxPointsToLearn, bool doTeach )
		{
			int pointsToLearn = 0;
			TeachResult res = CheckTeachSkills( skill, m, maxPointsToLearn, ref pointsToLearn, doTeach );

			switch ( res )
			{
				case TeachResult.KnowsMoreThanMe:
					{
						Say( 501508 ); // I cannot teach thee, for thou knowest more than I!
						break;
					}
				case TeachResult.KnowsWhatIKnow:
					{
						Say( 501509 ); // I cannot teach thee, for thou knowest all I can teach!
						break;
					}
				case TeachResult.NotEnoughFreePoints:
				case TeachResult.SkillNotRaisable:
					{
						// Make sure this skill is marked to raise. If you are near the skill cap (700 points) you may need to lose some points in another skill first.
						m.SendLocalizedMessage( 501510, "", 0x22 );
						break;
					}
				case TeachResult.Success:
					{
						if ( doTeach )
						{
							Say( 501539 ); // Let me show thee something of how this is done.
							m.SendLocalizedMessage( 501540 ); // Your skill level increases.

							m_Teaching = (SkillName) ( -1 );

							if ( m is PlayerMobile )
								( (PlayerMobile) m ).Learning = (SkillName) ( -1 );
						}
						else
						{
							// I will teach thee all I know, if paid the amount in full.  The price is:
							Say( 1019077, AffixType.Append, String.Format( " {0}", pointsToLearn ), "" );
							Say( 1043108 ); // For less I shall teach thee less.

							m_Teaching = skill;

							if ( m is PlayerMobile )
								( (PlayerMobile) m ).Learning = skill;
						}

						return true;
					}
			}

			return false;
		}
		#endregion

		public override void AggressiveAction( Mobile aggressor, bool criminal )
		{
			base.AggressiveAction( aggressor, criminal );

			if ( m_AI != null )
				m_AI.OnAggressiveAction( aggressor );

			PlayerMobile pm = aggressor as PlayerMobile;

			if ( pm != null )
			{
				QuestSystem qs = pm.Quest;

				if ( qs != null )
					qs.OnAttack( this );
			}

			StopFlee();

			ForceReacquire();

			OrderType ct = m_ControlOrder;

			if ( aggressor.ChangingCombatant && ( m_bControlled || m_bSummoned ) && ( ct == OrderType.Come || ct == OrderType.Stay || ct == OrderType.Stop || ct == OrderType.None || ct == OrderType.Follow ) )
			{
				ControlTarget = aggressor;
				ControlOrder = OrderType.Attack;
			}
			else if ( Combatant == null && !m_bBardPacified )
			{
				Warmode = true;
				Combatant = aggressor;
			}
		}

		public override bool OnMoveOver( Mobile m )
		{
			if ( m is BaseCreature && !this.IsDeadBondedPet && !( (BaseCreature) m ).Controlled )
				return false;

			return base.OnMoveOver( m );
		}

		public virtual void AddCustomContextEntries( Mobile from, List<ContextMenuEntry> list )
		{
		}

		public override void GetContextMenuEntries( Mobile from, List<ContextMenuEntry> list )
		{
			base.GetContextMenuEntries( from, list );

			if ( m_AI != null && Commandable )
				m_AI.GetContextMenuEntries( from, list );

			if ( m_bTamable && !m_bControlled && from.Alive )
				list.Add( new TameEntry( from, this ) );

			if ( m_bControlled && m_ControlMaster == from && !m_bSummoned )
				list.Add( new RenameEntry( from, this ) );

			AddCustomContextEntries( from, list );

			if ( CanTeach && from.Alive )
			{
				Skills ourSkills = this.Skills;
				Skills theirSkills = from.Skills;

				for ( int i = 0; i < ourSkills.Length && i < theirSkills.Length; ++i )
				{
					Skill skill = ourSkills[i];
					Skill theirSkill = theirSkills[i];

					if ( skill != null && theirSkill != null && skill.Base >= 60.0 && CheckTeach( skill.SkillName, from ) )
					{
						double toTeach = skill.Base / 3.0;

						if ( toTeach > 42.0 )
							toTeach = 42.0;

						list.Add( new TeachEntry( (SkillName) i, this, from, ( toTeach > theirSkill.Base ) ) );
					}
				}
			}
		}

		public override bool HandlesOnSpeech( Mobile from )
		{
			InhumanSpeech speechType = this.SpeechType;

			if ( speechType != null && ( speechType.Flags & IHSFlags.OnSpeech ) != 0 && from.InRange( this, 3 ) )
				return true;

			return ( m_AI != null && m_AI.HandlesOnSpeech( from ) && from.InRange( this, m_iRangePerception ) );
		}

		public override void OnSpeech( SpeechEventArgs e )
		{
			InhumanSpeech speechType = this.SpeechType;

			if ( speechType != null && speechType.OnSpeech( this, e.Mobile, e.Speech ) )
				e.Handled = true;
			else if ( !e.Handled && m_AI != null && e.Mobile.InRange( this, m_iRangePerception ) )
				m_AI.OnSpeech( e );
		}

		public override bool IsHarmfulCriminal( Mobile target )
		{
			if ( ( Controlled && target == m_ControlMaster ) || ( Summoned && target == m_SummonMaster ) )
				return false;

			if ( target is BaseCreature && ( (BaseCreature) target ).InitialInnocent && !( (BaseCreature) target ).Controlled )
				return false;

			if ( target is PlayerMobile && ( (PlayerMobile) target ).PermaFlags.Count > 0 )
				return false;

			return base.IsHarmfulCriminal( target );
		}

		public override void CriminalAction( bool message )
		{
			base.CriminalAction( message );

			if ( Controlled || Summoned )
			{
				if ( m_ControlMaster != null && m_ControlMaster.IsPlayer )
					m_ControlMaster.CriminalAction( false );
				else if ( m_SummonMaster != null && m_SummonMaster.IsPlayer )
					m_SummonMaster.CriminalAction( false );
			}
		}

		public override void DoHarmful( Mobile target, bool indirect )
		{
			base.DoHarmful( target, indirect );

			if ( target == this || target == m_ControlMaster || target == m_SummonMaster || ( !Controlled && !Summoned ) )
				return;

			List<AggressorInfo> list = this.Aggressors;

			for ( int i = 0; i < list.Count; ++i )
			{
				AggressorInfo ai = list[i];

				if ( ai.Attacker == target )
					return;
			}

			list = this.Aggressed;

			for ( int i = 0; i < list.Count; ++i )
			{
				AggressorInfo ai = list[i];

				if ( ai.Defender == target )
				{
					if ( m_ControlMaster != null && m_ControlMaster.IsPlayer && m_ControlMaster.CanBeHarmful( target, false ) )
						m_ControlMaster.DoHarmful( target, true );
					else if ( m_SummonMaster != null && m_SummonMaster.IsPlayer && m_SummonMaster.CanBeHarmful( target, false ) )
						m_SummonMaster.DoHarmful( target, true );

					return;
				}
			}
		}

		private static Mobile m_NoDupeGuards;

		public void ReleaseGuardDupeLock()
		{
			m_NoDupeGuards = null;
		}

		public void ReleaseGuardLock()
		{
			EndAction( typeof( GuardedRegion ) );
		}

		private DateTime m_IdleReleaseTime;

		public virtual bool CheckIdle()
		{
			if ( Combatant != null )
				return false; // in combat.. not idling

			if ( m_IdleReleaseTime > DateTime.MinValue )
			{
				// idling...

				if ( DateTime.Now >= m_IdleReleaseTime )
				{
					m_IdleReleaseTime = DateTime.MinValue;
					return false; // idle is over
				}

				return true; // still idling
			}

			if ( 95 > Utility.Random( 100 ) )
				return false; // not idling, but don't want to enter idle state

			m_IdleReleaseTime = DateTime.Now + TimeSpan.FromSeconds( Utility.RandomMinMax( 15, 25 ) );

			if ( Body.IsHuman )
			{
				switch ( Utility.Random( 2 ) )
				{
					case 0:
						Animate( 5, 5, 1, true, true, 1 );
						break;
					case 1:
						Animate( 6, 5, 1, true, false, 1 );
						break;
				}
			}
			else if ( Body.IsAnimal )
			{
				switch ( Utility.Random( 3 ) )
				{
					case 0:
						Animate( 3, 3, 1, true, false, 1 );
						break;
					case 1:
						Animate( 9, 5, 1, true, false, 1 );
						break;
					case 2:
						Animate( 10, 5, 1, true, false, 1 );
						break;
				}
			}
			else if ( Body.IsMonster )
			{
				switch ( Utility.Random( 2 ) )
				{
					case 0:
						Animate( 17, 5, 1, true, false, 1 );
						break;
					case 1:
						Animate( 18, 5, 1, true, false, 1 );
						break;
				}
			}

			PlaySound( GetIdleSound() );
			return true; // entered idle state
		}

		protected override void OnLocationChange( Point3D oldLocation )
		{
			Map map = this.Map;

			if ( PlayerRangeSensitive && m_AI != null && map != null && map.GetSector( this.Location ).Active )
				m_AI.Activate();

			base.OnLocationChange( oldLocation );
		}

		public override void OnMovement( Mobile m, Point3D oldLocation )
		{
			base.OnMovement( m, oldLocation );

			if ( ReacquireOnMovement || m_Paragon )
				ForceReacquire();

			if ( CausesTrueFear )
				CauseTrueFear( m, oldLocation );

			InhumanSpeech speechType = this.SpeechType;

			if ( speechType != null )
				speechType.OnMovement( this, m, oldLocation );

			/* Begin notice sound */
			if ( ( !m.Hidden || m.AccessLevel == AccessLevel.Player ) && m.IsPlayer && m_FightMode != FightMode.Aggressor && m_FightMode != FightMode.None && Combatant == null && !Controlled && !Summoned )
			{
				// If this creature defends itself but doesn't actively attack (animal) or
				// doesn't fight at all (vendor) then no notice sounds are played..
				// So, players are only notified of aggressive monsters

				// Monsters that are currently fighting are ignored

				// Controlled or summoned creatures are ignored

				if ( this.InRange( m.Location, 18 ) && !this.InRange( oldLocation, 18 ) )
				{
					if ( Body.IsMonster )
						Animate( 11, 5, 1, true, false, 1 );

					PlaySound( GetAngerSound() );
				}
			}
			/* End notice sound */

			if ( m_NoDupeGuards == m )
				return;

			if ( !Body.IsHuman || Kills >= 5 || AlwaysMurderer || AlwaysAttackable || m.Kills < 5 || !m.InRange( Location, 12 ) || !m.Alive )
				return;

			GuardedRegion guardedRegion = (GuardedRegion) this.Region.GetRegion( typeof( GuardedRegion ) );

			if ( guardedRegion != null )
			{
				if ( !guardedRegion.IsDisabled() && guardedRegion.IsGuardCandidate( m ) && BeginAction( typeof( GuardedRegion ) ) )
				{
					Say( 1013037 + Utility.Random( 16 ) );
					guardedRegion.CallGuards( this.Location );

					Timer.DelayCall( TimeSpan.FromSeconds( 5.0 ), new TimerCallback( ReleaseGuardLock ) );

					m_NoDupeGuards = m;
					Timer.DelayCall( TimeSpan.Zero, new TimerCallback( ReleaseGuardDupeLock ) );
				}
			}
		}

		#region Set[...]

		public void SetDamage( int val )
		{
			m_DamageMin = val;
			m_DamageMax = val;
		}

		public void SetDamage( int min, int max )
		{
			m_DamageMin = min;
			m_DamageMax = max;
		}

		public void SetHits( int val )
		{
			m_HitsMax = val;
			Hits = HitsMax;
		}

		public void SetHits( int min, int max )
		{
			m_HitsMax = Utility.RandomMinMax( min, max );
			Hits = HitsMax;
		}

		public void SetStam( int val )
		{
			m_StamMax = val;
			Stam = StamMax;
		}

		public void SetStam( int min, int max )
		{
			m_StamMax = Utility.RandomMinMax( min, max );
			Stam = StamMax;
		}

		public void SetMana( int val )
		{
			m_ManaMax = val;
			Mana = ManaMax;
		}

		public void SetMana( int min, int max )
		{
			m_ManaMax = Utility.RandomMinMax( min, max );
			Mana = ManaMax;
		}

		public void SetStr( int val )
		{
			RawStr = val;
			Hits = HitsMax;
		}

		public void SetStr( int min, int max )
		{
			RawStr = Utility.RandomMinMax( min, max );
			Hits = HitsMax;
		}

		public void SetDex( int val )
		{
			RawDex = val;
			Stam = StamMax;
		}

		public void SetDex( int min, int max )
		{
			RawDex = Utility.RandomMinMax( min, max );
			Stam = StamMax;
		}

		public void SetInt( int val )
		{
			RawInt = val;
			Mana = ManaMax;
		}

		public void SetInt( int min, int max )
		{
			RawInt = Utility.RandomMinMax( min, max );
			Mana = ManaMax;
		}

		public void SetDamageType( ResistanceType type, int min, int max )
		{
			SetDamageType( type, Utility.RandomMinMax( min, max ) );
		}

		public void SetDamageType( ResistanceType type, int val )
		{
			switch ( type )
			{
				case ResistanceType.Physical:
					m_PhysicalDamage = val;
					break;
				case ResistanceType.Fire:
					m_FireDamage = val;
					break;
				case ResistanceType.Cold:
					m_ColdDamage = val;
					break;
				case ResistanceType.Poison:
					m_PoisonDamage = val;
					break;
				case ResistanceType.Energy:
					m_EnergyDamage = val;
					break;
			}
		}

		public void SetResistance( ResistanceType type, int min, int max )
		{
			SetResistance( type, Utility.RandomMinMax( min, max ) );
		}

		public void SetResistance( ResistanceType type, int val )
		{
			switch ( type )
			{
				case ResistanceType.Physical:
					m_PhysicalResistance = val;
					break;
				case ResistanceType.Fire:
					m_FireResistance = val;
					break;
				case ResistanceType.Cold:
					m_ColdResistance = val;
					break;
				case ResistanceType.Poison:
					m_PoisonResistance = val;
					break;
				case ResistanceType.Energy:
					m_EnergyResistance = val;
					break;
			}

			UpdateResistances();
		}

		public void SetSkill( SkillName name, double val )
		{
			Skills[name].BaseFixedPoint = (int) ( val * 10 );
		}

		public void SetSkill( SkillName name, double min, double max )
		{
			int minFixed = (int) ( min * 10 );
			int maxFixed = (int) ( max * 10 );

			Skills[name].BaseFixedPoint = Utility.RandomMinMax( minFixed, maxFixed );
		}

		public void SetFameLevel( int level )
		{
			switch ( level )
			{
				case 1:
					Fame = Utility.RandomMinMax( 0, 1249 );
					break;
				case 2:
					Fame = Utility.RandomMinMax( 1250, 2499 );
					break;
				case 3:
					Fame = Utility.RandomMinMax( 2500, 4999 );
					break;
				case 4:
					Fame = Utility.RandomMinMax( 5000, 9999 );
					break;
				case 5:
					Fame = Utility.RandomMinMax( 10000, 10000 );
					break;
			}
		}

		public void SetKarmaLevel( int level )
		{
			switch ( level )
			{
				case 0:
					Karma = -Utility.RandomMinMax( 0, 624 );
					break;
				case 1:
					Karma = -Utility.RandomMinMax( 625, 1249 );
					break;
				case 2:
					Karma = -Utility.RandomMinMax( 1250, 2499 );
					break;
				case 3:
					Karma = -Utility.RandomMinMax( 2500, 4999 );
					break;
				case 4:
					Karma = -Utility.RandomMinMax( 5000, 9999 );
					break;
				case 5:
					Karma = -Utility.RandomMinMax( 10000, 10000 );
					break;
			}
		}

		#endregion

		public static void Cap( ref int val, int min, int max )
		{
			if ( val < min )
				val = min;
			else if ( val > max )
				val = max;
		}

		#region Pack & Loot

		public void PackPotion()
		{
			PackItem( Loot.RandomPotion() );
		}

		public void PackNecroScroll( int index )
		{
			if ( 0.05 <= Utility.RandomDouble() )
				return;

			PackItem( Loot.Construct( Loot.NecromancyScrollTypes, index ) );
		}

		public void PackSpellweavingScroll()
		{
			if ( 0.6 <= Utility.RandomDouble() )
				return;

			PackItem( Loot.Construct( Loot.ArcanistScrollTypes, Utility.RandomMinMax( 0, 12 ) ) );
		}

		public void PackMysticScroll( int index )
		{
			if ( 0.5 <= Utility.RandomDouble() )
				return;

			PackItem( Loot.Construct( Loot.MysticScrollTypes, index ) );
		}

		public void PackScroll( int minCircle, int maxCircle )
		{
			PackScroll( Utility.RandomMinMax( minCircle, maxCircle ) );
		}

		public void PackScroll( int circle )
		{
			int min = ( circle - 1 ) * 8;

			PackItem( Loot.RandomScroll( min, min + 7, SpellbookType.Regular ) );
		}

		public void PackMagicItems( int minLevel, int maxLevel )
		{
			PackMagicItems( minLevel, maxLevel, 0.30, 0.15 );
		}

		public void PackMagicItems( int minLevel, int maxLevel, double armorChance, double weaponChance )
		{
			if ( !PackArmor( minLevel, maxLevel, armorChance ) )
				PackWeapon( minLevel, maxLevel, weaponChance );
		}

		protected bool m_Spawning;
		protected int m_KillersLuck;

		public virtual void GenerateLoot( bool spawning )
		{
			m_Spawning = spawning;

			if ( !spawning )
				m_KillersLuck = LootPack.GetLuckChanceForKiller( this );

			GenerateLoot();

			m_Spawning = false;
			m_KillersLuck = 0;
		}

		public virtual void GenerateLoot()
		{
		}

		public bool CanAdd = true;

		public LootPack IncreaseLoot( LootPack pack )
		{
			LootPack packs = pack;

			if ( pack == LootPack.Poor )
				packs = LootPack.Meager;

			if ( pack == LootPack.Meager )
				packs = LootPack.Average;

			if ( pack == LootPack.Average )
				packs = LootPack.Rich;

			if ( pack == LootPack.Rich )
				packs = LootPack.FilthyRich;

			if ( pack == LootPack.FilthyRich )
				packs = LootPack.UltraRich;

			if ( pack == LootPack.UltraRich )
				packs = LootPack.SuperBoss;

			return packs;
		}


		public virtual void AddLoot( LootPack pack, int amount )
		{
			CanAdd = false;

			for ( int i = 0; i < amount; ++i )
				AddLoot( pack );

			if ( IsParagon )
				AddLoot( IncreaseLoot( pack ) );
		}

		public virtual void AddLoot( LootPack pack )
		{
			if ( Summoned )
				return;

			Container backpack = Backpack;

			if ( backpack == null )
			{
				backpack = new Backpack();

				backpack.Movable = false;

				AddItem( backpack );
			}

			if ( IsParagon && CanAdd )
			{
				LootPack pack1 = IncreaseLoot( pack );

				pack1.Generate( this, backpack, m_Spawning, m_KillersLuck );
			}

			pack.Generate( this, backpack, m_Spawning, m_KillersLuck );
		}

		public bool PackArmor( int minLevel, int maxLevel )
		{
			return PackArmor( minLevel, maxLevel, 1.0 );
		}

		public bool PackArmor( int minLevel, int maxLevel, double chance )
		{
			if ( chance <= Utility.RandomDouble() )
				return false;

			Cap( ref minLevel, 0, 5 );
			Cap( ref maxLevel, 0, 5 );

			Item item = Loot.RandomArmorOrShieldOrJewelry();

			if ( item == null )
				return false;

			int attributeCount, min, max;
			GetRandomAOSStats( minLevel, maxLevel, out attributeCount, out min, out max );

			if ( item is BaseArmor )
				BaseRunicTool.ApplyAttributesTo( (BaseArmor) item, attributeCount, min, max );
			else if ( item is BaseJewel )
				BaseRunicTool.ApplyAttributesTo( (BaseJewel) item, attributeCount, min, max );

			PackItem( item );

			return true;
		}

		public static void GetRandomAOSStats( int minLevel, int maxLevel, out int attributeCount, out int min, out int max )
		{
			int v = RandomMinMaxScaled( minLevel, maxLevel );

			if ( v >= 5 )
			{
				attributeCount = Utility.RandomMinMax( 2, 6 );
				min = 20;
				max = 70;
			}
			else if ( v == 4 )
			{
				attributeCount = Utility.RandomMinMax( 2, 4 );
				min = 20;
				max = 50;
			}
			else if ( v == 3 )
			{
				attributeCount = Utility.RandomMinMax( 2, 3 );
				min = 20;
				max = 40;
			}
			else if ( v == 2 )
			{
				attributeCount = Utility.RandomMinMax( 1, 2 );
				min = 10;
				max = 30;
			}
			else
			{
				attributeCount = 1;
				min = 10;
				max = 20;
			}
		}

		public static int RandomMinMaxScaled( int min, int max )
		{
			if ( min == max )
				return min;

			if ( min > max )
			{
				int hold = min;
				min = max;
				max = hold;
			}

			/* Example:
			 *    min: 1
			 *    max: 5
			 *  count: 5
			 * 
			 * total = (5*5) + (4*4) + (3*3) + (2*2) + (1*1) = 25 + 16 + 9 + 4 + 1 = 55
			 * 
			 * chance for min+0 : 25/55 : 45.45%
			 * chance for min+1 : 16/55 : 29.09%
			 * chance for min+2 :  9/55 : 16.36%
			 * chance for min+3 :  4/55 :  7.27%
			 * chance for min+4 :  1/55 :  1.81%
			 */

			int count = max - min + 1;
			int total = 0, toAdd = count;

			for ( int i = 0; i < count; ++i, --toAdd )
				total += toAdd * toAdd;

			int rand = Utility.Random( total );
			toAdd = count;

			int val = min;

			for ( int i = 0; i < count; ++i, --toAdd, ++val )
			{
				rand -= toAdd * toAdd;

				if ( rand < 0 )
					break;
			}

			return val;
		}

		public bool PackSlayer()
		{
			return PackSlayer( 0.05 );
		}

		public bool PackSlayer( double chance )
		{
			if ( chance <= Utility.RandomDouble() )
				return false;

			if ( Utility.RandomBool() )
			{
				BaseInstrument instrument = Loot.RandomInstrument();

				if ( instrument != null )
				{
					instrument.Slayer = SlayerGroup.GetLootSlayerType( GetType() );
					PackItem( instrument );
				}
			}

			return true;
		}

		public bool PackWeapon( int minLevel, int maxLevel )
		{
			return PackWeapon( minLevel, maxLevel, 1.0 );
		}

		public bool PackWeapon( int minLevel, int maxLevel, double chance )
		{
			if ( chance <= Utility.RandomDouble() )
				return false;

			Cap( ref minLevel, 0, 5 );
			Cap( ref maxLevel, 0, 5 );

			Item item = Loot.RandomWeaponOrJewelry();

			if ( item == null )
				return false;

			int attributeCount, min, max;
			GetRandomAOSStats( minLevel, maxLevel, out attributeCount, out min, out max );

			if ( item is BaseWeapon )
				BaseRunicTool.ApplyAttributesTo( (BaseWeapon) item, attributeCount, min, max );
			else if ( item is BaseJewel )
				BaseRunicTool.ApplyAttributesTo( (BaseJewel) item, attributeCount, min, max );

			PackItem( item );

			return true;
		}

		public void PackGold( int amount )
		{
			if ( amount > 0 )
				PackItem( new Gold( amount ) );
		}

		public void PackGold( int min, int max )
		{
			PackGold( Utility.RandomMinMax( min, max ) );
		}

		public void PackStatue( int min, int max )
		{
			PackStatue( Utility.RandomMinMax( min, max ) );
		}

		public void PackStatue( int amount )
		{
			for ( int i = 0; i < amount; ++i )
				PackStatue();
		}

		public void PackStatue()
		{
			PackItem( Loot.RandomStatue() );
		}

		public void PackGem()
		{
			PackGem( 1 );
		}

		public void PackGem( int min, int max )
		{
			PackGem( Utility.RandomMinMax( min, max ) );
		}

		public void PackGem( int amount )
		{
			if ( amount <= 0 )
				return;

			Item gem = Loot.RandomGem();

			gem.Amount = amount;

			PackItem( gem );
		}

		public void PackMysticReg( int min, int max )
		{
			PackMysticReg( Utility.RandomMinMax( min, max ) );
		}

		public void PackMysticReg( int amount )
		{
			for ( int i = 0; i < amount; ++i )
				PackMysticReg();
		}

		public void PackMysticReg()
		{
			PackItem( Loot.RandomMysticismReagent() );
		}

		public void PackNecroReg( int min, int max )
		{
			PackNecroReg( Utility.RandomMinMax( min, max ) );
		}

		public void PackNecroReg( int amount )
		{
			for ( int i = 0; i < amount; ++i )
				PackNecroReg();
		}

		public void PackNecroReg()
		{
			PackItem( Loot.RandomNecromancyReagent() );
		}

		public void PackReg( int min, int max )
		{
			PackReg( Utility.RandomMinMax( min, max ) );
		}

		public void PackReg( int amount )
		{
			if ( amount <= 0 )
				return;

			Item reg = Loot.RandomReagent();

			reg.Amount = amount;

			PackItem( reg );
		}

		public void PackItem( Item item )
		{
			if ( Summoned || item == null )
			{
				if ( item != null )
					item.Delete();

				return;
			}

			Container pack = Backpack;

			if ( pack == null )
			{
				pack = new Backpack();

				pack.Movable = false;

				AddItem( pack );
			}

			if ( !item.Stackable || !pack.TryDropItem( this, item, false ) ) // try stack
				pack.DropItem( item ); // failed, drop it anyway

		}

		#endregion

		public override void OnDoubleClick( Mobile from )
		{
			if ( from.AccessLevel >= AccessLevel.GameMaster && !Body.IsHuman )
			{
				Container pack = this.Backpack;

				if ( pack != null )
					pack.DisplayTo( from );
			}

			if ( this.DeathAdderCharmable && from.CanBeHarmful( this, false ) )
			{
				DeathAdder da = Spells.Necromancy.SummonFamiliarSpell.Table[from] as DeathAdder;

				if ( da != null && !da.Deleted )
				{
					from.SendAsciiMessage( "You charm the snake.  Select a target to attack." );
					from.Target = new DeathAdderCharmTarget( this );
				}
			}

			base.OnDoubleClick( from );
		}

		private class DeathAdderCharmTarget : Target
		{
			private BaseCreature m_Charmed;

			public DeathAdderCharmTarget( BaseCreature charmed )
				: base( -1, false, TargetFlags.Harmful )
			{
				m_Charmed = charmed;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				if ( !m_Charmed.DeathAdderCharmable || m_Charmed.Combatant != null || !from.CanBeHarmful( m_Charmed, false ) )
					return;

				DeathAdder da = Spells.Necromancy.SummonFamiliarSpell.Table[from] as DeathAdder;
				if ( da == null || da.Deleted )
					return;

				Mobile targ = targeted as Mobile;
				if ( targ == null || !from.CanBeHarmful( targ, false ) )
					return;

				from.RevealingAction();
				from.DoHarmful( targ, true );

				m_Charmed.Combatant = targ;

				if ( m_Charmed.AIObject != null )
					m_Charmed.AIObject.Action = ActionType.Combat;
			}
		}

		public override void AddNameProperties( ObjectPropertyList list )
		{
			base.AddNameProperties( list );

			if ( Controlled && Commandable )
			{
				if ( ControlOrder == OrderType.Guard )
					list.Add( 1080078 ); // guarding

				if ( IsGolem )
					list.Add( 1113697 ); // (Golem)

				if ( Summoned )
					list.Add( 1049646 ); // (summoned)
				else if ( IsBonded ) // Intentional difference (showing ONLY bonded when bonded instead of bonded & tame)
					list.Add( 1049608 ); // (bonded)
				else
					list.Add( 502006 ); // (tame)
			}
		}

		public override bool Move( Direction d )
		{
			if ( base.Move( d ) )
			{
				Server.Spells.Ninjitsu.DeathStrike.AddStep( this );

				return true;
			}

			return false;
		}

		public virtual double TreasureMapChance { get { return TreasureMap.LootChance; } }

		public virtual int TreasureMapLevel { get { return -1; } }

		public bool CheckItem( Item item, Type[] types )
		{
			Type t = item.GetType();

			for ( int i = 0; i < types.Length; i++ )
			{
				Type type = types[i] as Type;

				if ( type == t )
				{
					return false;
				}
			}

			return true;
		}

		public override bool DoEffectTimerOnDeath { get { return m_IsBonded; } }

		protected override bool OnBeforeDeath()
		{
			if ( GiftOfLifeSpell.UnderEffect( this ) )
			{
				Timer.DelayCall( TimeSpan.FromSeconds( 1.0 ), new TimerCallback(
					delegate
					{
						this.ResurrectPet();

						GiftOfLifeSpell.RemoveEffect( this );
					} ) );
			}

			// Recursos para la rama chocolatera de cooking 

			if ( m_Paragon && Paragon.SackOfSugarChance > Utility.RandomDouble() )
				PackItem( new SackOfSugar() );

			if ( m_Paragon && Paragon.VanillaChance > Utility.RandomDouble() )
				PackItem( new Vanilla() );

			if ( !Summoned && !NoKillAwards && !IsBonded && TreasureMapLevel >= 1 )
			{
				if ( m_Paragon && Paragon.ChestChance > Utility.RandomDouble() )
					PackItem( new ParagonChest( this.Name, TreasureMapLevel ) );
				else if ( TreasureMap.LootChance >= Utility.RandomDouble() )
					PackItem( new TreasureMap( TreasureMapLevel, Map ) );
			}

			if ( !Summoned && !m_HasGeneratedLoot && Region.IsPartOf( "Underworld" ) && 0.01 > Utility.RandomDouble() )
				PackItem( new LuckyCoin() );

			if ( !Summoned && !m_HasGeneratedLoot && Map == Map.TerMur )
			{
				if ( Fame >= 2000 && 0.1 > Utility.RandomDouble() )
					PackItem( new AncientPotteryFragments() );

				if ( Fame >= 10000 && 0.2 > Utility.RandomDouble() )
					PackItem( new TatteredAncientScroll() );

				if ( Fame >= 25000 && 0.3 > Utility.RandomDouble() )
					PackItem( new UntranslatedAncientTome() );
			}

			if ( !Summoned && !m_HasGeneratedLoot && m_IsMinichampMonster && this.Fame > Utility.Random( 100000 ) )
			{
				Type essenceType = MiniChampInfo.GetInfo( m_MinichampType ).EssenceType;

				Item essence = null;

				try { essence = (Item) Activator.CreateInstance( essenceType ); }
				catch { }

				if ( essence != null )
					PackItem( essence );
			}

			this.InvokeLootGenerated( new LootGeneratedEventArgs() );

			if ( !Summoned && !m_HasGeneratedLoot && Region.Name == "Doom" && !( this is DarkGuardian ) )
			{
				int bones = Engines.Quests.Doom.TheSummoningQuest.GetDaemonBonesFor( this );

				if ( bones > 0 )
					PackItem( new DaemonBone( bones ) );
			}

			if ( !Summoned && !NoKillAwards && !m_HasGeneratedLoot )
			{
				m_HasGeneratedLoot = true;
				GenerateLoot( false );
			}

			if ( IsAnimatedDead )
				Effects.SendLocationEffect( Location, Map, 0x3728, 13, 1, 0x461, 4 );

			InhumanSpeech speechType = this.SpeechType;

			if ( speechType != null )
				speechType.OnDeath( this );

			if ( m_ReceivedHonorContext != null )
				m_ReceivedHonorContext.OnTargetKilled();

			return base.OnBeforeDeath();
		}

		#region Events

		public static event LootGeneratedEventHandler LootGenerated;

		protected void InvokeLootGenerated( LootGeneratedEventArgs args )
		{
			if ( LootGenerated != null )
				LootGenerated( this, args );
		}

		#endregion

		private bool m_NoKillAwards;

		public bool NoKillAwards
		{
			get { return m_NoKillAwards; }
			set { m_NoKillAwards = value; }
		}

		public int ComputeBonusDamage( ArrayList list, Mobile m )
		{
			int bonus = 0;

			for ( int i = list.Count - 1; i >= 0; --i )
			{
				DamageEntry de = (DamageEntry) list[i];

				if ( de.Damager == m || !( de.Damager is BaseCreature ) )
					continue;

				BaseCreature bc = (BaseCreature) de.Damager;
				Mobile master = null;

				master = bc.GetMaster();

				if ( master == m )
					bonus += de.DamageGiven;
			}

			return bonus;
		}

		public Mobile GetMaster()
		{
			if ( Controlled && ControlMaster != null )
				return ControlMaster;
			else if ( Summoned && SummonMaster != null )
				return SummonMaster;

			return null;
		}

		public static List<DamageStore> GetLootingRights( List<DamageEntry> damageEntries, int hitsMax )
		{
			List<DamageStore> rights = new List<DamageStore>();

			for ( int i = damageEntries.Count - 1; i >= 0; --i )
			{
				if ( i >= damageEntries.Count )
					continue;

				DamageEntry de = damageEntries[i];

				if ( de.HasExpired )
				{
					damageEntries.RemoveAt( i );
					continue;
				}

				int damage = de.DamageGiven;

				List<DamageEntry> respList = de.Responsible;

				if ( respList != null )
				{
					for ( int j = 0; j < respList.Count; ++j )
					{
						DamageEntry subEntry = respList[j];
						Mobile master = subEntry.Damager;

						if ( master == null || master.Deleted || !master.IsPlayer )
							continue;

						bool needNewSubEntry = true;

						for ( int k = 0; needNewSubEntry && k < rights.Count; ++k )
						{
							DamageStore ds = rights[k];

							if ( ds.Mobile == master )
							{
								ds.Damage += subEntry.DamageGiven;
								needNewSubEntry = false;
							}
						}

						if ( needNewSubEntry )
							rights.Add( new DamageStore( master, subEntry.DamageGiven ) );

						damage -= subEntry.DamageGiven;
					}
				}

				Mobile m = de.Damager;

				if ( m == null || m.Deleted || !m.IsPlayer )
					continue;

				if ( damage <= 0 )
					continue;

				bool needNewEntry = true;

				for ( int j = 0; needNewEntry && j < rights.Count; ++j )
				{
					DamageStore ds = rights[j];

					if ( ds.Mobile == m )
					{
						ds.Damage += damage;
						needNewEntry = false;
					}
				}

				if ( needNewEntry )
					rights.Add( new DamageStore( m, damage ) );
			}

			if ( rights.Count > 0 )
			{
				if ( rights.Count > 1 )
					rights.Sort();

				int topDamage = rights[0].Damage;
				int minDamage;

				if ( hitsMax >= 3000 )
					minDamage = topDamage / 16;
				else if ( hitsMax >= 1000 )
					minDamage = topDamage / 8;
				else if ( hitsMax >= 200 )
					minDamage = topDamage / 4;
				else
					minDamage = topDamage / 2;

				int totalDamage = 0;

				for ( int i = 0; i < rights.Count; ++i )
				{
					DamageStore ds = rights[i];

					totalDamage += ds.Damage;
				}

				for ( int i = 0; i < rights.Count; ++i )
				{
					DamageStore ds = rights[i];

					ds.HasRight = ( ds.Damage >= minDamage );

					if ( totalDamage != 0 )
						ds.DamagePercent = (double) ( (double) ds.Damage / (double) totalDamage );
				}
			}

			return rights;
		}

		public delegate void KilledByHandler( BaseCreature bc, Mobile killer );

		public static event KilledByHandler KilledBy;

		private void InvokeKilledBy( Mobile mob )
		{
			if ( KilledBy != null )
				KilledBy( this, mob );
		}

		public virtual void OnKilledBy( Mobile killer )
		{
			InvokeKilledBy( killer );
		}

		public virtual void OnTamed( Mobile master )
		{
		}

		public void CalculateTitlesScore( PlayerMobile pm, int m_SpawnLevel, int type )
		{
			double value = pm.ChampionTiers[type];

			if ( ( m_SpawnLevel >= 1 && m_SpawnLevel <= 2 && value >= 0 && value + m_SpawnLevel < 10 ) || ( m_SpawnLevel >= 3 && m_SpawnLevel <= 4 && value >= 10 && value + m_SpawnLevel < 20 ) )
			{
				// TODO: verify
				pm.ChampionTiers[type] += ( (double) m_SpawnLevel / 500 );
			}
		}

		protected override void OnAfterDeath( Container c )
		{
			MeerMage.StopEffect( this, false );

			ArcaneEmpowermentSpell.StopBuffing( this, false );

			if ( IsBonded )
			{
				int sound = this.GetDeathSound();

				if ( sound >= 0 )
					Effects.PlaySound( this, this.Map, sound );

				Warmode = false;

				Poison = null;
				Combatant = null;

				BleedAttack.EndBleed( this, false );
				Spells.Necromancy.StrangleSpell.RemoveCurse( this );

				Hits = 0;
				Stam = 0;
				Mana = 0;

				IsDeadPet = true;

				ProcessDeltaQueue();
				this.SendIncomingPacket();
				this.SendIncomingPacket();

				List<AggressorInfo> aggressors = this.Aggressors;

				for ( int i = 0; i < aggressors.Count; ++i )
				{
					AggressorInfo info = aggressors[i];

					if ( info.Attacker.Combatant == this )
						info.Attacker.Combatant = null;
				}

				List<AggressorInfo> aggressed = this.Aggressed;

				for ( int i = 0; i < aggressed.Count; ++i )
				{
					AggressorInfo info = aggressed[i];

					if ( info.Defender.Combatant == this )
						info.Defender.Combatant = null;
				}

				Mobile owner = this.ControlMaster;

				if ( owner == null || owner.Deleted || owner.Map != this.Map || !owner.InRange( this, 12 ) || !this.CanSee( owner ) || !this.InLOS( owner ) )
				{
					if ( this.OwnerAbandonTime == DateTime.MinValue )
						this.OwnerAbandonTime = DateTime.Now;
				}
				else
				{
					this.OwnerAbandonTime = DateTime.MinValue;
				}

				CheckStatTimers();
			}
			else
			{
				if ( !Summoned && !m_NoKillAwards )
				{
					int totalFame = Fame / 100;
					int totalKarma = -Karma / 100;

					for ( int i = 0; i < this.DamageEntries.Count; i++ )
					{
						DamageEntry entry = this.DamageEntries[i];

						if ( !entry.HasExpired && entry.DamageGiven > ( this.HitsMax / 10 ) )
						{
							if ( entry.Damager is VoidCreature )
								( (VoidCreature) entry.Damager ).OnKill( this );
						}
					}

					List<DamageStore> list = GetLootingRights( this.DamageEntries, this.HitsMax );

					bool givenQuestKill = false;
					bool givenFactionKill = false;

					bool givesLoyaltyAward = ( LoyaltyPointsAward > 0 );
					LoyaltyGroupInfo loyaltyGroup = LoyaltyGroupInfo.GetInfo( LoyaltyGroupEnemy );

					for ( int i = 0; i < list.Count; ++i )
					{
						DamageStore ds = list[i];

						if ( !ds.HasRight )
							continue;

						Titles.AwardFame( ds.Mobile, totalFame, true );
						Titles.AwardKarma( ds.Mobile, totalKarma, true );

						OnKilledBy( ds.Mobile );

						if ( !givenFactionKill )
						{
							givenFactionKill = true;
							Faction.HandleDeath( this, ds.Mobile );
						}

						if ( givenQuestKill )
							continue;

						PlayerMobile pm = ds.Mobile as PlayerMobile;

						if ( pm != null )
						{
							QuestSystem qs = pm.Quest;

							if ( qs != null )
							{
								qs.OnKill( this, c );
								givenQuestKill = true;
							}

							QuestHelper.CheckCreature( pm, this );

							if ( this is WeakSkeleton )
								pm.CheckKRStartingQuestStep( 19 );

							if ( givesLoyaltyAward && ( loyaltyGroup.Checker == null || loyaltyGroup.Checker( pm, this ) ) )
							{
								// TODO: guessing here, check the real formula @ OSI.
								int loyaltyPoints = LoyaltyPointsAward / ( i + 1 );

								if ( loyaltyPoints > 0 )
								{
									pm.LoyaltyInfo.Award( LoyaltyGroupEnemy, loyaltyPoints );

									// Your loyalty to ~1_GROUP~ has increased by ~2_AMOUNT~
									pm.SendLocalizedMessage( 1115920, String.Format( "#{0}\t{1}", loyaltyGroup.GroupName.ToString(), loyaltyPoints.ToString() ), 0x21 );
								}
							}
						}
					}

					Renowned.CheckDropReward( this, c );

					( (Corpse) c ).AssignInstancedLoot();
				}

				base.OnAfterDeath( c );

				if ( DeleteCorpseOnDeath )
					c.Delete();
			}
		}

		/* 
		 * To save on cpu usage, X-RunUO creatures only reacquire creatures under the following circumstances:
		 *  - 10 seconds have elapsed since the last time it tried
		 *  - The creature was attacked
		 *  - Some creatures, like dragons, will reacquire when they see someone move
		 * 
		 * This functionality appears to be implemented on OSI as well
		 */

		private DateTime m_NextReacquireTime;

		public DateTime NextReacquireTime { get { return m_NextReacquireTime; } set { m_NextReacquireTime = value; } }

		public virtual TimeSpan ReacquireDelay { get { return TimeSpan.FromSeconds( 10.0 * m_dPassiveSpeed ); } }
		public virtual bool ReacquireOnMovement { get { return false; } }

		public void ForceReacquire()
		{
			m_NextReacquireTime = DateTime.MinValue;
		}

		public override void OnDelete()
		{
			Mobile m = m_ControlMaster;

			SetControlMaster( null );
			SummonMaster = null;

			if ( m_ReceivedHonorContext != null )
				m_ReceivedHonorContext.Cancel();

			base.OnDelete();

			if ( m != null )
				m.InvalidateProperties();
		}

		public override bool CanBeHarmful( Mobile target, bool message, bool ignoreOurBlessedness )
		{
			if ( target is BaseFactionGuard )
				return false;

			if ( ( target is BaseVendor && ( (BaseVendor) target ).IsInvulnerable ) || target is PlayerVendor || target is TownCrier )
			{
				if ( message )
				{
					if ( target.Title == null )
						SendAsciiMessage( "{0} the vendor cannot be harmed.", target.Name );
					else
						SendAsciiMessage( "{0} {1} cannot be harmed.", target.Name, target.Title );
				}

				return false;
			}

			return base.CanBeHarmful( target, message, ignoreOurBlessedness );
		}

		public override bool CanBeRenamedBy( Mobile from )
		{
			bool ret = base.CanBeRenamedBy( from );

			if ( Controlled && from == ControlMaster )
				ret = true;

			return ret;
		}

		public bool SetControlMaster( Mobile m )
		{
			if ( m == null )
			{
				ControlMaster = null;
				Controlled = false;
				ControlTarget = null;
				ControlOrder = OrderType.None;
				Guild = null;

				Delta( MobileDelta.Noto );
			}
			else
			{
				if ( Spawner != null && Spawner.UnlinkOnTaming )
				{
					Spawner.Remove( this );
					Spawner = null;
				}

				if ( m.Followers + ControlSlots > m.FollowersMax )
				{
					m.SendLocalizedMessage( 1049607 ); // You have too many followers to control that creature.
					return false;
				}

				CurrentWayPoint = null; // so tamed animals don't try to go back

				ControlMaster = m;
				Controlled = true;
				ControlTarget = null;
				ControlOrder = OrderType.Come;
				Guild = null;

				Delta( MobileDelta.Noto );
			}

			return true;
		}

		private static bool m_Summoning;

		public static bool Summoning
		{
			get { return m_Summoning; }
			set { m_Summoning = value; }
		}

		public static bool Summon( BaseCreature creature, Mobile caster, Point3D p, int sound, TimeSpan duration )
		{
			return Summon( creature, true, caster, p, sound, duration );
		}

		public static bool Summon( BaseCreature creature, bool controlled, Mobile caster, Point3D p, int sound, TimeSpan duration )
		{
			if ( caster.Followers + creature.ControlSlots > caster.FollowersMax )
			{
				caster.SendLocalizedMessage( 1049645 ); // You have too many followers to summon that creature.
				creature.Delete();
				return false;
			}

			m_Summoning = true;

			creature.RangeHome = 10;
			creature.Summoned = true;

			if ( controlled )
				creature.SetControlMaster( caster );

			creature.SummonMaster = caster;

			Container pack = creature.Backpack;

			if ( pack != null )
			{
				for ( int i = pack.Items.Count - 1; i >= 0; --i )
				{
					if ( i >= pack.Items.Count )
						continue;

					( (Item) pack.Items[i] ).Delete();
				}
			}

			double hitsScalar = 1.0 + ( ArcaneEmpowermentSpell.GetSummonHitsBonus( caster ) / 100 );

			if ( hitsScalar != 1.0 )
				creature.SetHits( (int) ( creature.HitsMax * hitsScalar ) );

			new UnsummonTimer( caster, creature, duration ).Start();
			creature.m_SummonEnd = DateTime.Now + duration;

			creature.MoveToWorld( p, caster.Map );

			Effects.PlaySound( p, creature.Map, sound );

			if ( creature is EnergyVortex || creature is BladeSpirits )
				SpellHelper.CheckSummonLimits( creature );

			m_Summoning = false;

			return true;
		}

		private static bool EnableRummaging = true;

		private const double ChanceToRummage = 0.5; // 50%

		private const double MinutesToNextRummageMin = 1.0;
		private const double MinutesToNextRummageMax = 4.0;

		private const double MinutesToNextChanceMin = 0.25;
		private const double MinutesToNextChanceMax = 0.75;

		private DateTime m_NextRummageTime;

		public virtual bool IsDispellable { get { return Summoned && !IsAnimatedDead; } }

		#region True Fear
		public virtual bool CausesTrueFear { get { return false; } }

		private static List<Mobile> m_TrueFearCooldown = new List<Mobile>();

		private const int TrueFearRange = 8;

		public virtual void CauseTrueFear( Mobile m, Point3D oldLocation )
		{
			base.OnMovement( m, oldLocation );

			if ( m.Alive && m.IsPlayer && this.InRange( m.Location, TrueFearRange ) && !this.InRange( oldLocation, TrueFearRange ) )
			{
				if ( !m_TrueFearCooldown.Contains( m ) )
				{
					int seconds = (int) ( 13.0 - ( m.Skills[SkillName.MagicResist].Value / 10.0 ) );

					if ( seconds < 1 )
						seconds = 1;

					int number;

					if ( seconds <= 2 )
						number = 1080339; // A sense of discomfort passes through you, but it fades quickly
					else if ( seconds <= 4 )
						number = 1080340; // An unfamiliar fear washes over you, and for a moment you're unable to move
					else if ( seconds <= 7 )
						number = 1080341; // Panic grips you! You're unable to move, to think, to feel anything but fear!
					else if ( seconds <= 10 )
						number = 1080342; // Terror slices into your very being, destroying any chance of resisting ~1_name~ you might have had
					else
						number = 1080343; // Everything around you dissolves into darkness as ~1_name~'s burning eyes fill your vision

					m.SendLocalizedMessage( number, this.Name, 0x21 );

					m_TrueFearCooldown.Add( m );

					m.Frozen = true;

					Timer.DelayCall( TimeSpan.FromSeconds( seconds ), new TimerCallback(
						delegate
						{
							m.Frozen = false;
							m.SendLocalizedMessage( 1005603 ); // You can move again!
						} ) );

					Timer.DelayCall( TimeSpan.FromMinutes( 5.0 ), new TimerCallback(
						delegate { m_TrueFearCooldown.Remove( m ); } ) );
				}
			}
		}
		#endregion

		#region Damaging Aura
		private DateTime m_NextAura;

		public virtual bool HasAura { get { return false; } }
		public virtual TimeSpan AuraInterval { get { return TimeSpan.FromSeconds( 5 ); } }
		public virtual int AuraRange { get { return 4; } }

		public virtual int AuraBaseDamage { get { return 5; } }
		public virtual int AuraPhysicalDamage { get { return 0; } }
		public virtual int AuraFireDamage { get { return 100; } }
		public virtual int AuraColdDamage { get { return 0; } }
		public virtual int AuraPoisonDamage { get { return 0; } }
		public virtual int AuraEnergyDamage { get { return 0; } }
		public virtual int AuraChaosDamage { get { return 0; } }

		public virtual void AuraDamage()
		{
			if ( !Alive || IsDeadBondedPet )
				return;

			List<Mobile> list = new List<Mobile>();

			foreach ( Mobile m in this.GetMobilesInRange( AuraRange ) )
			{
				if ( m == this || !CanBeHarmful( m, false ) || !InLOS( m ) )
					continue;

				if ( m is BaseCreature )
				{
					BaseCreature bc = (BaseCreature) m;

					if ( bc.Controlled || bc.Summoned || bc.Team != Team )
						list.Add( m );
				}
				else if ( m.IsPlayer )
				{
					list.Add( m );
				}
			}

			foreach ( Mobile m in list )
			{
				AOS.Damage( m, this, AuraBaseDamage, AuraPhysicalDamage, AuraFireDamage, AuraColdDamage, AuraPoisonDamage, AuraEnergyDamage, AuraChaosDamage );
				AuraEffect( m );
			}
		}

		public virtual void AuraEffect( Mobile m )
		{
		}
		#endregion

		public virtual void OnThink()
		{
			if ( EnableRummaging && CanRummageCorpses && !Summoned && !Controlled && DateTime.Now >= m_NextRummageTime )
			{
				double min, max;

				if ( ChanceToRummage > Utility.RandomDouble() && Rummage() )
				{
					min = MinutesToNextRummageMin;
					max = MinutesToNextRummageMax;
				}
				else
				{
					min = MinutesToNextChanceMin;
					max = MinutesToNextChanceMax;
				}

				double delay = min + ( Utility.RandomDouble() * ( max - min ) );
				m_NextRummageTime = DateTime.Now + TimeSpan.FromMinutes( delay );
			}

			if ( HasBreath && !Summoned && DateTime.Now >= m_NextBreathTime ) // tested: controled dragons do breath fire, what about summoned skeletal dragons?
			{
				Mobile target = this.Combatant;

				if ( target != null && target.Alive && !target.IsDeadBondedPet && CanBeHarmful( target ) && target.Map == this.Map && !IsDeadBondedPet && target.InRange( this, BreathRange ) && this.InLOS( target ) && !BardPacified )
					BreathStart( target );

				m_NextBreathTime = DateTime.Now + TimeSpan.FromSeconds( BreathMinDelay + ( Utility.RandomDouble() * BreathMaxDelay ) );
			}

			if ( HasAura && DateTime.Now >= m_NextAura )
			{
				AuraDamage();
				m_NextAura = DateTime.Now + AuraInterval;
			}
		}

		public virtual bool Rummage()
		{
			Corpse toRummage = null;

			foreach ( Item item in this.GetItemsInRange( 2 ) )
			{
				if ( item is Corpse && item.Items.Count > 0 )
				{
					toRummage = (Corpse) item;
					break;
				}
			}

			if ( toRummage == null )
				return false;

			Container pack = this.Backpack;

			if ( pack == null )
				return false;

			List<Item> items = toRummage.Items;

			bool rejected;
			LRReason reason;

			for ( int i = 0; i < items.Count; ++i )
			{
				Item item = items[Utility.Random( items.Count )];

				Lift( item, item.Amount, out rejected, out reason );

				if ( !rejected && Drop( this, new Point3D( -1, -1, 0 ) ) )
				{
					// *rummages through a corpse and takes an item*
					PublicOverheadMessage( MessageType.Emote, 0x3B2, 1008086 );

					Mobile owner = toRummage.Owner as Mobile;

					if ( owner != null && owner.IsPlayer )
						m_RummagedItems.Add( item, owner );

					return true;
				}
			}

			return false;
		}

		private Dictionary<Item, Mobile> m_RummagedItems = new Dictionary<Item, Mobile>();

		public Dictionary<Item, Mobile> RummagedItems { get { return m_RummagedItems; } }

		public void Pacify( Mobile master, DateTime endtime )
		{
			BardPacified = true;
			BardEndTime = endtime;
		}

		public override Mobile GetDamageMaster( Mobile damagee )
		{
			if ( m_bBardProvoked && damagee == m_bBardTarget )
				return m_bBardMaster;
			else if ( m_bControlled && m_ControlMaster != null )
				return m_ControlMaster;
			else if ( m_bSummoned && m_SummonMaster != null )
				return m_SummonMaster;

			return base.GetDamageMaster( damagee );
		}

		public void Provoke( Mobile master, Mobile target, bool bSuccess )
		{
			BardProvoked = true;

			//this.PublicOverheadMessage( MessageType.Emote, EmoteHue, false, "*looks furious*" );

			string format = String.Format( "{0}\t{1}\t{2}", master.Name, this.Name, target.Name );

			foreach ( NetState ns in master.GetClientsInRange( 8 ) )
			{
				if ( ns.Mobile != master )
				{
					// You notice ~1_NAME~ provoking ~2_VAL~ to attack ~3_VAL~.
					ns.Mobile.SendLocalizedMessage( 1080028, format );
				}
			}

			if ( bSuccess )
			{
				PlaySound( GetIdleSound() );

				BardMaster = master;
				BardTarget = target;
				Combatant = target;
				BardEndTime = DateTime.Now + TimeSpan.FromSeconds( 30.0 );

				if ( target is BaseCreature )
				{
					BaseCreature t = (BaseCreature) target;

					t.BardProvoked = true;

					t.BardMaster = master;
					t.BardTarget = this;
					t.Combatant = this;
					t.BardEndTime = DateTime.Now + TimeSpan.FromSeconds( 30.0 );
				}
			}
			else
			{
				PlaySound( GetAngerSound() );

				BardMaster = master;
				BardTarget = target;
			}
		}

		public bool FindMyName( string str, bool bWithAll )
		{
			int i, j;

			string name = this.Name;

			if ( name == null || str.Length < name.Length )
				return false;

			string[] wordsString = str.Split( ' ' );
			string[] wordsName = name.Split( ' ' );

			for ( j = 0; j < wordsName.Length; j++ )
			{
				string wordName = wordsName[j];

				bool bFound = false;
				for ( i = 0; i < wordsString.Length; i++ )
				{
					string word = wordsString[i];

					if ( Insensitive.Equals( word, wordName ) )
						bFound = true;

					if ( bWithAll && Insensitive.Equals( word, "all" ) )
						return true;
				}

				if ( !bFound )
					return false;
			}

			return true;
		}

		public static void TeleportPets( Mobile master, Point3D loc, Map map )
		{
			TeleportPets( master, loc, map, false );
		}

		public static void TeleportPets( Mobile master, Point3D loc, Map map, bool onlyBonded )
		{
			ArrayList move = new ArrayList();

			foreach ( Mobile m in master.GetMobilesInRange( 3 ) )
			{
				if ( m is BaseCreature )
				{
					BaseCreature pet = (BaseCreature) m;

					if ( pet.Controlled && pet.ControlMaster == master )
					{
						if ( !onlyBonded || pet.IsBonded )
						{
							if ( pet.ControlOrder == OrderType.Guard || pet.ControlOrder == OrderType.Follow || pet.ControlOrder == OrderType.Come )
								move.Add( pet );
						}
					}
				}
			}

			foreach ( Mobile m in move )
				m.MoveToWorld( loc, map );
		}

		public virtual void ResurrectPet()
		{
			if ( !IsDeadPet )
				return;

			OnBeforeResurrect();

			Poison = null;

			Warmode = false;

			Hits = 10;
			Stam = StamMax;
			Mana = 0;

			ProcessDeltaQueue();

			IsDeadPet = false;

			Effects.SendPacket( Location, Map, new BondedStatus( 0, this.Serial, 0 ) );

			this.SendIncomingPacket();
			this.SendIncomingPacket();

			OnAfterResurrect();

			Mobile owner = this.ControlMaster;

			if ( owner == null || owner.Deleted || owner.Map != this.Map || !owner.InRange( this, 12 ) || !this.CanSee( owner ) || !this.InLOS( owner ) )
			{
				if ( this.OwnerAbandonTime == DateTime.MinValue )
					this.OwnerAbandonTime = DateTime.Now;
			}
			else
			{
				this.OwnerAbandonTime = DateTime.MinValue;
			}

			CheckStatTimers();
		}

		public override bool CanBeDamaged()
		{
			if ( IsDeadPet )
				return false;

			return base.CanBeDamaged();
		}

		public virtual bool PlayerRangeSensitive { get { return true; } }

		public override void OnSectorDeactivate()
		{
			if ( PlayerRangeSensitive && m_AI != null && !Controlled && m_CurrentWayPoint == null )
				m_AI.Deactivate();

			base.OnSectorDeactivate();
		}

		public override void OnSectorActivate()
		{
			if ( PlayerRangeSensitive && m_AI != null && !Controlled )
				m_AI.Activate();

			base.OnSectorActivate();
		}

		// used for deleting untamed creatures [in houses]
		private int m_RemoveStep;

		[CommandProperty( AccessLevel.GameMaster )]
		public int RemoveStep { get { return m_RemoveStep; } set { m_RemoveStep = value; } }

		// used for deleting untamed creatures [on save]
		private bool m_RemoveOnSave;

		[CommandProperty( AccessLevel.GameMaster )]
		public bool RemoveOnSave { get { return m_RemoveOnSave; } set { m_RemoveOnSave = value; } }

		#region Medusa petrification

		private bool m_Petrified;
		private int m_StatueAnimation;
		private int m_StatueFrames;

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Petrified { get { return m_Petrified; } set { m_Petrified = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int StatueAnimation { get { return m_StatueAnimation; } set { m_StatueAnimation = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int StatueFrames { get { return m_StatueFrames; } set { m_StatueFrames = value; } }

		public void OnRequestedAnimation( Mobile from )
		{
			if ( m_Petrified )
				from.Send( new UpdateStatueAnimation( this, 1, m_StatueAnimation, m_StatueFrames ) );
		}

		public override void OnAosSingleClick( Mobile from )
		{
			if ( m_Petrified ) // do not show names of petrified creatures
				return;

			base.OnAosSingleClick( from );
		}

		#endregion
	}

	public class LoyaltyTimer : Timer
	{
		private static TimeSpan InternalDelay = TimeSpan.FromMinutes( 5.0 );

		public static void Initialize()
		{
			new LoyaltyTimer().Start();
		}

		public LoyaltyTimer()
			: base( InternalDelay, InternalDelay )
		{
			m_NextHourlyCheck = DateTime.Now + TimeSpan.FromHours( 1.0 );
		}

		private DateTime m_NextHourlyCheck;

		protected override void OnTick()
		{
			bool hasHourElapsed = ( DateTime.Now >= m_NextHourlyCheck );

			if ( hasHourElapsed )
				m_NextHourlyCheck = DateTime.Now + TimeSpan.FromHours( 1.0 );

			ArrayList toRelease = new ArrayList();

			// added array for wild creatures in house regions to be removed
			ArrayList toRemove = new ArrayList();

			foreach ( Mobile m in World.Instance.Mobiles )
			{
				if ( m is BaseMount && ( (BaseMount) m ).Rider != null )
				{
					( (BaseCreature) m ).OwnerAbandonTime = DateTime.MinValue;
					continue;
				}

				if ( m is BaseCreature )
				{
					BaseCreature c = (BaseCreature) m;

					if ( c.IsDeadPet )
					{
						Mobile owner = c.ControlMaster;

						if ( owner == null || owner.Deleted || owner.Map != c.Map || !owner.InRange( c, 12 ) || !c.CanSee( owner ) || !c.InLOS( owner ) )
						{
							if ( c.OwnerAbandonTime == DateTime.MinValue )
								c.OwnerAbandonTime = DateTime.Now;
							else if ( ( c.OwnerAbandonTime + c.BondingAbandonDelay ) <= DateTime.Now )
								toRemove.Add( c );
						}
						else
						{
							c.OwnerAbandonTime = DateTime.MinValue;
						}
					}
					else if ( c.Controlled && c.Commandable && c.Loyalty > PetLoyalty.None && c.Map != Map.Internal )
					{
						//Mobile owner = c.ControlMaster;

						// changed loyalty decrement
						if ( hasHourElapsed )
						{
							--c.Loyalty;

							if ( c.Loyalty == PetLoyalty.Confused )
							{
								c.Say( 1043270, c.Name ); // * ~1_NAME~ looks around desperately *
								c.PlaySound( c.GetIdleSound() );
							}
						}

						c.OwnerAbandonTime = DateTime.MinValue;

						if ( c.Loyalty == PetLoyalty.None )
							toRelease.Add( c );
					}

					// added lines to check if a wild creature in a house region has to be removed or not
					if ( !c.Controlled && c.Region is HouseRegion && c.CanBeDamaged() )
					{
						c.RemoveStep++;

						if ( c.RemoveStep >= 20 )
							toRemove.Add( c );
					}
					else
					{
						c.RemoveStep = 0;
					}
				}
			}

			foreach ( BaseCreature c in toRelease )
			{
				c.Say( 1043255, c.Name ); // ~1_NAME~ appears to have decided that is better off without a master!
				c.Loyalty = PetLoyalty.WonderfullyHappy;
				c.IsBonded = false;
				c.BondingBegin = DateTime.MinValue;
				c.OwnerAbandonTime = DateTime.MinValue;
				c.ControlTarget = null;
				//c.ControlOrder = OrderType.Release;
				c.AIObject.DoOrderRelease(); // this will prevent no release of creatures left alone with AI disabled (and consequent bug of Followers)
				c.RemoveOnSave = true;
			}

			// added code to handle removing of wild creatures in house regions
			foreach ( BaseCreature c in toRemove )
			{
				c.Delete();
			}
		}
	}
}