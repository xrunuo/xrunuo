using System;
using Server;

namespace Server.Items
{
	public class ClaininsSpellbook : Spellbook, ICollectionItem
	{
		public override int LabelNumber { get { return 1073262; } } // Clainin's Spellbook - Museum of Vesper Replica

		[Constructable]
		public ClaininsSpellbook()
		{
			Hue = 2125;
			Attributes.RegenMana = 3;
			Attributes.LowerRegCost = 15;
			Attributes.Luck = 80;
		}

		public ClaininsSpellbook( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();

			if ( Attributes.SpellChanneling != 0 )
				Attributes.SpellChanneling = 0;
		}
	}
}