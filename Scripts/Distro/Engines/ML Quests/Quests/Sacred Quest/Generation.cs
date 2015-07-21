using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Engines.Quests.SacredQuest
{
	public class Generator
	{
		#region Generation
		public static void Initialize()
		{
			CommandSystem.Register( "GenSacredQuest", AccessLevel.Developer, new CommandEventHandler( GenQuest_Command ) );
		}

		[Usage( "GenSacredQuest" )]
		private static void GenQuest_Command( CommandEventArgs e )
		{
			e.Mobile.SendMessage( "Creating Sacred Quest..." );

			Mobile m;
			Item item;
			KeyFragmentGlobe globe;
			KeyFragmentSpawner spawner;

			// Tomb of Kings Guardians
			m = new AbyssGuardian();
			m.MoveToWorld( new Point3D( 33, 38, 13 ), Map.TerMur );
			m.Direction = Direction.East;
			m = new AbyssGuardian();
			m.MoveToWorld( new Point3D( 42, 38, 13 ), Map.TerMur );
			m.Direction = Direction.West;

			// Underworld Guards
			m = new UnderworldGuard();
			m.MoveToWorld( new Point3D( 1127, 1202, -2 ), Map.TerMur );
			m.Direction = Direction.North;
			m = new UnderworldGuard();
			m.MoveToWorld( new Point3D( 1130, 1202, -2 ), Map.TerMur );
			m.Direction = Direction.North;

			// Garamon
			m = new Garamon();
			m.MoveToWorld( new Point3D( 1128, 1165, -12 ), Map.TerMur );
			m.Direction = Direction.South;

			// Underworld Secret Rooms
			globe = new KeyFragmentGlobe( 0x456 );
			globe.MoveToWorld( new Point3D( 1070, 1197, -40 ), Map.TerMur );
			globe.KeySpawner = spawner = new BlueKeyFragmentSpawner();
			spawner.MoveToWorld( new Point3D( 1070, 1196, -35 ), Map.TerMur );
			item = new GlobeSwitch( globe );
			item.MoveToWorld( new Point3D( 1069, 1193, -32 ), Map.TerMur );

			globe = new KeyFragmentGlobe( 0x455 );
			globe.MoveToWorld( new Point3D( 1004, 1052, -30 ), Map.TerMur );
			globe.KeySpawner = spawner = new RedKeyFragmentSpawner();
			spawner.MoveToWorld( new Point3D( 1004, 1051, -25 ), Map.TerMur );
			item = new GlobeSwitch( globe );
			item.MoveToWorld( new Point3D( 1005, 1044, -22 ), Map.TerMur );

			e.Mobile.SendMessage( "Generation completed!" );
		}
		#endregion
	}
}