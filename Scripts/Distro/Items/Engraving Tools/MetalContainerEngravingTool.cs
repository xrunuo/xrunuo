using System;
using System.Linq;

namespace Server.Items
{
	public class MetalContainerEngravingTool : BaseEngravingTool
	{
		public override int LabelNumber { get { return 1072154; } } // metal container engraving tool

		private static Type[] m_MetalContainers = new Type[]
			{
				typeof( MetalChest ),	typeof( ParagonChest )
			};

		public override bool ValidateItem( Item item )
		{
			return m_MetalContainers.Contains( item.GetType() );
		}

		[Constructable]
		public MetalContainerEngravingTool()
			: base( 0x1EB8 )
		{
			Hue = 1165;
			Weight = 5.0;
		}

		public MetalContainerEngravingTool( Serial serial )
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