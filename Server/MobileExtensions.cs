using System;
using System.Linq;

using Server;
using Server.Items;
using Server.Targeting;

namespace Server
{
	public static class MobileExtensions
	{
		public static void DeleteItemsByType( this Mobile m, Type type )
		{
			m.GetEquippedItems().Where( item => item.GetType() == type ).Each( item => item.Delete() );

			if ( m.Backpack != null )
				m.Backpack.DeleteItemsByType( type );
		}

		public static void DeleteItemsByType<T>( this Mobile m ) where T : Item
		{
			m.DeleteItemsByType( typeof( T ) );
		}

		public static bool HasFreeHand( this Mobile m )
		{
			return m.FindItemOnLayer( Layer.TwoHanded ) == null;
		}

		public static bool PlaceInBackpack( this Mobile m, Item item )
		{
			if ( item.Deleted )
				return false;

			Container pack = m.Backpack;

			return pack != null && pack.TryDropItem( m, item, false );
		}

		public static bool AddToBackpack( this Mobile m, Item item )
		{
			if ( item.Deleted )
				return false;

			if ( !m.PlaceInBackpack( item ) )
			{
				Point3D loc = m.Location;
				Map map = m.Map;

				if ( ( map == null || map == Map.Internal ) && m.LogoutMap != null )
				{
					loc = m.LogoutLocation;
					map = m.LogoutMap;
				}

				item.MoveToWorld( loc, map );
				return false;
			}

			return true;
		}

		public static void ClearHands( this Mobile m )
		{
			m.ClearHand( m.FindItemOnLayer( Layer.OneHanded ) );
			m.ClearHand( m.FindItemOnLayer( Layer.TwoHanded ) );
		}

		public static void ClearHand( this Mobile m, Item item )
		{
			if ( item != null && item.Movable && !item.AllowEquipedCast( m ) )
			{
				Container pack = m.Backpack;

				if ( pack == null )
					m.AddToBackpack( item );
				else
					pack.DropItem( item );
			}
		}
	}
}
