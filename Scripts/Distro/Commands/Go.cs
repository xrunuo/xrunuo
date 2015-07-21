using System;
using System.Collections.Generic;
using Server;
using Server.Gumps;
using Server.Items;

namespace Server.Scripts.Commands
{
	public class Go
	{
		public static void Initialize()
		{
			CommandSystem.Register( "Go", AccessLevel.Counselor, new CommandEventHandler( Go_OnCommand ) );
		}

		[Usage( "Go [name | serial | (x y [z]) | (deg min (N | S) deg min (E | W))]" )]
		[Description( "With no arguments, this command brings up the go menu. With one argument, (name), you are moved to that regions \"go location.\" Or, if a numerical value is specified for one argument, (serial), you are moved to that object. Two or three arguments, (x y [z]), will move your character to that location. When six arguments are specified, (deg min (N | S) deg min (E | W)), your character will go to an approximate of those sextant coordinates." )]
		private static void Go_OnCommand( CommandEventArgs e )
		{
			Mobile from = e.Mobile;

			if ( e.Length == 0 )
			{
				GoGump.DisplayTo( from );
			}
			else if ( e.Length == 1 )
			{
				try
				{
					int ser = e.GetInt32( 0 );

					IEntity ent = World.Instance.FindEntity( ser );

					if ( ent is Item )
					{
						Item item = (Item) ent;

						Map map = item.Map;
						Point3D loc = item.GetWorldLocation();

						Mobile owner = item.RootParent as Mobile;

						if ( owner != null && ( owner.Map != null && owner.Map != Map.Internal ) && !from.CanSee( owner ) )
						{
							from.SendMessage( "You can not go to what you can not see." );
							return;
						}
						else if ( owner != null && ( owner.Map == null || owner.Map == Map.Internal ) && owner.Hidden && owner.AccessLevel >= from.AccessLevel )
						{
							from.SendMessage( "You can not go to what you can not see." );
							return;
						}
						else if ( !FixMap( ref map, ref loc, item ) )
						{
							from.SendMessage( "That is an internal item and you cannot go to it." );
							return;
						}

						from.MoveToWorld( loc, map );

						return;
					}
					else if ( ent is Mobile )
					{
						Mobile m = (Mobile) ent;

						Map map = m.Map;
						Point3D loc = m.Location;

						Mobile owner = m;

						if ( owner != null && ( owner.Map != null && owner.Map != Map.Internal ) && !from.CanSee( owner ) )
						{
							from.SendMessage( "You can not go to what you can not see." );
							return;
						}
						else if ( owner != null && ( owner.Map == null || owner.Map == Map.Internal ) && owner.Hidden && owner.AccessLevel >= from.AccessLevel )
						{
							from.SendMessage( "You can not go to what you can not see." );
							return;
						}
						else if ( !FixMap( ref map, ref loc, m ) )
						{
							from.SendMessage( "That is an internal mobile and you cannot go to it." );
							return;
						}

						from.MoveToWorld( loc, map );

						return;
					}
					else
					{
						string name = e.GetString( 0 );
						Map map;

						for ( int i = 0; i < Map.AllMaps.Count; ++i )
						{
							map = Map.AllMaps[i];

							if ( map.MapIndex == 0x7F || map.MapIndex == 0xFF )
								continue;

							if ( Insensitive.Equals( name, map.Name ) )
							{
								from.Map = map;
								return;
							}
						}

						Dictionary<string, Region> list = from.Map.Regions;

						foreach ( KeyValuePair<string, Region> kvp in list )
						{
							Region r = kvp.Value;

							if ( Insensitive.Equals( r.Name, name ) )
							{
								from.Location = new Point3D( r.GoLocation );
								return;
							}
						}

						for ( int i = 0; i < Map.AllMaps.Count; ++i )
						{
							Map m = Map.AllMaps[i];

							if ( m.MapIndex == 0x7F || m.MapIndex == 0xFF || from.Map == m )
								continue;

							foreach ( Region r in m.Regions.Values )
							{
								if ( Insensitive.Equals( r.Name, name ) )
								{
									from.MoveToWorld( r.GoLocation, m );
									return;
								}
							}
						}

						if ( ser != 0 )
							from.SendMessage( "No object with that serial was found." );
						else
							from.SendMessage( "No region with that name was found." );

						return;
					}
				}
				catch
				{
				}

				from.SendMessage( "Region name not found" );
			}
			else if ( e.Length == 2 )
			{
				Map map = from.Map;

				if ( map != null )
				{
					int x = e.GetInt32( 0 ), y = e.GetInt32( 1 );
					int z = map.GetAverageZ( x, y );

					from.Location = new Point3D( x, y, z );
				}
			}
			else if ( e.Length == 3 )
			{
				from.Location = new Point3D( e.GetInt32( 0 ), e.GetInt32( 1 ), e.GetInt32( 2 ) );
			}
			else if ( e.Length == 6 )
			{
				Map map = from.Map;

				if ( map != null )
				{
					Point3D p = Sextant.ReverseLookup( map, e.GetInt32( 3 ), e.GetInt32( 0 ), e.GetInt32( 4 ), e.GetInt32( 1 ), Insensitive.Equals( e.GetString( 5 ), "E" ), Insensitive.Equals( e.GetString( 2 ), "S" ) );

					if ( p != Point3D.Zero )
					{
						from.Location = p;
					}
					else
					{
						from.SendMessage( "Sextant reverse lookup failed." );
					}
				}
			}
			else
			{
				from.SendMessage( "Format: Go [name | serial | (x y [z]) | (deg min (N | S) deg min (E | W)]" );
			}
		}

		private static bool FixMap( ref Map map, ref Point3D loc, Item item )
		{
			if ( map == null || map == Map.Internal )
			{
				Mobile m = item.RootParent as Mobile;

				return ( m != null && FixMap( ref map, ref loc, m ) );
			}

			return true;
		}

		private static bool FixMap( ref Map map, ref Point3D loc, Mobile m )
		{
			if ( map == null || map == Map.Internal )
			{
				map = m.LogoutMap;
				loc = m.LogoutLocation;
			}

			return ( map != null && map != Map.Internal );
		}
	}
}
