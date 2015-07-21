using System;
using Server;

namespace Server.Items
{
	public class WoodPulp : Item, ICommodity
	{
		public override int LabelNumber { get { return 1113136; } } // wood pulp

		[Constructable]
		public WoodPulp()
			: this( 1 )
		{
		}

		[Constructable]
		public WoodPulp( int amount )
			: base( 0x26B8 )
		{
			Stackable = true;
			Weight = 1.0;
			Amount = amount;
			Hue = 0x353;
		}

		public WoodPulp( Serial serial )
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