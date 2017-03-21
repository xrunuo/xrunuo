using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Server;
using Server.Engines.Guilds;
using Server.Items;
using Server.Guilds;
using Server.Mobiles;
using Server.Prompts;
using Server.Targeting;
using Server.Accounting;
using Server.Scripts.Commands;
using Server.Engines.BuffIcons;
using Server.Events;

namespace Server.Factions
{
	[CustomEnum( new string[] { "Minax", "Council of Mages", "True Britannians", "Shadowlords" } )]
	public abstract class Faction : IComparable
	{
		public const int MinFactionKillPoints = -12;

		private FactionDefinition m_Definition;
		private FactionState m_State;
		private StrongholdRegion m_StrongholdRegion;

		public StrongholdRegion StrongholdRegion { get { return m_StrongholdRegion; } set { m_StrongholdRegion = value; } }

		public FactionDefinition Definition
		{
			get { return m_Definition; }
			set
			{
				m_Definition = value;
				m_StrongholdRegion = new StrongholdRegion( this );
			}
		}

		public FactionState State { get { return m_State; } set { m_State = value; } }

		public Election Election { get { return m_State.Election; } set { m_State.Election = value; } }

		public Mobile Commander { get { return m_State.Commander; } set { m_State.Commander = value; } }

		public int Tithe { get { return m_State.Tithe; } set { m_State.Tithe = value; } }

		public int Silver { get { return m_State.Silver; } set { m_State.Silver = value; } }

		public PlayerStateCollection Members { get { return m_State.Members; } set { m_State.Members = value; } }

		public static readonly TimeSpan LeavePeriod = TimeSpan.FromDays( 3.0 );

		public bool FactionMessageReady { get { return m_State.FactionMessageReady; } }

		public void Broadcast( string text )
		{
			Broadcast( 0x3B2, text );
		}

		public void Broadcast( int hue, string text )
		{
			PlayerStateCollection members = Members;

			for ( int i = 0; i < members.Count; ++i )
			{
				members[i].Mobile.SendMessage( hue, text );
			}
		}

		public void Broadcast( int number )
		{
			PlayerStateCollection members = Members;

			for ( int i = 0; i < members.Count; ++i )
			{
				members[i].Mobile.SendLocalizedMessage( number );
			}
		}

		public void Broadcast( string format, params object[] args )
		{
			Broadcast( String.Format( format, args ) );
		}

		public void Broadcast( int hue, string format, params object[] args )
		{
			Broadcast( hue, String.Format( format, args ) );
		}

		public void BeginBroadcast( Mobile from )
		{
			from.Prompt = new BroadcastPrompt( this );
		}

		public void EndBroadcast( Mobile from, string text )
		{
			if ( from.AccessLevel == AccessLevel.Player )
				m_State.RegisterBroadcast();

			Broadcast( Definition.HueBroadcast, "{0} [Commander] {1} : {2}", from.Name, Definition.FriendlyName, text );
		}

		private class BroadcastPrompt : Prompt
		{
			// Enter Faction Message
			public override int MessageCliloc { get { return 1010265; } }

			private Faction m_Faction;

			public BroadcastPrompt( Faction faction )
			{
				m_Faction = faction;
			}

			public override void OnResponse( Mobile from, string text )
			{
				m_Faction.EndBroadcast( from, text );
			}
		}

		public void BeginHonorLeadership( Mobile from )
		{
			from.SendLocalizedMessage( 502090 ); // Click on the player whom you wish to honor.
			from.BeginTarget( 12, false, TargetFlags.None, new TargetCallback( HonorLeadership_OnTarget ) );
		}

		public void HonorLeadership_OnTarget( Mobile from, object obj )
		{
			if ( obj is Mobile )
			{
				Mobile recv = (Mobile) obj;

				PlayerState giveState = PlayerState.Find( from );
				PlayerState recvState = PlayerState.Find( recv );

				if ( giveState == null )
					return;

				if ( recvState == null || recvState.Faction != giveState.Faction )
				{
					from.SendLocalizedMessage( 1042497 ); // Only faction mates can be honored this way.
				}
				else if ( giveState.KillPoints < 5 ) // TODO: Verify 5 or 10
				{
					from.SendLocalizedMessage( 1042499 ); // You must have at least five kill points to honor them.
				}
				else
				{
					giveState.KillPoints -= 5;
					recvState.KillPoints += 4;

					// TODO: Confirm no message sent to giver
					recv.SendLocalizedMessage( 1042500 ); // You have been honored with four kill points.
				}
			}
			else
			{
				from.SendLocalizedMessage( 1042496 ); // You may only honor another player.
			}
		}

		public void AddMember( Mobile mob )
		{
			Members.Add( new PlayerState( mob, this, Members ) );

			mob.AddToBackpack( FactionItem.Imbue( new Robe(), this, false, Definition.HuePrimary ) );
			mob.SendLocalizedMessage( 1010374 ); // You have been granted a robe which signifies your faction

			mob.InvalidateProperties();
			mob.Delta( MobileDelta.Noto );

			var followers = World.Instance.Mobiles
				.OfType<BaseCreature>()
				.Where( bc => bc.Controlled && bc.ControlMaster == mob );

			foreach ( Mobile follower in followers )
			{
				follower.InvalidateProperties();
				follower.Delta( MobileDelta.Noto );
			}

			mob.FixedEffect( 0x373A, 10, 30 );
			mob.PlaySound( 0x209 );
		}

		public static bool IsNearType( Mobile mob, Type type, int range )
		{
			bool mobs = type.IsSubclassOf( typeof( Mobile ) );
			bool items = type.IsSubclassOf( typeof( Item ) );

			IEnumerable<object> eable;

			if ( mobs )
				eable = mob.GetMobilesInRange( range );
			else if ( items )
				eable = mob.GetItemsInRange( range );
			else
				return false;

			foreach ( object obj in eable )
			{
				if ( type.IsAssignableFrom( obj.GetType() ) )
				{
					return true;
				}
			}

			return false;
		}

		public void RemovePlayerState( PlayerState pl )
		{
			if ( pl == null || !Members.Contains( pl ) )
				return;

			Members.Remove( pl );

			PlayerMobile pm = (PlayerMobile) pl.Mobile;
			if ( pm == null )
				return;

			Mobile mob = pl.Mobile;
			if ( pm.FactionPlayerState == pl )
			{
				pm.FactionPlayerState = null;

				mob.InvalidateProperties();
				mob.Delta( MobileDelta.Noto );

				if ( Election.IsCandidate( mob ) )
					Election.RemoveCandidate( mob );

				if ( pl.Finance != null )
					pl.Finance.Finance = null;

				if ( pl.Sheriff != null )
					pl.Sheriff.Sheriff = null;

				Election.RemoveVoter( mob );

				if ( Commander == mob )
					Commander = null;

				pm.ValidateEquipment();
			}
		}

		public void RemoveMember( Mobile mob )
		{
			PlayerState pl = PlayerState.Find( mob );

			if ( pl == null || !Members.Contains( pl ) )
				return;

			Members.Remove( pl );

			if ( mob is PlayerMobile )
				( (PlayerMobile) mob ).FactionPlayerState = null;

			mob.InvalidateProperties();
			mob.Delta( MobileDelta.Noto );

			// TODO: refactor this, maybe a Mobile.GetFollowers() ?
			var followers = World.Instance.Mobiles
				.OfType<BaseCreature>()
				.Where( bc => bc.Controlled && bc.ControlMaster == mob );

			foreach ( Mobile follower in followers )
			{
				follower.InvalidateProperties();
				follower.Delta( MobileDelta.Noto );
			}

			if ( Election.IsCandidate( mob ) )
				Election.RemoveCandidate( mob );

			Election.RemoveVoter( mob );

			if ( pl.Finance != null )
				pl.Finance.Finance = null;

			if ( pl.Sheriff != null )
				pl.Sheriff.Sheriff = null;

			if ( Commander == mob )
				Commander = null;

			if ( mob is PlayerMobile )
				( (PlayerMobile) mob ).ValidateEquipment();
		}

		public void JoinGuilded( PlayerMobile mob, Guild guild )
		{
			Account acc = mob.Account as Account;

			if ( acc == null || acc.Trial )
			{
				guild.RemoveMember( mob );
				mob.SendLocalizedMessage( 1111857 ); // You have been kicked out of your guild! Trial account players may not remain in a guild which is allied with a faction.
			}
			else if ( mob.Young )
			{
				guild.RemoveMember( mob );
				mob.SendLocalizedMessage( 1042283 ); // You have been kicked out of your guild!  Young players may not remain in a guild which is allied with a faction.
			}
			else if ( AlreadyHasCharInFaction( mob ) )
			{
				guild.RemoveMember( mob );
				mob.SendLocalizedMessage( 1005281 ); // You have been kicked out of your guild due to factional overlap
			}
			else if ( IsFactionBanned( mob ) )
			{
				guild.RemoveMember( mob );
				mob.SendLocalizedMessage( 1005052 ); // You are currently banned from the faction system
			}
			else
			{
				AddMember( mob );
				mob.SendLocalizedMessage( 1042756, true, " " + m_Definition.FriendlyName ); // You are now joining a faction:
			}
		}

		public void JoinAlone( Mobile mob )
		{
			AddMember( mob );
			mob.SendLocalizedMessage( 1005058 ); // You have joined the faction
		}

		private bool AlreadyHasCharInFaction( Mobile mob )
		{
			Account acct = mob.Account as Account;

			if ( acct != null )
			{
				for ( int i = 0; i < acct.Length; ++i )
				{
					Mobile c = acct[i];

					Faction f = Find( c );

					if ( f != null && f != this )
						return true;
				}
			}

			return false;
		}

		public static bool IsFactionBanned( Mobile mob )
		{
			Account acct = mob.Account as Account;

			if ( acct == null )
			{
				return false;
			}

			return ( acct.GetTag( "FactionBanned" ) != null );
		}

		public void OnJoinAccepted( Mobile mob )
		{
			PlayerMobile pm = mob as PlayerMobile;

			if ( pm == null )
				return; // sanity

			PlayerState pl = PlayerState.Find( pm );
			Account acc = pm.Account as Account;

			if ( acc == null || acc.Trial )
			{
				pm.SendLocalizedMessage( 1111858 ); // You cannot join a faction as a trial account player
			}
			else if ( pm.Young )
			{
				pm.SendLocalizedMessage( 1010104 ); // You cannot join a faction as a young player
			}
			else if ( pl != null && pl.IsLeaving )
			{
				pm.SendLocalizedMessage( 1005051 ); // You cannot use the faction stone until you have finished quitting your current faction
			}
			else if ( AlreadyHasCharInFaction( pm ) )
			{
				pm.SendLocalizedMessage( 1080111 ); // You cannot join this faction because you've already declared your allegiance to another.
			}
			else if ( IsFactionBanned( mob ) )
			{
				pm.SendLocalizedMessage( 1005052 ); // You are currently banned from the faction system
			}
			else if ( pm.Guild != null )
			{
				Guild guild = pm.Guild as Guild;

				if ( guild.Leader != pm )
					pm.SendLocalizedMessage( 1005057 ); // You cannot join a faction because you are in a guild and not the guildmaster
				else if ( guild.Type != GuildType.Regular )
					pm.SendLocalizedMessage( 1042161 ); // You cannot join a faction because your guild is an Order or Chaos type.
				else if ( guild.Allies.Count > 0 )
					pm.SendLocalizedMessage( 1080454 ); // Your guild cannot join a faction while in alliance with non-factioned guilds.
				else if ( !CanHandleInflux( guild.Members.Count ) )
					pm.SendLocalizedMessage( 1018031 ); // In the interest of faction stability, this faction declines to accept new members for now.
				else
				{
					ArrayList members = new ArrayList( guild.Members );

					for ( int i = 0; i < members.Count; ++i )
					{
						PlayerMobile member = members[i] as PlayerMobile;

						if ( member == null )
						{
							continue;
						}

						JoinGuilded( member, guild );
					}
				}
			}
			else if ( !CanHandleInflux( 1 ) )
			{
				pm.SendLocalizedMessage( 1018031 ); // In the interest of faction stability, this faction declines to accept new members for now.
			}
			else
			{
				JoinAlone( mob );
			}
		}

		public bool IsCommander( Mobile mob )
		{
			if ( mob == null )
			{
				return false;
			}

			return ( mob.AccessLevel >= AccessLevel.GameMaster || mob == Commander );
		}

		public Faction()
		{
			m_State = new FactionState( this );
		}

		public override string ToString()
		{
			return m_Definition.FriendlyName;
		}

		public int CompareTo( object obj )
		{
			return m_Definition.Sort - ( (Faction) obj ).m_Definition.Sort;
		}

		public static bool CheckLeaveTimer( Mobile mob )
		{
			PlayerState pl = PlayerState.Find( mob );

			if ( pl == null || !pl.IsLeaving )
			{
				return false;
			}

			if ( ( pl.Leaving + LeavePeriod ) >= DateTime.Now )
			{
				return false;
			}

			mob.SendLocalizedMessage( 1005163 ); // You have now quit your faction

			pl.Faction.RemoveMember( mob );

			return true;
		}

		public void DecayKillPoints()
		{
			// Publish 75: Faction score now decays at a rate of 1% (rounded down) of score per day.
			// Scores of 99 or below will have a scaling chance to decay 1 point per day. The closer
			// the score is to 0 the smaller the chance is of having point decay.

			PlayerStateCollection members = Members;

			ArrayList list = new ArrayList( members );

			for ( int i = 0; i < list.Count; ++i )
			{
				PlayerState pl = (PlayerState) list[i];

				if ( pl.KillPoints < 100 )
				{
					if ( pl.KillPoints > Utility.Random( 100 ) )
						pl.KillPoints--;
				}
				else
				{
					pl.KillPoints -= ( pl.KillPoints / 100 );
				}
			}
		}

		public void UpdateRanks()
		{
			RankDefinition[] ranks = m_Definition.Ranks;

			PlayerStateCollection members = Members;

			ArrayList list = new ArrayList( members );
			list.Sort();

			// Publish 75: Players with 0 Faction score will always be assigned Rank 1
			// and are no longer counted in Faction membership totals when determining
			// the size of ranking brackets.
			for ( int i = 0; i < list.Count; ++i )
			{
				PlayerState pl = (PlayerState) list[i];

				if ( pl.KillPoints <= 0 )
				{
					pl.Rank = ranks[ranks.Length - 1];
					list.RemoveAt( i );
					--i;
				}
			}

			for ( int i = 0; i < list.Count; ++i )
			{
				PlayerState pl = (PlayerState) list[i];

				int percent;

				if ( list.Count == 1 )
					percent = 1000;
				else
					percent = ( i * 1000 ) / ( list.Count - 1 );

				RankDefinition rank = null;

				for ( int j = 0; j < ranks.Length; ++j )
				{
					RankDefinition check = ranks[j];

					if ( percent >= check.Required )
					{
						rank = check;
						break;
					}
				}

				pl.Rank = rank;
			}
		}

		public static void Initialize()
		{
			EventSink.Login += new LoginEventHandler( EventSink_Login );
			EventSink.Logout += new LogoutEventHandler( EventSink_Logout );
			EventSink.ServerStarted += new ServerStartedEventHandler( EventSink_ServerStarted );
			EventSink.WorldSave += new WorldSaveEventHandler( EventSink_Save );

			Timer.DelayCall( TimeSpan.FromSeconds( 30.0 ), TimeSpan.FromSeconds( 30.0 ), new TimerCallback( ProcessTick ) );

			CommandSystem.Register( "FactionElection", AccessLevel.GameMaster, new CommandEventHandler( FactionElection_OnCommand ) );
			CommandSystem.Register( "FactionCommander", AccessLevel.Administrator, new CommandEventHandler( FactionCommander_OnCommand ) );
			CommandSystem.Register( "FactionItemReset", AccessLevel.Administrator, new CommandEventHandler( FactionItemReset_OnCommand ) );
			CommandSystem.Register( "FactionReset", AccessLevel.Administrator, new CommandEventHandler( FactionReset_OnCommand ) );
			CommandSystem.Register( "FactionTownReset", AccessLevel.Administrator, new CommandEventHandler( FactionTownReset_OnCommand ) );
			CommandSystem.Register( "FactionRemoveLoss", AccessLevel.GameMaster, new CommandEventHandler( FactionRemoveLoss_OnCommand ) );
			CommandSystem.Register( "FactionSetKillPoints", AccessLevel.GameMaster, new CommandEventHandler( FactionSetKillPoints_OnCommand ) );
			CommandSystem.Register( "FactionGetKillPoints", AccessLevel.GameMaster, new CommandEventHandler( FactionGetKillPoints_OnCommand ) );
		}

		public static void EventSink_ServerStarted()
		{
			FactionCollection factions = Faction.Factions;
			foreach ( Faction faction in factions )
			{
				faction.DecayKillPoints();
				faction.UpdateRanks();
			}
		}

		public static void EventSink_Save( WorldSaveEventArgs e )
		{
			FactionCollection factions = Faction.Factions;
			foreach ( Faction faction in factions )
			{
				faction.UpdateRanks();
			}
		}

		[Usage( "FactionGetKillPoints" )]
		private static void FactionGetKillPoints_OnCommand( CommandEventArgs e )
		{
			e.Mobile.SendMessage( "Select target." );
			e.Mobile.BeginTarget( -1, false, TargetFlags.None,
				( from, targeted ) =>
				{
					if ( targeted is Mobile )
					{
						PlayerState pl = PlayerState.Find( (Mobile) targeted );

						if ( pl != null )
							from.SendMessage( "{0}", pl.KillPoints );
					}
				} );
		}

		[Usage( "FactionSetKillPoints <killpoints>" )]
		private static void FactionSetKillPoints_OnCommand( CommandEventArgs e )
		{
			if ( e.Arguments.Length != 1 )
			{
				e.Mobile.SendMessage( "Usage: .FactionSetKillPoints <killpoints>" );
			}
			else
			{
				int killPoints = Utility.ToInt32( e.Arguments[0] );

				e.Mobile.SendMessage( "Select target." );
				e.Mobile.BeginTarget( -1, false, TargetFlags.None,
					( from, targeted ) =>
					{
						if ( targeted is Mobile )
						{
							PlayerState pl = PlayerState.Find( (Mobile) targeted );

							if ( pl != null )
								pl.KillPoints = killPoints;
						}
					} );
			}
		}

		[Usage( "FactionRemoveLoss" )]
		[Description( "Elimina el estado de skill loss del personaje de faccion seleccionado." )]
		private static void FactionRemoveLoss_OnCommand( CommandEventArgs e )
		{
			e.Mobile.Target = new FactionRemoveLossTarget();
			e.Mobile.SendMessage( "Selecciona un objetivo." );
		}

		private class FactionRemoveLossTarget : Target
		{
			public FactionRemoveLossTarget()
				: base( 15, false, TargetFlags.None )
			{
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				if ( targeted is PlayerMobile )
				{
					ClearSkillLoss( (Mobile) targeted );
					from.SendMessage( "Done!" );
				}
				else
				{
					from.SendLocalizedMessage( 1046439 ); // That is not a valid target.
				}
			}
		}

		public static void FactionTownReset_OnCommand( CommandEventArgs e )
		{
			MonolithCollection monoliths = BaseMonolith.Monoliths;

			for ( int i = 0; i < monoliths.Count; ++i )
			{
				monoliths[i].Sigil = null;
			}

			TownCollection towns = Town.Towns;

			for ( int i = 0; i < towns.Count; ++i )
			{
				towns[i].Silver = 0;
				towns[i].Sheriff = null;
				towns[i].Finance = null;
				towns[i].Tax = 0;
				towns[i].Owner = null;
			}

			SigilCollection sigils = Sigil.Sigils;

			for ( int i = 0; i < sigils.Count; ++i )
			{
				sigils[i].Corrupted = null;
				sigils[i].Corrupting = null;
				sigils[i].LastStolen = DateTime.MinValue;
				sigils[i].GraceStart = DateTime.MinValue;
				sigils[i].CorruptionStart = DateTime.MinValue;
				sigils[i].PurificationStart = DateTime.MinValue;
				sigils[i].LastMonolith = null;
				sigils[i].ReturnHome();
			}

			FactionCollection factions = Faction.Factions;

			for ( int i = 0; i < factions.Count; ++i )
			{
				Faction f = factions[i];

				ArrayList list = new ArrayList( f.State.FactionItems );

				for ( int j = 0; j < list.Count; ++j )
				{
					FactionItem fi = (FactionItem) list[j];

					if ( fi.Expiration == DateTime.MinValue )
					{
						fi.Item.Delete();
					}
					else
					{
						fi.Detach();
					}
				}
			}
		}

		public static void FactionReset_OnCommand( CommandEventArgs e )
		{
			MonolithCollection monoliths = BaseMonolith.Monoliths;

			for ( int i = 0; i < monoliths.Count; ++i )
			{
				monoliths[i].Sigil = null;
			}

			TownCollection towns = Town.Towns;

			for ( int i = 0; i < towns.Count; ++i )
			{
				towns[i].Silver = 0;
				towns[i].Sheriff = null;
				towns[i].Finance = null;
				towns[i].Tax = 0;
				towns[i].Owner = null;
			}

			SigilCollection sigils = Sigil.Sigils;

			for ( int i = 0; i < sigils.Count; ++i )
			{
				sigils[i].Corrupted = null;
				sigils[i].Corrupting = null;
				sigils[i].LastStolen = DateTime.MinValue;
				sigils[i].GraceStart = DateTime.MinValue;
				sigils[i].CorruptionStart = DateTime.MinValue;
				sigils[i].PurificationStart = DateTime.MinValue;
				sigils[i].LastMonolith = null;
				sigils[i].ReturnHome();
			}

			FactionCollection factions = Faction.Factions;

			for ( int i = 0; i < factions.Count; ++i )
			{
				Faction f = factions[i];

				ArrayList list = new ArrayList( f.Members );

				for ( int j = 0; j < list.Count; ++j )
				{
					f.RemoveMember( ( (PlayerState) list[j] ).Mobile );
				}

				list = new ArrayList( f.State.FactionItems );

				for ( int j = 0; j < list.Count; ++j )
				{
					FactionItem fi = (FactionItem) list[j];

					if ( fi.Expiration == DateTime.MinValue )
						fi.Item.Delete();
					else
						fi.Detach();
				}

				list = new ArrayList( f.Traps );

				for ( int j = 0; j < list.Count; ++j )
					( (BaseFactionTrap) list[j] ).Delete();
			}
		}

		public static void FactionItemReset_OnCommand( CommandEventArgs e )
		{
			ArrayList pots = new ArrayList();

			foreach ( Item item in World.Instance.Items )
			{
				if ( item is IFactionItem && !( item is HoodedShroudOfShadows ) )
					pots.Add( item );
			}

			int[] hues = new int[Factions.Count * 2];

			for ( int i = 0; i < Factions.Count; ++i )
			{
				hues[0 + ( i * 2 )] = Factions[i].Definition.HuePrimary;
				hues[1 + ( i * 2 )] = Factions[i].Definition.HueSecondary;
			}

			int count = 0;

			for ( int i = 0; i < pots.Count; ++i )
			{
				Item item = (Item) pots[i];
				IFactionItem fci = (IFactionItem) item;

				if ( fci.FactionItemState != null || item.LootType != LootType.Blessed )
					continue;

				bool isHued = false;

				for ( int j = 0; j < hues.Length; ++j )
				{
					if ( item.Hue == hues[j] )
					{
						isHued = true;
						break;
					}
				}

				if ( isHued )
				{
					fci.FactionItemState = null;
					++count;
				}
			}

			e.Mobile.SendMessage( "{0} items reset", count );
		}

		public static void FactionCommander_OnCommand( CommandEventArgs e )
		{
			e.Mobile.SendMessage( "Target a player to make them the faction commander." );
			e.Mobile.BeginTarget( -1, false, TargetFlags.None, new TargetCallback( FactionCommander_OnTarget ) );
		}

		public static void FactionCommander_OnTarget( Mobile from, object obj )
		{
			if ( obj is PlayerMobile )
			{
				Mobile targ = (Mobile) obj;
				PlayerState pl = PlayerState.Find( targ );

				if ( pl != null )
				{
					pl.Faction.Commander = targ;
					from.SendMessage( "You have appointed them as the faction commander." );
				}
				else
				{
					from.SendMessage( "They are not in a faction." );
				}
			}
			else
			{
				from.SendMessage( "That is not a player." );
			}
		}

		public static void FactionElection_OnCommand( CommandEventArgs e )
		{
			e.Mobile.SendMessage( "Target a faction stone to open its election properties." );
			e.Mobile.BeginTarget( -1, false, TargetFlags.None, new TargetCallback( FactionElection_OnTarget ) );
		}

		public static void FactionElection_OnTarget( Mobile from, object obj )
		{
			if ( obj is FactionStone )
			{
				Faction faction = ( (FactionStone) obj ).Faction;

				if ( faction != null )
					from.SendGump( new ElectionManagementGump( faction.Election ) );
				//from.SendGump( new Gumps.PropertiesGump( from, faction.Election ) );
				else
					from.SendMessage( "That stone has no faction assigned." );
			}
			else
			{
				from.SendMessage( "That is not a faction stone." );
			}
		}

		public static async void FactionKick_OnCommand( CommandEventArgs e )
		{
			Mobile from = e.Mobile;

			from.SendMessage( "Target a player to remove them from their faction." );
			object obj = await from.PickTarget( -1, false, TargetFlags.None );

			if ( obj is Mobile )
			{
				Mobile mob = (Mobile) obj;
				PlayerState pl = PlayerState.Find( (Mobile) mob );

				if ( pl != null )
				{
					pl.Faction.RemoveMember( mob );

					mob.SendMessage( "You have been kicked from your faction." );
					from.SendMessage( "They have been kicked from their faction." );
				}
				else
				{
					from.SendMessage( "They are not in a faction." );
				}
			}
			else
			{
				from.SendMessage( "That is not a player." );
			}
		}

		public static void ProcessTick()
		{
			SigilCollection sigils = Sigil.Sigils;

			for ( int i = 0; i < sigils.Count; ++i )
			{
				Sigil sigil = sigils[i];

				if ( !sigil.IsBeingCorrupted && sigil.GraceStart != DateTime.MinValue && ( sigil.GraceStart + Sigil.CorruptionGrace ) < DateTime.Now )
				{
					if ( sigil.LastMonolith is StrongholdMonolith )
					{
						sigil.Corrupting = sigil.LastMonolith.Faction;
						sigil.CorruptionStart = DateTime.Now;
					}
					else
					{
						sigil.Corrupting = null;
						sigil.CorruptionStart = DateTime.MinValue;
					}

					sigil.GraceStart = DateTime.MinValue;
				}

				if ( sigil.LastMonolith == null || sigil.LastMonolith.Sigil == null )
				{
					if ( ( sigil.LastStolen + Sigil.ReturnPeriod ) < DateTime.Now )
						sigil.ReturnHome();
				}
				else
				{
					if ( sigil.IsBeingCorrupted && ( sigil.CorruptionStart + Sigil.CorruptionPeriod ) < DateTime.Now )
					{
						sigil.Corrupted = sigil.Corrupting;
						sigil.Corrupting = null;
						sigil.CorruptionStart = DateTime.MinValue;
						sigil.GraceStart = DateTime.MinValue;
					}
					else if ( sigil.IsPurifying && ( sigil.PurificationStart + Sigil.PurificationPeriod ) < DateTime.Now )
					{
						sigil.PurificationStart = DateTime.MinValue;
						sigil.Corrupted = null;
						sigil.Corrupting = null;
						sigil.CorruptionStart = DateTime.MinValue;
						sigil.GraceStart = DateTime.MinValue;
					}
				}
			}
		}

		#region Skill Loss
		public const double SkillLossFactor = 1.0 / 3;
		public static readonly TimeSpan SkillLossPeriod = TimeSpan.FromMinutes( 20.0 );

		private static Hashtable m_SkillLoss = new Hashtable();

		private class SkillLossContext
		{
			public Timer m_Timer;
			public ArrayList m_Mods;
		}

		public static void ApplySkillLoss( Mobile mob )
		{
			SkillLossContext context = (SkillLossContext) m_SkillLoss[mob];

			if ( context != null )
				return;

			context = new SkillLossContext();
			m_SkillLoss[mob] = context;

			ArrayList mods = context.m_Mods = new ArrayList();

			for ( int i = 0; i < mob.Skills.Length; ++i )
			{
				Skill sk = mob.Skills[i];
				double baseValue = sk.Base;

				if ( baseValue > 0 )
				{
					SkillMod mod = new DefaultSkillMod( sk.SkillName, true, -( baseValue * SkillLossFactor ) );

					mods.Add( mod );
					mob.AddSkillMod( mod );
				}
			}

			context.m_Timer = Timer.DelayCall( SkillLossPeriod, new TimerStateCallback( ClearSkillLoss_Callback ), mob );

			// Stat Loss / All of your skills have been reduced by one third.
			mob.AddBuff( new BuffInfo( BuffIcon.FactionStatLoss, 1153800, 1153826, SkillLossPeriod, mob, true ) );
		}

		private static void ClearSkillLoss_Callback( object state )
		{
			ClearSkillLoss( (Mobile) state );
		}

		public static void ClearSkillLoss( Mobile mob )
		{
			SkillLossContext context = (SkillLossContext) m_SkillLoss[mob];

			if ( context == null )
				return;

			m_SkillLoss.Remove( mob );

			ArrayList mods = context.m_Mods;

			for ( int i = 0; i < mods.Count; ++i )
				mob.RemoveSkillMod( (SkillMod) mods[i] );

			context.m_Timer.Stop();

			mob.RemoveBuff( BuffIcon.FactionStatLoss );
		}

		public static bool HasSkillLoss( Mobile mob )
		{
			SkillLossContext context = (SkillLossContext) m_SkillLoss[mob];
			return ( context != null );
		}
		#endregion

		public int AwardSilver( Mobile mob, int silver )
		{
			if ( silver <= 0 || !mob.Alive )
				return 0;

			int tithed = ( silver * Tithe ) / 100;

			Silver += tithed;

			silver = silver - tithed;

			if ( silver > 0 )
				mob.AddToBackpack( new Silver( silver ) );

			return silver;
		}

		public virtual int MaximumTraps { get { return 15; } }

		public FactionTrapCollection Traps { get { return m_State.Traps; } set { m_State.Traps = value; } }

		public static readonly bool StabilitySystem = false;

		public const int StabilityFactor = 300; // 300% greater (3 times) than smallest faction
		public const int StabilityActivation = 200; // Stablity code goes into effect when largest faction has > 200 people

		public static Faction FindSmallestFaction()
		{
			FactionCollection factions = Factions;
			Faction smallest = null;

			for ( int i = 0; i < factions.Count; ++i )
			{
				Faction faction = factions[i];

				if ( smallest == null || faction.Members.Count < smallest.Members.Count )
				{
					smallest = faction;
				}
			}

			return smallest;
		}

		public static bool StabilityActive()
		{
			if ( !StabilitySystem )
				return false;

			FactionCollection factions = Factions;

			for ( int i = 0; i < factions.Count; ++i )
			{
				Faction faction = factions[i];

				if ( faction.Members.Count > StabilityActivation )
				{
					return true;
				}
			}

			return false;
		}

		public bool CanHandleInflux( int influx )
		{
			if ( !StabilityActive() )
			{
				return true;
			}

			Faction smallest = FindSmallestFaction();

			if ( smallest == null )
			{
				return true; // sanity
			}

			if ( StabilityFactor > 0 && ( ( ( this.Members.Count + influx ) * 100 ) / StabilityFactor ) > smallest.Members.Count )
			{
				return false;
			}

			return true;
		}

		public static bool IsInnocentAttackingFactioner( Mobile m, Factions.Faction f )
		{
			if ( f == null )
				return false;

			if ( Find( m ) == null )
			{
				List<AggressorInfo> list = m.Aggressed;

				for ( int i = 0; i < list.Count; ++i )
				{
					AggressorInfo info = list[i];

					if ( Find( info.Defender ) == f )
						return true;
				}
			}

			return false;
		}

		public static void HandleDeath( Mobile victim, Mobile killer )
		{
			if ( victim.Map != Faction.Facet )
				return;

			if ( killer == null )
				killer = victim.FindMostRecentDamager( true );

			PlayerState killerState = PlayerState.Find( killer );

			Container pack = victim.Backpack;

			if ( pack != null )
			{
				Container killerPack = ( killer == null ? null : killer.Backpack );
				Item[] sigils = pack.FindItemsByType( typeof( Sigil ) );

				for ( int i = 0; i < sigils.Length; ++i )
				{
					Sigil sigil = (Sigil) sigils[i];

					if ( killerState != null && killerPack != null )
					{
						if ( Sigil.ExistsOn( killer ) )
						{
							sigil.ReturnHome();
							killer.SendLocalizedMessage( 1010258 ); // The sigil has gone back to its home location because you already have a sigil.
						}
						else if ( !killerPack.TryDropItem( killer, sigil, false ) )
						{
							sigil.ReturnHome();
							killer.SendLocalizedMessage( 1010259 ); // The sigil has gone home because your backpack is full.
						}

						sigil.RestoreThief( killerState.Faction );
					}
					else
					{
						sigil.ReturnHome();
					}
				}
			}

			PlayerState victimState = PlayerState.Find( victim );

			if ( killerState == null )
			{
				if ( victimState != null && killer is BaseCreature && victim is PlayerMobile )
				{
					BaseCreature slayingCreature = killer as BaseCreature;

					if ( slayingCreature.FactionAllegiance != null && slayingCreature.FactionAllegiance != victimState.Faction )
						ApplySkillLoss( victim );
				}

				return;
			}

			if ( victim is BaseCreature )
			{
				BaseCreature bc = (BaseCreature) victim;
				Faction victimFaction = bc.FactionAllegiance;

				if ( bc.Map == Faction.Facet && victimFaction != null && killerState.Faction != victimFaction )
				{
					int silver = killerState.Faction.AwardSilver( killer, bc.FactionSilverWorth );

					if ( silver > 0 )
					{
						killer.SendLocalizedMessage( 1042748, silver.ToString( "N0" ) ); // Thou hast earned ~1_AMOUNT~ silver for vanquishing the vile creature.
					}
				}

				return;
			}

			if ( victimState == null )
				return;

			if ( killerState.Faction != victimState.Faction )
			{
				if ( !victimState.CanGiveSilverTo( killer ) )
				{
					killer.SendLocalizedMessage( 1042231 ); // You have recently defeated this enemy and thus their death brings you no honor.
				}
				else
				{
					int totalAward = ComputeKillPointAward( victim );
					totalAward = Math.Min( totalAward, victimState.KillPoints - MinFactionKillPoints );

					if ( totalAward <= 0 )
					{
						if ( killer == victim || killerState.Faction != victimState.Faction )
							ApplySkillLoss( victim );
						killer.SendLocalizedMessage( 501693 ); // This victim is not worth enough to get kill points from. 
						return;
					}

					int killerAward = (int) Math.Ceiling( 0.4 * totalAward );
					int participantsAward = totalAward - killerAward;
					int awardedassistpoints = 0;

					var participants = GetFactionKillParticipants( victim );
					int participantAward = (int) Math.Ceiling( (double) participantsAward / participants.Count() );

					foreach ( var participant in participants )
					{
						if ( participant != killer )
						{
							if ( awardedassistpoints + participantAward > participantsAward )
								participantAward = participantsAward - awardedassistpoints;

							GiveKillAward( victim, participant, participantAward, 40 );

							awardedassistpoints += participantAward;
						}

						if ( awardedassistpoints >= participantsAward )
						{
							break;
						}
					}

					if ( awardedassistpoints + killerAward < totalAward )
						killerAward += ( participantsAward - awardedassistpoints );

					GiveKillAward( victim, killer, killerAward, 100 * victimState.Rank.Rank );

					victimState.KillPoints -= totalAward;

					victimState.OnGivenSilverTo( killer );
					victimState.Faction.UpdateRanks();
					killerState.Faction.UpdateRanks();
				}
			}
			if ( killer == victim || killerState.Faction != victimState.Faction )
				ApplySkillLoss( victim );

		}

		private static int ComputeKillPointAward( Mobile victim )
		{
			var victimState = PlayerState.Find( victim );

			int award;

			if ( HasSkillLoss( victim ) )
			{
				award = 1;
			}
			else
			{
				int rank = victimState.Rank.Rank;

				if ( rank < 4 )
					award = 4;
				else if ( rank < 7 )
					award = 8;
				else if ( rank < 10 )
					award = 12;
				else
					award = 16;
			}

			return Math.Max( victimState.KillPoints / 10, award );
		}

		private static IEnumerable<Mobile> GetFactionKillParticipants( Mobile victim )
		{
			return victim.DamageEntries
				.OrderByDescending( d => d.DamageGiven )
				.Select( d => d.Damager )
				.Where( m => AreFromDifferentFactions( victim, m ) )
				.Take( 4 ); // killer plus up to 3 assistants
		}

		private static bool AreFromDifferentFactions( Mobile victim, Mobile killer )
		{
			PlayerState victimState = PlayerState.Find( victim );
			PlayerState killerState = PlayerState.Find( killer );

			return victimState != null && killerState != null && victimState.Faction != killerState.Faction;
		}

		private static void GiveKillAward( Mobile victim, Mobile to, int killPoints, int silver )
		{
			PlayerState victimState = PlayerState.Find( victim );

			if ( victimState == null )
				return;

			PlayerState toState = PlayerState.Find( to );

			if ( toState == null )
				return;

			if ( victimState.KillPoints > 0 )
			{
				silver = toState.Faction.AwardSilver( to, silver );

				if ( silver > 0 )
					to.SendLocalizedMessage( 1042736, String.Format( "{0:N0} silver\t{1}", silver, victim.Name ) ); // You have earned ~1_SILVER_AMOUNT~ pieces for vanquishing ~2_PLAYER_NAME~!
			}

			if ( killPoints > 0 )
			{
				toState.KillPoints += killPoints;

				int offset = ( killPoints != 1 ? 0 : 2 ); // for pluralization

				string args = String.Format( "{0}\t{1}\t{2}", killPoints, victim.Name, to.Name );
				to.SendLocalizedMessage( 1042737 + offset, args ); // Thou hast been honored with ~1_KILL_POINTS~ kill point(s) for vanquishing ~2_DEAD_PLAYER~!
				victim.SendLocalizedMessage( 1042738 + offset, args ); // Thou has lost ~1_KILL_POINTS~ kill point(s) to ~3_ATTACKER_NAME~ for being vanquished!
			}
		}

		private static void EventSink_Logout( LogoutEventArgs e )
		{
			Mobile mob = e.Mobile;

			Container pack = mob.Backpack;

			if ( pack == null )
			{
				return;
			}

			Item[] sigils = pack.FindItemsByType( typeof( Sigil ) );

			for ( int i = 0; i < sigils.Length; ++i )
			{
				( (Sigil) sigils[i] ).ReturnHome();
			}
		}

		private static void EventSink_Login( LoginEventArgs e )
		{
			Mobile mob = e.Mobile;

			CheckLeaveTimer( mob );
		}

		public static readonly Map Facet = Map.Felucca;

		public static void WriteReference( GenericWriter writer, Faction fact )
		{
			int idx = Factions.IndexOf( fact );

			writer.WriteEncodedInt( (int) ( idx + 1 ) );
		}

		public static FactionCollection Factions { get { return Reflector.Factions; } }

		public static Faction ReadReference( GenericReader reader )
		{
			int idx = reader.ReadEncodedInt() - 1;

			if ( idx >= 0 && idx < Factions.Count )
			{
				return Factions[idx];
			}

			return null;
		}

		public static Faction Find( Mobile mob, bool inherit = false, bool creatureAllegiances = false )
		{
			PlayerState pl = PlayerState.Find( mob );

			if ( pl != null )
			{
				return pl.Faction;
			}

			if ( inherit && mob is BaseCreature )
			{
				BaseCreature bc = (BaseCreature) mob;

				if ( bc.Controlled )
				{
					return Find( bc.ControlMaster, false );
				}
				else if ( bc.Summoned )
				{
					return Find( bc.SummonMaster, false );
				}
				else if ( creatureAllegiances && mob is BaseFactionVendor )
				{
					return ( (BaseFactionVendor) mob ).Faction;
				}
				else if ( creatureAllegiances && mob is BaseFactionGuard )
				{
					return ( (BaseFactionGuard) mob ).Faction;
				}
				else if ( creatureAllegiances )
				{
					return bc.FactionAllegiance;
				}
			}

			return null;
		}

		public static Faction Parse( string name )
		{
			FactionCollection factions = Factions;

			for ( int i = 0; i < factions.Count; ++i )
			{
				Faction faction = factions[i];

				if ( Insensitive.Equals( faction.Definition.FriendlyName, name ) )
				{
					return faction;
				}
			}

			return null;
		}
	}

	public enum FactionKickType
	{
		Kick,
		Ban,
		Unban
	}

	public class FactionKickCommand : BaseCommand
	{
		private FactionKickType m_KickType;

		public FactionKickCommand( FactionKickType kickType )
		{
			m_KickType = kickType;

			AccessLevel = AccessLevel.GameMaster;
			Supports = CommandSupport.AllMobiles;
			ObjectTypes = ObjectTypes.Mobiles;

			switch ( m_KickType )
			{
				case FactionKickType.Kick:
					{
						Commands = new string[] { "FactionKick" };
						Usage = "FactionKick";
						Description = "Kicks the targeted player out of his current faction. This does not prevent them from rejoining.";
						break;
					}
				case FactionKickType.Ban:
					{
						Commands = new string[] { "FactionBan" };
						Usage = "FactionBan [username, optional for ban only]";
						Description = "Bans the account of a targeted player from joining factions. All players on the account are removed from their current faction, if any.";
						break;
					}
				case FactionKickType.Unban:
					{
						Commands = new string[] { "FactionUnban" };
						Usage = "FactionUnban";
						Description = "Unbans the account of a targeted player from joining factions.";
						break;
					}
			}
		}

		public override void Execute( CommandEventArgs e, object obj )
		{
			Mobile mob = (Mobile) obj;

			switch ( m_KickType )
			{
				case FactionKickType.Kick:
					{
						PlayerState pl = PlayerState.Find( mob );

						if ( pl != null )
						{
							pl.Faction.RemoveMember( mob );
							mob.SendMessage( "You have been kicked from your faction." );
							AddResponse( "They have been kicked from their faction." );
						}
						else
						{
							LogFailure( "They are not in a faction." );
						}

						break;
					}
				case FactionKickType.Ban:
					{
						Account acct = null;
						if ( e.Length == 1 )
						{
							string acc_name = e.Arguments[0];
							acct = Accounts.GetAccount( acc_name );
						}
						else
						{
							acct = mob.Account as Account;
						}

						if ( acct != null )
						{
							if ( acct.GetTag( "FactionBanned" ) == null )
							{
								acct.SetTag( "FactionBanned", "true" );
								e.Mobile.SendMessage( "The account {0} has been banned from joined factions.", acct );
								//AddResponse( "The account has been banned from joining factions." );
							}
							else
							{
								e.Mobile.SendMessage( "The account {0} is already banned from joined factions.", acct );
								//AddResponse( "The account is already banned from joining factions." );
							}

							for ( int i = 0; i < acct.Length; ++i )
							{
								mob = acct[i];

								if ( mob != null )
								{
									PlayerState pl = PlayerState.Find( mob );

									if ( pl != null )
									{
										pl.Faction.RemoveMember( mob );
										mob.SendMessage( "You have been kicked from your faction." );
										AddResponse( "They have been kicked from their faction." );
									}
								}
							}
						}
						else
						{
							LogFailure( "They have no assigned account." );
						}

						break;
					}
				case FactionKickType.Unban:
					{
						Account acct = null;
						if ( e.Length == 1 )
						{
							string acc_name = e.Arguments[0];
							acct = Accounts.GetAccount( acc_name );
						}
						else
						{
							acct = mob.Account as Account;
						}

						if ( acct != null )
						{
							if ( acct.GetTag( "FactionBanned" ) == null )
							{
								e.Mobile.SendMessage( "The account {0} has been banned from joining factions.", acct );
								//AddResponse( "The account is not already banned from joining factions." );
							}
							else
							{
								acct.RemoveTag( "FactionBanned" );
								e.Mobile.SendMessage( "The account {0} may now freely join factions.", acct );
								//AddResponse( "The account may now freely join factions." );
							}
						}
						else
						{
							LogFailure( "They have no assigned account." );
						}

						break;
					}
			}
		}
	}
}