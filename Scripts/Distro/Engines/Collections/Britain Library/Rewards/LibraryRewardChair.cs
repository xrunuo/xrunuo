using System;
using Server;

namespace Server.Items
{
	public class LibraryRewardChair : CozyElvenChair
	{
		public override int LabelNumber { get { return 1073340; } } // Friends of the Library Reading Chair

		[Constructable]
		public LibraryRewardChair()
		{
			Hue = 450;
		}

		public LibraryRewardChair( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}
}