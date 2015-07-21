using System;
using System.Linq;
using Server.Engines.Housing.Multis;
using Server.Items;
using Server.Mobiles;
using Server.Multis;

namespace Server.Engines.Housing
{
	public static class HousingHelper
	{
		public static void IsThereVendor( Point3D location, Map map, out bool vendor, out bool rentalContract )
		{
			vendor = false;
			rentalContract = false;

			var eable = map.GetObjectsInRange( location, 0 );

			foreach ( IEntity entity in eable )
			{
				if ( Math.Abs( location.Z - entity.Z ) <= 16 )
				{
					if ( entity is PlayerVendor || entity is PlayerBarkeeper || entity is PlayerVendorPlaceholder )
					{
						vendor = true;
						break;
					}

					if ( entity is VendorRentalContract )
					{
						rentalContract = true;
						break;
					}
				}
			}
		}

		public static IHouse FindHouseAt( Mobile m )
		{
			if ( m == null || m.Deleted )
				return null;

			return FindHouseAt( m.Location, m.Map, 16 );
		}

		public static IHouse FindHouseAt( Item item )
		{
			if ( item == null || item.Deleted )
				return null;

			return FindHouseAt( item.GetWorldLocation(), item.Map, item.ItemData.Height );
		}

		public static IHouse FindHouseAt( Point3D loc, Map map, int height )
		{
			if ( map == null || map == Map.Internal )
				return null;

			Sector sector = map.GetSector( loc );

			return sector.Multis.OfType<BaseHouse>().FirstOrDefault( h => h.IsInside( loc, height ) );
		}

		public static bool CheckLockedDown( Item item )
		{
			var house = FindHouseAt( item );

			return ( house != null && house.IsLockedDown( item ) );
		}

		public static bool CheckSecured( Item item )
		{
			var house = FindHouseAt( item );

			return ( house != null && house.IsSecure( item ) );
		}

		public static bool CheckLockedDownOrSecured( Item item )
		{
			var house = FindHouseAt( item );

			return ( house != null && ( house.IsSecure( item ) || house.IsLockedDown( item ) ) );
		}

		public static bool CheckHold( Mobile m, Container cont, Item item, bool message, bool checkItems, int plusItems, int plusWeight )
		{
			var house = FindHouseAt( cont );

			if ( house == null )
				return true;

			if ( house.IsSecure( cont ) && !house.CheckAosStorage( 1 + item.TotalItems + plusItems ) )
			{
				if ( message )
					m.SendLocalizedMessage( 1061839 ); // This action would exceed the secure storage limit of the house.

				return false;
			}

			return true;
		}

		public static bool CheckAccessible( Mobile m, Item item )
		{
			if ( m.AccessLevel >= AccessLevel.GameMaster )
			{
				return true; // Staff can access anything
			}

			var house = FindHouseAt( item );

			if ( house == null )
			{
				return true;
			}

			SecureAccessResult res = house.CheckSecureAccess( m, item );

			switch ( res )
			{
				case SecureAccessResult.Insecure:
					break;
				case SecureAccessResult.Accessible:
					return true;
				case SecureAccessResult.Inaccessible:
					return false;
			}

			if ( house.IsLockedDown( item ) )
			{
				return house.IsCoOwner( m ) && ( item is Container );
			}

			return true;
		}
	}
}
