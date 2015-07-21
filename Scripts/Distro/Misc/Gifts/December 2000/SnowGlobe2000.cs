using System;

namespace Server.Items
{
	public class SnowGlobe2000 : Item
	{
		public override int LabelNumber { get { return 1023630; } } // crystal ball

		public int offset;

		[Constructable]
		public SnowGlobe2000()
			: base( 0xE2E )
		{
			Stackable = false;

			Weight = 1.0;

			Light = LightType.Circle150;

			LootType = LootType.Blessed;

			offset = Utility.Random( 0, 18 );
		}

		public SnowGlobe2000( Serial serial )
			: base( serial )
		{
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			list.Add( 1041454 + offset );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version 

			writer.Write( (int) offset );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */reader.ReadInt();

			offset = reader.ReadInt();
		}
	}
}