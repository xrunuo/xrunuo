using System;
using Server;

namespace Server.Items
{
	public class DiseasedMachete : ElvenMachete
	{
		public override int LabelNumber { get { return 1073536; } } // Diseased Machete

		[Constructable]
		public DiseasedMachete()
		{
			WeaponAttributes.HitPoisonArea = 25;
		}


		public DiseasedMachete( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */reader.ReadInt();
		}
	}
}