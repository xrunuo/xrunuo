using System;

namespace Server.Items
{
	public class QuiverOfLightning : BaseQuiver
	{
		public override int LabelNumber { get { return 1073112; } } // Quiver of Lightning 

		public override int PhysicalDamage { get { return 50; } }
		public override int EnergyDamage { get { return 50; } }

		public override int WeightReduction { get { return 30; } }

		[Constructable]
		public QuiverOfLightning()
		{
			Hue = 1273;
		}

		public QuiverOfLightning( Serial serial )
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
