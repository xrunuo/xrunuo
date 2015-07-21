using System;
using Server;

namespace Server.Items
{
	public class AnimatedLegsOfTheInsaneTinker : PlateLegs
	{
		public override int LabelNumber { get { return 1113760; } } // Animated Legs of the Insane Tinker

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		public override bool CanImbue { get { return false; } }

		[Constructable]
		public AnimatedLegsOfTheInsaneTinker()
		{
			Hue = 0x420;

			Attributes.BonusDex = 5;
			Attributes.RegenStam = 2;
			Attributes.WeaponSpeed = 10;
			Attributes.WeaponDamage = 10;
			ArmorAttributes.LowerStatReq = 50;

			Resistances.Physical = 12;
			Resistances.Fire = 12;
			Resistances.Cold = 5;
			Resistances.Poison = 12;
		}

		public AnimatedLegsOfTheInsaneTinker( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadEncodedInt();
		}
	}
}
