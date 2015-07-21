using System;
using System.Linq;

using Server;

namespace Server.Misc
{
	public static class MapExtensions
	{
		public static int GetNameCliloc( this Map map )
		{
			int cliloc = -1;

			if ( map == Map.Felucca )
				cliloc = 1012001; // Felucca
			else if ( map == Map.Trammel )
				cliloc = 1012000; // Trammel
			else if ( map == Map.Ilshenar )
				cliloc = 1012002; // Ilshenar
			else if ( map == Map.Malas )
				cliloc = 1060643; // Malas
			else if ( map == Map.Tokuno )
				cliloc = 1063258; // Tokuno Islands
			else if ( map == Map.TerMur )
				cliloc = 1112178; // Ter Mur

			return cliloc;
		}
	}
}
