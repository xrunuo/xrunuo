using System;
using System.Collections;
using Server;
using Server.Mobiles;
using Server.Network;

namespace Server.Items
{
	public class CrystalBall : Item
	{
		public override int LabelNumber { get { return 1023630; } } // crystal ball

		[Constructable]
		public CrystalBall()
			: base( 0xE2E )
		{
			Weight = 10.0;

			Light = LightType.Circle150;
		}
		
		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			list.Add( 1041302 ); // Looking into the crystal ball, thou doth see swirling mists in which words form, 'Ask and be answered.'
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( from.InRange( this.GetWorldLocation(), 2 ) )
			{
				int offset = Utility.Random( 0, 71 );

				SendLocalizedMessageTo( from, 1007000 + offset );
			}
			else
			{
				from.LocalOverheadMessage( MessageType.Regular, 0x3B2, 1019045 ); // I can't reach that.
			}
		}

		public CrystalBall( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */reader.ReadEncodedInt();
		}
	}
}
