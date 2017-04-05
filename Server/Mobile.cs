using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server.Accounting;
using Server.ContextMenus;
using Server.Commands;
using Server.Events;
using Server.Guilds;
using Server.Gumps;
using Server.HuePickers;
using Server.Items;
using Server.Menus;
using Server.Mobiles;
using Server.Network;
using Server.Prompts;
using Server.Targeting;

namespace Server
{
	#region Callbacks
	public delegate void TargetCallback( Mobile from, object targeted );
	public delegate void TargetStateCallback( Mobile from, object targeted, object state );
	public delegate void TargetStateCallback<T>( Mobile from, object targeted, T state );
	#endregion

	#region [...]Mods
	public class ResistanceMod
	{
		private Mobile m_Owner;
		private ResistanceType m_Type;
		private int m_Offset;

		public Mobile Owner
		{
			get { return m_Owner; }
			set { m_Owner = value; }
		}

		public ResistanceType Type
		{
			get { return m_Type; }
			set
			{
				if ( m_Type != value )
				{
					m_Type = value;

					if ( m_Owner != null )
						m_Owner.UpdateResistances();
				}
			}
		}

		public int Offset
		{
			get { return m_Offset; }
			set
			{
				if ( m_Offset != value )
				{
					m_Offset = value;

					if ( m_Owner != null )
						m_Owner.UpdateResistances();
				}
			}
		}

		public ResistanceMod( ResistanceType type, int offset )
		{
			m_Type = type;
			m_Offset = offset;
		}
	}

	public class StatMod
	{
		private StatType m_Type;
		private string m_Name;
		private int m_Offset;
		private TimeSpan m_Duration;
		private DateTime m_Added;

		public StatType Type { get { return m_Type; } }
		public string Name { get { return m_Name; } }
		public int Offset { get { return m_Offset; } }

		public bool HasElapsed()
		{
			if ( m_Duration == TimeSpan.Zero )
				return false;

			return ( DateTime.UtcNow - m_Added ) >= m_Duration;
		}

		public StatMod( StatType type, string name, int offset )
			: this( type, name, offset, TimeSpan.MaxValue )
		{
		}

		public StatMod( StatType type, string name, int offset, TimeSpan duration )
		{
			m_Type = type;
			m_Name = string.Intern( name );
			m_Offset = offset;
			m_Duration = duration;
			m_Added = DateTime.UtcNow;
		}
	}

	public class AttributeMod
	{
		private AosAttribute m_Attribute;
		private int m_Offset;

		public AosAttribute Attribute { get { return m_Attribute; } }
		public int Offset { get { return m_Offset; } }

		public AttributeMod( AosAttribute attr, int offset )
		{
			m_Attribute = attr;
			m_Offset = offset;
		}
	}

	#endregion

	public class DamageEntry
	{
		private Mobile m_Damager;
		private int m_DamageGiven;
		private DateTime m_LastDamage;
		private List<DamageEntry> m_Responsible;

		public Mobile Damager { get { return m_Damager; } }
		public int DamageGiven { get { return m_DamageGiven; } set { m_DamageGiven = value; } }
		public DateTime LastDamage { get { return m_LastDamage; } set { m_LastDamage = value; } }
		public bool HasExpired { get { return ( DateTime.UtcNow > ( m_LastDamage + m_ExpireDelay ) ); } }
		public List<DamageEntry> Responsible { get { return m_Responsible; } set { m_Responsible = value; } }

		private static TimeSpan m_ExpireDelay = TimeSpan.FromMinutes( 2.0 );

		public static TimeSpan ExpireDelay
		{
			get { return m_ExpireDelay; }
			set { m_ExpireDelay = value; }
		}

		public DamageEntry( Mobile damager )
		{
			m_Damager = damager;
		}
	}

	#region Enums
	[Flags]
	public enum StatType
	{
		Str = 1,
		Dex = 2,
		Int = 4,
		All = 7
	}

	public enum StatLockType : byte
	{
		Up,
		Down,
		Locked
	}

	[Flags]
	public enum MobileDelta
	{
		None = 0x00000000,
		Name = 0x00000001,
		Flags = 0x00000002,
		Hits = 0x00000004,
		Mana = 0x00000008,
		Stam = 0x00000010,
		Stat = 0x00000020,
		Noto = 0x00000040,
		Gold = 0x00000080,
		Weight = 0x00000100,
		Direction = 0x00000200,
		Hue = 0x00000400,
		Body = 0x00000800,
		Armor = 0x00001000,
		StatCap = 0x00002000,
		GhostUpdate = 0x00004000,
		Followers = 0x00008000,
		Properties = 0x00010000,
		TithingPoints = 0x00020000,
		Resistances = 0x00040000,
		WeaponDamage = 0x00080000,
		Hair = 0x00100000,
		FacialHair = 0x00200000,
		Race = 0x00400000,

		Attributes = 0x0000001C
	}

	public enum HealthBarColor : ushort
	{
		Green = 1,
		Yellow = 2
	}

	public enum ResistanceType
	{
		Physical,
		Fire,
		Cold,
		Poison,
		Energy
	}

	public enum ApplyPoisonResult
	{
		Poisoned,
		Immune,
		HigherPoisonActive,
		Cured
	}
	#endregion

	public class MobileNotConnectedException : Exception
	{
		public MobileNotConnectedException( Mobile source, string message )
			: base( message )
		{
			this.Source = source.ToString();
		}
	}

	#region Delegates

	public delegate bool SkillCheckTargetHandler( Mobile from, SkillName skill, object target, double minSkill, double maxSkill );
	public delegate bool SkillCheckLocationHandler( Mobile from, SkillName skill, double minSkill, double maxSkill );

	public delegate bool SkillCheckDirectTargetHandler( Mobile from, SkillName skill, object target, double chance );
	public delegate bool SkillCheckDirectLocationHandler( Mobile from, SkillName skill, double chance );

	public delegate TimeSpan RegenRateHandler( Mobile from );

	public delegate bool AllowBeneficialHandler( Mobile from, Mobile target );
	public delegate bool AllowHarmfulHandler( Mobile from, Mobile target );

	public delegate Container CreateCorpseHandler( Mobile from, HairInfo hair, FacialHairInfo facialhair, List<Item> initialContent, List<Item> equipedItems );

	#endregion

	/// <summary>
	/// Base class representing players, npcs, and creatures.
	/// </summary>
	public partial class Mobile : IMobile, IHued, ISerializable, ISpawnable
	{
		private static readonly ILog log = LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

		#region Handlers

		public static AllowBeneficialHandler AllowBeneficialHandler { get; set; }
		public static AllowHarmfulHandler AllowHarmfulHandler { get; set; }

		public static SkillCheckTargetHandler SkillCheckTargetHandler { get; set; }
		public static SkillCheckLocationHandler SkillCheckLocationHandler { get; set; }
		public static SkillCheckDirectTargetHandler SkillCheckDirectTargetHandler { get; set; }
		public static SkillCheckDirectLocationHandler SkillCheckDirectLocationHandler { get; set; }

		public bool CheckSkill( SkillName skill, double minSkill, double maxSkill )
		{
			if ( SkillCheckLocationHandler == null )
				return false;

			return SkillCheckLocationHandler( this, skill, minSkill, maxSkill );
		}

		public bool CheckSkill( SkillName skill, double chance )
		{
			if ( SkillCheckDirectLocationHandler == null )
				return false;

			return SkillCheckDirectLocationHandler( this, skill, chance );
		}

		public bool CheckTargetSkill( SkillName skill, object target, double minSkill, double maxSkill )
		{
			if ( SkillCheckTargetHandler == null )
				return false;

			return SkillCheckTargetHandler( this, skill, target, minSkill, maxSkill );
		}

		public bool CheckTargetSkill( SkillName skill, object target, double chance )
		{
			if ( SkillCheckDirectTargetHandler == null )
				return false;

			return SkillCheckDirectTargetHandler( this, skill, target, chance );
		}

		#endregion

		#region Regeneration

		public static RegenRateHandler HitsRegenRateHandler { get; set; }
		public static TimeSpan DefaultHitsRate { get; set; }

		public static RegenRateHandler StamRegenRateHandler { get; set; }
		public static TimeSpan DefaultStamRate { get; set; }

		public static RegenRateHandler ManaRegenRateHandler { get; set; }
		public static TimeSpan DefaultManaRate { get; set; }

		public static TimeSpan GetHitsRegenRate( Mobile m )
		{
			if ( HitsRegenRateHandler == null )
				return DefaultHitsRate;

			return HitsRegenRateHandler( m );
		}

		public static TimeSpan GetStamRegenRate( Mobile m )
		{
			if ( StamRegenRateHandler == null )
				return DefaultStamRate;

			return StamRegenRateHandler( m );
		}

		public static TimeSpan GetManaRegenRate( Mobile m )
		{
			if ( ManaRegenRateHandler == null )
				return DefaultManaRate;

			return ManaRegenRateHandler( m );
		}

		#endregion

		private class MovementRecord
		{
			public DateTime m_End;

			private static Queue<MovementRecord> m_InstancePool = new Queue<MovementRecord>();

			public static MovementRecord NewInstance( DateTime end )
			{
				MovementRecord r;

				if ( m_InstancePool.Count > 0 )
				{
					r = m_InstancePool.Dequeue();

					r.m_End = end;
				}
				else
				{
					r = new MovementRecord( end );
				}

				return r;
			}

			private MovementRecord( DateTime end )
			{
				m_End = end;
			}

			public bool Expired()
			{
				bool v = ( DateTime.UtcNow >= m_End );

				if ( v )
					m_InstancePool.Enqueue( this );

				return v;
			}
		}

		#region Var declarations
		private Serial m_Serial;
		private Map m_Map;
		private Point3D m_Location;
		private Direction m_Direction;
		private Body m_Body;
		private int m_Hue;
		private Poison m_Poison;
		private Timer m_PoisonTimer;
		private BaseGuild m_Guild;
		private string m_GuildTitle;
		private bool m_Criminal;
		private string m_Name;
		private int m_Kills, m_ShortTermMurders;
		private int m_SpeechHue, m_EmoteHue, m_WhisperHue, m_YellHue;
		private string m_Language;
		private NetState m_NetState;
		private bool m_Female, m_Warmode, m_Hidden, m_Blessed;
		private int m_StatCap;
		private int m_Str, m_Dex, m_Int;
		private int m_Hits, m_Stam, m_Mana;
		private int m_Fame, m_Karma;
		private AccessLevel m_AccessLevel;
		private Skills m_Skills;
		private List<Item> m_EquippedItems;
		private bool m_Player;
		private string m_Title;
		private string m_Profile;
		private bool m_ProfileLocked;
		private int m_LightLevel;
		private int m_TotalGold, m_TotalWeight;
		private List<StatMod> m_StatMods;
		private ISpell m_Spell;
		private Target m_Target;
		private Prompt m_Prompt;
		private ContextMenu m_ContextMenu;
		private List<AggressorInfo> m_Aggressors, m_Aggressed;
		private Mobile m_Combatant;
		private List<Mobile> m_Stabled;
		private bool m_AutoPageNotify;
		private bool m_Meditating;
		private bool m_CanHearGhosts;
		private bool m_CanSwim, m_CantWalk;
		private int m_TithingPoints;
		private bool m_DisplayGuildTitle;
		private Mobile m_GuildFealty;
		private DateTime m_NextSpellTime;
		private DateTime[] m_StuckMenuUses;
		private Timer m_ExpireCriminal;
		private Timer m_ExpireAggrTimer;
		private Timer m_LogoutTimer;
		private Timer m_ManaTimer, m_HitsTimer, m_StamTimer;
		private DateTime m_NextSkillTime;
		private DateTime m_NextActionTime;
		private DateTime m_NextActionMessage;
		private bool m_Paralyzed;
		private ParalyzedTimer m_ParaTimer;
		private bool m_Frozen;
		private FrozenTimer m_FrozenTimer;
		private bool m_IsStealthing;
		private int m_AllowedStealthSteps;
		private int m_Hunger;
		private int m_NameHue = -1;
		private Region m_Region;
		private bool m_DisarmReady, m_StunReady;
		private int m_BaseSoundID;
		private bool m_Squelched;
		private int m_MeleeDamageAbsorb;
		private int m_MagicDamageAbsorb;
		private int m_Followers;
		private List<object> m_Actions;
		private Queue<MovementRecord> m_MoveRecords;
		private int m_WarmodeChanges = 0;
		private DateTime m_NextWarmodeChange;
		private WarmodeTimer m_WarmodeTimer;
		private int m_Thirst, m_BAC;
		private VirtueInfo m_Virtues;
		private object m_Party;
		private Body m_BodyMod;
		private Race m_Race;
		private List<AttributeMod> m_AttributeMods;

		#endregion

		#region Instances
		private int m_InstanceID;

		[CommandProperty( AccessLevel.GameMaster )]
		public int InstanceID
		{
			get { return m_InstanceID; }
			set
			{
				if ( m_Deleted )
					return;

				if ( m_InstanceID != value )
				{
					if ( m_NetState != null )
						m_NetState.ValidateAllTrades();

					this.ClearScreen();
					this.SendRemovePacket();

					for ( int i = 0; i < m_EquippedItems.Count; ++i )
						m_EquippedItems[i].InstanceID = value;

					m_InstanceID = value;

					if ( m_NetState != null )
					{
						m_NetState.Sequence = 0;
						ClearFastwalkStack();
					}

					this.SendEverything();
					this.SendIncomingPacket();
				}
			}
		}
		#endregion

		[CommandProperty( AccessLevel.GameMaster )]
		public Race Race
		{
			get
			{
				if ( m_Race == null )
					m_Race = Race.DefaultRace;

				return m_Race;
			}
			set
			{
				Race oldRace = this.Race;

				m_Race = value;

				if ( m_Race == null )
					m_Race = Race.DefaultRace;

				this.Body = m_Race.Body( this );
				this.UpdateResistances();

				Delta( MobileDelta.Race );

				OnRaceChange( oldRace );
			}
		}

		protected virtual void OnRaceChange( Race oldRace )
		{
		}

		private static TimeSpan WarmodeSpamCatch = TimeSpan.FromSeconds( 0.5 );
		private static TimeSpan WarmodeSpamDelay = TimeSpan.FromSeconds( 2.0 );
		private const int WarmodeCatchCount = 4; // Allow four warmode changes in 0.5 seconds, any more will be delay for two seconds

		private List<ResistanceMod> m_ResistMods;

		private int[] m_Resistances;

		public int[] Resistances { get { return m_Resistances; } }

		public virtual int BasePhysicalResistance { get { return 0; } }
		public virtual int BaseFireResistance { get { return 0; } }
		public virtual int BaseColdResistance { get { return 0; } }
		public virtual int BasePoisonResistance { get { return 0; } }
		public virtual int BaseEnergyResistance { get { return 0; } }

		public virtual void ComputeLightLevels( out int global, out int personal )
		{
			ComputeBaseLightLevels( out global, out personal );

			if ( m_Region != null )
				m_Region.AlterLightLevel( this, ref global, ref personal );
		}

		public virtual void ComputeBaseLightLevels( out int global, out int personal )
		{
			global = 0;
			personal = m_LightLevel;
		}

		public virtual void CheckLightLevels( bool forceResend )
		{
		}

		[CommandProperty( AccessLevel.Counselor )]
		public virtual int PhysicalResistance
		{
			get { return GetResistance( ResistanceType.Physical ); }
		}

		[CommandProperty( AccessLevel.Counselor )]
		public virtual int FireResistance
		{
			get { return GetResistance( ResistanceType.Fire ); }
		}

		[CommandProperty( AccessLevel.Counselor )]
		public virtual int ColdResistance
		{
			get { return GetResistance( ResistanceType.Cold ); }
		}

		[CommandProperty( AccessLevel.Counselor )]
		public virtual int PoisonResistance
		{
			get { return GetResistance( ResistanceType.Poison ); }
		}

		[CommandProperty( AccessLevel.Counselor )]
		public virtual int EnergyResistance
		{
			get { return GetResistance( ResistanceType.Energy ); }
		}

		public virtual void UpdateResistances()
		{
			if ( m_Resistances == null )
				m_Resistances = new int[5] { int.MinValue, int.MinValue, int.MinValue, int.MinValue, int.MinValue };

			bool delta = false;

			for ( int i = 0; i < m_Resistances.Length; ++i )
			{
				if ( m_Resistances[i] != int.MinValue )
				{
					m_Resistances[i] = int.MinValue;
					delta = true;
				}
			}

			if ( delta )
				Delta( MobileDelta.Resistances );
		}

		public virtual int GetResistance( ResistanceType type )
		{
			if ( m_Resistances == null )
				m_Resistances = new int[5] { int.MinValue, int.MinValue, int.MinValue, int.MinValue, int.MinValue };

			int v = (int) type;

			if ( v < 0 || v >= m_Resistances.Length )
				return 0;

			int res = m_Resistances[v];

			if ( res == int.MinValue )
			{
				ComputeResistances();
				res = m_Resistances[v];
			}

			return res;
		}

		public virtual void AddResistanceMod( ResistanceMod toAdd )
		{
			if ( m_ResistMods == null )
				m_ResistMods = new List<ResistanceMod>( 2 );

			m_ResistMods.Add( toAdd );
			UpdateResistances();
		}

		public virtual void RemoveResistanceMod( ResistanceMod toRemove )
		{
			if ( m_ResistMods != null )
			{
				m_ResistMods.Remove( toRemove );

				if ( m_ResistMods.Count == 0 )
					m_ResistMods = null;
			}

			UpdateResistances();
		}

		private static int m_MaxPlayerResistance = 70;

		public static int MaxPlayerResistance
		{
			get { return m_MaxPlayerResistance; }
			set { m_MaxPlayerResistance = value; }
		}

		public virtual void ComputeResistances()
		{
			if ( m_Resistances == null )
				m_Resistances = new int[5] { int.MinValue, int.MinValue, int.MinValue, int.MinValue, int.MinValue };

			for ( int i = 0; i < m_Resistances.Length; ++i )
				m_Resistances[i] = 0;

			m_Resistances[0] += this.BasePhysicalResistance;
			m_Resistances[1] += this.BaseFireResistance;
			m_Resistances[2] += this.BaseColdResistance;
			m_Resistances[3] += this.BasePoisonResistance;
			m_Resistances[4] += this.BaseEnergyResistance;

			for ( int i = 0; m_ResistMods != null && i < m_ResistMods.Count; ++i )
			{
				ResistanceMod mod = m_ResistMods[i];
				int v = (int) mod.Type;

				if ( v >= 0 && v < m_Resistances.Length )
					m_Resistances[v] += mod.Offset;
			}

			if ( CheckResistancesInItems )
			{
				for ( int i = 0; i < m_EquippedItems.Count; ++i )
				{
					Item item = m_EquippedItems[i];

					if ( item.CheckPropertyConfliction( this ) )
						continue;

					m_Resistances[0] += item.PhysicalResistance;
					m_Resistances[1] += item.FireResistance;
					m_Resistances[2] += item.ColdResistance;
					m_Resistances[3] += item.PoisonResistance;
					m_Resistances[4] += item.EnergyResistance;
				}
			}

			for ( int i = 0; i < m_Resistances.Length; ++i )
			{
				int min = GetMinResistance( (ResistanceType) i );
				int max = GetMaxResistance( (ResistanceType) i );

				if ( max < min )
					max = min;

				if ( m_Resistances[i] > max )
					m_Resistances[i] = max;
				else if ( m_Resistances[i] < min )
					m_Resistances[i] = min;
			}
		}

		public virtual bool CheckResistancesInItems { get { return true; } }

		public virtual int GetMinResistance( ResistanceType type )
		{
			return int.MinValue;
		}

		public virtual int GetMaxResistance( ResistanceType type )
		{
			if ( m_Player )
			{
				if ( Race == Race.Elf && ( type == ResistanceType.Energy ) )
					return 75;

				return m_MaxPlayerResistance;
			}

			return int.MaxValue;
		}

		public virtual void SendPropertiesTo( Mobile from )
		{
			from.Send( PropertyList );
		}

		public virtual void OnAosSingleClick( Mobile from )
		{
			ObjectPropertyListPacket opl = this.PropertyList;

			if ( opl.Header > 0 )
			{
				int hue;

				if ( m_NameHue != -1 )
					hue = m_NameHue;
				else if ( m_AccessLevel > AccessLevel.Player )
					hue = 11;
				else
					hue = Notoriety.GetHue( Notoriety.Compute( from, this ) );

				from.Send( new MessageLocalized( this.Serial, Body, MessageType.Label, hue, 3, opl.Header, Name, opl.HeaderArgs ) );
			}
		}

		public virtual string ApplyNameSuffix( string suffix )
		{
			return suffix;
		}

		public virtual void AddNameProperties( ObjectPropertyList list )
		{
			string name = Name;

			if ( name == null )
				name = String.Empty;

			string prefix = "";

			if ( ShowFameTitle && ( m_Player || m_Body.IsHuman ) && m_Fame >= 10000 )
				prefix = m_Female ? "Lady" : "Lord";

			string suffix = "";

			if ( PropertyTitle && Title != null && Title.Length > 0 )
				suffix = Title;

			BaseGuild guild = m_Guild;

			if ( guild != null && ( m_Player && m_DisplayGuildTitle ) )
			{
				if ( suffix.Length > 0 )
					suffix = String.Format( "{0} [{1}]", suffix, Utility.FixHtml( guild.Abbreviation ) );
				else
					suffix = String.Format( "[{0}]", Utility.FixHtml( guild.Abbreviation ) );
			}

			suffix = ApplyNameSuffix( suffix );

			list.Add( 1050045, "{0} \t{1}\t {2}", prefix, name, suffix ); // ~1_PREFIX~~2_NAME~~3_SUFFIX~

			if ( guild != null && ( m_DisplayGuildTitle || ( m_Player && guild.Type != GuildType.Regular ) ) )
			{
				string type;

				if ( guild.Type >= 0 && (int) guild.Type < m_GuildTypes.Length )
					type = m_GuildTypes[(int) guild.Type];
				else
					type = "";

				string title = GuildTitle;

				if ( title == null )
					title = "";
				else
					title = title.Trim();

				if ( title.Length > 0 )
					list.Add( "{0}, {1} Guild{2}", Utility.FixHtml( title ), Utility.FixHtml( guild.Name ), type );
				else
					list.Add( Utility.FixHtml( guild.Name ) );
			}

			EventSink.InvokeOPLRequest( new OPLRequestArgs( this, list ) );
		}

		public virtual void GetProperties( ObjectPropertyList list )
		{
			AddNameProperties( list );
		}

		public virtual void GetChildProperties( ObjectPropertyList list, Item item )
		{
		}

		public virtual void GetChildNameProperties( ObjectPropertyList list, Item item )
		{
		}

		private void UpdateAggrExpire()
		{
			if ( m_Deleted || ( m_Aggressors.Count == 0 && m_Aggressed.Count == 0 ) )
			{
				StopAggrExpire();
			}
			else if ( m_ExpireAggrTimer == null )
			{
				m_ExpireAggrTimer = new ExpireAggressorsTimer( this );
				m_ExpireAggrTimer.Start();
			}
		}

		private void StopAggrExpire()
		{
			if ( m_ExpireAggrTimer != null )
				m_ExpireAggrTimer.Stop();

			m_ExpireAggrTimer = null;
		}

		private void CheckAggrExpire()
		{
			for ( int i = m_Aggressors.Count - 1; i >= 0; --i )
			{
				if ( i >= m_Aggressors.Count )
					continue;

				AggressorInfo info = m_Aggressors[i];

				if ( info.Expired )
				{
					Mobile attacker = info.Attacker;
					attacker.RemoveAggressed( this );

					m_Aggressors.RemoveAt( i );
					info.Free();

					if ( m_NetState != null && CanSee( attacker ) && this.InUpdateRange( attacker ) )
						m_NetState.Send( new MobileIncoming( this, attacker ) );
				}
			}

			for ( int i = m_Aggressed.Count - 1; i >= 0; --i )
			{
				if ( i >= m_Aggressed.Count )
					continue;

				AggressorInfo info = m_Aggressed[i];

				if ( info.Expired )
				{
					Mobile defender = info.Defender;
					defender.RemoveAggressor( this );

					m_Aggressed.RemoveAt( i );
					info.Free();

					if ( m_NetState != null && CanSee( defender ) && this.InUpdateRange( defender ) )
						m_NetState.Send( new MobileIncoming( this, defender ) );
				}
			}

			UpdateAggrExpire();
		}

		public List<Mobile> Stabled { get { return m_Stabled; } }

		[CommandProperty( AccessLevel.Counselor, AccessLevel.GameMaster )]
		public VirtueInfo Virtues { get { return m_Virtues; } set { } }

		public object Party { get { return m_Party; } set { m_Party = value; } }

		/// <summary>
		/// Overridable. Virtual event invoked when <paramref name="skill" /> changes in some way.
		/// </summary>
		public virtual void OnSkillInvalidated( Skill skill )
		{
		}

		private class WarmodeTimer : Timer
		{
			private Mobile m_Mobile;
			private bool m_Value;

			public bool Value
			{
				get
				{
					return m_Value;
				}
				set
				{
					m_Value = value;
				}
			}

			public WarmodeTimer( Mobile m, bool value )
				: base( WarmodeSpamDelay )
			{
				m_Mobile = m;
				m_Value = value;
			}

			protected override void OnTick()
			{
				m_Mobile.Warmode = m_Value;
				m_Mobile.m_WarmodeChanges = 0;

				m_Mobile.m_WarmodeTimer = null;
			}
		}

		/// <summary>
		/// Overridable. Virtual event invoked when a client, <paramref name="from" />, invokes a 'help request' for the Mobile. Seemingly no longer functional in newer clients.
		/// </summary>
		public virtual void OnHelpRequest( Mobile from )
		{
		}

		public void DelayChangeWarmode( bool value )
		{
			if ( m_WarmodeTimer != null )
			{
				m_WarmodeTimer.Value = value;
				return;
			}

			if ( m_Warmode == value )
				return;

			DateTime now = DateTime.UtcNow, next = m_NextWarmodeChange;

			if ( now > next || m_WarmodeChanges == 0 )
			{
				m_WarmodeChanges = 1;
				m_NextWarmodeChange = now + WarmodeSpamCatch;
			}
			else if ( m_WarmodeChanges == WarmodeCatchCount )
			{
				m_WarmodeTimer = new WarmodeTimer( this, value );
				m_WarmodeTimer.Start();

				return;
			}
			else
			{
				++m_WarmodeChanges;
			}

			Warmode = value;
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int MeleeDamageAbsorb
		{
			get
			{
				return m_MeleeDamageAbsorb;
			}
			set
			{
				m_MeleeDamageAbsorb = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int MagicDamageAbsorb
		{
			get
			{
				return m_MagicDamageAbsorb;
			}
			set
			{
				m_MagicDamageAbsorb = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int SkillsTotal
		{
			get
			{
				return m_Skills == null ? 0 : m_Skills.Total;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int SkillsCap
		{
			get
			{
				return m_Skills == null ? 0 : m_Skills.Cap;
			}
			set
			{
				if ( m_Skills != null )
					m_Skills.Cap = value;
			}
		}

		public bool InLOS( object target )
		{
			if ( m_Deleted || m_Map == null )
				return false;
			else if ( m_AccessLevel > AccessLevel.Player )
				return true;
			else if ( target is Item && ( (Item) target ).RootParent == this )
				return true;

			return m_Map.LineOfSight( this, target );
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int BaseSoundID
		{
			get
			{
				return m_BaseSoundID;
			}
			set
			{
				m_BaseSoundID = value;
			}
		}

		public DateTime NextCombatTime
		{
			get
			{
				return m_NextCombatTime;
			}
			set
			{
				m_NextCombatTime = value;
			}
		}

		public bool BeginAction( object toLock )
		{
			if ( m_Actions == null )
			{
				m_Actions = new List<object>( 2 );

				m_Actions.Add( toLock );

				return true;
			}
			else if ( !m_Actions.Contains( toLock ) )
			{
				m_Actions.Add( toLock );

				return true;
			}

			return false;
		}

		public bool CanBeginAction( object toLock )
		{
			return ( m_Actions == null || !m_Actions.Contains( toLock ) );
		}

		public void EndAction( object toLock )
		{
			if ( m_Actions != null )
			{
				m_Actions.Remove( toLock );

				if ( m_Actions.Count == 0 )
					m_Actions = null;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int NameHue
		{
			get
			{
				return m_NameHue;
			}
			set
			{
				m_NameHue = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int Hunger
		{
			get
			{
				return m_Hunger;
			}
			set
			{
				int oldValue = m_Hunger;

				if ( oldValue != value )
				{
					m_Hunger = value;

					EventSink.InvokeHungerChanged( new HungerChangedEventArgs( this, oldValue ) );
				}
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int Thirst
		{
			get
			{
				return m_Thirst;
			}
			set
			{
				m_Thirst = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int BAC
		{
			get
			{
				return m_BAC;
			}
			set
			{
				m_BAC = value;
			}
		}

		private DateTime m_LastMoveTime;

		public bool IsStealthing
		{
			get { return m_IsStealthing; }
			set { m_IsStealthing = value; }
		}

		/// <summary>
		/// Gets or sets the number of steps this player may take when hidden before being revealed.
		/// </summary>
		[CommandProperty( AccessLevel.GameMaster )]
		public int AllowedStealthSteps
		{
			get
			{
				return m_AllowedStealthSteps;
			}
			set
			{
				m_AllowedStealthSteps = value;
			}
		}

		/* Logout:
		 *
		 * When a client logs into mobile x
		 *  - if ( x is Internalized ) move x to logout location and map
		 *
		 * When a client attached to a mobile disconnects
		 *  - LogoutTimer is started
		 *	   - Delay is taken from Region.GetLogoutDelay to allow insta-logout regions.
		 *     - OnTick : Location and map are stored, and mobile is internalized
		 *
		 * Some things to consider:
		 *  - An internalized person getting killed (say, by poison). Where does the body go?
		 *  - Regions now have a GetLogoutDelay( Mobile m ); virtual function (see above)
		 */
		private Point3D m_LogoutLocation;
		private Map m_LogoutMap;

		public virtual TimeSpan GetLogoutDelay()
		{
			return Region.GetLogoutDelay( this );
		}

		private StatLockType m_StrLock, m_DexLock, m_IntLock;

		private Item m_Holding;

		public Item Holding
		{
			get
			{
				return m_Holding;
			}
			set
			{
				if ( m_Holding != value )
				{
					if ( m_Holding != null )
					{
						TotalWeight -= m_Holding.TotalWeight + m_Holding.PileWeight;

						if ( m_Holding.HeldBy == this )
							m_Holding.HeldBy = null;
					}

					if ( value != null && m_Holding != null )
						this.DropHolding();

					m_Holding = value;

					if ( m_Holding != null )
					{
						TotalWeight += m_Holding.TotalWeight + m_Holding.PileWeight;

						if ( m_Holding.HeldBy == null )
							m_Holding.HeldBy = this;
					}
				}
			}
		}

		public DateTime LastMoveTime
		{
			get
			{
				return m_LastMoveTime;
			}
			set
			{
				m_LastMoveTime = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public virtual bool Paralyzed
		{
			get
			{
				return m_Paralyzed;
			}
			set
			{
				if ( m_Paralyzed != value )
				{
					m_Paralyzed = value;
					Delta( MobileDelta.Flags );

					this.SendLocalizedMessage( m_Paralyzed ? 502381 : 502382 );

					if ( m_ParaTimer != null )
					{
						m_ParaTimer.Stop();
						m_ParaTimer = null;
					}
				}
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool DisarmReady
		{
			get
			{
				return m_DisarmReady;
			}
			set
			{
				m_DisarmReady = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool StunReady
		{
			get
			{
				return m_StunReady;
			}
			set
			{
				m_StunReady = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Frozen
		{
			get
			{
				return m_Frozen;
			}
			set
			{
				if ( m_Frozen != value )
				{
					m_Frozen = value;

					Delta( MobileDelta.Flags );

					if ( m_FrozenTimer != null )
					{
						m_FrozenTimer.Stop();
						m_FrozenTimer = null;
					}
				}
			}
		}

		public void Paralyze( TimeSpan duration, bool effect = true )
		{
			if ( !m_Paralyzed )
			{
				Paralyzed = true;

				m_ParaTimer = new ParalyzedTimer( this, duration );
				m_ParaTimer.Start();

				if ( effect )
					new FrozenEffectTimer( this ).Start();
			}
		}

		public void Freeze( TimeSpan duration, bool effect = true )
		{
			if ( !m_Frozen )
			{
				m_Frozen = true;

				m_FrozenTimer = new FrozenTimer( this, duration );
				m_FrozenTimer.Start();

				if ( effect )
					new FrozenEffectTimer( this ).Start();
			}
		}

		/// <summary>
		/// Gets or sets the <see cref="StatLockType">lock state</see> for the <see cref="RawStr" /> property.
		/// </summary>
		[CommandProperty( AccessLevel.Counselor, AccessLevel.GameMaster )]
		public StatLockType StrLock
		{
			get
			{
				return m_StrLock;
			}
			set
			{
				if ( m_StrLock != value )
				{
					m_StrLock = value;

					if ( m_NetState != null )
						m_NetState.Send( new StatLockInfo( this ) );
				}
			}
		}

		/// <summary>
		/// Gets or sets the <see cref="StatLockType">lock state</see> for the <see cref="RawDex" /> property.
		/// </summary>
		[CommandProperty( AccessLevel.Counselor, AccessLevel.GameMaster )]
		public StatLockType DexLock
		{
			get
			{
				return m_DexLock;
			}
			set
			{
				if ( m_DexLock != value )
				{
					m_DexLock = value;

					if ( m_NetState != null )
						m_NetState.Send( new StatLockInfo( this ) );
				}
			}
		}

		/// <summary>
		/// Gets or sets the <see cref="StatLockType">lock state</see> for the <see cref="RawInt" /> property.
		/// </summary>
		[CommandProperty( AccessLevel.Counselor, AccessLevel.GameMaster )]
		public StatLockType IntLock
		{
			get
			{
				return m_IntLock;
			}
			set
			{
				if ( m_IntLock != value )
				{
					m_IntLock = value;

					if ( m_NetState != null )
						m_NetState.Send( new StatLockInfo( this ) );
				}
			}
		}

		public override string ToString()
		{
			return String.Format( "0x{0:X} \"{1}\"", this.Serial.Value, Name );
		}

		public DateTime NextActionTime
		{
			get { return m_NextActionTime; }
			set { m_NextActionTime = value; }
		}

		public DateTime NextActionMessage
		{
			get { return m_NextActionMessage; }
			set { m_NextActionMessage = value; }
		}

		public static readonly TimeSpan ActionMessageDelay = TimeSpan.FromSeconds( 0.125 );

		public virtual void SendSkillMessage()
		{
			if ( DateTime.UtcNow < NextActionMessage )
				return;

			NextActionMessage = DateTime.UtcNow + ActionMessageDelay;

			SendLocalizedMessage( 500118 ); // You must wait a few moments to use another skill.
		}

		public virtual void SendActionMessage()
		{
			if ( DateTime.UtcNow < NextActionMessage )
				return;

			NextActionMessage = DateTime.UtcNow + ActionMessageDelay;

			SendLocalizedMessage( 500119 ); // You must wait to perform another action.
		}

		public virtual bool CanRegenHits { get { return this.Alive; } }
		public virtual bool CanRegenStam { get { return this.Alive; } }
		public virtual bool CanRegenMana { get { return this.Alive; } }

		#region Timers

		private class ManaTimer : Timer
		{
			private Mobile m_Owner;

			public ManaTimer( Mobile m )
				: base( Mobile.GetManaRegenRate( m ), Mobile.GetManaRegenRate( m ) )
			{
				m_Owner = m;
			}

			protected override void OnTick()
			{
				if ( m_Owner.CanRegenMana )// m_Owner.Alive )
					m_Owner.Mana++;

				Delay = Interval = Mobile.GetManaRegenRate( m_Owner );
			}
		}

		private class HitsTimer : Timer
		{
			private Mobile m_Owner;

			public HitsTimer( Mobile m )
				: base( Mobile.GetHitsRegenRate( m ), Mobile.GetHitsRegenRate( m ) )
			{
				m_Owner = m;
			}

			protected override void OnTick()
			{
				if ( m_Owner.CanRegenHits )// m_Owner.Alive && !m_Owner.Poisoned )
					m_Owner.Hits++;

				Delay = Interval = Mobile.GetHitsRegenRate( m_Owner );
			}
		}

		private class StamTimer : Timer
		{
			private Mobile m_Owner;

			public StamTimer( Mobile m )
				: base( Mobile.GetStamRegenRate( m ), Mobile.GetStamRegenRate( m ) )
			{
				m_Owner = m;
			}

			protected override void OnTick()
			{
				if ( m_Owner.CanRegenStam )
					m_Owner.Stam++;

				Delay = Interval = Mobile.GetStamRegenRate( m_Owner );
			}
		}

		private class LogoutTimer : Timer
		{
			private Mobile m_Mobile;

			public LogoutTimer( Mobile m )
				: base( TimeSpan.FromDays( 1.0 ) )
			{
				m_Mobile = m;
			}

			protected override void OnTick()
			{
				if ( m_Mobile.m_Map != Map.Internal )
				{
					EventSink.InvokeLogout( new LogoutEventArgs( m_Mobile ) );

					m_Mobile.m_LogoutLocation = m_Mobile.m_Location;
					m_Mobile.m_LogoutMap = m_Mobile.m_Map;

					m_Mobile.Internalize();
				}
			}
		}

		private class FrozenEffectTimer : Timer
		{
			private Mobile m_Mobile;

			public FrozenEffectTimer( Mobile m )
				: base( TimeSpan.Zero, TimeSpan.FromSeconds( 1.0 ) )
			{
				m_Mobile = m;
			}

			protected override void OnTick()
			{
				if ( m_Mobile.Frozen || m_Mobile.Paralyzed )
					m_Mobile.FixedParticles( 0x376A, 9, 32, 0x13AF, 0, 0, EffectLayer.RightFoot, 0 );
				else
					Stop();
			}
		}

		private class ParalyzedTimer : Timer
		{
			private Mobile m_Mobile;

			public ParalyzedTimer( Mobile m, TimeSpan duration )
				: base( duration )
			{
				m_Mobile = m;
			}

			protected override void OnTick()
			{
				m_Mobile.Paralyzed = false;
			}
		}

		private class FrozenTimer : Timer
		{
			private Mobile m_Mobile;

			public FrozenTimer( Mobile m, TimeSpan duration )
				: base( duration )
			{
				m_Mobile = m;
			}

			protected override void OnTick()
			{
				m_Mobile.Frozen = false;
			}
		}

		private static TimeSpan m_ExpireCriminalDelay = TimeSpan.FromMinutes( 2.0 );

		public static TimeSpan ExpireCriminalDelay
		{
			get { return m_ExpireCriminalDelay; }
			set { m_ExpireCriminalDelay = value; }
		}

		private class ExpireCriminalTimer : Timer
		{
			private Mobile m_Mobile;

			public ExpireCriminalTimer( Mobile m )
				: base( m_ExpireCriminalDelay )
			{
				m_Mobile = m;
			}

			protected override void OnTick()
			{
				m_Mobile.Criminal = false;
			}
		}

		private class ExpireAggressorsTimer : Timer
		{
			private Mobile m_Mobile;

			public ExpireAggressorsTimer( Mobile m )
				: base( TimeSpan.FromSeconds( 5.0 ), TimeSpan.FromSeconds( 5.0 ) )
			{
				m_Mobile = m;
			}

			protected override void OnTick()
			{
				if ( m_Mobile.Deleted || ( m_Mobile.Aggressors.Count == 0 && m_Mobile.Aggressed.Count == 0 ) )
					m_Mobile.StopAggrExpire();
				else
					m_Mobile.CheckAggrExpire();
			}
		}

		#endregion

		private DateTime m_NextCombatTime;

		public DateTime NextSkillTime
		{
			get { return m_NextSkillTime; }
			set { m_NextSkillTime = value; }
		}

		public List<AggressorInfo> Aggressors
		{
			get { return m_Aggressors; }
		}

		public List<AggressorInfo> Aggressed
		{
			get { return m_Aggressed; }
		}

		private int m_ChangingCombatant;

		public bool ChangingCombatant
		{
			get { return ( m_ChangingCombatant > 0 ); }
		}

		public virtual bool CanAttack( Mobile m )
		{
			return ( this.CanSee( m ) && this.InLOS( m ) && this.InUpdateRange( m ) );
		}

		public virtual void OnAttack( Mobile m )
		{
		}

		/// <summary>
		/// Overridable. Gets or sets which Mobile that this Mobile is currently engaged in combat with.
		/// <seealso cref="OnCombatantChange" />
		/// </summary>
		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Combatant
		{
			get
			{
				return m_Combatant;
			}
			set
			{
				if ( m_Deleted )
					return;

				if ( m_Combatant != value && value != this )
				{
					Mobile old = m_Combatant;

					++m_ChangingCombatant;
					m_Combatant = value;

					if ( ( m_Combatant != null && !CanBeHarmful( m_Combatant, false ) ) || !Region.OnCombatantChange( this, old, m_Combatant ) )
					{
						m_Combatant = old;
						--m_ChangingCombatant;
						return;
					}

					if ( m_NetState != null )
						m_NetState.Send( new ChangeCombatant( m_Combatant ) );

					#region Combat
					if ( Combatant == null )
					{
						if ( m_CombatContext != null )
							m_CombatContext.EndCombat();

						m_CombatContext = null;
					}
					else
					{
						if ( m_CombatContext == null )
							m_CombatContext = new CombatContext( this );

						m_CombatContext.BeginCombat();
					}
					#endregion

					if ( m_Combatant != null && CanBeHarmful( m_Combatant, false ) )
					{
						DoHarmful( m_Combatant );

						if ( m_Combatant != null )
							m_Combatant.PlaySound( m_Combatant.GetAngerSound() );
					}

					OnCombatantChange();
					--m_ChangingCombatant;
				}
			}
		}

		/// <summary>
		/// Overridable. Virtual event invoked after the <see cref="Combatant" /> property has changed.
		/// <seealso cref="Combatant" />
		/// </summary>
		public virtual void OnCombatantChange()
		{
		}

		public virtual void AggressiveAction( Mobile aggressor )
		{
			AggressiveAction( aggressor, false );
		}

		public virtual void AggressiveAction( Mobile aggressor, bool criminal )
		{
			if ( aggressor == this )
				return;

			AggressiveActionEventArgs args = AggressiveActionEventArgs.Create( this, aggressor, criminal );

			EventSink.InvokeAggressiveAction( args );

			args.Free();

			#region Combat
			if ( Combatant == aggressor )
			{
				if ( m_CombatContext == null )
					m_CombatContext = new CombatContext( this );

				m_CombatContext.RefreshExpireCombatant();
			}
			#endregion

			bool addAggressor = true;

			List<AggressorInfo> list = m_Aggressors;

			for ( int i = 0; i < list.Count; ++i )
			{
				AggressorInfo info = list[i];

				if ( info.Attacker == aggressor )
				{
					info.Refresh();
					info.CriminalAggression = criminal;
					info.CanReportMurder = criminal;

					addAggressor = false;
				}
			}

			list = aggressor.m_Aggressors;

			for ( int i = 0; i < list.Count; ++i )
			{
				AggressorInfo info = list[i];

				if ( info.Attacker == this )
				{
					info.Refresh();

					addAggressor = false;
				}
			}

			bool addAggressed = true;

			list = m_Aggressed;

			for ( int i = 0; i < list.Count; ++i )
			{
				AggressorInfo info = list[i];

				if ( info.Defender == aggressor )
				{
					info.Refresh();

					addAggressed = false;
				}
			}

			list = aggressor.m_Aggressed;

			for ( int i = 0; i < list.Count; ++i )
			{
				AggressorInfo info = list[i];

				if ( info.Defender == this )
				{
					info.Refresh();
					info.CriminalAggression = criminal;
					info.CanReportMurder = criminal;

					addAggressed = false;
				}
			}

			bool setCombatant = false;

			if ( addAggressor )
			{
				m_Aggressors.Add( AggressorInfo.Create( aggressor, this, criminal ) );

				if ( this.CanSee( aggressor ) && m_NetState != null )
					m_NetState.Send( new MobileIncoming( this, aggressor ) );

				if ( Combatant == null )
					setCombatant = true;

				UpdateAggrExpire();
			}

			if ( addAggressed )
			{
				aggressor.m_Aggressed.Add( AggressorInfo.Create( aggressor, this, criminal ) );

				if ( this.CanSee( aggressor ) && m_NetState != null )
					m_NetState.Send( new MobileIncoming( this, aggressor ) );

				if ( Combatant == null )
					setCombatant = true;

				UpdateAggrExpire();
			}

			if ( setCombatant )
				Combatant = aggressor;

			Region.OnAggressed( aggressor, this, criminal );
		}

		public void RemoveAggressed( Mobile aggressed )
		{
			if ( m_Deleted )
				return;

			List<AggressorInfo> list = m_Aggressed;

			for ( int i = 0; i < list.Count; ++i )
			{
				AggressorInfo info = list[i];

				if ( info.Defender == aggressed )
				{
					m_Aggressed.RemoveAt( i );
					info.Free();

					if ( m_NetState != null && this.CanSee( aggressed ) )
						m_NetState.Send( new MobileIncoming( this, aggressed ) );

					break;
				}
			}

			UpdateAggrExpire();
		}

		public void RemoveAggressor( Mobile aggressor )
		{
			if ( m_Deleted )
				return;

			List<AggressorInfo> list = m_Aggressors;

			for ( int i = 0; i < list.Count; ++i )
			{
				AggressorInfo info = list[i];

				if ( info.Attacker == aggressor )
				{
					m_Aggressors.RemoveAt( i );
					info.Free();

					if ( m_NetState != null && this.CanSee( aggressor ) )
						m_NetState.Send( new MobileIncoming( this, aggressor ) );

					break;
				}
			}

			UpdateAggrExpire();
		}

		#region Combat
		private CombatContext m_CombatContext;

		private class CombatContext
		{
			private Mobile m_Owner;
			private Timer m_CombatTimer;
			private Timer m_ExpireCombatant;

			public CombatContext( Mobile owner )
			{
				m_Owner = owner;
			}

			public void BeginCombat()
			{
				if ( m_ExpireCombatant == null )
					m_ExpireCombatant = new ExpireCombatantTimer( m_Owner );

				m_ExpireCombatant.Start();

				if ( m_CombatTimer == null )
					m_CombatTimer = new CombatTimer( m_Owner );

				m_CombatTimer.Start();
			}

			public void RefreshExpireCombatant()
			{
				if ( m_ExpireCombatant == null )
					m_ExpireCombatant = new ExpireCombatantTimer( m_Owner );
				else
					m_ExpireCombatant.Stop();

				m_ExpireCombatant.Start();
			}

			public void EndCombat()
			{
				if ( m_ExpireCombatant != null )
					m_ExpireCombatant.Stop();

				if ( m_CombatTimer != null )
					m_CombatTimer.Stop();

				m_ExpireCombatant = null;
				m_CombatTimer = null;
			}
		}

		private class CombatTimer : Timer
		{
			private readonly Mobile m_Mobile;

			public CombatTimer( Mobile m )
				: base( TimeSpan.Zero, TimeSpan.FromMilliseconds( 10.0 ), 0 )
			{
				m_Mobile = m;
			}

			protected override void OnTick()
			{
				if ( DateTime.UtcNow > m_Mobile.NextCombatTime )
				{
					Mobile combatant = m_Mobile.Combatant;

					if ( combatant == null || combatant.Deleted || m_Mobile.Deleted || combatant.Map != m_Mobile.Map || !combatant.Alive || !m_Mobile.Alive || !m_Mobile.CanSee( combatant ) || combatant.IsDeadBondedPet || m_Mobile.IsDeadBondedPet || combatant.Hidden )
					{
						m_Mobile.Combatant = null;
						return;
					}

					IWeapon weapon = m_Mobile.Weapon;

					if ( (int) m_Mobile.GetDistanceToSqrt( combatant ) > weapon.GetMaxRange( m_Mobile, combatant ) )
						return;

					if ( m_Mobile.InLOS( combatant ) )
					{
						weapon.OnBeforeSwing( m_Mobile, combatant );

						m_Mobile.RevealingAction();
						m_Mobile.NextCombatTime = DateTime.UtcNow + weapon.OnSwing( m_Mobile, combatant );
					}
				}
			}
		}

		private class ExpireCombatantTimer : Timer
		{
			private readonly Mobile m_Mobile;

			public ExpireCombatantTimer( Mobile m )
				: base( TimeSpan.FromMinutes( 1.0 ) )
			{
				m_Mobile = m;
			}

			protected override void OnTick()
			{
				m_Mobile.Combatant = null;
			}
		}
		#endregion

		[CommandProperty( AccessLevel.GameMaster )]
		public int TotalGold
		{
			get
			{
				return m_TotalGold;
			}
			set
			{
				if ( m_TotalGold != value )
				{
					m_TotalGold = value;

					Delta( MobileDelta.Gold );
				}
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int TithingPoints
		{
			get
			{
				return m_TithingPoints;
			}
			set
			{
				if ( m_TithingPoints != value )
				{
					m_TithingPoints = value;

					Delta( MobileDelta.TithingPoints );
				}
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int Followers
		{
			get
			{
				return m_Followers;
			}
			set
			{
				if ( m_Followers != value )
				{
					m_Followers = value;

					Delta( MobileDelta.Followers );
				}
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int FollowersMax { get { return 5; } }

		public virtual void UpdateTotals()
		{
			if ( m_EquippedItems == null )
				return;

			int oldValue = m_TotalWeight;

			m_TotalGold = 0;
			m_TotalWeight = 0;

			for ( int i = 0; i < m_EquippedItems.Count; ++i )
			{
				Item item = m_EquippedItems[i];

				item.UpdateTotals();

				if ( !( item is BankBox ) )
				{
					if ( !( item is Container && ( (Container) item ).UseLockedRestriction && ( (Container) item ).IsLockedContainer ) )
						m_TotalGold += item.TotalGold;

					m_TotalWeight += item.TotalWeight + item.PileWeight;
				}
			}

			if ( m_Holding != null )
				m_TotalWeight += m_Holding.TotalWeight + m_Holding.PileWeight;

			OnWeightChange( oldValue );
		}

		public virtual int GetMaxWeight()
		{
			int maxweight = 40 + (int) ( 3.5 * this.Str );

			/*
			 * Racial Abilities: Strong Back (Humans)
			 * Humans have an increased carrying capacity above what is determined by their strength.
			 */
			if ( Race == Race.Human )
				maxweight += 60;

			return maxweight;
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int TotalWeight
		{
			get
			{
				return m_TotalWeight;
			}
			set
			{
				int oldValue = m_TotalWeight;

				if ( oldValue != value )
				{
					m_TotalWeight = value;

					Delta( MobileDelta.Weight );

					OnWeightChange( oldValue );
				}
			}
		}

		public void ClearQuestArrow()
		{
			m_QuestArrow = null;
		}

		public void ClearTarget()
		{
			m_Target = null;
		}

		private bool m_TargetLocked;

		public bool TargetLocked
		{
			get { return m_TargetLocked; }
			set
			{
				m_TargetLocked = value;

				if ( !m_TargetLocked && m_Target != null && m_NetState != null )
					m_NetState.Send( m_Target.GetPacketFor( m_NetState ) );
			}
		}

		public Target Target
		{
			get
			{
				return m_Target;
			}
			set
			{
				Target oldTarget = m_Target;
				Target newTarget = value;

				if ( oldTarget == newTarget )
					return;

				m_Target = null;

				if ( oldTarget != null && newTarget != null )
					oldTarget.Cancel( this, TargetCancelType.Overriden );

				m_Target = newTarget;

				if ( newTarget != null && m_NetState != null && !m_TargetLocked )
					m_NetState.Send( newTarget.GetPacketFor( m_NetState ) );

				OnTargetChange();
			}
		}

		/// <summary>
		/// Overridable. Virtual event invoked after the <see cref="Target">Target property</see> has changed.
		/// </summary>
		protected virtual void OnTargetChange()
		{
		}

		public ContextMenu ContextMenu
		{
			get
			{
				return m_ContextMenu;
			}
			set
			{
				m_ContextMenu = value;

				if ( m_ContextMenu != null )
					Send( new DisplayContextMenu( m_ContextMenu ) );
			}
		}

		public virtual bool CheckContextMenuDisplay( IEntity target )
		{
			return true;
		}

		public Prompt Prompt
		{
			get
			{
				return m_Prompt;
			}
			set
			{
				Prompt oldPrompt = m_Prompt;
				Prompt newPrompt = value;

				if ( oldPrompt == newPrompt )
					return;

				m_Prompt = null;

				if ( oldPrompt != null && newPrompt != null )
					oldPrompt.OnCancel( this );

				m_Prompt = newPrompt;

				if ( newPrompt != null )
					newPrompt.SendTo( this );
			}
		}

		private bool InternalOnMove( Direction d )
		{
			if ( !OnMove( d ) )
				return false;

			MovementEventArgs e = MovementEventArgs.Create( this, d );

			EventSink.InvokeMovement( e );

			bool ret = !e.Blocked;

			e.Free();

			return ret;
		}

		/// <summary>
		/// Overridable. Event invoked before the Mobile <see cref="Move">moves</see>.
		/// </summary>
		/// <returns>True if the move is allowed, false if not.</returns>
		protected virtual bool OnMove( Direction d )
		{
			if ( m_Hidden && m_AccessLevel == AccessLevel.Player )
			{
				if ( m_AllowedStealthSteps-- <= 0 || ( d & Direction.Running ) != 0 || this.Mounted )
					RevealingAction();
			}

			return true;
		}

		private static Packet[] m_MovingPacketCache = new Packet[8];

		private bool m_Pushing;

		public bool Pushing
		{
			get
			{
				return m_Pushing;
			}
			set
			{
				m_Pushing = value;
			}
		}

		private static TimeSpan m_WalkFoot = TimeSpan.FromSeconds( 0.4 );
		private static TimeSpan m_RunFoot = TimeSpan.FromSeconds( 0.2 );
		private static TimeSpan m_WalkMount = TimeSpan.FromSeconds( 0.2 );
		private static TimeSpan m_RunMount = TimeSpan.FromSeconds( 0.1 );

		public static TimeSpan WalkFoot { get { return m_WalkFoot; } }
		public static TimeSpan RunFoot { get { return m_RunFoot; } }
		public static TimeSpan WalkMount { get { return m_WalkMount; } }
		public static TimeSpan RunMount { get { return m_RunMount; } }

		private DateTime m_EndQueue;

		private static AccessLevel m_FwdAccessOverride = AccessLevel.GameMaster;
		private static bool m_FwdEnabled = true;
		private static bool m_FwdUOTDOverride = false;
		private static int m_FwdMaxSteps = 4;

		public static AccessLevel FwdAccessOverride { get { return m_FwdAccessOverride; } set { m_FwdAccessOverride = value; } }
		public static bool FwdEnabled { get { return m_FwdEnabled; } set { m_FwdEnabled = value; } }
		public static bool FwdUOTDOverride { get { return m_FwdUOTDOverride; } set { m_FwdUOTDOverride = value; } }
		public static int FwdMaxSteps { get { return m_FwdMaxSteps; } set { m_FwdMaxSteps = value; } }

		public virtual void ClearFastwalkStack()
		{
			if ( m_MoveRecords != null && m_MoveRecords.Count > 0 )
				m_MoveRecords.Clear();

			m_EndQueue = DateTime.UtcNow;
		}

		public virtual bool CheckMovement( Direction d, out int newZ )
		{
			return Movement.Movement.CheckMovement( this, d, out newZ );
		}

		public virtual void OnAfterMove( Point3D oldLocation )
		{
		}

		private static int m_GlobalMaxUpdateRange = 24;

		public static int GlobalMaxUpdateRange
		{
			get { return m_GlobalMaxUpdateRange; }
			set { m_GlobalMaxUpdateRange = value; }
		}

		private bool m_ForcedWalk, m_ForcedRun;

		public bool ForcedWalk
		{
			get { return m_ForcedWalk; }
			set
			{
				m_ForcedWalk = value;
				InvalidateSpeed();
			}
		}

		public bool ForcedRun
		{
			get { return m_ForcedRun; }
			set
			{
				m_ForcedRun = value;
				InvalidateSpeed();
			}
		}

		public void InvalidateSpeed()
		{
			if ( m_ForcedWalk )
				Send( SpeedControl.WalkSpeed );
			else if ( m_ForcedRun )
				Send( SpeedControl.MountSpeed );
			else
				Send( SpeedControl.Disable );
		}

		public TimeSpan ComputeMovementSpeed()
		{
			return ComputeMovementSpeed( this.Direction, false );
		}
		public TimeSpan ComputeMovementSpeed( Direction dir )
		{
			return ComputeMovementSpeed( dir, true );
		}
		public virtual TimeSpan ComputeMovementSpeed( Direction dir, bool checkTurning )
		{
			TimeSpan delay;

			if ( Mounted )
				delay = ( dir & Direction.Running ) != 0 ? m_RunMount : m_WalkMount;
			else
				delay = ( dir & Direction.Running ) != 0 ? m_RunFoot : m_WalkFoot;

			return delay;
		}

		/// <summary>
		/// Overridable. Virtual event invoked when a Mobile <paramref name="m" /> moves off this Mobile.
		/// </summary>
		/// <returns>True if the move is allowed, false if not.</returns>
		public virtual bool OnMoveOff( Mobile m )
		{
			return true;
		}

		public virtual bool IsDeadBondedPet { get { return false; } }

		public virtual bool HasFreeMovement()
		{
			return false;
		}

		/// <summary>
		/// Overridable. Event invoked when a Mobile <paramref name="m" /> moves over this Mobile.
		/// </summary>
		/// <returns>True if the move is allowed, false if not.</returns>
		public virtual bool OnMoveOver( Mobile m )
		{
			if ( m_Map == null || m_Deleted )
				return true;

			if ( ( m_Map.Rules & MapRules.FreeMovement ) == 0 )
			{
				if ( !Alive || !m.Alive || IsDeadBondedPet || m.IsDeadBondedPet )
					return true;
				else if ( m_Hidden && m_AccessLevel > AccessLevel.Player )
					return true;
				else if ( m.HasFreeMovement() )
					return true;

				if ( !m.m_Pushing )
				{
					m.m_Pushing = true;

					int number;

					if ( m.AccessLevel > AccessLevel.Player )
					{
						number = m_Hidden ? 1019041 : 1019040;
					}
					else
					{
						if ( m.Stam == m.StamMax )
						{
							number = m_Hidden ? 1019043 : 1019042;
							m.Stam -= 10;

							m.RevealingAction();
						}
						else
						{
							return false;
						}
					}

					m.SendLocalizedMessage( number );
				}
			}

			return true;
		}

		/// <summary>
		/// Overridable. Virtual event invoked when the Mobile sees another Mobile, <paramref name="m" />, move.
		/// </summary>
		public virtual void OnMovement( Mobile m, Point3D oldLocation )
		{
		}

		public ISpell Spell
		{
			get
			{
				return m_Spell;
			}
			set
			{
				if ( m_Spell != null && value != null )
					log.Info( "Warning: Spell has been overwritten" );

				m_Spell = value;
			}
		}

		[CommandProperty( AccessLevel.Administrator )]
		public bool AutoPageNotify
		{
			get
			{
				return m_AutoPageNotify;
			}
			set
			{
				m_AutoPageNotify = value;
			}
		}

		public virtual void CriminalAction( bool message )
		{
			if ( m_Deleted )
				return;

			Criminal = true;

			this.Region.OnCriminalAction( this, message );
		}

		public bool CanUseStuckMenu()
		{
			if ( m_StuckMenuUses == null )
			{
				return true;
			}
			else
			{
				for ( int i = 0; i < m_StuckMenuUses.Length; ++i )
				{
					if ( ( DateTime.UtcNow - m_StuckMenuUses[i] ) > TimeSpan.FromDays( 1.0 ) )
						return true;
				}

				return false;
			}
		}

		public void UsedStuckMenu()
		{
			if ( m_StuckMenuUses == null )
				m_StuckMenuUses = new DateTime[2];

			for ( int i = 0; i < m_StuckMenuUses.Length; ++i )
			{
				if ( ( DateTime.UtcNow - m_StuckMenuUses[i] ) > TimeSpan.FromDays( 1.0 ) )
				{
					m_StuckMenuUses[i] = DateTime.UtcNow;
					return;
				}
			}
		}

		public virtual bool IsSnoop( Mobile from )
		{
			return ( from != this );
		}

		/// <summary>
		/// Overridable. Any call to <see cref="Resurrect" /> will silently fail if this method returns false.
		/// <seealso cref="Resurrect" />
		/// </summary>
		public virtual bool CheckResurrect()
		{
			return true;
		}

		/// <summary>
		/// Overridable. Event invoked before the Mobile is <see cref="Resurrect">resurrected</see>.
		/// <seealso cref="Resurrect" />
		/// </summary>
		public virtual void OnBeforeResurrect()
		{
		}

		/// <summary>
		/// Overridable. Event invoked after the Mobile is <see cref="Resurrect">resurrected</see>.
		/// <seealso cref="Resurrect" />
		/// </summary>
		public virtual void OnAfterResurrect()
		{
		}

		public void Resurrect( Mobile healer = null )
		{
			if ( !Alive )
			{
				if ( !Region.OnResurrect( this, healer ) )
					return;

				if ( !CheckResurrect() )
					return;

				OnBeforeResurrect();

				BankBox box = FindBankNoCreate();

				if ( box != null && box.Opened )
					box.Close();

				Poison = null;

				Warmode = false;

				Hits = 10;
				Stam = StamMax;
				Mana = 0;

				BodyMod = (Body) 0;
				Body = this.Race.AliveBody( this );

				ProcessDeltaQueue();

				for ( int i = m_EquippedItems.Count - 1; i >= 0; --i )
				{
					if ( i >= m_EquippedItems.Count )
						continue;

					Item item = m_EquippedItems[i];

					if ( item.ItemID == 0x204E )
						item.Delete();
				}

				this.SendIncomingPacket();
				this.SendIncomingPacket();

				OnAfterResurrect();

				//Send( new DeathStatus( false ) );
			}
		}

		private IAccount m_Account;

		[CommandProperty( AccessLevel.Counselor, AccessLevel.Administrator + 1 )]
		public IAccount Account
		{
			get
			{
				return m_Account;
			}
			set
			{
				m_Account = value;
			}
		}

		private bool m_Deleted;

		public bool Deleted
		{
			get
			{
				return m_Deleted;
			}
		}

		public void DropHolding()
		{
			Item holding = m_Holding;

			if ( holding != null )
			{
				if ( !holding.Deleted && holding.Map == Map.Internal )
					if ( !this.AddToBackpack( holding ) && holding.QuestItem )
						m_Backpack.DropItem( holding );

				Holding = null;
				holding.ClearBounce();
			}
		}

		public virtual void Delete()
		{
			if ( m_Deleted )
				return;
			else if ( !World.OnDelete( this ) )
				return;

			if ( m_NetState != null )
				m_NetState.CancelAllTrades();

			if ( m_NetState != null )
				m_NetState.Dispose();

			this.DropHolding();

			Region.OnRegionChange( this, m_Region, null );

			m_Region = null;
			//Is the above line REALLY needed?  The old Region system did NOT have said line
			//and worked fine, because of this a LOT of extra checks have to be done everywhere...
			//I guess this should be there for Garbage collection purposes, but, still, is it /really/ needed?

			if ( m_Spawner != null )
			{
				m_Spawner.Remove( this );
				m_Spawner = null;
			}

			OnDelete();

			for ( int i = m_EquippedItems.Count - 1; i >= 0; --i )
				if ( i < m_EquippedItems.Count )
					m_EquippedItems[i].OnParentDeleted( this );

			this.SendRemovePacket();

			if ( m_Guild != null )
				m_Guild.OnDelete( this );

			m_Deleted = true;

			if ( m_Map != null )
			{
				m_Map.OnLeave( this );
				m_Map = null;
			}

			m_Hair = null;
			m_FacialHair = null;

			m_MountItem = null;

			World.RemoveMobile( this );

			EventSink.InvokeDeleted( new DeletedEventArgs( this ) );

			StopAggrExpire();

			CheckAggrExpire();

			if ( m_PoisonTimer != null )
				m_PoisonTimer.Stop();

			if ( m_HitsTimer != null )
				m_HitsTimer.Stop();

			if ( m_StamTimer != null )
				m_StamTimer.Stop();

			if ( m_ManaTimer != null )
				m_ManaTimer.Stop();

			if ( m_LogoutTimer != null )
				m_LogoutTimer.Stop();

			if ( m_ExpireCriminal != null )
				m_ExpireCriminal.Stop();

			if ( m_WarmodeTimer != null )
				m_WarmodeTimer.Stop();

			if ( m_ParaTimer != null )
				m_ParaTimer.Stop();

			if ( m_FrozenTimer != null )
				m_FrozenTimer.Stop();

			if ( m_AutoManifestTimer != null )
				m_AutoManifestTimer.Stop();

			OnAfterDelete();

			FreeCache();
		}

		/// <summary>
		/// Overridable. Virtual event invoked before the mobile is deleted.
		/// </summary>
		public virtual void OnDelete()
		{
		}

		/// <summary>
		/// Overridable. Virtual event invoked after the mobile is deleted.
		/// </summary>
		public virtual void OnAfterDelete()
		{
		}

		/// <summary>
		/// Overridable. Returns true if the player is alive, false if otherwise. By default, this is computed by: <c>!Deleted &amp;&amp; (!Player || !Body.IsGhost)</c>
		/// </summary>
		[CommandProperty( AccessLevel.Counselor )]
		public virtual bool Alive
		{
			get
			{
				return !m_Deleted && ( !m_Player || !m_Body.IsGhost );
			}
		}

		public virtual bool CheckSpellCast( ISpell spell )
		{
			return true;
		}

		/// <summary>
		/// Overridable. Virtual event invoked when the Mobile casts a <paramref name="spell" />.
		/// </summary>
		/// <param name="spell"></param>
		public virtual void OnSpellCast( ISpell spell )
		{
		}

		/// <summary>
		/// Overridable. Virtual event invoked after <see cref="TotalWeight" /> changes.
		/// </summary>
		public virtual void OnWeightChange( int oldValue )
		{
		}

		/// <summary>
		/// Overridable. Virtual event invoked when the <see cref="Skill.Base" /> or <see cref="Skill.BaseFixedPoint" /> property of <paramref name="skill" /> changes.
		/// </summary>
		public virtual void OnSkillChange( SkillName skill, double oldBase )
		{
		}

		public virtual bool AllowSkillUse( SkillName name )
		{
			return true;
		}

		public virtual bool UseSkill( int skillID )
		{
			return Skills.UseSkill( this, skillID );
		}

		private static CreateCorpseHandler m_CreateCorpse;

		public static CreateCorpseHandler CreateCorpseHandler
		{
			get { return m_CreateCorpse; }
			set { m_CreateCorpse = value; }
		}

		public virtual DeathMoveResult GetParentMoveResultFor( Item item )
		{
			return item.OnParentDeath( this );
		}

		public virtual DeathMoveResult GetInventoryMoveResultFor( Item item )
		{
			return item.OnInventoryDeath( this );
		}

		public virtual bool RetainPackLocsOnDeath { get { return true; } }

		public void Kill()
		{
			if ( !CanBeDamaged() )
				return;
			else if ( !Alive || IsDeadBondedPet )
				return;
			else if ( m_Deleted )
				return;
			else if ( !Region.OnBeforeDeath( this ) )
				return;
			else if ( !OnBeforeDeath() )
				return;

			BankBox box = FindBankNoCreate();

			if ( box != null && box.Opened )
				box.Close();

			if ( m_NetState != null )
				m_NetState.CancelAllTrades();

			if ( m_Spell != null )
				m_Spell.OnCasterKilled();

			if ( m_Target != null )
				m_Target.Cancel( this, TargetCancelType.Canceled );

			DisruptiveAction();

			Warmode = false;

			this.DropHolding();

			Hits = 0;
			Stam = 0;
			Mana = 0;

			Poison = null;
			Combatant = null;

			if ( Paralyzed )
			{
				Paralyzed = false;

				if ( m_ParaTimer != null )
					m_ParaTimer.Stop();
			}

			if ( Frozen )
			{
				Frozen = false;

				if ( m_FrozenTimer != null )
					m_FrozenTimer.Stop();
			}

			List<Item> content = new List<Item>();
			List<Item> equip = new List<Item>();
			List<Item> moveToPack = new List<Item>();

			List<Item> itemsCopy = new List<Item>( m_EquippedItems );

			Container pack = this.Backpack;

			for ( int i = 0; i < itemsCopy.Count; ++i )
			{
				Item item = itemsCopy[i];

				if ( item == pack )
					continue;

				DeathMoveResult res = GetParentMoveResultFor( item );

				switch ( res )
				{
					case DeathMoveResult.MoveToCorpse:
						{
							content.Add( item );
							equip.Add( item );
							break;
						}
					case DeathMoveResult.MoveToBackpack:
						{
							moveToPack.Add( item );
							break;
						}
				}
			}

			if ( pack != null )
			{
				List<Item> packCopy = pack.Items;

				for ( int i = 0; i < packCopy.Count; ++i )
				{
					Item item = packCopy[i];

					DeathMoveResult res = GetInventoryMoveResultFor( item );

					if ( res == DeathMoveResult.MoveToCorpse )
						content.Add( item );
					else
						moveToPack.Add( item );
				}

				for ( int i = 0; i < moveToPack.Count; ++i )
				{
					Item item = moveToPack[i];

					if ( RetainPackLocsOnDeath && item.Parent == pack )
						continue;

					pack.DropItem( item );
				}
			}

			HairInfo hair = null;
			if ( m_Hair != null )
				hair = new HairInfo( m_Hair.ItemID, m_Hair.Hue );

			FacialHairInfo facialhair = null;
			if ( m_FacialHair != null )
				facialhair = new FacialHairInfo( m_FacialHair.ItemID, m_FacialHair.Hue );

			Container c = ( m_CreateCorpse == null ? null : m_CreateCorpse( this, hair, facialhair, content, equip ) );

			if ( !m_DeathList.Contains( this ) )
			{
				if ( DoEffectTimerOnDeath )
				{
					m_DeathList.Add( this );
					Timer.DelayCall( TimeSpan.FromSeconds( 1.25 ), new TimerStateCallback( DelFromDeathList ), this );
				}

				InvokeDead( new DeadEventArgs( c ) );
			}

			OnAfterDeath( c );
		}

		public virtual bool DoEffectTimerOnDeath { get { return false; } }

		private static ArrayList m_DeathList = new ArrayList();

		private static void DelFromDeathList( object o )
		{
			try
			{
				Mobile m = o as Mobile;
				m_DeathList.Remove( m );
			}
			catch { }
		}

		private Container m_Corpse;

		[CommandProperty( AccessLevel.GameMaster )]
		public Container Corpse
		{
			get
			{
				return m_Corpse;
			}
			set
			{
				m_Corpse = value;
			}
		}

		/// <summary>
		/// Overridable. Event invoked before the Mobile is <see cref="Kill">killed</see>.
		/// <seealso cref="Kill" />
		/// <seealso cref="OnAfterDeath" />
		/// </summary>
		/// <returns>True to continue with death, false to override it.</returns>
		public virtual bool OnBeforeDeath()
		{
			return true;
		}

		/// <summary>
		/// Overridable. Event invoked after the Mobile is <see cref="Kill">killed</see>. Primarily, this method is responsible for deleting an NPC or turning a PC into a ghost.
		/// <seealso cref="Kill" />
		/// <seealso cref="OnBeforeDeath" />
		/// </summary>
		protected virtual void OnAfterDeath( Container c )
		{
			int sound = this.GetDeathSound();

			if ( sound >= 0 )
				Effects.PlaySound( this, this.Map, sound );

			if ( !m_Player )
			{
				Delete();
			}
			else
			{
				Send( DeathStatus.Instantiate( true ) );

				Warmode = false;

				BodyMod = (Body) 0;
				Body = this.Race.GhostBody( this );

				Item deathShroud = new Item( 0x204E );

				deathShroud.Movable = false;
				deathShroud.Layer = Layer.OuterTorso;

				AddItem( deathShroud );

				m_EquippedItems.Remove( deathShroud );
				m_EquippedItems.Insert( 0, deathShroud );

				Poison = null;
				Combatant = null;

				Hits = 0;
				Stam = 0;
				Mana = 0;

				EventSink.InvokePlayerDeath( new PlayerDeathEventArgs( this ) );

				ProcessDeltaQueue();

				Send( DeathStatus.Instantiate( false ) );

				CheckStatTimers();
			}
		}

		public virtual int GetAngerSound()
		{
			if ( m_BaseSoundID != 0 )
				return m_BaseSoundID;

			return -1;
		}

		public virtual int GetIdleSound()
		{
			if ( m_BaseSoundID != 0 )
				return m_BaseSoundID + 1;

			return -1;
		}

		public virtual int GetAttackSound()
		{
			if ( m_BaseSoundID != 0 )
				return m_BaseSoundID + 2;

			return -1;
		}

		public virtual int GetHurtSound()
		{
			if ( m_BaseSoundID != 0 )
				return m_BaseSoundID + 3;

			return -1;
		}

		public virtual int GetDeathSound()
		{
			if ( m_BaseSoundID != 0 )
			{
				return m_BaseSoundID + 4;
			}
			else if ( m_Body.IsHuman )
			{
				return Utility.Random( m_Female ? 0x314 : 0x423, m_Female ? 4 : 5 );
			}
			else
			{
				return -1;
			}
		}

		private static char[] m_GhostChars = new char[2] { 'o', 'O' };

		public static char[] GhostChars { get { return m_GhostChars; } set { m_GhostChars = value; } }

		private static bool m_NoSpeechLOS;

		public static bool NoSpeechLOS { get { return m_NoSpeechLOS; } set { m_NoSpeechLOS = value; } }

		private static TimeSpan m_AutoManifestTimeout = TimeSpan.FromSeconds( 5.0 );

		public static TimeSpan AutoManifestTimeout { get { return m_AutoManifestTimeout; } set { m_AutoManifestTimeout = value; } }

		private Timer m_AutoManifestTimer;

		private class AutoManifestTimer : Timer
		{
			private Mobile m_Mobile;

			public AutoManifestTimer( Mobile m, TimeSpan delay )
				: base( delay )
			{
				m_Mobile = m;
			}

			protected override void OnTick()
			{
				if ( !m_Mobile.Alive )
					m_Mobile.Warmode = false;
			}
		}

		public virtual bool CheckTarget( Mobile from, Target targ, object targeted )
		{
			return true;
		}

		public static bool InsuranceEnabled { get; set; }

		public virtual bool CheckLOSOnUse { get { return true; } }

		public static Item LiftItemDupe( Item oldItem, int amount )
		{
			Item item;
			try
			{
				item = (Item) Activator.CreateInstance( oldItem.GetType() );
			}
			catch
			{
				log.Info( "Warning: 0x{0:X}: Item must have a zero paramater constructor to be separated from a stack. '{1}'.", oldItem.Serial.Value, oldItem.GetType().Name );
				return null;
			}
			item.Visible = oldItem.Visible;
			item.Movable = oldItem.Movable;
			item.LootType = oldItem.LootType;
			item.Direction = oldItem.Direction;
			item.Hue = oldItem.PrivateHue;
			item.ItemID = oldItem.ItemID;
			item.Location = oldItem.Location;
			item.Layer = oldItem.Layer;
			item.Name = oldItem.Name;
			item.Weight = oldItem.Weight;
			item.Amount = oldItem.Amount - amount;
			item.Map = oldItem.Map;
			item.QuestItem = oldItem.QuestItem;

			oldItem.Amount = amount;
			oldItem.OnAfterDuped( item );

			if ( oldItem.Parent is Mobile )
				( (Mobile) oldItem.Parent ).AddItem( item );
			else if ( oldItem.Parent is Item )
				( (Item) oldItem.Parent ).AddItem( item );

			item.Delta( ItemDelta.Update );

			return item;
		}

		private static object m_GhostMutateContext = new object();

		public virtual bool MutateSpeech( ArrayList hears, ref string text, ref object context )
		{
			if ( Alive )
				return false;

			if ( this.Skills[SkillName.SpiritSpeak].Value >= 100.0 )
				return false;

			StringBuilder sb = new StringBuilder( text.Length, text.Length );

			for ( int i = 0; i < text.Length; ++i )
			{
				if ( text[i] != ' ' )
					sb.Append( m_GhostChars[Utility.Random( m_GhostChars.Length )] );
				else
					sb.Append( ' ' );
			}

			text = sb.ToString();
			context = m_GhostMutateContext;
			return true;
		}

		public virtual void Manifest( TimeSpan delay )
		{
			Warmode = true;

			if ( m_AutoManifestTimer == null )
				m_AutoManifestTimer = new AutoManifestTimer( this, delay );
			else
				m_AutoManifestTimer.Stop();

			m_AutoManifestTimer.Start();
		}

		public virtual bool CheckSpeechManifest()
		{
			if ( Alive )
				return false;

			TimeSpan delay = m_AutoManifestTimeout;

			if ( delay > TimeSpan.Zero && ( !Warmode || m_AutoManifestTimer != null ) )
			{
				Manifest( delay );
				return true;
			}

			return false;
		}

		public virtual bool CheckHearsMutatedSpeech( Mobile m, object context )
		{
			if ( context == m_GhostMutateContext )
				return ( m.Alive && !m.CanHearGhosts );

			return true;
		}

		private void AddSpeechItemsFrom( ArrayList list, Container cont )
		{
			for ( int i = 0; i < cont.Items.Count; ++i )
			{
				Item item = (Item) cont.Items[i];

				if ( item.HandlesOnSpeech )
					list.Add( item );

				if ( item is Container )
					AddSpeechItemsFrom( list, (Container) item );
			}
		}

		private class LocationComparer : IComparer
		{
			private static LocationComparer m_Instance;

			public static LocationComparer GetInstance( IPoint3D relativeTo )
			{
				if ( m_Instance == null )
					m_Instance = new LocationComparer( relativeTo );
				else
					m_Instance.m_RelativeTo = relativeTo;

				return m_Instance;
			}

			private IPoint3D m_RelativeTo;

			public LocationComparer( IPoint3D relativeTo )
			{
				m_RelativeTo = relativeTo;
			}

			private int GetDistance( IPoint3D p )
			{
				int x = m_RelativeTo.X - p.X;
				int y = m_RelativeTo.Y - p.Y;
				int z = m_RelativeTo.Z - p.Z;

				x *= 11;
				y *= 11;

				return ( x * x ) + ( y * y ) + ( z * z );
			}

			public int Compare( object x, object y )
			{
				IPoint3D a = x as IPoint3D;
				IPoint3D b = y as IPoint3D;

				return GetDistance( a ) - GetDistance( b );
			}
		}

		private static ArrayList m_Hears;
		private static ArrayList m_OnSpeech;

		private List<DamageEntry> m_DamageEntries;

		public List<DamageEntry> DamageEntries
		{
			get { return m_DamageEntries; }
		}

		public static Mobile GetDamagerFrom( DamageEntry de )
		{
			return ( de == null ? null : de.Damager );
		}

		public Mobile FindMostRecentDamager( bool allowSelf )
		{
			return GetDamagerFrom( FindMostRecentDamageEntry( allowSelf ) );
		}

		public DamageEntry FindMostRecentDamageEntry( bool allowSelf )
		{
			for ( int i = m_DamageEntries.Count - 1; i >= 0; --i )
			{
				if ( i >= m_DamageEntries.Count )
					continue;

				DamageEntry de = m_DamageEntries[i];

				if ( de.HasExpired )
					m_DamageEntries.RemoveAt( i );
				else if ( allowSelf || de.Damager != this )
					return de;
			}

			return null;
		}

		public Mobile FindLeastRecentDamager( bool allowSelf )
		{
			return GetDamagerFrom( FindLeastRecentDamageEntry( allowSelf ) );
		}

		public DamageEntry FindLeastRecentDamageEntry( bool allowSelf )
		{
			for ( int i = 0; i < m_DamageEntries.Count; ++i )
			{
				if ( i < 0 )
					continue;

				DamageEntry de = m_DamageEntries[i];

				if ( de.HasExpired )
				{
					m_DamageEntries.RemoveAt( i );
					--i;
				}
				else if ( allowSelf || de.Damager != this )
				{
					return de;
				}
			}

			return null;
		}

		public Mobile FindMostTotalDamger( bool allowSelf )
		{
			return GetDamagerFrom( FindMostTotalDamageEntry( allowSelf ) );
		}

		public DamageEntry FindMostTotalDamageEntry( bool allowSelf )
		{
			DamageEntry mostTotal = null;

			for ( int i = m_DamageEntries.Count - 1; i >= 0; --i )
			{
				if ( i >= m_DamageEntries.Count )
					continue;

				DamageEntry de = m_DamageEntries[i];

				if ( de.HasExpired )
					m_DamageEntries.RemoveAt( i );
				else if ( ( allowSelf || de.Damager != this ) && ( mostTotal == null || de.DamageGiven > mostTotal.DamageGiven ) )
					mostTotal = de;
			}

			return mostTotal;
		}

		public Mobile FindLeastTotalDamger( bool allowSelf )
		{
			return GetDamagerFrom( FindLeastTotalDamageEntry( allowSelf ) );
		}

		public DamageEntry FindLeastTotalDamageEntry( bool allowSelf )
		{
			DamageEntry mostTotal = null;

			for ( int i = m_DamageEntries.Count - 1; i >= 0; --i )
			{
				if ( i >= m_DamageEntries.Count )
					continue;

				DamageEntry de = m_DamageEntries[i];

				if ( de.HasExpired )
					m_DamageEntries.RemoveAt( i );
				else if ( ( allowSelf || de.Damager != this ) && ( mostTotal == null || de.DamageGiven < mostTotal.DamageGiven ) )
					mostTotal = de;
			}

			return mostTotal;
		}

		public DamageEntry FindDamageEntryFor( Mobile m )
		{
			for ( int i = m_DamageEntries.Count - 1; i >= 0; --i )
			{
				if ( i >= m_DamageEntries.Count )
					continue;

				DamageEntry de = m_DamageEntries[i];

				if ( de.HasExpired )
					m_DamageEntries.RemoveAt( i );
				else if ( de.Damager == m )
					return de;
			}

			return null;
		}

		public virtual Mobile GetDamageMaster( Mobile damagee )
		{
			return null;
		}

		public virtual DamageEntry RegisterDamage( int amount, Mobile from )
		{
			DamageEntry de = FindDamageEntryFor( from );

			if ( de == null )
				de = new DamageEntry( from );

			de.DamageGiven += amount;
			de.LastDamage = DateTime.UtcNow;

			m_DamageEntries.Remove( de );
			m_DamageEntries.Add( de );

			Mobile master = from.GetDamageMaster( this );

			if ( master != null )
			{
				List<DamageEntry> list = de.Responsible;

				if ( list == null )
					de.Responsible = list = new List<DamageEntry>();

				DamageEntry resp = null;

				for ( int i = 0; i < list.Count; ++i )
				{
					DamageEntry check = list[i];

					if ( check.Damager == master )
					{
						resp = check;
						break;
					}
				}

				if ( resp == null )
					list.Add( resp = new DamageEntry( master ) );

				resp.DamageGiven += amount;
				resp.LastDamage = DateTime.UtcNow;
			}

			return de;
		}

		private Mobile m_LastKiller;

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile LastKiller
		{
			get { return m_LastKiller; }
			set { m_LastKiller = value; }
		}

		public virtual bool CanBeDamaged()
		{
			return !m_Blessed;
		}

		public virtual void Damage( int amount )
		{
			Damage( amount, null );
		}

		public virtual void Damage( int amount, Mobile from )
		{
			Damage( amount, from, true );
		}

		public virtual void Damage( int amount, Mobile from, bool informMount )
		{
			if ( !CanBeDamaged() )
				return;

			if ( !Region.OnDamage( this, ref amount ) )
				return;

			BeforeDamageEventArgs args = new BeforeDamageEventArgs( this, from, amount );

			EventSink.InvokeBeforeDamage( args );

			amount = args.Amount;

			if ( amount > 0 )
			{
				int oldHits = Hits;
				int newHits = oldHits - amount;

				if ( newHits < 0 )
					amount = oldHits;

				if ( m_Spell != null )
					m_Spell.OnCasterHurt();

				if ( from != null )
					RegisterDamage( amount, from );

				DisruptiveAction();

				if ( ( m_Skills[SkillName.Hiding].Value * 0.75 / 100.0 ) < Utility.RandomDouble() )
					Hidden = false;

				Paralyzed = false;

				InvokeDamaged( new DamagedEventArgs( amount, from ) );

				OnDamage( amount, from, newHits < 0 );

				IMount m = this.Mount;
				if ( m != null && informMount )
					m.OnRiderDamaged( amount, from, newHits < 0 );

				if ( newHits < 0 )
				{
					m_LastKiller = from;

					Hits = 0;

					if ( oldHits >= 0 )
						Kill();
				}
				else
				{
					Hits = newHits;
				}
			}
		}

		/// <summary>
		/// Overridable. Virtual event invoked when the Mobile is <see cref="Damage">damaged</see>. It is called before <see cref="Hits">hit points</see> are lowered or the Mobile is <see cref="Kill">killed</see>.
		/// <seealso cref="Damage" />
		/// <seealso cref="Hits" />
		/// <seealso cref="Kill" />
		/// </summary>
		public virtual void OnDamage( int amount, Mobile from, bool willKill )
		{
		}

		public void Heal( int amount, Mobile healer = null, bool message = true )
		{
			if ( !Alive || IsDeadBondedPet )
				return;

			if ( !Region.OnHeal( this, healer, ref amount ) )
				return;

			if ( ( Hits + amount ) > HitsMax )
				amount = HitsMax - Hits;

			Hits += amount;

			if ( message && amount > 0 && m_NetState != null )
				m_NetState.Send( new MessageLocalizedAffix( Serial.MinusOne, -1, MessageType.Label, 0x3B2, 3, 1008158, "", AffixType.Append | AffixType.System, amount.ToString(), "" ) );
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Squelched
		{
			get
			{
				return m_Squelched;
			}
			set
			{
				m_Squelched = value;
			}
		}

		public virtual void Deserialize( GenericReader reader )
		{
			int version = reader.ReadInt();

			switch ( version )
			{
				case 36:
				case 35:
				case 34:
					{
						byte hairflag = reader.ReadByte();

						if ( ( hairflag & 0x01 ) != 0 )
							m_Hair = new HairInfo( reader );

						if ( ( hairflag & 0x02 ) != 0 )
							m_FacialHair = new FacialHairInfo( reader );

						m_InstanceID = reader.ReadInt();

						m_Race = reader.ReadRace();

						if ( version < 36 )
							reader.ReadDeltaTime();

						m_TithingPoints = reader.ReadInt();

						m_Corpse = reader.ReadItem() as Container;

						m_CreationTime = reader.ReadDateTime();

						m_Stabled = reader.ReadStrongMobileList();

						m_CantWalk = reader.ReadBool();

						m_Virtues = new VirtueInfo( reader );

						m_Thirst = reader.ReadInt();
						m_BAC = reader.ReadInt();

						m_ShortTermMurders = reader.ReadInt();

						if ( version < 35 )
							reader.ReadInt();

						m_MagicDamageAbsorb = reader.ReadInt();

						m_GuildFealty = reader.ReadMobile();
						m_Guild = reader.ReadGuild();
						m_DisplayGuildTitle = reader.ReadBool();

						m_CanSwim = reader.ReadBool();

						m_Squelched = reader.ReadBool();

						m_Holding = reader.ReadItem();

						m_BaseSoundID = reader.ReadInt();

						m_DisarmReady = reader.ReadBool();
						m_StunReady = reader.ReadBool();

						m_StatCap = reader.ReadInt();

						m_NameHue = reader.ReadInt();

						m_Hunger = reader.ReadInt();

						m_Location = reader.ReadPoint3D();
						m_Body = new Body( reader.ReadInt() );
						m_Name = reader.ReadString();
						if ( m_Name != null )
							m_Name = string.Intern( m_Name );
						m_GuildTitle = reader.ReadString();
						m_Criminal = reader.ReadBool();
						m_Kills = reader.ReadInt();
						m_SpeechHue = reader.ReadInt();
						m_EmoteHue = reader.ReadInt();
						m_WhisperHue = reader.ReadInt();
						m_YellHue = reader.ReadInt();
						m_Language = reader.ReadString();
						if ( m_Language != null )
							m_Language = string.Intern( m_Language );
						m_Female = reader.ReadBool();
						m_Warmode = reader.ReadBool();
						m_Hidden = reader.ReadBool();
						m_Direction = (Direction) reader.ReadByte();
						m_Hue = reader.ReadInt();
						m_Str = reader.ReadInt();
						m_Dex = reader.ReadInt();
						m_Int = reader.ReadInt();
						m_Hits = reader.ReadInt();
						m_Stam = reader.ReadInt();
						m_Mana = reader.ReadInt();
						m_Map = reader.ReadMap();
						m_Blessed = reader.ReadBool();
						m_Fame = reader.ReadInt();
						m_Karma = reader.ReadInt();
						m_AccessLevel = (AccessLevel) reader.ReadByte();

						m_Skills = new Skills( this, reader );

						int itemCount = reader.ReadInt();

						m_EquippedItems = new List<Item>( itemCount );

						for ( int i = 0; i < itemCount; ++i )
						{
							Item item = reader.ReadItem();

							if ( item != null )
								m_EquippedItems.Add( item );
						}

						m_Player = reader.ReadBool();
						m_Title = reader.ReadString();
						if ( m_Title != null )
							m_Title = string.Intern( m_Title );
						m_Profile = reader.ReadString();
						m_ProfileLocked = reader.ReadBool();

						m_AutoPageNotify = reader.ReadBool();

						m_LogoutLocation = reader.ReadPoint3D();
						m_LogoutMap = reader.ReadMap();

						m_StrLock = (StatLockType) reader.ReadByte();
						m_DexLock = (StatLockType) reader.ReadByte();
						m_IntLock = (StatLockType) reader.ReadByte();

						m_StatMods = new List<StatMod>();

						if ( reader.ReadBool() )
						{
							m_StuckMenuUses = new DateTime[reader.ReadInt()];

							for ( int i = 0; i < m_StuckMenuUses.Length; ++i )
							{
								m_StuckMenuUses[i] = reader.ReadDateTime();
							}
						}
						else
						{
							m_StuckMenuUses = null;
						}

						if ( m_Player && m_Map != Map.Internal )
						{
							m_LogoutLocation = m_Location;
							m_LogoutMap = m_Map;

							m_Map = Map.Internal;
						}

						if ( m_Map != null )
							m_Map.OnEnter( this );

						if ( m_Criminal )
						{
							if ( m_ExpireCriminal == null )
								m_ExpireCriminal = new ExpireCriminalTimer( this );

							m_ExpireCriminal.Start();
						}

						if ( ShouldCheckStatTimers )
							CheckStatTimers();

						UpdateRegion();

						UpdateResistances();

						break;
					}
			}
		}

		public virtual bool ShouldCheckStatTimers { get { return true; } }

		public virtual void CheckStatTimers()
		{
			if ( m_Deleted )
				return;

			if ( Hits < HitsMax )
			{
				if ( CanRegenHits )
				{
					if ( m_HitsTimer == null )
						m_HitsTimer = new HitsTimer( this );

					m_HitsTimer.Start();
				}
				else if ( m_HitsTimer != null )
				{
					m_HitsTimer.Stop();
				}
			}
			else
			{
				Hits = HitsMax;
			}

			if ( Stam < StamMax )
			{
				if ( CanRegenStam )
				{
					if ( m_StamTimer == null )
						m_StamTimer = new StamTimer( this );

					m_StamTimer.Start();
				}
				else if ( m_StamTimer != null )
				{
					m_StamTimer.Stop();
				}
			}
			else
			{
				Stam = StamMax;
			}

			if ( Mana < ManaMax )
			{
				if ( CanRegenMana )
				{
					if ( m_ManaTimer == null )
						m_ManaTimer = new ManaTimer( this );

					m_ManaTimer.Start();
				}
				else if ( m_ManaTimer != null )
				{
					m_ManaTimer.Stop();
				}
			}
			else
			{
				Mana = ManaMax;
			}
		}

		private DateTime m_CreationTime;

		[CommandProperty( AccessLevel.GameMaster )]
		public DateTime CreationTime
		{
			get
			{
				return m_CreationTime;
			}
		}

		int ISerializable.TypeReference
		{
			get { return m_TypeRef; }
		}

		Serial ISerializable.SerialIdentity
		{
			get { return this.Serial; }
		}

		public virtual void Serialize( GenericWriter writer )
		{
			writer.Write( (int) 36 ); // version

			/* Version 32 */
			byte hairflag = 0x00;

			if ( m_Hair != null )
				hairflag |= 0x01;
			if ( m_FacialHair != null )
				hairflag |= 0x02;

			writer.Write( (byte) hairflag );

			if ( ( hairflag & 0x01 ) != 0 )
				m_Hair.Serialize( writer );
			if ( ( hairflag & 0x02 ) != 0 )
				m_FacialHair.Serialize( writer );

			/* Version 31 */
			writer.Write( m_InstanceID );

			/* Version 30 */
			writer.Write( m_Race );

			writer.Write( (int) m_TithingPoints );

			writer.Write( m_Corpse );

			writer.Write( m_CreationTime );

			writer.WriteMobileList( m_Stabled, true );

			writer.Write( m_CantWalk );

			VirtueInfo.Serialize( writer, m_Virtues );

			writer.Write( m_Thirst );
			writer.Write( m_BAC );

			writer.Write( m_ShortTermMurders );

			writer.Write( m_MagicDamageAbsorb );

			writer.Write( m_GuildFealty );

			writer.Write( m_Guild );

			writer.Write( m_DisplayGuildTitle );

			writer.Write( m_CanSwim );

			writer.Write( m_Squelched );

			writer.Write( m_Holding );

			writer.Write( m_BaseSoundID );

			writer.Write( m_DisarmReady );
			writer.Write( m_StunReady );

			//Poison.Serialize( m_Poison, writer );

			writer.Write( m_StatCap );

			writer.Write( m_NameHue );

			writer.Write( m_Hunger );

			writer.Write( m_Location );
			writer.Write( (int) m_Body );
			writer.Write( m_Name );
			writer.Write( m_GuildTitle );
			writer.Write( m_Criminal );
			writer.Write( m_Kills );
			writer.Write( m_SpeechHue );
			writer.Write( m_EmoteHue );
			writer.Write( m_WhisperHue );
			writer.Write( m_YellHue );
			writer.Write( m_Language );
			writer.Write( m_Female );
			writer.Write( m_Warmode );
			writer.Write( m_Hidden );
			writer.Write( (byte) m_Direction );
			writer.Write( m_Hue );
			writer.Write( m_Str );
			writer.Write( m_Dex );
			writer.Write( m_Int );
			writer.Write( m_Hits );
			writer.Write( m_Stam );
			writer.Write( m_Mana );

			writer.Write( m_Map );

			writer.Write( m_Blessed );
			writer.Write( m_Fame );
			writer.Write( m_Karma );
			writer.Write( (byte) m_AccessLevel );
			m_Skills.Serialize( writer );

			writer.Write( m_EquippedItems.Count );

			for ( int i = 0; i < m_EquippedItems.Count; ++i )
				writer.Write( (Item) m_EquippedItems[i] );

			writer.Write( m_Player );
			writer.Write( m_Title );
			writer.Write( m_Profile );
			writer.Write( m_ProfileLocked );
			writer.Write( m_AutoPageNotify );

			writer.Write( m_LogoutLocation );
			writer.Write( m_LogoutMap );

			writer.Write( (byte) m_StrLock );
			writer.Write( (byte) m_DexLock );
			writer.Write( (byte) m_IntLock );

			if ( m_StuckMenuUses != null )
			{
				writer.Write( true );

				writer.Write( m_StuckMenuUses.Length );

				for ( int i = 0; i < m_StuckMenuUses.Length; ++i )
				{
					writer.Write( m_StuckMenuUses[i] );
				}
			}
			else
			{
				writer.Write( false );
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int LightLevel
		{
			get
			{
				return m_LightLevel;
			}
			set
			{
				if ( m_LightLevel != value )
				{
					m_LightLevel = value;

					CheckLightLevels( false );

					/*if ( m_NetState != null )
						m_NetState.Send( new PersonalLightLevel( this ) );*/
				}
			}
		}

		[CommandProperty( AccessLevel.Counselor, AccessLevel.GameMaster )]
		public string Profile
		{
			get
			{
				return m_Profile;
			}
			set
			{
				m_Profile = value;
			}
		}

		[CommandProperty( AccessLevel.Counselor, AccessLevel.GameMaster )]
		public bool ProfileLocked
		{
			get
			{
				return m_ProfileLocked;
			}
			set
			{
				m_ProfileLocked = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster, AccessLevel.Administrator )]
		public bool Player
		{
			get
			{
				return m_Player;
			}
			set
			{
				m_Player = value;
				InvalidateProperties();

				CheckStatTimers();
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public string Title
		{
			get
			{
				return m_Title;
			}
			set
			{
				m_Title = value;

				if ( m_Title != null )
					m_Title = string.Intern( m_Title );

				InvalidateProperties();
			}
		}

		private static string[] m_AccessLevelNames = new string[]
			{
				"a player",
				"a counselor",
				"a game master",
				"a seer",
				"an administrator",
				"a developer",
				"an owner"
			};

		public static string GetAccessLevelName( AccessLevel level )
		{
			return m_AccessLevelNames[(int) level];
		}

		public virtual bool CanPaperdollBeOpenedBy( Mobile from )
		{
			return ( Body.IsHuman || Body.IsGhost || IsBodyMod );
		}

		public virtual void GetChildContextMenuEntries( Mobile from, List<ContextMenuEntry> list, Item item )
		{
		}

		public virtual void GetContextMenuEntries( Mobile from, List<ContextMenuEntry> list )
		{
			if ( m_Deleted )
				return;

			if ( CanPaperdollBeOpenedBy( from ) )
				list.Add( new PaperdollEntry( this ) );

			if ( from == this && Backpack != null && CanSee( Backpack ) && CheckAlive( false ) )
				list.Add( new OpenBackpackEntry( this ) );
		}

		public void Internalize()
		{
			Map = Map.Internal;
		}

		/// <summary>
		/// Provides an enumerable instance over all the equipped items of this mobile.
		/// </summary>
		public IEnumerable<Item> GetEquippedItems()
		{
			if ( m_EquippedItems == null )
				return Enumerable.Empty<Item>();

			return m_EquippedItems.ToArray();
		}

		/// <summary>
		/// Overridable. Virtual event invoked when <paramref name="item" /> is <see cref="AddItem">added</see> from the Mobile, such as when it is equiped.
		/// <seealso cref="Items" />
		/// <seealso cref="OnItemRemoved" />
		/// </summary>
		public virtual void OnItemAdded( Item item )
		{
		}

		/// <summary>
		/// Overridable. Virtual event invoked when <paramref name="item" /> is <see cref="RemoveItem">removed</see> from the Mobile.
		/// <seealso cref="Items" />
		/// <seealso cref="OnItemAdded" />
		/// </summary>
		public virtual void OnItemRemoved( Item item )
		{
		}

		/// <summary>
		/// Overridable. Virtual event invoked when <paramref name="item" /> is becomes a child of the Mobile; it's worn or contained at some level of the Mobile's <see cref="Mobile.Backpack">backpack</see> or <see cref="Mobile.BankBox">bank box</see>
		/// <seealso cref="OnSubItemRemoved" />
		/// <seealso cref="OnItemAdded" />
		/// </summary>
		public virtual void OnSubItemAdded( Item item )
		{
		}

		/// <summary>
		/// Overridable. Virtual event invoked when <paramref name="item" /> is removed from the Mobile, its <see cref="Mobile.Backpack">backpack</see>, or its <see cref="Mobile.BankBox">bank box</see>.
		/// <seealso cref="OnSubItemAdded" />
		/// <seealso cref="OnItemRemoved" />
		/// </summary>
		public virtual void OnSubItemRemoved( Item item )
		{
		}

		public virtual void OnItemBounceCleared( Item item )
		{
		}

		public virtual void OnSubItemBounceCleared( Item item )
		{
		}

		public void AddItem( Item item )
		{
			if ( item == null || item.Deleted )
				return;

			if ( item.Parent == this )
				return;
			else if ( item.Parent is Mobile )
				( (Mobile) item.Parent ).RemoveItem( item );
			else if ( item.Parent is Item )
				( (Item) item.Parent ).RemoveItem( item );
			else
				item.SendRemovePacket();

			item.Parent = this;
			item.Map = m_Map;

			m_EquippedItems.Add( item );

			if ( !( item is BankBox ) )
			{
				TotalWeight += item.TotalWeight + item.PileWeight;

				if ( !( item is Container && ( (Container) item ).UseLockedRestriction && ( (Container) item ).IsLockedContainer ) )
					TotalGold += item.TotalGold;
			}

			item.Delta( ItemDelta.Update );

			item.OnAdded( this );
			OnItemAdded( item );

			if ( item.PhysicalResistance != 0 || item.FireResistance != 0 || item.ColdResistance != 0 ||
				item.PoisonResistance != 0 || item.EnergyResistance != 0 )
				UpdateResistances();
		}

		private static IWeapon m_DefaultWeapon;

		public static IWeapon DefaultWeapon
		{
			get
			{
				return m_DefaultWeapon;
			}
			set
			{
				m_DefaultWeapon = value;
			}
		}

		public void RemoveItem( Item item )
		{
			if ( item == null || m_EquippedItems == null )
				return;

			if ( m_EquippedItems.Contains( item ) )
			{
				item.SendRemovePacket();

				m_EquippedItems.Remove( item );

				if ( !( item is BankBox ) )
				{
					TotalWeight -= item.TotalWeight + item.PileWeight;

					if ( !( item is Container && ( (Container) item ).UseLockedRestriction && ( (Container) item ).IsLockedContainer ) )
					{
						if ( ( TotalGold - item.TotalGold ) > 0 )
							TotalGold -= item.TotalGold;
						else
							TotalGold = 0;
					}
				}

				item.Parent = null;

				item.OnRemoved( this );
				OnItemRemoved( item );

				if ( item.PhysicalResistance != 0 || item.FireResistance != 0 || item.ColdResistance != 0 ||
					item.PoisonResistance != 0 || item.EnergyResistance != 0 )
					UpdateResistances();
			}
		}

		public void SendSound( int soundID )
		{
			if ( soundID != -1 && m_NetState != null )
				Send( GenericPackets.PlaySound( soundID, this ) );
		}

		public void SendSound( int soundID, IPoint3D p )
		{
			if ( soundID != -1 && m_NetState != null )
				Send( GenericPackets.PlaySound( soundID, p ) );
		}

		public void PlaySound( int soundId )
		{
			if ( soundId == -1 )
				return;

			if ( m_Map != null )
			{
				Packet p = null;

				foreach ( NetState state in this.GetClientsInRange() )
				{
					if ( state.Mobile.CanSee( this ) )
					{
						if ( p == null )
							p = Packet.Acquire( GenericPackets.PlaySound( soundId, this ) );

						state.Send( p );
					}
				}

				Packet.Release( p );
			}
		}

		[CommandProperty( AccessLevel.Counselor )]
		public Skills Skills
		{
			get
			{
				return m_Skills;
			}
			set
			{
			}
		}

		[CommandProperty( AccessLevel.Counselor, AccessLevel.Owner )]
		public AccessLevel AccessLevel
		{
			get
			{
				return m_AccessLevel;
			}
			set
			{
				AccessLevel oldValue = m_AccessLevel;

				if ( oldValue != value )
				{
					m_AccessLevel = value;
					Delta( MobileDelta.Noto );
					InvalidateProperties();

					SendMessage( "Your access level has been changed. You are now {0}.", GetAccessLevelName( value ) );

					this.ClearScreen();
					this.SendEverything();

					OnAccessLevelChanged( oldValue );
				}
			}
		}

		public virtual void OnAccessLevelChanged( AccessLevel oldLevel )
		{
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int Fame
		{
			get
			{
				return m_Fame;
			}
			set
			{
				int oldValue = m_Fame;

				if ( oldValue != value )
				{
					m_Fame = value;

					if ( ShowFameTitle && ( m_Player || m_Body.IsHuman ) && ( oldValue >= 10000 ) != ( value >= 10000 ) )
						InvalidateProperties();

					OnFameChange( oldValue );
				}
			}
		}

		public virtual void OnFameChange( int oldValue )
		{
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int Karma
		{
			get
			{
				return m_Karma;
			}
			set
			{
				int old = m_Karma;

				if ( old != value )
				{
					m_Karma = value;
					OnKarmaChange( old );
				}
			}
		}

		public virtual void OnKarmaChange( int oldValue )
		{
		}

		/// <summary>
		/// Mobile did something which should unhide him.
		/// </summary>
		public virtual void RevealingAction()
		{
			if ( m_Hidden && m_AccessLevel == AccessLevel.Player )
				Hidden = false;

			DisruptiveAction(); // Anything that unhides you will also distrupt meditation
		}

		#region Say/SayTo/Emote/Whisper/Yell
		public void SayTo( Mobile to, bool ascii, string text )
		{
			PrivateOverheadMessage( MessageType.Regular, m_SpeechHue, ascii, text, to.NetState );
		}

		public void SayTo( Mobile to, string text )
		{
			SayTo( to, false, text );
		}

		public void SayTo( Mobile to, string format, params object[] args )
		{
			SayTo( to, false, String.Format( format, args ) );
		}

		public void SayTo( Mobile to, bool ascii, string format, params object[] args )
		{
			SayTo( to, ascii, String.Format( format, args ) );
		}

		public void SayTo( Mobile to, int number )
		{
			to.Send( new MessageLocalized( this.Serial, Body, MessageType.Regular, m_SpeechHue, 3, number, Name, "" ) );
		}

		public void SayTo( Mobile to, int number, string args )
		{
			to.Send( new MessageLocalized( this.Serial, Body, MessageType.Regular, m_SpeechHue, 3, number, Name, args ) );
		}

		public void Say( bool ascii, string text )
		{
			PublicOverheadMessage( MessageType.Regular, m_SpeechHue, ascii, text );
		}

		public void Say( string text )
		{
			PublicOverheadMessage( MessageType.Regular, m_SpeechHue, false, text );
		}

		public void Say( string format, params object[] args )
		{
			Say( String.Format( format, args ) );
		}

		public void Say( int number, AffixType type, string affix, string args )
		{
			PublicOverheadMessage( MessageType.Regular, m_SpeechHue, number, type, affix, args );
		}

		public void Say( int number )
		{
			Say( number, "" );
		}

		public void Say( int number, string args )
		{
			PublicOverheadMessage( MessageType.Regular, m_SpeechHue, number, args );
		}

		public void Emote( string text )
		{
			PublicOverheadMessage( MessageType.Emote, m_EmoteHue, false, text );
		}

		public void Emote( string format, params object[] args )
		{
			Emote( String.Format( format, args ) );
		}

		public void Emote( int number )
		{
			Emote( number, "" );
		}

		public void Emote( int number, string args )
		{
			PublicOverheadMessage( MessageType.Emote, m_EmoteHue, number, args );
		}

		public void Whisper( string text )
		{
			PublicOverheadMessage( MessageType.Whisper, m_WhisperHue, false, text );
		}

		public void Whisper( string format, params object[] args )
		{
			Whisper( String.Format( format, args ) );
		}

		public void Whisper( int number )
		{
			Whisper( number, "" );
		}

		public void Whisper( int number, string args )
		{
			PublicOverheadMessage( MessageType.Whisper, m_WhisperHue, number, args );
		}

		public void Yell( string text )
		{
			PublicOverheadMessage( MessageType.Yell, m_YellHue, false, text );
		}

		public void Yell( string format, params object[] args )
		{
			Yell( String.Format( format, args ) );
		}

		public void Yell( int number )
		{
			Yell( number, "" );
		}

		public void Yell( int number, string args )
		{
			PublicOverheadMessage( MessageType.Yell, m_YellHue, number, args );
		}
		#endregion

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Blessed
		{
			get
			{
				return m_Blessed;
			}
			set
			{
				if ( m_Blessed != value )
				{
					m_Blessed = value;
					Delta( MobileDelta.Flags );
					UpdateHealthBar( HealthBarColor.Yellow, m_Blessed );
				}
			}
		}

		public bool Send( Packet p, bool throwOnOffline = false )
		{
			if ( m_NetState == null )
			{
				if ( throwOnOffline )
					throw new MobileNotConnectedException( this, "Packet could not be sent." );

				return false;
			}

			m_NetState.Send( p );
			return true;
		}

		public bool SendHuePicker( HuePicker p, bool throwOnOffline = false )
		{
			if ( m_NetState == null )
			{
				if ( throwOnOffline )
					throw new MobileNotConnectedException( this, "Hue picker could not be sent." );

				return false;
			}

			p.SendTo( m_NetState );
			return true;
		}

		public Gump FindGump( Type type )
		{
			if ( m_NetState == null )
				return null;

			return m_NetState.Gumps.FirstOrDefault( gump => type.IsAssignableFrom( gump.GetType() ) );
		}

		public bool CloseGump( Type type, int buttonId = 0 )
		{
			if ( m_NetState == null )
				return false;

			Gump gump = FindGump( type );

			if ( gump != null )
			{
				m_NetState.Send( new CloseGump( gump.TypeID, buttonId ) );
				m_NetState.RemoveGump( gump );

				gump.OnServerClose( m_NetState );
			}

			return true;
		}

		public bool CloseAllGumps()
		{
			if ( m_NetState == null )
				return false;

			foreach ( Gump gump in m_NetState.Gumps )
			{
				m_NetState.Send( new CloseGump( gump.TypeID, 0 ) );

				gump.OnServerClose( m_NetState );
			}

			m_NetState.ClearGumps();

			return true;
		}

		public bool HasGump( Type type )
		{
			return ( FindGump( type ) != null );
		}

		public bool SendGump( Gump g, bool throwOnOffline = false )
		{
			if ( m_NetState == null )
			{
				if ( throwOnOffline )
					throw new MobileNotConnectedException( this, "Gump could not be sent." );

				return false;
			}

			g.SendTo( m_NetState );
			return true;
		}

		public bool SendMenu( IMenu m, bool throwOnOffline = false )
		{
			if ( m_NetState == null )
			{
				if ( throwOnOffline )
					throw new MobileNotConnectedException( this, "Menu could not be sent." );

				return false;
			}

			m.SendTo( m_NetState );
			return true;
		}

		/// <summary>
		/// Overridable. Event invoked before the Mobile says something.
		/// <seealso cref="DoSpeech" />
		/// </summary>
		public virtual void OnSaid( SpeechEventArgs e )
		{
			if ( m_Squelched )
			{
				this.SendLocalizedMessage( 500168 ); // You can not say anything, you have been muted.
				e.Blocked = true;
			}

			if ( !e.Blocked )
				RevealingAction();
		}

		public virtual bool HandlesOnSpeech( Mobile from )
		{
			return false;
		}

		/// <summary>
		/// Overridable. Virtual event invoked when the Mobile hears speech. This event will only be invoked if <see cref="HandlesOnSpeech" /> returns true.
		/// <seealso cref="DoSpeech" />
		/// </summary>
		public virtual void OnSpeech( SpeechEventArgs e )
		{
		}

		[CommandProperty( AccessLevel.Counselor )]
		public Serial Serial
		{
			get
			{
				return m_Serial;
			}
		}

		[CommandProperty( AccessLevel.Counselor, AccessLevel.GameMaster )]
		public Map Map
		{
			get
			{
				return m_Map;
			}
			set
			{
				if ( m_Deleted )
					return;

				if ( m_Map != value )
				{
					if ( m_NetState != null )
						m_NetState.ValidateAllTrades();

					Map oldMap = m_Map;

					if ( oldMap != null )
					{
						oldMap.OnLeave( this );

						this.ClearScreen();
						this.SendRemovePacket();
					}

					for ( int i = 0; i < m_EquippedItems.Count; ++i )
						m_EquippedItems[i].Map = value;

					m_Map = value;

					UpdateRegion();

					if ( m_Map != null )
						m_Map.OnEnter( this );

					NetState ns = m_NetState;

					if ( m_Map != null )
					{
						if ( ns != null && m_Map != null )
						{
							ns.Sequence = 0;
							ns.Send( new MapChange( this ) );
							ns.Send( SeasonChange.Instantiate( GetSeason(), true ) );
							ns.Send( new MobileUpdate( this ) );
							ClearFastwalkStack();
						}
					}

					if ( ns != null )
					{
						if ( m_Map != null )
							Send( new ServerChange( this, m_Map ) );

						ns.Sequence = 0;
						ClearFastwalkStack();

						Send( new MobileIncoming( this, this ) );
						Send( new MobileUpdate( this ) );
						CheckLightLevels( true );
						Send( new MobileUpdate( this ) );
					}

					this.SendEverything();
					this.SendIncomingPacket();

					if ( ns != null )
					{
						ns.Sequence = 0;
						ClearFastwalkStack();

						Send( new MobileIncoming( this, this ) );
						Send( SupportedFeatures.Instantiate( ns ) );
						Send( new MobileUpdate( this ) );
						Send( new MobileAttributes( this ) );
					}

					EventSink.InvokeMapChanged( new MapChangedEventArgs( this, oldMap ) );

					OnMapChange( oldMap );
				}
			}
		}

		public void UpdateRegion()
		{
			if ( m_Deleted )
				return;

			Region newRegion = Region.Find( m_Location, m_Map );

			if ( newRegion != m_Region )
			{
				Region.OnRegionChange( this, m_Region, newRegion );

				m_Region = newRegion;
				OnRegionChange( m_Region, newRegion );
			}
		}

		/// <summary>
		/// Overridable. Virtual event invoked when <see cref="Map" /> changes.
		/// </summary>
		protected virtual void OnMapChange( Map oldMap )
		{
		}

		public virtual bool CanBeBeneficial( Mobile target )
		{
			return CanBeBeneficial( target, true, false );
		}

		public virtual bool CanBeBeneficial( Mobile target, bool message )
		{
			return CanBeBeneficial( target, message, false );
		}

		public virtual bool CanBeBeneficial( Mobile target, bool message, bool allowDead )
		{
			if ( target == null )
				return false;

			if ( m_Deleted || target.m_Deleted || !Alive || IsDeadBondedPet || ( !allowDead && ( !target.Alive || IsDeadBondedPet ) ) )
			{
				if ( message )
					SendLocalizedMessage( 1001017 ); // You can not perform beneficial acts on your target.

				return false;
			}

			if ( target == this )
				return true;

			if ( /*m_Player &&*/ !Region.AllowBeneficial( this, target ) )
			{
				// TODO: Pets
				//if ( !(target.m_Player || target.Body.IsHuman || target.Body.IsAnimal) )
				//{
				if ( message )
					SendLocalizedMessage( 1001017 ); // You can not perform beneficial acts on your target.

				return false;
				//}
			}

			return true;
		}

		public virtual bool IsBeneficialCriminal( Mobile target )
		{
			if ( this == target )
				return false;

			int n = Notoriety.Compute( this, target );

			return ( n == Notoriety.Criminal || n == Notoriety.Murderer );
		}

		/// <summary>
		/// Overridable. Event invoked when the Mobile <see cref="DoBeneficial">does a beneficial action</see>.
		/// </summary>
		public virtual void OnBeneficialAction( Mobile target, bool isCriminal )
		{
			if ( isCriminal )
				CriminalAction( false );
		}

		public virtual void DoBeneficial( Mobile target )
		{
			if ( target == null )
				return;

			OnBeneficialAction( target, IsBeneficialCriminal( target ) );

			Region.OnBeneficialAction( this, target );
			target.Region.OnGotBeneficialAction( this, target );
		}

		public virtual bool BeneficialCheck( Mobile target )
		{
			if ( CanBeBeneficial( target, true ) )
			{
				DoBeneficial( target );
				return true;
			}

			return false;
		}

		public virtual bool CanBeHarmful( Mobile target )
		{
			return CanBeHarmful( target, true );
		}

		public virtual bool CanBeHarmful( Mobile target, bool message )
		{
			return CanBeHarmful( target, message, false );
		}

		public virtual bool CanBeHarmful( Mobile target, bool message, bool ignoreOurBlessedness )
		{
			if ( target == null )
				return false;

			if ( m_Deleted || ( !ignoreOurBlessedness && m_Blessed ) || target.m_Deleted || target.m_Blessed || !Alive || IsDeadBondedPet || !target.Alive || target.IsDeadBondedPet )
			{
				if ( message )
					SendLocalizedMessage( 1001018 ); // You can not perform negative acts on your target.

				return false;
			}

			if ( target == this )
				return true;

			// TODO: Pets
			if ( /*m_Player &&*/ !Region.AllowHarmful( this, target ) )//(target.m_Player || target.Body.IsHuman) && !Region.AllowHarmful( this, target )  )
			{
				if ( message )
					SendLocalizedMessage( 1001018 ); // You can not perform negative acts on your target.

				return false;
			}

			return true;
		}

		public virtual int Luck
		{
			get { return 0; }
		}

		public void AddAttributeMod( AttributeMod mod )
		{
			if ( m_AttributeMods == null )
				m_AttributeMods = new List<AttributeMod>( 1 );

			m_AttributeMods.Add( mod );

			InvalidateProperties();
		}

		public void RemoveAttributeMod( AttributeMod mod )
		{
			if ( mod == null )
				return;

			if ( m_AttributeMods != null )
			{
				m_AttributeMods.Remove( mod );

				if ( m_AttributeMods.Count == 0 )
					m_AttributeMods = null;
			}

			InvalidateProperties();
		}

		public virtual int GetMagicalAttribute( AosAttribute attr )
		{
			int value = AosAttributes.GetValue( this, attr );

			if ( m_AttributeMods != null )
			{
				for ( int i = 0; i < m_AttributeMods.Count; i++ )
				{
					AttributeMod mod = m_AttributeMods[i];

					if ( mod.Attribute == attr )
						value += mod.Offset;
				}
			}

			return value;
		}

		public virtual bool IsHarmfulCriminal( Mobile target )
		{
			if ( this == target )
				return false;

			return ( Notoriety.Compute( this, target ) == Notoriety.Innocent );
		}

		/// <summary>
		/// Overridable. Event invoked when the Mobile <see cref="DoHarmful">does a harmful action</see>.
		/// </summary>
		public virtual void OnHarmfulAction( Mobile target, bool isCriminal )
		{
			if ( isCriminal )
				CriminalAction( false );
		}

		public virtual void DoHarmful( Mobile target )
		{
			DoHarmful( target, false );
		}

		public virtual void DoHarmful( Mobile target, bool indirect )
		{
			if ( target == null )
				return;

			bool isCriminal = IsHarmfulCriminal( target );

			OnHarmfulAction( target, isCriminal );
			target.AggressiveAction( this, isCriminal );

			Region.OnDidHarmful( this, target );
			target.Region.OnGotHarmful( this, target );

			if ( !indirect )
				Combatant = target;

			#region Combat
			if ( m_CombatContext == null )
				m_CombatContext = new CombatContext( this );

			m_CombatContext.RefreshExpireCombatant();
			#endregion

			HarmfulActionEventArgs args = HarmfulActionEventArgs.Create( this, target, isCriminal );

			EventSink.InvokeHarmfulAction( args );

			args.Free();
		}

		public virtual bool HarmfulCheck( Mobile target )
		{
			if ( CanBeHarmful( target ) )
			{
				DoHarmful( target );
				return true;
			}

			return false;
		}

		#region Stats

		/// <summary>
		/// Gets a list of all <see cref="StatMod">StatMod's</see> currently active for the Mobile.
		/// </summary>
		public List<StatMod> StatMods { get { return m_StatMods; } }

		public bool RemoveStatMod( string name )
		{
			for ( int i = 0; i < m_StatMods.Count; ++i )
			{
				StatMod check = m_StatMods[i];

				if ( check.Name == name )
				{
					m_StatMods.RemoveAt( i );
					CheckStatTimers();
					Delta( MobileDelta.Stat | GetStatDelta( check.Type ) );

					return true;
				}
			}

			return false;
		}

		public StatMod GetStatMod( string name )
		{
			for ( int i = 0; i < m_StatMods.Count; ++i )
			{
				StatMod check = m_StatMods[i];

				if ( check.Name == name )
					return check;
			}

			return null;
		}

		public void AddStatMod( StatMod mod )
		{
			for ( int i = 0; i < m_StatMods.Count; ++i )
			{
				StatMod check = m_StatMods[i];

				if ( check.Name == mod.Name )
				{
					Delta( MobileDelta.Stat | GetStatDelta( check.Type ) );
					m_StatMods.RemoveAt( i );
					break;
				}
			}

			m_StatMods.Add( mod );
			Delta( MobileDelta.Stat | GetStatDelta( mod.Type ) );
			CheckStatTimers();
		}

		private MobileDelta GetStatDelta( StatType type )
		{
			MobileDelta delta = 0;

			if ( ( type & StatType.Str ) != 0 )
				delta |= MobileDelta.Hits;

			if ( ( type & StatType.Dex ) != 0 )
				delta |= MobileDelta.Stam;

			if ( ( type & StatType.Int ) != 0 )
				delta |= MobileDelta.Mana;

			return delta;
		}

		/// <summary>
		/// Computes the total modified offset for the specified stat type. Expired <see cref="StatMod" /> instances are removed.
		/// </summary>
		public int GetStatOffset( StatType type )
		{
			int offset = 0;

			for ( int i = 0; i < m_StatMods.Count; ++i )
			{
				StatMod mod = m_StatMods[i];

				if ( mod.HasElapsed() )
				{
					m_StatMods.RemoveAt( i );

					Delta( MobileDelta.Stat | GetStatDelta( mod.Type ) );
					CheckStatTimers();

					--i;
				}
				else if ( ( mod.Type & type ) != 0 )
				{
					offset += mod.Offset;
				}
			}

			return offset;
		}

		/// <summary>
		/// Overridable. Virtual event invoked when the <see cref="RawStr" /> changes.
		/// <seealso cref="RawStr" />
		/// <seealso cref="OnRawStatChange" />
		/// </summary>
		public virtual void OnRawStrChange( int oldValue )
		{
		}

		/// <summary>
		/// Overridable. Virtual event invoked when <see cref="RawDex" /> changes.
		/// <seealso cref="RawDex" />
		/// <seealso cref="OnRawStatChange" />
		/// </summary>
		public virtual void OnRawDexChange( int oldValue )
		{
		}

		/// <summary>
		/// Overridable. Virtual event invoked when the <see cref="RawInt" /> changes.
		/// <seealso cref="RawInt" />
		/// <seealso cref="OnRawStatChange" />
		/// </summary>
		public virtual void OnRawIntChange( int oldValue )
		{
		}

		/// <summary>
		/// Overridable. Virtual event invoked when the <see cref="RawStr" />, <see cref="RawDex" />, or <see cref="RawInt" /> changes.
		/// <seealso cref="OnRawStrChange" />
		/// <seealso cref="OnRawDexChange" />
		/// <seealso cref="OnRawIntChange" />
		/// </summary>
		public virtual void OnRawStatChange( StatType stat, int oldValue )
		{
		}

		/// <summary>
		/// Gets or sets the base, unmodified, strength of the Mobile. Ranges from 1 to 65000, inclusive.
		/// <seealso cref="Str" />
		/// <seealso cref="StatMod" />
		/// <seealso cref="OnRawStrChange" />
		/// <seealso cref="OnRawStatChange" />
		/// </summary>
		[CommandProperty( AccessLevel.GameMaster )]
		public int RawStr
		{
			get
			{
				return m_Str;
			}
			set
			{
				Utility.FixMinMax( ref value, 1, 65000 );

				if ( m_Str != value )
				{
					int oldValue = m_Str;

					m_Str = value;
					Delta( MobileDelta.Stat | MobileDelta.Hits );

					if ( Hits < HitsMax )
					{
						if ( m_HitsTimer == null )
							m_HitsTimer = new HitsTimer( this );

						m_HitsTimer.Start();
					}
					else if ( Hits > HitsMax )
					{
						Hits = HitsMax;
					}

					OnRawStrChange( oldValue );
					OnRawStatChange( StatType.Str, oldValue );
				}
			}
		}

		/// <summary>
		/// Gets or sets the effective strength of the Mobile. This is the sum of the <see cref="RawStr" /> plus any additional modifiers. Any attempts to set this value when under the influence of a <see cref="StatMod" /> will result in no change. It ranges from 1 to 65000, inclusive.
		/// <seealso cref="RawStr" />
		/// <seealso cref="StatMod" />
		/// </summary>
		[CommandProperty( AccessLevel.GameMaster )]
		public virtual int Str
		{
			get
			{
				int value = m_Str + GetStatOffset( StatType.Str );

				Utility.FixMinMax( ref value, 1, 65000 );

				return value;
			}
			set
			{
				if ( m_StatMods.Count == 0 )
					RawStr = value;
			}
		}

		/// <summary>
		/// Gets or sets the base, unmodified, dexterity of the Mobile. Ranges from 1 to 65000, inclusive.
		/// <seealso cref="Dex" />
		/// <seealso cref="StatMod" />
		/// <seealso cref="OnRawDexChange" />
		/// <seealso cref="OnRawStatChange" />
		/// </summary>
		[CommandProperty( AccessLevel.GameMaster )]
		public int RawDex
		{
			get
			{
				return m_Dex;
			}
			set
			{
				Utility.FixMinMax( ref value, 1, 65000 );

				if ( m_Dex != value )
				{
					int oldValue = m_Dex;

					m_Dex = value;
					Delta( MobileDelta.Stat | MobileDelta.Stam );

					if ( Stam < StamMax )
					{
						if ( m_StamTimer == null )
							m_StamTimer = new StamTimer( this );

						m_StamTimer.Start();
					}
					else if ( Stam > StamMax )
					{
						Stam = StamMax;
					}

					OnRawDexChange( oldValue );
					OnRawStatChange( StatType.Dex, oldValue );
				}
			}
		}

		/// <summary>
		/// Gets or sets the effective dexterity of the Mobile. This is the sum of the <see cref="RawDex" /> plus any additional modifiers. Any attempts to set this value when under the influence of a <see cref="StatMod" /> will result in no change. It ranges from 1 to 65000, inclusive.
		/// <seealso cref="RawDex" />
		/// <seealso cref="StatMod" />
		/// </summary>
		[CommandProperty( AccessLevel.GameMaster )]
		public virtual int Dex
		{
			get
			{
				int value = m_Dex + GetStatOffset( StatType.Dex );

				Utility.FixMinMax( ref value, 1, 65000 );

				return value;
			}
			set
			{
				if ( m_StatMods.Count == 0 )
					RawDex = value;
			}
		}

		/// <summary>
		/// Gets or sets the base, unmodified, intelligence of the Mobile. Ranges from 1 to 65000, inclusive.
		/// <seealso cref="Int" />
		/// <seealso cref="StatMod" />
		/// <seealso cref="OnRawIntChange" />
		/// <seealso cref="OnRawStatChange" />
		/// </summary>
		[CommandProperty( AccessLevel.GameMaster )]
		public int RawInt
		{
			get
			{
				return m_Int;
			}
			set
			{
				Utility.FixMinMax( ref value, 1, 65000 );

				if ( m_Int != value )
				{
					int oldValue = m_Int;

					m_Int = value;
					Delta( MobileDelta.Stat | MobileDelta.Mana );

					if ( Mana < ManaMax )
					{
						if ( m_ManaTimer == null )
							m_ManaTimer = new ManaTimer( this );

						m_ManaTimer.Start();
					}
					else if ( Mana > ManaMax )
					{
						Mana = ManaMax;
					}

					OnRawIntChange( oldValue );
					OnRawStatChange( StatType.Int, oldValue );
				}
			}
		}

		/// <summary>
		/// Gets or sets the effective intelligence of the Mobile. This is the sum of the <see cref="RawInt" /> plus any additional modifiers. Any attempts to set this value when under the influence of a <see cref="StatMod" /> will result in no change. It ranges from 1 to 65000, inclusive.
		/// <seealso cref="RawInt" />
		/// <seealso cref="StatMod" />
		/// </summary>
		[CommandProperty( AccessLevel.GameMaster )]
		public virtual int Int
		{
			get
			{
				int value = m_Int + GetStatOffset( StatType.Int );

				Utility.FixMinMax( ref value, 1, 65000 );

				return value;
			}
			set
			{
				if ( m_StatMods.Count == 0 )
					RawInt = value;
			}
		}

		/// <summary>
		/// Gets or sets the current hit point of the Mobile. This value ranges from 0 to <see cref="HitsMax" />, inclusive. When set to the value of <see cref="HitsMax" />, the <see cref="AggressorInfo.CanReportMurder">CanReportMurder</see> flag of all aggressors is reset to false, and the list of damage entries is cleared.
		/// </summary>
		[CommandProperty( AccessLevel.GameMaster )]
		public int Hits
		{
			get
			{
				return m_Hits;
			}
			set
			{
				if ( m_Deleted )
					return;

				if ( value < 0 )
				{
					value = 0;
				}
				else if ( value >= HitsMax )
				{
					value = HitsMax;

					if ( m_HitsTimer != null )
						m_HitsTimer.Stop();

					for ( int i = 0; i < m_Aggressors.Count; i++ ) // Reset reports on full HP.
						m_Aggressors[i].CanReportMurder = false;

					if ( m_DamageEntries.Count > 0 )
						m_DamageEntries.Clear(); // Reset damage entries on full HP.
				}

				if ( value < HitsMax )
				{
					if ( CanRegenHits )
					{
						if ( m_HitsTimer == null )
							m_HitsTimer = new HitsTimer( this );

						m_HitsTimer.Start();
					}
					else if ( m_HitsTimer != null )
					{
						m_HitsTimer.Stop();
					}
				}

				if ( m_Hits != value )
				{
					m_Hits = value;
					Delta( MobileDelta.Hits );
				}
			}
		}

		/// <summary>
		/// Overridable. Gets the maximum hit point of the Mobile. By default, this returns: <c>50 + (<see cref="Str" /> / 2)</c>
		/// </summary>
		[CommandProperty( AccessLevel.GameMaster )]
		public virtual int HitsMax
		{
			get
			{
				return 50 + ( Str / 2 );
			}
		}

		/// <summary>
		/// Gets or sets the current stamina of the Mobile. This value ranges from 0 to <see cref="StamMax" />, inclusive.
		/// </summary>
		[CommandProperty( AccessLevel.GameMaster )]
		public int Stam
		{
			get
			{
				return m_Stam;
			}
			set
			{
				if ( m_Deleted )
					return;

				if ( value < 0 )
				{
					value = 0;
				}
				else if ( value >= StamMax )
				{
					value = StamMax;

					if ( m_StamTimer != null )
						m_StamTimer.Stop();
				}

				if ( value < StamMax )
				{
					if ( CanRegenStam )
					{
						if ( m_StamTimer == null )
							m_StamTimer = new StamTimer( this );

						m_StamTimer.Start();
					}
					else if ( m_StamTimer != null )
					{
						m_StamTimer.Stop();
					}
				}

				if ( m_Stam != value )
				{
					m_Stam = value;
					Delta( MobileDelta.Stam );
				}
			}
		}

		/// <summary>
		/// Overridable. Gets the maximum stamina of the Mobile. By default, this returns: <c><see cref="Dex" /></c>
		/// </summary>
		[CommandProperty( AccessLevel.GameMaster )]
		public virtual int StamMax
		{
			get
			{
				return Dex;
			}
		}

		/// <summary>
		/// Gets or sets the current mana of the Mobile. This value ranges from 0 to <see cref="ManaMax" />, inclusive.
		/// </summary>
		[CommandProperty( AccessLevel.GameMaster )]
		public int Mana
		{
			get
			{
				return m_Mana;
			}
			set
			{
				if ( m_Deleted )
					return;

				if ( value < 0 )
				{
					value = 0;
				}
				else if ( value >= ManaMax )
				{
					value = ManaMax;

					if ( m_ManaTimer != null )
						m_ManaTimer.Stop();

					if ( Meditating )
					{
						Meditating = false;
						SendLocalizedMessage( 501846 ); // You are at peace.

						OnFinishMeditation();
					}
				}

				if ( value < ManaMax )
				{
					if ( CanRegenMana )
					{
						if ( m_ManaTimer == null )
							m_ManaTimer = new ManaTimer( this );

						m_ManaTimer.Start();
					}
					else if ( m_ManaTimer != null )
					{
						m_ManaTimer.Stop();
					}
				}

				if ( m_Mana != value )
				{
					m_Mana = value;
					Delta( MobileDelta.Mana );
				}
			}
		}

		/// <summary>
		/// Overridable. Gets the maximum mana of the Mobile. By default, this returns: <c><see cref="Int" /></c>
		/// </summary>
		[CommandProperty( AccessLevel.GameMaster )]
		public virtual int ManaMax
		{
			get
			{
				return Int;
			}
		}

		#endregion

		/// <summary>
		/// Overridable. Called when a mobile finish meditation.
		/// </summary>
		public virtual void OnFinishMeditation()
		{
		}

		public virtual int HuedItemID
		{
			get
			{
				return ( m_Female ? 0x2107 : 0x2106 );
			}
		}

		private int m_HueMod = -1;

		[Hue, CommandProperty( AccessLevel.GameMaster )]
		public int HueMod
		{
			get
			{
				return m_HueMod;
			}
			set
			{
				if ( m_HueMod != value )
				{
					m_HueMod = value;

					Delta( MobileDelta.Hue );
				}
			}
		}

		[Hue, CommandProperty( AccessLevel.GameMaster )]
		public virtual int Hue
		{
			get
			{
				if ( m_HueMod != -1 )
					return m_HueMod;

				return m_Hue;
			}
			set
			{
				int oldHue = m_Hue;

				if ( oldHue != value )
				{
					m_Hue = value;

					Delta( MobileDelta.Hue );
				}
			}
		}


		public void SetDirection( Direction dir )
		{
			m_Direction = dir;
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Direction Direction
		{
			get
			{
				return m_Direction;
			}
			set
			{
				if ( m_Direction != value )
				{
					m_Direction = value;

					Delta( MobileDelta.Direction );
					//ProcessDelta();
				}
			}
		}

		public virtual int GetSeason()
		{
			if ( m_Map != null )
				return m_Map.Season;

			return 1;
		}

		public virtual bool Flying
		{
			get { return false; }
			set { }
		}

		public virtual int GetPacketFlags()
		{
			int flags = 0x0;

			if ( m_Frozen || m_Paralyzed )
				flags |= 0x01;

			if ( m_Female )
				flags |= 0x02;

			if ( m_Blessed )
				flags |= 0x08;

			if ( HasFreeMovement() )
				flags |= 0x10;

			if ( m_Warmode )
				flags |= 0x40;

			if ( m_Hidden )
				flags |= 0x80;

			return flags;
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Female
		{
			get
			{
				return m_Female;
			}
			set
			{
				if ( m_Female != value )
				{
					m_Female = value;
					Delta( MobileDelta.Flags );
					OnGenderChanged( !m_Female );
				}
			}
		}

		public virtual void OnGenderChanged( bool oldFemale )
		{
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Warmode
		{
			get
			{
				return m_Warmode;
			}
			set
			{
				if ( m_Deleted )
					return;

				if ( m_Warmode != value )
				{
					if ( m_AutoManifestTimer != null )
					{
						m_AutoManifestTimer.Stop();
						m_AutoManifestTimer = null;
					}

					m_Warmode = value;
					Delta( MobileDelta.Flags );

					if ( m_NetState != null )
						Send( SetWarMode.Instantiate( value ) );

					if ( !m_Warmode )
						Combatant = null;

					if ( !Alive )
					{
						if ( value )
							Delta( MobileDelta.GhostUpdate );
						else
							this.SendRemovePacket( false );
					}

					OnWarmodeChanged();
				}
			}
		}

		public virtual void OnWarmodeChanged()
		{
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public virtual bool Hidden
		{
			get
			{
				return m_Hidden;
			}
			set
			{
				if ( m_Hidden != value )
				{
					m_IsStealthing = false;
					m_AllowedStealthSteps = 0;

					m_Hidden = value;
					//Delta( MobileDelta.Flags );

					if ( m_Map != null )
					{
						Packet p = null;

						foreach ( NetState state in this.GetClientsInRange() )
						{
							if ( !state.Mobile.CanSee( this ) )
							{
								if ( p == null )
									p = this.RemovePacket;

								state.Send( p );
							}
							else
							{
								state.Send( new MobileIncoming( state.Mobile, this ) );

								if ( IsDeadBondedPet )
									state.Send( new BondedStatus( 0, this.Serial, 1 ) );

								if ( ObjectPropertyListPacket.Enabled )
									state.Send( OPLPacket );
							}
						}
					}
				}
			}
		}

		public virtual void OnConnected()
		{
		}

		public virtual void OnDisconnected()
		{
		}

		public virtual void OnClientChanged()
		{
		}

		public NetState NetState
		{
			get
			{
				if ( m_NetState != null && !m_NetState.Running )
					NetState = null;

				return m_NetState;
			}
			set
			{
				if ( m_NetState != value )
				{
					if ( m_Map != null )
						m_Map.OnClientChange( m_NetState, value, this );

					if ( m_Target != null )
						m_Target.Cancel( this, TargetCancelType.Disconnected );

					if ( m_QuestArrow != null )
						QuestArrow = null;

					if ( m_Spell != null )
						m_Spell.OnConnectionChanged();

					//if ( m_Spell != null )
					//	m_Spell.FinishSequence();

					if ( m_NetState != null )
						m_NetState.CancelAllTrades();

					BankBox box = FindBankNoCreate();

					if ( box != null && box.Opened )
						box.Close();

					m_NetState = value;

					if ( m_NetState == null )
					{
						OnDisconnected();
						EventSink.InvokeDisconnected( new DisconnectedEventArgs( this ) );

						// Disconnected, start the logout timer

						if ( m_LogoutTimer == null )
							m_LogoutTimer = new LogoutTimer( this );
						else
							m_LogoutTimer.Stop();

						m_LogoutTimer.Delay = GetLogoutDelay();
						m_LogoutTimer.Start();
					}
					else
					{
						OnConnected();
						EventSink.InvokeConnected( new ConnectedEventArgs( this ) );

						// Connected, stop the logout timer and if needed, move to the world

						if ( m_LogoutTimer != null )
							m_LogoutTimer.Stop();

						m_LogoutTimer = null;

						if ( m_Map == Map.Internal && m_LogoutMap != null )
						{
							Map = m_LogoutMap;
							Location = m_LogoutLocation;
						}
					}

					foreach ( var item in this.GetEquippedItems().OfType<SecureTradeContainer>() )
					{
						for ( int j = item.Items.Count - 1; j >= 0; --j )
						{
							if ( j < item.Items.Count )
							{
								( (Item) item.Items[j] ).OnSecureTrade( this, this, this, false );
								this.AddToBackpack( (Item) item.Items[j] );
							}
						}

						item.Delete();
					}

					this.DropHolding();
					OnClientChanged();
				}
			}
		}

		public virtual bool CanSee( object o )
		{
			if ( o is Item )
			{
				return CanSee( (Item) o );
			}
			else if ( o is Mobile )
			{
				return CanSee( (Mobile) o );
			}
			else
			{
				return true;
			}
		}

		public virtual bool CanSee( Item item )
		{
			if ( m_Map == Map.Internal )
				return false;
			else if ( item.Map == Map.Internal )
				return false;

			#region Instances
			if ( m_AccessLevel < AccessLevel.GameMaster )
			{
				if ( item.InstanceID != 0 && m_InstanceID != item.InstanceID )
					return false;
			}
			#endregion

			if ( item.Parent != null )
			{
				if ( item.Parent is Item )
				{
					Item parent = item.Parent as Item;

					if ( !( CanSee( parent ) && parent.IsChildVisibleTo( this, item ) ) )
						return false;
				}
				else if ( item.Parent is Mobile )
				{
					if ( !CanSee( (Mobile) item.Parent ) )
						return false;
				}
			}

			if ( item is BankBox )
			{
				BankBox box = item as BankBox;

				if ( box != null && m_AccessLevel <= AccessLevel.Counselor && ( box.Owner != this || !box.Opened ) )
					return false;
			}
			else if ( item is SecureTradeContainer )
			{
				SecureTrade trade = ( (SecureTradeContainer) item ).Trade;

				if ( trade != null && trade.From.Mobile != this && trade.To.Mobile != this )
					return false;
			}

			// Items dentro de una casa no se pueden ver si no se est� dentro de ella.

			/*Multis.BaseHouse house = Multis.HousingHelper.FindHouseAt( item );

			if ( house != null &&
				house != Multis.HousingHelper.FindHouseAt( this ) &&
				item.Parent == null &&
				!( item is Multis.BaseHouse ) &&
				!( item is BaseHouseDoor ) &&
				!( item is Static ) &&
				!( item is HouseTeleporter ) )
			{
				return false;
			}*/

			return !item.Deleted && item.Map == m_Map && ( item.Visible || m_AccessLevel > AccessLevel.Counselor );
		}

		public virtual bool CanSee( Mobile m )
		{
			if ( m_Deleted || m.m_Deleted || m_Map == Map.Internal || m.m_Map == Map.Internal )
				return false;

			#region Instances
			if ( m_AccessLevel < AccessLevel.GameMaster )
			{
				if ( m.InstanceID != 0 && m_InstanceID != m.InstanceID )
					return false;
			}
			#endregion

			return this == m || (
				m.m_Map == m_Map &&
				( !m.Hidden || m_AccessLevel > m.AccessLevel ) &&
				( m.Alive || !Alive || m_AccessLevel > AccessLevel.Player || m.Warmode ) );
		}

		public virtual bool CanBeRenamedBy( Mobile from )
		{
			/* Author: wyatt
			 * E-mail: wyatter@gmail.com
			 * Stuff with accesslevel lower than GameMaster
			 * can't renamed players by dragging their healthbar
			 */
			if ( from.AccessLevel >= AccessLevel.GameMaster )
			{
				return ( from.m_AccessLevel > m_AccessLevel );
			}

			return false;
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public string Language
		{
			get
			{
				return m_Language;
			}
			set
			{
				m_Language = value;
				if ( m_Language != null )
					m_Language = string.Intern( m_Language );
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int SpeechHue
		{
			get
			{
				return m_SpeechHue;
			}
			set
			{
				m_SpeechHue = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int EmoteHue
		{
			get
			{
				return m_EmoteHue;
			}
			set
			{
				m_EmoteHue = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int WhisperHue
		{
			get
			{
				return m_WhisperHue;
			}
			set
			{
				m_WhisperHue = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int YellHue
		{
			get
			{
				return m_YellHue;
			}
			set
			{
				m_YellHue = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public string GuildTitle
		{
			get
			{
				return m_GuildTitle;
			}
			set
			{
				string old = m_GuildTitle;

				if ( old != value )
				{
					m_GuildTitle = value;

					if ( m_Guild != null && !m_Guild.Disbanded && m_GuildTitle != null )
						this.SendLocalizedMessage( 1018026, true, m_GuildTitle ); // Your guild title has changed :

					InvalidateProperties();

					OnGuildTitleChange( old );
				}
			}
		}

		public virtual void OnGuildTitleChange( string oldTitle )
		{
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool DisplayGuildTitle
		{
			get
			{
				return m_DisplayGuildTitle;
			}
			set
			{
				m_DisplayGuildTitle = value;
				InvalidateProperties();
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile GuildFealty
		{
			get
			{
				return m_GuildFealty;
			}
			set
			{
				m_GuildFealty = value;
			}
		}

		private string m_NameMod;

		[CommandProperty( AccessLevel.GameMaster )]
		public string NameMod
		{
			get
			{
				return m_NameMod;
			}
			set
			{
				if ( m_NameMod != value )
				{
					m_NameMod = value;
					Delta( MobileDelta.Name );
					InvalidateProperties();
				}
			}
		}

		private bool m_YellowHealthbar;

		[CommandProperty( AccessLevel.GameMaster )]
		public bool YellowHealthbar
		{
			get
			{
				return m_YellowHealthbar;
			}
			set
			{
				m_YellowHealthbar = value;
				UpdateHealthBar( HealthBarColor.Yellow, m_YellowHealthbar );
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public string RawName
		{
			get { return m_Name; }
			set { Name = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public string Name
		{
			get
			{
				if ( m_NameMod != null )
					return m_NameMod;

				return m_Name;
			}
			set
			{
				if ( m_Name != value ) // I'm leaving out the && m_NameMod == null
				{
					m_Name = value;
					if ( m_Name != null )
						m_Name = string.Intern( m_Name );
					Delta( MobileDelta.Name );
					InvalidateProperties();
				}
			}
		}

		public BaseGuild Guild
		{
			get
			{
				return m_Guild;
			}
			set
			{
				BaseGuild old = m_Guild;

				if ( old != value )
				{
					if ( value == null )
						GuildTitle = null;

					m_Guild = value;

					Delta( MobileDelta.Noto );
					InvalidateProperties();

					OnGuildChange( old );
				}
			}
		}

		public virtual void OnGuildChange( BaseGuild oldGuild )
		{
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Poison Poison
		{
			get
			{
				return m_Poison;
			}
			set
			{
				m_Poison = value;
				UpdateHealthBar( HealthBarColor.Green, m_Poison != null );

				if ( m_PoisonTimer != null )
				{
					m_PoisonTimer.Stop();
					m_PoisonTimer = null;
				}

				if ( m_Poison != null )
				{
					m_PoisonTimer = m_Poison.ConstructTimer( this );

					if ( m_PoisonTimer != null )
						m_PoisonTimer.Start();
				}
				else
				{
					ParasiticPoison.RemoveInfo( this );
					DarkglowPoison.RemoveInfo( this );
				}

				CheckStatTimers();
			}
		}

		/// <summary>
		/// Overridable. Event invoked when a call to <see cref="ApplyPoison" /> failed because <see cref="CheckPoisonImmunity" /> returned false: the Mobile was resistant to the poison. By default, this broadcasts an overhead message: * The poison seems to have no effect. *
		/// <seealso cref="CheckPoisonImmunity" />
		/// <seealso cref="ApplyPoison" />
		/// <seealso cref="Poison" />
		/// </summary>
		public virtual void OnPoisonImmunity( Mobile from, Poison poison )
		{
			this.PublicOverheadMessage( MessageType.Emote, 0x3B2, 1005534 ); // * The poison seems to have no effect. *
		}

		/// <summary>
		/// Overridable. Virtual event invoked when a call to <see cref="ApplyPoison" /> failed because <see cref="CheckHigherPoison" /> returned false: the Mobile was already poisoned by an equal or greater strength poison.
		/// <seealso cref="CheckHigherPoison" />
		/// <seealso cref="ApplyPoison" />
		/// <seealso cref="Poison" />
		/// </summary>
		public virtual void OnHigherPoison( Mobile from, Poison poison )
		{
		}

		/// <summary>
		/// Overridable. Event invoked when a call to <see cref="ApplyPoison" /> succeeded. By default, this broadcasts an overhead message varying by the level of the poison. Example: * Zippy begins to spasm uncontrollably. *
		/// <seealso cref="ApplyPoison" />
		/// <seealso cref="Poison" />
		/// </summary>
		public virtual void OnPoisoned( Mobile from, Poison poison, Poison oldPoison )
		{
			if ( poison != null )
			{
				this.LocalOverheadMessage( MessageType.Regular, 0x22, 1042857 + ( poison.Level * 2 ) );
				this.NonlocalOverheadMessage( MessageType.Regular, 0x22, 1042858 + ( poison.Level * 2 ), Name );
			}
		}

		/// <summary>
		/// Overridable. Called from <see cref="ApplyPoison" />, this method checks if the Mobile is immune to some <see cref="Poison" />. If true, <see cref="OnPoisonImmunity" /> will be invoked and <see cref="ApplyPoisonResult.Immune" /> is returned.
		/// <seealso cref="OnPoisonImmunity" />
		/// <seealso cref="ApplyPoison" />
		/// <seealso cref="Poison" />
		/// </summary>
		public virtual bool CheckPoisonImmunity( Mobile from, Poison poison )
		{
			return false;
		}

		/// <summary>
		/// Overridable. Called from <see cref="ApplyPoison" />, this method checks if the Mobile is already poisoned by some <see cref="Poison" /> of equal or greater strength. If true, <see cref="OnHigherPoison" /> will be invoked and <see cref="ApplyPoisonResult.HigherPoisonActive" /> is returned.
		/// <seealso cref="OnHigherPoison" />
		/// <seealso cref="ApplyPoison" />
		/// <seealso cref="Poison" />
		/// </summary>
		public virtual bool CheckHigherPoison( Mobile from, Poison poison )
		{
			return ( m_Poison != null && m_Poison.Level >= poison.Level );
		}

		/// <summary>
		/// Overridable. Attempts to apply poison to the Mobile. Checks are made such that no <see cref="CheckHigherPoison">higher poison is active</see> and that the Mobile is not <see cref="CheckPoisonImmunity">immune to the poison</see>. Provided those assertions are true, the <paramref name="poison" /> is applied and <see cref="OnPoisoned" /> is invoked.
		/// <seealso cref="Poison" />
		/// <seealso cref="CurePoison" />
		/// </summary>
		/// <returns>One of four possible values:
		/// <list type="table">
		/// <item>
		/// <term><see cref="ApplyPoisonResult.Cured">Cured</see></term>
		/// <description>The <paramref name="poison" /> parameter was null and so <see cref="CurePoison" /> was invoked.</description>
		/// </item>
		/// <item>
		/// <term><see cref="ApplyPoisonResult.HigherPoisonActive">HigherPoisonActive</see></term>
		/// <description>The call to <see cref="CheckHigherPoison" /> returned false.</description>
		/// </item>
		/// <item>
		/// <term><see cref="ApplyPoisonResult.Immune">Immune</see></term>
		/// <description>The call to <see cref="CheckPoisonImmunity" /> returned false.</description>
		/// </item>
		/// <item>
		/// <term><see cref="ApplyPoisonResult.Poisoned">Poisoned</see></term>
		/// <description>The <paramref name="poison" /> was successfully applied.</description>
		/// </item>
		/// </list>
		/// </returns>
		public virtual ApplyPoisonResult ApplyPoison( Mobile from, Poison poison )
		{
			if ( poison == null )
			{
				CurePoison( from );
				return ApplyPoisonResult.Cured;
			}

			if ( CheckHigherPoison( from, poison ) )
			{
				OnHigherPoison( from, poison );
				return ApplyPoisonResult.HigherPoisonActive;
			}

			if ( CheckPoisonImmunity( from, poison ) )
			{
				OnPoisonImmunity( from, poison );
				return ApplyPoisonResult.Immune;
			}

			Poison oldPoison = m_Poison;
			this.Poison = poison;

			OnPoisoned( from, poison, oldPoison );

			return ApplyPoisonResult.Poisoned;
		}

		/// <summary>
		/// Overridable. Called from <see cref="CurePoison" />, this method checks to see that the Mobile can be cured of <see cref="Poison" />
		/// <seealso cref="CurePoison" />
		/// <seealso cref="Poison" />
		/// </summary>
		public virtual bool CheckCure( Mobile from )
		{
			return true;
		}

		/// <summary>
		/// Overridable. Virtual event invoked when a call to <see cref="CurePoison" /> succeeded.
		/// <seealso cref="CurePoison" />
		/// <seealso cref="CheckCure" />
		/// <seealso cref="Poison" />
		/// </summary>
		public virtual void OnCured( Mobile from, Poison oldPoison )
		{
		}

		/// <summary>
		/// Overridable. Virtual event invoked when a call to <see cref="CurePoison" /> failed.
		/// <seealso cref="CurePoison" />
		/// <seealso cref="CheckCure" />
		/// <seealso cref="Poison" />
		/// </summary>
		public virtual void OnFailedCure( Mobile from )
		{
		}

		/// <summary>
		/// Attempts to cure any poison that is currently active.
		/// </summary>
		/// <returns>True if poison was cured, false if otherwise.</returns>
		public bool CurePoison( Mobile from )
		{
			if ( CheckCure( from ) )
			{
				Poison oldPoison = m_Poison;
				this.Poison = null;

				OnCured( from, oldPoison );

				if ( oldPoison != null )
					EventSink.InvokePoisonCured( new PoisonCuredEventArgs( from, oldPoison ) );

				return true;
			}

			OnFailedCure( from );

			return false;
		}

		private ISpawner m_Spawner;

		public ISpawner Spawner { get { return m_Spawner; } set { m_Spawner = value; } }

		private Region m_WalkRegion;

		public Region WalkRegion { get { return m_WalkRegion; } set { m_WalkRegion = value; } }

		public virtual void OnBeforeSpawn( Point3D location, Map m )
		{
		}

		public virtual void OnAfterSpawn()
		{
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Poisoned
		{
			get
			{
				return ( m_Poison != null );
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool IsBodyMod
		{
			get
			{
				return ( m_BodyMod.BodyID != 0 );
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Body BodyMod
		{
			get
			{
				return m_BodyMod;
			}
			set
			{
				if ( m_BodyMod != value )
				{
					m_BodyMod = value;

					Delta( MobileDelta.Body );
					InvalidateProperties();

					CheckStatTimers();
				}
			}
		}

		private static int[] m_InvalidBodies = new int[]
			{
				32,
				156,
			};

		[Body, CommandProperty( AccessLevel.GameMaster )]
		public Body Body
		{
			get
			{
				if ( IsBodyMod )
					return m_BodyMod;

				return m_Body;
			}
			set
			{
				if ( m_Body != value && !IsBodyMod )
				{
					m_Body = SafeBody( value );

					Delta( MobileDelta.Body );
					InvalidateProperties();

					CheckStatTimers();
				}
			}
		}

		public virtual int SafeBody( int body )
		{
			int delta = -1;

			for ( int i = 0; delta < 0 && i < m_InvalidBodies.Length; ++i )
				delta = ( m_InvalidBodies[i] - body );

			if ( delta != 0 )
				return body;

			return 0;
		}

		[Body, CommandProperty( AccessLevel.GameMaster )]
		public int BodyValue
		{
			get
			{
				return Body.BodyID;
			}
			set
			{
				Body = value;
			}
		}

		IPoint3D IEntity.Location
		{
			get
			{
				return m_Location;
			}
		}

		IMap IEntity.Map
		{
			get
			{
				return m_Map;
			}
		}

		[CommandProperty( AccessLevel.Counselor, AccessLevel.GameMaster )]
		public Point3D Location
		{
			get
			{
				return m_Location;
			}
			set
			{
				SetLocation( value, true );
			}
		}

		public Point3D LogoutLocation
		{
			get
			{
				return m_LogoutLocation;
			}
			set
			{
				m_LogoutLocation = value;
			}
		}

		public Map LogoutMap
		{
			get
			{
				return m_LogoutMap;
			}
			set
			{
				m_LogoutMap = value;
			}
		}

		public Region Region
		{
			get
			{
				if ( m_Region == null )
					if ( this.Map == null )
						return Map.Internal.DefaultRegion;
					else
						return this.Map.DefaultRegion;
				else
					return m_Region;
			}
		}

		public void FreeCache()
		{
			ClearProperties();

			if ( m_RemovePacket != null )
			{
				m_RemovePacket.Release();
				m_RemovePacket = null;
			}
		}

		private Packet m_RemovePacket;

		public Packet RemovePacket
		{
			get
			{
				if ( m_RemovePacket == null )
				{
					m_RemovePacket = new RemoveMobile( this );
					m_RemovePacket.SetStatic();
				}

				return m_RemovePacket;
			}
		}

		private Packet m_OplPacket;

		public Packet OPLPacket
		{
			get
			{
				if ( m_OplPacket == null )
				{
					m_OplPacket = new OPLInfo( PropertyList );
					m_OplPacket.SetStatic();
				}

				return m_OplPacket;
			}
		}

		private static bool m_OldPropertyTitles;

		public static bool OldPropertyTitles
		{
			get
			{
				return m_OldPropertyTitles;
			}
			set
			{
				m_OldPropertyTitles = value;
			}
		}

		public virtual bool PropertyTitle
		{
			get
			{
				if ( !m_OldPropertyTitles )
					return true;

				return ClickTitle;
			}
		}

		private ObjectPropertyListPacket m_PropertyList;

		public ObjectPropertyListPacket PropertyList
		{
			get
			{
				if ( m_PropertyList == null )
				{
					m_PropertyList = new ObjectPropertyListPacket( this );

					GetProperties( m_PropertyList );

					m_PropertyList.Terminate();
					m_PropertyList.SetStatic();
				}

				return m_PropertyList;
			}
		}

		public void ClearProperties()
		{
			if ( m_PropertyList != null )
			{
				m_PropertyList.Release();
				m_PropertyList = null;
			}

			if ( m_OplPacket != null )
			{
				m_OplPacket.Release();
				m_OplPacket = null;
			}
		}

		public void InvalidateProperties()
		{
			if ( !ObjectPropertyListPacket.Enabled )
				return;

			if ( m_Map != null && m_Map != Map.Internal && !World.Loading )
			{
				ObjectPropertyListPacket oldList = m_PropertyList;

				if ( m_PropertyList != null )
				{
					m_PropertyList.Release();
					m_PropertyList = null;
				}

				ObjectPropertyListPacket newList = PropertyList;

				if ( oldList == null || oldList.Hash != newList.Hash )
				{
					if ( m_OplPacket != null )
					{
						m_OplPacket.Release();
						m_OplPacket = null;
					}

					Delta( MobileDelta.Properties );
				}
			}
			else
			{
				ClearProperties();
			}
		}

		private int m_SolidHueOverride = -1;

		[CommandProperty( AccessLevel.GameMaster )]
		public int SolidHueOverride
		{
			get { return m_SolidHueOverride; }
			set { if ( m_SolidHueOverride == value ) return; m_SolidHueOverride = value; Delta( MobileDelta.Hue | MobileDelta.Body ); }
		}

		public virtual void MoveToWorld( Point3D newLocation, Map map )
		{
			if ( m_Deleted )
				return;

			if ( m_Map == map )
			{
				SetLocation( newLocation, true );
				return;
			}

			BankBox box = FindBankNoCreate();

			if ( box != null && box.Opened )
				box.Close();

			Point3D oldLocation = m_Location;
			Map oldMap = m_Map;

			//Region oldRegion = m_Region;

			if ( oldMap != null )
			{
				oldMap.OnLeave( this );

				this.ClearScreen();
				this.SendRemovePacket();
			}

			for ( int i = 0; i < m_EquippedItems.Count; ++i )
				( (Item) m_EquippedItems[i] ).Map = map;

			m_Map = map;

			m_Location = newLocation;

			NetState ns = m_NetState;

			if ( m_Map != null )
			{
				m_Map.OnEnter( this );

				UpdateRegion();

				if ( ns != null && m_Map != null )
				{
					ns.Sequence = 0;
					ns.Send( new MapChange( this ) );
					ns.Send( SeasonChange.Instantiate( GetSeason(), true ) );
					ns.Send( new MobileUpdate( this ) );
					ClearFastwalkStack();
				}
			}
			else
			{
				UpdateRegion();
			}

			if ( ns != null )
			{
				if ( m_Map != null )
					Send( new ServerChange( this, m_Map ) );

				ns.Sequence = 0;
				ClearFastwalkStack();

				Send( new MobileIncoming( this, this ) );
				Send( new MobileUpdate( this ) );
				CheckLightLevels( true );
				Send( new MobileUpdate( this ) );
			}

			this.SendEverything();
			this.SendIncomingPacket();

			if ( ns != null )
			{
				m_NetState.Sequence = 0;
				ClearFastwalkStack();

				Send( new MobileIncoming( this, this ) );
				Send( SupportedFeatures.Instantiate( ns ) );
				Send( new MobileUpdate( this ) );
				Send( new MobileAttributes( this ) );
			}

			EventSink.InvokeMapChanged( new MapChangedEventArgs( this, oldMap ) );

			OnMapChange( oldMap );
			OnLocationChange( oldLocation );

			if ( m_Region != null )
				m_Region.OnLocationChanged( this, oldLocation );
		}

		public virtual void SetLocation( Point3D newLocation, bool isTeleport )
		{
			if ( m_Deleted )
				return;

			Point3D oldLocation = m_Location;

			if ( oldLocation != newLocation )
			{
				m_Location = newLocation;
				UpdateRegion();

				BankBox box = FindBankNoCreate();

				if ( box != null && box.Opened )
					box.Close();

				if ( m_NetState != null )
					m_NetState.ValidateAllTrades();

				if ( m_Map != null )
					m_Map.OnMove( oldLocation, this );

				if ( isTeleport && m_NetState != null )
				{
					m_NetState.Sequence = 0;
					m_NetState.Send( new MobileUpdate( this ) );
					ClearFastwalkStack();
				}

				InvokeLocationChanged( new LocationChangedEventArgs( oldLocation, newLocation, isTeleport ) );

				OnLocationChange( oldLocation );

				this.Region.OnLocationChanged( this, oldLocation );
			}
		}

		/// <summary>
		/// Overridable. Virtual event invoked when <see cref="Location" /> changes.
		/// </summary>
		protected virtual void OnLocationChange( Point3D oldLocation )
		{
		}

		#region Hair

		private HairInfo m_Hair;
		private FacialHairInfo m_FacialHair;

		[CommandProperty( AccessLevel.GameMaster )]
		public int HairItemID
		{
			get
			{
				if ( m_Hair == null )
					return 0;

				return m_Hair.ItemID;
			}
			set
			{
				if ( m_Hair == null && value > 0 )
					m_Hair = new HairInfo( value );
				else if ( value <= 0 )
					m_Hair = null;
				else
					m_Hair.ItemID = value;

				Delta( MobileDelta.Hair );
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int FacialHairItemID
		{
			get
			{
				if ( m_FacialHair == null )
					return 0;

				return m_FacialHair.ItemID;
			}
			set
			{
				if ( m_FacialHair == null && value > 0 )
					m_FacialHair = new FacialHairInfo( value );
				else if ( value <= 0 )
					m_FacialHair = null;
				else
					m_FacialHair.ItemID = value;

				Delta( MobileDelta.FacialHair );
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int HairHue
		{
			get
			{
				if ( m_Hair == null )
					return 0;
				return m_Hair.Hue;
			}
			set
			{
				if ( m_Hair != null )
				{
					m_Hair.Hue = value;
					Delta( MobileDelta.Hair );
				}
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int FacialHairHue
		{
			get
			{
				if ( m_FacialHair == null )
					return 0;

				return m_FacialHair.Hue;
			}
			set
			{
				if ( m_FacialHair != null )
				{
					m_FacialHair.Hue = value;
					Delta( MobileDelta.FacialHair );
				}
			}
		}

		#endregion

		private IWeapon m_Weapon;

		[CommandProperty( AccessLevel.GameMaster )]
		public virtual IWeapon Weapon
		{
			get
			{
				Item item = m_Weapon as Item;

				if ( item != null && !item.Deleted && item.Parent == this && CanSee( item ) )
					return m_Weapon;

				m_Weapon = null;

				item = this.FindItemOnLayer( Layer.OneHanded );

				if ( item == null )
					item = this.FindItemOnLayer( Layer.TwoHanded );

				if ( item is IWeapon )
					return ( m_Weapon = (IWeapon) item );
				else
					return GetDefaultWeapon();
			}
		}

		public virtual IWeapon GetDefaultWeapon()
		{
			return m_DefaultWeapon;
		}

		private BankBox m_BankBox;

		[CommandProperty( AccessLevel.GameMaster )]
		public BankBox BankBox
		{
			get
			{
				if ( m_BankBox != null && !m_BankBox.Deleted && m_BankBox.Parent == this )
					return m_BankBox;

				m_BankBox = this.FindItemOnLayer( Layer.Bank ) as BankBox;

				if ( m_BankBox == null )
					AddItem( m_BankBox = new BankBox( this ) );

				return m_BankBox;
			}
		}

		public BankBox FindBankNoCreate()
		{
			if ( m_BankBox != null && !m_BankBox.Deleted && m_BankBox.Parent == this )
				return m_BankBox;

			m_BankBox = this.FindItemOnLayer( Layer.Bank ) as BankBox;

			return m_BankBox;
		}

		private Container m_Backpack;

		[CommandProperty( AccessLevel.GameMaster )]
		public Container Backpack
		{
			get
			{
				if ( m_Backpack != null && !m_Backpack.Deleted && m_Backpack.Parent == this )
					return m_Backpack;

				return ( m_Backpack = ( this.FindItemOnLayer( Layer.Backpack ) as Container ) );
			}
		}

		public IEnumerable<Item> GetInventoryItems()
		{
			if ( Backpack == null )
				return Enumerable.Empty<Item>();

			return Backpack.Items;
		}

		public virtual bool KeepsItemsOnDeath { get { return m_AccessLevel > AccessLevel.Player; } }

		[CommandProperty( AccessLevel.Counselor, AccessLevel.GameMaster )]
		public int X
		{
			get { return m_Location.X; }
			set { Location = new Point3D( value, m_Location.Y, m_Location.Z ); }
		}

		[CommandProperty( AccessLevel.Counselor, AccessLevel.GameMaster )]
		public int Y
		{
			get { return m_Location.Y; }
			set { Location = new Point3D( m_Location.X, value, m_Location.Z ); }
		}

		[CommandProperty( AccessLevel.Counselor, AccessLevel.GameMaster )]
		public int Z
		{
			get { return m_Location.Z; }
			set { Location = new Point3D( m_Location.X, m_Location.Y, value ); }
		}

		public Item FindItemOnLayer( Layer layer )
		{
			return this.GetEquippedItems().FirstOrDefault( item => !item.Deleted && item.Layer == layer );
		}

		#region Effects & Particles

		public void MovingEffect( IEntity to, int itemID, int speed, int duration, bool fixedDirection, bool explodes, int hue, int renderMode )
		{
			Effects.SendMovingEffect( this, to, itemID, speed, duration, fixedDirection, explodes, hue, renderMode );
		}

		public void MovingEffect( IEntity to, int itemID, int speed, int duration, bool fixedDirection, bool explodes )
		{
			Effects.SendMovingEffect( this, to, itemID, speed, duration, fixedDirection, explodes, 0, 0 );
		}

		public void MovingParticles( IEntity to, int itemID, int speed, int duration, bool fixedDirection, bool explodes, int hue, int renderMode, int effect, int explodeEffect, int explodeSound, EffectLayer layer, int unknown )
		{
			Effects.SendMovingParticles( this, to, itemID, speed, duration, fixedDirection, explodes, hue, renderMode, effect, explodeEffect, explodeSound, layer, unknown );
		}

		public void MovingParticles( IEntity to, int itemID, int speed, int duration, bool fixedDirection, bool explodes, int hue, int renderMode, int effect, int explodeEffect, int explodeSound, int unknown )
		{
			Effects.SendMovingParticles( this, to, itemID, speed, duration, fixedDirection, explodes, hue, renderMode, effect, explodeEffect, explodeSound, (EffectLayer) 255, unknown );
		}

		public void MovingParticles( IEntity to, int itemID, int speed, int duration, bool fixedDirection, bool explodes, int effect, int explodeEffect, int explodeSound, int unknown )
		{
			Effects.SendMovingParticles( this, to, itemID, speed, duration, fixedDirection, explodes, effect, explodeEffect, explodeSound, unknown );
		}

		public void MovingParticles( IEntity to, int itemID, int speed, int duration, bool fixedDirection, bool explodes, int effect, int explodeEffect, int explodeSound )
		{
			Effects.SendMovingParticles( this, to, itemID, speed, duration, fixedDirection, explodes, 0, 0, effect, explodeEffect, explodeSound, 0 );
		}

		public void FixedEffect( int itemID, int speed, int duration, int hue, int renderMode )
		{
			Effects.SendTargetEffect( this, itemID, speed, duration, hue, renderMode );
		}

		public void FixedEffect( int itemID, int speed, int duration )
		{
			Effects.SendTargetEffect( this, itemID, speed, duration, 0, 0 );
		}

		public void FixedParticles( int itemID, int speed, int duration, int effect, int hue, int renderMode, EffectLayer layer, int unknown )
		{
			Effects.SendTargetParticles( this, itemID, speed, duration, hue, renderMode, effect, layer, unknown );
		}

		public void FixedParticles( int itemID, int speed, int duration, int effect, int hue, int renderMode, EffectLayer layer )
		{
			Effects.SendTargetParticles( this, itemID, speed, duration, hue, renderMode, effect, layer, 0 );
		}

		public void FixedParticles( int itemID, int speed, int duration, int effect, EffectLayer layer, int unknown )
		{
			Effects.SendTargetParticles( this, itemID, speed, duration, 0, 0, effect, layer, unknown );
		}

		public void FixedParticles( int itemID, int speed, int duration, int effect, EffectLayer layer )
		{
			Effects.SendTargetParticles( this, itemID, speed, duration, 0, 0, effect, layer, 0 );
		}

		public void BoltEffect( int hue )
		{
			Effects.SendBoltEffect( this, true, hue );
		}

		#endregion

		public virtual bool CheckLift( Mobile from, Item item, ref LRReason reject )
		{
			return true;
		}

		public virtual bool CheckNonlocalLift( Mobile from, Item item )
		{
			if ( from == this || ( from.AccessLevel > this.AccessLevel && from.AccessLevel >= AccessLevel.GameMaster ) )
				return true;

			return false;
		}

		public bool HasTrade
		{
			get
			{
				if ( m_NetState != null )
					return m_NetState.Trades.Any();

				return false;
			}
		}

		public virtual bool CheckTrade( Mobile to, Item item, SecureTradeContainer cont, bool message, bool checkItems, int plusItems, int plusWeight )
		{
			return true;
		}

		/// <summary>
		/// Overridable. Event invoked when a Mobile (<paramref name="from" />) drops an <see cref="Item"><paramref name="dropped" /></see> onto the Mobile.
		/// </summary>
		public virtual bool OnDragDrop( Mobile from, Item dropped )
		{
			if ( from == this )
			{
				Container pack = this.Backpack;

				if ( pack != null )
					return dropped.DropToItem( from, pack, new Point3D( -1, -1, 0 ), 0x0 );

				return false;
			}
			else if ( from.Player && this.Player && from.Alive && this.Alive && from.InRange( Location, 2 ) )
			{
				NetState ourState = m_NetState;
				NetState theirState = from.m_NetState;

				if ( ourState != null && theirState != null )
				{
					SecureTradeContainer cont = theirState.FindTradeContainer( this );

					if ( !from.CheckTrade( this, dropped, cont, true, true, 0, 0 ) )
						return false;

					if ( cont == null )
						cont = theirState.AddTrade( ourState );

					cont.DropItem( dropped );

					return true;
				}

				return false;
			}
			else
			{
				return false;
			}
		}

		public virtual bool CheckEquip( Item item )
		{
			for ( int i = 0; i < m_EquippedItems.Count; ++i )
				if ( ( (Item) m_EquippedItems[i] ).CheckConflictingLayer( this, item, item.Layer ) || item.CheckConflictingLayer( this, (Item) m_EquippedItems[i], ( (Item) m_EquippedItems[i] ).Layer ) )
					return false;

			return true;
		}

		public virtual bool OverridesRaceReq { get { return false; } }

		/// <summary>
		/// Overridable. Virtual event invoked when the Mobile attempts to wear <paramref name="item" />.
		/// </summary>
		/// <returns>True if the request is accepted, false if otherwise.</returns>
		public virtual bool OnEquip( Item item )
		{
			return true;
		}

		/// <summary>
		/// Overridable. Virtual event invoked when the Mobile attempts to lift <paramref name="item" />.
		/// </summary>
		/// <returns>True if the lift is allowed, false if otherwise.</returns>
		/// <example>
		/// The following example demonstrates usage. It will disallow any attempts to pick up a pick axe if the Mobile does not have enough strength.
		/// <code>
		/// public override bool OnDragLift( Item item )
		/// {
		///		if ( item is Pickaxe &amp;&amp; this.Str &lt; 60 )
		///		{
		///			SendMessage( "That is too heavy for you to lift." );
		///			return false;
		///		}
		///
		///		return base.OnDragLift( item );
		/// }</code>
		/// </example>
		public virtual bool OnDragLift( Item item )
		{
			return true;
		}

		/// <summary>
		/// Overridable. Virtual event invoked when the Mobile attempts to drop <paramref name="item" /> into a <see cref="Container"><paramref name="container" /></see>.
		/// </summary>
		/// <returns>True if the drop is allowed, false if otherwise.</returns>
		public virtual bool OnDroppedItemInto( Item item, Container container, Point3D loc )
		{
			return true;
		}

		/// <summary>
		/// Overridable. Virtual event invoked when the Mobile attempts to drop <paramref name="item" /> directly onto another <see cref="Item" />, <paramref name="target" />. This is the case of stacking items.
		/// </summary>
		/// <returns>True if the drop is allowed, false if otherwise.</returns>
		public virtual bool OnDroppedItemOnto( Item item, Item target )
		{
			return true;
		}

		/// <summary>
		/// Overridable. Virtual event invoked when the Mobile attempts to drop <paramref name="item" /> into another <see cref="Item" />, <paramref name="target" />. The target item is most likely a <see cref="Container" />.
		/// </summary>
		/// <returns>True if the drop is allowed, false if otherwise.</returns>
		public virtual bool OnDroppedItemToItem( Item item, Item target, Point3D loc )
		{
			return true;
		}

		/// <summary>
		/// Overridable. Virtual event invoked when the Mobile attempts to give <paramref name="item" /> to a Mobile (<paramref name="target" />).
		/// </summary>
		/// <returns>True if the drop is allowed, false if otherwise.</returns>
		public virtual bool OnDroppedItemToMobile( Item item, Mobile target )
		{
			return true;
		}

		/// <summary>
		/// Overridable. Virtual event invoked when the Mobile attempts to drop <paramref name="item" /> to the world at a <see cref="Point3D"><paramref name="location" /></see>.
		/// </summary>
		/// <returns>True if the drop is allowed, false if otherwise.</returns>
		public virtual bool OnDroppedItemToWorld( Item item, Point3D location )
		{
			return true;
		}

		/// <summary>
		/// Overridable. Virtual event when <paramref name="from" /> successfully uses <paramref name="item" /> while it's on this Mobile.
		/// <seealso cref="Item.OnItemUsed" />
		/// </summary>
		public virtual void OnItemUsed( Mobile from, Item item )
		{
		}

		public virtual bool CheckNonlocalDrop( Mobile from, Item item, Item target )
		{
			if ( from == this || ( from.AccessLevel > this.AccessLevel && from.AccessLevel >= AccessLevel.GameMaster ) )
				return true;

			return false;
		}

		public virtual bool CheckItemUse( Mobile from, Item item )
		{
			return true;
		}

		/// <summary>
		/// Overridable. Virtual event invoked when <paramref name="from" /> successfully lifts <paramref name="item" /> from this Mobile.
		/// <seealso cref="Item.OnItemLifted" />
		/// </summary>
		public virtual void OnItemLifted( Mobile from, Item item )
		{
		}

		public virtual bool AllowItemUse( Item item )
		{
			return true;
		}

		public virtual bool AllowEquipFrom( Mobile mob )
		{
			return ( mob == this || ( mob.AccessLevel >= AccessLevel.GameMaster && mob.AccessLevel > this.AccessLevel ) );
		}

		internal int m_TypeRef;

		/// <summary>
		/// Default constructor. Used when creating a new mobile.
		/// </summary>
		public Mobile()
			: this( SerialGenerator.GetNewMobileSerial() )
		{
			DefaultMobileInit();

			World.AddMobile( this );
		}

		/// <summary>
		/// Serialization constructor. Used when deserializing an existing mobile.
		/// </summary>
		public Mobile( Serial serial )
		{
			m_Serial = serial;
			m_Region = Map.Internal.DefaultRegion;

			m_Aggressors = new List<AggressorInfo>( 1 );
			m_Aggressed = new List<AggressorInfo>( 1 );
			m_NextSkillTime = DateTime.MinValue;
			m_DamageEntries = new List<DamageEntry>( 1 );

			RegisterType();
		}

		private void RegisterType()
		{
			Type ourType = this.GetType();
			m_TypeRef = World.m_MobileTypes.IndexOf( ourType );

			if ( m_TypeRef == -1 )
			{
				World.m_MobileTypes.Add( ourType );
				m_TypeRef = World.m_MobileTypes.Count - 1;
			}
		}

		/// <summary>
		/// Initialize some fields when creating a fresh new Mobile. Otherwise these should be initialized by <see cref="Deserialize"/> method.
		/// </summary>
		private void DefaultMobileInit()
		{
			m_StatCap = 225;
			m_Skills = new Skills( this );
			m_EquippedItems = new List<Item>( 1 );
			m_StatMods = new List<StatMod>( 1 );
			Map = Map.Internal;
			m_AutoPageNotify = true;
			m_Virtues = new VirtueInfo();
			m_Stabled = new List<Mobile>( 1 );
			m_CreationTime = DateTime.UtcNow;
		}

		private static Queue<Mobile> m_DeltaQueue = new Queue<Mobile>();

		private bool m_InDeltaQueue;
		private MobileDelta m_DeltaFlags;

		public virtual void Delta( MobileDelta flag )
		{
			if ( m_Map == null || m_Map == Map.Internal || m_Deleted )
				return;

			m_DeltaFlags |= flag;

			if ( !m_InDeltaQueue )
			{
				m_InDeltaQueue = true;

				m_DeltaQueue.Enqueue( this );
			}
		}

		public void UpdateHealthBar( HealthBarColor color, bool enabled )
		{
			if ( m_Deleted || m_Map == null )
				return;

			Packet p = Packet.Acquire( new HealthBarStatus( this, color, enabled ) );

			foreach ( NetState state in m_Map.GetClientsInRange( m_Location ) )
			{
				if ( state.Mobile.CanSee( this ) )
					state.Send( p );
			}

			Packet.Release( p );
		}

		public void ProcessDelta()
		{
			Mobile m = this;
			MobileDelta delta;
			MobileDelta attrs;

			delta = m.m_DeltaFlags;

			if ( delta == MobileDelta.None )
				return;

			attrs = delta & MobileDelta.Attributes;

			m.m_DeltaFlags = MobileDelta.None;
			m.m_InDeltaQueue = false;

			bool sendHits = false, sendStam = false, sendMana = false, sendAll = false, sendAny = false;
			bool sendIncoming = false, sendNonlocalIncoming = false;
			bool sendUpdate = false, sendRemove = false;
			bool sendPublicStats = false, sendPrivateStats = false;
			bool sendMoving = false, sendNonlocalMoving = false;
			bool sendOPLUpdate = ObjectPropertyListPacket.Enabled && ( delta & MobileDelta.Properties ) != 0;
			bool sendHair = false, sendFacialHair = false, removeHair = false, removeFacialHair = false;

			if ( attrs != MobileDelta.None )
			{
				sendAny = true;

				if ( attrs == MobileDelta.Attributes )
				{
					sendAll = true;
				}
				else
				{
					sendHits = ( ( attrs & MobileDelta.Hits ) != 0 );
					sendStam = ( ( attrs & MobileDelta.Stam ) != 0 );
					sendMana = ( ( attrs & MobileDelta.Mana ) != 0 );
				}
			}

			if ( ( delta & MobileDelta.GhostUpdate ) != 0 )
			{
				sendNonlocalIncoming = true;
			}

			if ( ( delta & MobileDelta.Hue ) != 0 )
			{
				sendNonlocalIncoming = true;
				sendUpdate = true;
				sendRemove = true;
			}

			if ( ( delta & MobileDelta.Direction ) != 0 )
			{
				sendNonlocalMoving = true;
				sendUpdate = true;
			}

			if ( ( delta & MobileDelta.Body ) != 0 )
			{
				sendUpdate = true;
				sendIncoming = true;
			}

			if ( ( delta & ( MobileDelta.Flags | MobileDelta.Noto ) ) != 0 )
			{
				sendMoving = true;
			}

			if ( ( delta & MobileDelta.Name ) != 0 )
			{
				sendAll = false;
				sendHits = false;
				sendAny = sendStam || sendMana;
				sendPublicStats = true;
			}

			if ( ( delta & ( MobileDelta.WeaponDamage | MobileDelta.Resistances | MobileDelta.Stat | MobileDelta.Weight | MobileDelta.Gold | MobileDelta.Armor | MobileDelta.StatCap | MobileDelta.Followers | MobileDelta.TithingPoints | MobileDelta.Race ) ) != 0 )
			{
				sendPrivateStats = true;
			}

			if ( ( delta & MobileDelta.Hair ) != 0 )
			{
				if ( m.HairItemID <= 0 )
					removeHair = true;

				sendHair = true;
			}

			if ( ( delta & MobileDelta.FacialHair ) != 0 )
			{
				if ( m.FacialHairItemID <= 0 )
					removeFacialHair = true;

				sendFacialHair = true;
			}

			Packet[] cache = m_MovingPacketCache;

			if ( sendMoving || sendNonlocalMoving )
			{
				for ( int i = 0; i < cache.Length; ++i )
					Packet.Release( ref cache[i] );
			}

			NetState ourState = m.m_NetState;

			if ( ourState != null )
			{
				if ( sendUpdate )
				{
					ourState.Sequence = 0;
					ourState.Send( new MobileUpdate( m ) );
					ClearFastwalkStack();
				}

				if ( sendIncoming )
					ourState.Send( new MobileIncoming( m, m ) );

				if ( sendMoving )
				{
					int noto = Notoriety.Compute( m, m );
					ourState.Send( cache[noto] = Packet.Acquire( GenericPackets.MobileMoving( m, noto ) ) );
				}

				if ( sendPublicStats || sendPrivateStats )
				{
					ourState.Send( new MobileStatus( m ) );
				}
				else if ( sendAll )
				{
					ourState.Send( new MobileAttributes( m ) );
				}
				else if ( sendAny )
				{
					if ( sendHits )
						ourState.Send( new MobileHits( m ) );

					if ( sendStam )
						ourState.Send( new MobileStam( m ) );

					if ( sendMana )
						ourState.Send( new MobileMana( m ) );
				}

				if ( sendStam || sendMana )
				{
					IParty ip = m_Party as IParty;

					if ( ip != null && sendStam )
						ip.OnStamChanged( this );

					if ( ip != null && sendMana )
						ip.OnManaChanged( this );
				}

				if ( sendHair )
				{
					if ( removeHair )
						ourState.Send( new RemoveHair( m ) );
					else
						ourState.Send( new HairEquipUpdate( m ) );
				}

				if ( sendFacialHair )
				{
					if ( removeFacialHair )
						ourState.Send( new RemoveFacialHair( m ) );
					else
						ourState.Send( new FacialHairEquipUpdate( m ) );
				}

				if ( sendOPLUpdate )
					ourState.Send( OPLPacket );
			}

			sendMoving = sendMoving || sendNonlocalMoving;
			sendIncoming = sendIncoming || sendNonlocalIncoming;
			sendHits = sendHits || sendAll;

			if ( m.m_Map != null && ( sendRemove || sendIncoming || sendPublicStats || sendHits || sendMoving || sendOPLUpdate || sendHair || sendFacialHair ) )
			{
				Mobile beholder;

				Packet hitsPacket = null;
				Packet statPacketTrue = null, statPacketFalse = null;
				Packet deadPacket = null;
				Packet hairPacket = null, facialhairPacket = null;

				foreach ( NetState state in m.GetClientsInRange() )
				{
					beholder = state.Mobile;

					if ( beholder != m && beholder.CanSee( m ) )
					{
						if ( sendRemove )
							state.Send( m.RemovePacket );

						if ( sendIncoming )
						{
							state.Send( new MobileIncoming( beholder, m ) );

							if ( m.IsDeadBondedPet )
							{
								if ( deadPacket == null )
									deadPacket = Packet.Acquire( new BondedStatus( 0, m.Serial, 1 ) );

								state.Send( deadPacket );
							}
						}

						if ( sendMoving )
						{
							int noto = Notoriety.Compute( beholder, m );

							Packet p = cache[noto];

							if ( p == null )
								cache[noto] = p = Packet.Acquire( GenericPackets.MobileMoving( m, noto ) );

							state.Send( p );
						}

						if ( sendPublicStats )
						{
							if ( m.CanBeRenamedBy( beholder ) )
							{
								if ( statPacketTrue == null )
									statPacketTrue = Packet.Acquire( new MobileStatusCompact( true, m ) );

								state.Send( statPacketTrue );
							}
							else
							{
								if ( statPacketFalse == null )
									statPacketFalse = Packet.Acquire( new MobileStatusCompact( false, m ) );

								state.Send( statPacketFalse );
							}
						}
						else if ( sendHits )
						{
							if ( hitsPacket == null )
								hitsPacket = Packet.Acquire( new MobileHitsN( m ) );

							state.Send( hitsPacket );
						}

						if ( sendHair )
						{
							if ( hairPacket == null )
							{
								if ( removeHair )
									hairPacket = Packet.Acquire( new RemoveHair( m ) );
								else
									hairPacket = Packet.Acquire( new HairEquipUpdate( m ) );
							}

							state.Send( hairPacket );
						}

						if ( sendFacialHair )
						{
							if ( facialhairPacket == null )
							{
								if ( removeFacialHair )
									facialhairPacket = Packet.Acquire( new RemoveFacialHair( m ) );
								else
									facialhairPacket = Packet.Acquire( new FacialHairEquipUpdate( m ) );
							}

							state.Send( facialhairPacket );
						}

						if ( sendOPLUpdate )
							state.Send( OPLPacket );
					}
				}

				Packet.Release( hitsPacket );
				Packet.Release( statPacketTrue );
				Packet.Release( statPacketFalse );
				Packet.Release( deadPacket );
				Packet.Release( hairPacket );
				Packet.Release( facialhairPacket );
			}

			if ( !sendMoving && !sendNonlocalMoving )
				return;

			for ( int i = 0; i < cache.Length; i++ )
				Packet.Release( ref cache[i] );
		}

		public static void ProcessDeltaQueue()
		{
			int count = m_DeltaQueue.Count;
			int index = 0;

			while ( m_DeltaQueue.Count > 0 && index++ < count )
			{
				Mobile mobile = m_DeltaQueue.Dequeue();

				try
				{
					mobile.ProcessDelta();
				}
				catch ( Exception e )
				{
					log.Error( "Exception disarmed in Mobile.ProcessDeltaQueue in {0}: {1}", mobile, e );
				}
			}
		}

		[CommandProperty( AccessLevel.Counselor, AccessLevel.GameMaster )]
		public int Kills
		{
			get
			{
				return m_Kills;
			}
			set
			{
				int oldValue = m_Kills;

				if ( m_Kills != value )
				{
					m_Kills = value;

					if ( m_Kills < 0 )
						m_Kills = 0;

					if ( ( oldValue >= 5 ) != ( m_Kills > 5 ) )
					{
						Delta( MobileDelta.Noto );
						InvalidateProperties();
					}

					OnKillsChange( oldValue );
				}
			}
		}

		public virtual void OnKillsChange( int oldValue )
		{
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int ShortTermMurders
		{
			get
			{
				return m_ShortTermMurders;
			}
			set
			{
				if ( m_ShortTermMurders != value )
				{
					m_ShortTermMurders = value;

					if ( m_ShortTermMurders < 0 )
						m_ShortTermMurders = 0;
				}
			}
		}

		[CommandProperty( AccessLevel.Counselor, AccessLevel.GameMaster )]
		public bool Criminal
		{
			get
			{
				return m_Criminal;
			}
			set
			{
				if ( m_Criminal != value )
				{
					m_Criminal = value;
					Delta( MobileDelta.Noto );
					InvalidateProperties();
				}

				if ( m_Criminal )
				{
					if ( m_ExpireCriminal == null )
						m_ExpireCriminal = new ExpireCriminalTimer( this );
					else
						m_ExpireCriminal.Stop();

					m_ExpireCriminal.Start();
				}
				else if ( m_ExpireCriminal != null )
				{
					m_ExpireCriminal.Stop();
					m_ExpireCriminal = null;
				}
			}
		}

		public bool Murderer => m_Kills >= 5;

		public bool CheckAlive( bool message = true )
		{
			if ( !Alive )
			{
				if ( message )
					this.LocalOverheadMessage( MessageType.Regular, 0x3B2, 1019048 ); // I am dead and cannot do that.

				return false;
			}
			else
			{
				return true;
			}
		}

		#region Overhead messages

		public void PublicOverheadMessage( MessageType type, int hue, bool ascii, string text, bool noLineOfSight = true )
		{
			if ( m_Map != null )
			{
				Packet p = null;

				foreach ( NetState state in m_Map.GetClientsInRange( m_Location ) )
				{
					if ( state.Mobile.CanSee( this ) && ( noLineOfSight || state.Mobile.InLOS( this ) ) )
					{
						if ( p == null )
						{
							if ( ascii )
								p = new AsciiMessage( this.Serial, Body, type, hue, 3, Name, text );
							else
								p = new UnicodeMessage( this.Serial, Body, type, hue, 3, m_Language, Name, text );

							p.Acquire();
						}

						state.Send( p );
					}
				}

				Packet.Release( p );
			}
		}

		public void PublicOverheadMessage( MessageType type, int hue, int number, string args = "", bool noLineOfSight = true )
		{
			if ( m_Map != null )
			{
				Packet p = null;

				foreach ( NetState state in m_Map.GetClientsInRange( m_Location ) )
				{
					if ( state.Mobile.CanSee( this ) && ( noLineOfSight || state.Mobile.InLOS( this ) ) )
					{
						if ( p == null )
							p = Packet.Acquire( new MessageLocalized( this.Serial, Body, type, hue, 3, number, Name, args ) );

						state.Send( p );
					}
				}

				Packet.Release( p );
			}
		}

		public void PublicOverheadMessage( MessageType type, int hue, int number, AffixType affixType, string affix, string args, bool noLineOfSight = true )
		{
			if ( m_Map != null )
			{
				Packet p = null;

				foreach ( NetState state in m_Map.GetClientsInRange( m_Location ) )
				{
					if ( state.Mobile.CanSee( this ) && ( noLineOfSight || state.Mobile.InLOS( this ) ) )
					{
						if ( p == null )
							p = Packet.Acquire( new MessageLocalizedAffix( this.Serial, Body, type, hue, 3, number, Name, affixType, affix, args ) );

						state.Send( p );
					}
				}

				Packet.Release( p );
			}
		}

		public void PrivateOverheadMessage( MessageType type, int hue, bool ascii, string text, NetState state )
		{
			if ( state == null )
				return;

			if ( ascii )
				state.Send( new AsciiMessage( this.Serial, Body, type, hue, 3, Name, text ) );
			else
				state.Send( new UnicodeMessage( this.Serial, Body, type, hue, 3, m_Language, Name, text ) );
		}

		public void PrivateOverheadMessage( MessageType type, int hue, int number, NetState state )
		{
			PrivateOverheadMessage( type, hue, number, "", state );
		}

		public void PrivateOverheadMessage( MessageType type, int hue, int number, string args, NetState state )
		{
			if ( state == null )
				return;

			state.Send( new MessageLocalized( this.Serial, Body, type, hue, 3, number, Name, args ) );
		}

		public void LocalOverheadMessage( MessageType type, int hue, bool ascii, string text )
		{
			NetState ns = m_NetState;

			if ( ns != null )
			{
				if ( ascii )
					ns.Send( new AsciiMessage( this.Serial, Body, type, hue, 3, Name, text ) );
				else
					ns.Send( new UnicodeMessage( this.Serial, Body, type, hue, 3, m_Language, Name, text ) );
			}
		}

		public void LocalOverheadMessage( MessageType type, int hue, int number, string args = "" )
		{
			NetState ns = m_NetState;

			if ( ns != null )
				ns.Send( new MessageLocalized( this.Serial, Body, type, hue, 3, number, Name, args ) );
		}

		public void NonlocalOverheadMessage( MessageType type, int hue, int number, string args = "" )
		{
			if ( m_Map != null )
			{
				Packet p = null;

				foreach ( NetState state in m_Map.GetClientsInRange( m_Location ) )
				{
					if ( state != m_NetState && state.Mobile.CanSee( this ) )
					{
						if ( p == null )
							p = Packet.Acquire( new MessageLocalized( this.Serial, Body, type, hue, 3, number, Name, args ) );

						state.Send( p );
					}
				}

				if ( p != null )
					p.Release();
			}
		}

		public void NonlocalOverheadMessage( MessageType type, int hue, bool ascii, string text )
		{
			if ( m_Map != null )
			{
				Packet p = null;

				foreach ( NetState state in m_Map.GetClientsInRange( m_Location ) )
				{
					if ( state != m_NetState && state.Mobile.CanSee( this ) )
					{
						if ( p == null )
						{
							if ( ascii )
								p = new AsciiMessage( this.Serial, Body, type, hue, 3, Name, text );
							else
								p = new UnicodeMessage( this.Serial, Body, type, hue, 3, Language, Name, text );

							p.Acquire();
						}

						state.Send( p );
					}
				}

				Packet.Release( p );
			}
		}

		#endregion

		#region SendLocalizedMessage

		public void SendLocalizedMessage( int number )
		{
			NetState ns = m_NetState;

			if ( ns != null )
				ns.Send( MessageLocalized.InstantiateGeneric( number ) );
		}

		public void SendLocalizedMessage( int number, string args, int hue = 0x3B2 )
		{
			if ( hue == 0x3B2 && ( args == null || args.Length == 0 ) )
			{
				NetState ns = m_NetState;

				if ( ns != null )
					ns.Send( MessageLocalized.InstantiateGeneric( number ) );
			}
			else
			{
				NetState ns = m_NetState;

				if ( ns != null )
					ns.Send( new MessageLocalized( Serial.MinusOne, -1, MessageType.Regular, hue, 3, number, "System", args ) );
			}
		}

		public void SendLocalizedMessage( int number, bool append, string affix, string args = "", int hue = 0x3B2 )
		{
			NetState ns = m_NetState;

			if ( ns != null )
				ns.Send( new MessageLocalizedAffix( Serial.MinusOne, -1, MessageType.Regular, hue, 3, number, "System", ( append ? AffixType.Append : AffixType.Prepend ) | AffixType.System, affix, args ) );
		}

		#endregion

		#region Send[ASCII]Message

		public void SendMessage( string text )
		{
			SendMessage( 0x3B2, text );
		}

		public void SendMessage( string format, params object[] args )
		{
			SendMessage( 0x3B2, String.Format( format, args ) );
		}

		public void SendMessage( int hue, string text )
		{
			NetState ns = m_NetState;

			if ( ns != null )
				ns.Send( new UnicodeMessage( Serial.MinusOne, -1, MessageType.Regular, hue, 3, "ENU", "System", text ) );
		}

		public void SendMessage( int hue, string format, params object[] args )
		{
			SendMessage( hue, String.Format( format, args ) );
		}

		public void SendAsciiMessage( string text )
		{
			SendAsciiMessage( 0x3B2, text );
		}

		public void SendAsciiMessage( string format, params object[] args )
		{
			SendAsciiMessage( 0x3B2, String.Format( format, args ) );
		}

		public void SendAsciiMessage( int hue, string text )
		{
			NetState ns = m_NetState;

			if ( ns != null )
				ns.Send( new AsciiMessage( Serial.MinusOne, -1, MessageType.Regular, hue, 3, "System", text ) );
		}

		public void SendAsciiMessage( int hue, string format, params object[] args )
		{
			SendAsciiMessage( hue, String.Format( format, args ) );
		}

		#endregion

		public void LaunchBrowser( string url )
		{
			if ( m_NetState != null )
				m_NetState.LaunchBrowser( url );
		}

		public void InitStats( int str, int dex, int intel )
		{
			m_Str = str;
			m_Dex = dex;
			m_Int = intel;

			Hits = HitsMax;
			Stam = StamMax;
			Mana = ManaMax;

			Delta( MobileDelta.Stat | MobileDelta.Hits | MobileDelta.Stam | MobileDelta.Mana );
		}

		public virtual void DisplayPaperdollTo( Mobile to )
		{
			EventSink.InvokePaperdollRequest( new PaperdollRequestEventArgs( to, this ) );
		}

		private static bool m_DisableDismountInWarmode;

		public static bool DisableDismountInWarmode { get { return m_DisableDismountInWarmode; } set { m_DisableDismountInWarmode = value; } }

		/// <summary>
		/// Overridable. Event invoked when the Mobile is double clicked. By default, this method can either dismount or open the paperdoll.
		/// <seealso cref="CanPaperdollBeOpenedBy" />
		/// <seealso cref="DisplayPaperdollTo" />
		/// </summary>
		public virtual void OnDoubleClick( Mobile from )
		{
			if ( this == from && ( !m_DisableDismountInWarmode || !m_Warmode ) )
			{
				IMount mount = Mount;

				if ( mount != null )
				{
					mount.Rider = null;
					return;
				}
			}

			if ( CanPaperdollBeOpenedBy( from ) )
				DisplayPaperdollTo( from );
		}

		/// <summary>
		/// Overridable. Virtual event invoked when the Mobile is double clicked by someone who is over 18 tiles away.
		/// <seealso cref="OnDoubleClick" />
		/// </summary>
		public virtual void OnDoubleClickOutOfRange( Mobile from )
		{
		}

		/// <summary>
		/// Overridable. Virtual event invoked when the Mobile is double clicked by someone who can no longer see the Mobile. This may happen, for example, using 'Last Object' after the Mobile has hidden.
		/// <seealso cref="OnDoubleClick" />
		/// </summary>
		public virtual void OnDoubleClickCantSee( Mobile from )
		{
			from.LocalOverheadMessage( MessageType.Regular, 0x3B2, 1019045 ); // I can't reach that.
		}

		/// <summary>
		/// Overridable. Event invoked when the Mobile is double clicked by someone who is not alive. Similar to <see cref="OnDoubleClick" />, this method will show the paperdoll. It does not, however, provide any dismount functionality.
		/// <seealso cref="OnDoubleClick" />
		/// </summary>
		public virtual void OnDoubleClickDead( Mobile from )
		{
			if ( CanPaperdollBeOpenedBy( from ) )
				DisplayPaperdollTo( from );
		}

		/// <summary>
		/// Overridable. Event invoked when the Mobile requests to open his own paperdoll via the 'Open Paperdoll' macro.
		/// </summary>
		public virtual void OnPaperdollRequest()
		{
			if ( CanPaperdollBeOpenedBy( this ) )
				DisplayPaperdollTo( this );
		}

		public const int BodyWeight = 10;

		/// <summary>
		/// Overridable. Event invoked when <paramref name="from" /> wants to see this Mobile's stats.
		/// </summary>
		/// <param name="from"></param>
		public virtual void OnStatsQuery( Mobile from )
		{
			if ( from.Map == this.Map && from.CanSee( this ) && from.InUpdateRange( this ) )
				from.Send( new MobileStatus( from, this ) );

			if ( from == this )
				Send( new StatLockInfo( this ) );

			IParty ip = m_Party as IParty;

			if ( ip != null )
				ip.OnStatsQuery( from, this );
		}

		/// <summary>
		/// Overridable. Event invoked when <paramref name="from" /> wants to see this Mobile's skills.
		/// </summary>
		public virtual void OnSkillsQuery( Mobile from )
		{
			if ( from == this )
				Send( new SkillUpdate( m_Skills ) );
		}

		/// <summary>
		/// Overridable. Virtual event invoked when <see cref="Region" /> changes.
		/// </summary>
		public virtual void OnRegionChange( Region oldRegion, Region newRegion )
		{
		}

		private Item m_MountItem;

		[CommandProperty( AccessLevel.GameMaster )]
		public IMount Mount
		{
			get
			{
				IMountItem mountItem = null;

				if ( m_MountItem != null && !m_MountItem.Deleted && m_MountItem.Parent == this )
					mountItem = (IMountItem) m_MountItem;

				if ( mountItem == null )
					m_MountItem = ( mountItem = ( this.FindItemOnLayer( Layer.Mount ) as IMountItem ) ) as Item;

				return mountItem == null ? null : mountItem.Mount;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Mounted
		{
			get
			{
				return ( Mount != null );
			}
		}

		private QuestArrow m_QuestArrow;

		public QuestArrow QuestArrow
		{
			get
			{
				return m_QuestArrow;
			}
			set
			{
				if ( m_QuestArrow != value )
				{
					if ( m_QuestArrow != null )
						m_QuestArrow.Stop();

					m_QuestArrow = value;
				}
			}
		}

		private static string[] m_GuildTypes = new string[]
			{
				"",
				" (Chaos)",
				" (Order)"
			};

		public virtual bool CanTarget { get { return true; } }
		public virtual bool ClickTitle { get { return true; } }

		public virtual bool ShowFameTitle { get { return true; } }

		public virtual void DisruptiveAction()
		{
			if ( Meditating )
			{
				Meditating = false;
				SendLocalizedMessage( 500134 ); // You stop meditating.
			}
		}

		#region Armor
		public Item ShieldArmor
		{
			get
			{
				return this.FindItemOnLayer( Layer.TwoHanded ) as Item;
			}
		}

		public Item NeckArmor
		{
			get
			{
				return this.FindItemOnLayer( Layer.Neck ) as Item;
			}
		}

		public Item HandArmor
		{
			get
			{
				return this.FindItemOnLayer( Layer.Gloves ) as Item;
			}
		}

		public Item HeadArmor
		{
			get
			{
				return this.FindItemOnLayer( Layer.Helm ) as Item;
			}
		}

		public Item ArmsArmor
		{
			get
			{
				return this.FindItemOnLayer( Layer.Arms ) as Item;
			}
		}

		public Item LegsArmor
		{
			get
			{
				Item ar = this.FindItemOnLayer( Layer.InnerLegs ) as Item;

				if ( ar == null )
					ar = this.FindItemOnLayer( Layer.Pants ) as Item;

				return ar;
			}
		}

		public Item ChestArmor
		{
			get
			{
				Item ar = this.FindItemOnLayer( Layer.InnerTorso ) as Item;

				if ( ar == null )
					ar = this.FindItemOnLayer( Layer.Shirt ) as Item;

				return ar;
			}
		}

		public Item Talisman
		{
			get
			{
				return this.FindItemOnLayer( Layer.Talisman ) as Item;
			}
		}

		#endregion

		/// <summary>
		/// Gets or sets the maximum attainable value for <see cref="RawStr" />, <see cref="RawDex" />, and <see cref="RawInt" />.
		/// </summary>
		[CommandProperty( AccessLevel.GameMaster )]
		public int StatCap
		{
			get
			{
				return m_StatCap;
			}
			set
			{
				if ( m_StatCap != value )
				{
					m_StatCap = value;

					Delta( MobileDelta.StatCap );
				}
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Meditating
		{
			get
			{
				return m_Meditating;
			}
			set
			{
				m_Meditating = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool CanSwim
		{
			get
			{
				return m_CanSwim;
			}
			set
			{
				m_CanSwim = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool CantWalk
		{
			get
			{
				return m_CantWalk;
			}
			set
			{
				m_CantWalk = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool CanHearGhosts
		{
			get
			{
				return m_CanHearGhosts || AccessLevel >= AccessLevel.Counselor;
			}
			set
			{
				m_CanHearGhosts = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int RawStatTotal
		{
			get
			{
				return RawStr + RawDex + RawInt;
			}
		}

		public DateTime NextSpellTime
		{
			get
			{
				return m_NextSpellTime;
			}
			set
			{
				m_NextSpellTime = value;
			}
		}

		/// <summary>
		/// Overridable. Virtual event invoked when the sector this Mobile is in gets <see cref="Sector.Activate">activated</see>.
		/// </summary>
		public virtual void OnSectorActivate()
		{
		}

		/// <summary>
		/// Overridable. Virtual event invoked when the sector this Mobile is in gets <see cref="Sector.Deactivate">deactivated</see>.
		/// </summary>
		public virtual void OnSectorDeactivate()
		{
		}

		public bool Attack( Mobile m )
		{
			if ( !CanAttack( m ) )
				return false;

			Combatant = m;

			OnAttack( m );

			return true;
		}

		public virtual bool Move( Direction d )
		{
			if ( m_Deleted )
				return false;

			Map map = m_Map;

			if ( map == null )
				return false;

			BankBox box = FindBankNoCreate();

			if ( box != null && box.Opened )
				box.Close();

			Point3D newLocation = Location;
			Point3D oldLocation = newLocation;

			if ( ( m_Direction & Direction.Mask ) == ( d & Direction.Mask ) )
			{
				// We are actually moving (not just a direction change)

				if ( m_Spell != null && !m_Spell.OnCasterMoving( d ) )
					return false;

				if ( m_Paralyzed || m_Frozen )
				{
					SendLocalizedMessage( 500111 ); // You are frozen and can not move.

					return false;
				}

				int newZ;

				if ( CheckMovement( d, out newZ ) )
				{
					int x = m_Location.X, y = m_Location.Y;
					int oldX = x, oldY = y;
					int oldZ = m_Location.Z;

					switch ( d & Direction.Mask )
					{
						case Direction.North:
							--y;
							break;
						case Direction.Right:
							++x;
							--y;
							break;
						case Direction.East:
							++x;
							break;
						case Direction.Down:
							++x;
							++y;
							break;
						case Direction.South:
							++y;
							break;
						case Direction.Left:
							--x;
							++y;
							break;
						case Direction.West:
							--x;
							break;
						case Direction.Up:
							--x;
							--y;
							break;
					}

					newLocation.X = x;
					newLocation.Y = y;
					newLocation.Z = newZ;

					m_Pushing = false;

					var objectsAtOldPoint = map.GetObjectsAtWorldPoint( new Point2D( oldX, oldY ) ).ToArray();

					foreach ( var m in objectsAtOldPoint.OfType<Mobile>() )
					{
						if ( m != this && ( m.Z + 15 ) > oldZ && ( oldZ + 15 ) > m.Z && !m.OnMoveOff( this ) )
							return false;
					}

					foreach ( var item in objectsAtOldPoint.OfType<Item>() )
					{
						if ( ( item.Z == oldZ || ( ( item.Z + item.ItemData.Height ) >= oldZ && ( oldZ + 15 ) > item.Z ) ) && !item.OnMoveOff( this ) )
							return false;
					}

					var objectsAtNewPoint = map.GetObjectsAtWorldPoint( new Point2D( x, y ) ).ToArray();

					foreach ( var m in objectsAtNewPoint.OfType<Mobile>() )
					{
						if ( ( m.Z + 15 ) > newZ && ( newZ + 15 ) > m.Z && !m.OnMoveOver( this ) )
							return false;
					}

					foreach ( var item in objectsAtNewPoint.OfType<Item>() )
					{
						if ( item.AtWorldPoint( x, y ) && ( item.Z == newZ || ( ( item.Z + item.ItemData.Height ) >= newZ && ( newZ + 15 ) > item.Z ) ) && !item.OnMoveOver( this ) )
							return false;
					}

					if ( !Region.CanMove( this, d, newLocation, oldLocation, map ) )
						return false;

					if ( !InternalOnMove( d ) )
						return false;

					if ( m_FwdEnabled && m_NetState != null && m_AccessLevel < m_FwdAccessOverride )
					{
						if ( m_MoveRecords == null )
							m_MoveRecords = new Queue<MovementRecord>( 6 );

						while ( m_MoveRecords.Count > 0 )
						{
							MovementRecord r = (MovementRecord) m_MoveRecords.Peek();

							if ( r.Expired() )
								m_MoveRecords.Dequeue();
							else
								break;
						}

						if ( m_MoveRecords.Count >= m_FwdMaxSteps )
						{
							FastWalkEventArgs fw = new FastWalkEventArgs( m_NetState );
							EventSink.InvokeFastWalk( fw );

							if ( fw.Blocked )
								return false;
						}

						TimeSpan delay = ComputeMovementSpeed( d );

						DateTime end;

						if ( m_MoveRecords.Count > 0 )
							end = m_EndQueue + delay;
						else
							end = DateTime.UtcNow + delay;

						m_MoveRecords.Enqueue( MovementRecord.NewInstance( end ) );

						m_EndQueue = end;
					}

					m_LastMoveTime = DateTime.UtcNow;
					newLocation = new Point3D( x, y, newZ );
				}
				else
				{
					return false;
				}

				DisruptiveAction();
			}

			if ( m_NetState != null )
				m_NetState.Send( MovementAck.Instantiate( m_NetState.Sequence, this ) );//new MovementAck( m_NetState.Sequence, this ) );

			SetLocation( newLocation, false );
			SetDirection( d );

			ArrayList moveList = new ArrayList();

			foreach ( object o in map.GetObjectsInRange( m_Location, Mobile.GlobalMaxUpdateRange ) )
			{
				if ( o == this )
					continue;

				if ( o is Mobile )
				{
					moveList.Add( o );
				}
				else if ( o is Item )
				{
					Item item = (Item) o;

					if ( item.HandlesOnMovement )
						moveList.Add( item );
				}
			}

			Packet[] cache = m_MovingPacketCache;

			for ( int i = 0; i < cache.Length; ++i )
				Packet.Release( ref cache[i] );

			for ( int i = 0; i < moveList.Count; ++i )
			{
				object o = moveList[i];

				if ( o is Mobile )
				{
					Mobile m = (Mobile) moveList[i];
					NetState ns = m.NetState;

					if ( ns != null && m.CanSee( this ) && m.InUpdateRange( this ) )
					{
						int noto = Notoriety.Compute( m, this );
						Packet p = cache[noto];

						if ( p == null )
							p = cache[noto] = Packet.Acquire( GenericPackets.MobileMoving( this, noto ) );

						ns.Send( p );
					}

					m.OnMovement( this, oldLocation );
				}
				else if ( o is Item )
				{
					( (Item) o ).OnMovement( this, oldLocation );
				}
			}

			for ( int i = 0; i < cache.Length; ++i )
				Packet.Release( ref cache[i] );

			if ( moveList.Count > 0 )
				moveList.Clear();

			OnAfterMove( oldLocation );
			return true;
		}

		public void CastSpell( int spellId, Item book = null, IEntity target = null )
		{
			EventSink.InvokeCastSpellRequest( new CastSpellRequestEventArgs( this, spellId, book, target ) );
		}

		public void UseRacialAbility( int abilityId )
		{
			EventSink.InvokeRacialAbilityRequest( new RacialAbilityRequestEventArgs( this, abilityId ) );
		}

		public virtual void Use( Item item )
		{
			if ( item == null || item.Deleted )
				return;

			DisruptiveAction();

			if ( m_Spell != null && !m_Spell.OnCasterUsingObject( item ) )
				return;

			object root = item.RootParent;
			bool okay = false;

			if ( !this.InUpdateRange( item.GetWorldLocation() ) )
				item.OnDoubleClickOutOfRange( this );
			else if ( !CanSee( item ) || ( item.CheckLOSOnUse && !InLOS( item ) ) )
				item.OnDoubleClickCantSee( this );
			else if ( !item.IsAccessibleTo( this ) )
			{
				Region reg = Region.Find( item.GetWorldLocation(), item.Map );

				if ( reg == null || !reg.SendInaccessibleMessage( item, this ) )
					item.OnDoubleClickNotAccessible( this );
			}
			else if ( !CheckAlive( false ) )
				item.OnDoubleClickDead( this );
			else if ( item.InSecureTrade )
				item.OnDoubleClickSecureTrade( this );
			else if ( !AllowItemUse( item ) )
				okay = false;
			else if ( !item.CheckItemUse( this, item ) )
				okay = false;
			else if ( root != null && root is Mobile && ( (Mobile) root ).IsSnoop( this ) )
				item.OnSnoop( this );
			else if ( m_Region.OnDoubleClick( this, item ) )
				okay = true;

			if ( okay )
			{
				if ( !item.Deleted )
					item.OnItemUsed( this, item );

				if ( !item.Deleted )
					item.OnDoubleClick( this );
			}
		}

		public void Use( Mobile m )
		{
			if ( m == null || m.Deleted )
				return;

			DisruptiveAction();

			if ( m_Spell != null && !m_Spell.OnCasterUsingObject( m ) )
				return;

			if ( !this.InUpdateRange( m ) )
				m.OnDoubleClickOutOfRange( this );
			else if ( !CanSee( m ) || ( m.CheckLOSOnUse && !InLOS( m ) ) )
				m.OnDoubleClickCantSee( this );
			else if ( !CheckAlive( false ) )
				m.OnDoubleClickDead( this );
			else if ( m_Region.OnDoubleClick( this, m ) && !m.Deleted )
				m.OnDoubleClick( this );
		}

		public void Lift( Item item, int amount, out bool rejected, out LRReason reject )
		{
			rejected = true;
			reject = LRReason.Inspecific;

			if ( item == null )
				return;

			Mobile from = this;
			NetState state = m_NetState;

			if ( from.AccessLevel >= AccessLevel.GameMaster || DateTime.UtcNow >= from.NextActionTime )
			{
				if ( from.CheckAlive() )
				{
					from.DisruptiveAction();

					if ( from.Holding != null )
					{
						reject = LRReason.AreHolding;
					}
					else if ( from.AccessLevel < AccessLevel.GameMaster && !from.InRange( item.GetWorldLocation(), 2 ) )
					{
						reject = LRReason.OutOfRange;
					}
					else if ( !from.CanSee( item ) || !from.InLOS( item ) )
					{
						reject = LRReason.OutOfSight;
					}
					else if ( !item.VerifyMove( from ) )
					{
						reject = LRReason.CannotLift;
					}
					else if ( !item.IsAccessibleTo( from ) )
					{
						reject = LRReason.CannotLift;
					}
					else if ( item.CheckLift( from, item, ref reject ) )
					{
						object root = item.RootParent;

						if ( root != null && root is Mobile && !( (Mobile) root ).CheckNonlocalLift( from, item ) )
						{
							reject = LRReason.TryToSteal;
						}
						else if ( !from.OnDragLift( item ) || !item.OnDragLift( from ) )
						{
							reject = LRReason.Inspecific;
						}
						else if ( !from.CheckAlive() )
						{
							reject = LRReason.Inspecific;
						}
						else
						{
							if ( item.Parent != null && item.Parent is Container )
								( (Container) item.Parent ).FreePosition( item.GridLocation );

							item.SetLastMoved();

							if ( item.Spawner != null )
							{
								item.Spawner.Remove( item );
								item.Spawner = null;
							}

							if ( amount == 0 )
								amount = 1;

							if ( amount > item.Amount )
								amount = item.Amount;

							int oldAmount = item.Amount;
							//item.Amount = amount; //Set in LiftItemDupe

							if ( amount < oldAmount )
								LiftItemDupe( item, amount );

							InvokeItemLifted( new ItemLiftedEventArgs( item, amount ) );

							item.RecordBounce();
							item.OnItemLifted( from, item );
							item.Internalize();

							from.Holding = item;

							from.NextActionTime = DateTime.UtcNow + TimeSpan.FromSeconds( 0.5 );

							Point3D fixLoc = item.Location;
							Map fixMap = item.Map;
							bool shouldFix = ( item.Parent == null );

							if ( fixMap != null && shouldFix )
								fixMap.FixColumn( fixLoc.X, fixLoc.Y );

							reject = LRReason.Inspecific;
							rejected = false;
						}
					}
				}
				else
				{
					reject = LRReason.Inspecific;
				}
			}
			else
			{
				SendActionMessage();
				reject = LRReason.Inspecific;
			}

			if ( rejected && state != null )
			{
				state.Send( LiftRej.Instantiate( reject ) );

				if ( item.Parent is Item )
					state.Send( new ContainerContentUpdate( item ) );
				else if ( item.Parent is Mobile )
					state.Send( new EquipUpdate( item ) );
				else
					item.SendInfoTo( state );

				if ( ObjectPropertyListPacket.Enabled && item.Parent != null )
					state.Send( item.OPLPacket );
			}
		}

		public bool Drop( Item to, Point3D loc, byte gridloc )
		{
			Mobile from = this;
			Item item = from.Holding;

			if ( item == null )
				return false;

			from.Holding = null;
			bool bounced = false;

			item.SetLastMoved();

			if ( to == null || !item.DropToItem( from, to, loc, gridloc ) )
			{
				item.Bounce( from );
				bounced = true;
			}

			item.ClearBounce();

			InvokeItemDropped( new ItemDroppedEventArgs( item, bounced ) );

			return !bounced;
		}

		public bool Drop( Point3D loc )
		{
			Mobile from = this;
			Item item = from.Holding;

			if ( item == null )
				return false;

			from.Holding = null;
			bool bounced = false;

			item.SetLastMoved();

			if ( !item.DropToWorld( from, loc ) )
			{
				item.Bounce( from );
				bounced = true;
			}

			item.ClearBounce();

			InvokeItemDropped( new ItemDroppedEventArgs( item, bounced ) );

			return !bounced;
		}

		public bool Drop( Mobile to, Point3D loc )
		{
			Mobile from = this;
			Item item = from.Holding;

			if ( item == null )
				return false;

			from.Holding = null;
			bool bounced = false;

			item.SetLastMoved();

			if ( to == null || !item.DropToMobile( from, to, loc ) )
			{
				item.Bounce( from );
				bounced = true;
			}

			item.ClearBounce();

			InvokeItemDropped( new ItemDroppedEventArgs( item, bounced ) );

			return !bounced;
		}

		public void DoSpeech( string text, int[] keywords, MessageType type, int hue )
		{
			if ( m_Deleted || CommandSystem.Handle( this, text ) )
				return;

			int range = 15;

			switch ( type )
			{
				case MessageType.Regular:
					m_SpeechHue = hue;
					break;
				case MessageType.Emote:
					m_EmoteHue = hue;
					break;
				case MessageType.Whisper:
					m_WhisperHue = hue;
					range = 1;
					break;
				case MessageType.Yell:
					m_YellHue = hue;
					range = 18;
					break;
				case MessageType.Guild:
					{
						if ( GuildChat.Handler != null )
						{
							GuildChat.Handler.OnGuildMessage( this, text );
							return;
						}

						break;
					}
				case MessageType.Alliance:
					{
						if ( GuildChat.Handler != null )
						{
							GuildChat.Handler.OnAllianceMessage( this, text );
							return;
						}

						break;
					}
				case MessageType.GM:
					{
						if ( AccessLevel == AccessLevel.Player )
							return;

						var listeners = World.Mobiles.Where( m => m.AccessLevel >= AccessLevel.Counselor );

						if ( listeners.Any() )
							listeners.SendPacket( new UnicodeMessage( Serial, Body, MessageType.GM, SpeechHue, 3, Language, Name, text ) );

						break;
					}
				default:
					type = MessageType.Regular;
					break;
			}

			SpeechEventArgs regArgs = new SpeechEventArgs( this, text, type, hue, keywords );

			EventSink.InvokeSpeech( regArgs );
			m_Region.OnSpeech( regArgs );
			OnSaid( regArgs );

			if ( regArgs.Blocked )
				return;

			text = regArgs.Speech;

			if ( text == null || text.Length == 0 )
				return;

			if ( m_Hears == null )
				m_Hears = new ArrayList();
			else if ( m_Hears.Count > 0 )
				m_Hears.Clear();

			if ( m_OnSpeech == null )
				m_OnSpeech = new ArrayList();
			else if ( m_OnSpeech.Count > 0 )
				m_OnSpeech.Clear();

			ArrayList hears = m_Hears;
			ArrayList onSpeech = m_OnSpeech;

			if ( m_Map != null )
			{
				foreach ( object o in m_Map.GetObjectsInRange( m_Location, range ) )
				{
					if ( o is Mobile )
					{
						Mobile heard = (Mobile) o;

						if ( heard.CanSee( this ) && ( m_NoSpeechLOS || !heard.Player || heard.InLOS( this ) ) )
						{
							if ( heard.m_NetState != null )
								hears.Add( heard );

							if ( heard.HandlesOnSpeech( this ) )
								onSpeech.Add( heard );

							foreach ( var item in heard.GetEquippedItems() )
							{
								if ( item.HandlesOnSpeech )
									onSpeech.Add( item );

								if ( item is Container )
									AddSpeechItemsFrom( onSpeech, (Container) item );
							}
						}
					}
					else if ( o is Item )
					{
						if ( ( (Item) o ).HandlesOnSpeech )
							onSpeech.Add( o );

						if ( o is Container )
							AddSpeechItemsFrom( onSpeech, (Container) o );
					}
				}

				object mutateContext = null;
				string mutatedText = text;
				SpeechEventArgs mutatedArgs = null;

				if ( MutateSpeech( hears, ref mutatedText, ref mutateContext ) )
					mutatedArgs = new SpeechEventArgs( this, mutatedText, type, hue, new int[0] );

				CheckSpeechManifest();

				ProcessDelta();

				Packet regp = null;
				Packet mutp = null;

				for ( int i = 0; i < hears.Count; ++i )
				{
					Mobile heard = (Mobile) hears[i];

					if ( mutatedArgs == null || !CheckHearsMutatedSpeech( heard, mutateContext ) )
					{
						heard.OnSpeech( regArgs );

						NetState ns = heard.NetState;

						if ( ns != null )
						{
							if ( regp == null )
								regp = Packet.Acquire( new UnicodeMessage( this.Serial, Body, type, hue, 3, m_Language, Name, text ) );

							ns.Send( regp );
						}
					}
					else
					{
						heard.OnSpeech( mutatedArgs );

						NetState ns = heard.NetState;

						if ( ns != null )
						{
							if ( mutp == null )
								mutp = Packet.Acquire( new UnicodeMessage( this.Serial, Body, type, hue, 3, m_Language, Name, mutatedText ) );

							ns.Send( mutp );
						}
					}
				}

				Packet.Release( regp );
				Packet.Release( mutp );

				if ( onSpeech.Count > 1 )
					onSpeech.Sort( LocationComparer.GetInstance( this ) );

				for ( int i = 0; i < onSpeech.Count; ++i )
				{
					object obj = onSpeech[i];

					if ( obj is Mobile )
					{
						Mobile heard = (Mobile) obj;

						if ( mutatedArgs == null || !CheckHearsMutatedSpeech( heard, mutateContext ) )
							heard.OnSpeech( regArgs );
						else
							heard.OnSpeech( mutatedArgs );
					}
					else
					{
						Item item = (Item) obj;

						item.OnSpeech( regArgs );
					}
				}
			}
		}

		public void Animate( int action, int subAction = 0 )
		{
			InvokeAnimated( new AnimatedEventArgs( action, subAction ) );
		}

		public void Animate( int action, int frameCount, int repeatCount, bool forward, bool repeat, int delay )
		{
			Map map = this.Map;

			if ( map != null )
			{
				this.ProcessDelta();

				Packet p = null;

				foreach ( NetState state in map.GetClientsInRange( this.Location ) )
				{
					if ( state.Mobile.CanSee( this ) )
					{
						state.Mobile.ProcessDelta();

						if ( p == null )
							p = Packet.Acquire( new OldMobileAnimation( this, action, frameCount, repeatCount, forward, repeat, delay ) );

						state.Send( p );
					}
				}

				if ( p != null )
					p.Release();
			}
		}

		public virtual bool UseSkill( SkillName name )
		{
			return Skills.UseSkill( this, name );
		}

		public bool EquipItem( Item item )
		{
			if ( item == null || item.Deleted || !item.CanEquip( this ) )
				return false;

			if ( CheckEquip( item ) && OnEquip( item ) && item.OnEquip( this ) )
			{
				if ( m_Spell != null && !m_Spell.OnCasterEquiping( item ) )
					return false;

				AddItem( item );
				item.OnAfterEquip( this );
				return true;
			}

			return false;
		}

		public virtual bool IsGoodAligned()
		{
			return Karma >= 0 && !Murderer;
		}

		public virtual bool IsEvilAligned()
		{
			return !IsGoodAligned();
		}
	}
}
