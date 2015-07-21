using System;
using Server;

namespace Server.Items
{
	public class TrueWarCleaver : WarCleaver
	{
		public override int LabelNumber { get { return 1073528; } } // True War Cleaver

		[Constructable]
		public TrueWarCleaver()
		{
			Attributes.RegenHits = 2;
			Attributes.WeaponDamage = 4;
		}


		public TrueWarCleaver( Serial serial )
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