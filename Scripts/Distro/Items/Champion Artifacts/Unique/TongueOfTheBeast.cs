using System;
using Server;

namespace Server.Items
{
	public class TongueOfTheBeast : WoodenKiteShield
	{
		public override int LabelNumber { get { return 1112405; } } // Tongue of the Beast [Replica]

		public override int InitMinHits { get { return 150; } }
		public override int InitMaxHits { get { return 150; } }

		public override bool CanFortify { get { return false; } }

		[Constructable]
		public TongueOfTheBeast()
		{
			Hue = 1109;

			Attributes.SpellChanneling = 1;
			Attributes.CastSpeed = 1;
			Attributes.RegenStam = 3;
			Attributes.RegenMana = 3;
			Resistances.Physical = 10;
			Resistances.Energy = 4;
		}

		public TongueOfTheBeast( Serial serial )
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
