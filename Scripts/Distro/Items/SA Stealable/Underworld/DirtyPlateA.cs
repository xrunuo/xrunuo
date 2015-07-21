using System;

namespace Server.Items
{
	public class DirtyPlateA : StealableArtifact
	{
		public override int ArtifactRarity { get { return 5; } }

		[Constructable]
		public DirtyPlateA()
			: base( 0x09AE )
		{
			Name = Utility.RandomBool() ? "Dirty Plate" : "Half Eaten Supper";
			Weight = 10.0;
		}

		public DirtyPlateA( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}
}
