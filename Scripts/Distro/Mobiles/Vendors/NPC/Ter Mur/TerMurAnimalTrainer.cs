using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	public class TerMurAnimalTrainer : AnimalTrainer
	{
		public override Race DefaultRace { get { return Race.Gargoyle; } }

		[Constructable]
		public TerMurAnimalTrainer()
		{
			Title = "the Animal Trainer";
		}

		public override void InitOutfit()
		{
			AddItem( new GargishLeatherLeggings( Utility.RandomNeutralHue() ) );
			AddItem( new GargishLeatherChest( GetRandomHue() ) );
			AddItem( new GargishLeatherArms( Utility.RandomNeutralHue() ) );
			AddItem( new GargishLeatherKilt( Utility.RandomNeutralHue() ) );
		}

		public TerMurAnimalTrainer( Serial serial )
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