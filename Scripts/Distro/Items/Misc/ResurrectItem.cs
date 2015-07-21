using System;
using Server.Gumps;

namespace Server.Items
{
	public class ResurrectItem : Item
	{
		private ResurrectMessage m_Message;

		[Constructable]
		public ResurrectItem( ResurrectMessage message )
			: base( 0x1BC3 )
		{
			m_Message = message;

			Movable = false;

			Visible = false;
		}

		public ResurrectItem( Serial serial )
			: base( serial )
		{
		}

		public override bool HandlesOnMovement { get { return true; } } // Tell the core that we implement OnMovement

		public override void OnMovement( Mobile m, Point3D oldLocation )
		{
			if ( m.Alive )
			{
				return;
			}

			if ( !m.InRange( GetWorldLocation(), 2 ) )
			{
				m.SendLocalizedMessage( 500446 ); // That is too far away.
			}
			else if ( m.Map != null && m.Map.CanFit( m.Location, 16, false, false ) )
			{
				m.SendGump( new ResurrectGump( m, m_Message ) );
			}
			else
			{
				m.SendLocalizedMessage( 502391 ); // Thou can not be resurrected there!
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version

			writer.Write( (int) m_Message );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 1:
					{
						m_Message = (ResurrectMessage) reader.ReadInt();

						goto case 0;
					}
				case 0:
					{
						break;
					}
			}
		}
	}
}