using System;

namespace Server.Items
{
	public class FruitBasketA : StealableFoodArtifact
	{
		public override int ArtifactRarity { get { return 5; } }

		[Constructable]
		public FruitBasketA()
			: base( 1, 0x993 )
		{
			FillFactor = 5;
		}

		public FruitBasketA( Serial serial )
			: base( serial )
		{
		}

		public override bool Eat( Mobile from )
		{
			if ( !Movable )
			{
				return false;
			}

			if ( !base.Eat( from ) )
			{
				return false;
			}

			from.AddToBackpack( new Basket() );
			return true;
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
