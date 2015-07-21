using System;
using Server;

namespace Server.Items
{
	public class BladeOfTheRighteousA : StealableLongswordArtifact
	{
		public override int LabelNumber { get { return 1061107; } } // Blade of the Righteous
		public override int ArtifactRarity { get { return 10; } }

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		[Constructable]
		public BladeOfTheRighteousA()
		{
			Hue = 0x47E;
			Slayer = SlayerName.Demon;
			WeaponAttributes.HitLeechHits = 50;
			WeaponAttributes.UseBestSkill = 1;
			Attributes.BonusHits = 10;
			Attributes.WeaponDamage = 50;
		}

		public BladeOfTheRighteousA( Serial serial )
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

			if ( Slayer == SlayerName.Unused1 )
			{
				Slayer = SlayerName.Demon;
			}
		}
	}
}
