using System;

namespace Server.Items
{
	public class GargishProtectiveTotem : StealableArtifact
	{
		public override int ArtifactRarity { get { return 6; } }

		public override int LabelNumber { get { return 1095955; } } // Gargish Protective Totem

		[Constructable]
		public GargishProtectiveTotem()
			: base( 0x42BB )
		{
			Weight = 10.0;
		}

		public GargishProtectiveTotem( Serial serial )
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
