using System;

namespace Server.Items
{
	public class RedVelvetBox : Container
	{
		public override int LabelNumber { get { return 1077596; } } // A Red Velvet Box

		[Constructable]
		public RedVelvetBox()
			: base( 3706 )
		{
			Weight = 2.0;
			Hue = 32;
		}

		public RedVelvetBox( Serial serial )
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
