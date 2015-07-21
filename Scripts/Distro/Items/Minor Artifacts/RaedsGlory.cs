using System;
using Server;

namespace Server.Items
{
	public class RaedsGlory : WarCleaver
	{
		public override int LabelNumber { get { return 1075036; } } // Raed's Glory

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		public override bool CanImbue { get { return false; } }

		[Constructable]
		public RaedsGlory()
		{
			Hue = 486;
			Attributes.WeaponSpeed = 20;
			Attributes.SpellChanneling = 1;
			Attributes.CastSpeed = 1;
			Attributes.BonusMana = 8;
			WeaponAttributes.HitLeechHits = 40;
			Weight = 10.0;
		}

		public RaedsGlory( Serial serial )
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

			Weight = 10.0;
		}
	}
}