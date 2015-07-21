using System;
using Server;

namespace Server.Items
{
	public class ResonantStaffOfEnlightenment : QuarterStaff
	{
		public override int LabelNumber { get { return 1113757; } } // Resonant Staff of Enlightenment

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		public override bool CanImbue { get { return false; } }

		[Constructable]
		public ResonantStaffOfEnlightenment()
		{
			Hue = 0x3DE;

			WeaponAttributes.HitMagicArrow = 40;
			WeaponAttributes.MageWeapon = 20;
			Attributes.SpellChanneling = 1;
			Attributes.CastSpeed = 1;
			Attributes.BonusInt = 5;
			Attributes.DefendChance = 10;
			Attributes.LowerManaCost = 5;
			Attributes.WeaponSpeed = 20;
			Attributes.WeaponDamage = -40;

			switch ( Utility.Random( 5 ) )
			{
				case 0: AbsorptionAttributes.KineticResonance = 20; break;
				case 1: AbsorptionAttributes.FireResonance = 20; break;
				case 2: AbsorptionAttributes.ColdResonance = 20; break;
				case 3: AbsorptionAttributes.PoisonResonance = 20; break;
				case 4: AbsorptionAttributes.EnergyResonance = 20; break;
			}

			// TODO (SA): Randomize weapon elemental damages
		}

		public ResonantStaffOfEnlightenment( Serial serial )
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