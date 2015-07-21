using System;
using Server;

namespace Server.Items
{
	public class AxeOfAbandon : LargeBattleAxe
	{
		public override int LabelNumber { get { return 1113863; } } // Axe of Abandon

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		public override bool CanImbue { get { return false; } }

		[Constructable]
		public AxeOfAbandon()
		{
			Hue = 0x164;

			WeaponAttributes.HitLowerDefend = 40;
			WeaponAttributes.BattleLust = 1;
			Attributes.AttackChance = 15;
			Attributes.DefendChance = 10;
			Attributes.CastSpeed = 1;
			Attributes.WeaponSpeed = 30;
			Attributes.WeaponDamage = 50;
		}

		public AxeOfAbandon( Serial serial )
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