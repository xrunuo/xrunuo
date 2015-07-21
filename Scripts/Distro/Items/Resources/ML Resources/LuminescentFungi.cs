using System;

namespace Server.Items
{
	public class LuminescentFungi : Item, ICommodity
	{
		[Constructable]
		public LuminescentFungi()
			: this( 1 )
		{
		}

		[Constructable]
		public LuminescentFungi( int amount )
			: base( 0x3191 )
		{
			Stackable = true;
			Weight = 1;
			Amount = amount;
		}

		public LuminescentFungi( Serial serial )
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