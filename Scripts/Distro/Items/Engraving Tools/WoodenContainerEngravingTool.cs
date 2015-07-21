using System;
using System.Linq;

namespace Server.Items
{
	public class WoodenContainerEngravingTool : BaseEngravingTool
	{
		public override int LabelNumber { get { return 1072153; } } // wooden container engraving tool

		private static Type[] m_WoodenContainers = new Type[]
			{
				typeof( WoodenChest ),			typeof( WoodenBox ),
				typeof( SmallCrate ),			typeof( MediumCrate ),
				typeof( LargeCrate ),			typeof( WoodenBox ),
				typeof( EmptyBookcase ),		typeof( EmptyBookcase ),
				typeof( Armoire ),				typeof( PlainWoodenChest ),
				typeof( OrnateWoodenChest ),	typeof( GildedWoodenChest ),
				typeof( WoodenFootLocker ),		typeof( FinishedWoodenChest ),
				typeof( TallCabinet ),			typeof( ShortCabinet ),
				typeof( RedArmoire ),			typeof( ElegantArmoire ),
				typeof( MapleArmoire ),			typeof( CherryArmoire ),
				typeof( Keg ),					typeof( FancyElvenArmoire ),
				typeof( SimpleElvenArmoire ),	typeof( RarewoodChest ),
				typeof( GargishChest )
			};

		public override bool ValidateItem( Item item )
		{
			return m_WoodenContainers.Contains( item.GetType() );
		}

		[Constructable]
		public WoodenContainerEngravingTool()
			: base( 0x1034 )
		{
			Hue = 1165;
			Weight = 5.0;
		}

		public WoodenContainerEngravingTool( Serial serial )
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