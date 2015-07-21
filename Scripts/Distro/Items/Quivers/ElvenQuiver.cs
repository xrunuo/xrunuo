using System;

namespace Server.Items
{
	public class ElvenQuiver : BaseQuiver
	{
		public override int WeightReduction { get { return 30; } }

		[Constructable]
		public ElvenQuiver()
		{
			Hue = 2210;
		}

		public ElvenQuiver( Serial serial )
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
