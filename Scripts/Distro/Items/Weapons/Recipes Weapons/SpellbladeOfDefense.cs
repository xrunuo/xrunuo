using System;
using Server;

namespace Server.Items
{
	public class SpellbladeOfDefense : ElvenSpellBlade
	{
		public override int LabelNumber { get { return 1073516; } } // Spellblade of Defense

		[Constructable]
		public SpellbladeOfDefense()
		{
			Attributes.DefendChance = 5;
		}


		public SpellbladeOfDefense( Serial serial )
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

			/*int version = */reader.ReadInt();
		}
	}
}