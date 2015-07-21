using System;
using Server;

namespace Server.Items
{
	public class CrownOfTalKeesh : Bandana
	{
		public override int LabelNumber { get { return 1094903; } } // Crown of Tal'Keesh [Replica]

		public override int InitMinHits { get { return 150; } }
		public override int InitMaxHits { get { return 150; } }

		public override bool CanFortify { get { return false; } }

		[Constructable]
		public CrownOfTalKeesh()
		{
			Hue = 0x4F2;

			Attributes.BonusInt = 8;
			Attributes.RegenMana = 4;
			Attributes.SpellDamage = 10;

			Resistances.Fire = 2;
			Resistances.Cold = 4;
			Resistances.Poison = 12;
			Resistances.Energy = 12;
		}

		public CrownOfTalKeesh( Serial serial )
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

			int version = reader.ReadInt();

			if ( version < 1 )
			{
				Resistances.Fire = 2;
				Resistances.Cold = 4;
				Resistances.Poison = 12;
				Resistances.Energy = 12;
			}
		}
	}
}
