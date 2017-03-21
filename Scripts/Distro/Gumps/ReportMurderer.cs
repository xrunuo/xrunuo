using System;
using System.Collections;
using Server;
using Server.Misc;
using Server.Gumps;
using Server.Network;
using Server.Mobiles;
using Server.Events;

namespace Server.Gumps
{
	public class ReportMurdererGump : Gump
	{
		public override int TypeID { get { return 0x3A9A; } }

		public static void Initialize()
		{
			EventSink.PlayerDeath += new PlayerDeathEventHandler( EventSink_PlayerDeath );
		}

		private int m_Idx;
		private ArrayList m_Killers;

		public static void EventSink_PlayerDeath( PlayerDeathEventArgs e )
		{
			Mobile m = e.Mobile;

			ArrayList killers = new ArrayList();
			ArrayList toGive = new ArrayList();

			foreach ( AggressorInfo ai in m.Aggressors )
			{
				if ( ai.Attacker.IsPlayer && ai.CanReportMurder && !ai.Reported )
				{
					killers.Add( ai.Attacker );
					ai.Reported = true;
				}

				if ( ai.Attacker.IsPlayer && ( DateTime.Now - ai.LastCombatTime ) < TimeSpan.FromSeconds( 30.0 ) && !toGive.Contains( ai.Attacker ) )
					toGive.Add( ai.Attacker );
			}

			foreach ( AggressorInfo ai in m.Aggressed )
			{
				if ( ai.Defender.IsPlayer && ( DateTime.Now - ai.LastCombatTime ) < TimeSpan.FromSeconds( 30.0 ) && !toGive.Contains( ai.Defender ) )
					toGive.Add( ai.Defender );
			}

			foreach ( Mobile g in toGive )
			{
				int n = Notoriety.Compute( g, m );

				int ourKarma = g.Karma;

				bool innocent = ( n == Notoriety.Innocent );
				bool criminal = ( n == Notoriety.Criminal || n == Notoriety.Murderer );

				int fameAward = m.Fame / 200;
				int karmaAward = 0;

				if ( innocent )
					karmaAward = ( ourKarma > -2500 ? -850 : -110 - ( m.Karma / 100 ) );
				else if ( criminal )
					karmaAward = 50;

				Titles.AwardFame( g, fameAward, false );
				Titles.AwardKarma( g, karmaAward, true );
			}

			if ( m is PlayerMobile && ( (PlayerMobile) m ).NpcGuild == NpcGuild.ThievesGuild )
				return;

			if ( killers.Count > 0 )
				new GumpTimer( m, killers ).Start();
		}

		private class GumpTimer : Timer
		{
			private Mobile m_Victim;
			private ArrayList m_Killers;

			public GumpTimer( Mobile victim, ArrayList killers )
				: base( TimeSpan.FromSeconds( 4.0 ) )
			{
				m_Victim = victim;
				m_Killers = killers;
			}

			protected override void OnTick()
			{
				m_Victim.SendGump( new ReportMurdererGump( m_Victim, m_Killers ) );
			}
		}

		public ReportMurdererGump( Mobile victum, ArrayList killers )
			: this( victum, killers, 0 )
		{
		}

		private ReportMurdererGump( Mobile victum, ArrayList killers, int idx )
			: base( 20, 30 )
		{
			m_Killers = killers;
			m_Idx = idx;
			BuildGump();
		}

		private void BuildGump()
		{
			AddBackground( 0, 0, 400, 350, 0xA28 );

			AddHtml( 20, 50, 200, 35, ( (Mobile) m_Killers[m_Idx] ).Name, false, false );

			AddHtmlLocalized( 20, 85, 300, 35, 1049066, false, false ); // Would you like to report this character as a murderer?

			AddButton( 20, 125, 0xFA5, 0xFA7, 1, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 60, 125, 45, 20, 1046362, false, false ); // Yes

			AddButton( 110, 125, 0xFA5, 0xFA7, 0, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 150, 125, 65, 20, 1046363, false, false ); // No    
		}

		public override void OnResponse( NetState state, RelayInfo info )
		{
			Mobile from = state.Mobile;

			switch ( info.ButtonID )
			{
				case 1:
					{
						Mobile killer = (Mobile) m_Killers[m_Idx];

						if ( killer != null && !killer.Deleted )
						{
							killer.Kills++;
							killer.ShortTermMurders++;

							if ( killer is PlayerMobile )
								( (PlayerMobile) killer ).ResetKillTime();

							killer.SendLocalizedMessage( 1049067 ); // You have been reported for murder!

							if ( killer.Kills == 5 )
								killer.SendLocalizedMessage( 502134 ); // You are now known as a murderer!
							else if ( SkillHandlers.Stealing.SuspendOnMurder && killer.Kills == 1 && killer is PlayerMobile && ( (PlayerMobile) killer ).NpcGuild == NpcGuild.ThievesGuild )
								killer.SendLocalizedMessage( 501562 ); // You have been suspended by the Thieves Guild.
						}
						break;
					}
				case 0:
					{
						break;
					}
			}

			m_Idx++;

			if ( m_Idx < m_Killers.Count )
				from.SendGump( new ReportMurdererGump( from, m_Killers, m_Idx ) );
		}
	}
}