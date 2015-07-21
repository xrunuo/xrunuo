using System;

namespace Server.Items
{
	public class QuiverOfFire : BaseQuiver
	{
		public override int LabelNumber { get { return 1073109; } } // Quiver of Fire 

		public override int PhysicalDamage { get { return 50; } }
		public override int FireDamage { get { return 50; } }

		public override int WeightReduction { get { return 30; } }

		[Constructable]
		public QuiverOfFire()
		{
			Hue = 1255;
		}

		public QuiverOfFire( Serial serial )
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
