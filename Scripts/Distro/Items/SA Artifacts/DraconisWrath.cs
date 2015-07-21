using System;
using Server;

namespace Server.Items
{
	public class DraconisWrath : Katana
	{
		public override int LabelNumber { get { return 1114789; } } // Draconi's Wrath

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		public override bool CanImbue { get { return false; } }

		[Constructable]
		public DraconisWrath()
		{
			Hue = 1177;

			AbsorptionAttributes.FireEater = 20;
			WeaponAttributes.HitFireball = 60;
			WeaponAttributes.UseBestSkill = 1;
			Attributes.AttackChance = 15;
			Attributes.WeaponDamage = 50;
		}

		public DraconisWrath( Serial serial )
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