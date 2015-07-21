using System;

namespace Server.Items
{
	public class DreadHornMane : Item, ICommodity
	{
		public override int LabelNumber { get { return 1032682; } } // Dread Horn Mane

		[Constructable]
		public DreadHornMane()
			: this( 1 )
		{
		}

		[Constructable]
		public DreadHornMane( int amount )
			: base( 0x318A )
		{
			Stackable = true;
			Weight = 1;
			Amount = amount;
		}

		public DreadHornMane( Serial serial )
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