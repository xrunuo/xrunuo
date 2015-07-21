using System;

namespace Server.Items
{
	public class JugsofGoblinRotgut : StealableArtifact
	{
		public override int ArtifactRarity { get { return 3; } }

		public override int LabelNumber { get { return 1113681; } } // jugs of goblin rotgut

		[Constructable]
		public JugsofGoblinRotgut()
			: base( 0x098E )
		{
			Weight = 10.0;
		}

		public JugsofGoblinRotgut( Serial serial )
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
