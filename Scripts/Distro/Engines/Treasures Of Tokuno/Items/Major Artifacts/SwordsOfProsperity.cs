using System;
using Server;

namespace Server.Items
{
	public class SwordsOfProsperity : Daisho
	{
		public override int LabelNumber { get { return 1070963; } } // Swords of Prosperity

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		public override bool CanImbue { get { return false; } }

		[Constructable]
		public SwordsOfProsperity()
		{
			Attributes.SpellChanneling = 1;
			WeaponAttributes.MageWeapon = 30;
			Attributes.Luck = 200;
			Attributes.CastSpeed = 2;
		}

		public SwordsOfProsperity( Serial serial )
			: base( serial )
		{
		}

		public override void GetDamageTypes( Mobile wielder, out int phys, out int fire, out int cold, out int pois, out int nrgy )
		{
			fire = 100;

			pois = phys = cold = nrgy = 0;
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
