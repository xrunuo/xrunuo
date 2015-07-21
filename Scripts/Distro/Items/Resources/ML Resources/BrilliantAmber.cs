using System;

namespace Server.Items
{
	public class BrilliantAmber : Item, ICommodity
	{
		public override int LabelNumber { get { return 1032697; } } // Blue Diamond

		[Constructable]
		public BrilliantAmber()
			: this( 1 )
		{
		}

		[Constructable]
		public BrilliantAmber( int amount )
			: base( 0x3199 )
		{
			Stackable = true;
			Weight = 1;
			Amount = amount;
		}

		public BrilliantAmber( Serial serial )
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