using System;
using Server.Items;

namespace Server.Mobiles
{
	public class WeakSkeleton : Skeleton
	{
		[Constructable]
		public WeakSkeleton()
		{
			SetHits( 10, 15 );

			PackItem( new MagicArrowScroll() );
			PackItem( new LesserHealPotion() );
		}

		public WeakSkeleton( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */reader.ReadInt();
		}
	}
}