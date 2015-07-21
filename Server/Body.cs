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
using System.IO;

namespace Server
{
	public enum BodyType : byte
	{
		Empty,
		Monster,
		Sea,
		Animal,
		Human,
		Equipment
	}

	public struct Body
	{
		private int m_BodyID;

		private static BodyType[] m_Types;

		static Body()
		{
			if ( File.Exists( "Data/bodyTable.cfg" ) )
			{
				using ( StreamReader bin = new StreamReader( "Data/bodyTable.cfg" ) )
				{
					string line;
					m_Types = new BodyType[1000];
					while ( ( line = bin.ReadLine() ) != null )
					{
						if ( ( line.Length == 0 ) || line.StartsWith( "#" ) )
							continue;

						char[] delimiters = new char[1] { '\t' };

						string[] data = line.Split( delimiters );

						try
						{
							int BodyID = int.Parse( data[0] );

							BodyType type = (BodyType) Enum.Parse( typeof( BodyType ), data[1], true );

							if ( ( BodyID >= 0 ) && ( BodyID < m_Types.Length ) )
								m_Types[BodyID] = type;

							continue;
						}
						catch
						{
							Console.WriteLine( "Warning: Invalid bodyTable entry:" );
							Console.WriteLine( line );
							continue;
						}
					}
					return;
				}
			}
			Console.WriteLine( "Warning: Data/bodyTable.cfg does not exist" );
		}

		public Body( int bodyID )
		{
			m_BodyID = bodyID;
		}

		public BodyType Type
		{
			get
			{
				if ( m_BodyID >= 0 && m_BodyID < m_Types.Length )
					return m_Types[m_BodyID];
				else
					return BodyType.Empty;
			}
		}

		public bool IsHuman
		{
			get
			{
				return m_BodyID >= 0
					&& m_BodyID < m_Types.Length
					&& m_Types[m_BodyID] == BodyType.Human
					&& m_BodyID != 402
					&& m_BodyID != 403
					&& m_BodyID != 607
					&& m_BodyID != 608
					&& m_BodyID != 694
					&& m_BodyID != 695
					&& m_BodyID != 970;
			}
		}

		public bool IsMale
		{
			get
			{
				return m_BodyID == 183
					|| m_BodyID == 185
					|| m_BodyID == 400
					|| m_BodyID == 402
					|| m_BodyID == 605
					|| m_BodyID == 607
					|| m_BodyID == 666
					|| m_BodyID == 694
					|| m_BodyID == 750;
			}
		}

		public bool IsFemale
		{
			get
			{
				return m_BodyID == 184
					|| m_BodyID == 186
					|| m_BodyID == 401
					|| m_BodyID == 403
					|| m_BodyID == 606
					|| m_BodyID == 608
					|| m_BodyID == 667
					|| m_BodyID == 695
					|| m_BodyID == 751;
			}
		}

		public bool IsGhost
		{
			get
			{
				return m_BodyID == 402
					|| m_BodyID == 403
					|| m_BodyID == 607
					|| m_BodyID == 608
					|| m_BodyID == 694
					|| m_BodyID == 695
					|| m_BodyID == 970;
			}
		}

		public bool IsMonster
		{
			get
			{
				return m_BodyID >= 0
					&& m_BodyID < m_Types.Length
					&& m_Types[m_BodyID] == BodyType.Monster;
			}
		}

		public bool IsAnimal
		{
			get
			{
				return m_BodyID >= 0
					&& m_BodyID < m_Types.Length
					&& m_Types[m_BodyID] == BodyType.Animal;
			}
		}

		public bool IsEmpty
		{
			get
			{
				return m_BodyID >= 0
					&& m_BodyID < m_Types.Length
					&& m_Types[m_BodyID] == BodyType.Empty;
			}
		}

		public bool IsSea
		{
			get
			{
				return m_BodyID >= 0
					&& m_BodyID < m_Types.Length
					&& m_Types[m_BodyID] == BodyType.Sea;
			}
		}

		public bool IsEquipment
		{
			get
			{
				return m_BodyID >= 0
					&& m_BodyID < m_Types.Length
					&& m_Types[m_BodyID] == BodyType.Equipment;
			}
		}

		public int BodyID
		{
			get
			{
				return m_BodyID;
			}
		}

		public static implicit operator int( Body a )
		{
			return a.m_BodyID;
		}

		public static implicit operator Body( int a )
		{
			return new Body( a );
		}

		public override string ToString()
		{
			return string.Format( "0x{0:X}", m_BodyID );
		}

		public override int GetHashCode()
		{
			return m_BodyID;
		}

		public override bool Equals( object o )
		{
			if ( o == null || !( o is Body ) ) return false;

			return ( (Body) o ).m_BodyID == m_BodyID;
		}

		public static bool operator ==( Body l, Body r )
		{
			return l.m_BodyID == r.m_BodyID;
		}

		public static bool operator !=( Body l, Body r )
		{
			return l.m_BodyID != r.m_BodyID;
		}

		public static bool operator >( Body l, Body r )
		{
			return l.m_BodyID > r.m_BodyID;
		}

		public static bool operator >=( Body l, Body r )
		{
			return l.m_BodyID >= r.m_BodyID;
		}

		public static bool operator <( Body l, Body r )
		{
			return l.m_BodyID < r.m_BodyID;
		}

		public static bool operator <=( Body l, Body r )
		{
			return l.m_BodyID <= r.m_BodyID;
		}
	}
}