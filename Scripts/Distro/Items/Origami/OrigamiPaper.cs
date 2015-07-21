using System;
using Server;
using Server.Network;

namespace Server.Items
{
	public class OrigamiPaper : Item
	{
		public override int LabelNumber { get { return 1030288; } } // origami paper

		[Constructable]
		public OrigamiPaper()
			: base( 0x2830 )
		{
		}

		public OrigamiPaper( Serial serial )
			: base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( !IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
			}
			else
			{
				switch ( Utility.Random( 6 ) )
				{
					case 0:
						from.AddToBackpack( new ButterflyOrigami() );
						break;
					case 1:
						from.AddToBackpack( new SwanOrigami() );
						break;
					case 2:
						from.AddToBackpack( new FrogOrigami() );
						break;
					case 3:
						from.AddToBackpack( new ShapeOrigami() );
						break;
					case 4:
						from.AddToBackpack( new BirdOrigami() );
						break;
					case 5:
						from.AddToBackpack( new FishOrigami() );
						break;
				}

				from.SendLocalizedMessage( 1070822 ); // You fold the paper into an interesting shape.			

				this.Delete();
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}
}
