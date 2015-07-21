using System;

namespace Server.Items
{
	public class PerfectEmerald : Item, ICommodity
	{
		public override int LabelNumber { get { return 1032692; } } // Perfect Emerald

		[Constructable]
		public PerfectEmerald()
			: this( 1 )
		{
		}

		[Constructable]
		public PerfectEmerald( int amount )
			: base( 0x3194 )
		{
			Stackable = true;
			Weight = 1;
			Amount = amount;
		}

		public PerfectEmerald( Serial serial )
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