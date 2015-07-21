using System;
using Server;

namespace Server.Items
{
	public class TheMostKnowledgePerson : BaseOuterTorso
	{
		public override int LabelNumber { get { return 1094893; } } // The Most Knowledge Person [Replica]

		public override int InitMinHits { get { return 150; } }
		public override int InitMaxHits { get { return 150; } }

		public override bool CanFortify { get { return false; } }
		public override bool CanBeBlessed { get { return false; } }

		public override int StrengthReq { get { return 0; } }

		[Constructable]
		public TheMostKnowledgePerson()
			: base( 0x2684 )
		{
			Hue = 0x242;
			Weight = 1.0;

			Attributes.BonusHits = Utility.RandomMinMax( 3, 5 );
		}

		public TheMostKnowledgePerson( Serial serial )
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
