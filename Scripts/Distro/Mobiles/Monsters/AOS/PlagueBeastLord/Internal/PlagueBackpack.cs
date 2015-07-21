using System;
using Server.Multis;
using Server.Mobiles;
using Server.Network;

namespace Server.Items
{
	public class PlagueBackpack : Container
	{
		public override int DefaultGumpID { get { return 0x2a63; } }
		public override int DefaultDropSound { get { return 927; } }

		public override Rectangle2D Bounds
		{
			get { return new Rectangle2D( 10, 10, 300, 300 ); }
		}

		public PlagueBackpack()
			: base( 0x261b ) //, 0x2a63, 927)
		{
			Layer = Layer.Backpack;
			Weight = 3.0;
			Movable = false;
		}


		public PlagueBackpack( Serial serial )
			: base( serial )
		{
		}

		public void MeterOrgano( Item organo, int x, int y )
		{
			AddItem( organo );
			organo.Location = new Point3D( x, y, 0 );
		}

		public override bool IsAccessibleTo( Mobile m )
		{ return ( m.CanSee( (Mobile) this.RootParent ) ); }


		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			/*int version = */reader.ReadInt();
		}
	}
}
