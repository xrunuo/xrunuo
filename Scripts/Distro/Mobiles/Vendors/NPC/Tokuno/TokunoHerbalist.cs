using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	public class TokunoHerbalist : Herbalist
	{
		[Constructable]
		public TokunoHerbalist()
		{
		}

		public override VendorShoeType ShoeType { get { return VendorShoeType.NinjaTabi; } }

		public override void InitOutfit()
		{
			AddItem( new LeatherNinjaPants() );
			AddItem( new HalfApron() );
			AddItem( new HakamaShita() );
		}

		public TokunoHerbalist( Serial serial )
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