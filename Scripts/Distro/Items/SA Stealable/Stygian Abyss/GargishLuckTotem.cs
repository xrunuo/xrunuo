using System;

namespace Server.Items
{
	public class GargishLuckTotem : StealableArtifact
	{
		public override int ArtifactRarity { get { return 6; } }

		public override int LabelNumber { get { return 1095960; } } // Gargish Luck Totem

		[Constructable]
		public GargishLuckTotem()
			: base( 0x42C0 )
		{
			Weight = 10.0;
		}

		public GargishLuckTotem( Serial serial )
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
