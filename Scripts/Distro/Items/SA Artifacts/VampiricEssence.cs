using System;
using Server;

namespace Server.Items
{
	public class VampiricEssence : Cutlass
	{
		public override int LabelNumber { get { return 1113873; } } // Vampiric Essence

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		public override bool CanImbue { get { return false; } }

		[Constructable]
		public VampiricEssence()
		{
			Hue = 0x14C;

			WeaponAttributes.BloodDrinker = 1;
			WeaponAttributes.HitLeechHits = 100;
			WeaponAttributes.HitHarm = 50;
			Attributes.WeaponSpeed = 20;
			Attributes.WeaponDamage = 50;
		}

		public override void GetDamageTypes( Mobile wielder, out int phys, out int fire, out int cold, out int pois, out int nrgy )
		{
			fire = pois = phys = nrgy = 0;

			cold = 100;
		}

		public VampiricEssence( Serial serial )
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