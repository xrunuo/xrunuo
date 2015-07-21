using System;
using System.Collections.Generic;

namespace Server.Engines.Craft
{
	public class CraftItemCol : Dictionary<int, CraftItem>
	{
		public CraftItemCol()
		{
		}

		public CraftItem GetAt( int index )
		{
			if ( !ContainsKey( index ) )
				return null;

			return this[index];
		}

		public CraftItem SearchForSubclass( Type type )
		{
			foreach ( CraftItem craftItem in Values )
			{
				if ( craftItem.ItemType == type || type.IsSubclassOf( craftItem.ItemType ) )
					return craftItem;
			}

			return null;
		}

		public CraftItem SearchFor( Type type )
		{
			foreach ( CraftItem craftItem in Values )
			{
				if ( craftItem.ItemType == type )
					return craftItem;
			}

			return null;
		}
	}
}