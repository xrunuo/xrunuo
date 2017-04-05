using System;

using Server;
using Server.Gumps;
using Server.Network;
using Server.Mobiles;

namespace Server.Items
{
	public class PeerlessTeleporter : Teleporter
	{
		[Constructable]
		public PeerlessTeleporter()
			: base()
		{
		}

		public PeerlessTeleporter( Serial serial )
			: base( serial )
		{
		}

		public override bool OnMoveOver( Mobile m )
		{
			if ( m is PlayerMobile )
			{
				m.CloseGump<ConfirmPeerlessExitGump>();
				m.SendGump( new ConfirmPeerlessExitGump( this ) );
			}

			return true;
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}

		public class ConfirmPeerlessExitGump : Gump
		{
			public override int TypeID { get { return 0x1E242; } }

			private PeerlessTeleporter m_Teleporter;

			public ConfirmPeerlessExitGump( PeerlessTeleporter teleporter )
				: base( 120, 50 )
			{
				m_Teleporter = teleporter;
				AddPage( 0 );

				Closable = false;
				AddImageTiled( 0, 0, 348, 262, 0xA8E );
				AddAlphaRegion( 0, 0, 348, 262 );
				AddImage( 0, 15, 0x27A8 );
				AddImageTiled( 0, 30, 17, 200, 0x27A7 );
				AddImage( 0, 230, 0x27AA );
				AddImage( 15, 0, 0x280C );
				AddImageTiled( 30, 0, 300, 17, 0x280A );
				AddImage( 315, 0, 0x280E );
				AddImage( 15, 244, 0x280C );
				AddImageTiled( 30, 244, 300, 17, 0x280A );
				AddImage( 315, 244, 0x280E );
				AddImage( 330, 15, 0x27A8 );
				AddImageTiled( 330, 30, 17, 200, 0x27A7 );
				AddImage( 330, 230, 0x27AA );
				AddImage( 333, 2, 0x2716 );
				AddImage( 333, 248, 0x2716 );
				AddImage( 2, 248, 0x2716 );
				AddImage( 2, 2, 0x2716 );
				AddHtmlLocalized( 25, 22, 200, 20, 1049004, 0x7D00, false, false ); // Confirm
				AddImage( 25, 40, 0xBBF );
				AddHtmlLocalized( 25, 55, 300, 120, 1075026, 0xFFFFFF, false, false ); // Are you sure you wish to teleport?
				AddRadio( 25, 175, 0x25F8, 0x25FB, true, 1 );
				AddRadio( 25, 210, 0x25F8, 0x25FB, false, 2 );
				AddHtmlLocalized( 60, 180, 280, 20, 1074976, 0xFFFFFF, false, false ); // Yes
				AddHtmlLocalized( 60, 215, 280, 20, 1074977, 0xFFFFFF, false, false ); // No
				AddButton( 265, 220, 0xF7, 0xF8, 7, GumpButtonType.Reply, 0 );
			}

			public override void OnResponse( NetState sender, RelayInfo info )
			{
				Mobile m = sender.Mobile;

				if ( info.IsSwitched( 1 ) )
				{
					if ( m_Teleporter.Active )
						m_Teleporter.StartTeleport( m );
				}
			}
		}
	}
}
