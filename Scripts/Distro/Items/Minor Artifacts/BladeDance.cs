using System;
using Server;

namespace Server.Items
{
	public class BladeDance : RuneBlade
	{
		public override int LabelNumber { get { return 1075033; } } // Blade Dance

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		public override bool CanImbue { get { return false; } }

		[Constructable]
		public BladeDance()
		{
			Hue = 1644;
			WeaponAttributes.HitLeechMana = 20;
			Attributes.WeaponDamage = 30;
			Attributes.BonusMana = 8;
			WeaponAttributes.UseBestSkill = 1;
			Attributes.SpellChanneling = 1;
			Attributes.CastSpeed = 1;
		}

		public BladeDance( Serial serial )
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