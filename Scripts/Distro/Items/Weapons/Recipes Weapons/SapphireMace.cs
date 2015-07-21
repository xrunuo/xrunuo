using System;
using Server;

namespace Server.Items
{
	public class SapphireMace : DiamondMace
	{
		public override int LabelNumber { get { return 1073531; } } // Sapphire Mace

		[Constructable]
		public SapphireMace()
		{
			Resistances.Energy = 5;
		}


		public SapphireMace( Serial serial )
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