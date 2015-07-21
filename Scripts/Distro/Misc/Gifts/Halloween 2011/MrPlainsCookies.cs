using System;
using System.Collections;
using Server.Network;

namespace Server.Items
{
	public class MrPlainsCookies : Food
	{
		private InternalTimer toothache;

		[Constructable]
		public MrPlainsCookies()
			: base( 0x160C )
		{
			Name = "Mr. Plain's Cookies";
			Hue = 2115;
			Stackable = false;
			this.Weight = 1.0;
			this.FillFactor = 0;
		}

		public MrPlainsCookies( Serial serial )
			: base( serial )
		{
		}

		public override bool Eat( Mobile from )
		{
			from.PlaySound( Utility.Random( 0x3A, 3 ) );

			if ( from.Body.IsHuman && !from.Mounted )
				from.Animate( 34, 5, 1, true, false, 0 );

			if ( Poison != null )
				from.ApplyPoison( Poisoner, Poison );

			if ( Utility.RandomDouble() < 0.05 )
				GiveToothAche( from, 0 );
			else
				from.SendLocalizedMessage( 1077387 );

			Consume();
			return true;
		}

		public void GiveToothAche( Mobile from, int seq )
		{
			if ( toothache != null )
				toothache.Stop();
			
			from.SendLocalizedMessage( 1077388 + seq );

			if ( seq < 5 )
			{
				toothache = new InternalTimer( this, from, seq, TimeSpan.FromSeconds( 15 ) );
				toothache.Start();
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}

		private class InternalTimer : Timer
		{
			private MrPlainsCookies m_Cookies;
			private int m_Sequencer;
			private Mobile m_Mobile;

			public InternalTimer( MrPlainsCookies cookies, Mobile m, int sequencer, TimeSpan delay )
				: base( delay )
			{
				m_Cookies = cookies;
				m_Mobile = m;
				m_Sequencer = sequencer;
			}

			protected override void OnTick()
			{
				if ( m_Mobile != null )
				{
					m_Cookies.GiveToothAche( m_Mobile, m_Sequencer + 1 );
				}
			}
		}
	}
}