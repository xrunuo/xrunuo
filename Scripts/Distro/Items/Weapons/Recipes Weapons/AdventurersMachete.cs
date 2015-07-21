using System;
using Server;

namespace Server.Items
{
	public class AdventurersMachete : ElvenMachete
	{
		public override int LabelNumber { get { return 1073533; } } // Adventurer's Machetee

		[Constructable]
		public AdventurersMachete()
		{
			Attributes.Luck = 20;
		}


		public AdventurersMachete( Serial serial )
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