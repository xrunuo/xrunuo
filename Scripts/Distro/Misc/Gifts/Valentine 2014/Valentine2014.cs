using System;
using Server;
using Server.Items;

namespace Server.Misc
{
	public class ValentineGiftGiver2014 : GiftGiver
	{
		public static void Initialize()
		{
			GiftGiving.Register( new ValentineGiftGiver2014() );
		}

		public override DateTime Start { get { return new DateTime( 2014, 2, 14, 10, 0, 0 ); } }
		public override DateTime Finish { get { return new DateTime( 2014, 2, 23, 6, 0, 0 ); } }

		public override void GiveGift( Mobile mob )
		{
			Container redVelvetBox = new RedVelvetBox();
			redVelvetBox.DropItem( new ValentineCard() );
			redVelvetBox.DropItem( new ValentineCard() );
			redVelvetBox.DropItem( new ValentineCard() );
			redVelvetBox.DropItem( new CupidStatue() );
			redVelvetBox.DropItem( new HeartShapedBox() );
			redVelvetBox.DropItem( new RoseInVase() );

			switch ( GiveGift( mob, redVelvetBox ) )
			{
				case GiftResult.Backpack:
					mob.SendLocalizedMessage( 1077486 ); // Happy Valentine's Day! We have placed a gift for you in your backpack.
					break;
				case GiftResult.BankBox:
					mob.SendLocalizedMessage( 1077487 ); // Happy Valentine's Day! We have placed a gift for you in your bank box.
					break;
			}
		}
	}
}