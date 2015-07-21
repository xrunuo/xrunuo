using System;
using Server;

namespace Server.Items
{
	public class FierySpellblade : ElvenSpellBlade
	{
		public override int LabelNumber { get { return 1020526; } } // Fiery Spellblade

		[Constructable]
		public FierySpellblade()
		{
			Resistances.Fire = 5;
		}


		public FierySpellblade( Serial serial )
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