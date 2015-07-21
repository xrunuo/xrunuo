using System;
using Server.Network;
using Server.Spells;

namespace Server.Items
{
	public class SpellweavingSpellbook : Spellbook
	{
		public override SpellbookType SpellbookType { get { return SpellbookType.Arcanist; } }
		public override int BookOffset { get { return 600; } }
		public override int BookCount { get { return 16; } }

		[Constructable]
		public SpellweavingSpellbook()
			: this( (ulong) 0 )
		{
		}

		[Constructable]
		public SpellweavingSpellbook( ulong content )
			: base( content, 0x2D50 )
		{
			Hue = 2210;
			Layer = Layer.OneHanded;
		}

		public SpellweavingSpellbook( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
			Layer = Layer.OneHanded;
		}
	}
}
