using System;
using Server;

namespace Server.Items
{
	[TypeAlias( "Server.Items.ObsidianEarringsArmor" )]
	public class ObsidianEarrings : GargishEarrings
	{
		public override int LabelNumber { get { return 1113820; } } // Obsidian Earrings

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		public override bool CanImbue { get { return false; } }

		[Constructable]
		public ObsidianEarrings()
		{
			Hue = 1156;

			Attributes.CastingFocus = 4;
			Attributes.BonusMana = 8;
			Attributes.RegenStam = 2;
			Attributes.RegenMana = 2;
			Attributes.SpellDamage = 8;
			Resistances.Physical = 3;
			Resistances.Fire = 8;
			Resistances.Cold = 8;
			Resistances.Poison = 1;
			Resistances.Energy = 10;
		}

		public ObsidianEarrings( Serial serial )
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
