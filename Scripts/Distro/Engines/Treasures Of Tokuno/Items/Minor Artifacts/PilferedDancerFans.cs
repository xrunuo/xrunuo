using System;
using Server;

namespace Server.Items
{
	public class PilferedDancerFans : Tessen
	{
		public override int LabelNumber { get { return 1070916; } } // Pilfered Dancer Fans

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		public override bool CanImbue { get { return false; } }

		[Constructable]
		public PilferedDancerFans()
		{
			Attributes.SpellChanneling = 1;
			Attributes.CastSpeed = 1;
			Attributes.DefendChance = 5;
			Attributes.CastRecovery = 2;
			Attributes.WeaponSpeed = 20;
			Attributes.WeaponDamage = 20;
		}

		public PilferedDancerFans( Serial serial )
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
		}
	}
}
