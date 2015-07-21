using System;
using Server;

namespace Server.Items
{
	public class GlobeSwitch : Item
	{
		private KeyFragmentGlobe m_Globe;

		public GlobeSwitch( KeyFragmentGlobe globe )
			: base( 0x1091 )
		{
			m_Globe = globe;

			Movable = false;
		}

		public override void OnDoubleClick( Mobile from )
		{
			Effects.PlaySound( Location, Map, 0x3E5 );

			if ( ItemID == 0x1091 )
				ItemID = 0x1092;
			else
				ItemID = 0x1091;

			if ( m_Globe == null || m_Globe.IsActive() )
			{
				// There seems to be no further effect right now.
				SendLocalizedMessageTo( from, 1042900 );
			}
			else
			{
				// You hear a deep rumbling as something seems to happen.
				SendLocalizedMessageTo( from, 1042901 );

				m_Globe.BeginUp();
			}
		}

		public GlobeSwitch( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.Write( (Item) m_Globe );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();

			m_Globe = reader.ReadItem() as KeyFragmentGlobe;
		}
	}
}