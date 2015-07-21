using System;

namespace Server.Items
{
	public class SackOfSugar : Item
	{
		[Constructable]
		public SackOfSugar()
			: base( 0x1039 )
		{
			Weight = 1.0;
			Stackable = true;
			Hue = 0x83F;
			Name = "Sack Of Sugar";

		}

		public SackOfSugar( Serial serial ) : base( serial ) { }

		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 ); }

		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt(); }
	}
}