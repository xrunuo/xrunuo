using System;
using Server;

namespace Server.Items
{
	public class TrueLeafblade : Leafblade
	{
		public override int LabelNumber { get { return 1073521; } } // True Leafblade

		[Constructable]
		public TrueLeafblade()
		{
			Resistances.Poison = 5;
		}


		public TrueLeafblade( Serial serial )
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