using System;
using Server;

namespace Server.Items
{
	public class HuntersHeaddress : DeerMask
	{
		public override int LabelNumber { get { return 1061595; } } // Hunter's Headdress

		public override int ArtifactRarity { get { return 11; } }

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		[Constructable]
		public HuntersHeaddress()
		{
			Hue = 0x594;

			SkillBonuses.SetValues( 0, SkillName.Archery, 20 );

			Attributes.BonusDex = 8;
			Attributes.NightSight = 1;
			Attributes.AttackChance = 15;

			Resistances.Cold = 15;
		}

		public HuntersHeaddress( Serial serial )
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