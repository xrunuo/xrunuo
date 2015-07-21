using System;
using Server;

namespace Server.Items
{
	public class CavalrysFolly : BladedStaff
	{
		public override int LabelNumber { get { return 1115446; } } // Cavalry's Folly

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		public override bool CanImbue { get { return false; } }

		[Constructable]
		public CavalrysFolly()
		{
			Hue = 0xCE;

			WeaponAttributes.HitFireball = 40;
			WeaponAttributes.HitLowerDefend = 40;
			Attributes.RegenHits = 2;
			Attributes.AttackChance = 10;
			Attributes.WeaponSpeed = 35;
			Attributes.WeaponDamage = 45;
		}

		public CavalrysFolly( Serial serial )
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