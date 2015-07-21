using System;
using Server;

namespace Server.Items
{
	public class OrcishMachete : ElvenMachete
	{
		public override int LabelNumber { get { return 1073534; } } // Orcish Machete

		[Constructable]
		public OrcishMachete()
		{
			Attributes.WeaponDamage = 10;
			Attributes.BonusInt = -5;

		}


		public OrcishMachete( Serial serial )
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