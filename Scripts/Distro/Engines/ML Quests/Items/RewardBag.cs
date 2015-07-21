using System;
using Server;
using Server.Engines.Quests;
using Reward = Server.Engines.Quests.BaseReward;

namespace Server.Items
{
	public abstract class BaseRewardBag : Bag
	{
		public abstract int ItemAmount { get; }

		public abstract int MinIntensity { get; }
		public abstract int MaxIntensity { get; }

		public abstract int MinProperties { get; }
		public abstract int MaxProperties { get; }

		public BaseRewardBag()
		{
			Hue = Reward.RewardBagHue();

			for ( int i = 0; i < ItemAmount; i++ )
			{
				if ( 0.05 > Utility.RandomDouble() ) // check
					DropItem( new RandomTalisman() );
				else
					DropItem( Reward.RandomItem( Utility.RandomMinMax( MinProperties, MaxProperties ), MinIntensity, MaxIntensity ) );
			}
		}

		public BaseRewardBag( Serial serial )
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

			int version = reader.ReadInt();
		}
	}

	public class SmallTrinketBag : BaseRewardBag
	{
		public override int ItemAmount { get { return 2; } }

		public override int MinIntensity { get { return 10; } }
		public override int MaxIntensity { get { return 50; } }

		public override int MinProperties { get { return 0; } }
		public override int MaxProperties { get { return 2; } }

		[Constructable]
		public SmallTrinketBag()
		{
		}

		public SmallTrinketBag( Serial serial )
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

			int version = reader.ReadInt();
		}
	}

	public class TrinketBag : BaseRewardBag
	{
		public override int ItemAmount { get { return 3; } }

		public override int MinIntensity { get { return 10; } }
		public override int MaxIntensity { get { return 70; } }

		public override int MinProperties { get { return 1; } }
		public override int MaxProperties { get { return 3; } }

		[Constructable]
		public TrinketBag()
		{
		}

		public TrinketBag( Serial serial )
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

			int version = reader.ReadInt();
		}
	}

	public class TreasureBag : BaseRewardBag
	{
		public override int ItemAmount { get { return 4; } }

		public override int MinIntensity { get { return 20; } }
		public override int MaxIntensity { get { return 80; } }

		public override int MinProperties { get { return 2; } }
		public override int MaxProperties { get { return 4; } }

		[Constructable]
		public TreasureBag()
		{
		}

		public TreasureBag( Serial serial )
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

			int version = reader.ReadInt();
		}
	}

	public class LargeTreasureBag : BaseRewardBag
	{
		public override int ItemAmount { get { return 5; } }

		public override int MinIntensity { get { return 30; } }
		public override int MaxIntensity { get { return 90; } }

		public override int MinProperties { get { return 3; } }
		public override int MaxProperties { get { return 6; } }

		[Constructable]
		public LargeTreasureBag()
		{
		}

		public LargeTreasureBag( Serial serial )
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

			int version = reader.ReadInt();
		}
	}
}