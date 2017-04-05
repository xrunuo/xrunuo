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
		private static readonly ILog log = LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

		private static readonly BodyType[] m_Types;

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
							log.Warning( "Invalid bodyTable entry: {0}", line );
							continue;
						}
					}
					return;
				}
			}
			log.Warning( "Data/bodyTable.cfg does not exist" );
		}

		public Body( int bodyID )
		{
			BodyID = bodyID;
		}

		public BodyType Type
		{
			get
			{
				if ( BodyID >= 0 && BodyID < m_Types.Length )
					return m_Types[BodyID];
				else
					return BodyType.Empty;
			}
		}

		public bool IsHuman => BodyID >= 0
		                       && BodyID < m_Types.Length
		                       && m_Types[BodyID] == BodyType.Human
		                       && BodyID != 402
		                       && BodyID != 403
		                       && BodyID != 607
		                       && BodyID != 608
		                       && BodyID != 694
		                       && BodyID != 695
		                       && BodyID != 970;

		public bool IsMale => BodyID == 183
		                      || BodyID == 185
		                      || BodyID == 400
		                      || BodyID == 402
		                      || BodyID == 605
		                      || BodyID == 607
		                      || BodyID == 666
		                      || BodyID == 694
		                      || BodyID == 750;

		public bool IsFemale => BodyID == 184
		                        || BodyID == 186
		                        || BodyID == 401
		                        || BodyID == 403
		                        || BodyID == 606
		                        || BodyID == 608
		                        || BodyID == 667
		                        || BodyID == 695
		                        || BodyID == 751;

		public bool IsGhost => BodyID == 402
		                       || BodyID == 403
		                       || BodyID == 607
		                       || BodyID == 608
		                       || BodyID == 694
		                       || BodyID == 695
		                       || BodyID == 970;

		public bool IsMonster => BodyID >= 0
		                         && BodyID < m_Types.Length
		                         && m_Types[BodyID] == BodyType.Monster;

		public bool IsAnimal => BodyID >= 0
		                        && BodyID < m_Types.Length
		                        && m_Types[BodyID] == BodyType.Animal;

		public bool IsEmpty => BodyID >= 0
		                       && BodyID < m_Types.Length
		                       && m_Types[BodyID] == BodyType.Empty;

		public bool IsSea => BodyID >= 0
		                     && BodyID < m_Types.Length
		                     && m_Types[BodyID] == BodyType.Sea;

		public bool IsEquipment => BodyID >= 0
		                           && BodyID < m_Types.Length
		                           && m_Types[BodyID] == BodyType.Equipment;

		public int BodyID { get; }

		public static implicit operator int( Body a )
		{
			return a.BodyID;
		}

		public static implicit operator Body( int a )
		{
			return new Body( a );
		}

		public override string ToString()
		{
			return string.Format( "0x{0:X}", BodyID );
		}

		public override int GetHashCode()
		{
			return BodyID;
		}

		public override bool Equals( object o )
		{
			if ( o == null || !( o is Body ) ) return false;

			return ( (Body) o ).BodyID == BodyID;
		}

		public static bool operator ==( Body l, Body r )
		{
			return l.BodyID == r.BodyID;
		}

		public static bool operator !=( Body l, Body r )
		{
			return l.BodyID != r.BodyID;
		}

		public static bool operator >( Body l, Body r )
		{
			return l.BodyID > r.BodyID;
		}

		public static bool operator >=( Body l, Body r )
		{
			return l.BodyID >= r.BodyID;
		}

		public static bool operator <( Body l, Body r )
		{
			return l.BodyID < r.BodyID;
		}

		public static bool operator <=( Body l, Body r )
		{
			return l.BodyID <= r.BodyID;
		}
	}
}
