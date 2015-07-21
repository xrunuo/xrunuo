using System;
using Server;

namespace Server.Items
{
	public class ArcanicRuneStone : Item, ICommodity
	{
		public override int LabelNumber { get { return 1113352; } } // arcanic rune stone

		[Constructable]
		public ArcanicRuneStone()
			: this( 1 )
		{
		}

		[Constructable]
		public ArcanicRuneStone( int amount )
			: base( 0x573C )
		{
			Weight = 0.1;
			Stackable = true;
			Amount = amount;
		}

		public ArcanicRuneStone( Serial serial )
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
