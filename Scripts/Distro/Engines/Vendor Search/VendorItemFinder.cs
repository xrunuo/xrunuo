using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Misc;
using Server.Events;

namespace Server.Engines.VendorSearch
{
	public class VendorItemFinder
	{
		public static readonly int MaxResults = 25;

		private static VendorItemFinder m_Instance;

		public static VendorItemFinder Instance
		{
			get
			{
				if ( m_Instance == null )
					m_Instance = new VendorItemFinder();

				return m_Instance;
			}
		}

		public static void Initialize()
		{
			EventSink.ServerStarted += new ServerStartedEventHandler( () => Instance.UpdateCache() );
			EventSink.WorldSave += new WorldSaveEventHandler( ( e ) => Instance.UpdateCache() );
		}

		private IVendorSearchItem[] m_WorldVendorItemsCache;
		private object m_SyncRoot;

		public VendorItemFinder()
		{
			m_WorldVendorItemsCache = new IVendorSearchItem[0];
			m_SyncRoot = new object();
		}

		public void UpdateCache()
		{
			lock ( m_SyncRoot )
				m_WorldVendorItemsCache = FindWorldVendorItems().ToArray();
		}

		private IEnumerable<IVendorSearchItem> FindWorldVendorItems()
		{
			var allVendors = World.Instance.Mobiles.OfType<PlayerVendor>();
			var allVendorItems = allVendors.SelectMany( v => v.GetAllVendorItems().Where( vi => vi.IsForSale ) );

			foreach ( VendorItem vi in allVendorItems )
			{
				if ( vi.Item is Container )
				{
					foreach ( var contained in GetContainedItems( vi, (Container) vi.Item ) )
					{
						yield return contained;
					}
				}
				else
					yield return VendorSearchItem.CreateForItem( vi );
			}
		}

		private static IEnumerable<IVendorSearchItem> GetContainedItems( VendorItem vi, Container cont )
		{
			foreach ( Item item in cont.Items )
			{
				if ( item is Container )
				{
					foreach ( var contained in GetContainedItems( vi, (Container) item ) )
					{
						yield return contained;
					}
				}
				else
					yield return VendorSearchItem.CreateForContainedItem( vi, item );
			}
		}

		public Task<IVendorSearchItem[]> FindVendorItemsAsync( SearchCriteria criteria, int page = 0 )
		{
			return new Task<IVendorSearchItem[]>( () =>
				{
					return FindVendorItems( criteria, page );
				} );
		}

		public IVendorSearchItem[] FindVendorItems( SearchCriteria criteria, int page = 0 )
		{
			lock ( m_SyncRoot )
			{
				return m_WorldVendorItemsCache
					.Where( vi => vi.Valid && criteria.Matches( vi ) )
					.OrderBy( vi => vi.Price )
					.Skip( MaxResults * page )
					.Take( MaxResults )
					.ToArray();
			}
		}
	}
}
