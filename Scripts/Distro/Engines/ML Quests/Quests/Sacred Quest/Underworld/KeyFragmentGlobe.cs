using System;
using Server;

namespace Server.Items
{
	public class KeyFragmentGlobe : Item
	{
		public override int LabelNumber { get { return 1033911; } } // globe of sosaria

		private Timer m_StateTimer;
		private int m_OffsetZ;
		private KeyFragmentSpawner m_KeySpawner;

		public KeyFragmentSpawner KeySpawner
		{
			get { return m_KeySpawner; }
			set { m_KeySpawner = value; }
		}

		public KeyFragmentGlobe( int hue )
			: base( 0x3660 )
		{
			Hue = hue;
			Movable = false;
		}

		public bool IsActive()
		{
			return m_StateTimer != null;
		}

		public void BeginUp()
		{
			if ( m_StateTimer != null )
				return;

			if ( m_KeySpawner != null )
				m_KeySpawner.Visible = true;

			m_StateTimer = Timer.DelayCall( TimeSpan.FromSeconds( 2.0 ), TimeSpan.FromSeconds( 2.0 ), new TimerCallback(
				delegate
				{
					m_OffsetZ++;
					Z++;

					if ( m_OffsetZ == 10 )
					{
						Effects.PlaySound( Location, Map, 0x475 );
						m_StateTimer.Stop();
						m_StateTimer = Timer.DelayCall( TimeSpan.FromSeconds( 10.0 ), new TimerCallback( BeginDown ) );
					}
					else
					{
						Effects.PlaySound( Location, Map, 0x477 );
					}
				}
			) );
		}

		public void BeginDown()
		{
			m_StateTimer = Timer.DelayCall( TimeSpan.FromSeconds( 2.0 ), TimeSpan.FromSeconds( 2.0 ), new TimerCallback(
				delegate
				{
					m_OffsetZ--;
					Z--;

					if ( m_OffsetZ == 0 )
					{
						Effects.PlaySound( Location, Map, 0x475 );
						m_StateTimer.Stop();
						m_StateTimer = null;

						if ( m_KeySpawner != null )
							m_KeySpawner.Visible = false;
					}
					else
					{
						Effects.PlaySound( Location, Map, 0x477 );
					}
				}
			) );
		}

		public KeyFragmentGlobe( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.Write( (Item) m_KeySpawner );
			writer.Write( (int) m_OffsetZ );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();

			m_KeySpawner = reader.ReadItem() as KeyFragmentSpawner;
			m_OffsetZ = reader.ReadInt();

			if ( m_OffsetZ != 0 )
			{
				Z -= m_OffsetZ;
				m_OffsetZ = 0;
			}
		}
	}
}