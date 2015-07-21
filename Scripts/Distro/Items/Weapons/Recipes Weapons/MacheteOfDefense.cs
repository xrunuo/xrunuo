using System;
using Server;

namespace Server.Items
{
	public class MacheteOfDefense : ElvenMachete
	{
		public override int LabelNumber { get { return 1073535; } } // Machete of Defense

		[Constructable]
		public MacheteOfDefense()
		{
			Attributes.DefendChance = 5;

		}


		public MacheteOfDefense( Serial serial )
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