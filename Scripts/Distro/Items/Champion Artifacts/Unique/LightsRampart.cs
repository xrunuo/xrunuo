using System;
using Server;

namespace Server.Items
{
	public class LightsRampart : MetalShield
	{
		public override int LabelNumber { get { return 1112399; } } // Light's Rampart [Replica]

		public override int InitMinHits { get { return 150; } }
		public override int InitMaxHits { get { return 150; } }

		public override bool CanFortify { get { return false; } }

		[Constructable]
		public LightsRampart()
		{
			Hue = 1136;

			Attributes.DefendChance = 20;
			Attributes.SpellChanneling = 1;
			Attributes.CastSpeed = 1;
			Resistances.Physical = 4;
			Resistances.Fire = 4;
			Resistances.Cold = 13;
			Resistances.Poison = 3;
			Resistances.Energy = 3;
		}

		public LightsRampart( Serial serial )
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
