using System;

namespace Server.Items
{
	[FlipableAttribute( 0x2BDD, 0x2BDE )]
	public class RedCandyCane : Food
	{
		public override int LabelNumber { get { return 1031229; } } // Red Candy Cane

		[Constructable]
		public RedCandyCane()
			: this( Utility.RandomBool() ? 0x2BDD : 0x2BDE )
		{
		}

		[Constructable]
		public RedCandyCane( int itemID )
			: base( itemID )
		{
			Weight = 1.0;

			LootType = LootType.Blessed;
		}

		public RedCandyCane( Serial serial )
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