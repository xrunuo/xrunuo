using System;
using System.Collections;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	public class GenericSellInfo : IShopSellInfo
	{
		private Hashtable m_Table = new Hashtable();
		private Type[] m_Types;

		public GenericSellInfo()
		{
		}

		public void Add( Type type, int price )
		{
			m_Table[type] = price;
			m_Types = null;
		}

		public int GetSellPriceFor( Item item )
		{
			int price = (int) m_Table[item.GetType()];

			if ( ( item is BaseWeapon && ( (BaseWeapon) item ).Exceptional ) || ( item is BaseArmor && ( (BaseArmor) item ).Exceptional ) )
				price = (int) ( price * 1.25 );

			if ( item is BaseBeverage )
			{
				int price1 = price, price2 = price;

				if ( item is Pitcher )
				{
					price1 = 3;
					price2 = 5;
				}
				else if ( item is BeverageBottle )
				{
					price1 = 3;
					price2 = 3;
				}
				else if ( item is Jug )
				{
					price1 = 6;
					price2 = 6;
				}

				BaseBeverage bev = (BaseBeverage) item;

				if ( bev.IsEmpty || bev.Content == BeverageType.Milk )
					price = price1;
				else
					price = price2;
			}

			return price;
		}

		public int GetBuyPriceFor( Item item )
		{
			return (int) ( 1.90 * GetSellPriceFor( item ) );
		}

		public Type[] Types
		{
			get
			{
				if ( m_Types == null )
				{
					m_Types = new Type[m_Table.Keys.Count];
					m_Table.Keys.CopyTo( m_Types, 0 );
				}

				return m_Types;
			}
		}

		public string GetNameFor( Item item )
		{
			if ( item.Name != null )
				return item.Name;
			else
				return item.LabelNumber.ToString();
		}

		public bool IsSellable( Item item )
		{
			return IsInList( item.GetType() );
		}

		public bool IsResellable( Item item )
		{
			return IsInList( item.GetType() );
		}

		public bool IsInList( Type type )
		{
			Object o = m_Table[type];

			return o != null;
		}
	}
}