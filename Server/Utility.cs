using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Xml;
using Microsoft.Win32;
using Server.Network;

namespace Server
{
	public static class Utility
	{
		private static Encoding m_UTF8, m_UTF8WithEncoding;

		public static Random RandomGenerator { get; } = new Random();

		public static Encoding UTF8
		{
			get
			{
				if ( m_UTF8 == null )
					m_UTF8 = new UTF8Encoding( false, false );

				return m_UTF8;
			}
		}

		public static Encoding UTF8WithEncoding
		{
			get
			{
				if ( m_UTF8WithEncoding == null )
					m_UTF8WithEncoding = new UTF8Encoding( true, false );

				return m_UTF8WithEncoding;
			}
		}

		public static void Seperate( StringBuilder sb, string value, string seperator )
		{
			if ( sb.Length > 0 )
				sb.Append( seperator );

			sb.Append( value );
		}

		public static string Intern( string str )
		{
			if ( str == null )
				return null;
			else if ( str.Length == 0 )
				return String.Empty;

			return String.Intern( str );
		}

		public static void Intern( ref string str )
		{
			str = Intern( str );
		}

		public static bool IsValidIP( string text )
		{
			bool valid = true;

			IPMatch( text, IPAddress.None, ref valid );

			return valid;
		}

		public static bool IPMatch( string val, IPAddress ip )
		{
			bool valid = true;

			return IPMatch( val, ip, ref valid );
		}

		public static string FixHtml( string str )
		{
			if ( str == null )
				return "";

			bool hasOpen = ( str.IndexOf( '<' ) >= 0 );
			bool hasClose = ( str.IndexOf( '>' ) >= 0 );
			bool hasPound = ( str.IndexOf( '#' ) >= 0 );

			if ( !hasOpen && !hasClose && !hasPound )
				return str;

			StringBuilder sb = new StringBuilder( str );

			if ( hasOpen )
				sb.Replace( '<', '(' );

			if ( hasClose )
				sb.Replace( '>', ')' );

			if ( hasPound )
				sb.Replace( '#', '-' );

			return sb.ToString();
		}

		public static bool IPMatch( string val, IPAddress ip, ref bool valid )
		{
			valid = true;

			string[] split = val.Split( '.' );

			for ( int i = 0; i < 4; ++i )
			{
				int lowPart, highPart;

				if ( i >= split.Length )
				{
					lowPart = 0;
					highPart = 255;
				}
				else
				{
					string pattern = split[i];

					if ( pattern == "*" )
					{
						lowPart = 0;
						highPart = 255;
					}
					else
					{
						lowPart = 0;
						highPart = 0;

						bool highOnly = false;
						int lowBase = 10;
						int highBase = 10;

						for ( int j = 0; j < pattern.Length; ++j )
						{
							char c = (char) pattern[j];

							if ( c == '?' )
							{
								if ( !highOnly )
								{
									lowPart *= lowBase;
									lowPart += 0;
								}

								highPart *= highBase;
								highPart += highBase - 1;
							}
							else if ( c == '-' )
							{
								highOnly = true;
								highPart = 0;
							}
							else if ( c == 'x' || c == 'X' )
							{
								lowBase = 16;
								highBase = 16;
							}
							else if ( c >= '0' && c <= '9' )
							{
								int offset = c - '0';

								if ( !highOnly )
								{
									lowPart *= lowBase;
									lowPart += offset;
								}

								highPart *= highBase;
								highPart += offset;
							}
							else if ( c >= 'a' && c <= 'f' )
							{
								int offset = 10 + ( c - 'a' );

								if ( !highOnly )
								{
									lowPart *= lowBase;
									lowPart += offset;
								}

								highPart *= highBase;
								highPart += offset;
							}
							else if ( c >= 'A' && c <= 'F' )
							{
								int offset = 10 + ( c - 'A' );

								if ( !highOnly )
								{
									lowPart *= lowBase;
									lowPart += offset;
								}

								highPart *= highBase;
								highPart += offset;
							}
							else
							{
								valid = false;
							}
						}
					}
				}

				int b = (byte) ( Utility.GetAddressValue( ip ) >> ( i * 8 ) );

				if ( b < lowPart || b > highPart )
					return false;
			}

			return true;
		}

		public static bool IPMatchClassC( IPAddress ip1, IPAddress ip2 )
		{
			return ( ( Utility.GetAddressValue( ip1 ) & 0xFFFFFF ) == ( Utility.GetAddressValue( ip2 ) & 0xFFFFFF ) );
		}

		public static int InsensitiveCompare( string first, string second )
		{
			return Insensitive.Compare( first, second );
		}

		public static bool InsensitiveStartsWith( string first, string second )
		{
			return Insensitive.StartsWith( first, second );
		}

		#region To[Something]
		public static bool ToBoolean( string value )
		{
			try
			{
				return Convert.ToBoolean( value );
			}
			catch
			{
				return false;
			}
		}

		public static double ToDouble( string value )
		{
			try
			{
				return Convert.ToDouble( value );
			}
			catch
			{
				return 0.0;
			}
		}

		public static TimeSpan ToTimeSpan( string value )
		{
			try
			{
				return TimeSpan.Parse( value );
			}
			catch
			{
				return TimeSpan.Zero;
			}
		}

		public static int ToInt32( string value )
		{
			try
			{
				if ( value.StartsWith( "0x" ) )
				{
					return Convert.ToInt32( value.Substring( 2 ), 16 );
				}
				else
				{
					return Convert.ToInt32( value );
				}
			}
			catch
			{
				return 0;
			}
		}

		#endregion

		#region Get[Something]
		public static int GetInt32( string intString, int defaultValue )
		{
			try
			{
				return XmlConvert.ToInt32( intString );
			}
			catch
			{
				try
				{
					return Convert.ToInt32( intString );
				}
				catch
				{
					return defaultValue;
				}
			}
		}

		public static DateTime GetDateTime( string dateTimeString, DateTime defaultValue )
		{
			try
			{
				return XmlConvert.ToDateTime( dateTimeString, XmlDateTimeSerializationMode.Local );
			}
			catch
			{
				try
				{
					return DateTime.Parse( dateTimeString );
				}
				catch
				{
					return defaultValue;
				}
			}
		}

		public static TimeSpan GetTimeSpan( string timeSpanString, TimeSpan defaultValue )
		{
			try
			{
				return XmlConvert.ToTimeSpan( timeSpanString );
			}
			catch
			{
				return defaultValue;
			}
		}

		public static string GetAttribute( XmlElement node, string attributeName )
		{
			return GetAttribute( node, attributeName, null );
		}

		public static string GetAttribute( XmlElement node, string attributeName, string defaultValue )
		{
			if ( node == null )
				return defaultValue;

			XmlAttribute attr = node.Attributes[attributeName];

			if ( attr == null )
				return defaultValue;

			return attr.Value;
		}

		public static string GetText( XmlElement node, string defaultValue )
		{
			if ( node == null )
				return defaultValue;

			return node.InnerText;
		}

		public static int GetAddressValue( IPAddress address )
		{
			return (int) GetLongAddressValue( address );
		}

		public static long GetLongAddressValue( IPAddress address )
		{
#pragma warning disable 618
			return address.Address;
#pragma warning restore 618
		}

		#endregion

		public static double RandomDouble()
		{
			return RandomGenerator.NextDouble();
		}

		public static Direction GetDirection( IPoint2D from, IPoint2D to )
		{
			int dx = to.X - from.X;
			int dy = to.Y - from.Y;

			int adx = Math.Abs( dx );
			int ady = Math.Abs( dy );

			if ( adx >= ady * 3 )
			{
				if ( dx > 0 )
					return Direction.East;
				else
					return Direction.West;
			}
			else if ( ady >= adx * 3 )
			{
				if ( dy > 0 )
					return Direction.South;
				else
					return Direction.North;
			}
			else if ( dx > 0 )
			{
				if ( dy > 0 )
					return Direction.Down;
				else
					return Direction.Right;
			}
			else
			{
				if ( dy > 0 )
					return Direction.Left;
				else
					return Direction.Up;
			}
		}

		public static bool CanMobileFit( int z, Tile[] tiles )
		{
			int checkHeight = 15;
			int checkZ = z;

			for ( int i = 0; i < tiles.Length; ++i )
			{
				Tile tile = tiles[i];

				if ( ( ( checkZ + checkHeight ) > tile.Z && checkZ < ( tile.Z + tile.Height ) )/* || (tile.Z < (checkZ + checkHeight) && (tile.Z + tile.Height) > checkZ)*/ )
				{
					return false;
				}
				else if ( checkHeight == 0 && tile.Height == 0 && checkZ == tile.Z )
				{
					return false;
				}
			}

			return true;
		}

		public static bool IsInContact( Tile check, Tile[] tiles )
		{
			int checkHeight = check.Height;
			int checkZ = check.Z;

			for ( int i = 0; i < tiles.Length; ++i )
			{
				Tile tile = tiles[i];

				if ( ( ( checkZ + checkHeight ) > tile.Z && checkZ < ( tile.Z + tile.Height ) )/* || (tile.Z < (checkZ + checkHeight) && (tile.Z + tile.Height) > checkZ)*/ )
				{
					return true;
				}
				else if ( checkHeight == 0 && tile.Height == 0 && checkZ == tile.Z )
				{
					return true;
				}
			}

			return false;
		}

		public static object GetArrayCap( Array array, int index )
		{
			return GetArrayCap( array, index, null );
		}

		public static object GetArrayCap( Array array, int index, object emptyValue )
		{
			if ( array.Length > 0 )
			{
				if ( index < 0 )
				{
					index = 0;
				}
				else if ( index >= array.Length )
				{
					index = array.Length - 1;
				}

				return array.GetValue( index );
			}
			else
			{
				return emptyValue;
			}
		}

		//4d6+8 would be: Utility.Dice( 4, 6, 8 )
		public static int Dice( int numDice, int numSides, int bonus )
		{
			int total = 0;
			for ( int i = 0; i < numDice; ++i )
				total += Random( numSides ) + 1;
			total += bonus;
			return total;
		}

		public static string RandomList( params string[] list )
		{
			return list[RandomGenerator.Next( list.Length )];
		}

		public static int RandomList( params int[] list )
		{
			return list[RandomGenerator.Next( list.Length )];
		}

		public static T RandomList<T>( params T[] list )
		{
			return list[RandomGenerator.Next( list.Length )];
		}

		public static bool RandomBool()
		{
			return ( RandomGenerator.Next( 2 ) == 0 );
		}

		public static int RandomMinMax( int min, int max )
		{
			if ( min > max )
			{
				int copy = min;
				min = max;
				max = copy;
			}
			else if ( min == max )
			{
				return min;
			}

			return min + RandomGenerator.Next( ( max - min ) + 1 );
		}

		public static int Random( int from, int count )
		{
			if ( count == 0 )
			{
				return from;
			}
			else if ( count > 0 )
			{
				return from + RandomGenerator.Next( count );
			}
			else
			{
				return from - RandomGenerator.Next( -count );
			}
		}

		public static int Random( int count )
		{
			return RandomGenerator.Next( count );
		}

		#region FixValues
		public static void FixMin( ref int value, int min )
		{
			if ( value < min )
				value = min;
		}

		public static void FixMin( ref double value, double min )
		{
			if ( value < min )
				value = min;
		}

		public static void FixMax( ref int value, int max )
		{
			if ( value > max )
				value = max;
		}

		public static void FixMax( ref double value, double max )
		{
			if ( value > max )
				value = max;
		}

		public static void FixMinMax( ref int value, int min, int max )
		{
			FixMin( ref value, min );
			FixMax( ref value, max );
		}

		public static void FixMinMax( ref double value, double min, double max )
		{
			FixMin( ref value, min );
			FixMax( ref value, max );
		}
		#endregion

		#region Random Hues

		public static int RandomNondyedHue()
		{
			switch ( Random( 6 ) )
			{
				case 0:
					return RandomPinkHue();
				case 1:
					return RandomBlueHue();
				case 2:
					return RandomGreenHue();
				case 3:
					return RandomOrangeHue();
				case 4:
					return RandomRedHue();
				case 5:
					return RandomYellowHue();
			}

			return 0;
		}

		public static int RandomPinkHue()
		{
			return Random( 1201, 54 );
		}

		public static int RandomBlueHue()
		{
			return Random( 1301, 54 );
		}

		public static int RandomGreenHue()
		{
			return Random( 1401, 54 );
		}

		public static int RandomOrangeHue()
		{
			return Random( 1501, 54 );
		}

		public static int RandomRedHue()
		{
			return Random( 1601, 54 );
		}

		public static int RandomYellowHue()
		{
			return Random( 1701, 54 );
		}

		public static int RandomNeutralHue()
		{
			return Random( 1801, 108 );
		}

		public static int RandomSnakeHue()
		{
			return Random( 2001, 18 );
		}

		public static int RandomBirdHue()
		{
			return Random( 2101, 30 );
		}

		public static int RandomSlimeHue()
		{
			return Random( 2201, 24 );
		}

		public static int RandomAnimalHue()
		{
			return Random( 2301, 18 );
		}

		public static int RandomMetalHue()
		{
			return Random( 2401, 30 );
		}

		public static int ClipDyedHue( int hue )
		{
			if ( hue < 2 )
				return 2;
			else if ( hue > 1001 )
				return 1001;
			else
				return hue;
		}

		public static int RandomDyedHue()
		{
			return Random( 2, 1000 );
		}

		public static int ClipSkinHue( int hue )
		{
			if ( hue < 1002 )
				return 1002;
			else if ( hue > 1058 )
				return 1058;
			else
				return hue;
		}

		public static int RandomSkinHue()
		{
			return Random( 1002, 57 ) | 0x8000;
		}

		public static int ClipHairHue( int hue )
		{
			if ( hue < 1102 )
				return 1102;
			else if ( hue > 1149 )
				return 1149;
			else
				return hue;
		}

		public static int RandomHairHue()
		{
			return Random( 1102, 48 );
		}

		#endregion

		private static readonly SkillName[] m_AllSkills = {
				SkillName.Alchemy,
				SkillName.Anatomy,
				SkillName.AnimalLore,
				SkillName.ItemID,
				SkillName.ArmsLore,
				SkillName.Parry,
				SkillName.Begging,
				SkillName.Blacksmith,
				SkillName.Fletching,
				SkillName.Peacemaking,
				SkillName.Camping,
				SkillName.Carpentry,
				SkillName.Cartography,
				SkillName.Cooking,
				SkillName.DetectHidden,
				SkillName.Discordance,
				SkillName.EvalInt,
				SkillName.Healing,
				SkillName.Fishing,
				SkillName.Forensics,
				SkillName.Herding,
				SkillName.Hiding,
				SkillName.Provocation,
				SkillName.Inscribe,
				SkillName.Lockpicking,
				SkillName.Magery,
				SkillName.MagicResist,
				SkillName.Tactics,
				SkillName.Snooping,
				SkillName.Musicianship,
				SkillName.Poisoning,
				SkillName.Archery,
				SkillName.SpiritSpeak,
				SkillName.Stealing,
				SkillName.Tailoring,
				SkillName.AnimalTaming,
				SkillName.TasteID,
				SkillName.Tinkering,
				SkillName.Tracking,
				SkillName.Veterinary,
				SkillName.Swords,
				SkillName.Macing,
				SkillName.Fencing,
				SkillName.Wrestling,
				SkillName.Lumberjacking,
				SkillName.Mining,
				SkillName.Meditation,
				SkillName.Stealth,
				SkillName.RemoveTrap,
				SkillName.Necromancy,
				SkillName.Focus,
				SkillName.Chivalry,
				SkillName.Bushido,
				SkillName.Ninjitsu,
				SkillName.Spellweaving,
				SkillName.Throwing,
				SkillName.Mysticism,
				SkillName.Imbuing
			};

		private static readonly SkillName[] m_CombatSkills = {
				SkillName.Archery,
				SkillName.Swords,
				SkillName.Macing,
				SkillName.Fencing,
				SkillName.Wrestling
			};

		private static readonly SkillName[] m_CraftSkills = {
				SkillName.Alchemy,
				SkillName.Blacksmith,
				SkillName.Fletching,
				SkillName.Carpentry,
				SkillName.Cartography,
				SkillName.Cooking,
				SkillName.Inscribe,
				SkillName.Tailoring,
				SkillName.Tinkering
			};

		public static SkillName RandomSkill()
		{
			return m_AllSkills[Utility.Random( m_AllSkills.Length )];
		}

		public static SkillName RandomCombatSkill()
		{
			return m_CombatSkills[Utility.Random( m_CombatSkills.Length )];
		}

		public static SkillName RandomCraftSkill()
		{
			return m_CraftSkills[Utility.Random( m_CraftSkills.Length )];
		}

		public static void FixPoints( ref Point3D top, ref Point3D bottom )
		{
			if ( bottom.X < top.X )
			{
				int swap = top.X;
				top.X = bottom.X;
				bottom.X = swap;
			}

			if ( bottom.Y < top.Y )
			{
				int swap = top.Y;
				top.Y = bottom.Y;
				bottom.Y = swap;
			}

			if ( bottom.Z < top.Z )
			{
				int swap = top.Z;
				top.Z = bottom.Z;
				bottom.Z = swap;
			}
		}

		public static bool RangeCheck( IPoint2D p1, IPoint2D p2, int range )
		{
			return ( p1.X >= ( p2.X - range ) )
				&& ( p1.X <= ( p2.X + range ) )
				&& ( p1.Y >= ( p2.Y - range ) )
				&& ( p2.Y <= ( p2.Y + range ) );
		}

		public static void FormatBuffer( TextWriter output, Stream input, int length )
		{
			output.WriteLine( "        0  1  2  3  4  5  6  7   8  9  A  B  C  D  E  F" );
			output.WriteLine( "       -- -- -- -- -- -- -- --  -- -- -- -- -- -- -- --" );

			int byteIndex = 0;

			int whole = length >> 4;
			int rem = length & 0xF;

			for ( int i = 0; i < whole; ++i, byteIndex += 16 )
			{
				StringBuilder bytes = new StringBuilder( 49 );
				StringBuilder chars = new StringBuilder( 16 );

				for ( int j = 0; j < 16; ++j )
				{
					int c = input.ReadByte();

					bytes.Append( c.ToString( "X2" ) );

					if ( j != 7 )
					{
						bytes.Append( ' ' );
					}
					else
					{
						bytes.Append( "  " );
					}

					if ( c >= 0x20 && c < 0x80 )
					{
						chars.Append( (char) c );
					}
					else
					{
						chars.Append( '.' );
					}
				}

				output.Write( byteIndex.ToString( "X4" ) );
				output.Write( "   " );
				output.Write( bytes.ToString() );
				output.Write( "  " );
				output.WriteLine( chars.ToString() );
			}

			if ( rem != 0 )
			{
				StringBuilder bytes = new StringBuilder( 49 );
				StringBuilder chars = new StringBuilder( rem );

				for ( int j = 0; j < 16; ++j )
				{
					if ( j < rem )
					{
						int c = input.ReadByte();

						bytes.Append( c.ToString( "X2" ) );

						if ( j != 7 )
						{
							bytes.Append( ' ' );
						}
						else
						{
							bytes.Append( "  " );
						}

						if ( c >= 0x20 && c < 0x80 )
						{
							chars.Append( (char) c );
						}
						else
						{
							chars.Append( '.' );
						}
					}
					else
					{
						bytes.Append( "   " );
					}
				}

				output.Write( byteIndex.ToString( "X4" ) );
				output.Write( "   " );
				output.Write( bytes.ToString() );
				output.Write( "  " );
				output.WriteLine( chars.ToString() );
			}
		}

		#region ConsoleColor
		private static readonly Stack<ConsoleColor> m_ConsoleColors = new Stack<ConsoleColor>();

		public static void PushColor( ConsoleColor color )
		{
			try
			{
				m_ConsoleColors.Push( Console.ForegroundColor );
				Console.ForegroundColor = color;
			}
			catch
			{
			}
		}

		public static void PopColor()
		{
			try
			{
				Console.ForegroundColor = m_ConsoleColors.Pop();
			}
			catch
			{
			}
		}
		#endregion

		public static bool NumberBetween( double num, int bound1, int bound2, double allowance )
		{
			if ( bound1 > bound2 )
			{
				int i = bound1;
				bound1 = bound2;
				bound2 = i;
			}

			return ( num < bound2 + allowance && num > bound1 - allowance );
		}

		public static double GetDistanceToSqrt( Point3D p1, Point3D p2 )
		{
			int xDelta = p1.X - p2.X;
			int yDelta = p1.Y - p2.Y;

			return Math.Sqrt( ( xDelta * xDelta ) + ( yDelta * yDelta ) );
		}

		public static bool IsUsingMulticlient( NetState state, int maxclients )
		{
			if ( Core.Config.Login.MaxLoginsPerPC <= 0 )
				return false;

			IPAddress ippublic = state.Address;
			IPAddress iplocal = state.ClientAddress;

			int count = 0;

			foreach ( var compState in GameServer.Instance.Clients )
			{
				if ( ippublic.Equals( compState.Address ) && iplocal.Equals( compState.ClientAddress ) )
				{
					++count;

					if ( count > maxclients )
						return true;
				}
			}

			return false;
		}

		#region Virtual Hair
		public static void AssignRandomHair( Mobile m )
		{
			AssignRandomHair( m, true );
		}

		public static void AssignRandomHair( Mobile m, int hue )
		{
			m.HairItemID = m.Race.RandomHair( m );
			m.HairHue = hue;
		}

		public static void AssignRandomHair( Mobile m, bool randomHue )
		{
			m.HairItemID = m.Race.RandomHair( m );

			if ( randomHue )
				m.HairHue = m.Race.RandomHairHue();
		}

		public static void AssignRandomFacialHair( Mobile m )
		{
			AssignRandomFacialHair( m, true );
		}

		public static void AssignRandomFacialHair( Mobile m, int hue )
		{
			m.FacialHairHue = m.Race.RandomFacialHair( m );
			m.FacialHairHue = hue;
		}

		public static void AssignRandomFacialHair( Mobile m, bool randomHue )
		{
			m.FacialHairItemID = m.Race.RandomFacialHair( m );

			if ( randomHue )
				m.FacialHairHue = m.Race.RandomHairHue();
		}
		#endregion

		public static T[] Shuffle<T>( this T[] source )
		{
			return Shuffle( source, 0, source.Length );
		}

		public static T[] Shuffle<T>( this T[] source, int index, int length )
		{
			T[] sorted = new T[source.Length];
			byte[] randoms = new byte[sorted.Length];

			source.CopyTo( sorted, 0 );

			RandomGenerator.NextBytes( randoms );
			Array.Sort( randoms, sorted, index, length );

			return sorted;
		}

		public static String RemoveHtml( String str )
		{
			return str.Replace( "<", "" ).Replace( ">", "" ).Trim();
		}

		public static bool IsNumeric( String str )
		{
			return !Regex.IsMatch( str, "[^0-9]" );
		}

		public static bool IsAlpha( String str )
		{
			return !Regex.IsMatch( str, "[^a-z]", RegexOptions.IgnoreCase );
		}

		public static bool IsAlphaNumeric( String str )
		{
			return !Regex.IsMatch( str, "[^a-z0-9]", RegexOptions.IgnoreCase );
		}

		public static void SendPacket( this IEnumerable<Mobile> mobiles, Packet p )
		{
			p.Acquire();

			try
			{
				foreach ( var m in mobiles )
				{
					m.Send( p );
				}
			}
			finally
			{
				p.Release();
			}
		}

		public static void Each<T>( this IEnumerable<T> eable, Action<T> action )
		{
			foreach ( var obj in eable )
			{
				action( obj );
			}
		}

		public static int ToUnixTime( this DateTime value )
		{
			return (int) ( value - new DateTime( 1970, 1, 1 ).ToLocalTime() ).TotalSeconds;
		}

		public static string FormatByteAmount( long totalBytes )
		{
			if ( totalBytes > 1000000000 )
				return String.Format( "{0:F1} GB", (double) totalBytes / 1073741824 );

			if ( totalBytes > 1000000 )
				return String.Format( "{0:F1} MB", (double) totalBytes / 1048576 );

			if ( totalBytes > 1000 )
				return String.Format( "{0:F1} KB", (double) totalBytes / 1024 );

			return String.Format( "{0} Bytes", totalBytes );
		}

		public static T[] GetCustomAttributes<T>( this MemberInfo info, bool inherit ) where T : Attribute
		{
			return (T[]) info.GetCustomAttributes( typeof( T ), inherit );
		}

		public static IEnumerator<T> GetEnumerator<T>( this T[] array )
		{
			return array.AsEnumerable().GetEnumerator();
		}
	}
}