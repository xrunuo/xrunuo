using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
	public class BasketWeavingBook : Item
	{
		[Constructable]
		public BasketWeavingBook()
			: base( 0xFF4 )
		{
			Name = "Making Valuables With Basket Weaving";
			Weight = 5.0;
		}

		public BasketWeavingBook( Serial serial )
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

		public override void OnDoubleClick( Mobile from )
		{
			PlayerMobile pm = from as PlayerMobile;

			if ( !IsChildOf( from.Backpack ) )
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
			else if ( pm == null || from.Skills[SkillName.Tinkering].Base < 100.0 )
				from.SendLocalizedMessage( 1112255 ); // Only a Grandmaster Tinker can learn from this book.
			else if ( pm.BasketWeaving )
				pm.SendLocalizedMessage( 1080066 ); // You have already learned this information.
			else
			{
				pm.BasketWeaving = true;
				pm.SendLocalizedMessage( 1112254 ); // You have learned to make baskets. You will need gardeners to make reeds out of plants for you to make these items.
				Delete();
			}
		}
	}
}
