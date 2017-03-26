using System;
using System.Threading;
using System.Collections;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Factions;
using Server.Accounting;

namespace Server.Engines.Reports
{
	public class Reports
	{
		public static readonly bool Enabled = Core.Config.Reports.Enabled;

		public static void Initialize()
		{
			if ( !Enabled )
				return;

			m_StatsHistory = new SnapshotHistory();
			m_StatsHistory.Load();

			m_StaffHistory = new StaffHistory();
			m_StaffHistory.Load();

			DateTime now = DateTime.UtcNow;

			DateTime date = now.Date;
			TimeSpan timeOfDay = now.TimeOfDay;

			m_GenerateTime = date + TimeSpan.FromHours( Math.Ceiling( timeOfDay.TotalHours ) );

			Timer.DelayCall( TimeSpan.FromMinutes( 0.5 ), TimeSpan.FromMinutes( 0.5 ), new TimerCallback( CheckRegenerate ) );
		}

		private static DateTime m_GenerateTime;

		public static void CheckRegenerate()
		{
			if ( DateTime.UtcNow < m_GenerateTime )
				return;

			Generate();
			m_GenerateTime += TimeSpan.FromHours( 1.0 );
		}

		private static SnapshotHistory m_StatsHistory;
		private static StaffHistory m_StaffHistory;

		public static StaffHistory StaffHistory { get { return m_StaffHistory; } }

		public static void Generate()
		{
			Snapshot ss = new Snapshot();

			ss.TimeStamp = DateTime.UtcNow;

			FillSnapshot( ss );

			m_StatsHistory.Snapshots.Add( ss );
			m_StaffHistory.QueueStats.Add( new QueueStatus( Engines.Help.PageQueue.List.Count ) );

			ThreadPool.QueueUserWorkItem( new WaitCallback( UpdateOutput ), ss );
		}

		private static void UpdateOutput( object state )
		{
			m_StatsHistory.Save();
			m_StaffHistory.Save();

			HtmlRenderer renderer = new HtmlRenderer( "stats", (Snapshot) state, m_StatsHistory );
			renderer.Render();
			renderer.Upload();

			renderer = new HtmlRenderer( "staff", m_StaffHistory );
			renderer.Render();
			renderer.Upload();
		}

		public static void FillSnapshot( Snapshot ss )
		{
			ss.Children.Add( CompileGeneralStats() );

			PersistableObject[] obs = CompileFactionReports();

			for ( int i = 0; i < obs.Length; ++i )
				ss.Children.Add( obs[i] );
		}

		public static Report CompileGeneralStats()
		{
			Report report = new Report( "General Stats", "200" );

			report.Columns.Add( "50%", "left" );
			report.Columns.Add( "50%", "left" );

			int npcs = 0, players = 0;

			foreach ( Mobile mob in World.Mobiles )
			{
				if ( mob.Player )
					++players;
				else
					++npcs;
			}

			report.Items.Add( "NPCs", npcs, "N0" );
			report.Items.Add( "Players", players, "N0" );
			report.Items.Add( "Clients", GameServer.Instance.ClientCount, "N0" );
			report.Items.Add( "Accounts", Accounts.Count, "N0" );
			report.Items.Add( "Items", World.ItemCount, "N0" );
			report.Items.Add( "Memory (MB)", GC.GetTotalMemory( false ) / ( 1024 * 1024 ), "N0" );

			return report;
		}

		public static PersistableObject[] CompileFactionReports()
		{
			return new PersistableObject[]
				{
					CompileFactionMembershipChart(),
					CompileFactionReport(),
					CompileFactionTownReport(),
					CompileSigilReport(),
					CompileFactionLeaderboard()
				};
		}

		public static Chart CompileFactionMembershipChart()
		{
			PieChart chart = new PieChart( "Faction Membership", "graphs_faction_membership", true );

			FactionCollection factions = Faction.Factions;

			for ( int i = 0; i < factions.Count; ++i )
				chart.Items.Add( factions[i].Definition.FriendlyName, factions[i].Members.Count );

			return chart;
		}

		public static Report CompileFactionLeaderboard()
		{
			Report report = new Report( "Faction Leaderboard", "60%" );

			report.Columns.Add( "28%", "center", "Name" );
			report.Columns.Add( "28%", "center", "Faction" );
			report.Columns.Add( "28%", "center", "Office" );
			report.Columns.Add( "16%", "center", "Kill Points" );

			ArrayList list = new ArrayList();

			FactionCollection factions = Faction.Factions;

			for ( int i = 0; i < factions.Count; ++i )
			{
				Faction faction = factions[i];

				list.AddRange( faction.Members );
			}

			list.Sort();
			list.Reverse();

			for ( int i = 0; i < list.Count && i < 15; ++i )
			{
				PlayerState pl = (PlayerState) list[i];

				string office;

				if ( pl.Faction.Commander == pl.Mobile )
					office = "Commanding Lord";
				else if ( pl.Finance != null )
					office = String.Format( "{0} Finance Minister", pl.Finance.Definition.FriendlyName );
				else if ( pl.Sheriff != null )
					office = String.Format( "{0} Sheriff", pl.Sheriff.Definition.FriendlyName );
				else
					office = "";

				ReportItem item = new ReportItem();

				item.Values.Add( pl.Mobile.Name );
				item.Values.Add( pl.Faction.Definition.FriendlyName );
				item.Values.Add( office );
				item.Values.Add( pl.KillPoints.ToString(), "N0" );

				report.Items.Add( item );
			}

			return report;
		}

		public static Report CompileFactionReport()
		{
			Report report = new Report( "Faction Statistics", "80%" );

			report.Columns.Add( "20%", "center", "Name" );
			report.Columns.Add( "20%", "center", "Commander" );
			report.Columns.Add( "15%", "center", "Members" );
			report.Columns.Add( "15%", "center", "Merchants" );
			report.Columns.Add( "15%", "center", "Kill Points" );
			report.Columns.Add( "15%", "center", "Silver" );

			FactionCollection factions = Faction.Factions;

			for ( int i = 0; i < factions.Count; ++i )
			{
				Faction faction = factions[i];
				PlayerStateCollection members = faction.Members;

				int totalKillPoints = 0;
				int totalMerchants = 0;

				for ( int j = 0; j < members.Count; ++j )
				{
					totalKillPoints += members[j].KillPoints;

					if ( members[j].MerchantTitle != MerchantTitle.None )
						++totalMerchants;
				}

				ReportItem item = new ReportItem();

				item.Values.Add( faction.Definition.FriendlyName );
				item.Values.Add( faction.Commander == null ? "" : faction.Commander.Name );
				item.Values.Add( faction.Members.Count.ToString(), "N0" );
				item.Values.Add( totalMerchants.ToString(), "N0" );
				item.Values.Add( totalKillPoints.ToString(), "N0" );
				item.Values.Add( faction.Silver.ToString(), "N0" );

				report.Items.Add( item );
			}

			return report;
		}

		public static Report CompileSigilReport()
		{
			Report report = new Report( "Faction Town Sigils", "50%" );

			report.Columns.Add( "35%", "center", "Town" );
			report.Columns.Add( "35%", "center", "Controller" );
			report.Columns.Add( "30%", "center", "Capturable" );

			SigilCollection sigils = Sigil.Sigils;

			for ( int i = 0; i < sigils.Count; ++i )
			{
				Sigil sigil = sigils[i];

				string controller = "Unknown";

				Mobile mob = sigil.RootParent as Mobile;

				if ( mob != null )
				{
					Faction faction = Faction.Find( mob );

					if ( faction != null )
						controller = faction.Definition.FriendlyName;
				}
				else if ( sigil.LastMonolith != null && sigil.LastMonolith.Faction != null )
				{
					controller = sigil.LastMonolith.Faction.Definition.FriendlyName;
				}

				ReportItem item = new ReportItem();

				item.Values.Add( sigil.Town == null ? "" : sigil.Town.Definition.FriendlyName );
				item.Values.Add( controller );
				item.Values.Add( sigil.IsPurifying ? "No" : "Yes" );

				report.Items.Add( item );
			}

			return report;
		}

		public static Report CompileFactionTownReport()
		{
			Report report = new Report( "Faction Towns", "80%" );

			report.Columns.Add( "20%", "center", "Name" );
			report.Columns.Add( "20%", "center", "Owner" );
			report.Columns.Add( "17%", "center", "Sheriff" );
			report.Columns.Add( "17%", "center", "Finance Minister" );
			report.Columns.Add( "13%", "center", "Silver" );
			report.Columns.Add( "13%", "center", "Prices" );

			TownCollection towns = Town.Towns;

			for ( int i = 0; i < towns.Count; ++i )
			{
				Town town = towns[i];

				string prices = "Normal";

				if ( town.Tax < 0 )
					prices = town.Tax.ToString() + "%";
				else if ( town.Tax > 0 )
					prices = "+" + town.Tax.ToString() + "%";

				ReportItem item = new ReportItem();

				item.Values.Add( town.Definition.FriendlyName );
				item.Values.Add( town.Owner == null ? "Neutral" : town.Owner.Definition.FriendlyName );
				item.Values.Add( town.Sheriff == null ? "" : town.Sheriff.Name );
				item.Values.Add( town.Finance == null ? "" : town.Finance.Name );
				item.Values.Add( town.Silver.ToString(), "N0" );
				item.Values.Add( prices );

				report.Items.Add( item );
			}

			return report;
		}
	}
}