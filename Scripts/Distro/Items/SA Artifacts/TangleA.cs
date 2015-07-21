using System;
using Server;

namespace Server.Items
{
	public class TangleA : HalfApron
	{
		public override int LabelNumber { get { return 1114784; } } // Tangle

		[Constructable]
		public TangleA()
			: base( 1159 )
		{
			Weight = 2.0;

			Attributes.BonusInt = 10;
			Attributes.RegenMana = 2;
			Attributes.DefendChance = 5;
		}

		public TangleA( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}
}