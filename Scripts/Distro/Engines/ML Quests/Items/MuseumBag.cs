using System;
using Server;
using Server.Engines.Quests;
using Reward = Server.Engines.Quests.BaseReward;

namespace Server.Items
{
	public abstract class BaseMuseumBag : Bag
	{
		public abstract int GoldAmount { get; }

		public BaseMuseumBag()
		{
			Hue = Reward.RewardBagHue();

			DropItem( new Gold( GoldAmount + Utility.Random( 1000 ) ) );
			DropItem( TerMurBook.ConstructRandom() );
		}

		public BaseMuseumBag( Serial serial )
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

	public class MeagerMuseumBag : BaseMuseumBag
	{
		public override int LabelNumber { get { return 1112993; } } // Meager Museum Bag

		public override int GoldAmount { get { return 3000; } }

		[Constructable]
		public MeagerMuseumBag()
		{
			int gems = Utility.RandomMinMax( 4, 5 );

			for ( int i = 0; i < gems; i++ )
				DropItem( Loot.RandomGem() );
		}

		public MeagerMuseumBag( Serial serial )
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

	public class DustyMuseumBag : BaseMuseumBag
	{
		public override int LabelNumber { get { return 1112994; } } // Dusty Museum Bag

		public override int GoldAmount { get { return 6000; } }

		[Constructable]
		public DustyMuseumBag()
		{
			int gems = Utility.RandomMinMax( 7, 8 );

			for ( int i = 0; i < gems; i++ )
				DropItem( Loot.RandomGem() );

			int rareGems = Utility.RandomMinMax( 2, 3 );

			for ( int i = 0; i < rareGems; i++ )
				DropItem( Loot.RandomRareGem() );
		}

		public DustyMuseumBag( Serial serial )
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

	public class BulgingMuseumBag : BaseMuseumBag
	{
		public override int LabelNumber { get { return 1112995; } } // Bulging Museum Bag

		public override int GoldAmount { get { return 10000; } }

		[Constructable]
		public BulgingMuseumBag()
		{
			int gems = Utility.RandomMinMax( 10, 11 );

			for ( int i = 0; i < gems; i++ )
				DropItem( Loot.RandomGem() );

			int rareGems = Utility.RandomMinMax( 4, 5 );

			for ( int i = 0; i < rareGems; i++ )
				DropItem( Loot.RandomRareGem() );
		}

		public BulgingMuseumBag( Serial serial )
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