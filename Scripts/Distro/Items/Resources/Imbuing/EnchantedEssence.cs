using System;
using Server;

namespace Server.Items
{
	public class EnchantedEssence : Item, ICommodity
	{
		[Constructable]
		public EnchantedEssence()
			: this( 1 )
		{
		}

		[Constructable]
		public EnchantedEssence( int amount )
			: base( 0x2DB2 )
		{
			Stackable = true;
			Weight = 1.0;
			Amount = amount;
		}

		public EnchantedEssence( Serial serial )
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