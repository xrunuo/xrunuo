using System;
using Server;

namespace Server.Items
{
	public class TitansHammerA : StealableWarHammerArtifact
	{
		public override int LabelNumber { get { return 1060024; } } // Titan's Hammer
		public override int ArtifactRarity { get { return 10; } }

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		[Constructable]
		public TitansHammerA()
		{
			Hue = 0x482;
			WeaponAttributes.HitEnergyArea = 100;
			Attributes.BonusStr = 15;
			Attributes.AttackChance = 15;
			Attributes.WeaponDamage = 50;
		}

		public TitansHammerA( Serial serial )
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
