using System;
using System.Collections;
using Server;
using Server.Engines.Housing;
using Server.Engines.Housing.Items;
using Server.Multis;

namespace Server.Mobiles
{
	public class VendorInventory
	{
		public static readonly TimeSpan GracePeriod = TimeSpan.FromDays( 9.0 );

		private IHouse m_House;
		private string m_VendorName;
		private string m_ShopName;
		private Mobile m_Owner;

		private ArrayList m_Items;
		private int m_Gold;

		private DateTime m_ExpireTime;
		private Timer m_ExpireTimer;

		public VendorInventory( IHouse house, Mobile owner, string vendorName, string shopName )
		{
			m_House = house;
			m_Owner = owner;
			m_VendorName = vendorName;
			m_ShopName = shopName;

			m_Items = new ArrayList();

			m_ExpireTime = DateTime.UtcNow + GracePeriod;
			m_ExpireTimer = new ExpireTimer( this, GracePeriod );
			m_ExpireTimer.Start();
		}

		public IHouse House { get { return m_House; } set { m_House = value; } }

		public string VendorName { get { return m_VendorName; } set { m_VendorName = value; } }

		public string ShopName { get { return m_ShopName; } set { m_ShopName = value; } }

		public Mobile Owner { get { return m_Owner; } set { m_Owner = value; } }

		public ArrayList Items { get { return m_Items; } }

		public int Gold { get { return m_Gold; } set { m_Gold = value; } }

		public DateTime ExpireTime { get { return m_ExpireTime; } }

		public void AddItem( Item item )
		{
			item.Internalize();
			m_Items.Add( item );
		}

		public void Delete()
		{
			try
			{
				foreach ( Item item in Items )
				{
					item.Delete();
				}

				Items.Clear();
				Gold = 0;

				if ( House != null )
				{
					House.VendorInventories.Remove( this );
				}

				m_ExpireTimer.Stop();
			}
			catch ( Exception e )
			{
				Logger.Error( "{0}", e );
			}
		}

		public void Serialize( GenericWriter writer )
		{
			writer.WriteEncodedInt( 0 ); // version

			writer.Write( (Mobile) m_Owner );
			writer.Write( (string) m_VendorName );
			writer.Write( (string) m_ShopName );

			writer.WriteItemList( m_Items, true );
			writer.Write( (int) m_Gold );

			writer.WriteDeltaTime( m_ExpireTime );
		}

		public VendorInventory( IHouse house, GenericReader reader )
		{
			m_House = house;

			/*int version = */
			reader.ReadEncodedInt();

			m_Owner = reader.ReadMobile();
			m_VendorName = reader.ReadString();
			m_ShopName = reader.ReadString();

			m_Items = reader.ReadItemList();
			m_Gold = reader.ReadInt();

			m_ExpireTime = reader.ReadDeltaTime();

			if ( m_Items.Count == 0 && m_Gold == 0 )
			{
				Timer.DelayCall( TimeSpan.Zero, new TimerCallback( Delete ) );
			}
			else
			{
				TimeSpan delay = m_ExpireTime - DateTime.UtcNow;
				m_ExpireTimer = new ExpireTimer( this, delay > TimeSpan.Zero ? delay : TimeSpan.Zero );
				m_ExpireTimer.Start();
			}
		}

		private class ExpireTimer : Timer
		{
			private VendorInventory m_Inventory;

			public ExpireTimer( VendorInventory inventory, TimeSpan delay )
				: base( delay )
			{
				m_Inventory = inventory;
			}

			protected override void OnTick()
			{
				var house = m_Inventory.House;

				if ( house != null )
				{
					if ( m_Inventory.Gold > 0 )
					{
						if ( house.MovingCrate == null )
						{
							house.MovingCrate = new MovingCrate( house );
						}

						Banker.Deposit( house.MovingCrate, m_Inventory.Gold );
					}

					foreach ( Item item in m_Inventory.Items )
					{
						if ( !item.Deleted )
						{
							house.DropToMovingCrate( item );
						}
					}

					m_Inventory.Gold = 0;
					m_Inventory.Items.Clear();
				}

				m_Inventory.Delete();
			}
		}
	}
}
