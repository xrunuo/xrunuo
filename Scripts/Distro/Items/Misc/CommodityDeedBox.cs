using System;
using Server;
using Server.Engines.VeteranRewards;

namespace Server.Items
{
	public class CommodityDeedBox : WoodenBox, IRewardItem
	{
		private bool m_IsRewardItem;

		[CommandProperty( AccessLevel.GameMaster )]
		public bool IsRewardItem
		{
			get { return m_IsRewardItem; }
			set { m_IsRewardItem = value; }
		}

		public override int LabelNumber { get { return 1080523; } } // Commodity Deed Box

		[Constructable]
		public CommodityDeedBox()
		{
			Hue = 0x47;
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			if ( m_IsRewardItem )
				list.Add( 1076217 ); // 1st Year Veteran Reward
		}

		public CommodityDeedBox( Serial serial )
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

		public static CommodityDeedBox Find( Item deed )
		{
			Item parent = deed;

			while ( parent != null && !( parent is CommodityDeedBox ) )
				parent = parent.Parent as Item;

			return parent as CommodityDeedBox;
		}
	}
}