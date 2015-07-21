using System;
using Server;

namespace Server.Items
{
	public class MagicalResidue : Item, ICommodity
	{
		[Constructable]
		public MagicalResidue()
			: this( 1 )
		{
		}

		[Constructable]
		public MagicalResidue( int amount )
			: base( 0x2DB1 )
		{
			Stackable = true;
			Weight = 1.0;
			Amount = amount;
		}

		public MagicalResidue( Serial serial )
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