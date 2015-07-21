using System;
using Server;
using Server.Engines.Quests;
using Reward = Server.Engines.Quests.BaseReward;

namespace Server.Items
{
	public abstract class BaseDustyBackpack : Backpack
	{
		public abstract int ItemAmount { get; }

		public abstract int MinIntensity { get; }
		public abstract int MaxIntensity { get; }

		public abstract int MinProperties { get; }
		public abstract int MaxProperties { get; }

		public abstract int GoldAmount { get; }

		public BaseDustyBackpack()
		{
			Hue = Utility.RandomMetalHue();

			for ( int i = 0; i < ItemAmount; i++ )
				DropItem( Reward.RandomItem( Utility.RandomMinMax( MinProperties, MaxProperties ), MinIntensity, MaxIntensity ) );

			DropItem( new Gold( GoldAmount + Utility.Random( 1000 ) ) );
			DropItem( TerMurBook.ConstructRandom() );
		}

		public BaseDustyBackpack( Serial serial )
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

	public class DustyAdventurerBackpack : BaseDustyBackpack
	{
		public override int LabelNumber { get { return 1113189; } } // Dusty Adventurer's Backpack

		public override int ItemAmount { get { return 3; } }
		public override int GoldAmount { get { return 2000; } }

		public override int MinIntensity { get { return 10; } }
		public override int MaxIntensity { get { return 70; } }

		public override int MinProperties { get { return 1; } }
		public override int MaxProperties { get { return 3; } }

		[Constructable]
		public DustyAdventurerBackpack()
		{
			int gems = Utility.RandomMinMax( 2, 3 );

			for ( int i = 0; i < gems; i++ )
				DropItem( Loot.RandomGem() );

			DropItem( new MagicalResidue( 20 ) );
		}

		public DustyAdventurerBackpack( Serial serial )
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

	public class DustyExplorerBackpack : BaseDustyBackpack
	{
		public override int LabelNumber { get { return 1113190; } } // Dusty Explorer's Backpack

		public override int ItemAmount { get { return 3; } }
		public override int GoldAmount { get { return 4000; } }

		public override int MinIntensity { get { return 20; } }
		public override int MaxIntensity { get { return 80; } }

		public override int MinProperties { get { return 2; } }
		public override int MaxProperties { get { return 4; } }

		[Constructable]
		public DustyExplorerBackpack()
		{
			int gems = Utility.RandomMinMax( 2, 3 );

			for ( int i = 0; i < gems; i++ )
				DropItem( Loot.RandomRareGem() );

			DropItem( new EnchantedEssence( 10 ) );
		}

		public DustyExplorerBackpack( Serial serial )
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

	public class DustyHunterBackpack : BaseDustyBackpack
	{
		public override int LabelNumber { get { return 1113191; } } // Dusty Hunter's Backpack

		public override int ItemAmount { get { return 5; } }
		public override int GoldAmount { get { return 6000; } }

		public override int MinIntensity { get { return 30; } }
		public override int MaxIntensity { get { return 90; } }

		public override int MinProperties { get { return 3; } }
		public override int MaxProperties { get { return 5; } }

		[Constructable]
		public DustyHunterBackpack()
		{
			int gems = Utility.RandomMinMax( 2, 3 );

			for ( int i = 0; i < gems; i++ )
				DropItem( Loot.RandomRareGem() );

			DropItem( new RelicFragment() );
		}

		public DustyHunterBackpack( Serial serial )
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