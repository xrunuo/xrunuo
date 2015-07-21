/*
 * SunUO
 * $Id: ProfileViewer.cs 518 2006-02-10 22:43:54Z make $
 *
 * (c) 2005-2006 Max Kellermann <max@duempel.org>
 *
 *  This program is free software: you can redistribute it and/or modify modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation; version 2 of the License.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with this program; if not, write to the Free Software
 *  Foundation, Inc., 675 Mass Ave, Cambridge, MA 02139, USA.
 *
 */

using System;
using Server.Gumps;
using Server.Network;

namespace Server.Profiler
{
	public class ProfileViewer : Gump
	{
		public static void Initialize()
		{
			CommandSystem.Register( "Profiler", AccessLevel.Administrator, new CommandEventHandler( OnViewProfile ) );
		}

		[Usage( "Profiler" )]
		[Description( "Browse profiling information" )]
		private static void OnViewProfile( CommandEventArgs e )
		{
			e.Mobile.CloseGump( typeof( ProfileViewer ) );
			e.Mobile.SendGump( new ProfileViewer() );
		}

		private static readonly String[] TimerNames = new String[(int) MainProfile.TimerId.Count]{
			"Idle", "MobileDelta", "ItemDelta", "Timers", "Network",
		};

		private const int LabelHue = 0x480;

		public ProfileViewer()
			: base( 200, 150 )
		{
			AddBackground( 0, 0, 360, 255, 5054 );
			AddBlackAlpha( 10, 10, 340, 235 );

			int y = 10;

			// Header
			AddLabel( 110, y, LabelHue, "Current" );
			AddLabel( 240, y, LabelHue, "Total" );

			y += 25;

			// Timer values
			MainProfile current = Core.CurrentProfile;
			MainProfile total = Core.TotalProfile;
			TimeSpan currentElapsed = Core.Now - current.Start;
			TimeSpan totalElapsed = Core.Now - total.Start;

			for ( int i = 0; i < TimerNames.Length; i++, y += 20 )
			{
				TimeSpan currentValue = current.Timer( (MainProfile.TimerId) i );
				TimeSpan totalValue = total.Timer( (MainProfile.TimerId) i );

				AddLabel( 20, y, LabelHue, TimerNames[i] + ":" );
				AddLabel( 110, y, LabelHue, FormatTimeSpan( currentValue ) );
				if ( currentElapsed.Ticks > 0 )
					AddLabel( 180, y, LabelHue, String.Format( "{0:0.0%}", (double) currentValue.Ticks / (double) currentElapsed.Ticks ) );
				AddLabel( 240, y, LabelHue, FormatTimeSpan( totalValue ) );
				if ( totalElapsed.Ticks > 0 )
					AddLabel( 310, y, LabelHue, String.Format( "{0:0.0%}", (double) totalValue.Ticks / (double) totalElapsed.Ticks ) );
			}

			y += 5;

			// Iterations and iteration rate
			AddLabel( 20, y, LabelHue, "Iterations:" );
			AddLabel( 110, y, LabelHue, current.Iterations.ToString() );
			AddLabel( 240, y, LabelHue, total.Iterations.ToString() );
			y += 20;

			AddLabel( 20, y, LabelHue, "Rate:" );
			AddLabel( 110, y, LabelHue, FormatRate( current.Iterations, currentElapsed ) );
			AddLabel( 240, y, LabelHue, FormatRate( total.Iterations, totalElapsed ) );
			y += 20;

			// Average sleep
			AddLabel( 20, y, LabelHue, "Avg. sleep:" );
			if ( current.Iterations > 0 )
				AddLabel( 110, y, LabelHue, FormatTimeSpan( new TimeSpan( current.Timer( MainProfile.TimerId.Idle ).Ticks / (long) current.Iterations ) ) );
			if ( total.Iterations > 0 )
				AddLabel( 240, y, LabelHue, FormatTimeSpan( new TimeSpan( total.Timer( MainProfile.TimerId.Idle ).Ticks / (long) total.Iterations ) ) );
			y += 25;

			// Buttons
			AddButton( 50, y, 2152, 2154, 1, GumpButtonType.Reply, 0 );
			AddLabel( 85, y + 3, LabelHue, "Refresh" );
			AddButton( 150, y, 2152, 2154, 2, GumpButtonType.Reply, 0 );
			AddLabel( 185, y + 3, LabelHue, "Reset" );
			AddButton( 250, y, 2152, 2154, 0, GumpButtonType.Reply, 0 );
			AddLabel( 285, y + 3, LabelHue, "Close" );
		}

		private static string FormatTimeSpan( TimeSpan ts )
		{
			if ( ts.Days >= 1 )
				return String.Format( "{0}d{1}h", ts.Days, ts.Hours );
			else if ( ts.Hours >= 1 )
				return String.Format( "{0}h{1}m", ts.Hours, ts.Minutes );
			else if ( ts.Minutes >= 1 )
				return String.Format( "{0}m{1}s", ts.Minutes, ts.Seconds );
			else if ( ts.Seconds >= 10 )
				return String.Format( "{0}s", ts.Seconds );
			else if ( ts.Seconds >= 1 )
				return String.Format( "{0}s{1}ms", ts.Seconds, ts.Milliseconds );
			else if ( ts.Milliseconds >= 10 )
				return String.Format( "{0}ms", ts.Milliseconds );
			else if ( ts.Milliseconds >= 1 )
				return String.Format( "{0}ms{1}us", ts.Milliseconds, ( ts.Ticks / 10 ) % 1000 );
			else
				return String.Format( "{0}us", ts.Ticks / 10 );
		}

		private static string FormatRate( ulong iterations, TimeSpan elapsed )
		{
			double seconds = elapsed.TotalSeconds;

			if ( seconds <= 0.0 )
				return "n/a";

			return String.Format( "{0:0.0} Hz", iterations / seconds );
		}

		private void AddBlackAlpha( int x, int y, int width, int height )
		{
			AddImageTiled( x, y, width, height, 2624 );
			AddAlphaRegion( x, y, width, height );
		}

		public override void OnResponse( GameClient sender, RelayInfo info )
		{
			if ( sender.Mobile.AccessLevel < AccessLevel.Administrator )
				return;

			switch ( info.ButtonID )
			{
				case 1: // Refresh
					sender.Mobile.SendGump( new ProfileViewer() );
					break;

				case 2: // Reset
					Core.ResetCurrentProfile();
					sender.Mobile.SendGump( new ProfileViewer() );
					break;
			}
		}
	}
}
