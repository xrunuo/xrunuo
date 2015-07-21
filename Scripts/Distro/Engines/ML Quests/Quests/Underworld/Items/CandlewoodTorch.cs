using System;
using Server;

namespace Server.Items
{
	public class CandlewoodTorch : Torch
	{
		public override int LabelNumber { get { return 1094957; } } // Candlewood Torch

		[Constructable]
		public CandlewoodTorch()
		{
			LootType = LootType.Blessed;

			Attributes.SpellChanneling = 1;
			Attributes.CastSpeed = -1;
		}

		public CandlewoodTorch( Serial serial )
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

			/*int version = */
			reader.ReadInt();

			if ( Attributes.CastSpeed != -1 )
				Attributes.CastSpeed = -1;
		}
	}
}
