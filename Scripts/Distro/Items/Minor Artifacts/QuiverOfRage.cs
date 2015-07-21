using System;

namespace Server.Items
{
	public class QuiverOfRage : BaseQuiver
	{
		public override int LabelNumber { get { return 1075038; } } // Quiver of Rage

		public override int PhysicalDamage { get { return 20; } }
		public override int FireDamage { get { return 20; } }
		public override int ColdDamage { get { return 20; } }
		public override int PoisonDamage { get { return 20; } }
		public override int EnergyDamage { get { return 20; } }

		public override int WeightReduction { get { return 25; } }

		[Constructable]
		public QuiverOfRage()
		{
			Hue = 588;
		}

		public QuiverOfRage( Serial serial )
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