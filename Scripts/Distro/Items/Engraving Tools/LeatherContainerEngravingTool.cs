using System;
using System.Linq;

namespace Server.Items
{
	public class LeatherContainerEngravingTool : BaseEngravingTool
	{
		public override int LabelNumber { get { return 1072152; } } // Leather Container Engraving Tool

		private static Type[] m_LeatherContainers = new Type[]
			{
				typeof( Bag ),	typeof( Backpack ),
			};

		public override bool ValidateItem( Item item )
		{
			return m_LeatherContainers.Contains( item.GetType() );
		}

		[Constructable]
		public LeatherContainerEngravingTool()
			: base( 0xF9D )
		{
			Hue = 1165;
			Weight = 5.0;
		}

		public LeatherContainerEngravingTool( Serial serial )
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