using System;
using Server;

namespace Server.Items
{
	public class AxesOfFury : DualShortAxes
	{
		public override int LabelNumber { get { return 1113517; } } // Axes of Fury

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		public override bool CanImbue { get { return false; } }

		[Constructable]
		public AxesOfFury()
		{
			Hue = 238;

			WeaponAttributes.HitFireball = 45;
			WeaponAttributes.HitLowerDefend = 40;
			Attributes.BonusDex = 5;
			Attributes.AttackChance = 20;
			Attributes.DefendChance = -15;
			Attributes.WeaponSpeed = 30;
			Attributes.WeaponDamage = 45;
		}

		public AxesOfFury( Serial serial )
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