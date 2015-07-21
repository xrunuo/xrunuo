using System;
using Server;

namespace Server.Items
{
	public class WildfireBow : ElvenCompositeLongBow
	{
		public override int LabelNumber { get { return 1075044; } } // Wildfire Bow

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		public override bool CanImbue { get { return false; } }

		[Constructable]
		public WildfireBow()
		{
			Hue = 1161;
			SkillBonuses.SetValues( 0, SkillName.Archery, 10.0 );
			WeaponAttributes.Velocity = 15;
			Resistances.Fire = 25;
		}

		public override void GetDamageTypes( Mobile wielder, out int phys, out int fire, out int cold, out int pois, out int nrgy )
		{
			fire = 100;

			cold = phys = pois = nrgy = 0;
		}

		public WildfireBow( Serial serial )
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