using System;

namespace Server.Items
{
	public class TowerLantern : StealableLightArtifact
	{
		public override int ArtifactRarity { get { return 3; } }

		public override int LitItemID { get { return 0x24BF; } }
		public override int UnlitItemID { get { return 0x24C0; } }

		[Constructable]
		public TowerLantern()
			: base( 0x24C0 )
		{
			Duration = TimeSpan.Zero; // Never burnt out

			Burning = false;

			Light = LightType.Circle300;

		}

		public TowerLantern( Serial serial )
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