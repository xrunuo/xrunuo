using System;
using Server.Items;

namespace Server.Items
{
	public class TailorStone : Item
	{
		[Constructable]
		public TailorStone()
			: base( 0xED4 )
		{
			Movable = false;
			Hue = 0x315;
			Name = "a Tailor Supply Stone";
		}

		public override void OnDoubleClick( Mobile from )
		{
			TailorBag tailorBag = new TailorBag();

			if ( !from.AddToBackpack( tailorBag ) )
			{
				tailorBag.Delete();
			}
		}

		public TailorStone( Serial serial )
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