using System;
using Server.Items;

namespace Server.Items
{
	[Flipable( 0x2783, 0x27CE )]
	public class FemaleKimono : BaseOuterTorso
	{
		[Constructable]
		public FemaleKimono()
			: this( 0 )
		{
		}

		[Constructable]
		public FemaleKimono( int hue )
			: base( 0x2783, hue )
		{
			Weight = 3.0;
		}

		public FemaleKimono( Serial serial )
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