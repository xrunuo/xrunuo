//
//  X-RunUO - Ultima Online Server Emulator
//  Copyright (C) 2015 Pedro Pardal
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//

using System;
using System.Collections;

namespace Server
{
	public class Insensitive
	{
		private static IComparer m_Comparer = CaseInsensitiveComparer.Default;

		public static IComparer Comparer
		{
			get { return m_Comparer; }
		}

		private Insensitive()
		{
		}

		public static int Compare( string a, string b )
		{
			return m_Comparer.Compare( a, b );
		}

		public static bool Equals( string a, string b )
		{
			if ( a == null && b == null )
				return true;
			else if ( a == null || b == null || a.Length != b.Length )
				return false;

			return ( m_Comparer.Compare( a, b ) == 0 );
		}

		public static bool StartsWith( string a, string b )
		{
			if ( a == null || b == null || a.Length < b.Length )
				return false;

			return ( m_Comparer.Compare( a.Substring( 0, b.Length ), b ) == 0 );
		}

		public static bool EndsWith( string a, string b )
		{
			if ( a == null || b == null || a.Length < b.Length )
				return false;

			return ( m_Comparer.Compare( a.Substring( a.Length - b.Length ), b ) == 0 );
		}

		public static bool Contains( string a, string b )
		{
			if ( a == null || b == null || a.Length < b.Length )
				return false;

			a = a.ToLower();
			b = b.ToLower();

			return ( a.IndexOf( b ) >= 0 );
		}
	}
}