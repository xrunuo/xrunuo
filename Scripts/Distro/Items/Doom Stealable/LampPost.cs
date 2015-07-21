using System;

namespace Server.Items
{
	public class LampPost : StealableLightArtifact
	{
		public override int ArtifactRarity { get { return 3; } }

		public override int LitItemID { get { return 0xB25; } }
		public override int UnlitItemID { get { return 0xB24; } }

		[Constructable]
		public LampPost()
			: base( 0xB24 )
		{
			Duration = TimeSpan.Zero; // Never burnt out

			Burning = true;

			Light = LightType.Circle300;

			switch ( Utility.Random( 7 ) )
			{
				case 0:
					Hue = 1150;
					break;
				case 1:
					Hue = 1154;
					break;
				case 2:
					Hue = 1175;
					break;
				case 3:
					Hue = 1276;
					break;
				case 4:
					Hue = 1268;
					break;
				case 5:
					Hue = 505;
					break;
				case 6:
					Hue = 0;
					break;
			}
		}

		public LampPost( Serial serial )
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

			if ( ItemID != 0xB24 )
			{
				ItemID = 0xB24;
			}
		}
	}
}
