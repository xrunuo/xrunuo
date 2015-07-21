using System;

namespace Server.Items
{
	public class QuiverOfBlight : BaseQuiver
	{
		public override int LabelNumber { get { return 1073111; } } // Quiver Of Blight

		public override int ColdDamage { get { return 50; } }
		public override int PoisonDamage { get { return 50; } }

		public override int WeightReduction { get { return 30; } }

		[Constructable]
		public QuiverOfBlight()
		{
			Hue = 1267;
		}

		public QuiverOfBlight( Serial serial )
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
