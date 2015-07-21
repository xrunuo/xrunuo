using System;
using Server;

namespace Server.Items
{
	public class SummonersKilt : GargishClothKilt
	{
		public override int LabelNumber { get { return 1113540; } } // Summoner's Kilt

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		[Constructable]
		public SummonersKilt()
		{
			Hue = 1284;

			Attributes.CastingFocus = 2;
			Attributes.BonusMana = 5;
			Attributes.RegenMana = 2;
			Attributes.SpellDamage = 5;
			Attributes.LowerManaCost = 8;
			Attributes.LowerRegCost = 10;

			Resistances.Cold = 15;
			Resistances.Energy = 15;
		}

		public SummonersKilt( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadEncodedInt();
		}
	}
}
