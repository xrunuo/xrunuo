using System;
using System.Collections;
using Server;
using Server.Mobiles;
using Server.Network;

namespace Server.Items
{
	public class PuzzleLever : Item
	{
		private LampController m_Controller;

		private int m_Code;

		[CommandProperty( AccessLevel.GameMaster )]
		public int Code { get { return m_Code; } set { m_Code = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public LampController Controller { get { return m_Controller; } set { m_Controller = value; } }

		[Constructable]
		public PuzzleLever( int code )
			: base( 0x108E )
		{
			Hue = 0x66D;

			Movable = false;

			m_Code = code;
		}

		public PuzzleLever( Serial serial )
			: base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( !from.InRange( this.GetWorldLocation(), 1 ) )
			{
				from.LocalOverheadMessage( MessageType.Regular, 0x3B2, 500446 ); //That is too far away
				return;
			}

			if ( from.Hidden )
			{
				from.PrivateOverheadMessage( MessageType.Regular, 0x3B2, true, "You can not use that while hidden", from.Client );
				return;
			}

			if ( m_Controller != null && m_Controller.CanActive )
			{
				if ( m_Controller.Code.Length < 4 )
				{
					m_Controller.Code += m_Code;

					if ( ItemID == 0x108E )
						ItemID = 0x108C;
					else if ( ItemID == 0x108C )
						ItemID = 0x108E;

					Effects.PlaySound( Location, Map, 0x3E8 );
				}
			}
			else
			{
				from.SendLocalizedMessage( 1060001 ); // You throw the switch, but the mechanism cannot be engaged again so soon.
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.Write( (int) m_Code );

			writer.Write( m_Controller );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();

			m_Code = reader.ReadInt();

			m_Controller = reader.ReadItem() as LampController;
		}
	}
}