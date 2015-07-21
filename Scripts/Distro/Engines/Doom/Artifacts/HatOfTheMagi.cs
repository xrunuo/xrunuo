using System;
using Server;

namespace Server.Items
{
	public class HatOfTheMagi : WizardsHat
	{
		public override int LabelNumber { get { return 1061597; } } // Hat of the Magi

		public override int ArtifactRarity { get { return 11; } }

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		[Constructable]
		public HatOfTheMagi()
		{
			Hue = 0x481;

			Attributes.BonusInt = 8;
			Attributes.RegenMana = 4;
			Attributes.SpellDamage = 10;
			Resistances.Poison = 15;
			Resistances.Energy = 15;
		}

		public HatOfTheMagi( Serial serial )
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
