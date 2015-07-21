using System;
using Server;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Items
{
	public class SweatOfParoxysmus : Item
	{
		public override int LabelNumber { get { return 1072081; } } // Sweat of Paroxysmus

		[Constructable]
		public SweatOfParoxysmus()
			: base( 0xF01 )
		{
			Weight = 1.0;
		}

		public SweatOfParoxysmus( Serial serial )
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