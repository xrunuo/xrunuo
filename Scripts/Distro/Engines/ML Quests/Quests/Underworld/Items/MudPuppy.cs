using System;
using Server;

namespace Server.Items
{
	public class MudPuppy : BigFish
	{
		public override int LabelNumber { get { return 1095117; } } // Mud Puppy

		[Constructable]
		public MudPuppy()
		{
			Hue = 0x1BD;
		}

		public MudPuppy( Serial serial )
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

			/*int version = */
			reader.ReadInt();
		}
	}
}