using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	public class TokunoBaker : Baker
	{
		[Constructable]
		public TokunoBaker()
		{
		}

		public override VendorShoeType ShoeType { get { return VendorShoeType.SamuraiTabi; } }

		public override void InitOutfit()
		{
			AddItem( new Hakama() );
			AddItem( new HalfApron() );
			AddItem( new HakamaShita() );
		}

		public TokunoBaker( Serial serial )
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

			/*int version = */reader.ReadInt();
		}
	}
}