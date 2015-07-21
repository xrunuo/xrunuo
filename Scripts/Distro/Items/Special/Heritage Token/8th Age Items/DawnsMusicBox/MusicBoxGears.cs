/***************************************************************************
 *                                   MusixBoxGears.cs
 *                            		------------------
 *  begin                	: August, 2007
 *  version					: 2.0 **VERSION FOR RUNUO 2.0**
 *  copyright            	: Matteo Visintin
 *  email                	: tocasia@alice.it
 *  msn						: Matteo_Visintin@hotmail.com
 ***************************************************************************/

/***************************************************************************
 *
 *   This program is free software; you can redistribute it and/or modify
 *   it under the terms of the GNU General Public License as published by
 *   the Free Software Foundation; either version 2 of the License, or
 *   (at your option) any later version.
 *
 ***************************************************************************/

using System;
using Server.Targeting;

namespace Server.Items.MusicBox
{
	[Flipable( 0x1053, 0x1054 )]
	public class MusicBoxGears : Item
	{
		#region fields
		private MusicName m_Music;
		#endregion

		#region properties
		[CommandProperty( AccessLevel.GameMaster )]
		public MusicName Music
		{
			get { return m_Music; }
		}
		#endregion

		#region constructors
		[Constructable]
		public MusicBoxGears()
			: this( TrackInfo.RandomSong() )
		{
		}

		[Constructable]
		public MusicBoxGears( MusicName music )
			: base( 0x1053 )
		{
			m_Music = music;
			Weight = 1.0;
		}

		public MusicBoxGears( Serial serial )
			: base( serial )
		{
		}
		#endregion

		#region members
		#region virtual members
		public override LocalizedText GetNameProperty()
		{
			TrackInfo ti = TrackInfo.GetInfo( m_Music );

			switch ( ti.Rarity )
			{
				default:
				case TrackRarity.Common:
					return new LocalizedText( 1075204 ); // Gear for Dawn's Music Box (Common)
				case TrackRarity.UnCommon:
					return new LocalizedText( 1075205 ); // Gear for Dawn's Music Box (Uncommon)
				case TrackRarity.Rare:
					return new LocalizedText( 1075206 ); // Gear for Dawn's Music Box (Rare)
			}
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			TrackInfo ti = TrackInfo.GetInfo( m_Music );
			list.Add( ti.Label );
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( IsChildOf( from.Backpack ) )
			{
				from.BeginTarget( 3, false, TargetFlags.None, new TargetCallback( OnTarget ) );
				from.SendMessage( "Select a Dawn's music box to add this gears to." );
			}
			else
			{
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
			}
		}

		public virtual void OnTarget( Mobile from, object obj )
		{
			if ( Deleted )
				return;

			DawnsMusicBox mb = obj as DawnsMusicBox;

			if ( mb == null )
			{
				from.SendMessage( "That is not a Dawn's music box." );
			}
			else
			{
				if ( mb.AddSong( m_Music ) )
				{
					from.SendMessage( "You have added this gear to the music box." );
					Delete();
				}
				else
					from.SendMessage( "This gear is already present in this box." );
			}
		}
		#endregion

		public static MusicBoxGears RandomMusixBoxGears( TrackRarity rarity )
		{
			return new MusicBoxGears( TrackInfo.RandomSong( rarity ) );
		}

		public static MusicBoxGears RandomMusixBoxGears()
		{
			return new MusicBoxGears( TrackInfo.RandomSong() );
		}
		#endregion

		#region serial-deserial
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.Write( (int) m_Music );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();

			m_Music = (MusicName) reader.ReadInt();
		}
		#endregion
	}
}
