using System;
using Server;

namespace Server.Items
{
	public class BlazeOfDeath : Halberd
	{
		public override int LabelNumber { get { return 1063486; } } // Blaze of Death

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		public override bool CanImbue { get { return false; } }

		[Constructable]
		public BlazeOfDeath()
		{
			Hue = 0x501;
			WeaponAttributes.HitFireArea = 50;
			WeaponAttributes.HitFireball = 50;
			Attributes.WeaponSpeed = 25;
			Attributes.WeaponDamage = 35;
			Resistances.Fire = 10;
			WeaponAttributes.LowerStatReq = 100;
		}

		public override void GetDamageTypes( Mobile wielder, out int phys, out int fire, out int cold, out int pois, out int nrgy )
		{
			fire = 50;
			phys = 50;

			cold = pois = nrgy = 0;
		}

		public BlazeOfDeath( Serial serial )
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