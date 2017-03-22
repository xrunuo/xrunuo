using System;
using Server;
using Server.Engines.Housing.Multis;
using Server.Items;
using Server.Targeting;
using Server.Multis;
using Server.Events;

namespace Server.Mobiles
{
	public class Parrot : Mobile
	{
		private DateTime m_BirthDate;
		private ParrotPerchAddon m_Perch;

		[CommandProperty( AccessLevel.GameMaster )]
		public DateTime BirthDate
		{
			get { return m_BirthDate; }
			set { m_BirthDate = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public ParrotPerchAddon Perch
		{
			get { return m_Perch; }
			set { m_Perch = value; }
		}

		public static int GetAge( DateTime birthdate )
		{
			TimeSpan ts = DateTime.UtcNow - birthdate;
			return (int) ( ts.TotalDays / 7 );
		}

		public Parrot( string name, int hue, DateTime birthdate )
		{
			Name = name;
			Title = "the parrot";
			Body = 0x11A;
			Hue = hue;
			m_BirthDate = birthdate;
			Blessed = true;
			Hits = HitsMax;
		}

		public Parrot( int hue )
			: this( "a pet parrot", hue, DateTime.UtcNow )
		{
		}

		public Parrot()
			: this( Utility.RandomDyedHue() )
		{
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			int age = GetAge( m_BirthDate );

			if ( age > 0 )
				list.Add( 1072627, age.ToString() ); // ~1_AGE~ weeks old
		}

		public override bool HandlesOnSpeech( Mobile from )
		{
			return ( from.Alive && 0.05 > Utility.RandomDouble() && from.GetDistanceToSqrt( this ) <= 10 );
		}

		public override void OnSpeech( SpeechEventArgs e )
		{
			Mobile from = e.Mobile;

			if ( e.Handled )
				return;

			Say( e.Speech );
			PlaySound( Utility.RandomMinMax( 0xBF, 0xC3 ) );
			Spells.SpellHelper.Turn( this, from );
		}

		public override bool CanBeRenamedBy( Mobile from )
		{
			if ( base.CanBeRenamedBy( from ) )
				return true;

			if ( Perch == null )
				return false;

			BaseHouse h = Perch.MyHouse;

			if ( h != null && ( h.Owner == from || h.CoOwners.Contains( from ) ) )
			{
				from.SendLocalizedMessage( 1072625 ); // As the house owner, you may rename this Parrot.
				return true;
			}

			return false;
		}

		public Parrot( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.Write( m_BirthDate );
			writer.Write( m_Perch );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();

			m_BirthDate = reader.ReadDateTime();
			m_Perch = reader.ReadItem() as ParrotPerchAddon;
		}
	}
}