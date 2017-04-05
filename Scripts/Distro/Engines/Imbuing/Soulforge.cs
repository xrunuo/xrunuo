using System;
using System.Linq;

using Server;
using Server.Mobiles;
using Server.Engines.Loyalty;

namespace Server.Engines.Imbuing
{
	public class Soulforge
	{
		private static bool CheckTile( int tileId )
		{
			if ( tileId >= 0x423B && tileId <= 0x4243 )
				return true;

			if ( tileId >= 0x424B && tileId <= 0x4257 )
				return true;

			if ( tileId >= 0x4263 && tileId <= 0x4272 )
				return true;

			if ( tileId >= 0x4277 && tileId <= 0x4286 )
				return true;

			return false;
		}

		//private static readonly Point3D m_QueenSoulforgeLoc = new Point3D( 749, 3375, -65 );

		public static bool CheckQueen( Mobile from )
		{
			PlayerMobile pm = from as PlayerMobile;

			if ( pm != null && pm.Region.IsPartOf( "Queen's Palace" ) )
				return pm.LoyaltyInfo.GetValue( LoyaltyGroup.GargoyleQueen ) >= 10000;

			return true;
		}

		public static bool CheckProximity( Mobile from, int range )
		{
			Map map = from.Map;

			if ( map == null )
				return false;

			bool found = from.GetItemsInRange( range ).Any( i => CheckTile( i.ItemID ) );

			for ( int x = -range; !found && x <= range; ++x )
			{
				for ( int y = -range; !found && y <= range; ++y )
				{
					Tile[] tiles = map.Tiles.GetStaticTiles( from.X + x, from.Y + y, true );

					for ( int i = 0; !found && i < tiles.Length; ++i )
					{
						if ( CheckTile( tiles[i].ID & TileData.MaxItemValue ) )
							found = true;
					}
				}
			}

			return found;
		}
	}
}
