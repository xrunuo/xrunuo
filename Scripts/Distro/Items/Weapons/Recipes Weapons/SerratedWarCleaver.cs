using System;
using Server;

namespace Server.Items
{
	public class SerratedWarCleaver : WarCleaver
	{
		public override int LabelNumber { get { return 1073527; } } // Serrated War Cleaver

		[Constructable]
		public SerratedWarCleaver()
		{
			Attributes.WeaponDamage = 7;
		}


		public SerratedWarCleaver( Serial serial )
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