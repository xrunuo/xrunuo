using System;
using Server;

namespace Server.Items
{
	public class GuardianAxe : OrnateAxe
	{
		public override int LabelNumber { get { return 1073545; } } // Guardian Axe

		[Constructable]
		public GuardianAxe()
		{
			Attributes.BonusHits = 4;
			Attributes.RegenHits = 1;
		}


		public GuardianAxe( Serial serial )
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