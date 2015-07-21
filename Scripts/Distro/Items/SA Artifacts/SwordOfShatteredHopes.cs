using System;
using Server;

namespace Server.Items
{
	public class SwordOfShatteredHopes : GlassSword
	{
		public override int LabelNumber { get { return 1112770; } } // Sword of Shattered Hopes

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		public override int ArtifactRarity { get { return 10; } }

		public override bool CanImbue { get { return false; } }

		[Constructable]
		public SwordOfShatteredHopes()
		{
			WeaponAttributes.SplinteringWeapon = 20;
			WeaponAttributes.HitDispel = 25;
			Attributes.WeaponSpeed = 30;
			Attributes.WeaponDamage = 50;
			Resistances.Fire = 15;
		}

		public SwordOfShatteredHopes( Serial serial )
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