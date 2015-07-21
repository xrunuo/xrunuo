using System;
using Server;
using Server.Gumps;
using Server.Network;

namespace Server.Items
{
	public class RolledMap : Item
	{
		public override int LabelNumber { get { return 1025357; } } // rolled map

		[Constructable]
		public RolledMap()
			: base( 0x14ED )
		{
			Weight = 1.0;
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( from.Map != this.Map || !from.InRange( GetWorldLocation(), 2 ) )
				from.LocalOverheadMessage( MessageType.Regular, 0x3B2, 1019045 ); // I can't reach that.
			else
			{
				from.CloseGump( typeof( UnderworldMapGump ) );
				from.SendGump( new UnderworldMapGump() );
			}
		}

		public RolledMap( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}
}