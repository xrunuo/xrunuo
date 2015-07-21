using System;

namespace Server.Items
{
	public class Vanilla : Item
	{
		[Constructable]
		public Vanilla()
			: base( 0xE2A )
		{
			Weight = 1.0;
			Stackable = true;
			Hue = 0x1C1;
			Name = "Vanilla";

		}

		public Vanilla( Serial serial ) : base( serial ) { }

		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 ); }

		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt(); }
	}
}