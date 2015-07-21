using System;
using Server;

namespace Server.Items
{
	public class ValkyriesGlaive : StealableSoulGlaiveArtifact
	{
		public override int ArtifactRarity { get { return 5; } }

		public override int LabelNumber { get { return 1113531; } } // Valkyrie's Glaive

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		public override int ItemID { get { return 0x090A; } }

		[Constructable]
		public ValkyriesGlaive()
		{
			Weight = 10.0;
			Hue = 0x4B2;

			Slayer = SlayerName.Undead;

			WeaponAttributes.HitFireball = 40;

			Attributes.SpellChanneling = 1;
			Attributes.CastSpeed = 1;
			Attributes.WeaponSpeed = 20;
			Attributes.WeaponDamage = 20;
			Attributes.BonusStr = 5;
		}

		public ValkyriesGlaive( Serial serial )
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
