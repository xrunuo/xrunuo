using System;

namespace Server.Items
{
	public class SkullCandle : StealableLightArtifact
	{
		public override int ArtifactRarity { get { return 1; } }

		public override int LitItemID { get { return 0x1858; } }
		public override int UnlitItemID { get { return 0x1857; } }

		[Constructable]
		public SkullCandle()
			: base( 0x1858 )
		{
			Duration = TimeSpan.Zero; // Never burnt out

			Burning = true;

			Light = LightType.Circle150;
		}

		public SkullCandle( Serial serial )
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
