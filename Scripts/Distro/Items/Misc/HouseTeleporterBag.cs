using System;
using Server;
using Server.Engines.Housing.Items;

namespace Server.Items
{
	public class HouseTeleporterBag : Bag
	{
		public override int LabelNumber { get { return 1113857; } } // house teleporter

		[Constructable]
		public HouseTeleporterBag()
		{
			Hue = 0x538;

			int itemId = Utility.RandomMinMax( 0x40AF, 0x40BB );

			var source = new HouseTeleporter( itemId ) { Movable = true };
			var target = new HouseTeleporter( itemId ) { Movable = true };

			source.Target = target;
			target.Target = source;

			DropItem( source );
			DropItem( target );

			DropItem( new HouseTeleporterInstructions() );
		}

		public HouseTeleporterBag( Serial serial )
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

	public class HouseTeleporterInstructions : Item
	{
		public override int LabelNumber { get { return 1115122; } } // Care Instructions

		[Constructable]
		public HouseTeleporterInstructions()
			: base( 0xEBD )
		{
			Hue = 0xC3;
			Weight = 1.0;
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			list.Add( 1115123 ); // Congratulations on becoming the<br> owner of your very own house<br> teleporter set!
			list.Add( 1115124 ); // To use them, lock one down in your<br> home then lock the other down in<br> the home of a trusted friend.
		}

		public HouseTeleporterInstructions( Serial serial )
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
