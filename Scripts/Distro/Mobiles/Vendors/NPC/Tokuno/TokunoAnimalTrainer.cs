using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	public class TokunoAnimalTrainer : AnimalTrainer
	{
		[Constructable]
		public TokunoAnimalTrainer()
		{
		}

		public override void InitOutfit()
		{
			AddItem( new QuarterStaff() );
			AddItem( new ShortPants() );
			AddItem( new LeatherNinjaBelt() );
			AddItem( new HakamaShita() );
		}

		public TokunoAnimalTrainer( Serial serial )
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