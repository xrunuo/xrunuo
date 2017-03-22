using System;
using Server;

namespace Server.Items
{
	public class FallenMysticsSpellbook : MysticSpellbook
	{
		public override int LabelNumber { get { return 1113867; } } // Fallen Mystic's Spellbook

		[Constructable]
		public FallenMysticsSpellbook()
		{
			Attributes.RegenMana = 1;
			Attributes.SpellDamage = 10;
			Attributes.CastSpeed = 1;
			Attributes.CastRecovery = 1;
			Attributes.LowerManaCost = 5;
			Attributes.LowerRegCost = 10;
			Slayer = SlayerName.Fey;
			SkillBonuses.SetValues( 0, SkillName.Mysticism, 10.0 );
			Hue = 0x378;
		}

		public FallenMysticsSpellbook( Serial serial )
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
