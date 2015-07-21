using System;

namespace Server.Items
{
	public class FireRuby : Item, ICommodity
	{
		public override int LabelNumber { get { return 1032695; } } // Fire Ruby

		[Constructable]
		public FireRuby()
			: this( 1 )
		{
		}

		[Constructable]
		public FireRuby( int amount )
			: base( 0x3197 )
		{
			Stackable = true;
			Weight = 1;
			Amount = amount;
		}

		public FireRuby( Serial serial )
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