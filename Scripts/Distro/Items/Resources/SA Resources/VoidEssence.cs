using System;
using Server;

namespace Server.Items
{
	public class VoidEssence : Item
	{
		public override int LabelNumber { get { return 1112327; } } // Void Essence

		[Constructable]
		public VoidEssence()
			: this( 1 )
		{
		}

		[Constructable]
		public VoidEssence( int amount )
			: base( 0x4007 )
		{
			Weight = 0.1;
			Stackable = true;
			Hue = 2101;
			Amount = amount;
		}

		public VoidEssence( Serial serial )
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
