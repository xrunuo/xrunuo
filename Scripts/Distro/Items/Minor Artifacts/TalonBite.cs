using System;
using Server;

namespace Server.Items
{
	public class TalonBite : OrnateAxe
	{
		public override int LabelNumber { get { return 1075029; } } // Talon Bite

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		public override bool CanImbue { get { return false; } }

		[Constructable]
		public TalonBite()
		{
			Hue = 1150;
			Attributes.WeaponSpeed = 20;
			WeaponAttributes.UseBestSkill = 1;
			WeaponAttributes.HitHarm = 33;
			Attributes.WeaponDamage = 35;
			Attributes.BonusDex = 8;
			SkillBonuses.SetValues( 0, SkillName.Tactics, 10.0 );
			Weight = 12.0;
		}

		public TalonBite( Serial serial )
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

			Weight = 12.0;
		}
	}
}