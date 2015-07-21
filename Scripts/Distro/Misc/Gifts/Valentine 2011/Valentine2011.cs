using System;
using Server;
using Server.Items;

namespace Server.Misc
{
	public class ValentineGiftGiver2011 : GiftGiver
	{
		public static void Initialize()
		{
			GiftGiving.Register( new ValentineGiftGiver2011() );
		}

		// OSI dates are 2011/2/9 to 2011/3/9
		public override DateTime Start { get { return new DateTime( 2011, 2, 11 ); } }
		public override DateTime Finish { get { return new DateTime( 2011, 2, 26 ); } }

		public override void GiveGift( Mobile mob )
		{
			Container basket = new PicnicBasket();
			basket.Hue = Utility.RandomDyedHue();
			basket.AddItem( new ValentineBear( mob ) );

			switch ( GiveGift( mob, basket ) )
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