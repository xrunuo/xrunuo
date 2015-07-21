using System;
using Server.Items;

namespace Server.Items
{
	[Flipable( 0x2794, 0x27DF )]
	public class ClothNinjaJacket : BaseShirt
	{
		[Constructable]
		public ClothNinjaJacket()
			: this( 0 )
		{
		}

		[Constructable]
		public ClothNinjaJacket( int hue )
			: base( 0x2794, hue )
		{
			Weight = 5.0;
			Layer = Layer.InnerTorso;
		}

		public ClothNinjaJacket( Serial serial )
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