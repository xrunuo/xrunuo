using System;
using Server;

namespace Server.Items
{
	public class Mangler : Broadsword
	{
		public override int LabelNumber { get { return 1114842; } } // Mangler

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		public override bool CanImbue { get { return false; } }

		[Constructable]
		public Mangler()
		{
			Hue = 1191;

			WeaponAttributes.UseBestSkill = 1;
			WeaponAttributes.HitHarm = 50;
			WeaponAttributes.HitLeechMana = 50;
			WeaponAttributes.HitLowerDefend = 30;
			Attributes.WeaponSpeed = 25;
			Attributes.WeaponDamage = 50;
		}

		public Mangler( Serial serial )
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