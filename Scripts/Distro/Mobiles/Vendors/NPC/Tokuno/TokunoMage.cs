using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	public class TokunoMage : Mage
	{
		[Constructable]
		public TokunoMage()
		{
		}

		public override VendorShoeType ShoeType { get { return VendorShoeType.SamuraiTabi; } }

		public override void InitOutfit()
		{
			AddItem( new HakamaShita() );
			AddItem( new Hakama() );
		}

		public TokunoMage( Serial serial )
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