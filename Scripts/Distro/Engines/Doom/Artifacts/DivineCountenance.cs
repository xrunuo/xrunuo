using System;
using Server;

namespace Server.Items
{
	public class DivineCountenance : HornedTribalMask
	{
		public override int LabelNumber { get { return 1061289; } } // Divine Countenance

		public override int ArtifactRarity { get { return 11; } }

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		[Constructable]
		public DivineCountenance()
		{
			Hue = 0x482;

			Attributes.BonusInt = 8;
			Attributes.RegenMana = 2;
			Attributes.ReflectPhysical = 15;
			Attributes.LowerManaCost = 8;
			Resistances.Energy = 20;
		}

		public DivineCountenance( Serial serial )
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