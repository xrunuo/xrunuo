using System;
using Server;

namespace Server.Items
{
	public class IcySpellblade : ElvenSpellBlade
	{
		public override int LabelNumber { get { return 1073514; } } //Icy Spellblade

		[Constructable]
		public IcySpellblade()
		{
			Resistances.Cold = 5;
		}


		public IcySpellblade( Serial serial )
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