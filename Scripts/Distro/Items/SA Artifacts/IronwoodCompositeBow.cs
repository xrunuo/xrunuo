using System;
using Server;

namespace Server.Items
{
	public class IronwoodCompositeBow : CompositeBow
	{
		public override int LabelNumber { get { return 1113759; } } // Ironwood Composite Bow

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		public override bool CanImbue { get { return false; } }

		[Constructable]
		public IronwoodCompositeBow()
		{
			Hue = 0x378;

			WeaponAttributes.Velocity = 30;
			WeaponAttributes.HitLowerDefend = 30;
			WeaponAttributes.HitFireball = 40;
			Attributes.BonusDex = 5;
			Attributes.WeaponSpeed = 25;
			Attributes.WeaponDamage = 45;
			Slayer = SlayerName.Fey;
		}

		public IronwoodCompositeBow( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadEncodedInt();
		}
	}
}
