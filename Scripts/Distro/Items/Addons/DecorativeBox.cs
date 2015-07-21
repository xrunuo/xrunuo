using System;
using Server;


namespace Server.Items
{
	[FlipableAttribute( 0x2DF4, 0x2DF3 )]
	public class DecorativeBox : Item
	{
		public override int LabelNumber { get { return 1031763; } } // DecorativeBox

		[Constructable]
		public DecorativeBox()
			: base( 0x2DF4 )
		{
		}

		public DecorativeBox( Serial serial )
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