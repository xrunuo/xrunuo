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
		private IEntity m_Entity;
		private ObjectPropertyList m_List;

		public IEntity Entity { get { return m_Entity; } }
		public ObjectPropertyList List { get { return m_List; } }

		public OPLRequestArgs( IEntity entity, ObjectPropertyList list )
		{
			m_Entity = entity;
			m_List = list;
		}
	}

	public class ClientVersionReceivedArgs : EventArgs
	{
		private GameClient m_State;
		private ClientVersion m_Version;

		public GameClient State { get { return m_State; } }
		public ClientVersion Version { get { return m_Version; } }

		public ClientVersionReceivedArgs( GameClient state, ClientVersion cv )
		{
			m_State = state;
			m_Version = cv;
		}
	}

	public class BoatMovementRequestEventArgs : EventArgs
	{
		private Mobile m_Mobile;
		private Direction m_Direction;
		private int m_Speed;

		public Mobile Mobile { get { return m_Mobile; } }
		public Direction Direction { get { return m_Direction; } }
		public int Speed { get { return m_Speed; } }

		public BoatMovementRequestEventArgs( Mobile m, Direction direction, int speed )
		{
			m_Mobile = m;
			m_Direction = direction;
			m_Speed = speed;
		}
	}

	public class RacialAbilityRequestEventArgs : EventArgs
	{
		private Mobile m_Mobile;
		private int m_AbilityID;

		public Mobile Mobile { get { return m_Mobile; } }
		public int AbilityID { get { return m_AbilityID; } }

		public RacialAbilityRequestEventArgs( Mobile m, int abilityID )
		{
			m_Mobile = m;
			m_AbilityID = abilityID;
		}
	}

	public class TargetByResourceMacroEventArgs : EventArgs
	{
		private GameClient m_State;
		private Item m_Tool;
		private int m_ResourceType;

		public GameClient NetState { get { return m_State; } }
		public Item Tool { get { return m_Tool; } }
		public int ResourceType { get { return m_ResourceType; } }

		public TargetByResourceMacroEventArgs( GameClient state, Item tool, int type )
		{
			m_State = state;
			m_Tool = tool;
			m_ResourceType = type;
		}
	}

	public class EquipMacroEventArgs : EventArgs
	{
		private GameClient m_State;
		private List<int> m_List;

		public GameClient NetState { get { return m_State; } }
		public List<int> List { get { return m_List; } }

		public EquipMacroEventArgs( GameClient state, List<int> list )
		{
			m_State = state;
			m_List = list;
		}
	}

	public class UnequipMacroEventArgs : EventArgs
	{
		private GameClient m_State;
		private List<int> m_List;

		public GameClient NetState { get { return m_State; } }
		public List<int> List { get { return m_List; } }

		public UnequipMacroEventArgs( GameClient state, List<int> list )
		{
			m_State = state;
			m_List = list;
		}
	}

	public class CreateGuildEventArgs : EventArgs
	{
		private int m_Id;

		public int Id { get { return m_Id; } set { m_Id = value; } }

		private static Queue<CreateGuildEventArgs> m_Pool = new Queue<CreateGuildEventArgs>();

		public static CreateGuildEventArgs Create( int id )
		{
			CreateGuildEventArgs args;

			if ( m_Pool.Count > 0 )
			{
				args = m_Pool.Dequeue();

				args.m_Id = id;
			}
			else
			{
				args = new CreateGuildEventArgs( id );
			}

			return args;
		}

		private CreateGuildEventArgs( int id )
		{
			m_Id = id;
		}

		public void Free()
		{
			m_Pool.Enqueue( this );
		}
	}

	public class EquipLastWeaponMacroEventArgs : EventArgs
	{
		private Mobile m_Mobile;

		public Mobile Mobile { get { return m_Mobile; } }

		public EquipLastWeaponMacroEventArgs( Mobile mobile )
		{
			m_Mobile = mobile;
		}
	}

	public class GuildGumpRequestArgs : EventArgs
	{
		private Mobile m_Mobile;

		public Mobile Mobile { get { return m_Mobile; } }

		public GuildGumpRequestArgs( Mobile mobile )
		{
			m_Mobile = mobile;
		}
	}

	public class QuestGumpRequestArgs : EventArgs
	{
		private Mobile m_Mobile;

		public Mobile Mobile { get { return m_Mobile; } }

		public QuestGumpRequestArgs( Mobile mobile )
		{
			m_Mobile = mobile;
		}
	}

	public class SetAbilityEventArgs : EventArgs
	{
		private Mobile m_Mobile;
		private int m_Index;

		public Mobile Mobile { get { return m_Mobile; } }
		public int Index { get { return m_Index; } }

		public SetAbilityEventArgs( Mobile mobile, int index )
		{
			m_Mobile = mobile;
			m_Index = index;
		}
	}

	public class DeleteRequestEventArgs : EventArgs
	{
		private GameClient m_State;
		private int m_Index;

		public GameClient State { get { return m_State; } }
		public int Index { get { return m_Index; } }

		public DeleteRequestEventArgs( GameClient state, int index )
		{
			m_State = state;
			m_Index = index;
		}
	}

	public class GameLoginEventArgs : EventArgs
	{
		private GameClient m_State;
		private string m_Username;
		private string m_Password;
		private bool m_Accepted;
		private CityInfo[] m_CityInfo;

		public GameClient State { get { return m_State; } }
		public string Username { get { return m_Username; } }
		public string Password { get { return m_Password; } }
		public bool Accepted { get { return m_Accepted; } set { m_Accepted = value; } }
		public CityInfo[] CityInfo { get { return m_CityInfo; } set { m_CityInfo = value; } }

		public GameLoginEventArgs( GameClient state, string un, string pw )
		{
			m_State = state;
			m_Username = un;
			m_Password = pw;
		}
	}

	public class AggressiveActionEventArgs : EventArgs
	{
		private Mobile m_Aggressed;
		private Mobile m_Aggressor;
		private bool m_Criminal;

		public Mobile Aggressed { get { return m_Aggressed; } }
		public Mobile Aggressor { get { return m_Aggressor; } }
		public bool Criminal { get { return m_Criminal; } }

		private static Queue<AggressiveActionEventArgs> m_Pool = new Queue<AggressiveActionEventArgs>();

		public static AggressiveActionEventArgs Create( Mobile aggressed, Mobile aggressor, bool criminal )
		{
			AggressiveActionEventArgs args;

			if ( m_Pool.Count > 0 )
			{
				args = m_Pool.Dequeue();

				args.m_Aggressed = aggressed;
				args.m_Aggressor = aggressor;
				args.m_Criminal = criminal;
			}
			else
			{
				args = new AggressiveActionEventArgs( aggressed, aggressor, criminal );
			}

			return args;
		}

		private AggressiveActionEventArgs( Mobile aggressed, Mobile aggressor, bool criminal )
		{
			m_Aggressed = aggressed;
			m_Aggressor = aggressor;
			m_Criminal = criminal;
		}

		public void Free()
		{
			m_Pool.Enqueue( this );
		}
	}

	public class HarmfulActionEventArgs : EventArgs
	{
		private Mobile m_Source;
		private Mobile m_Target;
		private bool m_Criminal;

		public Mobile Source { get { return m_Source; } }
		public Mobile Target { get { return m_Target; } }
		public bool Criminal { get { return m_Criminal; } }

		private static Queue<HarmfulActionEventArgs> m_Pool = new Queue<HarmfulActionEventArgs>();

		public static HarmfulActionEventArgs Create( Mobile source, Mobile target, bool criminal )
		{
			HarmfulActionEventArgs args;

			if ( m_Pool.Count > 0 )
			{
				args = m_Pool.Dequeue();

				args.m_Source = source;
				args.m_Target = target;
				args.m_Criminal = criminal;
			}
			else
			{
				args = new HarmfulActionEventArgs( source, target, criminal );
			}

			return args;
		}

		private HarmfulActionEventArgs( Mobile source, Mobile target, bool criminal )
		{
			m_Source = source;
			m_Target = target;
			m_Criminal = criminal;
		}

		public void Free()
		{
			m_Pool.Enqueue( this );
		}
	}

	public class ProfileRequestEventArgs : EventArgs
	{
		private Mobile m_Beholder;
		private Mobile m_Beheld;

		public Mobile Beholder { get { return m_Beholder; } }
		public Mobile Beheld { get { return m_Beheld; } }

		public ProfileRequestEventArgs( Mobile beholder, Mobile beheld )
		{
			m_Beholder = beholder;
			m_Beheld = beheld;
		}
	}

	public class ChangeProfileRequestEventArgs : EventArgs
	{
		private Mobile m_Beholder;
		private Mobile m_Beheld;
		private string m_Text;

		public Mobile Beholder { get { return m_Beholder; } }
		public Mobile Beheld { get { return m_Beheld; } }
		public string Text { get { return m_Text; } }

		public ChangeProfileRequestEventArgs( Mobile beholder, Mobile beheld, string text )
		{
			m_Beholder = beholder;
			m_Beheld = beheld;
			m_Text = text;
		}
	}

	public class PaperdollRequestEventArgs : EventArgs
	{
		private Mobile m_Beholder;
		private Mobile m_Beheld;

		public Mobile Beholder { get { return m_Beholder; } }
		public Mobile Beheld { get { return m_Beheld; } }

		public PaperdollRequestEventArgs( Mobile beholder, Mobile beheld )
		{
			m_Beholder = beholder;
			m_Beheld = beheld;
		}
	}

	public class AccountLoginEventArgs : EventArgs
	{
		private GameClient m_State;
		private string m_Username;
		private string m_Password;

		private bool m_Accepted;
		private ALRReason m_RejectReason;

		public GameClient State { get { return m_State; } }
		public string Username { get { return m_Username; } }
		public string Password { get { return m_Password; } }
		public bool Accepted { get { return m_Accepted; } set { m_Accepted = value; } }
		public ALRReason RejectReason { get { return m_RejectReason; } set { m_RejectReason = value; } }

		public AccountLoginEventArgs( GameClient state, string un, string pw )
		{
			m_State = state;
			m_Username = un;
			m_Password = pw;
		}
	}

	public class VirtueItemRequestEventArgs : EventArgs
	{
		private Mobile m_Beholder;
		private Mobile m_Beheld;
		private int m_GumpID;

		public Mobile Beholder { get { return m_Beholder; } }
		public Mobile Beheld { get { return m_Beheld; } }
		public int GumpID { get { return m_GumpID; } }

		public VirtueItemRequestEventArgs( Mobile beholder, Mobile beheld, int gumpID )
		{
			m_Beholder = beholder;
			m_Beheld = beheld;
			m_GumpID = gumpID;
		}
	}

	public class VirtueGumpRequestEventArgs : EventArgs
	{
		private Mobile m_Beholder, m_Beheld;

		public Mobile Beholder { get { return m_Beholder; } }
		public Mobile Beheld { get { return m_Beheld; } }

		public VirtueGumpRequestEventArgs( Mobile beholder, Mobile beheld )
		{
			m_Beholder = beholder;
			m_Beheld = beheld;
		}
	}

	public class VirtueMacroEventArgs : EventArgs
	{
		private Mobile m_Mobile;
		private int m_VirtueID;

		public Mobile Mobile { get { return m_Mobile; } }
		public int VirtueID { get { return m_VirtueID; } }

		public VirtueMacroEventArgs( Mobile mobile, int virtueID )
		{
			m_Mobile = mobile;
			m_VirtueID = virtueID;
		}
	}

	public class ChatRequestEventArgs : EventArgs
	{
		private Mobile m_Mobile;

		public Mobile Mobile { get { return m_Mobile; } }

		public ChatRequestEventArgs( Mobile mobile )
		{
			m_Mobile = mobile;
		}
	}

	public class PlayerDeathEventArgs : EventArgs
	{
		private Mobile m_Mobile;

		public Mobile Mobile { get { return m_Mobile; } }

		public PlayerDeathEventArgs( Mobile mobile )
		{
			m_Mobile = mobile;
		}
	}

	public class RenameRequestEventArgs : EventArgs
	{
		private Mobile m_From, m_Target;
		private string m_Name;

		public Mobile From { get { return m_From; } }
		public Mobile Target { get { return m_Target; } }
		public string Name { get { return m_Name; } }

		public RenameRequestEventArgs( Mobile from, Mobile target, string name )
		{
			m_From = from;
			m_Target = target;
			m_Name = name;
		}
	}

	public class LogoutEventArgs : EventArgs
	{
		private Mobile m_Mobile;

		public Mobile Mobile { get { return m_Mobile; } }

		public LogoutEventArgs( Mobile m )
		{
			m_Mobile = m;
		}
	}

	public class SocketConnectEventArgs : EventArgs
	{
		private Socket m_Socket;
		private bool m_AllowConnection;

		public Socket Socket { get { return m_Socket; } }
		public bool AllowConnection { get { return m_AllowConnection; } set { m_AllowConnection = value; } }

		public SocketConnectEventArgs( Socket s )
		{
			m_Socket = s;
			m_AllowConnection = true;
		}
	}

	public class ConnectedEventArgs : EventArgs
	{
		private Mobile m_Mobile;

		public Mobile Mobile { get { return m_Mobile; } }

		public ConnectedEventArgs( Mobile m )
		{
			m_Mobile = m;
		}
	}

	public class DisconnectedEventArgs : EventArgs
	{
		private Mobile m_Mobile;

		public Mobile Mobile { get { return m_Mobile; } }

		public DisconnectedEventArgs( Mobile m )
		{
			m_Mobile = m;
		}
	}

	public class AnimateRequestEventArgs : EventArgs
	{
		private Mobile m_Mobile;
		private string m_Action;

		public Mobile Mobile { get { return m_Mobile; } }
		public string Action { get { return m_Action; } }

		public AnimateRequestEventArgs( Mobile m, string action )
		{
			m_Mobile = m;
			m_Action = action;
		}
	}

	public class CastSpellRequestEventArgs : EventArgs
	{
		private Mobile m_Mobile;
		private Item m_Spellbook;
		private int m_SpellID;
		private IEntity m_Target;

		public Mobile Mobile { get { return m_Mobile; } }
		public Item Spellbook { get { return m_Spellbook; } }
		public int SpellID { get { return m_SpellID; } }
		public IEntity Target { get { return m_Target; } }

		public CastSpellRequestEventArgs( Mobile m, int spellID, Item book )
			: this( m, spellID, book, null )
		{
		}

		public CastSpellRequestEventArgs( Mobile m, int spellID, Item book, IEntity target )
		{
			m_Mobile = m;
			m_Spellbook = book;
			m_SpellID = spellID;
			m_Target = target;
		}
	}

	public class OpenSpellbookRequestEventArgs : EventArgs
	{
		private Mobile m_Mobile;
		private int m_Type;

		public Mobile Mobile { get { return m_Mobile; } }
		public int Type { get { return m_Type; } }

		public OpenSpellbookRequestEventArgs( Mobile m, int type )
		{
			m_Mobile = m;
			m_Type = type;
		}
	}

	public class StunRequestEventArgs : EventArgs
	{
		private Mobile m_Mobile;

		public Mobile Mobile { get { return m_Mobile; } }

		public StunRequestEventArgs( Mobile m )
		{
			m_Mobile = m;
		}
	}

	public class DisarmRequestEventArgs : EventArgs
	{
		private Mobile m_Mobile;

		public Mobile Mobile { get { return m_Mobile; } }

		public DisarmRequestEventArgs( Mobile m )
		{
			m_Mobile = m;
		}
	}

	public class HelpRequestEventArgs : EventArgs
	{
		private Mobile m_Mobile;

		public Mobile Mobile { get { return m_Mobile; } }

		public HelpRequestEventArgs( Mobile m )
		{
			m_Mobile = m;
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
		private Exception m_Exception;
		private bool m_Close;

		public Exception Exception { get { return m_Exception; } }
		public bool Close { get { return m_Close; } set { m_Close = value; } }

		public CrashedEventArgs( Exception e )
		{
			m_Exception = e;
		}
	}

	public class HungerChangedEventArgs : EventArgs
	{
		private Mobile m_Mobile;
		private int m_OldValue;

		public Mobile Mobile { get { return m_Mobile; } }
		public int OldValue { get { return m_OldValue; } }

		public HungerChangedEventArgs( Mobile mobile, int oldValue )
		{
			m_Mobile = mobile;
			m_OldValue = oldValue;
		}
	}

	public class MapChangedEventArgs : EventArgs
	{
		private IEntity m_Entity;
		private Map m_OldMap;

		public IEntity Entity { get { return m_Entity; } }
		public Map OldMap { get { return m_OldMap; } }

		public MapChangedEventArgs( IEntity entity, Map oldMap )
		{
			m_Entity = entity;
			m_OldMap = oldMap;
		}
	}

	public class MovementEventArgs : EventArgs
	{
		private Mobile m_Mobile;
		private Direction m_Direction;
		private bool m_Blocked;

		public Mobile Mobile { get { return m_Mobile; } }
		public Direction Direction { get { return m_Direction; } }
		public bool Blocked { get { return m_Blocked; } set { m_Blocked = value; } }

		private static Queue<MovementEventArgs> m_Pool = new Queue<MovementEventArgs>();

		public static MovementEventArgs Create( Mobile mobile, Direction dir )
		{
			MovementEventArgs args;

			if ( m_Pool.Count > 0 )
			{
				args = m_Pool.Dequeue();

				args.m_Mobile = mobile;
				args.m_Direction = dir;
				args.m_Blocked = false;
			}
			else
			{
				args = new MovementEventArgs( mobile, dir );
			}

			return args;
		}

		public MovementEventArgs( Mobile mobile, Direction dir )
		{
			m_Mobile = mobile;
			m_Direction = dir;
		}

		public void Free()
		{
			m_Pool.Enqueue( this );
		}
	}

	public class ServerListEventArgs : EventArgs
	{
		private GameClient m_State;
		private IAccount m_Account;
		private bool m_Rejected;
		private List<ServerInfo> m_Servers;

		public GameClient State { get { return m_State; } }
		public IAccount Account { get { return m_Account; } }
		public bool Rejected { get { return m_Rejected; } set { m_Rejected = value; } }
		public List<ServerInfo> Servers { get { return m_Servers; } }

		public void AddServer( string name, IPEndPoint address )
		{
			AddServer( name, 0, TimeZone.CurrentTimeZone, address );
		}

		public void AddServer( string name, int fullPercent, TimeZone tz, IPEndPoint address )
		{
			m_Servers.Add( new ServerInfo( name, fullPercent, tz, address ) );
		}

		public ServerListEventArgs( GameClient state, IAccount account )
		{
			m_State = state;
			m_Account = account;
			m_Servers = new List<ServerInfo>();
		}
	}

	public struct SkillNameValue
	{
		private SkillName m_Name;
		private int m_Value;

		public SkillName Name { get { return m_Name; } }
		public int Value { get { return m_Value; } }

		public SkillNameValue( SkillName name, int value )
		{
			m_Name = name;
			m_Value = value;
		}
	}

	public class DeletedEventArgs : EventArgs
	{
		private Mobile m_Mobile;
		public Mobile Mobile { get { return m_Mobile; } }

		public DeletedEventArgs( Mobile mobile )
		{
			m_Mobile = mobile;
		}
	}

	public class BeforeDamageEventArgs : EventArgs
	{
		private Mobile m_Mobile;
		private Mobile m_From;
		private int m_Amount;

		public Mobile Mobile { get { return m_Mobile; } }
		public Mobile From { get { return m_From; } }
		public int Amount { get { return m_Amount; } set { m_Amount = value; } }

		public BeforeDamageEventArgs( Mobile mobile, Mobile from, int amount )
		{
			m_Mobile = mobile;
			m_From = from;
			m_Amount = amount;
		}
	}

	public class CreateCharRequestEventArgs : EventArgs
	{
		private GameClient m_State;
		private IAccount m_Account;
		private CityInfo m_City;
		private SkillNameValue[] m_Skills;
		private int m_ShirtHue, m_PantsHue;
		private int m_HairID, m_HairHue;
		private int m_BeardID, m_BeardHue;
		private string m_Name;
		private bool m_Female;
		private int m_Hue;
		private int m_Str, m_Dex, m_Int;
		private int m_Profession;
		private Mobile m_Mobile;
		private Race m_Race;

		public GameClient State { get { return m_State; } }
		public IAccount Account { get { return m_Account; } }
		public Mobile Mobile { get { return m_Mobile; } set { m_Mobile = value; } }
		public string Name { get { return m_Name; } }
		public bool Female { get { return m_Female; } }
		public int Hue { get { return m_Hue; } }
		public int Str { get { return m_Str; } }
		public int Dex { get { return m_Dex; } }
		public int Int { get { return m_Int; } }
		public CityInfo City { get { return m_City; } }
		public SkillNameValue[] Skills { get { return m_Skills; } }
		public int ShirtHue { get { return m_ShirtHue; } }
		public int PantsHue { get { return m_PantsHue; } }
		public int HairID { get { return m_HairID; } }
		public int HairHue { get { return m_HairHue; } }
		public int BeardID { get { return m_BeardID; } }
		public int BeardHue { get { return m_BeardHue; } }
		public int Profession { get { return m_Profession; } set { m_Profession = value; } }
		public Race Race { get { return m_Race; } }

		public CreateCharRequestEventArgs( GameClient state, IAccount a, string name, bool female, int hue, int str, int dex, int intel, CityInfo city, SkillNameValue[] skills, int shirtHue, int pantsHue, int hairID, int hairHue, int beardID, int beardHue, int profession, Race race )
		{
			m_State = state;
			m_Account = a;
			m_Name = name;
			m_Female = female;
			m_Hue = hue;
			m_Str = str;
			m_Dex = dex;
			m_Int = intel;
			m_City = city;
			m_Skills = skills;
			m_ShirtHue = shirtHue;
			m_PantsHue = pantsHue;
			m_HairID = hairID;
			m_HairHue = hairHue;
			m_BeardID = beardID;
			m_BeardHue = beardHue;
			m_Profession = profession;
			m_Race = race;
		}
	}

	public class CharacterCreatedEventArgs : EventArgs
	{
		private Mobile m_Mobile;

		public Mobile Mobile { get { return m_Mobile; } }

		public CharacterCreatedEventArgs( Mobile mobile )
		{
			m_Mobile = mobile;
		}
	}

	public class OpenDoorMacroEventArgs : EventArgs
	{
		private Mobile m_Mobile;

		public Mobile Mobile { get { return m_Mobile; } }

		public OpenDoorMacroEventArgs( Mobile mobile )
		{
			m_Mobile = mobile;
		}
	}

	public class SpeechEventArgs : EventArgs
	{
		private Mobile m_Mobile;
		private string m_Speech;
		private MessageType m_Type;
		private int m_Hue;
		private int[] m_Keywords;
		private bool m_Handled;
		private bool m_Blocked;

		public Mobile Mobile { get { return m_Mobile; } }
		public string Speech { get { return m_Speech; } set { m_Speech = value; } }
		public MessageType Type { get { return m_Type; } }
		public int Hue { get { return m_Hue; } }
		public int[] Keywords { get { return m_Keywords; } }
		public bool Handled { get { return m_Handled; } set { m_Handled = value; } }
		public bool Blocked { get { return m_Blocked; } set { m_Blocked = value; } }

		public bool HasKeyword( int keyword )
		{
			for ( int i = 0; i < m_Keywords.Length; ++i )
				if ( m_Keywords[i] == keyword )
					return true;

			return false;
		}

		public SpeechEventArgs( Mobile mobile, string speech, MessageType type, int hue, int[] keywords )
		{
			m_Mobile = mobile;
			m_Speech = speech;
			m_Type = type;
			m_Hue = hue;
			m_Keywords = keywords;
		}
	}

	public class LoginEventArgs : EventArgs
	{
		private Mobile m_Mobile;

		public Mobile Mobile { get { return m_Mobile; } }

		public LoginEventArgs( Mobile mobile )
		{
			m_Mobile = mobile;
		}
	}

	public class WorldSaveEventArgs : EventArgs
	{
		private bool m_Msg;

		public bool Message { get { return m_Msg; } }

		public WorldSaveEventArgs( bool msg )
		{
			m_Msg = msg;
		}
	}

	public class FastWalkEventArgs
	{
		private GameClient m_State;
		private bool m_Blocked;

		public FastWalkEventArgs( GameClient state )
		{
			m_State = state;
			m_Blocked = false;
		}

		public GameClient NetState { get { return m_State; } }
		public bool Blocked { get { return m_Blocked; } set { m_Blocked = value; } }
	}

	public class EventSink
	{
		private static EventSink m_Instance;

		public static EventSink Instance
		{
			get { return m_Instance; }
			set { m_Instance = value; }
		}

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
