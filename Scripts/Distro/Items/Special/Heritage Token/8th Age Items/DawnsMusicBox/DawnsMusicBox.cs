using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Engines.Housing;
using Server.Engines.Housing.Multis;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;
using Server.ContextMenus;
using Server.Engines.VeteranRewards;
using Server.Multis;


namespace Server.Items.MusicBox
{
	[Flipable( 0x2AF9, 0x2AFD )]
	public class DawnsMusicBox : Item, ISecurable, IRewardItem
	{
		#region Fields
		private List<MusicName> m_Tracks;
		private Timer m_PlayingTimer;
		private MusicName m_ActualSong;
		#endregion

		#region Constants
		public static readonly int MusicRange = 10;
		#endregion

		#region Properties
		public override int LabelNumber { get { return 1075198; } } // Dawn’s Music Box

		[CommandProperty( AccessLevel.GameMaster, AccessLevel.Developer )]
		public bool IsPlaying
		{
			get { return m_PlayingTimer != null; }
		}

		public List<MusicName> Tracks
		{
			get { return m_Tracks; }
		}

		[CommandProperty( AccessLevel.GameMaster, AccessLevel.Developer )]
		public MusicName ActualSong
		{
			get { return m_ActualSong; }
			set { m_ActualSong = value; InvalidateProperties(); }
		}
		#endregion

		#region Constructors
		[Constructable]
		public DawnsMusicBox()
			: base( 0x2AF9 )
		{
			Weight = 1.0;

			m_Tracks = new List<MusicName>();
			m_ActualSong = MusicName.Invalid;

			while ( Tracks.Count < 4 )
				AddSong( TrackInfo.RandomSong( TrackRarity.Common ) );
		}

		public DawnsMusicBox( Serial serial )
			: base( serial )
		{
		}
		#endregion

		#region ISecurable members
		private SecureLevel m_Level;

		[CommandProperty( AccessLevel.GameMaster )]
		public SecureLevel Level
		{
			get { return m_Level; }
			set { m_Level = value; }
		}
		#endregion

		#region IRewardItem members
		private bool m_IsRewardItem;

		[CommandProperty( AccessLevel.GameMaster )]
		public bool IsRewardItem
		{
			get { return m_IsRewardItem; }
			set { m_IsRewardItem = value; }
		}
		#endregion

		#region Members
		#region Virtual Members
		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			int commonSongs = 0;
			int unCommonSongs = 0;
			int rareSongs = 0;

			for ( int i = 0; i < m_Tracks.Count; i++ )
			{
				TrackInfo ti = TrackInfo.GetInfo( m_Tracks[i] );
				switch ( ti.Rarity )
				{
					case TrackRarity.Common:
						commonSongs++;
						break;
					case TrackRarity.UnCommon:
						unCommonSongs++;
						break;
					case TrackRarity.Rare:
						rareSongs++;
						break;
				}
			}

			if ( commonSongs > 0 )
				list.Add( 1075234, commonSongs.ToString() ); // ~1_NUMBER~ Common Tracks
			if ( unCommonSongs > 0 )
				list.Add( 1075235, unCommonSongs.ToString() ); // ~1_NUMBER~ Uncommon Tracks
			if ( rareSongs > 0 )
				list.Add( 1075236, rareSongs.ToString() ); // ~1_NUMBER~ Rare Tracks
		}

		public override void GetContextMenuEntries( Mobile from, List<ContextMenuEntry> list )
		{
			base.GetContextMenuEntries( from, list );

			SetSecureLevelEntry.AddTo( from, this, list ); // Set secure level
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( m_Tracks.Count < 1 )
			{
				from.SendMessage( "This music box is empty." );
			}
			else if ( IsOwner( from ) )
			{
				if ( !IsLockedDown )
					from.SendLocalizedMessage( 502692 ); // This must be in a house and be locked down to work.
				else
				{
					if ( from.HasGump( typeof( MusicGump ) ) )
						from.CloseGump( typeof( MusicGump ) );

					from.SendGump( new MusicGump( this ) );
				}
			}
			else
			{
				from.SendLocalizedMessage( 502691 ); // You must be the owner to use this.
			}
		}
		#endregion

		public bool AddSong( MusicName song )
		{
			if ( m_Tracks.Contains( song ) )
			{
				return false;
			}
			else
			{
				m_Tracks.Add( song );
				return true;
			}
		}

		public void Animate()
		{
			switch ( ItemID )
			{
				case 0x2AF9:
					ItemID = 0x2AFB;
					break;
				//            	case 0x2AFA:	ItemID = 0x2AFB; break;
				case 0x2AFB:
					ItemID = 0x2AFC;
					break;
				case 0x2AFC:
					ItemID = 0x2AF9;
					break;

				case 0x2AFD:
					ItemID = 0x2AFF;
					break;
				//            	case 0x2AFE:	ItemID = 0x2AFF; break;
				case 0x2AFF:
					ItemID = 0x2B00;
					break;
				case 0x2B00:
					ItemID = 0x2AFD;
					break;
			}
		}

		public bool IsOwner( Mobile mob )
		{
			if ( mob.AccessLevel >= AccessLevel.GameMaster )
				return true;

			IHouse house = HousingHelper.FindHouseAt( this );

			return ( house != null && house.IsOwner( mob ) );
		}

		public void ToggleMusic( Mobile m, bool play )
		{
			if ( m_ActualSong != MusicName.Invalid && m.Client != null )
			{
				m.Send( PlayMusic.InvalidInstance ); // Stop actual music

				if ( play )
					m.Send( PlayMusic.GetInstance( m_ActualSong ) );
			}
		}

		public void TogglePlaying( bool hasToStart )
		{
			ToggleTimer( hasToStart );

			string message = hasToStart ? "* The music box starts playing a song *" : "* The music box stops *";

			PublicOverheadMessage( MessageType.Regular, 0x5D, true, message );
			StopBoxesInRange();
			Map boxMap = this.Map;

			if ( boxMap != Map.Internal )
			{
				Point3D boxLoc = this.Location;

				foreach ( Mobile m in boxMap.GetMobilesInRange( boxLoc, MusicRange ) )
				{
					if ( m is Mobiles.PlayerMobile )
						ToggleMusic( m, hasToStart );
				}
			}
		}

		public void ToggleTimer( bool hasToStart )
		{
			if ( IsPlaying && !hasToStart )
			{
				if ( m_PlayingTimer != null && m_PlayingTimer.Running )	// remove correctly the timer...
					m_PlayingTimer.Stop();
				m_PlayingTimer = null;
			}
			else if ( !IsPlaying && hasToStart )
			{
				TrackInfo ti = TrackInfo.GetInfo( m_ActualSong );

				m_PlayingTimer = new PlayingTimer( (double) ti.Duration, this );	// add a new timer
				m_PlayingTimer.Start();
			}
		}

		public void StopBoxesInRange()
		{
			Map boxMap = this.Map;

			if ( boxMap != Map.Internal )
			{
				Point3D boxLoc = this.Location;

				foreach ( Item i in boxMap.GetItemsInRange( boxLoc, MusicRange ) )
				{
					if ( i is DawnsMusicBox && i != this )
					{
						DawnsMusicBox mb = (DawnsMusicBox) i;
						if ( mb.IsPlaying )
						{
							mb.ToggleTimer( false );
							mb.PublicOverheadMessage( MessageType.Regular, 0x5D, true, "* The musix box stops *" );
						}
					}
				}
			}
		}
		#endregion

		#region PlayingTimer
		internal class PlayingTimer : Timer
		{
			#region fields
			private DawnsMusicBox m_Box;
			private DateTime m_Until;
			#endregion

			#region constructors
			public PlayingTimer( double duration, DawnsMusicBox box )
				: base( TimeSpan.FromSeconds( 1.0 ), TimeSpan.FromSeconds( 1.0 ) )
			{
				m_Box = box;
				m_Until = DateTime.Now + TimeSpan.FromSeconds( duration );

			}
			#endregion

			#region members
			protected override void OnTick()
			{
				if ( DateTime.Now > m_Until )
				{
					if ( m_Box != null && !m_Box.Deleted )
						m_Box.TogglePlaying( false );
					else
						Stop();
				}
				else if ( m_Box != null && !m_Box.Deleted )
					m_Box.Animate();
			}
			#endregion
		}
		#endregion

		#region MusicGump
		private class MusicGump : Gump
		{
			#region Constants
			private static readonly int m_Fields = 9;
			private static readonly int m_HueTit = 32767;
			private static readonly int m_HueEnt = 32767;
			private static readonly int m_DeltaBut = 2;
			private static readonly int m_FieldsDist = 25;
			#endregion

			#region Fields
			private DawnsMusicBox m_Box;
			private List<int> m_Songs;
			private int m_Page;
			private bool m_HasStopSongEntry;
			#endregion

			#region Constructors
			public MusicGump( DawnsMusicBox box )
				: this( box, null, 1 )
			{
			}

			public MusicGump( DawnsMusicBox box, List<int> songs, int page )
				: base( 50, 50 )
			{
				Closable = false;
				Disposable = true;
				Dragable = true;
				Resizable = false;

				m_Box = box;
				m_Songs = songs;
				m_Page = page;

				m_HasStopSongEntry = m_Box.IsPlaying;

				if ( m_Songs == null )
					m_Songs = BuildList( box, m_HasStopSongEntry );

				Initialize();
			}
			#endregion

			#region Members
			private static List<int> BuildList( DawnsMusicBox box, bool hasStopSongEntry )
			{
				List<int> list = new List<int>();

				for ( int i = 0; i < box.Tracks.Count; i++ )
				{
					TrackInfo ti = TrackInfo.GetInfo( box.Tracks[i] );
					list.Add( ti.Label );
				}

				if ( hasStopSongEntry )
					list.Add( 1075207 ); // Stop Song

				return list;
			}

			public void Initialize()
			{
				AddPage( 0 );

				AddBackground( 0, 0, 275, 325, 9200 );

				AddImageTiled( 10, 10, 255, 25, 2624 );
				AddImageTiled( 10, 45, 255, 240, 2624 );
				AddImageTiled( 40, 295, 225, 20, 2624 );

				AddButton( 10, 295, 4017, 4018, 0, GumpButtonType.Reply, 0 );
				AddHtmlLocalized( 45, 295, 75, 20, 1011012, m_HueTit, false, false ); // CANCEL

				AddAlphaRegion( 10, 10, 255, 285 );
				AddAlphaRegion( 40, 295, 225, 20 );

				AddHtmlLocalized( 14, 12, 255, 25, 1075130, m_HueTit, false, false ); // Choose a track to play

				if ( m_Page > 1 )
					AddButton( 225, 297, 5603, 5607, 200, GumpButtonType.Reply, 0 ); // Previous page

				if ( m_Page < Math.Ceiling( m_Songs.Count / (double) m_Fields ) )
					AddButton( 245, 297, 5601, 5605, 300, GumpButtonType.Reply, 0 ); // Next Page

				int IndMax = ( m_Page * m_Fields ) - 1;
				int IndMin = ( m_Page * m_Fields ) - m_Fields;
				int IndTemp = 0;

				for ( int i = 0; i < m_Songs.Count; i++ )
				{
					if ( i >= IndMin && i <= IndMax )
					{
						AddHtmlLocalized( 35, 52 + ( IndTemp * m_FieldsDist ), 225, 20, m_Songs[i], m_HueEnt, false, false );
						AddButton( 15, 52 + m_DeltaBut + ( IndTemp * m_FieldsDist ), 1209, 1210, i + 1, GumpButtonType.Reply, 0 );
						IndTemp++;
					}
				}
			}

			public override void OnResponse( GameClient sender, RelayInfo info )
			{
				Mobile from = sender.Mobile;

				if ( info.ButtonID == 0 )
					return;
				else if ( info.ButtonID == 200 ) // Previous page
				{
					m_Page--;
					from.SendGump( new MusicGump( m_Box, m_Songs, m_Page ) );
				}
				else if ( info.ButtonID == 300 )  // Next Page
				{
					m_Page++;
					from.SendGump( new MusicGump( m_Box, m_Songs, m_Page ) );
				}
				else if ( m_HasStopSongEntry && info.ButtonID == m_Songs.Count )
				{
					m_Box.TogglePlaying( false );
				}
				else
				{
					TrackInfo ti = TrackInfo.GetInfo( m_Songs[info.ButtonID - 1] );
					m_Box.ActualSong = ti.Name;
					m_Box.TogglePlaying( true );
				}
			}
			#endregion
		}
		#endregion

		#region Serialization
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.Write( m_Tracks.Count );

			for ( int i = 0; i < m_Tracks.Count; i++ )
				writer.Write( (int) m_Tracks[i] );

			writer.Write( (int) m_Level );
			writer.Write( m_IsRewardItem );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 0:
					{
						if ( m_Tracks == null )
							m_Tracks = new List<MusicName>();

						int numSongs = reader.ReadInt();
						for ( int i = 0; i < numSongs; i++ )
							m_Tracks.Add( (MusicName) reader.ReadInt() );

						m_Level = (SecureLevel) reader.ReadInt();
						m_IsRewardItem = reader.ReadBool();

						ToggleTimer( false );

						break;
					}
			}
		}
		#endregion
	}
}
