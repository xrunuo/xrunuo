using System;
using Server;

namespace Server.Items
{
	public class SilverEtchedMace : DiamondMace
	{
		public override int LabelNumber { get { return 1073532; } } // Silver-Etched Mace

		[Constructable]
		public SilverEtchedMace()
		{
			Slayer = SlayerName.Undead;
		}


		public SilverEtchedMace( Serial serial )
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