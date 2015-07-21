using System;

namespace Server.Items
{
	public class TallCandle : StealableLightArtifact
	{
		public override int ArtifactRarity { get { return 5; } }

		public override int LitItemID { get { return 0x0B1A; } }
		public override int UnlitItemID { get { return 0x0A26; } }

		[Constructable]
		public TallCandle()
			: base( 0x0B1A )
		{
			Duration = TimeSpan.Zero; // Never burnt out

			Burning = true;

			Light = LightType.Circle150;
		}

		public TallCandle( Serial serial )
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
