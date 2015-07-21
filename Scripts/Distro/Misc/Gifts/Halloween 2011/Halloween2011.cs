using System;
using Server;
using Server.Items;

namespace Server.Misc
{
	public class Halloween2011 : GiftGiver
	{
		public static void Initialize()
		{
			GiftGiving.Register( new Halloween2011() );
		}

		public override DateTime Start { get { return new DateTime( 2011, 10, 29, 10, 01, 0, 0 ); } }
		public override DateTime Finish { get { return new DateTime( 2011, 11, 2 ); } }

		public override void GiveGift( Mobile mob )
		{
			GiftBox bag = new GiftBox();

			bag.DropItem( new HarvestWine() );
			bag.DropItem( new MurkyMilk( BeverageType.Milk ) );
			bag.DropItem( new CreepyCake() );
			bag.DropItem( new MrPlainsCookies() );
			bag.DropItem( new SmallHalloweenWeb() );

			switch ( GiveGift( mob, bag ) )
			{
				case GiftResult.Backpack:
					mob.SendMessage( String.Format( "Have a creepy Halloween 2011 from the entire {0} Staff Team. We have placed a goddie bag for you in your backpack.", Environment.Config.ServerName ) );
					break;
				case GiftResult.BankBox:
					mob.SendMessage( String.Format( "Have a creepy Halloween 2011 from the entire {0} Staff Team. We have placed a goddie bag for you in your bank box.", Environment.Config.ServerName ) );
					break;
			}
		}
	}
}
