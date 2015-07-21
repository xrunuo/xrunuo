using System;
using Server;

namespace Server.Items
{
	public class LegacyOfDespair : DreadSword
	{
		public override int LabelNumber { get { return 1113519; } } // Legacy of Despair

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		public override bool CanImbue { get { return false; } }

		[Constructable]
		public LegacyOfDespair()
		{
			Hue = 900;

			WeaponAttributes.HitCurse = 10;
			WeaponAttributes.HitLowerDefend = 50;
			WeaponAttributes.HitLowerAttack = 50;
			Attributes.WeaponSpeed = 30;
			Attributes.WeaponDamage = 60;
		}

		public override void GetDamageTypes( Mobile wielder, out int phys, out int fire, out int cold, out int pois, out int nrgy )
		{
			phys = fire = nrgy = 0;
			cold = 75;
			pois = 25;
		}

		public LegacyOfDespair( Serial serial )
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