using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Network;
using SpawnInstance = Server.Mobiles.CreatureSpawner.SpawnInstance;

namespace Server.Gumps
{
	public class CreatureSpawnerGump : Gump
	{
		private const int EntriesPerPage = 7;

		private const int FontColor = 0xFFFFFF;

		private const int LabelHue = 0x480;
		private const int GreenHue = 0x40;
		private const int RedHue = 0x20;

		private void AddHtmlColor( int x, int y, int width, int height, string text, int color, bool background, bool scrollbar )
		{
			AddHtml( x, y, width, height, String.Format( "<BASEFONT COLOR=#{0:X6}>{1}</BASEFONT>", color, text ), background, scrollbar );
		}

		private CreatureSpawner m_Spawner;

		public CreatureSpawnerGump( CreatureSpawner spawner )
			: base( 25, 25 )
		{
			m_Spawner = spawner;

			AddPage( 0 );

			// Gump Structure

			AddBackground( 0, 0, 520, 510, 0x13BE );
			AddImageTiled( 10, 10, 500, 30, 0xA40 );
			AddImageTiled( 10, 50, 500, 190, 0xA40 );
			AddImageTiled( 10, 250, 500, 190, 0xA40 );
			AddImageTiled( 10, 450, 500, 45, 0xA40 );
			AddAlphaRegion( 10, 10, 500, 485 );

			// Title

			AddHtmlColor( 10, 14, 500, 20, "<CENTER>CREATURE SPAWNER GUMP</CENTER>", FontColor, false, false );

			// Spawn Info

			AddTextEntry( 116, 60, 145, 20, 1271, 10, m_Spawner.SpawnName );
			AddLabel( 22, 60, 1259, "Spawn Name" );

			AddTextEntry( 116, 85, 74, 20, 1271, 11, m_Spawner.Count.ToString() );
			AddLabel( 22, 85, 1259, "Amount" );

			AddTextEntry( 116, 110, 74, 20, 1271, 12, m_Spawner.SpawnRange.ToString() );
			AddLabel( 22, 110, 1259, "Spawn Range" );

			AddTextEntry( 116, 135, 74, 20, 1271, 13, m_Spawner.HomeRange.ToString() );
			AddLabel( 22, 135, 1259, "Home Range" );

			AddTextEntry( 116, 160, 74, 20, 1271, 14, m_Spawner.MinDelay.ToString() );
			AddLabel( 22, 160, 1259, "Min Delay" );

			AddTextEntry( 116, 185, 74, 20, 1271, 15, m_Spawner.MaxDelay.ToString() );
			AddLabel( 22, 185, 1259, "Max Delay" );

			AddTextEntry( 116, 210, 74, 20, 1271, 16, m_Spawner.Team.ToString() );
			AddLabel( 22, 210, 1259, "Team" );

			AddCheck( 376, 60, 210, 211, m_Spawner.Active, 19 );
			AddLabel( 282, 60, 1259, "Active" );

			AddCheck( 376, 85, 210, 211, m_Spawner.CantWalk, 17 );
			AddLabel( 282, 85, 1259, "Cant Walk" );

			AddCheck( 376, 110, 210, 211, m_Spawner.Group, 18 );
			AddLabel( 282, 110, 1259, "Group" );

			AddCheck( 376, 135, 210, 211, m_Spawner.Blessed, 20 );
			AddLabel( 282, 135, 1259, "Blessed" );

			AddCheck( 376, 160, 210, 211, m_Spawner.Murderer, 21 );
			AddLabel( 282, 160, 1259, "Murderer" );

			AddCheck( 376, 185, 210, 211, m_Spawner.ScaledDelay, 22 );
			AddLabel( 282, 185, 1259, "Scaled Delay" );

			AddCheck( 376, 210, 210, 211, m_Spawner.Saturable, 23 );
			AddLabel( 282, 210, 1259, "Saturable" );

			AddButton( 20, 465, 247, 248, 20, GumpButtonType.Reply, 0 );

			if ( m_Spawner.Group && m_Spawner.TotalSpawned == 0 )
				AddLabel( 220, 465, 0x481, String.Format( "Next Group Spawn: {0}", ( m_Spawner.NextGroupRespawn - DateTime.Now ).ToString() ) );

			// Column header

			int offset = 255;

			AddLabelCropped( 22, offset, 100, 20, LabelHue, "Mobile" );
			AddLabelCropped( 192, offset, 120, 20, LabelHue, "State" );
			AddLabelCropped( 252, offset, 120, 20, LabelHue, "Next Spawn" );
			AddLabelCropped( 402, offset, 120, 20, LabelHue, "Saturation" );

			// Entry info

			offset += 30;

			List<SpawnInstance> instances = m_Spawner.Instances;

			int maxPage = ( instances.Count - 1 ) / EntriesPerPage;

			for ( int i = 0; i < instances.Count; i++ )
			{
				int page = i / EntriesPerPage;
				int entry = i % EntriesPerPage;

				if ( entry == 0 )
				{
					AddPage( page + 1 );

					offset = 285;

					if ( page > 0 )
						AddButton( 465, 252, 0x15E3, 0x15E7, 0, GumpButtonType.Page, page );
					else
						AddImage( 465, 252, 0x25EA );

					if ( page < maxPage )
						AddButton( 482, 252, 0x15E1, 0x15E5, 0, GumpButtonType.Page, page + 2 );
					else
						AddImage( 482, 252, 0x25E6 );
				}

				SpawnInstance si = instances[i];

				AddLabelCropped( 22, offset, 120, 20, LabelHue, GetMobile( si ) );
				AddLabelCropped( 192, offset, 120, 20, LabelHue, GetState( si ) );
				AddLabelCropped( 252, offset, 210, 20, LabelHue, GetNextSpawn( si ) );

				//if ( !si.IsSaturated )
				AddLabelCropped( 402, offset, 120, 20, GreenHue, "0%" );
				//else
				//	AddLabelCropped( 402, offset, 120, 20, RedHue, "100%" );

				offset += 20;
			}
		}

		private String GetMobile( SpawnInstance si )
		{
			if ( si.State == SpawnInstance.SpawnState.Active && si.Spawned != null )
				return si.Spawned.ToString();
			else
				return "--";
		}

		private String GetState( SpawnInstance si )
		{
			switch ( si.State )
			{
				case SpawnInstance.SpawnState.Active: return "Active";
				case SpawnInstance.SpawnState.Inactive: return "Inactive";
				case SpawnInstance.SpawnState.Respawning: return "Respawing";
			}

			return "--";
		}

		private String GetNextSpawn( SpawnInstance si )
		{
			if ( si.State == SpawnInstance.SpawnState.Respawning )
				return ( si.NextSpawn - DateTime.Now ).ToString();
			else
				return "--";
		}

		private void SetSettings( CreatureSpawner spawner, int count, TimeSpan minDelay, TimeSpan maxDelay, int team, int homeRange, int spawnRange, string spawnName, bool cantWalk, bool group, bool active, bool blessed, bool murderer, bool scaledDelay, bool saturable )
		{
			spawner.Count = count;
			spawner.MinDelay = minDelay;
			spawner.MaxDelay = maxDelay;
			spawner.Team = team;
			spawner.HomeRange = homeRange;
			spawner.SpawnRange = spawnRange;
			spawner.SpawnName = spawnName;
			spawner.CantWalk = cantWalk;
			spawner.Group = group;
			spawner.Active = active;
			spawner.Blessed = blessed;
			spawner.Murderer = murderer;
			spawner.ScaledDelay = scaledDelay;
			spawner.Saturable = saturable;
		}

		public override void OnResponse( NetState sender, RelayInfo info )
		{
			if ( info.ButtonID == 20 )
			{
				SetSettings
				(
					m_Spawner,
					Utility.ToInt32( info.GetTextEntry( 11 ).Text ), // Count
					Utility.ToTimeSpan( info.GetTextEntry( 14 ).Text ), // Min Delay,
					Utility.ToTimeSpan( info.GetTextEntry( 15 ).Text ), // Max Delay
					Utility.ToInt32( info.GetTextEntry( 16 ).Text ), // Team
					Utility.ToInt32( info.GetTextEntry( 13 ).Text ), // Home Range
					Utility.ToInt32( info.GetTextEntry( 12 ).Text ), // Spawn Range
					info.GetTextEntry( 10 ).Text, // Creature Name
					info.IsSwitched( 17 ), // Cant Walk
					info.IsSwitched( 18 ), // Group
					info.IsSwitched( 19 ), // Running
					info.IsSwitched( 20 ), // Blessed
					info.IsSwitched( 21 ), // Murderer
					info.IsSwitched( 22 ), // Scaled Delay
					info.IsSwitched( 23 ) // saturated
				);

				if ( m_Spawner.Active )
					m_Spawner.TotalRespawn();
				else
					m_Spawner.Despawn();

				sender.Mobile.SendMessage( 0x64, "* Cambios Guardados Correctamente *" );
			}
		}
	}
}