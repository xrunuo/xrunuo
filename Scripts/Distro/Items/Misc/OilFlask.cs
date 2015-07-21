using System;
using Server;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Items
{
	public class OilFlask : Item
	{
		public override int LabelNumber { get { return 1027192; } } // oil flask

		[Constructable]
		public OilFlask()
			: this( 1 )
		{
		}

		[Constructable]
		public OilFlask( int amount )
			: base( 0x1C18 )
		{
			Weight = 1.0;
			Stackable = true;
			Amount = amount;
		}

		public OilFlask( Serial serial )
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