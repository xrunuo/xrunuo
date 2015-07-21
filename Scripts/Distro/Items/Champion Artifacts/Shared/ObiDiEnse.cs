using System;
using Server;

namespace Server.Items
{
	public class ObiDiEnse : Obi
	{
		public override int LabelNumber { get { return 1112406; } } // Obi di Ense [Replica]

		public override int InitMinHits { get { return 150; } }
		public override int InitMaxHits { get { return 150; } }

		public override bool CanFortify { get { return false; } }

		[Constructable]
		public ObiDiEnse()
		{
			Attributes.BonusInt = 5;
			Attributes.NightSight = 1;

			SkillBonuses.SetValues( 0, SkillName.Focus, Utility.RandomBool() ? 5.0 : 10.0 );
		}

		public ObiDiEnse( Serial serial )
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
