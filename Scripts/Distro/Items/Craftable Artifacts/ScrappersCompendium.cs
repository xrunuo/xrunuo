using System;
using Server;

namespace Server.Items
{
	public class ScrappersCompendium : Spellbook
	{
		public override int LabelNumber { get { return 1072940; } }

		[Constructable]
		public ScrappersCompendium()
		{
			Attributes.SpellDamage = 25;
			Attributes.LowerManaCost = 10;
			Attributes.CastRecovery = 1;
			Attributes.CastSpeed = 1;
			Hue = 1172;
		}

		public ScrappersCompendium( Serial serial )
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
