using System;
using Server.Items;

namespace Server.Mobiles
{
	public class TokunoBanker : Banker
	{
		[Constructable]
		public TokunoBanker()
		{
		}

		public override VendorShoeType ShoeType { get { return VendorShoeType.NinjaTabi; } }

		public override void InitOutfit()
		{
			AddItem( new HalfApron() );
			AddItem( new Hakama() );
			AddItem( new HakamaShita() );
		}

		public TokunoBanker( Serial serial )
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