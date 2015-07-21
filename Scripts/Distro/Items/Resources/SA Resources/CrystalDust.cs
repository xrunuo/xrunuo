using System;
using Server;

namespace Server.Items
{
	public class CrystalDust : Item
	{
		public override int LabelNumber { get { return 1112328; } } // crystal dust

		[Constructable]
		public CrystalDust()
			: this( 1 )
		{
		}

		[Constructable]
		public CrystalDust( int amount )
			: base( 0x4009 )
		{
			Stackable = true;
			Amount = amount;
			Hue = 0x835;
			Weight = 1.0;
		}

		public CrystalDust( Serial serial )
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
