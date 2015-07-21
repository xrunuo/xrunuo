using System;
using Server;

namespace Server.Items
{
	public class BladeOfBattle : Shortblade
	{
		public override int LabelNumber { get { return 1113525; } } // Blade of Battle

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		public override bool CanImbue { get { return false; } }

		[Constructable]
		public BladeOfBattle()
		{
			Hue = 1150;

			WeaponAttributes.HitLowerDefend = 40;
			WeaponAttributes.BattleLust = 1;
			Attributes.AttackChance = 15;
			Attributes.DefendChance = 10;
			Attributes.WeaponSpeed = 25;
			Attributes.WeaponDamage = 50;
		}

		public BladeOfBattle( Serial serial )
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