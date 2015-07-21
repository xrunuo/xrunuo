using System;
using Server;

namespace Server.Items
{
	public class RoyalGuardInvestigator : Cloak
	{
		public override int LabelNumber { get { return 1112409; } } // Royal Guard Investigator [Replica]

		public override int InitMinHits { get { return 150; } }
		public override int InitMaxHits { get { return 150; } }

		public override bool CanFortify { get { return false; } }

		[Constructable]
		public RoyalGuardInvestigator()
		{
			Hue = 0x486; // TODO (SA): Taken from "Robe of Britannia Ari", verify hue

			SkillBonuses.SetValues( 0, SkillName.Stealth, 20.0 );
		}

		public RoyalGuardInvestigator( Serial serial )
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
