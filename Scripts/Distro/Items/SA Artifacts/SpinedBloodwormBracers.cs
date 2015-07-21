using System;
using Server;

namespace Server.Items
{
	public class SpinedBloodwormBracers : GargishClothArms
	{
		public override int LabelNumber { get { return 1113865; } } // Spined Bloodworm Bracers

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		[Constructable]
		public SpinedBloodwormBracers()
		{
			Hue = 438;

			AbsorptionAttributes.KineticEater = 10;
			Attributes.RegenHits = 2;
			Attributes.RegenHits = 2;
			Attributes.ReflectPhysical = 30;
			Attributes.WeaponDamage = 10;
			Resistances.Physical = 6;
			Resistances.Fire = 3;
			Resistances.Cold = 9;
			Resistances.Poison = 9;
			Resistances.Energy = 4;
		}

		public SpinedBloodwormBracers( Serial serial )
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