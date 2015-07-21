using System;

namespace Server.Items
{
	public class CapturedEssence : Item, ICommodity
	{
		public override int LabelNumber { get { return 1032686; } } // Captured Essence

		[Constructable]
		public CapturedEssence()
			: this( 1 )
		{
		}

		[Constructable]
		public CapturedEssence( int amount )
			: base( 0x318E )
		{
			Stackable = true;
			Weight = 1;
			Amount = amount;
		}

		public CapturedEssence( Serial serial )
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