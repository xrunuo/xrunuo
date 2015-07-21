using System;
using Server;

namespace Server.Items
{
	public class ConjurersLuckyGarb : Robe
	{
		public override int LabelNumber { get { return 1114052; } } // Conjurer's Garb

		[Constructable]
		public ConjurersLuckyGarb()
		{
			Hue = 1194;
			Attributes.RegenMana = 2;
			Attributes.DefendChance = 5;
			Attributes.Luck = 140;
		}

		public ConjurersLuckyGarb( Serial serial )
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