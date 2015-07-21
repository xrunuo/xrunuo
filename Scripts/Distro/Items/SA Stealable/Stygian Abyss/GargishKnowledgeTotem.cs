using System;

namespace Server.Items
{
	public class GargishKnowledgeTotem : StealableArtifact
	{
		public override int ArtifactRarity { get { return 7; } }

		public override int LabelNumber { get { return 1095961; } } // Gargish Knowledge Totem

		[Constructable]
		public GargishKnowledgeTotem()
			: base( 0x42C1 )
		{
			Weight = 10.0;
		}

		public GargishKnowledgeTotem( Serial serial )
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
