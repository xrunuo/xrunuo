using System;
using Server;

namespace Server.Items
{
	public class EvilOrcHelm : OrcHelm
	{
		public override int LabelNumber { get { return 1062021; } } // an evil orc helm

		[Constructable]
		public EvilOrcHelm()
		{
			Hue = 0x96D; // todo: verify
			Attributes.BonusStr = 10;
			Attributes.BonusDex = -10;
			Attributes.BonusInt = -10;
		}

		public override bool OnEquip( Mobile from )
		{
			from.CheckStatTimers();

			Misc.Titles.AwardKarma( from, -20, true ); // todo: verify

			return base.OnEquip( from );
		}

		public EvilOrcHelm( Serial serial )
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
				Attributes.BonusStr = 10;
				Attributes.BonusDex = -10;
				Attributes.BonusInt = -10;
			}
		}
	}
}
