using System;
using Server;

namespace Server.Items
{
	public class LightInTheVoid : StealableGargishTalwarArtifact
	{
		public override int ArtifactRarity { get { return 5; } }

		public override int LabelNumber { get { return 1113521; } } // Light in the Void

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		public override int ItemID { get { return 0x0908; } }

		[Constructable]
		public LightInTheVoid()
		{
			Weight = 10.0;
			Hue = 0x816;

			Slayer = SlayerName.Undead;

			WeaponAttributes.HitLightning = 45;
			WeaponAttributes.HitLowerDefend = 30;

			Attributes.AttackChance = 10;
			Attributes.WeaponSpeed = 20;
			Attributes.WeaponDamage = 35;
			Attributes.BonusStr = 8;
			Attributes.CastingFocus = 2;
			Attributes.CastSpeed = 1;
		}

		public LightInTheVoid( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}
}
