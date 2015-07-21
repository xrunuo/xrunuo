using System;
using Server;

namespace Server.Items
{
	public class EmeraldMace : DiamondMace
	{
		public override int LabelNumber { get { return 1073530; } } // Emerald Mace

		[Constructable]
		public EmeraldMace()
		{
			Resistances.Poison = 5;
		}


		public EmeraldMace( Serial serial )
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