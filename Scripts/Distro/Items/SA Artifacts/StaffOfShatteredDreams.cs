using System;
using Server;

namespace Server.Items
{
	public class StaffOfShatteredDreams : GlassStaff
	{
		public override int LabelNumber { get { return 1112771; } } // Staff of Shattered Dreams

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		public override int ArtifactRarity { get { return 11; } }

		[Constructable]
		public StaffOfShatteredDreams()
		{
			WeaponAttributes.SplinteringWeapon = 20;
			WeaponAttributes.HitDispel = 25;
			Attributes.SpellChanneling = 1;
			Attributes.WeaponDamage = 50;
			Resistances.Fire = 15;
		}

		public StaffOfShatteredDreams( Serial serial )
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