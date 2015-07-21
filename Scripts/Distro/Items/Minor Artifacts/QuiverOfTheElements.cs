using System;

namespace Server.Items
{
	public class QuiverOfTheElements : BaseQuiver
	{
		public override int LabelNumber { get { return 1075040; } } // Quiver of the Elements

		public override int ChaosDamage { get { return 100; } }
		public override int WeightReduction { get { return 50; } }

		[Constructable]
		public QuiverOfTheElements()
		{
			Hue = 235;
		}

		public QuiverOfTheElements( Serial serial )
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
