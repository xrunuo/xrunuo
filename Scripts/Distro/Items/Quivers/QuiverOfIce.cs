using System;

namespace Server.Items
{
	public class QuiverOfIce : BaseQuiver
	{
		public override int LabelNumber { get { return 1073110; } } // Quiver of Ice 

		public override int PhysicalDamage { get { return 50; } }
		public override int ColdDamage { get { return 50; } }

		public override int WeightReduction { get { return 30; } }

		[Constructable]
		public QuiverOfIce()
		{
			Hue = 1261;
		}

		public QuiverOfIce( Serial serial )
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
