using System;

namespace Server.Items
{
	[FlipableAttribute( 0x2BDF, 0x2BE0 )]
	public class GreenCandyCane : Food
	{
		public override int LabelNumber { get { return 1031231; } } // Green Candy Cane

		[Constructable]
		public GreenCandyCane()
			: this( Utility.RandomBool() ? 0x2BDF : 0x2BE0 )
		{
		}

		[Constructable]
		public GreenCandyCane( int itemID )
			: base( itemID )
		{
			Weight = 1.0;

			LootType = LootType.Blessed;
		}


		public GreenCandyCane( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */reader.ReadInt();
		}
	}
}