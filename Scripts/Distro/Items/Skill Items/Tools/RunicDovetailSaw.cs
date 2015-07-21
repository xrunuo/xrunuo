using System;
using Server;
using Server.Engines.Craft;

namespace Server.Items
{
	public class RunicDovetailSaw : BaseRunicTool
	{
		public override CraftSystem CraftSystem { get { return DefCarpentry.CraftSystem; } }

		public override LocalizedText GetNameProperty()
		{
			int num = 0;

			switch ( Resource )
			{
				case CraftResource.Oak:
					num = 1072634; // Oak Runic Dovetail Saw
					break;
				case CraftResource.Ash:
					num = 1072635; // Ash Runic Dovetail Saw
					break;
				case CraftResource.Yew:
					num = 1072636; // Yew Runic Dovetail Saw
					break;
				case CraftResource.Heartwood:
					num = 1072637; // Heartwood Runic Dovetail Saw
					break;
			}

			return new LocalizedText( num );
		}

		[Constructable]
		public RunicDovetailSaw( CraftResource resource )
			: base( resource, 0x1028 )
		{
			Weight = 2.0;
			Hue = CraftResources.GetHue( resource );
		}

		[Constructable]
		public RunicDovetailSaw( CraftResource resource, int uses )
			: base( resource, uses, 0x1028 )
		{
			Weight = 2.0;
			Hue = CraftResources.GetHue( resource );
		}

		public RunicDovetailSaw( Serial serial )
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