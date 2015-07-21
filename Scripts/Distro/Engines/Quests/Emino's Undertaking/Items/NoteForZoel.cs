using System;

namespace Server.Engines.Quests.SE
{
	public class NoteForZoel : Item
	{
		public override int LabelNumber { get { return 1063186; } } // A Note for Zoel

		[Constructable]
		public NoteForZoel()
			: base( 0x14F0 )
		{
			Weight = 1.0;

			Hue = 0x6C2;
		}

		public NoteForZoel( Serial serial )
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
