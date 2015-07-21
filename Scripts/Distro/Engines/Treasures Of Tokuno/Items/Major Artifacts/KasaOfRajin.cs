using System;
using Server;

namespace Server.Items
{
	public class KasaOfRajin : Kasa
	{
		public override int LabelNumber { get { return 1070969; } } // Kasa of the Raj-in

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		public override bool CanImbue { get { return false; } }

		public override bool CanBeBlessed { get { return false; } }

		[Constructable]
		public KasaOfRajin()
		{
			Attributes.SpellDamage = 12;

			Resistances.Physical = 12;
			Resistances.Fire = 12;
			Resistances.Cold = 12;
			Resistances.Poison = 12;
			Resistances.Energy = 12;
		}

		public KasaOfRajin( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 2 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			if ( version < 1 )
				LootType = LootType.Regular;

			if ( version < 2 )
			{
				Resistances.Physical = 12;
				Resistances.Fire = 12;
				Resistances.Cold = 12;
				Resistances.Poison = 12;
				Resistances.Energy = 12;
			}
		}
	}
}
