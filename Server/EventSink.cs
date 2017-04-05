using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using Server;
using Server.Accounting;
using Server.Commands;
using Server.Network;
using Server.Guilds;

namespace Server.Events
{
	public delegate void DeletedEventHandler( DeletedEventArgs e );
	public delegate void BeforeDamageEventHandler( BeforeDamageEventArgs e );
	public delegate void CreateCharRequestEventHandler( CreateCharRequestEventArgs e );
	public delegate void CharacterCreatedEventHandler( CharacterCreatedEventArgs e );
	public delegate void OpenDoorMacroEventHandler( OpenDoorMacroEventArgs e );
	public delegate void SpeechEventHandler( SpeechEventArgs e );
	public delegate void LoginEventHandler( LoginEventArgs e );
	public delegate void ServerListEventHandler( ServerListEventArgs e );
	public delegate void MovementEventHandler( MovementEventArgs e );
	public delegate void HungerChangedEventHandler( HungerChangedEventArgs e );
	public delegate void MapChangedEventHandler( MapChangedEventArgs e );
	public delegate void CrashedEventHandler( CrashedEventArgs e );
	public delegate void ShutdownEventHandler( ShutdownEventArgs e );
	public delegate void HelpRequestEventHandler( HelpRequestEventArgs e );
	public delegate void DisarmRequestEventHandler( DisarmRequestEventArgs e );
	public delegate void StunRequestEventHandler( StunRequestEventArgs e );
	public delegate void OpenSpellbookRequestEventHandler( OpenSpellbookRequestEventArgs e );
	public delegate void CastSpellRequestEventHandler( CastSpellRequestEventArgs e );
	public delegate void AnimateRequestEventHandler( AnimateRequestEventArgs e );
	public delegate void LogoutEventHandler( LogoutEventArgs e );
	public delegate void SocketConnectEventHandler( SocketConnectEventArgs e );
	public delegate void ConnectedEventHandler( ConnectedEventArgs e );
	public delegate void DisconnectedEventHandler( DisconnectedEventArgs e );
	public delegate void RenameRequestEventHandler( RenameRequestEventArgs e );
	public delegate void PlayerDeathEventHandler( PlayerDeathEventArgs e );
	public delegate void VirtueGumpRequestEventHandler( VirtueGumpRequestEventArgs e );
	public delegate void VirtueItemRequestEventHandler( VirtueItemRequestEventArgs e );
	public delegate void VirtueMacroEventHandler( VirtueMacroEventArgs e );
	public delegate void ChatRequestEventHandler( ChatRequestEventArgs e );
	public delegate void AccountLoginEventHandler( AccountLoginEventArgs e );
	public delegate void PaperdollRequestEventHandler( PaperdollRequestEventArgs e );
	public delegate void ProfileRequestEventHandler( ProfileRequestEventArgs e );
	public delegate void ChangeProfileRequestEventHandler( ChangeProfileRequestEventArgs e );
	public delegate void AggressiveActionEventHandler( AggressiveActionEventArgs e );
	public delegate void HarmfulActionEventHandler( HarmfulActionEventArgs e );
	public delegate void GameLoginEventHandler( GameLoginEventArgs e );
	public delegate void DeleteRequestEventHandler( DeleteRequestEventArgs e );
	public delegate void WorldLoadEventHandler();
	public delegate void WorldBeforeSaveEventHandler();
	public delegate void WorldSaveEventHandler( WorldSaveEventArgs e );
	public delegate void SetAbilityEventHandler( SetAbilityEventArgs e );
	public delegate void FastWalkEventHandler( FastWalkEventArgs e );
	public delegate void ServerStartedEventHandler();
	public delegate BaseGuild CreateGuildHandler( CreateGuildEventArgs e );
	public delegate void GuildGumpRequestHandler( GuildGumpRequestArgs e );
	public delegate void QuestGumpRequestHandler( QuestGumpRequestArgs e );
	public delegate void EquipLastWeaponMacroEventHandler( EquipLastWeaponMacroEventArgs e );
	public delegate void EquipMacroEventHandler( EquipMacroEventArgs e );
	public delegate void UnequipMacroEventHandler( UnequipMacroEventArgs e );
	public delegate void TargetByResourceMacroEventHandler( TargetByResourceMacroEventArgs e );
	public delegate void RacialAbilityRequestEventHandler( RacialAbilityRequestEventArgs e );
	public delegate void BoatMovementRequestEventHandler( BoatMovementRequestEventArgs e );
	public delegate void ClientVersionReceivedHandler( ClientVersionReceivedArgs e );
	public delegate void OPLRequestHandler( OPLRequestArgs e );
	public delegate void PoisonCuredEventHandler( PoisonCuredEventArgs e );

	public class PoisonCuredEventArgs : EventArgs
	{
		public Mobile Mobile { get; }
		public Poison Poison { get; }

		public PoisonCuredEventArgs( Mobile mobile, Poison poison )
		{
			Mobile = mobile;
			Poison = poison;
		}
	}

	public class OPLRequestArgs : EventArgs
	{
		public IEntity Entity { get; }

		public ObjectPropertyList List { get; }

		public OPLRequestArgs( IEntity entity, ObjectPropertyList list )
		{
			Entity = entity;
			List = list;
		}
	}

	public class ClientVersionReceivedArgs : EventArgs
	{
		public NetState State { get; }

		public ClientVersion Version { get; }

		public ClientVersionReceivedArgs( NetState state, ClientVersion cv )
		{
			State = state;
			Version = cv;
		}
	}

	public class BoatMovementRequestEventArgs : EventArgs
	{
		public Mobile Mobile { get; }

		public Direction Direction { get; }

		public int Speed { get; }

		public BoatMovementRequestEventArgs( Mobile m, Direction direction, int speed )
		{
			Mobile = m;
			Direction = direction;
			Speed = speed;
		}
	}

	public class RacialAbilityRequestEventArgs : EventArgs
	{
		public Mobile Mobile { get; }

		public int AbilityID { get; }

		public RacialAbilityRequestEventArgs( Mobile m, int abilityID )
		{
			Mobile = m;
			AbilityID = abilityID;
		}
	}

	public class TargetByResourceMacroEventArgs : EventArgs
	{
		public NetState NetState { get; }

		public Item Tool { get; }

		public int ResourceType { get; }

		public TargetByResourceMacroEventArgs( NetState state, Item tool, int type )
		{
			NetState = state;
			Tool = tool;
			ResourceType = type;
		}
	}

	public class EquipMacroEventArgs : EventArgs
	{
		public NetState NetState { get; }

		public List<int> List { get; }

		public EquipMacroEventArgs( NetState state, List<int> list )
		{
			NetState = state;
			List = list;
		}
	}

	public class UnequipMacroEventArgs : EventArgs
	{
		public NetState NetState { get; }

		public List<int> List { get; }

		public UnequipMacroEventArgs( NetState state, List<int> list )
		{
			NetState = state;
			List = list;
		}
	}

	public class CreateGuildEventArgs : EventArgs
	{
		public int Id { get; set; }

		private static readonly Queue<CreateGuildEventArgs> m_Pool = new Queue<CreateGuildEventArgs>();

		public static CreateGuildEventArgs Create( int id )
		{
			CreateGuildEventArgs args;

			if ( m_Pool.Count > 0 )
			{
				args = m_Pool.Dequeue();

				args.Id = id;
			}
			else
			{
				args = new CreateGuildEventArgs( id );
			}

			return args;
		}

		private CreateGuildEventArgs( int id )
		{
			Id = id;
		}

		public void Free()
		{
			m_Pool.Enqueue( this );
		}
	}

	public class EquipLastWeaponMacroEventArgs : EventArgs
	{
		public Mobile Mobile { get; }

		public EquipLastWeaponMacroEventArgs( Mobile mobile )
		{
			Mobile = mobile;
		}
	}

	public class GuildGumpRequestArgs : EventArgs
	{
		public Mobile Mobile { get; }

		public GuildGumpRequestArgs( Mobile mobile )
		{
			Mobile = mobile;
		}
	}

	public class QuestGumpRequestArgs : EventArgs
	{
		public Mobile Mobile { get; }

		public QuestGumpRequestArgs( Mobile mobile )
		{
			Mobile = mobile;
		}
	}

	public class SetAbilityEventArgs : EventArgs
	{
		public Mobile Mobile { get; }

		public int Index { get; }

		public SetAbilityEventArgs( Mobile mobile, int index )
		{
			Mobile = mobile;
			Index = index;
		}
	}

	public class DeleteRequestEventArgs : EventArgs
	{
		public NetState State { get; }

		public int Index { get; }

		public DeleteRequestEventArgs( NetState state, int index )
		{
			State = state;
			Index = index;
		}
	}

	public class GameLoginEventArgs : EventArgs
	{
		public NetState State { get; }

		public string Username { get; }

		public string Password { get; }

		public bool Accepted { get; set; }

		public CityInfo[] CityInfo { get; set; }

		public GameLoginEventArgs( NetState state, string un, string pw )
		{
			State = state;
			Username = un;
			Password = pw;
		}
	}

	public class AggressiveActionEventArgs : EventArgs
	{
		public Mobile Aggressed { get; private set; }

		public Mobile Aggressor { get; private set; }

		public bool Criminal { get; private set; }

		private static readonly Queue<AggressiveActionEventArgs> m_Pool = new Queue<AggressiveActionEventArgs>();

		public static AggressiveActionEventArgs Create( Mobile aggressed, Mobile aggressor, bool criminal )
		{
			AggressiveActionEventArgs args;

			if ( m_Pool.Count > 0 )
			{
				args = m_Pool.Dequeue();

				args.Aggressed = aggressed;
				args.Aggressor = aggressor;
				args.Criminal = criminal;
			}
			else
			{
				args = new AggressiveActionEventArgs( aggressed, aggressor, criminal );
			}

			return args;
		}

		private AggressiveActionEventArgs( Mobile aggressed, Mobile aggressor, bool criminal )
		{
			Aggressed = aggressed;
			Aggressor = aggressor;
			Criminal = criminal;
		}

		public void Free()
		{
			m_Pool.Enqueue( this );
		}
	}

	public class HarmfulActionEventArgs : EventArgs
	{
		public Mobile Source { get; private set; }

		public Mobile Target { get; private set; }

		public bool Criminal { get; private set; }

		private static readonly Queue<HarmfulActionEventArgs> m_Pool = new Queue<HarmfulActionEventArgs>();

		public static HarmfulActionEventArgs Create( Mobile source, Mobile target, bool criminal )
		{
			HarmfulActionEventArgs args;

			if ( m_Pool.Count > 0 )
			{
				args = m_Pool.Dequeue();

				args.Source = source;
				args.Target = target;
				args.Criminal = criminal;
			}
			else
			{
				args = new HarmfulActionEventArgs( source, target, criminal );
			}

			return args;
		}

		private HarmfulActionEventArgs( Mobile source, Mobile target, bool criminal )
		{
			Source = source;
			Target = target;
			Criminal = criminal;
		}

		public void Free()
		{
			m_Pool.Enqueue( this );
		}
	}

	public class ProfileRequestEventArgs : EventArgs
	{
		public Mobile Beholder { get; }

		public Mobile Beheld { get; }

		public ProfileRequestEventArgs( Mobile beholder, Mobile beheld )
		{
			Beholder = beholder;
			Beheld = beheld;
		}
	}

	public class ChangeProfileRequestEventArgs : EventArgs
	{
		public Mobile Beholder { get; }

		public Mobile Beheld { get; }

		public string Text { get; }

		public ChangeProfileRequestEventArgs( Mobile beholder, Mobile beheld, string text )
		{
			Beholder = beholder;
			Beheld = beheld;
			Text = text;
		}
	}

	public class PaperdollRequestEventArgs : EventArgs
	{
		public Mobile Beholder { get; }

		public Mobile Beheld { get; }

		public PaperdollRequestEventArgs( Mobile beholder, Mobile beheld )
		{
			Beholder = beholder;
			Beheld = beheld;
		}
	}

	public class AccountLoginEventArgs : EventArgs
	{
		public NetState State { get; }

		public string Username { get; }

		public string Password { get; }

		public bool Accepted { get; set; }

		public ALRReason RejectReason { get; set; }

		public AccountLoginEventArgs( NetState state, string un, string pw )
		{
			State = state;
			Username = un;
			Password = pw;
		}
	}

	public class VirtueItemRequestEventArgs : EventArgs
	{
		public Mobile Beholder { get; }

		public Mobile Beheld { get; }

		public int GumpID { get; }

		public VirtueItemRequestEventArgs( Mobile beholder, Mobile beheld, int gumpID )
		{
			Beholder = beholder;
			Beheld = beheld;
			GumpID = gumpID;
		}
	}

	public class VirtueGumpRequestEventArgs : EventArgs
	{
		public Mobile Beholder { get; }

		public Mobile Beheld { get; }

		public VirtueGumpRequestEventArgs( Mobile beholder, Mobile beheld )
		{
			Beholder = beholder;
			Beheld = beheld;
		}
	}

	public class VirtueMacroEventArgs : EventArgs
	{
		public Mobile Mobile { get; }

		public int VirtueID { get; }

		public VirtueMacroEventArgs( Mobile mobile, int virtueID )
		{
			Mobile = mobile;
			VirtueID = virtueID;
		}
	}

	public class ChatRequestEventArgs : EventArgs
	{
		public Mobile Mobile { get; }

		public ChatRequestEventArgs( Mobile mobile )
		{
			Mobile = mobile;
		}
	}

	public class PlayerDeathEventArgs : EventArgs
	{
		public Mobile Mobile { get; }

		public PlayerDeathEventArgs( Mobile mobile )
		{
			Mobile = mobile;
		}
	}

	public class RenameRequestEventArgs : EventArgs
	{
		public Mobile From { get; }

		public Mobile Target { get; }

		public string Name { get; }

		public RenameRequestEventArgs( Mobile from, Mobile target, string name )
		{
			From = from;
			Target = target;
			Name = name;
		}
	}

	public class LogoutEventArgs : EventArgs
	{
		public Mobile Mobile { get; }

		public LogoutEventArgs( Mobile m )
		{
			Mobile = m;
		}
	}

	public class SocketConnectEventArgs : EventArgs
	{
		public Socket Socket { get; }

		public bool AllowConnection { get; set; }

		public SocketConnectEventArgs( Socket s )
		{
			Socket = s;
			AllowConnection = true;
		}
	}

	public class ConnectedEventArgs : EventArgs
	{
		public Mobile Mobile { get; }

		public ConnectedEventArgs( Mobile m )
		{
			Mobile = m;
		}
	}

	public class DisconnectedEventArgs : EventArgs
	{
		public Mobile Mobile { get; }

		public DisconnectedEventArgs( Mobile m )
		{
			Mobile = m;
		}
	}

	public class AnimateRequestEventArgs : EventArgs
	{
		public Mobile Mobile { get; }

		public string Action { get; }

		public AnimateRequestEventArgs( Mobile m, string action )
		{
			Mobile = m;
			Action = action;
		}
	}

	public class CastSpellRequestEventArgs : EventArgs
	{
		public Mobile Mobile { get; }

		public Item Spellbook { get; }

		public int SpellID { get; }

		public IEntity Target { get; }

		public CastSpellRequestEventArgs( Mobile m, int spellID, Item book )
			: this( m, spellID, book, null )
		{
		}

		public CastSpellRequestEventArgs( Mobile m, int spellID, Item book, IEntity target )
		{
			Mobile = m;
			Spellbook = book;
			SpellID = spellID;
			Target = target;
		}
	}

	public class OpenSpellbookRequestEventArgs : EventArgs
	{
		public Mobile Mobile { get; }

		public int Type { get; }

		public OpenSpellbookRequestEventArgs( Mobile m, int type )
		{
			Mobile = m;
			Type = type;
		}
	}

	public class StunRequestEventArgs : EventArgs
	{
		public Mobile Mobile { get; }

		public StunRequestEventArgs( Mobile m )
		{
			Mobile = m;
		}
	}

	public class DisarmRequestEventArgs : EventArgs
	{
		public Mobile Mobile { get; }

		public DisarmRequestEventArgs( Mobile m )
		{
			Mobile = m;
		}
	}

	public class HelpRequestEventArgs : EventArgs
	{
		public Mobile Mobile { get; }

		public HelpRequestEventArgs( Mobile m )
		{
			Mobile = m;
		}
	}

	public class ShutdownEventArgs : EventArgs
	{
		public ShutdownEventArgs()
		{
		}
	}

	public class CrashedEventArgs : EventArgs
	{
		public Exception Exception { get; }

		public bool Close { get; set; }

		public CrashedEventArgs( Exception e )
		{
			Exception = e;
		}
	}

	public class HungerChangedEventArgs : EventArgs
	{
		public Mobile Mobile { get; }

		public int OldValue { get; }

		public HungerChangedEventArgs( Mobile mobile, int oldValue )
		{
			Mobile = mobile;
			OldValue = oldValue;
		}
	}

	public class MapChangedEventArgs : EventArgs
	{
		public IEntity Entity { get; }

		public Map OldMap { get; }

		public MapChangedEventArgs( IEntity entity, Map oldMap )
		{
			Entity = entity;
			OldMap = oldMap;
		}
	}

	public class MovementEventArgs : EventArgs
	{
		public Mobile Mobile { get; private set; }

		public Direction Direction { get; private set; }

		public bool Blocked { get; set; }

		private static readonly Queue<MovementEventArgs> m_Pool = new Queue<MovementEventArgs>();

		public static MovementEventArgs Create( Mobile mobile, Direction dir )
		{
			MovementEventArgs args;

			if ( m_Pool.Count > 0 )
			{
				args = m_Pool.Dequeue();

				args.Mobile = mobile;
				args.Direction = dir;
				args.Blocked = false;
			}
			else
			{
				args = new MovementEventArgs( mobile, dir );
			}

			return args;
		}

		public MovementEventArgs( Mobile mobile, Direction dir )
		{
			Mobile = mobile;
			Direction = dir;
		}

		public void Free()
		{
			m_Pool.Enqueue( this );
		}
	}

	public class ServerListEventArgs : EventArgs
	{
		public NetState State { get; }

		public IAccount Account { get; }

		public bool Rejected { get; set; }

		public List<ServerInfo> Servers { get; }

		public void AddServer( string name, IPEndPoint address )
		{
			AddServer( name, 0, TimeZone.CurrentTimeZone, address );
		}

		public void AddServer( string name, int fullPercent, TimeZone tz, IPEndPoint address )
		{
			Servers.Add( new ServerInfo( name, fullPercent, tz, address ) );
		}

		public ServerListEventArgs( NetState state, IAccount account )
		{
			State = state;
			Account = account;
			Servers = new List<ServerInfo>();
		}
	}

	public struct SkillNameValue
	{
		public SkillName Name { get; }

		public int Value { get; }

		public SkillNameValue( SkillName name, int value )
		{
			Name = name;
			Value = value;
		}
	}

	public class DeletedEventArgs : EventArgs
	{
		public Mobile Mobile { get; }

		public DeletedEventArgs( Mobile mobile )
		{
			Mobile = mobile;
		}
	}

	public class BeforeDamageEventArgs : EventArgs
	{
		public Mobile Mobile { get; }

		public Mobile From { get; }

		public int Amount { get; set; }

		public BeforeDamageEventArgs( Mobile mobile, Mobile from, int amount )
		{
			Mobile = mobile;
			From = from;
			Amount = amount;
		}
	}

	public class CreateCharRequestEventArgs : EventArgs
	{
		public NetState State { get; }

		public IAccount Account { get; }

		public Mobile Mobile { get; set; }

		public string Name { get; }

		public bool Female { get; }

		public int Hue { get; }

		public int Str { get; }

		public int Dex { get; }

		public int Int { get; }

		public CityInfo City { get; }

		public SkillNameValue[] Skills { get; }

		public int ShirtHue { get; }

		public int PantsHue { get; }

		public int HairID { get; }

		public int HairHue { get; }

		public int BeardID { get; }

		public int BeardHue { get; }

		public int Profession { get; set; }

		public Race Race { get; }

		public CreateCharRequestEventArgs( NetState state, IAccount a, string name, bool female, int hue, int str, int dex, int intel, CityInfo city, SkillNameValue[] skills, int shirtHue, int pantsHue, int hairID, int hairHue, int beardID, int beardHue, int profession, Race race )
		{
			State = state;
			Account = a;
			Name = name;
			Female = female;
			Hue = hue;
			Str = str;
			Dex = dex;
			Int = intel;
			City = city;
			Skills = skills;
			ShirtHue = shirtHue;
			PantsHue = pantsHue;
			HairID = hairID;
			HairHue = hairHue;
			BeardID = beardID;
			BeardHue = beardHue;
			Profession = profession;
			Race = race;
		}
	}

	public class CharacterCreatedEventArgs : EventArgs
	{
		public Mobile Mobile { get; }

		public CharacterCreatedEventArgs( Mobile mobile )
		{
			Mobile = mobile;
		}
	}

	public class OpenDoorMacroEventArgs : EventArgs
	{
		public Mobile Mobile { get; }

		public OpenDoorMacroEventArgs( Mobile mobile )
		{
			Mobile = mobile;
		}
	}

	public class SpeechEventArgs : EventArgs
	{
		public Mobile Mobile { get; }

		public string Speech { get; set; }

		public MessageType Type { get; }

		public int Hue { get; }

		public int[] Keywords { get; }

		public bool Handled { get; set; }

		public bool Blocked { get; set; }

		public bool HasKeyword( int keyword )
		{
			for ( int i = 0; i < Keywords.Length; ++i )
				if ( Keywords[i] == keyword )
					return true;

			return false;
		}

		public SpeechEventArgs( Mobile mobile, string speech, MessageType type, int hue, int[] keywords )
		{
			Mobile = mobile;
			Speech = speech;
			Type = type;
			Hue = hue;
			Keywords = keywords;
		}
	}

	public class LoginEventArgs : EventArgs
	{
		public Mobile Mobile { get; }

		public LoginEventArgs( Mobile mobile )
		{
			Mobile = mobile;
		}
	}

	public class WorldSaveEventArgs : EventArgs
	{
		public bool Message { get; }

		public WorldSaveEventArgs( bool msg )
		{
			Message = msg;
		}
	}

	public class FastWalkEventArgs
	{
		public FastWalkEventArgs( NetState state )
		{
			NetState = state;
			Blocked = false;
		}

		public NetState NetState { get; }

		public bool Blocked { get; set; }
	}

	public class EventSink
	{
		public static event DeletedEventHandler Deleted;
		public static event BeforeDamageEventHandler BeforeDamage;
		public static event CreateCharRequestEventHandler CreateCharRequest;
		public static event CharacterCreatedEventHandler CharacterCreated;
		public static event OpenDoorMacroEventHandler OpenDoorMacroUsed;
		public static event SpeechEventHandler Speech;
		public static event LoginEventHandler Login;
		public static event ServerListEventHandler ServerList;
		public static event MovementEventHandler Movement;
		public static event HungerChangedEventHandler HungerChanged;
		public static event MapChangedEventHandler MapChanged;
		public static event CrashedEventHandler Crashed;
		public static event ShutdownEventHandler Shutdown;
		public static event HelpRequestEventHandler HelpRequest;
		public static event DisarmRequestEventHandler DisarmRequest;
		public static event StunRequestEventHandler StunRequest;
		public static event OpenSpellbookRequestEventHandler OpenSpellbookRequest;
		public static event CastSpellRequestEventHandler CastSpellRequest;
		public static event AnimateRequestEventHandler AnimateRequest;
		public static event LogoutEventHandler Logout;
		public static event SocketConnectEventHandler SocketConnect;
		public static event ConnectedEventHandler Connected;
		public static event DisconnectedEventHandler Disconnected;
		public static event RenameRequestEventHandler RenameRequest;
		public static event PlayerDeathEventHandler PlayerDeath;
		public static event VirtueGumpRequestEventHandler VirtueGumpRequest;
		public static event VirtueItemRequestEventHandler VirtueItemRequest;
		public static event VirtueMacroEventHandler VirtueMacroUsed;
		public static event ChatRequestEventHandler ChatRequest;
		public static event AccountLoginEventHandler AccountLogin;
		public static event PaperdollRequestEventHandler PaperdollRequest;
		public static event ProfileRequestEventHandler ProfileRequest;
		public static event ChangeProfileRequestEventHandler ChangeProfileRequest;
		public static event AggressiveActionEventHandler AggressiveAction;
		public static event HarmfulActionEventHandler HarmfulAction;
		public static event CommandEventHandler Command;
		public static event GameLoginEventHandler GameLogin;
		public static event DeleteRequestEventHandler DeleteRequest;
		public static event WorldLoadEventHandler WorldLoad;
		public static event WorldBeforeSaveEventHandler WorldBeforeSave;
		public static event WorldSaveEventHandler WorldSave;
		public static event SetAbilityEventHandler SetAbility;
		public static event FastWalkEventHandler FastWalk;
		public static event CreateGuildHandler CreateGuild;
		public static event ServerStartedEventHandler ServerStarted;
		public static event GuildGumpRequestHandler GuildGumpRequest;
		public static event QuestGumpRequestHandler QuestGumpRequest;
		public static event EquipLastWeaponMacroEventHandler EquipLastWeaponMacroUsed;
		public static event EquipMacroEventHandler EquipMacro;
		public static event UnequipMacroEventHandler UnequipMacro;
		public static event TargetByResourceMacroEventHandler TargetByResourceMacro;
		public static event RacialAbilityRequestEventHandler RacialAbilityRequest;
		public static event BoatMovementRequestEventHandler BoatMovementRequest;
		public static event ClientVersionReceivedHandler ClientVersionReceived;
		public static event OPLRequestHandler OPLRequest;
		public static event PoisonCuredEventHandler PoisonCured;

		public static void InvokePoisonCured( PoisonCuredEventArgs e )
		{
			if ( PoisonCured != null )
				PoisonCured( e );
		}

		public static void InvokeOPLRequest( OPLRequestArgs e )
		{
			if ( OPLRequest != null )
				OPLRequest( e );
		}

		public static void InvokeClientVersionReceived( ClientVersionReceivedArgs e )
		{
			if ( ClientVersionReceived != null )
				ClientVersionReceived( e );
		}

		public static void InvokeTargetByResourceMacro( TargetByResourceMacroEventArgs e )
		{
			if ( TargetByResourceMacro != null )
				TargetByResourceMacro( e );
		}

		public static void InvokeEquipMacro( EquipMacroEventArgs e )
		{
			if ( EquipMacro != null )
				EquipMacro( e );
		}

		public static void InvokeUnequipMacro( UnequipMacroEventArgs e )
		{
			if ( UnequipMacro != null )
				UnequipMacro( e );
		}

		public static void InvokeBoatMovementRequest( BoatMovementRequestEventArgs e )
		{
			if ( BoatMovementRequest != null )
				BoatMovementRequest( e );
		}

		public static void InvokeRacialAbilityRequest( RacialAbilityRequestEventArgs e )
		{
			if ( RacialAbilityRequest != null )
				RacialAbilityRequest( e );
		}

		public static void InvokeServerStarted()
		{
			if ( ServerStarted != null )
				ServerStarted();
		}

		public BaseGuild InvokeCreateGuild( CreateGuildEventArgs e )
		{
			if ( CreateGuild != null )
				return CreateGuild( e );
			else
				return null;
		}

		public static void InvokeSetAbility( SetAbilityEventArgs e )
		{
			if ( SetAbility != null )
				SetAbility( e );
		}

		public static void InvokeEquipLastWeaponMacroUsed( EquipLastWeaponMacroEventArgs e )
		{
			if ( EquipLastWeaponMacroUsed != null )
				EquipLastWeaponMacroUsed( e );
		}

		public static void InvokeGuildGumpRequest( GuildGumpRequestArgs e )
		{
			if ( GuildGumpRequest != null )
				GuildGumpRequest( e );
		}

		public static void InvokeQuestGumpRequest( QuestGumpRequestArgs e )
		{
			if ( QuestGumpRequest != null )
				QuestGumpRequest( e );
		}

		public static void InvokeFastWalk( FastWalkEventArgs e )
		{
			if ( FastWalk != null )
				FastWalk( e );
		}

		public static void InvokeDeleteRequest( DeleteRequestEventArgs e )
		{
			if ( DeleteRequest != null )
				DeleteRequest( e );
		}

		public static void InvokeGameLogin( GameLoginEventArgs e )
		{
			if ( GameLogin != null )
				GameLogin( e );
		}

		public static void InvokeCommand( CommandEventArgs e )
		{
			if ( Command != null )
				Command( e );
		}

		public static void InvokeAggressiveAction( AggressiveActionEventArgs e )
		{
			if ( AggressiveAction != null )
				AggressiveAction( e );
		}

		public static void InvokeHarmfulAction( HarmfulActionEventArgs e )
		{
			if ( HarmfulAction != null )
				HarmfulAction( e );
		}

		public static void InvokeProfileRequest( ProfileRequestEventArgs e )
		{
			if ( ProfileRequest != null )
				ProfileRequest( e );
		}

		public static void InvokeChangeProfileRequest( ChangeProfileRequestEventArgs e )
		{
			if ( ChangeProfileRequest != null )
				ChangeProfileRequest( e );
		}

		public static void InvokePaperdollRequest( PaperdollRequestEventArgs e )
		{
			if ( PaperdollRequest != null )
				PaperdollRequest( e );
		}

		public static void InvokeAccountLogin( AccountLoginEventArgs e )
		{
			if ( AccountLogin != null )
				AccountLogin( e );
		}

		public static void InvokeChatRequest( ChatRequestEventArgs e )
		{
			if ( ChatRequest != null )
				ChatRequest( e );
		}

		public static void InvokeVirtueItemRequest( VirtueItemRequestEventArgs e )
		{
			if ( VirtueItemRequest != null )
				VirtueItemRequest( e );
		}

		public static void InvokeVirtueGumpRequest( VirtueGumpRequestEventArgs e )
		{
			if ( VirtueGumpRequest != null )
				VirtueGumpRequest( e );
		}

		public static void InvokeVirtueMacroUsed( VirtueMacroEventArgs e )
		{
			if ( VirtueMacroUsed != null )
				VirtueMacroUsed( e );
		}

		public static void InvokePlayerDeath( PlayerDeathEventArgs e )
		{
			if ( PlayerDeath != null )
				PlayerDeath( e );
		}

		public static void InvokeRenameRequest( RenameRequestEventArgs e )
		{
			if ( RenameRequest != null )
				RenameRequest( e );
		}

		public static void InvokeLogout( LogoutEventArgs e )
		{
			if ( Logout != null )
				Logout( e );
		}

		public static void InvokeSocketConnect( SocketConnectEventArgs e )
		{
			if ( SocketConnect != null )
				SocketConnect( e );
		}

		public static void InvokeConnected( ConnectedEventArgs e )
		{
			if ( Connected != null )
				Connected( e );
		}

		public static void InvokeDisconnected( DisconnectedEventArgs e )
		{
			if ( Disconnected != null )
				Disconnected( e );
		}

		public static void InvokeAnimateRequest( AnimateRequestEventArgs e )
		{
			if ( AnimateRequest != null )
				AnimateRequest( e );
		}

		public static void InvokeCastSpellRequest( CastSpellRequestEventArgs e )
		{
			if ( CastSpellRequest != null )
				CastSpellRequest( e );
		}

		public static void InvokeOpenSpellbookRequest( OpenSpellbookRequestEventArgs e )
		{
			if ( OpenSpellbookRequest != null )
				OpenSpellbookRequest( e );
		}

		public static void InvokeDisarmRequest( DisarmRequestEventArgs e )
		{
			if ( DisarmRequest != null )
				DisarmRequest( e );
		}

		public static void InvokeStunRequest( StunRequestEventArgs e )
		{
			if ( StunRequest != null )
				StunRequest( e );
		}

		public static void InvokeHelpRequest( HelpRequestEventArgs e )
		{
			if ( HelpRequest != null )
				HelpRequest( e );
		}

		public static void InvokeShutdown( ShutdownEventArgs e )
		{
			if ( Shutdown != null )
				Shutdown( e );
		}

		public static void InvokeCrashed( CrashedEventArgs e )
		{
			if ( Crashed != null )
				Crashed( e );
		}

		public static void InvokeHungerChanged( HungerChangedEventArgs e )
		{
			if ( HungerChanged != null )
				HungerChanged( e );
		}

		public static void InvokeMapChanged( MapChangedEventArgs e )
		{
			if ( MapChanged != null )
				MapChanged( e );
		}

		public static void InvokeMovement( MovementEventArgs e )
		{
			if ( Movement != null )
				Movement( e );
		}

		public static void InvokeServerList( ServerListEventArgs e )
		{
			if ( ServerList != null )
				ServerList( e );
		}

		public static void InvokeLogin( LoginEventArgs e )
		{
			if ( Login != null )
				Login( e );
		}

		public static void InvokeSpeech( SpeechEventArgs e )
		{
			if ( Speech != null )
				Speech( e );
		}

		public static void InvokeDeleted( DeletedEventArgs e )
		{
			if ( Deleted != null )
				Deleted( e );
		}

		public static void InvokeBeforeDamage( BeforeDamageEventArgs e )
		{
			if ( BeforeDamage != null )
				BeforeDamage( e );
		}

		public static void InvokeCreateCharRequest( CreateCharRequestEventArgs e )
		{
			if ( CreateCharRequest != null )
				CreateCharRequest( e );
		}

		public static void InvokeCharacterCreated( CharacterCreatedEventArgs e )
		{
			if ( CharacterCreated != null )
				CharacterCreated( e );
		}

		public static void InvokeOpenDoorMacroUsed( OpenDoorMacroEventArgs e )
		{
			if ( OpenDoorMacroUsed != null )
				OpenDoorMacroUsed( e );
		}

		public static void InvokeWorldLoad()
		{
			if ( WorldLoad != null )
				WorldLoad();
		}

		public static void InvokeWorldBeforeSave()
		{
			if ( WorldBeforeSave != null )
				WorldBeforeSave();
		}

		public static void InvokeWorldSave( WorldSaveEventArgs e )
		{
			if ( WorldSave != null )
				WorldSave( e );
		}

		public void Reset()
		{
			Deleted = null;
			BeforeDamage = null;
			CreateCharRequest = null;
			CharacterCreated = null;
			OpenDoorMacroUsed = null;
			Speech = null;
			Login = null;
			ServerList = null;
			Movement = null;
			HungerChanged = null;
			MapChanged = null;
			Crashed = null;
			Shutdown = null;
			HelpRequest = null;
			DisarmRequest = null;
			StunRequest = null;
			OpenSpellbookRequest = null;
			CastSpellRequest = null;
			AnimateRequest = null;
			Logout = null;
			SocketConnect = null;
			Connected = null;
			Disconnected = null;
			RenameRequest = null;
			PlayerDeath = null;
			VirtueGumpRequest = null;
			VirtueItemRequest = null;
			VirtueMacroUsed = null;
			ChatRequest = null;
			AccountLogin = null;
			PaperdollRequest = null;
			ProfileRequest = null;
			ChangeProfileRequest = null;
			AggressiveAction = null;
			HarmfulAction = null;
			Command = null;
			GameLogin = null;
			DeleteRequest = null;
			WorldLoad = null;
			WorldBeforeSave = null;
			WorldSave = null;
			SetAbility = null;
			GuildGumpRequest = null;
			QuestGumpRequest = null;
			EquipLastWeaponMacroUsed = null;
			EquipMacro = null;
			UnequipMacro = null;
			TargetByResourceMacro = null;
			RacialAbilityRequest = null;
			BoatMovementRequest = null;
			ClientVersionReceived = null;
		}
	}
}
