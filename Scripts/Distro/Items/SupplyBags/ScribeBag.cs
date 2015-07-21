using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
	public class ScribeBag : Bag
	{
		[Constructable]
		public ScribeBag()
			: this( 1 )
		{
			Movable = true;
			Hue = 0x105;
			Name = "a Scribe Kit";
		}

		[Constructable]
		public ScribeBag( int amount )
		{
			DropItem( new BagOfReagents( 5000 ) );
			DropItem( new BlankScroll( 500 ) );
			// DropItem( new SpellBook() );
		}


		public ScribeBag( Serial serial )
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