using System;
using Server;
using Server.Engines.Craft;

namespace Server.Items
{
	public class RunicFletchersTools : BaseRunicTool
	{
		public override CraftSystem CraftSystem { get { return DefBowFletching.CraftSystem; } }

		public override LocalizedText GetNameProperty()
		{
			int num = 0;

			switch ( Resource )
			{
				case CraftResource.Oak:
					num = 1072628; // Oak Runic Fletcher's Tools
					break;
				case CraftResource.Ash:
					num = 1072629; // Ash Runic Fletcher's Tools
					break;
				case CraftResource.Yew:
					num = 1072630; // Yew Runic Fletcher's Tools
					break;
				case CraftResource.Heartwood:
					num = 1072631; // Heartwood Runic Fletcher's Tools
					break;
			}

			return new LocalizedText( num );
		}

		[Constructable]
		public RunicFletchersTools( CraftResource resource )
			: base( resource, 0x1022 )
		{
			Weight = 2.0;
			Hue = CraftResources.GetHue( resource );
		}

		[Constructable]
		public RunicFletchersTools( CraftResource resource, int uses )
			: base( resource, uses, 0x1022 )
		{
			Weight = 2.0;
			Hue = CraftResources.GetHue( resource );
		}

		public RunicFletchersTools( Serial serial )
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