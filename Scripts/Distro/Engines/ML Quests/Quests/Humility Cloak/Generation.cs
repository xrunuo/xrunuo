using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Gumps;

namespace Server.Engines.Quests.HumilityCloak
{
	public class Generator
	{
		#region Generation
		public static void Initialize()
		{
			CommandSystem.Register( "GenHumilityQuest", AccessLevel.Developer, new CommandEventHandler( GenQuest_Command ) );
		}

		[Usage( "GenHumilityQuest" )]
		[Description( "Generates all needed items and mobiles for Humility Cloak Quest on Trammel and Felucca." )]
		private static void GenQuest_Command( CommandEventArgs e )
		{
			e.Mobile.SendMessage( "Creating Humility Cloak Quest..." );

			// var declarations
			CreatureSpawner spawner;

			// Gareth
			spawner = new CreatureSpawner( "Gareth", 1, 300, 600, 5, 1 );
			spawner.MoveToWorld( new Point3D( 2024, 2838, 20 ), Map.Trammel );
			spawner.Active = true;

			// Maribel
			spawner = new CreatureSpawner( "Maribel", 1, 300, 600, 5, 1 );
			spawner.MoveToWorld( new Point3D( 1460, 1657, 10 ), Map.Trammel );
			spawner.Active = true;

			// Deirdre
			spawner = new CreatureSpawner( "Deirdre", 1, 300, 600, 5, 1 );
			spawner.MoveToWorld( new Point3D( 1460, 1657, 10 ), Map.Felucca );
			spawner.Active = true;

			// Jason
			spawner = new CreatureSpawner( "Jason", 1, 300, 600, 5, 1 );
			spawner.MoveToWorld( new Point3D( 607, 2170, 0 ), Map.Trammel );
			spawner.Active = true;

			// Walton
			spawner = new CreatureSpawner( "Walton", 1, 300, 600, 5, 1 );
			spawner.MoveToWorld( new Point3D( 607, 2170, 0 ), Map.Felucca );
			spawner.Active = true;

			// Nelson
			spawner = new CreatureSpawner( "Nelson", 1, 300, 600, 5, 1 );
			spawner.MoveToWorld( new Point3D( 3442, 2637, 28 ), Map.Trammel );
			spawner.Active = true;

			// Kevin
			spawner = new CreatureSpawner( "Kevin", 1, 300, 600, 5, 1 );
			spawner.MoveToWorld( new Point3D( 2468, 474, 15 ), Map.Trammel );
			spawner.Active = true;

			// Sean
			spawner = new CreatureSpawner( "Sean", 1, 300, 600, 5, 1 );
			spawner.MoveToWorld( new Point3D( 2468, 474, 15 ), Map.Felucca );
			spawner.Active = true;

			// Triggers!
			HumilityCloakTrigger trigger;

			trigger = new HumilityCloakTrigger();
			trigger.MoveToWorld( new Point3D( 4273, 3697, 0 ), Map.Trammel );
			trigger = new HumilityCloakTrigger();
			trigger.MoveToWorld( new Point3D( 4274, 3697, 0 ), Map.Trammel );
			trigger = new HumilityCloakTrigger();
			trigger.MoveToWorld( new Point3D( 4273, 3696, 0 ), Map.Trammel );
			trigger = new HumilityCloakTrigger();
			trigger.MoveToWorld( new Point3D( 4274, 3696, 0 ), Map.Trammel );

			trigger = new HumilityCloakTrigger();
			trigger.MoveToWorld( new Point3D( 4273, 3697, 0 ), Map.Felucca );
			trigger = new HumilityCloakTrigger();
			trigger.MoveToWorld( new Point3D( 4274, 3697, 0 ), Map.Felucca );
			trigger = new HumilityCloakTrigger();
			trigger.MoveToWorld( new Point3D( 4273, 3696, 0 ), Map.Felucca );
			trigger = new HumilityCloakTrigger();
			trigger.MoveToWorld( new Point3D( 4274, 3696, 0 ), Map.Felucca );

			// Ilshenar WON'T work! :)

			e.Mobile.SendMessage( "Generation completed!" );
		}
		#endregion
	}
}