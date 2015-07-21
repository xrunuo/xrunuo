using System;
using Server;

namespace Server.Items
{
	public class TrueSpellblade : ElvenSpellBlade
	{
		public override int LabelNumber { get { return 1073513; } } //True Spellblade

		[Constructable]
		public TrueSpellblade()
		{
			Attributes.SpellChanneling = 1;
		}

		public TrueSpellblade( Serial serial )
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