using System;
using Server;

namespace Server.Items
{
	public class ScepterOfTheChief : Scepter
	{
		public override int LabelNumber { get { return 1072080; } } // Scepter of the Chief

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		[Constructable]
		public ScepterOfTheChief()
		{
			Hue = 1150;

			WeaponAttributes.HitLeechMana = 60;
			Attributes.RegenHits = 2;
			Attributes.WeaponDamage = 45;
			Attributes.ReflectPhysical = 15;
			WeaponAttributes.HitDispel = 100;
		}

		public override void GetDamageTypes( Mobile wielder, out int phys, out int fire, out int cold, out int pois, out int nrgy )
		{
			pois = 100;

			cold = fire = phys = nrgy = 0;
		}

		public ScepterOfTheChief( Serial serial )
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