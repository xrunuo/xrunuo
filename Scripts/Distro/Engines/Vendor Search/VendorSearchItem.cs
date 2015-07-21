using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.VendorSearch
{
	public interface IVendorSearchItem
	{
		bool Valid { get; }
		int Price { get; }
		Item Item { get; }
		PlayerVendor Vendor { get; }
		Container Container { get; }
		bool IsContained { get; }
	}

	public class VendorSearchItem : IVendorSearchItem
	{
		public static IVendorSearchItem CreateForItem( VendorItem vi )
		{
			return new VendorSearchItem( vi.Item, vi.Price, () => vi.Valid, vi.Vendor, vi.Item.Parent as Container, false );
		}

		public static IVendorSearchItem CreateForContainedItem( VendorItem vi, Item item )
		{
			return new VendorSearchItem( item, vi.Price, () => vi.Valid, vi.Vendor, vi.Item.Parent as Container, true );
		}

		private Item m_Item;
		private int m_Price;
		private Func<bool> m_ValidCallback;
		private PlayerVendor m_Vendor;
		private Container m_Container;
		private bool m_IsContained;

		public Item Item { get { return m_Item; } }
		public int Price { get { return m_Price; } }
		public bool Valid { get { return m_ValidCallback(); } }
		public PlayerVendor Vendor { get { return m_Vendor; } }
		public Container Container { get { return m_Container; } }
		public bool IsContained { get { return m_IsContained; } }

		public VendorSearchItem( Item item, int price, Func<bool> validCallback, PlayerVendor vendor, Container container, bool isContained )
		{
			m_Item = item;
			m_Price = price;
			m_ValidCallback = validCallback;
			m_Vendor = vendor;
			m_Container = container;
			m_IsContained = isContained;
		}
	}
}
