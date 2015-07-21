using System;

namespace Server.Items
{
	public class WhiteLeatherDyeTub : LeatherDyeTub
	{

		public override int LabelNumber { get { return 1149900; } } // White Leather Dye Tub

		[Constructable]
		public WhiteLeatherDyeTub()
		{
			Hue = DyedHue = 0x9C2;
			Redyable = false;
			LootType = LootType.Blessed;
		}

		public WhiteLeatherDyeTub( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

		}
	}
}
