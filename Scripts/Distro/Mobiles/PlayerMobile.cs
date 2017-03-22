using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Server.Accounting;
using Server.ContextMenus;
using Server.Engines.BuffIcons;
using Server.Engines.Collections;
using Server.Engines.Craft;
using Server.Engines.Guilds;
using Server.Engines.Help;
using Server.Engines.Housing;
using Server.Engines.Housing.Multis;
using Server.Engines.Loyalty;
using Server.Engines.PartySystem;
using Server.Engines.Quests;
using Server.Engines.Quests.HumilityCloak;
using Server.Events;
using Server.Factions;
using Server.Guilds;
using Server.Gumps;
using Server.Items;
using Server.Misc;
using Server.Multis;
using Server.Network;
using Server.Regions;
using Server.Spells;
using Server.Spells.Bard;
using Server.Spells.Bushido;
using Server.Spells.Fifth;
using Server.Spells.Mysticism;
using Server.Spells.Ninjitsu;
using Server.Spells.Seventh;
using Server.Spells.Spellweaving;
using Server.Targeting;

namespace Server.Mobiles
{
	#region Enums
	[Flags]
	public enum PlayerFlag
	{
		None = 0x00000000,

		// Legacy
		Glassblowing = 0x00000001,
		Masonry = 0x00000002,
		SandMining = 0x00000004,
		StoneMining = 0x00000008,
		ToggleMiningStone = 0x00000010,
		KarmaLocked = 0x00000020,
		AutoRenewInsurance = 0x00000040,
		UseOwnFilter = 0x00000080,
		PublicMyRunUO = 0x00000100,
		PagingSquelched = 0x00000200,
		Young = 0x00000400,

		// Samurai Empire
		AcceptGuildInvites = 0x00000800,
		DisplayChampionTitle = 0x00001000,

		// Mondain's Legacy
		Arcanist = 0x00002000,
		CanSummonFey = 0x00004000,
		CanSummonFiend = 0x00008000,
		FriendOfTheLibrary = 0x00010000,
		Bedlam = 0x00020000,

		// Stygian Abyss
		BasketWeaving = 0x00040000,
		SacredQuest = 0x00080000,
		CanBuyCarpets = 0x00100000,
		CanCraftPets = 0x00200000,
		GemsMining = 0x00400000,
		DisabledPvpWarning = 0x00800000,
		HasStatReward = 0x01000000
	}

	public enum NpcGuild
	{
		None,
		MagesGuild,
		WarriorsGuild,
		ThievesGuild,
		RangersGuild,
		HealersGuild,
		MinersGuild,
		MerchantsGuild,
		TinkersGuild,
		TailorsGuild,
		FishermensGuild,
		BardsGuild,
		BlacksmithsGuild
	}

	public enum GuildRank
	{
		None = 0,
		Ronin = 1,
		Member = 2,
		Emissary = 3,
		Warlord = 4,
		Leader = 5
	}

	public enum SolenFriendship
	{
		None,
		Red,
		Black
	}
	#endregion

	public class PlayerMobile : Mobile, IHonorTarget
	{
		private class CountAndTimeStamp
		{
			private int m_Count;
			private DateTime m_Stamp;

			public CountAndTimeStamp()
			{
			}

			public DateTime TimeStamp { get { return m_Stamp; } }
			public int Count
			{
				get { return m_Count; }
				set { m_Count = value; m_Stamp = DateTime.UtcNow; }
			}
		}

		private string m_LastPromotionCode;

		[CommandProperty( AccessLevel.Administrator )]
		public string LastPromotionCode { get { return m_LastPromotionCode; } set { m_LastPromotionCode = value; } }

		public DateTime m_LastLogin;

		[CommandProperty( AccessLevel.GameMaster )]
		public DateTime LastLogin { get { return m_LastLogin; } set { m_LastLogin = value; } }

		private int m_GuildRank;
		private bool m_NextEnhanceSuccess;

		[CommandProperty( AccessLevel.GameMaster )]
		public bool NextEnhanceSuccess { get { return m_NextEnhanceSuccess; } set { m_NextEnhanceSuccess = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int GuildRank { get { return m_GuildRank; } set { m_GuildRank = value; } }

		private DesignContext m_DesignContext;

		private NpcGuild m_NpcGuild;
		private DateTime m_NpcGuildJoinTime;
		private TimeSpan m_NpcGuildGameTime;
		private PlayerFlag m_Flags;
		private int m_StepsTaken;
		private int m_Profession;

		// Sacred Quest
		private DateTime m_SacredQuestNextChance;

		// Humility Cloak Quest
		private HumilityQuestStatus m_HumilityQuestStatus;
		private DateTime m_HumilityQuestNextChance;

		// Treasures of Tokuno
		private int m_ToTItemsTurnedIn;
		private double m_ToTTotalMonsterFame;

		// 10th Anniversary Artifacts
		private double m_TenthAnniversaryCredits;

		// Doom Artifacts
		private int m_DoomCredits;

		// Equip Last Weapon Client Macro
		private BaseWeapon m_LastEquipedWeapon;

		[CommandProperty( AccessLevel.GameMaster )]
		public BaseWeapon LastEquipedWeapon
		{
			get { return m_LastEquipedWeapon; }
			set { m_LastEquipedWeapon = value; }
		}

		// Collection Reward Titles
		private ArrayList m_CollectionTitles;
		private int m_CurrentCollectionTitle;

		public ArrayList CollectionTitles
		{
			get { return m_CollectionTitles; }
			set { m_CollectionTitles = value; }
		}

		public int CurrentCollectionTitle
		{
			get { return m_CurrentCollectionTitle; }
			set { m_CurrentCollectionTitle = value; InvalidateProperties(); }
		}

		// Variables para arreglo del Paralyzing Blow
		private DateTime m_NextPBlow;

		public DateTime NextPBlow
		{
			get { return m_NextPBlow; }
			set { m_NextPBlow = value; }
		}

		// Variables para arreglo del Disarm
		private DateTime m_NextDisarm;

		public DateTime NextDisarm
		{
			get { return m_NextDisarm; }
			set { m_NextDisarm = value; }
		}

		// Variables para arreglo del Bug de Explo y Arrow
		private DateTime m_LastExplo;
		private DateTime m_LastArrow;

		public DateTime LastExplo
		{
			get { return m_LastExplo; }
			set { m_LastExplo = value; }
		}

		public DateTime LastArrow
		{
			get { return m_LastArrow; }
			set { m_LastArrow = value; }
		}

		// AutoStable y AutoClaim
		private ArrayList m_AutoStabledPets;

		public ArrayList AutoStabledPets
		{
			get { return m_AutoStabledPets; }
			set { m_AutoStabledPets = value; }
		}

		// Items insured al morir el personaje (para reequip posterior)
		private List<Item> m_EquipInsuredItems;

		public List<Item> EquipInsuredItems
		{
			get { return m_EquipInsuredItems; }
			set { m_EquipInsuredItems = value; }
		}

		// Floor Traps
		private int m_FloorTrapsPlaced;

		public int FloorTrapsPlaced
		{
			get { return m_FloorTrapsPlaced; }
			set { m_FloorTrapsPlaced = value; }
		}

		// Fame award
		private bool m_BlocksFameAward;

		[CommandProperty( AccessLevel.GameMaster )]
		public bool BlocksFameAward
		{
			get { return m_BlocksFameAward; }
			set { m_BlocksFameAward = value; }
		}

		private LoyaltyInfo m_LoyaltyInfo;

		[CommandProperty( AccessLevel.GameMaster )]
		public LoyaltyInfo LoyaltyInfo
		{
			get { return m_LoyaltyInfo; }
			set { }
		}

		private TieredQuestInfo m_TieredQuestInfo;

		[CommandProperty( AccessLevel.GameMaster )]
		public TieredQuestInfo TieredQuestInfo
		{
			get { return m_TieredQuestInfo; }
			set { }
		}

		private DateTime m_LastForgedPardonUse;

		[CommandProperty( AccessLevel.GameMaster )]
		public DateTime LastForgedPardonUse
		{
			get { return m_LastForgedPardonUse; }
			set { m_LastForgedPardonUse = value; }
		}

		private DateTime m_NextTenthAnniversarySculptureUse;

		[CommandProperty( AccessLevel.GameMaster )]
		public DateTime NextTenthAnniversarySculptureUse
		{
			get { return m_NextTenthAnniversarySculptureUse; }
			set { m_NextTenthAnniversarySculptureUse = value; }
		}

		private DateTime m_NextAnkhPendantUse;

		[CommandProperty( AccessLevel.GameMaster )]
		public DateTime NextAnkhPendantUse
		{
			get { return m_NextAnkhPendantUse; }
			set { m_NextAnkhPendantUse = value; }
		}

		#region Starting Quest
		private byte m_KRStartingQuestStep;

		[CommandProperty( AccessLevel.GameMaster )]
		public byte KRStartingQuestStep
		{
			get { return m_KRStartingQuestStep; }
			set
			{
				m_KRStartingQuestStep = value;

				KRStartingQuest.DoStep( this );
			}
		}

		public void CheckKRStartingQuestStep( int step )
		{
			if ( m_KRStartingQuestStep < step && m_KRStartingQuestStep != 0 )
			{
				CloseGump( typeof( KRStartingQuestGump ) );
				CloseGump( typeof( KRStartingQuestCancelGump ) );

				KRStartingQuestStep = (byte) step;
			}
		}

		public void FinishKRStartingQuest()
		{
			m_KRStartingQuestStep = 0;
		}

		#endregion

		[CommandProperty( AccessLevel.GameMaster )]
		public override int Str
		{
			get
			{
				if ( base.Str > 150 )
					return 150;

				return base.Str;
			}
			set
			{
				if ( StatMods.Count == 0 )
					RawStr = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public override int Dex
		{
			get
			{
				if ( base.Dex > 150 )
					return 150;

				return base.Dex;
			}
			set
			{
				if ( StatMods.Count == 0 )
					RawDex = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public override int Int
		{
			get
			{
				if ( base.Int > 150 )
					return 150;

				return base.Int;
			}
			set
			{
				if ( StatMods.Count == 0 )
					RawInt = value;
			}
		}

		#region ML Quest System
		private List<BaseQuest> m_Quests;
		private Dictionary<QuestChain, BaseChain> m_Chains;

		public List<BaseQuest> Quests
		{
			get { return m_Quests; }
		}

		public Dictionary<QuestChain, BaseChain> Chains
		{
			get { return m_Chains; }
		}
		#endregion

		public AdvancedCharacterState ACState;

		public int Deaths;

		// Sacred Quest
		[CommandProperty( AccessLevel.GameMaster )]
		public DateTime SacredQuestNextChance
		{
			get { return m_SacredQuestNextChance; }
			set { m_SacredQuestNextChance = value; }
		}

		// Humility Cloak Quest
		[CommandProperty( AccessLevel.GameMaster )]
		public HumilityQuestStatus HumilityQuestStatus
		{
			get { return m_HumilityQuestStatus; }
			set { m_HumilityQuestStatus = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public DateTime HumilityQuestNextChance
		{
			get { return m_HumilityQuestNextChance; }
			set { m_HumilityQuestNextChance = value; }
		}

		// Treasures of Tokuno
		[CommandProperty( AccessLevel.GameMaster )]
		public int ToTItemsTurnedIn
		{
			get { return m_ToTItemsTurnedIn; }
			set { m_ToTItemsTurnedIn = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public double ToTTotalMonsterFame
		{
			get { return m_ToTTotalMonsterFame; }
			set { m_ToTTotalMonsterFame = value; }
		}

		// 10th Anniversary
		[CommandProperty( AccessLevel.GameMaster )]
		public double TenthAnniversaryCredits
		{
			get { return m_TenthAnniversaryCredits; }
			set { m_TenthAnniversaryCredits = value; }
		}

		// Doom Artifacts
		[CommandProperty( AccessLevel.GameMaster )]
		public int DoomCredits
		{
			get { return m_DoomCredits; }
			set { m_DoomCredits = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int Profession { get { return m_Profession; } set { m_Profession = value; } }

		public int StepsTaken { get { return m_StepsTaken; } set { m_StepsTaken = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public NpcGuild NpcGuild { get { return m_NpcGuild; } set { m_NpcGuild = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public DateTime NpcGuildJoinTime { get { return m_NpcGuildJoinTime; } set { m_NpcGuildJoinTime = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public TimeSpan NpcGuildGameTime { get { return m_NpcGuildGameTime; } set { m_NpcGuildGameTime = value; } }

		#region PlayerFlags
		public PlayerFlag Flags
		{
			get { return m_Flags; }
			set { m_Flags = value; }
		}

		// Default-Distro Flags
		[CommandProperty( AccessLevel.GameMaster )]
		public bool PagingSquelched
		{
			get { return GetFlag( PlayerFlag.PagingSquelched ); }
			set { SetFlag( PlayerFlag.PagingSquelched, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Glassblowing
		{
			get { return GetFlag( PlayerFlag.Glassblowing ); }
			set { SetFlag( PlayerFlag.Glassblowing, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Masonry
		{
			get { return GetFlag( PlayerFlag.Masonry ); }
			set { SetFlag( PlayerFlag.Masonry, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool SandMining
		{
			get { return GetFlag( PlayerFlag.SandMining ); }
			set { SetFlag( PlayerFlag.SandMining, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool StoneMining
		{
			get { return GetFlag( PlayerFlag.StoneMining ); }
			set { SetFlag( PlayerFlag.StoneMining, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool ToggleMiningStone
		{
			get { return GetFlag( PlayerFlag.ToggleMiningStone ); }
			set { SetFlag( PlayerFlag.ToggleMiningStone, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool KarmaLocked
		{
			get { return GetFlag( PlayerFlag.KarmaLocked ); }
			set { SetFlag( PlayerFlag.KarmaLocked, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool AutoRenewInsurance
		{
			get { return GetFlag( PlayerFlag.AutoRenewInsurance ); }
			set { SetFlag( PlayerFlag.AutoRenewInsurance, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool UseOwnFilter
		{
			get { return GetFlag( PlayerFlag.UseOwnFilter ); }
			set { SetFlag( PlayerFlag.UseOwnFilter, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool PublicMyRunUO
		{
			get { return GetFlag( PlayerFlag.PublicMyRunUO ); }
			set { SetFlag( PlayerFlag.PublicMyRunUO, value ); InvalidateMyRunUO(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool GemsMining
		{
			get { return GetFlag( PlayerFlag.GemsMining ); }
			set { SetFlag( PlayerFlag.GemsMining, value ); }
		}

		// Samurai Empire Flags
		[CommandProperty( AccessLevel.GameMaster )]
		public bool DisplayChampionTitle
		{
			get { return GetFlag( PlayerFlag.DisplayChampionTitle ); }
			set { SetFlag( PlayerFlag.DisplayChampionTitle, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool AcceptGuildInvites
		{
			get { return GetFlag( PlayerFlag.AcceptGuildInvites ); }
			set { SetFlag( PlayerFlag.AcceptGuildInvites, value ); }
		}

		// Mondain's Legacy Flags
		[CommandProperty( AccessLevel.GameMaster )]
		public bool Arcanist
		{
			get { return GetFlag( PlayerFlag.Arcanist ); }
			set { SetFlag( PlayerFlag.Arcanist, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool CanSummonFey
		{
			get { return GetFlag( PlayerFlag.CanSummonFey ); }
			set { SetFlag( PlayerFlag.CanSummonFey, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool CanSummonFiend
		{
			get { return GetFlag( PlayerFlag.CanSummonFiend ); }
			set { SetFlag( PlayerFlag.CanSummonFiend, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool FriendOfTheLibrary
		{
			get { return GetFlag( PlayerFlag.FriendOfTheLibrary ); }
			set { SetFlag( PlayerFlag.FriendOfTheLibrary, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Bedlam
		{
			get { return GetFlag( PlayerFlag.Bedlam ); }
			set { SetFlag( PlayerFlag.Bedlam, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool BasketWeaving
		{
			get { return GetFlag( PlayerFlag.BasketWeaving ); }
			set { SetFlag( PlayerFlag.BasketWeaving, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool SacredQuest
		{
			get { return GetFlag( PlayerFlag.SacredQuest ); }
			set { SetFlag( PlayerFlag.SacredQuest, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool CanBuyCarpets
		{
			get { return GetFlag( PlayerFlag.CanBuyCarpets ); }
			set { SetFlag( PlayerFlag.CanBuyCarpets, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool CanCraftPets
		{
			get { return GetFlag( PlayerFlag.CanCraftPets ); }
			set { SetFlag( PlayerFlag.CanCraftPets, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool DisabledPvpWarning
		{
			get { return GetFlag( PlayerFlag.DisabledPvpWarning ); }
			set { SetFlag( PlayerFlag.DisabledPvpWarning, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool HasStatReward
		{
			get { return GetFlag( PlayerFlag.HasStatReward ); }
			set { SetFlag( PlayerFlag.HasStatReward, value ); }
		}
		#endregion

		private DateTime m_AnkhNextUse;

		[CommandProperty( AccessLevel.GameMaster )]
		public DateTime AnkhNextUse
		{
			get { return m_AnkhNextUse; }
			set { m_AnkhNextUse = value; }
		}

		private DateTime m_NextGemOfSalvationUse;

		[CommandProperty( AccessLevel.GameMaster )]
		public DateTime NextGemOfSalvationUse
		{
			get { return m_NextGemOfSalvationUse; }
			set { m_NextGemOfSalvationUse = value; }
		}

		private DateTime m_NextSilverSaplingUse;

		[CommandProperty( AccessLevel.GameMaster )]
		public DateTime NextSilverSaplingUse
		{
			get { return m_NextSilverSaplingUse; }
			set { m_NextSilverSaplingUse = value; }
		}

		#region Bard Masteries
		private BardMastery m_BardMastery;
		private ResistanceType m_BardElementDamage;
		private DateTime m_NextBardMasterySwitch;
		private int m_BardMasteryLearnedMask;

		[CommandProperty( AccessLevel.GameMaster )]
		public ResistanceType BardElementDamage
		{
			get { return m_BardElementDamage; }
			set { m_BardElementDamage = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int BardMasteryLearnedMask
		{
			get { return m_BardMasteryLearnedMask; }
			set { m_BardMasteryLearnedMask = value; }
		}

		public BardMastery BardMastery
		{
			get { return m_BardMastery; }
			set
			{
				m_BardMastery = value;
				m_BardMasteryLearnedMask |= m_BardMastery.Mask;
			}
		}

		public DateTime NextBardMasterySwitch
		{
			get { return m_NextBardMasterySwitch; }
			set { m_NextBardMasterySwitch = value; }
		}

		public bool HasLearnedBardMastery( BardMastery mastery )
		{
			return ( m_BardMasteryLearnedMask & mastery.Mask ) != 0;
		}
		#endregion

		public static Direction GetDirection4( Point3D from, Point3D to )
		{
			int dx = from.X - to.X;
			int dy = from.Y - to.Y;

			int rx = dx - dy;
			int ry = dx + dy;

			Direction ret;

			if ( rx >= 0 && ry >= 0 )
				ret = Direction.West;
			else if ( rx >= 0 && ry < 0 )
				ret = Direction.South;
			else if ( rx < 0 && ry < 0 )
				ret = Direction.East;
			else
				ret = Direction.North;

			return ret;
		}

		public override bool OnDroppedItemToWorld( Item item, Point3D location )
		{
			if ( !base.OnDroppedItemToWorld( item, location ) )
				return false;

			BounceInfo bi = item.GetBounce();

			if ( bi != null )
			{
				Type type = item.GetType();

				if ( type.IsDefined( typeof( FurnitureAttribute ), true ) || type.IsDefined( typeof( DynamicFlipingAttribute ), true ) )
				{
					object[] objs = type.GetCustomAttributes( typeof( FlipableAttribute ), true );

					if ( objs != null && objs.Length > 0 )
					{
						FlipableAttribute fp = objs[0] as FlipableAttribute;

						if ( fp != null )
						{
							int[] itemIDs = fp.ItemIDs;

							Point3D oldWorldLoc = bi.m_WorldLoc;
							Point3D newWorldLoc = location;

							if ( oldWorldLoc.X != newWorldLoc.X || oldWorldLoc.Y != newWorldLoc.Y )
							{
								Direction dir = GetDirection4( oldWorldLoc, newWorldLoc );

								if ( itemIDs.Length == 2 )
								{
									switch ( dir )
									{
										case Direction.North:
										case Direction.South:
											item.ItemID = itemIDs[0];
											break;
										case Direction.East:
										case Direction.West:
											item.ItemID = itemIDs[1];
											break;
									}
								}
								else if ( itemIDs.Length == 4 )
								{
									switch ( dir )
									{
										case Direction.South:
											item.ItemID = itemIDs[0];
											break;
										case Direction.East:
											item.ItemID = itemIDs[1];
											break;
										case Direction.North:
											item.ItemID = itemIDs[2];
											break;
										case Direction.West:
											item.ItemID = itemIDs[3];
											break;
									}
								}
							}
						}
					}
				}
			}

			return true;
		}

		private bool m_Flying;

		[CommandProperty( AccessLevel.GameMaster )]
		public override bool Flying
		{
			get { return m_Flying; }
			set
			{
				if ( m_Flying != value )
				{
					m_Flying = value;
					Delta( MobileDelta.Flags );

					if ( !m_Flying )
						this.RemoveBuff( BuffIcon.Flying );
					else
						BuffInfo.AddBuff( this, new BuffInfo( BuffIcon.Flying, 1112193, 1112567 ) ); // Flying & You are flying.
				}
			}
		}

		public override int GetPacketFlags()
		{
			int flags = base.GetPacketFlags();

			if ( m_Flying )
				flags |= 0x04;

			return flags;
		}

		public bool GetFlag( PlayerFlag flag )
		{
			return ( ( m_Flags & flag ) != 0 );
		}

		public void SetFlag( PlayerFlag flag, bool value )
		{
			if ( value )
				m_Flags |= flag;
			else
				m_Flags &= ~flag;
		}

		public DesignContext DesignContext
		{
			get { return m_DesignContext; }
			set { m_DesignContext = value; }
		}

		public static void Initialize()
		{
			if ( FastwalkPrevention )
			{
				PacketHandler ph = PacketHandlers.Instance.GetHandler( 0x02 );

				ph.ThrottleCallback = new ThrottlePacketCallback( MovementThrottle_Callback );
			}

			EventSink.Login += new LoginEventHandler( OnLogin );
			EventSink.Logout += new LogoutEventHandler( OnLogout );
			EventSink.Connected += new ConnectedEventHandler( EventSink_Connected );
			EventSink.Disconnected += new DisconnectedEventHandler( EventSink_Disconnected );
		}

		public override void OnSkillInvalidated( Skill skill )
		{
			if ( skill.SkillName == SkillName.MagicResist )
				UpdateResistances();
		}

		public override int GetMaxResistance( ResistanceType type )
		{
			int max = base.GetMaxResistance( type );

			if ( type != ResistanceType.Physical && 60 < max && Spells.Fourth.CurseSpell.UnderEffect( this ) )
				max = 60;

			if ( StoneFormSpell.UnderEffect( this ) )
				max += StoneFormSpell.GetResistCapBonus( this );

			return max;
		}

		private int m_LastGlobalLight = -1, m_LastPersonalLight = -1;

		public override void OnClientChanged()
		{
			m_LastGlobalLight = -1;
			m_LastPersonalLight = -1;
		}

		public override void ComputeBaseLightLevels( out int global, out int personal )
		{
			global = LightCycle.ComputeLevelFor( this );

			if ( this.LightLevel < 21 && ( GetMagicalAttribute( MagicalAttribute.NightSight ) > 0 || this.Race == Race.Elf || this.AccessLevel >= AccessLevel.GameMaster ) )
				personal = 21;
			else
				personal = this.LightLevel;
		}

		public override void CheckLightLevels( bool forceResend )
		{
			NetState ns = this.NetState;

			if ( ns == null )
				return;

			int global, personal;

			ComputeLightLevels( out global, out personal );

			if ( !forceResend )
				forceResend = ( global != m_LastGlobalLight || personal != m_LastPersonalLight );

			if ( !forceResend )
				return;

			m_LastGlobalLight = global;
			m_LastPersonalLight = personal;

			ns.Send( GlobalLightLevel.Instantiate( global ) );
			ns.Send( new PersonalLightLevel( this, personal ) );
		}

		public override int GetMinResistance( ResistanceType type )
		{
			int magicResist = (int) ( Skills[SkillName.MagicResist].Value * 10 );
			int min = int.MinValue;

			if ( magicResist >= 1000 )
				min = 40 + ( ( magicResist - 1000 ) / 50 );
			else if ( magicResist >= 400 )
				min = ( magicResist - 400 ) / 15;

			if ( min > MaxPlayerResistance )
				min = MaxPlayerResistance;

			int baseMin = base.GetMinResistance( type );

			if ( min < baseMin )
				min = baseMin;

			return min;
		}

		public static void CheckTitlesAtrophy( Mobile from )
		{
			PlayerMobile pm = from as PlayerMobile;

			if ( pm == null )
				return;

			try
			{
				if ( ( pm.LastTierLoss + TimeSpan.FromDays( 1.0 ) ) < DateTime.UtcNow ) // TODO: verify
				{
					for ( int i = 0; i < pm.ChampionTiers.Length; i++ )
					{
						pm.ChampionTiers[i]--; // TODO: verify

						if ( pm.ChampionTiers[i] < 0 )
							pm.ChampionTiers[i] = 0;
					}

					pm.LastTierLoss = DateTime.UtcNow;
				}

				if ( ( pm.LastChampionTierLoss + TimeSpan.FromDays( 7.0 ) ) < DateTime.UtcNow ) // TODO: verify
				{
					for ( int i = 1; i < pm.SuperChampionTiers.Length; i++ )
					{
						pm.SuperChampionTiers[i]--; // TODO: verify

						if ( pm.SuperChampionTiers[i] < 0 )
							pm.SuperChampionTiers[i] = 0;
					}

					pm.LastChampionTierLoss = DateTime.UtcNow;
				}

				if ( ( pm.LastSuperChampionTierLoss + TimeSpan.FromDays( 30.0 ) ) < DateTime.UtcNow ) // // TODO: verify
				{
					pm.SuperChampionTiers[0]--; // TODO: verify

					if ( pm.SuperChampionTiers[0] < 0 )
						pm.SuperChampionTiers[0] = 0;

					pm.LastSuperChampionTierLoss = DateTime.UtcNow;
				}
			}
			catch
			{
			}
		}

		private static void AutoClaim( Mobile from )
		{
			if ( !from.Region.AllowAutoClaim( from ) )
				return;

			ArrayList list = new ArrayList();

			for ( int i = 0; i < from.Stabled.Count; ++i )
			{
				BaseCreature pet = from.Stabled[i] as BaseCreature;

				if ( ( pet == null || pet.Deleted ) )
				{
					pet.IsStabled = false;
					from.Stabled.RemoveAt( i );
					--i;
					continue;
				}

				if ( ( (PlayerMobile) from ).m_AutoStabledPets != null )
				{
					if ( ( (PlayerMobile) from ).m_AutoStabledPets.Contains( pet ) )
					{
						list.Add( pet );
						( (PlayerMobile) from ).m_AutoStabledPets.Remove( pet );
					}
				}
			}

			( (PlayerMobile) from ).m_AutoStabledPets = null;

			for ( int i = 0; i < list.Count; ++i )
			{
				BaseCreature pet = list[i] as BaseCreature;

				if ( pet == null || pet.Deleted )
					continue;

				if ( ( from.Followers + pet.ControlSlots ) <= from.FollowersMax )
				{
					pet.SetControlMaster( from );

					if ( pet.Summoned )
						pet.SummonMaster = from;

					pet.ControlTarget = from;
					pet.ControlOrder = OrderType.Follow;

					pet.MoveToWorld( from.Location, from.Map );

					pet.IsStabled = false;
					from.Stabled.Remove( pet );
				}
			}
		}

		private static void AutoStable( Mobile from )
		{
			ArrayList list = new ArrayList();

			foreach ( Mobile m in World.Instance.Mobiles )
			{
				BaseCreature creature = m as BaseCreature;

				if ( creature != null && creature.Controlled && creature.ControlMaster == from &&
					/*!creature.IsDeadPet &&*/ !creature.Summoned && !creature.Body.IsHuman &&
					!( ( creature is PackLlama || creature is PackHorse || creature is Beetle ) && ( creature.Backpack != null && creature.Backpack.Items.Count > 0 ) ) &&
					!( creature.Combatant != null && creature.InRange( creature.Combatant, 12 ) && creature.Map == creature.Combatant.Map ) )
				{
					if ( creature is BaseMount )
					{
						if ( ( (BaseMount) creature ).Rider == null )
							list.Add( creature );
					}
					else
					{
						list.Add( creature );
					}
				}

			}

			( (PlayerMobile) from ).m_AutoStabledPets = new ArrayList();

			for ( int i = 0; i < list.Count; ++i )
			{
				BaseCreature pet = list[i] as BaseCreature;

				if ( pet == null || pet.Deleted )
					continue;

				if ( pet != null && !pet.Deleted )
				{
					( (PlayerMobile) from ).m_AutoStabledPets.Add( pet );
				}
				pet.ControlTarget = null;
				pet.ControlOrder = OrderType.Stay;
				pet.Internalize();

				pet.SetControlMaster( null );
				pet.SummonMaster = null;

				pet.IsStabled = true;
				from.Stabled.Add( pet );
			}
		}

		private static void OnLogin( LoginEventArgs e )
		{
			Mobile from = e.Mobile;

			SacrificeVirtue.CheckAtrophy( from );
			JusticeVirtue.CheckAtrophy( from );
			CompassionVirtue.CheckAtrophy( from );
			HonorVirtue.CheckAtrophy( from );
			ValorVirtue.CheckAtrophy( from );

			CheckTitlesAtrophy( from );

			AutoClaim( from );

			if ( AccountHandler.LockdownLevel > AccessLevel.Player )
			{
				string notice;

				Accounting.Account acct = from.Account as Accounting.Account;

				if ( acct == null || !acct.HasAccess( from.NetState ) )
				{
					if ( from.AccessLevel == AccessLevel.Player )
						notice = "The server is currently under lockdown. No players are allowed to log in at this time.";
					else
						notice = "The server is currently under lockdown. You do not have sufficient access level to connect.";

					Timer.DelayCall( TimeSpan.FromSeconds( 1.0 ), new TimerStateCallback( Disconnect ), from );
				}
				else if ( from.AccessLevel >= AccessLevel.Administrator )
				{
					notice = "The server is currently under lockdown. As you are an administrator, you may change this from the [Admin gump.";
				}
				else
				{
					notice = "The server is currently under lockdown. You have sufficient access level to connect.";
				}

				from.SendGump( new NoticeGump( 1060637, 30720, notice, 0xFFC000, 300, 140, null, null ) );
			}

			if ( from is PlayerMobile && from.Race != Race.Gargoyle )
			{
				PlayerMobile pm = (PlayerMobile) from;

				if ( pm.NetState != null && pm.NetState.Version != null && pm.NetState.Version.IsEnhanced )
				{
					if ( pm.LastLogin == DateTime.MinValue )
						pm.KRStartingQuestStep++;
					else if ( pm.KRStartingQuestStep > 0 )
						KRStartingQuest.DoStep( pm );
				}
				else if ( pm.KRStartingQuestStep > 0 )
					pm.KRStartingQuestStep = 0;
			}
		}

		private bool m_NoDeltaRecursion;

		public void ValidateEquipment()
		{
			if ( m_NoDeltaRecursion || Map == null || Map == Map.Internal )
				return;

			m_NoDeltaRecursion = true;
			Timer.DelayCall( TimeSpan.Zero, new TimerCallback( ValidateEquipment_Sandbox ) );
		}

		private void ValidateEquipment_Sandbox()
		{
			try
			{
				if ( Map == null || Map == Map.Internal )
					return;

				bool moved = false;

				int str = this.Str;
				int dex = this.Dex;
				int intel = this.Int;

				#region Factions
				int factionItemCount = 0;
				#endregion

				Mobile from = this;

				foreach ( var item in from.GetEquippedItems() )
				{
					if ( item is BaseWeapon )
					{
						BaseWeapon weapon = (BaseWeapon) item;

						if ( str < AOS.Scale( weapon.StrengthReq, 100 - weapon.GetLowerStatReq() ) )
						{
							string name = weapon.Name;

							if ( name == null )
								name = String.Format( "#{0}", weapon.LabelNumber );

							from.SendLocalizedMessage( 1062001, name ); // You can no longer wield your ~1_WEAPON~
							from.AddToBackpack( weapon );
							moved = true;
						}
					}
					else if ( item is BaseArmor )
					{
						BaseArmor armor = (BaseArmor) item;

						bool drop = false;

						if ( !armor.AllowMaleWearer && from.Body.IsMale && from.AccessLevel < AccessLevel.GameMaster )
						{
							drop = true;
						}
						else if ( !armor.AllowFemaleWearer && from.Body.IsFemale && from.AccessLevel < AccessLevel.GameMaster )
						{
							drop = true;
						}
						else
						{
							int strBonus = armor.ComputeStatBonus( StatType.Str );
							int strReq = armor.ComputeStrReq();

							if ( str < strReq || ( str + strBonus ) < 1 )
								drop = true;
						}

						if ( drop )
						{
							string name = armor.Name;

							if ( name == null )
								name = String.Format( "#{0}", armor.LabelNumber );

							if ( armor is BaseShield )
								from.SendLocalizedMessage( 1062003, name ); // You can no longer equip your ~1_SHIELD~
							else
								from.SendLocalizedMessage( 1062002, name ); // You can no longer wear your ~1_ARMOR~

							from.AddToBackpack( armor );
							moved = true;
						}
					}

					FactionItem factionItem = FactionItem.Find( item );

					if ( factionItem != null )
					{
						bool drop = false;

						Faction ourFaction = Faction.Find( this );

						if ( ourFaction == null || ourFaction != factionItem.Faction )
							drop = true;
						else if ( ++factionItemCount > FactionItem.GetMaxWearables( this ) )
							drop = true;

						if ( drop )
						{
							from.AddToBackpack( item );
							moved = true;
						}
					}
				}

				if ( moved )
					from.SendLocalizedMessage( 500647 ); // Some equipment has been moved to your backpack.
			}
			catch ( Exception e )
			{
				Console.WriteLine( e );
			}
			finally
			{
				m_NoDeltaRecursion = false;
			}
		}

		public override void Delta( MobileDelta flag )
		{
			base.Delta( flag );

			if ( ( flag & MobileDelta.Stat ) != 0 )
				ValidateEquipment();

			if ( ( flag & ( MobileDelta.Name | MobileDelta.Hue ) ) != 0 )
				InvalidateMyRunUO();
		}

		private static void Disconnect( object state )
		{
			NetState ns = ( (Mobile) state ).NetState;

			if ( ns != null )
				ns.Dispose();
		}

		private static void OnLogout( LogoutEventArgs e )
		{
			Mobile from = e.Mobile;

			if ( from.Alive )
				AutoStable( from );
		}

		private static void EventSink_Connected( ConnectedEventArgs e )
		{
			PlayerMobile pm = e.Mobile as PlayerMobile;

			if ( pm != null )
			{
				pm.m_SessionStart = DateTime.UtcNow;

				if ( pm.m_Quest != null )
					pm.m_Quest.StartTimer();

				pm.BedrollLogout = false;
			}

			#region SpecialMoves
			Timer.DelayCall( TimeSpan.Zero, () =>
				{
					SpecialMove.ClearAllMoves( e.Mobile );
				} );
			#endregion
		}

		private static void EventSink_Disconnected( DisconnectedEventArgs e )
		{
			Mobile from = e.Mobile;
			DesignContext context = DesignContext.Find( from );

			if ( context != null )
			{
				/* Client disconnected
				 *  - Remove design context
				 *  - Eject all from house
				 *  - Restore relocated entities
				 */

				// Remove design context
				DesignContext.Remove( from );

				// Eject all from house
				from.RevealingAction();

				foreach ( Item item in context.Foundation.GetItems() )
					item.Location = context.Foundation.BanLocation;

				foreach ( Mobile mobile in context.Foundation.GetMobiles() )
					mobile.Location = context.Foundation.BanLocation;

				// Restore relocated entities
				context.Foundation.RestoreRelocatedEntities();
			}

			PlayerMobile pm = e.Mobile as PlayerMobile;

			if ( pm != null )
			{
				pm.m_GameTime += ( DateTime.UtcNow - pm.m_SessionStart );

				if ( pm.m_Quest != null )
					pm.m_Quest.StopTimer();

				pm.m_SpeechLog = null;
				pm.LastLogin = DateTime.UtcNow;
			}
		}

		public override void RevealingAction()
		{
			if ( m_DesignContext != null )
				return;

			Spells.Sixth.InvisibilitySpell.RemoveTimer( this );

			base.RevealingAction();
		}

		public override void OnWarmodeChanged()
		{
			if ( m_KRStartingQuestStep == 16 )
				CheckKRStartingQuestStep( 17 );
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public override bool Hidden
		{
			get
			{
				return base.Hidden;
			}
			set
			{
				base.Hidden = value;

				this.RemoveBuff( BuffIcon.Invisibility ); // Always remove, default to the hiding icon EXCEPT in the invis spell where it's explicitly set

				if ( !Hidden )
					this.RemoveBuff( BuffIcon.HidingAndOrStealth );
				else
					BuffInfo.AddBuff( this, new BuffInfo( BuffIcon.HidingAndOrStealth, 1075655 ) );	// Hidden/Stealthing & You Are Hidden
			}
		}

		public override void OnSubItemAdded( Item item )
		{
			if ( AccessLevel < AccessLevel.GameMaster && item.IsChildOf( this.Backpack ) )
			{
				int maxWeight = WeightOverloading.GetMaxWeight( this );
				int curWeight = Mobile.BodyWeight + this.TotalWeight;

				if ( curWeight > maxWeight )
					this.SendLocalizedMessage( 1019035, true, String.Format( " : {0} / {1}", curWeight, maxWeight ) );
			}
		}

		public override bool CanBeHarmful( Mobile target, bool message, bool ignoreOurBlessedness )
		{
			if ( m_DesignContext != null || ( target is PlayerMobile && ( (PlayerMobile) target ).m_DesignContext != null ) )
				return false;

			if ( ( target is BaseVendor && ( (BaseVendor) target ).IsInvulnerable ) || target is PlayerVendor || target is TownCrier )
			{
				if ( message )
				{
					if ( target.Title == null )
						SendMessage( "{0} the vendor cannot be harmed.", target.Name );
					else
						SendMessage( "{0} {1} cannot be harmed.", target.Name, target.Title );
				}

				return false;
			}

			return base.CanBeHarmful( target, message, ignoreOurBlessedness );
		}

		public override bool CanBeBeneficial( Mobile target, bool message, bool allowDead )
		{
			if ( m_DesignContext != null || ( target is PlayerMobile && ( (PlayerMobile) target ).m_DesignContext != null ) )
				return false;

			return base.CanBeBeneficial( target, message, allowDead );
		}

		public override bool CheckContextMenuDisplay( IEntity target )
		{
			return ( m_DesignContext == null );
		}

		public override void OnItemAdded( Item item )
		{
			base.OnItemAdded( item );

			if ( item is BaseArmor || item is BaseWeapon )
				Hits = Hits;
			Stam = Stam;
			Mana = Mana;

			if ( this.NetState != null )
				CheckLightLevels( false );

			InvalidateMyRunUO();
		}

		public override void OnItemRemoved( Item item )
		{
			base.OnItemRemoved( item );

			if ( item is BaseArmor || item is BaseWeapon )
				Hits = Hits;
			Stam = Stam;
			Mana = Mana;

			if ( this.NetState != null )
				CheckLightLevels( false );

			InvalidateMyRunUO();
		}

		public void InvalidateProps()
		{
			if ( TestCenter.Enabled )
				InvalidateProperties();

			if ( NetState != null && NetState.Version.IsEnhanced )
				Delta( MobileDelta.Attributes );
		}

		#region [Stats]Max
		[CommandProperty( AccessLevel.GameMaster )]
		public override int HitsMax
		{
			get
			{
				int strBase = this.Str;
				int strOffs = GetMagicalAttribute( MagicalAttribute.BonusHits );

				if ( strOffs > 25 )
					strOffs = 25;

				if ( this.BodyMod == 0x19 || this.BodyMod == 0xF6 )
					strOffs += 20;

				return ( strBase / 2 ) + 50 + strOffs;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public override int StamMax
		{
			get { return base.StamMax + GetMagicalAttribute( MagicalAttribute.BonusStam ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public override int ManaMax
		{
			get
			{
				int mana = base.ManaMax + GetMagicalAttribute( MagicalAttribute.BonusMana );

				// Bonus +20 de mana por ser elfo.
				if ( Race == Race.Elf )
					mana += 20;

				return mana;
			}
		}
		#endregion

		public override void OnFinishMeditation()
		{
			BuffInfo.RemoveBuff( this, BuffIcon.ActiveMeditation );
		}

		public override bool HasFreeMovement()
		{
			if ( Spells.Necromancy.WraithFormSpell.UnderEffect( this ) )
				return true;

			return base.HasFreeMovement();
		}

		public override bool Move( Direction d )
		{
			NetState ns = this.NetState;

			if ( ns != null )
			{
				if ( HasGump( typeof( ResurrectGump ) ) )
				{
					if ( Alive )
					{
						CloseGump( typeof( ResurrectGump ) );
					}
					else
					{
						SendLocalizedMessage( 500111 ); // You are frozen and cannot move.
						return false;
					}
				}
			}

			TimeSpan speed = ComputeMovementSpeed( d );

			if ( !base.Move( d ) )
				return false;

			m_NextMovementTime = DateTime.UtcNow + speed;

			return true;
		}

		public override bool CheckMovement( Direction d, out int newZ )
		{
			DesignContext context = m_DesignContext;

			if ( context == null )
				return base.CheckMovement( d, out newZ );

			HouseFoundation foundation = context.Foundation;

			newZ = foundation.Z + HouseFoundation.GetLevelZ( context.Level, foundation );

			int newX = this.X, newY = this.Y;
			Movement.Movement.Offset( d, ref newX, ref newY );

			int startX = foundation.X + foundation.Components.Min.X + 1;
			int startY = foundation.Y + foundation.Components.Min.Y + 1;
			int endX = startX + foundation.Components.Width - 1;
			int endY = startY + foundation.Components.Height - 2;

			return ( newX >= startX && newY >= startY && newX < endX && newY < endY && Map == foundation.Map );
		}

		public override bool AllowItemUse( Item item )
		{
			return DesignContext.Check( this );
		}

		public override bool AllowSkillUse( SkillName skill )
		{
			if ( !DesignContext.Check( this ) )
				return false;

			if ( AnimalForm.UnderTransformation( this ) )
			{
				if ( skill == SkillName.Stealth || skill == SkillName.DetectHidden
					|| skill == SkillName.Anatomy || skill == SkillName.Hiding
					|| skill == SkillName.AnimalLore || skill == SkillName.EvalInt )
				{
					return true;
				}

				SendLocalizedMessage( 1070771 ); // You cannot use that skill in this form.
				return false;
			}

			if ( !CanBeginAction( typeof( Engines.Imbuing.Imbuing ) ) )
			{
				SendSkillMessage();
				return false;
			}

			return true;
		}

		private bool m_LastProtectedMessage;
		private int m_NextProtectionCheck = 10;

		public virtual void RecheckTownProtection()
		{
			m_NextProtectionCheck = 10;

			Regions.GuardedRegion reg = (Regions.GuardedRegion) this.Region.GetRegion( typeof( Regions.GuardedRegion ) );
			bool isProtected = ( reg != null && !reg.IsDisabled() );

			if ( isProtected != m_LastProtectedMessage )
			{
				if ( isProtected )
					SendLocalizedMessage( 500112 ); // You are now under the protection of the town guards.
				else
					SendLocalizedMessage( 500113 ); // You have left the protection of the town guards.

				m_LastProtectedMessage = isProtected;
			}
		}

		public override void MoveToWorld( Point3D loc, Map map )
		{
			base.MoveToWorld( loc, map );

			RecheckTownProtection();
		}

		public override void SetLocation( Point3D loc, bool isTeleport )
		{
			if ( !isTeleport && AccessLevel == AccessLevel.Player )
			{
				// moving, not teleporting
				int zDrop = ( this.Location.Z - loc.Z );

				if ( zDrop > 20 ) // we fell more than one story
					Hits -= ( ( zDrop / 20 ) * 10 ) - 5; // deal some damage; does not kill, disrupt, etc
			}

			base.SetLocation( loc, isTeleport );

			if ( isTeleport || --m_NextProtectionCheck == 0 )
				RecheckTownProtection();
		}

		public override void GetContextMenuEntries( Mobile from, List<ContextMenuEntry> list )
		{
			base.GetContextMenuEntries( from, list );

			Party p = Engines.PartySystem.Party.Get( from );
			Party tp = Engines.PartySystem.Party.Get( this );

			if ( from == this )
			{
				if ( Alive )
				{
					bool enabled = VendorSearchQueryGump.IsInValidRegion( this );
					list.Add( new CallbackEntry( 1154679, new ContextCallback( VendorSearchGump ) ) { Enabled = enabled } );
				}

				list.Add( new CallbackEntry( 1049594, new ContextCallback( LoyaltyRatingMenu ) ) );

				if ( m_CollectionTitles.Count >= 2 )
					list.Add( new CallbackEntry( 6229, new ContextCallback( SelectRewardTitle ) ) );

				if ( m_Quest != null )
					m_Quest.GetContextMenuEntries( list );

				if ( Alive && InsuranceEnabled )
				{
					list.Add( new CallbackEntry( 6201, new ContextCallback( ToggleItemInsurance ) ) );
					list.Add( new CallbackEntry( 1114299, new ContextCallback( () => ItemInsuranceMenu.SendGump( this ) ) ) );
				}

				var house = HousingHelper.FindHouseAt( this );

				if ( house != null )
				{
					if ( Alive && house.InternalizedVendors.Count > 0 && house.IsOwner( this ) )
						list.Add( new CallbackEntry( 6204, new ContextCallback( GetVendor ) ) );

					list.Add( new CallbackEntry( 6207, new ContextCallback( LeaveHouse ) ) );
				}

				if ( m_JusticeProtectors.Count > 0 )
					list.Add( new CallbackEntry( 6157, new ContextCallback( CancelProtection ) ) );

				if ( Alive )
				{
					list.Add( new CallbackEntry( 6210, new ContextCallback( ToggleChampionTitles ) ) );

					QuestHelper.GetContextMenuEntries( list );
				}

				if ( p != null )
				{
					if ( p.Leader == from )
						list.Add( new CallbackEntry( 196, new ContextCallback( RemovePartyMember ) ) );
					else
						list.Add( new CallbackEntry( 195, new ContextCallback( RemovePartyMember ) ) );

					PartyMemberInfo mi = p[from];

					if ( mi != null )
						list.Add( new CallbackEntry( mi.CanLoot ? 199 : 194, new ContextCallback( TogglePartyLoot ) ) );
				}

				if ( DisabledPvpWarning )
					list.Add( new CallbackEntry( 1113797, new ContextCallback( EnablePvpWarning ) ) );
			}
			else
			{
				if ( p == null || p.Leader == from )
				{
					if ( tp == null && ( p == null || ( p.Members.Count + p.Candidates.Count ) < Engines.PartySystem.Party.Capacity ) )
						list.Add( new AddPartyMemberEntry( from, this ) );
					else if ( p != null && tp == p )
						list.Add( new CallbackEntry( 198, new ContextCallback( RemovePartyMember ) ) );
				}
			}
		}

		#region Vendor Search
		private void VendorSearchGump()
		{
			CloseGump( typeof( VendorSearchQueryGump ) );
			SendGump( new VendorSearchQueryGump( this ) );
		}
		#endregion

		private void LoyaltyRatingMenu()
		{
			CloseGump( typeof( LoyaltyGump ) );
			SendGump( new LoyaltyGump( this ) );
		}

		private void RemovePartyMember()
		{
			Party p = this.Party as Party;

			if ( p != null )
				p.Remove( this );
		}

		private void TogglePartyLoot()
		{
			Party p = this.Party as Party;

			if ( p != null )
			{
				PartyMemberInfo mi = p[this];

				if ( mi != null )
				{
					mi.CanLoot = !mi.CanLoot;

					if ( mi.CanLoot )
						SendLocalizedMessage( 1005447 ); // You have chosen to allow your party to loot your corpse.
					else
						SendLocalizedMessage( 1005448 ); // You have chosen to prevent your party from looting your corpse.
				}
			}
		}

		private void CancelProtection()
		{
			for ( int i = 0; i < m_JusticeProtectors.Count; ++i )
			{
				Mobile prot = (Mobile) m_JusticeProtectors[i];

				string args = String.Format( "{0}\t{1}", this.Name, prot.Name );

				prot.SendLocalizedMessage( 1049371, args ); // The protective relationship between ~1_PLAYER1~ and ~2_PLAYER2~ has been ended.
				this.SendLocalizedMessage( 1049371, args ); // The protective relationship between ~1_PLAYER1~ and ~2_PLAYER2~ has been ended.
			}

			m_JusticeProtectors.Clear();
		}

		#region Insurance

		private void ToggleItemInsurance()
		{
			if ( !CheckAlive() )
				return;

			this.BeginTarget( -1, false, TargetFlags.None, new TargetCallback( ToggleItemInsurance_Callback ) );
			SendLocalizedMessage( 1060868 ); // Target the item you wish to toggle insurance status on <ESC> to cancel
		}

		private void ToggleItemInsurance_Callback( Mobile from, object obj )
		{
			if ( !CheckAlive() )
				return;

			Item item = obj as Item;

			if ( item == null || !item.IsChildOf( this ) )
			{
				this.BeginTarget( -1, false, TargetFlags.None, new TargetCallback( ToggleItemInsurance_Callback ) );
				SendLocalizedMessage( 1060871, "", 0x23 ); // You can only insure items that you have equipped or that are in your backpack
			}
			else if ( item.Insured )
			{
				item.Insured = false;

				SendLocalizedMessage( 1060874, "", 0x35 ); // You cancel the insurance on the item

				this.BeginTarget( -1, false, TargetFlags.None, new TargetCallback( ToggleItemInsurance_Callback ) );
				SendLocalizedMessage( 1060868, "", 0x23 ); // Target the item you wish to toggle insurance status on <ESC> to cancel
			}
			else if ( !ItemInsuranceHelper.CanInsure( item ) || item.BlessedFor == from )
			{
				if ( item.LootType == LootType.Blessed || item.LootType == LootType.Newbied || item.BlessedFor == from )
					SendLocalizedMessage( 1060870, "", 0x23 ); // That item is blessed and does not need to be insured

				this.BeginTarget( -1, false, TargetFlags.None, new TargetCallback( ToggleItemInsurance_Callback ) );
				SendLocalizedMessage( 1060869, "", 0x23 ); // You cannot insure that
			}
			else
			{
				if ( !item.PayedInsurance )
				{
					int cost = ItemInsuranceHelper.GetInsuranceCost( item );

					if ( Banker.Withdraw( from, cost ) )
					{
						SendLocalizedMessage( 1060398, cost.ToString() ); // ~1_AMOUNT~ gold has been withdrawn from your bank box.
						item.PayedInsurance = true;
					}
					else
					{
						SendLocalizedMessage( 1061079, "", 0x23 ); // You lack the funds to purchase the insurance
						return;
					}
				}

				item.Insured = true;

				SendLocalizedMessage( 1060873, "", 0x23 ); // You have insured the item

				this.BeginTarget( -1, false, TargetFlags.None, new TargetCallback( ToggleItemInsurance_Callback ) );
				SendLocalizedMessage( 1060868, "", 0x23 ); // Target the item you wish to toggle insurance status on <ESC> to cancel
			}
		}

		#endregion

		private void ToggleChampionTitles()
		{
			if ( DisplayChampionTitle )
			{
				SendLocalizedMessage( 1062419, "", 0x23 ); // You have chosen to hide your monster kill title.

				DisplayChampionTitle = false;
			}
			else
			{
				SendLocalizedMessage( 1062418, "", 0x23 ); // You have chosen to display your monster kill title.

				DisplayChampionTitle = true;
			}
		}

		private void SelectRewardTitle()
		{
			CloseGump( typeof( SelectRewardTitleGump ) );
			SendGump( new SelectRewardTitleGump( this, m_CurrentCollectionTitle ) );
		}

		private void GetVendor()
		{
			var house = HousingHelper.FindHouseAt( this );

			if ( CheckAlive() && house != null && house.IsOwner( this ) && house.InternalizedVendors.Count > 0 )
			{
				CloseGump( typeof( ReclaimVendorGump ) );
				SendGump( new ReclaimVendorGump( house ) );
			}
		}

		private void LeaveHouse()
		{
			IHouse house = HousingHelper.FindHouseAt( this );

			if ( house != null )
				this.Location = house.BanLocation;
		}

		private void EnablePvpWarning()
		{
			DisabledPvpWarning = false;
			SendLocalizedMessage( 1113798 ); // Your PvP warning query has been re-enabled.
		}

		private delegate void ContextCallback();

		private class CallbackEntry : ContextMenuEntry
		{
			private ContextCallback m_Callback;

			public CallbackEntry( int number, ContextCallback callback )
				: this( number, -1, callback )
			{
			}

			public CallbackEntry( int number, int range, ContextCallback callback )
				: base( number, range )
			{
				m_Callback = callback;
			}

			public override void OnClick()
			{
				if ( m_Callback != null )
					m_Callback();
			}
		}

		public override void OnRegionChange( Region oldRegion, Region newRegion )
		{
			#region Vendor Search
			if ( HasGump( typeof( VendorSearchQueryGump ) ) && !VendorSearchQueryGump.IsInValidRegion( this ) )
				CloseGump( typeof( VendorSearchQueryGump ) );
			#endregion
		}

		public override void DisruptiveAction()
		{
			if ( Meditating )
				this.RemoveBuff( BuffIcon.ActiveMeditation );

			base.DisruptiveAction();
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( this == from && !Warmode )
			{
				IMount mount = Mount;

				if ( mount != null && !DesignContext.Check( this ) )
					return;
			}

			base.OnDoubleClick( from );
		}

		public override void DisplayPaperdollTo( Mobile to )
		{
			if ( DesignContext.Check( this ) )
				base.DisplayPaperdollTo( to );
		}

		private static bool m_NoRecursion;

		public override bool CheckEquip( Item item )
		{
			if ( !base.CheckEquip( item ) )
				return false;

			#region Factions
			FactionItem factionItem = FactionItem.Find( item );

			if ( factionItem != null )
			{
				Faction faction = Faction.Find( this );

				if ( faction == null )
				{
					SendLocalizedMessage( 1010371 ); // You cannot equip a faction item!
					return false;
				}
				else if ( faction != factionItem.Faction )
				{
					SendLocalizedMessage( 1010372 ); // You cannot equip an opposing faction's item!
					return false;
				}
				else
				{
					int maxWearables = FactionItem.GetMaxWearables( this );

					var wearables = this.GetEquippedItems()
						.Where( equipped => FactionItem.Find( equipped ) != null )
						.Count();

					if ( wearables > maxWearables )
					{
						SendLocalizedMessage( 1010373 ); // You do not have enough rank to equip more faction items!
						return false;
					}
				}
			}

			if ( item is IFactionArtifact )
			{
				PlayerState playerState = PlayerState.Find( this );
				IFactionArtifact artifact = item as IFactionArtifact;

				if ( playerState == null )
				{
					SendLocalizedMessage( 1010371 ); // You cannot equip a faction item!
					return false;
				}
				else if ( artifact.Owner != this )
				{
					SendLocalizedMessage( 500364, null, 0x23 ); // You can't use that, it belongs to someone else.
					return false;
				}
			}
			#endregion

			if ( this.AccessLevel < AccessLevel.GameMaster && item.Layer != Layer.Mount && this.HasTrade )
			{
				BounceInfo bounce = item.GetBounce();

				if ( bounce != null )
				{
					if ( bounce.m_Parent is Item )
					{
						Item parent = (Item) bounce.m_Parent;

						if ( parent == this.Backpack || parent.IsChildOf( this.Backpack ) )
							return true;
					}
					else if ( bounce.m_Parent == this )
					{
						return true;
					}
				}

				SendLocalizedMessage( 1004042 ); // You can only equip what you are already carrying while you have a trade pending.
				return false;
			}

			return true;
		}

		public override bool CheckTrade( Mobile to, Item item, SecureTradeContainer cont, bool message, bool checkItems, int plusItems, int plusWeight )
		{
			int msgNum = 0;

			if ( cont == null )
			{
				if ( to.Holding != null )
					msgNum = 1062727; // You cannot trade with someone who is dragging something.
				else if ( this.HasTrade )
					msgNum = 1062781; // You are already trading with someone else!
				else if ( to.HasTrade )
					msgNum = 1062779; // That person is already involved in a trade
			}

			if ( msgNum == 0 )
			{
				if ( cont != null )
				{
					plusItems += cont.TotalItems;
					plusWeight += cont.TotalWeight;
				}

				if ( this.Backpack == null || !this.Backpack.CheckHold( this, item, false, checkItems, plusItems, plusWeight ) )
					msgNum = 1004040; // You would not be able to hold this if the trade failed.
				else if ( to.Backpack == null || !to.Backpack.CheckHold( to, item, false, checkItems, plusItems, plusWeight ) )
					msgNum = 1004039; // The recipient of this trade would not be able to carry this.
				else
					msgNum = CheckContentForTrade( item );
			}

			if ( msgNum != 0 )
			{
				if ( message )
					this.SendLocalizedMessage( msgNum );

				return false;
			}

			return true;
		}

		private static int CheckContentForTrade( Item item )
		{
			if ( item is TrapableContainer && ( (TrapableContainer) item ).TrapType != TrapType.None )
				return 1004044; // You may not trade trapped items.

			if ( SkillHandlers.StolenItem.IsStolen( item ) )
				return 1004043; // You may not trade recently stolen items.

			if ( item is Container )
			{
				foreach ( Item subItem in item.Items )
				{
					int msg = CheckContentForTrade( subItem );

					if ( msg != 0 )
						return msg;
				}
			}

			return 0;
		}

		public override bool CheckNonlocalDrop( Mobile from, Item item, Item target )
		{
			if ( !base.CheckNonlocalDrop( from, item, target ) )
				return false;

			if ( from.AccessLevel >= AccessLevel.GameMaster )
				return true;

			Container pack = this.Backpack;
			if ( from == this && this.HasTrade && ( target == pack || target.IsChildOf( pack ) ) )
			{
				BounceInfo bounce = item.GetBounce();

				if ( bounce != null && bounce.m_Parent is Item )
				{
					Item parent = (Item) bounce.m_Parent;

					if ( parent == pack || parent.IsChildOf( pack ) )
						return true;
				}

				SendLocalizedMessage( 1004041 ); // You can't do that while you have a trade pending.
				return false;
			}

			return true;
		}

		protected override void OnLocationChange( Point3D oldLocation )
		{
			CheckLightLevels( false );

			DesignContext context = m_DesignContext;

			if ( context == null || m_NoRecursion )
				return;

			m_NoRecursion = true;

			HouseFoundation foundation = context.Foundation;

			int newX = this.X, newY = this.Y;
			int newZ = foundation.Z + HouseFoundation.GetLevelZ( context.Level, foundation );

			int startX = foundation.X + foundation.Components.Min.X + 1;
			int startY = foundation.Y + foundation.Components.Min.Y + 1;
			int endX = startX + foundation.Components.Width - 1;
			int endY = startY + foundation.Components.Height - 2;

			if ( newX >= startX && newY >= startY && newX < endX && newY < endY && Map == foundation.Map )
			{
				if ( Z != newZ )
					Location = new Point3D( X, Y, newZ );

				m_NoRecursion = false;
				return;
			}

			Location = new Point3D( foundation.X, foundation.Y, newZ );
			Map = foundation.Map;

			m_NoRecursion = false;
		}

		public override bool OnMoveOver( Mobile m )
		{
			if ( m is BaseCreature && !( (BaseCreature) m ).Controlled )
				return false;

			return base.OnMoveOver( m );
		}

		protected override void OnMapChange( Map oldMap )
		{
			if ( ( Map != Faction.Facet && oldMap == Faction.Facet ) || ( Map == Faction.Facet && oldMap != Faction.Facet ) )
				InvalidateProperties();

			DesignContext context = m_DesignContext;

			if ( context == null || m_NoRecursion )
				return;

			m_NoRecursion = true;

			HouseFoundation foundation = context.Foundation;

			if ( Map != foundation.Map )
				Map = foundation.Map;

			m_NoRecursion = false;
		}

		public override void OnBeneficialAction( Mobile target, bool isCriminal )
		{
			if ( m_SentHonorContext != null )
				m_SentHonorContext.OnSourceBeneficialAction( target );

			base.OnBeneficialAction( target, isCriminal );
		}

		public override void OnCombatantChange()
		{
			if ( Combatant != null && FloorTrapKit.IsAssembling( this ) )
				FloorTrapKit.StopAssembling( this, 1113510 ); // You begin combat and cease trap assembly.
		}

		public override void OnDamage( int amount, Mobile from, bool willKill )
		{
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

			if ( FloorTrapKit.IsAssembling( this ) )
				FloorTrapKit.StopAssembling( this, 1113512 ); // You are hit and cease trap assembly.

			if ( ExplodingTarPotion.IsSlept( this ) )
				ExplodingTarPotion.RemoveEffect( this );

			if ( Confidence.IsRegenerating( this ) )
				Confidence.StopRegenerating( this );

			WeightOverloading.FatigueOnDamage( this, amount );

			if ( m_ReceivedHonorContext != null )
				m_ReceivedHonorContext.OnTargetDamaged( from, amount );
			if ( m_SentHonorContext != null )
				m_SentHonorContext.OnSourceDamaged( from, amount );

			if ( amount > 0 && ParalyzingBlow.UnderEffect( this ) )
			{
				this.Frozen = false;
				ParalyzingBlow.m_Table.Remove( this );
			}

			#region Gargoyle Berserk
			if ( Race == Race.Gargoyle && !willKill && !Berserk )
			{
				if ( ( (float) ( Hits - amount ) / HitsMax ) < 0.8 )
				{
					if ( m_BerserkTimer != null )
						m_BerserkTimer.Stop();

					m_BerserkTimer = new BerserkTimer( this );
					m_BerserkTimer.Start();
				}
			}
			#endregion

			int soulCharge = ArmorAttributes.GetValue( this, ArmorAttribute.SoulCharge );

			if ( soulCharge > 0 && Mana < ManaMax )
			{
				int mana = (int) ( amount * soulCharge / 100.0 );

				if ( mana > 0 )
				{
					Mana += mana;

					Effects.SendPacket( this, Map, new TargetParticleEffect( this, 0x375A, 1, 10, 0x71, 2, 0x1AE9, 0, 0 ) );
					SendLocalizedMessage( 1113636 ); // The soul charge effect converts some of the damage you received into mana.
				}
			}

			GainBattleLust();

			#region Durability
			Layer layer;

			switch ( Utility.Random( 18 ) )
			{
				default:
				case 0:
					layer = Layer.Shoes;
					break;
				case 1:
					layer = Layer.Pants;
					break;
				case 2:
					layer = Layer.Shirt;
					break;
				case 3:
					layer = Layer.Helm;
					break;
				case 4:
					layer = Layer.Gloves;
					break;
				case 5:
					layer = Layer.Ring;
					break;
				case 6:
					layer = Layer.Talisman;
					break;
				case 7:
					layer = Layer.Neck;
					break;
				case 8:
					layer = Layer.Waist;
					break;
				case 9:
					layer = Layer.InnerTorso;
					break;
				case 10:
					layer = Layer.Bracelet;
					break;
				case 11:
					layer = Layer.MiddleTorso;
					break;
				case 12:
					layer = Layer.Earrings;
					break;
				case 13:
					layer = Layer.Arms;
					break;
				case 14:
					layer = Layer.Cloak;
					break;
				case 15:
					layer = Layer.OuterTorso;
					break;
				case 16:
					layer = Layer.OuterLegs;
					break;
				case 17:
					layer = Layer.InnerLegs;
					break;
			}

			IWearableDurability armor = this.FindItemOnLayer( layer ) as IWearableDurability;

			if ( armor != null && armor.CanLoseDurability )
				armor.OnHit( amount ); // call OnHit to lose durability
			#endregion

			base.OnDamage( amount, from, willKill );
		}

		#region Gargoyle Berserk
		private bool m_Berserk;
		private BerserkTimer m_BerserkTimer;

		public bool Berserk
		{
			get { return m_Berserk; }
			set { m_Berserk = value; }
		}

		public class BerserkTimer : Timer
		{
			private PlayerMobile m_Owner;

			public BerserkTimer( PlayerMobile owner )
				: base( TimeSpan.FromSeconds( 2.0 ), TimeSpan.FromSeconds( 2.0 ) )
			{
				m_Owner = owner;

				m_Owner.PlaySound( 0x20F );
				m_Owner.PlaySound( m_Owner.Body.IsFemale ? 0x338 : 0x44A );
				m_Owner.FixedParticles( 0x376A, 1, 31, 9961, 1160, 0, EffectLayer.Waist );
				m_Owner.FixedParticles( 0x37C4, 1, 31, 9502, 43, 2, EffectLayer.Waist );

				BuffInfo.AddBuff( m_Owner, new BuffInfo( BuffIcon.Berserk, 1080449, 1115021, "15\t3", false ) );

				m_Owner.Berserk = true;
			}

			protected override void OnTick()
			{
				float percentage = (float) m_Owner.Hits / m_Owner.HitsMax;

				if ( percentage >= 0.8 )
					RemoveEffect();
			}

			public void RemoveEffect()
			{
				m_Owner.PlaySound( 0xF8 );
				BuffInfo.RemoveBuff( m_Owner, BuffIcon.Berserk );

				m_Owner.Berserk = false;

				Stop();
			}
		}
		#endregion

		public static int ComputeSkillTotal( Mobile m )
		{
			int total = 0;

			for ( int i = 0; i < m.Skills.Length; ++i )
				total += m.Skills[i].BaseFixedPoint;

			return ( total / 10 );
		}

		public override bool DoEffectTimerOnDeath { get { return true; } }

		private static void GiftOfLifeRes_Callback( object state )
		{
			Mobile m = (Mobile) state;

			if ( !m.Alive )
				m.SendGump( new ResurrectGump( m ) );

			GiftOfLifeSpell.RemoveEffect( m );
		}

		private PlayerMobile m_InsuranceAward;
		private int m_InsuranceCost;
		private int m_InsuranceBonus;

		public override bool OnBeforeDeath()
		{
			Flying = false;

			if ( m_BerserkTimer != null )
				m_BerserkTimer.RemoveEffect();

			if ( SilverSaplingSeed.CanBeResurrected( this ) )
			{
				Timer.DelayCall<Mobile>( TimeSpan.FromSeconds( 1.0 ), new TimerStateCallback<Mobile>( SilverSaplingSeed.OfferResurrection ), this );
			}
			else if ( GiftOfLifeSpell.UnderEffect( this ) )
			{
				Timer.DelayCall( TimeSpan.FromSeconds( 5.0 ), new TimerStateCallback( GiftOfLifeRes_Callback ), this );
			}
			else if ( Backpack != null )
			{
				GemOfSalvation gem = Backpack.FindItemByType<GemOfSalvation>();

				if ( gem != null )
					Timer.DelayCall( TimeSpan.FromSeconds( 2.0 ), new TimerStateCallback<PlayerMobile>( gem.Use ), this );

				ShrineGem shrineGem = Backpack.FindItemByType<ShrineGem>();

				if ( shrineGem != null )
					Timer.DelayCall( TimeSpan.FromSeconds( 2.0 ), new TimerStateCallback<PlayerMobile>( shrineGem.Use ), this );
			}

			CheckKRStartingQuestStep( 25 );

			++Deaths;

			m_EquipInsuredItems = this.GetEquippedItems().Where( ( item ) => item != null && item.Insured ).ToList();

			AnimalForm.RemoveContext( this, true );

			if ( Sphynx.UnderEffect( this ) )
			{
				SendLocalizedMessage( 1060859 ); // The effects of the Sphynx have worn off.

				Sphynx.m_Table.Remove( this );
			}

			m_InsuranceCost = 0;
			m_InsuranceAward = FindInsuranceAward();

			if ( m_InsuranceAward != null )
				m_InsuranceAward.m_InsuranceBonus = 0;

			if ( m_ReceivedHonorContext != null )
				m_ReceivedHonorContext.OnTargetKilled();

			if ( m_SentHonorContext != null )
				m_SentHonorContext.OnSourceKilled();

			return base.OnBeforeDeath();
		}

		private PlayerMobile FindInsuranceAward()
		{
			Mobile killer = FindMostRecentDamager( false );

			if ( killer is BaseCreature )
			{
				Mobile master = ( (BaseCreature) killer ).GetMaster();

				if ( master != null )
					killer = master;
			}

			if ( killer == this )
				killer = null;

			return killer as PlayerMobile;
		}

		private bool CheckInsuranceOnDeath( Item item )
		{
			if ( InsuranceEnabled && item.Insured )
			{
				int itemCost = ItemInsuranceHelper.GetInsuranceCost( item );
				int halvedItemCost = itemCost / 2;

				if ( AutoRenewInsurance )
				{
					int victimCost = itemCost;

					if ( Banker.Withdraw( this, victimCost ) )
					{
						m_InsuranceCost += victimCost;
						item.PayedInsurance = true;
					}
					else
					{
						SendLocalizedMessage( 1061079, "", 0x23 ); // You lack the funds to purchase the insurance
						item.PayedInsurance = false;
						item.Insured = false;
					}
				}
				else
				{
					item.PayedInsurance = false;
					item.Insured = false;
				}

				if ( m_InsuranceAward != null )
				{
					int killerAward = ( ( Faction.Find( m_InsuranceAward ) == null )
											? halvedItemCost
											: itemCost );

					if ( Banker.Deposit( m_InsuranceAward, killerAward ) )
						m_InsuranceAward.m_InsuranceBonus += killerAward;
				}

				return true;
			}

			return false;
		}

		public override DeathMoveResult GetParentMoveResultFor( Item item )
		{
			if ( CheckInsuranceOnDeath( item ) )
				return DeathMoveResult.MoveToBackpack;

			DeathMoveResult res = base.GetParentMoveResultFor( item );

			if ( res == DeathMoveResult.MoveToCorpse && item.Movable && this.Young )
				res = DeathMoveResult.MoveToBackpack;

			return res;
		}

		public override DeathMoveResult GetInventoryMoveResultFor( Item item )
		{
			if ( CheckInsuranceOnDeath( item ) )
				return DeathMoveResult.MoveToBackpack;

			DeathMoveResult res = base.GetInventoryMoveResultFor( item );

			if ( res == DeathMoveResult.MoveToCorpse && item.Movable && this.Young )
				res = DeathMoveResult.MoveToBackpack;

			return res;
		}

		protected override void OnAfterDeath( Container c )
		{
			Corpse corpse = c as Corpse;

			corpse.InsuredItems = m_EquipInsuredItems;
			m_EquipInsuredItems = null;

			base.OnAfterDeath( c );

			HueMod = -1;
			NameMod = null;
			SavagePaintExpiration = TimeSpan.Zero;

			SetHairMods( -1, -1 );

			if ( this.AccessLevel == AccessLevel.Player )
				Hidden = false;

			PolymorphSpell.StopTimer( this );
			IncognitoSpell.StopTimer( this );
			DisguiseGump.StopTimer( this );

			EndAction( typeof( PolymorphSpell ) );
			EndAction( typeof( IncognitoSpell ) );

			MeerMage.StopEffect( this, false );

			ArcaneEmpowermentSpell.StopBuffing( this, false );

			SkillHandlers.StolenItem.ReturnOnDeath( this, c );

			if ( m_PermaFlags.Count > 0 )
			{
				m_PermaFlags.Clear();

				if ( c is Corpse )
					( (Corpse) c ).Criminal = true;

				if ( SkillHandlers.Stealing.ClassicMode )
					Criminal = true;
			}

			if ( this.LastKiller != null )
			{
				PlayerMobile k = this.LastKiller as PlayerMobile;

				Guild k_Guild = null;
				Guild m_Guild = null;

				if ( k != null && this != null )
				{
					k_Guild = (Guild) k.Guild;
					m_Guild = (Guild) this.Guild;
				}

				if ( k_Guild != null && m_Guild != null && k_Guild.IsWar( m_Guild ) && k_Guild.GetMaxKills( m_Guild ) != 0 )
					k_Guild.AddKills( m_Guild, 1 );
			}

			if ( this.Kills >= 5 && DateTime.UtcNow >= m_NextJustAward )
			{
				Mobile m = FindMostRecentDamager( false );

				if ( m is BaseCreature )
					m = ( (BaseCreature) m ).GetMaster();

				if ( m != null && m.IsPlayer && m != this )
				{
					bool gainedPath = false;

					int pointsToGain = 0;

					pointsToGain += (int) Math.Sqrt( this.GameTime.TotalSeconds * 4 );
					pointsToGain *= 5;
					pointsToGain += (int) Math.Pow( this.Skills.Total / 250, 2 );

					if ( VirtueHelper.Award( m, VirtueName.Justice, pointsToGain, ref gainedPath ) )
					{
						if ( gainedPath )
							m.SendLocalizedMessage( 1049367 ); // You have gained a path in Justice!
						else
							m.SendLocalizedMessage( 1049363 ); // You have gained in Justice.

						m.FixedParticles( 0x375A, 9, 20, 5027, EffectLayer.Waist );
						m.PlaySound( 0x1F7 );

						m_NextJustAward = DateTime.UtcNow + TimeSpan.FromMinutes( pointsToGain / 3 );
					}
				}
			}

			if ( m_InsuranceCost > 0 )
				SendLocalizedMessage( 1060398, m_InsuranceCost.ToString() ); // ~1_AMOUNT~ gold has been withdrawn from your bank box.

			if ( m_InsuranceAward != null )
			{
				PlayerMobile pm = m_InsuranceAward;

				if ( pm.m_InsuranceBonus > 0 )
					pm.SendLocalizedMessage( 1060397, pm.m_InsuranceBonus.ToString() ); // ~1_AMOUNT~ gold has been deposited into your bank box.
			}

			Mobile killer = this.FindMostRecentDamager( true );

			if ( killer is BaseCreature )
			{
				BaseCreature bc = (BaseCreature) killer;

				Mobile master = bc.GetMaster();
				if ( master != null )
					killer = master;
			}

			if ( this.Young && m_KRStartingQuestStep == 0 )
			{
				Point3D dest = GetYoungDeathDestination();

				if ( dest != Point3D.Zero )
				{
					this.Location = dest;
					Timer.DelayCall( TimeSpan.FromSeconds( 2.5 ), new TimerCallback( SendYoungDeathNotice ) );
				}
			}

			Faction.HandleDeath( this, killer );

			if ( Region.IsPartOf( typeof( UnderworldDeathRegion ) ) )
			{
				Point3D dest = new Point3D( 1060, 1066, -42 );

				MoveToWorld( dest, Map.TerMur );
				c.MoveToWorld( dest, Map.TerMur );

				SendLocalizedMessage( 1113566 ); // You will find your remains at the entrance of the maze. 
			}

			Send( new DisplayWaypoint( c, WaypointType.Corpse, 1028198 ) ); // corpse
		}

		public override void OnItemLifted( Mobile from, Item item )
		{
			if ( item.Parent is KRStartingQuestContainer )
				CheckKRStartingQuestStep( 8 );

			if ( item.Parent is InstancedCorpse )
			{
				InstancedCorpse c = (InstancedCorpse) item.Parent;

				if ( c.TotalItems == 1 )
					CheckKRStartingQuestStep( 21 );
			}
		}

		private DateTime m_NextDrinkConflagrationPotion;
		private DateTime m_NextDrinkMaskOfDeathPotion;
		private DateTime m_NextDrinkConfusionPotion;
		private DateTime m_NextDrinkExplodingTarPotion;

		private ArrayList m_PermaFlags;
		private ArrayList m_VisList;
		private Hashtable m_AntiMacroTable;
		private TimeSpan m_GameTime;
		private TimeSpan m_ShortTermElapse;
		private TimeSpan m_LongTermElapse;
		private DateTime m_SessionStart;
		private DateTime m_LastEscortTime;
		private DateTime m_LastPetBallTime;
		private DateTime m_NextSmithBulkOrder;
		private DateTime m_NextTailorBulkOrder;
		private DateTime m_SavagePaintExpiration;
		private SkillName m_Learning = (SkillName) ( -1 );

		private DateTime m_NextPuzzleAttempt;

		[CommandProperty( AccessLevel.GameMaster )]
		public DateTime NextPuzzleAttempt
		{
			get { return m_NextPuzzleAttempt; }
			set { m_NextPuzzleAttempt = value; }
		}

		private DateTime m_NextLuckyCoinWish;

		[CommandProperty( AccessLevel.GameMaster )]
		public DateTime NextLuckyCoinWish
		{
			get { return m_NextLuckyCoinWish; }
			set { m_NextLuckyCoinWish = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public TimeSpan NextDrinkConflagrationPotion
		{
			get
			{
				TimeSpan ts = m_NextDrinkConflagrationPotion - DateTime.UtcNow;

				if ( ts < TimeSpan.Zero )
					ts = TimeSpan.Zero;

				return ts;
			}
			set
			{
				try
				{
					m_NextDrinkConflagrationPotion = DateTime.UtcNow + value;
				}
				catch
				{
				}
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public TimeSpan NextDrinkMaskOfDeathPotion
		{
			get
			{
				TimeSpan ts = m_NextDrinkMaskOfDeathPotion - DateTime.UtcNow;

				if ( ts < TimeSpan.Zero )
					ts = TimeSpan.Zero;

				return ts;
			}
			set
			{
				try
				{
					m_NextDrinkMaskOfDeathPotion = DateTime.UtcNow + value;
				}
				catch
				{
				}
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public TimeSpan NextDrinkConfusionPotion
		{
			get
			{
				TimeSpan ts = m_NextDrinkConfusionPotion - DateTime.UtcNow;

				if ( ts < TimeSpan.Zero )
					ts = TimeSpan.Zero;

				return ts;
			}
			set
			{
				try
				{
					m_NextDrinkConfusionPotion = DateTime.UtcNow + value;
				}
				catch
				{
				}
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public TimeSpan NextDrinkExplodingTarPotion
		{
			get
			{
				TimeSpan ts = m_NextDrinkExplodingTarPotion - DateTime.UtcNow;

				if ( ts < TimeSpan.Zero )
					ts = TimeSpan.Zero;

				return ts;
			}
			set
			{
				try
				{
					m_NextDrinkExplodingTarPotion = DateTime.UtcNow + value;
				}
				catch
				{
				}
			}
		}

		public SkillName Learning { get { return m_Learning; } set { m_Learning = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public TimeSpan SavagePaintExpiration
		{
			get
			{
				TimeSpan ts = m_SavagePaintExpiration - DateTime.UtcNow;

				if ( ts < TimeSpan.Zero )
					ts = TimeSpan.Zero;

				return ts;
			}
			set { m_SavagePaintExpiration = DateTime.UtcNow + value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public TimeSpan NextSmithBulkOrder
		{
			get
			{
				TimeSpan ts = m_NextSmithBulkOrder - DateTime.UtcNow;

				if ( ts < TimeSpan.Zero )
					ts = TimeSpan.Zero;

				return ts;
			}
			set
			{
				try { m_NextSmithBulkOrder = DateTime.UtcNow + value; }
				catch { }
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public TimeSpan NextTailorBulkOrder
		{
			get
			{
				TimeSpan ts = m_NextTailorBulkOrder - DateTime.UtcNow;

				if ( ts < TimeSpan.Zero )
					ts = TimeSpan.Zero;

				return ts;
			}
			set
			{
				try { m_NextTailorBulkOrder = DateTime.UtcNow + value; }
				catch { }
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public DateTime LastEscortTime
		{
			get { return m_LastEscortTime; }
			set { m_LastEscortTime = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public DateTime LastPetBallTime
		{
			get { return m_LastPetBallTime; }
			set { m_LastPetBallTime = value; }
		}

		public PlayerMobile()
		{
			m_CollectionTitles = new ArrayList();
			m_CollectionTitles.Add( 1073995 ); // [No Title]

			m_AutoStabledPets = new ArrayList();

			#region ML Quest System
			m_Quests = new List<BaseQuest>();
			m_Chains = new Dictionary<QuestChain, BaseChain>();
			m_DoneQuests = new List<QuestRestartInfo>();
			#endregion

			m_VisList = new ArrayList();
			m_PermaFlags = new ArrayList();
			m_AntiMacroTable = new Hashtable();

			m_BOBFilter = new Engines.BulkOrders.BOBFilter();

			m_GameTime = TimeSpan.Zero;
			m_ShortTermElapse = TimeSpan.FromHours( 8.0 );
			m_LongTermElapse = TimeSpan.FromHours( 40.0 );

			m_JusticeProtectors = new ArrayList();

			m_LoyaltyInfo = new LoyaltyInfo();
			m_TieredQuestInfo = new TieredQuestInfo();

			InvalidateMyRunUO();
		}

		public override bool OverridesRaceReq
		{
			get
			{
				return this.FindItemOnLayer( Layer.Earrings ) is MorphEarrings;
			}
		}

		public override bool MutateSpeech( ArrayList hears, ref string text, ref object context )
		{
			if ( Alive )
				return false;

			if ( Skills[SkillName.SpiritSpeak].Value >= 100.0 )
				return false;

			for ( int i = 0; i < hears.Count; ++i )
			{
				object o = hears[i];

				if ( o != this && o is Mobile && ( (Mobile) o ).Skills[SkillName.SpiritSpeak].Value >= 100.0 )
					return false;
			}

			return base.MutateSpeech( hears, ref text, ref context );
		}

		#region Poison

		public override void OnCured( Mobile from, Poison oldPoison )
		{
			BuffInfo.RemoveBuff( this, BuffIcon.Poison );
		}

		public override ApplyPoisonResult ApplyPoison( Mobile from, Poison poison )
		{
			if ( !Alive )
				return ApplyPoisonResult.Immune;

			if ( this.BodyMod == 0x7A && ( poison == Poison.Lesser || poison == Poison.Regular || poison == Poison.Greater ) )
				return ApplyPoisonResult.Immune;

			if ( Spells.Necromancy.EvilOmenSpell.CheckEffect( this ) )
				return base.ApplyPoison( from, PoisonImpl.IncreaseLevel( poison ) );

			return base.ApplyPoison( from, poison );
		}

		public override bool CheckPoisonImmunity( Mobile from, Poison poison )
		{
			if ( this.Young )
				return true;

			return base.CheckPoisonImmunity( from, poison );
		}

		public override void OnPoisonImmunity( Mobile from, Poison poison )
		{
			if ( this.Young )
				SendLocalizedMessage( 502808 ); // You would have been poisoned, were you not new to the land of Britannia. Be careful in the future.
			else
				base.OnPoisonImmunity( from, poison );
		}
		#endregion

		public PlayerMobile( Serial s )
			: base( s )
		{
			m_VisList = new ArrayList();
			m_AntiMacroTable = new Hashtable();
			InvalidateMyRunUO();
		}

		public ArrayList VisibilityList
		{
			get { return m_VisList; }
		}

		public ArrayList PermaFlags
		{
			get { return m_PermaFlags; }
		}

		public override int Luck
		{
			get
			{
				int luck = GetMagicalAttribute( MagicalAttribute.Luck );

				if ( FontOfFortune.HasBlessing( this, FontOfFortune.BlessingType.Luck ) )
					luck += 400;

				return luck;
			}
		}

		public override bool IsHarmfulCriminal( Mobile target )
		{
			if ( SkillHandlers.Stealing.ClassicMode && target is PlayerMobile && ( (PlayerMobile) target ).m_PermaFlags.Count > 0 )
			{
				int noto = Notoriety.Compute( this, target );

				if ( noto == Notoriety.Innocent )
					target.Delta( MobileDelta.Noto );

				return false;
			}

			if ( target is BaseCreature && ( (BaseCreature) target ).InitialInnocent && !( (BaseCreature) target ).Controlled )
				return false;

			return base.IsHarmfulCriminal( target );
		}

		public bool AntiMacroCheck( Skill skill, object obj )
		{
			if ( obj == null || m_AntiMacroTable == null || this.AccessLevel != AccessLevel.Player )
				return true;

			Hashtable tbl = (Hashtable) m_AntiMacroTable[skill];
			if ( tbl == null )
				m_AntiMacroTable[skill] = tbl = new Hashtable();

			CountAndTimeStamp count = (CountAndTimeStamp) tbl[obj];
			if ( count != null )
			{
				if ( count.TimeStamp + SkillCheck.AntiMacroExpire <= DateTime.UtcNow )
				{
					count.Count = 1;
					return true;
				}
				else
				{
					++count.Count;
					if ( count.Count <= SkillCheck.Allowance )
						return true;
					else
						return false;
				}
			}
			else
			{
				tbl[obj] = count = new CountAndTimeStamp();
				count.Count = 1;

				return true;
			}
		}

		private void RevertHair()
		{
			SetHairMods( -1, -1 );
		}

		private Engines.BulkOrders.BOBFilter m_BOBFilter;

		public Engines.BulkOrders.BOBFilter BOBFilter
		{
			get { return m_BOBFilter; }
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			m_CollectionTitles = new ArrayList();

			switch ( version )
			{
				case 56:
					{
						bool hasAnyBardMastery = reader.ReadBool();

						if ( hasAnyBardMastery )
						{
							m_BardMastery = BardMastery.GetFromId( reader.ReadEncodedInt() );
							m_BardElementDamage = (ResistanceType) reader.ReadEncodedInt();
							m_NextBardMasterySwitch = reader.ReadDateTime();
							m_BardMasteryLearnedMask = reader.ReadEncodedInt();
						}

						goto case 55;
					}
				case 55:
					{
						m_NextAnkhPendantUse = reader.ReadDateTime();

						goto case 54;
					}
				case 54:
					{
						m_NextTenthAnniversarySculptureUse = reader.ReadDateTime();

						goto case 53;
					}
				case 53:
					{
						m_LastForgedPardonUse = reader.ReadDateTime();

						goto case 52;
					}
				case 52:
					{
						m_DoomCredits = reader.ReadInt();

						m_TieredQuestInfo = new TieredQuestInfo( reader );

						m_LoyaltyInfo = new LoyaltyInfo( reader );

						m_FloorTrapsPlaced = reader.ReadInt();

						m_NextPuzzleAttempt = reader.ReadDateTime();

						m_NextDrinkExplodingTarPotion = reader.ReadDateTime();

						m_NextLuckyCoinWish = reader.ReadDateTime();

						m_NextSilverSaplingUse = reader.ReadDateTime();

						m_SacredQuestNextChance = reader.ReadDateTime();

						m_HumilityQuestStatus = (HumilityQuestStatus) reader.ReadInt();
						m_HumilityQuestNextChance = reader.ReadDateTime();

						m_NextGemOfSalvationUse = reader.ReadDateTime();

						m_AnkhNextUse = reader.ReadDateTime();

						m_Quests = QuestReader.Quests( reader, this );
						m_Chains = QuestReader.Chains( reader );

						m_TenthAnniversaryCredits = reader.ReadDouble();

						m_KRStartingQuestStep = reader.ReadByte();

						m_CurrentCollectionTitle = reader.ReadInt();
						int titlecount = reader.ReadInt();

						for ( int i = 0; i < titlecount; i++ )
						{
							int title = reader.ReadInt();
							m_CollectionTitles.Add( title );
						}

						int recipeCount = reader.ReadInt();

						if ( recipeCount > 0 )
						{
							m_AcquiredRecipes = new Hashtable();

							for ( int i = 0; i < recipeCount; i++ )
							{
								int r = reader.ReadInt();
								if ( reader.ReadBool() )	//Don't add in recipies which we haven't gotten or have been removed
									m_AcquiredRecipes.Add( r, true );
							}
						}

						m_AutoStabledPets = reader.ReadMobileList();

						ACState = (AdvancedCharacterState) reader.ReadInt();

						Deaths = reader.ReadInt();

						NextDrinkConflagrationPotion = reader.ReadTimeSpan();
						NextDrinkMaskOfDeathPotion = reader.ReadTimeSpan();
						NextDrinkConfusionPotion = reader.ReadTimeSpan();

						m_GuildRank = reader.ReadInt();

						m_LastLogin = reader.ReadDateTime();

						m_ToTItemsTurnedIn = reader.ReadInt();
						m_ToTTotalMonsterFame = reader.ReadDouble();

						LastTierLoss = reader.ReadDeltaTime();

						LastChampionTierLoss = reader.ReadDeltaTime();

						LastSuperChampionTierLoss = reader.ReadDeltaTime();

						int length_super = reader.ReadInt();

						for ( int i = 0; i < length_super; i++ )
							SuperChampionTiers[i] = reader.ReadInt();

						int length = reader.ReadInt();

						for ( int i = 0; i < length; i++ )
							ChampionTiers[i] = reader.ReadDouble();

						m_LastValorLoss = reader.ReadDeltaTime();

						m_LastHonorLoss = reader.ReadDeltaTime();
						m_SolenFriendship = (SolenFriendship) reader.ReadEncodedInt();

						m_Quest = QuestSerializer.DeserializeQuest( reader );

						if ( m_Quest != null )
							m_Quest.From = this;

						int count = reader.ReadEncodedInt();

						if ( count > 0 )
						{
							m_DoneQuests = new List<QuestRestartInfo>();

							for ( int i = 0; i < count; ++i )
							{
								Type questType = QuestSerializer.ReadType( QuestSystem.QuestTypes, reader );
								DateTime restartTime = reader.ReadDateTime();

								m_DoneQuests.Add( new QuestRestartInfo( questType, restartTime ) );
							}
						}

						m_Profession = reader.ReadEncodedInt();

						m_LastCompassionLoss = reader.ReadDeltaTime();

						m_CompassionGains = reader.ReadEncodedInt();

						if ( m_CompassionGains > 0 )
							m_NextCompassionDay = reader.ReadDeltaTime();

						m_BOBFilter = new Engines.BulkOrders.BOBFilter( reader );

						if ( reader.ReadBool() )
						{
							m_HairModID = reader.ReadInt();
							m_HairModHue = reader.ReadInt();
							m_BeardModID = reader.ReadInt();
							m_BeardModHue = reader.ReadInt();

							// We cannot call SetHairMods( -1, -1 ) here because the items have not yet loaded
							Timer.DelayCall( TimeSpan.Zero, new TimerCallback( RevertHair ) );
						}

						SavagePaintExpiration = reader.ReadTimeSpan();

						if ( SavagePaintExpiration > TimeSpan.Zero )
						{
							BodyMod = ( Female ? 184 : 183 );
							HueMod = 0;
						}

						m_NpcGuild = (NpcGuild) reader.ReadInt();
						m_NpcGuildJoinTime = reader.ReadDateTime();
						m_NpcGuildGameTime = reader.ReadTimeSpan();

						m_PermaFlags = reader.ReadMobileList();

						NextTailorBulkOrder = reader.ReadTimeSpan();

						NextSmithBulkOrder = reader.ReadTimeSpan();

						m_LastJusticeLoss = reader.ReadDeltaTime();
						m_JusticeProtectors = reader.ReadMobileList();

						m_LastSacrificeGain = reader.ReadDeltaTime();
						m_LastSacrificeLoss = reader.ReadDeltaTime();
						m_AvailableResurrects = reader.ReadInt();

						m_Flags = (PlayerFlag) reader.ReadInt();

						m_LongTermElapse = reader.ReadTimeSpan();
						m_ShortTermElapse = reader.ReadTimeSpan();
						m_GameTime = reader.ReadTimeSpan();

						break;
					}
			}

			#region ML Quest System
			if ( m_Quests == null )
				m_Quests = new List<BaseQuest>();

			if ( m_Chains == null )
				m_Chains = new Dictionary<QuestChain, BaseChain>();

			if ( m_DoneQuests == null )
				m_DoneQuests = new List<QuestRestartInfo>();
			#endregion

			if ( this != null && this.Region.IsPartOf( typeof( DoomLampRoom ) ) )
			{
				Rectangle2D rect = new Rectangle2D( 342, 168, 16, 16 );

				int x = Utility.Random( rect.X, rect.Width );
				int y = Utility.Random( rect.Y, rect.Height );

				if ( x >= 345 && x <= 352 && y >= 173 && y <= 179 )
				{
					x = 353;
					y = 172;
				}

				this.MoveToWorld( new Point3D( x, y, -1 ), Map.Malas );
			}

			// Professions weren't verified on 1.0 RC0
			if ( !CharacterCreation.VerifyProfession( m_Profession ) )
				m_Profession = 0;

			if ( m_PermaFlags == null )
				m_PermaFlags = new ArrayList();

			if ( m_JusticeProtectors == null )
				m_JusticeProtectors = new ArrayList();

			if ( m_BOBFilter == null )
				m_BOBFilter = new Engines.BulkOrders.BOBFilter();

			if ( m_LoyaltyInfo == null )
				m_LoyaltyInfo = new LoyaltyInfo();

			if ( m_TieredQuestInfo == null )
				m_TieredQuestInfo = new TieredQuestInfo();

			List<Mobile> list = this.Stabled;

			for ( int i = 0; i < list.Count; ++i )
			{
				BaseCreature bc = list[i] as BaseCreature;

				if ( bc != null )
					bc.IsStabled = true;
			}

			if ( Hidden ) // Hiding is the only buff where it has an effect that's serialized.
				this.AddBuff( new BuffInfo( BuffIcon.HidingAndOrStealth, 1075655 ) );
		}

		private void ClearAntiMacroTable()
		{
			foreach ( Hashtable t in m_AntiMacroTable.Values )
			{
				ArrayList remove = new ArrayList();

				foreach ( CountAndTimeStamp time in t.Values )
				{
					if ( time.TimeStamp + SkillCheck.AntiMacroExpire <= DateTime.UtcNow )
						remove.Add( time );
				}

				for ( int i = 0; i < remove.Count; ++i )
					t.Remove( remove[i] );
			}
		}

		private void DecayKills()
		{
			if ( m_ShortTermElapse < this.GameTime )
			{
				m_ShortTermElapse += TimeSpan.FromHours( 8 );

				if ( ShortTermMurders > 0 )
					--ShortTermMurders;
			}

			if ( m_LongTermElapse < this.GameTime )
			{
				m_LongTermElapse += TimeSpan.FromHours( 40 );

				if ( Kills > 0 )
					--Kills;
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			ClearAntiMacroTable();
			DecayKills();

			base.Serialize( writer );

			writer.Write( (int) 56 ); // version

			/* Version 56 */
			bool hasAnyBardMastery = ( m_BardMastery != null );
			writer.Write( (bool) hasAnyBardMastery );

			if ( hasAnyBardMastery )
			{
				writer.WriteEncodedInt( (int) m_BardMastery.Id );
				writer.WriteEncodedInt( (int) m_BardElementDamage );
				writer.Write( (DateTime) m_NextBardMasterySwitch );
				writer.WriteEncodedInt( (int) m_BardMasteryLearnedMask );
			}

			/* Version 55 */
			writer.Write( (DateTime) m_NextAnkhPendantUse );

			/* Version 54 */
			writer.Write( (DateTime) m_NextTenthAnniversarySculptureUse );

			/* Version 53 */
			writer.Write( (DateTime) m_LastForgedPardonUse );

			/* Version 51 */
			writer.Write( (int) m_DoomCredits );

			/* Version 50 */
			TieredQuestInfo.Serialize( writer, m_TieredQuestInfo );

			/* Version 49 */
			LoyaltyInfo.Serialize( writer, m_LoyaltyInfo );

			/* Version 48 */
			writer.Write( (int) m_FloorTrapsPlaced );

			/* Version 47 */
			writer.Write( (DateTime) m_NextPuzzleAttempt );

			/* Version 45 */
			writer.Write( (DateTime) m_NextDrinkExplodingTarPotion );

			/* Version 44 */
			writer.Write( (DateTime) m_NextLuckyCoinWish );

			/* Version 43 */
			writer.Write( (DateTime) m_NextSilverSaplingUse );

			/* Version 42 */
			writer.Write( (DateTime) m_SacredQuestNextChance );

			/* Version 41 */
			writer.Write( (int) m_HumilityQuestStatus );
			writer.Write( (DateTime) m_HumilityQuestNextChance );

			/* Version 40 */
			writer.Write( (DateTime) m_NextGemOfSalvationUse );

			/* Version 39 */
			writer.Write( (DateTime) m_AnkhNextUse );

			/* Version 38 */
			QuestWriter.Quests( writer, m_Quests );
			QuestWriter.Chains( writer, m_Chains );

			/* Version 36 */
			writer.Write( (double) m_TenthAnniversaryCredits );

			/* Version 35 */
			writer.Write( (byte) m_KRStartingQuestStep );

			/* Version 31 */
			writer.Write( (int) m_CurrentCollectionTitle );
			writer.Write( (int) m_CollectionTitles.Count );

			for ( int i = 0; i < m_CollectionTitles.Count; i++ )
				writer.Write( (int) m_CollectionTitles[i] );

			/* Version 28 */
			if ( m_AcquiredRecipes == null )
			{
				writer.Write( (int) 0 );
			}
			else
			{
				writer.Write( m_AcquiredRecipes.Count );

				foreach ( object o in m_AcquiredRecipes.Keys )
				{
					int i = (int) o;

					writer.Write( i );
					writer.Write( (bool) m_AcquiredRecipes[i] );
				}
			}

			if ( m_AutoStabledPets == null )
				m_AutoStabledPets = new ArrayList();

			writer.WriteMobileList( m_AutoStabledPets, true );

			writer.Write( (int) ACState );

			writer.Write( (int) Deaths );

			writer.Write( NextDrinkConflagrationPotion );
			writer.Write( NextDrinkMaskOfDeathPotion );
			writer.Write( NextDrinkConfusionPotion );

			writer.Write( (int) m_GuildRank );

			writer.Write( (DateTime) m_LastLogin );

			writer.Write( (int) m_ToTItemsTurnedIn );
			writer.Write( (double) m_ToTTotalMonsterFame );

			writer.WriteDeltaTime( LastTierLoss );
			writer.WriteDeltaTime( LastChampionTierLoss );
			writer.WriteDeltaTime( LastSuperChampionTierLoss );

			writer.Write( (int) SuperChampionTiers.Length );

			for ( int i = 0; i < SuperChampionTiers.Length; i++ )
				writer.Write( (int) SuperChampionTiers[i] );

			writer.Write( (int) ChampionTiers.Length );

			for ( int i = 0; i < ChampionTiers.Length; i++ )
				writer.Write( (double) ChampionTiers[i] );

			writer.WriteDeltaTime( m_LastValorLoss );

			writer.WriteDeltaTime( m_LastHonorLoss );

			writer.WriteEncodedInt( (int) m_SolenFriendship );

			QuestSerializer.Serialize( m_Quest, writer );

			if ( m_DoneQuests == null )
			{
				writer.WriteEncodedInt( (int) 0 );
			}
			else
			{
				writer.WriteEncodedInt( (int) m_DoneQuests.Count );

				for ( int i = 0; i < m_DoneQuests.Count; ++i )
				{
					QuestRestartInfo restartInfo = m_DoneQuests[i];

					QuestSerializer.Write( (Type) restartInfo.QuestType, QuestSystem.QuestTypes, writer );
					writer.Write( (DateTime) restartInfo.RestartTime );
				}
			}

			writer.WriteEncodedInt( (int) m_Profession );

			writer.WriteDeltaTime( m_LastCompassionLoss );

			writer.WriteEncodedInt( m_CompassionGains );

			if ( m_CompassionGains > 0 )
				writer.WriteDeltaTime( m_NextCompassionDay );

			m_BOBFilter.Serialize( writer );

			bool useMods = ( m_HairModID != -1 || m_BeardModID != -1 );

			writer.Write( useMods );

			if ( useMods )
			{
				writer.Write( (int) m_HairModID );
				writer.Write( (int) m_HairModHue );
				writer.Write( (int) m_BeardModID );
				writer.Write( (int) m_BeardModHue );
			}

			writer.Write( SavagePaintExpiration );

			writer.Write( (int) m_NpcGuild );
			writer.Write( (DateTime) m_NpcGuildJoinTime );
			writer.Write( (TimeSpan) m_NpcGuildGameTime );

			writer.WriteMobileList( m_PermaFlags, true );

			writer.Write( NextTailorBulkOrder );
			writer.Write( NextSmithBulkOrder );

			writer.WriteDeltaTime( m_LastJusticeLoss );
			writer.WriteMobileList( m_JusticeProtectors, true );

			writer.WriteDeltaTime( m_LastSacrificeGain );
			writer.WriteDeltaTime( m_LastSacrificeLoss );
			writer.Write( m_AvailableResurrects );

			writer.Write( (int) m_Flags );

			writer.Write( m_LongTermElapse );
			writer.Write( m_ShortTermElapse );
			writer.Write( this.GameTime );
		}

		public void ResetKillTime()
		{
			m_ShortTermElapse = this.GameTime + TimeSpan.FromHours( 8 );
			m_LongTermElapse = this.GameTime + TimeSpan.FromHours( 40 );
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public DateTime SessionStart
		{
			get { return m_SessionStart; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public TimeSpan GameTime
		{
			get
			{
				if ( NetState != null )
					return m_GameTime + ( DateTime.UtcNow - m_SessionStart );
				else
					return m_GameTime;
			}
		}

		public override bool CanSee( Mobile m )
		{
			if ( m is CharacterStatue )
				( (CharacterStatue) m ).OnRequestedAnimation( this );

			if ( m is BaseCreature )
				( (BaseCreature) m ).OnRequestedAnimation( this );

			if ( m is PlayerMobile && ( (PlayerMobile) m ).m_VisList.Contains( this ) )
				return true;

			if ( m is PlayerMobile && !m.Alive && Skills[SkillName.SpiritSpeak].Value >= 100.0 )
				return true;

			return base.CanSee( m );
		}

		public override bool CanSee( Item item )
		{
			if ( m_DesignContext != null && m_DesignContext.Foundation.IsHiddenToCustomizer( item ) )
				return false;

			return base.CanSee( item );
		}

		public override bool CheckSpellCast( ISpell spell )
		{
			if ( FloorTrapKit.IsAssembling( this ) )
				FloorTrapKit.StopAssembling( this, 1113511 ); // You cast a spell and cease trap assembly.

			return base.CheckSpellCast( spell );
		}

		public override void OnAfterDelete()
		{
			base.OnAfterDelete();

			Faction faction = Faction.Find( this );

			if ( faction != null )
				faction.RemoveMember( this );

			BaseHouse.HandleDeletion( this );

			foreach ( CollectionController collection in CollectionController.WorldCollections )
			{
				if ( collection.Table.Contains( this ) )
					collection.Table.Remove( this );
			}
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			if ( m_CurrentCollectionTitle > 0 )
				list.Add( (int) m_CollectionTitles[m_CurrentCollectionTitle] );

			if ( TestCenter.Enabled )
			{
				list.Add( 1060847, String.Format( "Kills: {0}	/ Deaths: {1}", this.Kills, this.Deaths ) );
				list.Add( 1060415, GetMagicalAttribute( MagicalAttribute.AttackChance ).ToString() );
				list.Add( 1060408, GetMagicalAttribute( MagicalAttribute.DefendChance ).ToString() );
				list.Add( 1060486, GetMagicalAttribute( MagicalAttribute.WeaponSpeed ).ToString() ); // TODO: Show it in %
				list.Add( 1060401, GetMagicalAttribute( MagicalAttribute.WeaponDamage ).ToString() );
				list.Add( 1060433, GetMagicalAttribute( MagicalAttribute.LowerManaCost ).ToString() );
			}

			if ( Map == Faction.Facet )
			{
				PlayerState pl = PlayerState.Find( this );

				if ( pl != null )
				{
					Faction faction = pl.Faction;

					if ( faction.Commander == this )
						list.Add( 1042733, faction.Definition.PropName ); // Commanding Lord of the ~1_FACTION_NAME~
					else if ( pl.Sheriff != null )
						list.Add( 1042734, "{0}\t{1}", pl.Sheriff.Definition.FriendlyName, faction.Definition.PropName ); // The Sheriff of  ~1_CITY~, ~2_FACTION_NAME~
					else if ( pl.Finance != null )
						list.Add( 1042735, "{0}\t{1}", pl.Finance.Definition.FriendlyName, faction.Definition.PropName ); // The Finance Minister of ~1_CITY~, ~2_FACTION_NAME~
					else if ( pl.MerchantTitle != MerchantTitle.None )
						list.Add( 1060776, "{0}\t{1}", MerchantTitles.GetInfo( pl.MerchantTitle ).Title, faction.Definition.PropName ); // ~1_val~, ~2_val~
					else
						list.Add( 1060776, "{0}\t{1}", pl.Rank.Title, faction.Definition.PropName ); // ~1_val~, ~2_val~
				}
			}
		}

		protected override bool OnMove( Direction d )
		{
			if ( AccessLevel != AccessLevel.Player )
				return true;

			if ( Hidden )
			{
				if ( !Mounted && Skills.Stealth.Value >= 25.0 )
				{
					bool running = ( d & Direction.Running ) != 0;

					if ( running )
					{
						if ( ( AllowedStealthSteps -= 2 ) <= 0 )
							RevealingAction();
					}
					else if ( AllowedStealthSteps-- <= 0 )
					{
						AllowedStealthSteps = 0;
						Server.SkillHandlers.Stealth.OnUse( this );
					}
				}
				else
				{
					RevealingAction();
				}

				#region Passive detect hidden
				if ( Map == Map.Felucca )
				{
					int hiding = Skills[SkillName.Hiding].Fixed;
					int stealth = Skills[SkillName.Stealth].Fixed;
					int divisor = hiding + stealth;

					foreach ( Mobile m in this.GetMobilesInRange( 4 ) )
					{
						if ( !IsValidPassiveDetector( m ) )
							continue;

						int tracking = m.Skills[SkillName.Tracking].Fixed;
						int detectHidden = m.Skills[SkillName.DetectHidden].Fixed;

						if ( m.Race == Race.Elf )
							tracking = 1000;

						int distance = Math.Max( 1, (int) this.GetDistanceToSqrt( m ) );

						int chance;
						if ( divisor > 0 )
							chance = 50 * ( tracking + detectHidden ) / divisor;
						else
							chance = 100;

						chance /= distance;

						if ( chance > Utility.Random( 100 ) )
						{
							PrivateOverheadMessage( MessageType.Regular, 0x3B2, 500814, this.NetState ); // You have been revealed!
							RevealingAction();

							break;
						}
					}
				}
				#endregion
			}

			if ( FloorTrapKit.IsAssembling( this ) )
				FloorTrapKit.StopAssembling( this, 1113509 ); // You move away and cease trap assembly.

			Spells.Ninjitsu.DeathStrike.AddStep( this );

			return true;
		}

		#region Passive detect hidden
		private bool IsValidPassiveDetector( Mobile from )
		{
			if ( from == this || from.AccessLevel > AccessLevel.Player )
				return false;

			if ( from.Hidden || !from.Alive || !from.IsPlayer )
				return false;

			Party party = from.Party as Party;
			if ( party != null && party.Contains( this ) )
				return false;

			Guild guild = from.Guild as Guild;
			if ( guild != null )
			{
				if ( guild.IsMember( this ) )
					return false;

				Guild ourGuild = this.Guild as Guild;
				if ( ourGuild != null && ourGuild.IsAlly( guild ) )
					return false;
			}

			return true;
		}
		#endregion

		private bool m_BedrollLogout;

		public bool BedrollLogout
		{
			get { return m_BedrollLogout; }
			set { m_BedrollLogout = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public override bool Paralyzed
		{
			get
			{
				return base.Paralyzed;
			}
			set
			{
				base.Paralyzed = value;

				if ( value )
					this.AddBuff( new BuffInfo( BuffIcon.Paralyze, 1075827 ) ); //Paralyze/You are frozen and can not move
				else
					this.RemoveBuff( BuffIcon.Paralyze );
			}
		}

		public override bool IsBeneficialCriminal( Mobile target )
		{
			if ( target is BaseCreature && ( (BaseCreature) target ).ControlMaster == this )
				return false;

			return base.IsBeneficialCriminal( target );
		}

		#region Factions
		private PlayerState m_FactionPlayerState;

		public PlayerState FactionPlayerState
		{
			get { return m_FactionPlayerState; }
			set { m_FactionPlayerState = value; }
		}
		#endregion

		#region Quests
		private QuestSystem m_Quest;
		private List<QuestRestartInfo> m_DoneQuests;
		private SolenFriendship m_SolenFriendship;

		public QuestSystem Quest
		{
			get { return m_Quest; }
			set { m_Quest = value; }
		}

		public List<QuestRestartInfo> DoneQuests
		{
			get { return m_DoneQuests; }
			set { m_DoneQuests = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public SolenFriendship SolenFriendship
		{
			get { return m_SolenFriendship; }
			set { m_SolenFriendship = value; }
		}
		#endregion

		#region MyRunUO Invalidation
		private bool m_ChangedMyRunUO;

		public bool ChangedMyRunUO
		{
			get { return m_ChangedMyRunUO; }
			set { m_ChangedMyRunUO = value; }
		}

		public void InvalidateMyRunUO()
		{
			if ( !Deleted && !m_ChangedMyRunUO )
			{
				m_ChangedMyRunUO = true;
				//Engines.MyRunUO.MyRunUO.QueueMobileUpdate( this );
			}
		}

		public override void OnKillsChange( int oldValue )
		{
			if ( this.Young && this.Kills > oldValue )
			{
				Account acc = this.Account as Account;

				if ( acc != null )
					acc.RemoveYoungStatus( 0 );
			}

			InvalidateMyRunUO();
		}

		public override void OnGenderChanged( bool oldFemale )
		{
			InvalidateMyRunUO();
		}

		public override void OnGuildChange( Server.Guilds.BaseGuild oldGuild )
		{
			InvalidateMyRunUO();
		}

		public override void OnGuildTitleChange( string oldTitle )
		{
			InvalidateMyRunUO();
		}

		public override void OnKarmaChange( int oldValue )
		{
			InvalidateMyRunUO();
		}

		public override void OnFameChange( int oldValue )
		{
			InvalidateMyRunUO();
		}

		public override void OnSkillChange( SkillName skill, double oldBase )
		{
			if ( this.Young && this.SkillsTotal >= 4500 )
			{
				Account acc = this.Account as Account;

				if ( acc != null )
					acc.RemoveYoungStatus( 1019036 ); // You have successfully obtained a respectable skill level, and have outgrown your status as a young player!
			}

			InvalidateMyRunUO();
		}

		public override void OnAccessLevelChanged( AccessLevel oldLevel )
		{
			InvalidateMyRunUO();
		}

		public override void OnRawStatChange( StatType stat, int oldValue )
		{
			InvalidateMyRunUO();
		}

		public override void OnDelete()
		{
			if ( m_ReceivedHonorContext != null )
				m_ReceivedHonorContext.Cancel();
			if ( m_SentHonorContext != null )
				m_SentHonorContext.Cancel();

			InvalidateMyRunUO();
		}
		#endregion

		#region Fastwalk Prevention
		public static bool FastwalkPrevention = true; // Is fastwalk prevention enabled?
		public static TimeSpan FastwalkThreshold = TimeSpan.FromSeconds( 0.04 ); // Fastwalk prevention will become active after 0.4 seconds

		private DateTime m_NextMovementTime;

		public virtual bool UsesFastwalkPrevention { get { return FastwalkPrevention && ( AccessLevel < AccessLevel.Counselor ); } }

		public override TimeSpan ComputeMovementSpeed( Direction dir, bool checkTurning )
		{
			bool running = ( ( dir & Direction.Running ) != 0 );
			bool mounted = ( Mounted || Flying );

			if ( checkTurning && ( dir & Direction.Mask ) != ( this.Direction & Direction.Mask ) )
				return TimeSpan.FromSeconds( 0.1 );

			if ( ForcedWalk )
				return ( mounted ? Mobile.WalkMount : Mobile.WalkFoot );

			if ( ForcedRun || mounted )
				return ( running ? Mobile.RunMount : Mobile.WalkMount );

			return ( running ? Mobile.RunFoot : Mobile.WalkFoot );
		}

		public static bool MovementThrottle_Callback( NetState ns )
		{
			PlayerMobile pm = ns.Mobile as PlayerMobile;

			if ( pm == null || !pm.UsesFastwalkPrevention )
				return true;

			return ( pm.m_NextMovementTime < DateTime.UtcNow );
		}
		#endregion

		#region Battle Lust
		private int m_BattleLust;
		private DateTime m_LastBattleLustGain;
		private BattleLustTimer m_BattleLustTimer;

		[CommandProperty( AccessLevel.GameMaster )]
		public int BattleLust
		{
			get { return m_BattleLust; }
			set
			{
				m_BattleLust = value;

				if ( m_BattleLust < 0 )
					m_BattleLust = 0;

				if ( m_BattleLust > 0 && m_BattleLustTimer == null )
				{
					m_BattleLustTimer = new BattleLustTimer( this );
					m_BattleLustTimer.Start();
				}
				else if ( m_BattleLust == 0 && m_BattleLustTimer != null )
				{
					m_BattleLustTimer.Stop();
					m_BattleLustTimer = null;
				}
			}
		}

		public void GainBattleLust()
		{
			BaseWeapon weapon = Weapon as BaseWeapon;

			if ( weapon != null && weapon.WeaponAttributes[WeaponAttribute.BattleLust] != 0 )
			{
				if ( DateTime.UtcNow > ( m_LastBattleLustGain + TimeSpan.FromSeconds( 2.0 ) ) )
				{
					SendLocalizedMessage( 1113748 ); // The damage you received fuels your battle fury.
					BattleLust++;

					m_LastBattleLustGain = DateTime.UtcNow;
				}
			}
		}

		public int GetBattleLust( Mobile defender )
		{
			BaseWeapon weapon = Weapon as BaseWeapon;

			if ( weapon != null && weapon.WeaponAttributes[WeaponAttribute.BattleLust] != 0 )
			{
				int lust = Math.Min( m_BattleLust, 15 * Aggressors.Count );

				return Math.Min( lust, defender.IsPlayer ? 45 : 90 );
			}

			return 0;
		}

		public class BattleLustTimer : Timer
		{
			private PlayerMobile m_Owner;

			public BattleLustTimer( PlayerMobile owner )
				: base( TimeSpan.FromSeconds( 6.0 ), TimeSpan.FromSeconds( 6.0 ) )
			{
				m_Owner = owner;
			}

			protected override void OnTick()
			{
				m_Owner.BattleLust--;
			}
		}
		#endregion

		#region Damage Eater
		public class DamageEaterTimer : Timer
		{
			private PlayerMobile m_Owner;
			private Queue<int> m_HealingCharges;

			public void EnqueueCharge( int amount )
			{
				if ( m_HealingCharges.Count < 20 )
					m_HealingCharges.Enqueue( amount );
			}

			public DamageEaterTimer( PlayerMobile owner )
				: base( TimeSpan.FromSeconds( 2.0 ), TimeSpan.FromSeconds( 2.0 ) )
			{
				m_Owner = owner;

				m_HealingCharges = new Queue<int>();
			}

			protected override void OnTick()
			{
				if ( !m_Owner.Alive || m_Owner.Hits == m_Owner.HitsMax )
					m_HealingCharges.Clear();

				if ( m_HealingCharges.Count == 0 )
				{
					Stop();
				}
				else
				{
					int amount = m_HealingCharges.Dequeue();

					m_Owner.Hits += amount;

					m_Owner.FixedParticles( 0x375A, 1, 10, 0x1AE9, 0x21, 2, EffectLayer.Head, 0 );
					m_Owner.SendLocalizedMessage( 1113617 ); // Some of the damage you received has been converted to heal you.
				}
			}
		}

		private DamageEaterTimer m_DamageEaterTimer;

		public void EatDamage( int amount )
		{
			if ( amount > 0 )
			{
				if ( m_DamageEaterTimer == null )
					m_DamageEaterTimer = new DamageEaterTimer( this );
				else
					m_DamageEaterTimer.Stop();

				m_DamageEaterTimer.EnqueueCharge( amount );
				m_DamageEaterTimer.Start();
			}
		}
		#endregion

		#region Hair and beard mods
		private int m_HairModID = -1, m_HairModHue;
		private int m_BeardModID = -1, m_BeardModHue;

		public void SetHairMods( int hairID, int beardID )
		{
			if ( hairID == -1 )
				InternalRestoreHair( true, ref m_HairModID, ref m_HairModHue );
			else if ( hairID != -2 )
				InternalChangeHair( true, hairID, ref m_HairModID, ref m_HairModHue );

			if ( beardID == -1 )
				InternalRestoreHair( false, ref m_BeardModID, ref m_BeardModHue );
			else if ( beardID != -2 )
				InternalChangeHair( false, beardID, ref m_BeardModID, ref m_BeardModHue );
		}

		private void CreateHair( bool hair, int id, int hue )
		{
			if ( hair )
			{
				//TODO Verification?
				HairItemID = id;
				HairHue = hue;
			}
			else
			{
				FacialHairItemID = id;
				FacialHairHue = hue;
			}
		}

		private void InternalRestoreHair( bool hair, ref int id, ref int hue )
		{
			if ( id == -1 )
				return;

			if ( hair )
				HairItemID = 0;
			else
				FacialHairItemID = 0;

			//if( id != 0 )
			CreateHair( hair, id, hue );

			id = -1;
			hue = 0;
		}

		private void InternalChangeHair( bool hair, int id, ref int storeID, ref int storeHue )
		{
			if ( storeID == -1 )
			{
				storeID = hair ? HairItemID : FacialHairItemID;
				storeHue = hair ? HairHue : FacialHairHue;
			}
			CreateHair( hair, id, 0 );
		}
		#endregion

		#region Virtues
		private DateTime m_LastSacrificeGain;
		private DateTime m_LastSacrificeLoss;
		private int m_AvailableResurrects;

		public DateTime LastSacrificeGain { get { return m_LastSacrificeGain; } set { m_LastSacrificeGain = value; } }
		public DateTime LastSacrificeLoss { get { return m_LastSacrificeLoss; } set { m_LastSacrificeLoss = value; } }
		public int AvailableResurrects { get { return m_AvailableResurrects; } set { m_AvailableResurrects = value; } }

		private DateTime m_NextJustAward;
		private DateTime m_LastJusticeLoss;
		private ArrayList m_JusticeProtectors;

		public DateTime LastJusticeLoss { get { return m_LastJusticeLoss; } set { m_LastJusticeLoss = value; } }
		public ArrayList JusticeProtectors { get { return m_JusticeProtectors; } set { m_JusticeProtectors = value; } }

		private DateTime m_LastCompassionLoss;
		private DateTime m_NextCompassionDay;
		private int m_CompassionGains;

		public DateTime LastCompassionLoss { get { return m_LastCompassionLoss; } set { m_LastCompassionLoss = value; } }
		public DateTime NextCompassionDay { get { return m_NextCompassionDay; } set { m_NextCompassionDay = value; } }
		public int CompassionGains { get { return m_CompassionGains; } set { m_CompassionGains = value; } }

		private DateTime m_LastValorLoss;

		public DateTime LastValorLoss { get { return m_LastValorLoss; } set { m_LastValorLoss = value; } }

		private DateTime m_LastHonorLoss;
		private DateTime m_LastHonorUse;
		private bool m_HonorActive;
		private HonorContext m_ReceivedHonorContext;
		private HonorContext m_SentHonorContext;

		public DateTime LastHonorLoss { get { return m_LastHonorLoss; } set { m_LastHonorLoss = value; } }
		public DateTime LastHonorUse { get { return m_LastHonorUse; } set { m_LastHonorUse = value; } }
		public bool HonorActive { get { return m_HonorActive; } set { m_HonorActive = value; } }
		public HonorContext ReceivedHonorContext { get { return m_ReceivedHonorContext; } set { m_ReceivedHonorContext = value; } }
		public HonorContext SentHonorContext { get { return m_SentHonorContext; } set { m_SentHonorContext = value; } }
		#endregion

		#region Young system
		[CommandProperty( AccessLevel.GameMaster )]
		public bool Young
		{
			get { return GetFlag( PlayerFlag.Young ); }
			set { SetFlag( PlayerFlag.Young, value ); InvalidateProperties(); }
		}

		public override string ApplyNameSuffix( string suffix )
		{
			if ( Young )
			{
				if ( suffix.Length == 0 )
					suffix = "(Young)";
				else
					suffix = String.Concat( suffix, " (Young)" );
			}

			#region Factions
			if ( this.Map == Faction.Facet )
			{
				Faction faction = Faction.Find( this );

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


		public override TimeSpan GetLogoutDelay()
		{
			if ( Young || BedrollLogout )
				return TimeSpan.Zero;

			return base.GetLogoutDelay();
		}

		private DateTime m_LastYoungMessage = DateTime.MinValue;

		public bool CheckYoungProtection( Mobile from )
		{
			if ( !this.Young )
				return false;

			if ( Region.IsPartOf( typeof( DungeonRegion ) ) )
				return false;

			if ( this.Quest != null && this.Quest.IgnoreYoungProtection( from ) )
				return false;

			if ( m_KRStartingQuestStep > 0 )
				return false;

			if ( DateTime.UtcNow - m_LastYoungMessage > TimeSpan.FromMinutes( 1.0 ) )
			{
				m_LastYoungMessage = DateTime.UtcNow;
				SendLocalizedMessage( 1019067 ); // A monster looks at you menacingly but does not attack.  You would be under attack now if not for your status as a new citizen of Britannia.
			}

			return true;
		}

		private DateTime m_LastYoungHeal = DateTime.MinValue;

		public bool CheckYoungHealTime()
		{
			if ( DateTime.UtcNow - m_LastYoungHeal > TimeSpan.FromMinutes( 5.0 ) )
			{
				m_LastYoungHeal = DateTime.UtcNow;
				return true;
			}

			return false;
		}

		private static Point3D[] m_TrammelDeathDestinations = new Point3D[]
			{
				new Point3D( 1481, 1612, 20 ),
				new Point3D( 2708, 2153,  0 ),
				new Point3D( 2249, 1230,  0 ),
				new Point3D( 5197, 3994, 37 ),
				new Point3D( 1412, 3793,  0 ),
				new Point3D( 3688, 2232, 20 ),
				new Point3D( 2578,  604,  0 ),
				new Point3D( 4397, 1089,  0 ),
				new Point3D( 5741, 3218, -2 ),
				new Point3D( 2996, 3441, 15 ),
				new Point3D(  624, 2225,  0 ),
				new Point3D( 1916, 2814,  0 ),
				new Point3D( 2929,  854,  0 ),
				new Point3D(  545,  967,  0 ),
				new Point3D( 3469, 2559, 36 )
			};

		private static Point3D[] m_IlshenarDeathDestinations = new Point3D[]
			{
				new Point3D( 1216,  468, -13 ),
				new Point3D(  723, 1367, -60 ),
				new Point3D(  745,  725, -28 ),
				new Point3D(  281, 1017,   0 ),
				new Point3D(  986, 1011, -32 ),
				new Point3D( 1175, 1287, -30 ),
				new Point3D( 1533, 1341,  -3 ),
				new Point3D(  529,  217, -44 ),
				new Point3D( 1722,  219,  96 )
			};

		private static Point3D[] m_MalasDeathDestinations = new Point3D[]
			{
				new Point3D( 2079, 1376, -70 ),
				new Point3D(  944,  519, -71 )
			};

		private static Point3D[] m_TokunoDeathDestinations = new Point3D[]
			{
				new Point3D( 1166,  801, 27 ),
				new Point3D(  782, 1228, 25 ),
				new Point3D(  268,  624, 15 )
			};

		public Point3D GetYoungDeathDestination()
		{
			if ( this.Region.IsPartOf( typeof( Regions.Jail ) ) )
				return Point3D.Zero;

			Point3D[] list;

			if ( this.Map == Map.Trammel )
				list = m_TrammelDeathDestinations;
			else if ( this.Map == Map.Ilshenar )
				list = m_IlshenarDeathDestinations;
			else if ( this.Map == Map.Malas )
				list = m_MalasDeathDestinations;
			else if ( this.Map == Map.Tokuno )
				list = m_TokunoDeathDestinations;
			else
				return Point3D.Zero;

			IPoint2D loc;

			if ( this.Region == null )
			{
				loc = this.Location;
			}
			else
			{
				string regName = this.Region.Name;

				// If the character is in a dungeon, get the entrance location
				switch ( regName )
				{
					case "Covetous":
						loc = new Point2D( 2499, 916 );
						break;
					case "Deceit":
						loc = new Point2D( 4111, 429 );
						break;
					case "Despise":
						loc = new Point2D( 1296, 1082 );
						break;
					case "Destard":
						loc = new Point2D( 1176, 2635 );
						break;
					case "Hythloth":
						loc = new Point2D( 4722, 3814 );
						break;
					case "Shame":
						loc = new Point2D( 512, 1559 );
						break;
					case "Wrong":
						loc = new Point2D( 2042, 226 );
						break;
					case "Terathan Keep":
						loc = new Point2D( 5426, 3120 );
						break;
					case "Fire":
						loc = new Point2D( 2922, 3402 );
						break;
					case "Ice":
						loc = new Point2D( 1996, 80 );
						break;
					case "Orc Cave":
						loc = new Point2D( 1014, 1434 );
						break;
					case "Misc Dungeons":
						loc = new Point2D( 1492, 1641 );
						break;
					case "Rock Dungeon":
						loc = new Point2D( 1788, 571 );
						break;
					case "Spider Cave":
						loc = new Point2D( 1420, 910 );
						break;
					case "Spectre Dungeon":
						loc = new Point2D( 1362, 1031 );
						break;
					case "Blood Dungeon":
						loc = new Point2D( 1745, 1236 );
						break;
					case "Wisp Dungeon":
						loc = new Point2D( 652, 1301 );
						break;
					case "Ankh Dungeon":
						loc = new Point2D( 668, 928 );
						break;
					case "Exodus Dungeon":
						loc = new Point2D( 827, 777 );
						break;
					case "Sorcerer's Dungeon":
						loc = new Point2D( 546, 455 );
						break;
					case "Ancient Lair":
						loc = new Point2D( 938, 494 );
						break;
					case "Doom":
					case "Doom Gauntlet":
						loc = new Point2D( 2357, 1268 );
						break;
					default:
						loc = this.Location;
						break;
				}
			}

			Point3D dest = Point3D.Zero;
			int sqDistance = int.MaxValue;

			for ( int i = 0; i < list.Length; i++ )
			{
				Point3D curDest = list[i];

				int width = loc.X - curDest.X;
				int height = loc.Y - curDest.Y;
				int curSqDistance = width * width + height * height;

				if ( curSqDistance < sqDistance )
				{
					dest = curDest;
					sqDistance = curSqDistance;
				}
			}

			return dest;
		}

		private void SendYoungDeathNotice()
		{
			this.SendGump( new YoungDeathNotice() );
		}
		#endregion

		#region Speech log
		private SpeechLog m_SpeechLog;

		public SpeechLog SpeechLog { get { return m_SpeechLog; } }

		public override void OnSpeech( SpeechEventArgs e )
		{
			if ( SpeechLog.Enabled && this.NetState != null )
			{
				if ( m_SpeechLog == null )
					m_SpeechLog = new SpeechLog();

				m_SpeechLog.Add( e.Mobile, e.Speech );
			}
		}
		#endregion

		#region Champion Titles
		public double[] ChampionTiers = new double[11];
		public int[] SuperChampionTiers = new int[12];
		public DateTime LastTierLoss;
		public DateTime LastChampionTierLoss;
		public DateTime LastSuperChampionTierLoss;
		#endregion

		#region Recipes

		private Hashtable m_AcquiredRecipes;

		public virtual bool HasRecipe( Recipe r )
		{
			if ( r == null )
				return false;

			return HasRecipe( r.ID );
		}

		public virtual bool HasRecipe( int recipeID )
		{
			if ( m_AcquiredRecipes != null && m_AcquiredRecipes.ContainsKey( recipeID ) )
				return (bool) m_AcquiredRecipes[recipeID];

			return false;
		}

		public virtual void AcquireRecipe( Recipe r )
		{
			if ( r != null )
				AcquireRecipe( r.ID );
		}

		public virtual void AcquireRecipe( int recipeID )
		{
			if ( m_AcquiredRecipes == null )
				m_AcquiredRecipes = new Hashtable();

			m_AcquiredRecipes.Add( recipeID, true );
		}

		public virtual void ResetRecipes()
		{
			m_AcquiredRecipes = null;
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int KnownRecipes
		{
			get
			{
				if ( m_AcquiredRecipes == null )
					return 0;

				return m_AcquiredRecipes.Count;
			}
		}


		#endregion
	}
}
