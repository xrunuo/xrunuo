using System;
using System.Xml;
using System.Collections;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Events;

namespace Server.Regions
{
	public class MazeOfDeath
	{
		public static void Initialize()
		{
			new MazeOfDeath( Map.TerMur, new Rectangle2D( new Point2D( 1057, 990 ), new Point2D( 1064, 1027 ) ) );
			new MazeOfDeath( Map.TerMur, new Rectangle2D( new Point2D( 1057, 1028 ), new Point2D( 1064, 1062 ) ) );
		}

		private Map m_Map;
		private Rectangle2D m_Bounds;
		private BitArray[] m_Traps;

		public Map Map { get { return m_Map; } }
		public Rectangle2D Bounds { get { return m_Bounds; } }

		public MazeOfDeath( Map map, Rectangle2D bounds )
		{
			m_Map = map;
			m_Bounds = bounds;

			m_Traps = new BitArray[bounds.Height - 2];

			for ( int i = 0; i < m_Traps.Length; i++ )
				m_Traps[i] = new BitArray( bounds.Width, true );

			Randomize();

			new MazeOfDeathRegion( this );
		}

		public CompassDirection GetCompassDirection( Point3D loc )
		{
			int i = loc.Y - m_Bounds.Y;
			int j = loc.X - m_Bounds.X;

			CompassDirection dir = CompassDirection.None;

			if ( i > 0 && i < ( m_Bounds.Height - 1 ) )
			{
				if ( j > 0 && !m_Traps[i - 1][j - 1] )
					dir |= CompassDirection.West;

				if ( j < ( m_Bounds.Width - 1 ) && !m_Traps[i - 1][j + 1] )
					dir |= CompassDirection.East;
			}

			if ( i > 0 && ( i == 1 || !m_Traps[i - 2][j] ) )
				dir |= CompassDirection.North;

			if ( i < ( m_Bounds.Height - 1 ) && ( i == m_Bounds.Height - 2 || !m_Traps[i][j] ) )
				dir |= CompassDirection.South;

			return dir;
		}

		public bool HasTrap( Point3D loc )
		{
			int i = loc.Y - m_Bounds.Y;
			int j = loc.X - m_Bounds.X;

			// La primera y la ultima fila nunca se chequean
			if ( i < 1 || i >= ( m_Bounds.Height - 1 ) )
				return false;

			return m_Traps[i - 1][j];
		}

		public void Randomize()
		{
			Array.ForEach<BitArray>( m_Traps, e => e.SetAll( true ) );

			int i = 0;
			int j = Utility.Random( m_Bounds.Width );

			m_Traps[i][j] = false;
			m_Traps[i + 1][j] = false;

			i = 1;

			while ( i < ( m_Bounds.Height - 3 ) )
			{
				bool canLeft = ( j > 0 ) && m_Traps[i][j - 1] && m_Traps[i - 1][j - 1];
				bool canRight = ( j < ( m_Bounds.Width - 1 ) ) && m_Traps[i][j + 1] && m_Traps[i - 1][j + 1];

				switch ( Utility.Random( 3 ) )
				{
					case 0:
						{
							if ( canLeft )
							{
								j--;
								break;
							}
							else
								goto case 2;
						}
					case 1:
						{
							if ( canRight )
							{
								j++;
								break;
							}
							else
								goto case 2;
						}
					case 2:
						{
							i++;
							break;
						}
				}

				m_Traps[i][j] = false;
			}
		}
	}

	public class MazeOfDeathRegion : BaseRegion
	{
		private MazeOfDeath m_Maze;

		public MazeOfDeathRegion( MazeOfDeath maze )
			: base( null, maze.Map, Region.Find( new Point3D( maze.Bounds.Start, 0 ), maze.Map ), maze.Bounds )
		{
			m_Maze = maze;

			Register();
		}

		public override void OnExit( Mobile m )
		{
			m.CloseGump( typeof( CompassGump ) );
		}

		public override void OnLocationChanged( Mobile m, Point3D oldLocation )
		{
			base.OnLocationChanged( m, oldLocation );

			if ( m.IsPlayer )
			{
				if ( m_Maze.HasTrap( m.Location ) )
				{
					switch ( Utility.Random( 5 ) )
					{
						default:
						case 0:
							{
								AOS.Damage( m, Utility.RandomMinMax( 70, 90 ), 0, 100, 0, 0, 0 );

								Effects.SendPacket( m.Location, m.Map, new GraphicalEffect( EffectType.FixedXYZ, Serial.Zero, Serial.Zero, 0x3709, m.Location, m.Location, 10, 30, true, false ) );
								Effects.PlaySound( m.Location, m.Map, 0x54 );

								m.LocalOverheadMessage( MessageType.Regular, 238, 1010524, "" ); // Searing heat scorches thy skin.

								break;
							}
						case 1:
							{
								AOS.Damage( m, Utility.RandomMinMax( 60, 80 ), 0, 0, 100, 0, 0 );

								Effects.PlaySound( m.Location, m.Map, 0x223 );

								m.LocalOverheadMessage( MessageType.Regular, 98, 1010525, "" ); // Pain lances through thee from a sharp metal blade.

								break;
							}
						case 2:
							{
								AOS.Damage( m, Utility.RandomMinMax( 55, 75 ), 0, 0, 0, 0, 100 );

								Effects.SendPacket( m.Location, m.Map, new GraphicalEffect( EffectType.Lightning, m.Serial, Serial.Zero, 0, m.Location, m.Location, 0, 0, false, false ) );
								Effects.SendPacket( m.Location, m.Map, new ParticleEffect( EffectType.FixedFrom, m.Serial, Serial.Zero, 0, m.Location, m.Location, 0, 0, false, false, 0, 0, 0x13A7, 0, 0, m.Serial, (int) Layer.Shoes, 0 ) );
								Effects.PlaySound( m.Location, m.Map, 0x29 );

								m.LocalOverheadMessage( MessageType.Regular, 218, 1010526, "" ); // Lightning arcs through thy body.

								break;
							}
						case 3:
							{
								m.ApplyPoison( m, Poison.Deadly );

								Effects.SendPacket( m.Location, m.Map, new GraphicalEffect( EffectType.FixedXYZ, Serial.Zero, Serial.Zero, 0x113A, m.Location, m.Location, 10, 20, true, false ) );
								Effects.PlaySound( m.Location, m.Map, 0x231 );

								m.LocalOverheadMessage( MessageType.Regular, 68, 1010523, "" ); // A toxic vapor envelops thee.

								break;
							}
						case 4:
							{
								Point3D location1 = m.Location;
								location1.Z = 49;

								Point3D location2 = m.Location;
								location2.Z = -1;

								Effects.SendPacket( m, m.Map, new HuedEffect( EffectType.Moving, Serial.Zero, m.Serial, 0x11B7, location1, location2, 20, 0, true, true, 0, 0 ) );
								Effects.PlaySound( new Point3D( m.X, m.Y, -1 ), m.Map, m.Female ? 0x14B : 0x154 );
								Effects.PlaySound( new Point3D( m.X, m.Y, -1 ), m.Map, 0x307 );

								m.Send( new AsciiMessage( Serial.MinusOne, 0xFFFF, MessageType.Label, 0x66D, 3, "", "A speeding rock hits you in the head!" ) );

								AOS.Damage( m, Utility.RandomMinMax( 60, 80 ), 100, 0, 0, 0, 0 );

								m.SendLocalizedMessage( 502382 ); // You can move!

								if ( Utility.RandomBool() )
								{
									Effects.SendPacket( m, m.Map, new GraphicalEffect( EffectType.FixedXYZ, Serial.Zero, Serial.Zero, 0x36BD, new Point3D( m.X, m.Y, 0 ), new Point3D( m.X, m.Y, 0 ), 20, 10, true, false ) );
									Effects.SendPacket( m, m.Map, new GraphicalEffect( EffectType.FixedFrom, m.Serial, Serial.Zero, 0x36BD, new Point3D( m.X, m.Y, -1 ), new Point3D( m.X, m.Y, -1 ), 20, 10, true, false ) );

									for ( int k = 0; k < 5; k++ )
									{
										Effects.SendPacket( m, m.Map, new GraphicalEffect( EffectType.Moving, Serial.Zero, Serial.Zero, 0x1363 + Utility.Random( 0, 11 ), new Point3D( m.X, m.Y, 0 ), new Point3D( m.X, m.Y, 0 ), 5, 0, false, false ) );

										Effects.PlaySound( new Point3D( m.X, m.Y, -1 ), m.Map, 0x13F );
										Effects.PlaySound( new Point3D( m.X, m.Y, -1 ), m.Map, 0x154 );
									}

									AOS.Damage( m, Utility.RandomMinMax( 40, 60 ), 100, 0, 0, 0, 0 );

									m.Say( "OUCH!" );
									m.Send( new AsciiMessage( Serial.MinusOne, 0xFFFF, MessageType.Label, 0x66D, 3, "", "You are pinned down by the weight of the boulder!!!" ) );
								}

								break;
							}
					}
				}
				else if ( m.Backpack.FindItemByType<GoldenCompass>( false ) != null )
				{
					m.CloseGump( typeof( CompassGump ) );
					m.SendGump( new CompassGump( m_Maze.GetCompassDirection( m.Location ) ) );
				}
			}
		}
	}

	public class UnderworldDeathRegion : BaseRegion
	{
		// ¿A alguien se le ocurre un nombre mejor?

		public UnderworldDeathRegion( XmlElement xml, Map map, Region parent )
			: base( xml, map, parent )
		{
			EventSink.Logout += new LogoutEventHandler( OnLogout );
		}

		protected void OnLogout( LogoutEventArgs e )
		{
			if ( GetMobiles().Contains( e.Mobile ) )
				e.Mobile.MoveToWorld( new Point3D( 1071, 1057, -43 ), Map.TerMur );
		}

		public override void OnEnter( Mobile m )
		{
			if ( m.IsPlayer )
			{
				if ( m.Alive )
				{
					m.Frozen = true;

					// You are filled with a sense of dread and impending doom!
					m.LocalOverheadMessage( MessageType.Regular, 33, 1113580 );

					Timer.DelayCall( TimeSpan.FromSeconds( 2.0 ), new TimerCallback(
						delegate
						{
							if ( m.Backpack.FindItemByType<GoldenCompass>( false ) != null )
							{
								// I better proceed with caution.
								m.LocalOverheadMessage( MessageType.Regular, 946, 1113582 );
							}
							else
							{
								// I might need something to help me navigate through this.
								m.LocalOverheadMessage( MessageType.Regular, 946, 1113581 );
							}

							Timer.DelayCall( TimeSpan.FromSeconds( 2.0 ), new TimerCallback( delegate { m.Frozen = false; } ) );
						}
					) );
				}
				else
				{
					m.MoveToWorld( new Point3D( 1060, 1066, -42 ), Map.TerMur );
				}
			}
		}
	}
}
