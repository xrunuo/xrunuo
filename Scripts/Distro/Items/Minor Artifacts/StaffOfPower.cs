using System;
using Server;

namespace Server.Items
{
	public class StaffOfPower : BlackStaff
	{
		public override int LabelNumber { get { return 1070692; } } // Staff of Power

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		public override bool CanImbue { get { return false; } }

		[Constructable]
		public StaffOfPower()
		{
			Hue = Utility.RandomBool() ? 0x4F2 : 1154;
			WeaponAttributes.MageWeapon = 15;
			Attributes.SpellChanneling = 1;
			Attributes.CastSpeed = 1;
			Attributes.SpellDamage = 5;
			Attributes.CastRecovery = 2;
			Attributes.LowerManaCost = 5;
		}

		public StaffOfPower( Serial serial )
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