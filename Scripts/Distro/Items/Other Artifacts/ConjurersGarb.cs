using System;
using Server;

namespace Server.Items
{
	public class ConjurersGarb : Robe
	{
		public override int LabelNumber { get { return 1114052; } } // Conjurer's Garb

		[Constructable]
		public ConjurersGarb()
		{
			Hue = 1194;
			Attributes.RegenMana = 2;
			Attributes.DefendChance = 5;
		}

		public ConjurersGarb( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}
}