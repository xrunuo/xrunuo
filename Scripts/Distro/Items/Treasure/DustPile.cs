using System;

namespace Server.Items
{
	public class DustPile : Item
	{
		public override int LabelNumber { get { return 1115939; } } // a dust pile<br>

		[Constructable]
		public DustPile()
			: base( 0x573D )
		{
			Hue = 2955;
			Weight = 1.0;
			Stackable = false;

			Timer.DelayCall( TimeSpan.FromMinutes( 1.0 ), Delete );
		}

		public DustPile( Serial serial )
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