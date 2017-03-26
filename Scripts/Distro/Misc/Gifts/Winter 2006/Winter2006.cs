using System;
using Server;
using Server.Items;

namespace Server.Misc
{
	public class WinterGiftGiver2006 : GiftGiver
	{
		public static void Initialize()
		{
			GiftGiving.Register( new WinterGiftGiver2006() );
		}

		public override DateTime Start { get { return new DateTime( 2007, 12, 23, 6, 0, 0, 0 ); } }
		public override DateTime Finish { get { return new DateTime( 2008, 1, 15 ); } }

		public override void GiveGift( Mobile mob )
		{
			Container stocking = null;

			if ( Utility.RandomBool() )
				stocking = new GreenStocking();
			else
				stocking = new RedStocking();

			stocking.DropItem( new SnowPile() );
			stocking.DropItem( new HolidayCard( mob.Name ) );
			stocking.DropItem( new GingerbreadCookie( true ) );
			stocking.DropItem( new GingerbreadCookie( true ) );
			stocking.DropItem( new RecipeScroll( 93 ) );
			stocking.DropItem( new RedCandyCane( 0x2BDD ) );
			stocking.DropItem( new RedCandyCane( 0x2BDE ) );
			stocking.DropItem( new GreenCandyCane( 0x2BDF ) );
			stocking.DropItem( new GreenCandyCane( 0x2BE0 ) );
			stocking.DropItem( new GingerbreadHouseDeed() );

			switch ( GiveGift( mob, stocking ) )
			{
				case GiftResult.Backpack:
					mob.SendMessage( String.Format( "Happy Holidays from the entire {0} Staff Team. We have placed a stocking for you in your backpack.", Core.Config.ServerName ) );
					break;
				case GiftResult.BankBox:
					mob.SendMessage( String.Format( "Happy Holidays from the entire {0} Staff Team. We have placed a stocking for you in your bank box.", Core.Config.ServerName ) );
					break;
			}
		}
	}
}