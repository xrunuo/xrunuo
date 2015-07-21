using System;
using System.Collections;
using Server;
using Server.Multis;
using Server.Network;

namespace Server.Items
{

	[Furniture]
	[Flipable( 0x2D05, 0x2D06 )]
	public class SimpleElvenArmoire : BaseContainer
	{
		public override int DefaultGumpID { get { return 0x4E; } }
		public override int DefaultDropSound { get { return 0x42; } }

		public override Rectangle2D Bounds
		{
			get { return new Rectangle2D( 30, 30, 90, 150 ); }
		}

		[Constructable]
		public SimpleElvenArmoire()
			: base( 0x2D05 )
		{
			Weight = 0.0;
		}

		public override void DisplayTo( Mobile m )
		{
			if ( DynamicFurniture.Open( this, m ) )
				base.DisplayTo( m );
		}

		public SimpleElvenArmoire( Serial serial )
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

			DynamicFurniture.Close( this );
		}
	}
}

