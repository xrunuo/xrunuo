using System;
using Server;

namespace Server.Items
{
	public class StaffOfResonance : StealableGlassStaffArtifact
	{
		public override int ArtifactRarity { get { return 5; } }

		public override int LabelNumber { get { return 1113527; } } // Staff of Resonance

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		[Constructable]
		public StaffOfResonance()
		{
			Hue = 0x83B;
			Weight = 10.0;

			WeaponAttributes.HitHarm = 50;
			WeaponAttributes.MageWeapon = 20;
			Attributes.SpellChanneling = 1;
			Attributes.CastSpeed = 1;
			Attributes.DefendChance = 10;
			Attributes.WeaponSpeed = 20;
			Attributes.WeaponDamage = -40;
			Attributes.LowerManaCost = 5;

			switch ( Utility.Random( 5 ) )
			{
				case 0: AbsorptionAttributes.KineticResonance = 20; break;
				case 1: AbsorptionAttributes.FireResonance = 20; break;
				case 2: AbsorptionAttributes.ColdResonance = 20; break;
				case 3: AbsorptionAttributes.PoisonResonance = 20; break;
				case 4: AbsorptionAttributes.EnergyResonance = 20; break;
			}
		}

		public override void GetDamageTypes( Mobile wielder, out int phys, out int fire, out int cold, out int pois, out int nrgy )
		{
			fire = cold = nrgy = phys = 0;
			pois = 100;
		}

		public StaffOfResonance( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}
}
