using System;
using Server;

namespace Server.Items
{
	public class KatrinasCrook : ShepherdsCrook
	{
		public override int ArtifactRarity { get { return 11; } }

		public override int LabelNumber { get { return 1079789; } } // Katrina's Crook

		public override int InitMinHits { get { return 120; } }
		public override int InitMaxHits { get { return 120; } }

		public override bool CanImbue { get { return false; } }

		[Constructable]
		public KatrinasCrook()
		{
			WeaponAttributes.HitLeechStam = 40;
			WeaponAttributes.HitLeechMana = 40;
			WeaponAttributes.HitLeechHits = 40;
			Attributes.WeaponDamage = 60;
			Resistances.Physical = 15;
		}

		public KatrinasCrook( Serial serial )
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
