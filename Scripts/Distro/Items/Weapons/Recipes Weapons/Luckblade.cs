using System;
using Server;

namespace Server.Items
{
	public class Luckblade : Leafblade
	{
		public override int LabelNumber { get { return 1073522; } } // Luckblade

		[Constructable]
		public Luckblade()
		{
			Attributes.Luck = 20;
		}


		public Luckblade( Serial serial )
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