using System;
using Server;
using Server.Commands;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Gumps;

namespace Server
{
	public class KRStartingQuest
	{
		#region Generation
		public static void Initialize()
		{
			CommandSystem.Register( "GenKRStartingQuest", AccessLevel.Developer, new CommandEventHandler( GenQuest_Command ) );
		}

		[Usage( "GenKRStartingQuest" )]
		[Description( "Generates all needed items for KR Starting Quest on Trammel Old Haven." )]
		private static void GenQuest_Command( CommandEventArgs e )
		{
			e.Mobile.SendMessage( "Creating KR Starting Quest..." );

			// var declarations
			KRStartingQuestTrigger trigger;
			KRStartingQuestContainer container;
			KRStartingQuestGate gate;
			KRWaypointRemover remover;
			KRStartingQuestTeleporter teleporter;
			CreatureSpawner spawner;

			trigger = new KRStartingQuestTrigger( 3 );
			trigger.MoveToWorld( new Point3D( 3646, 2674, -1 ), Map.Trammel );
			trigger = new KRStartingQuestTrigger( 3 );
			trigger.MoveToWorld( new Point3D( 3647, 2674, -2 ), Map.Trammel );
			trigger = new KRStartingQuestTrigger( 3 );
			trigger.MoveToWorld( new Point3D( 3648, 2674, -2 ), Map.Trammel );
			trigger = new KRStartingQuestTrigger( 3 );
			trigger.MoveToWorld( new Point3D( 3649, 2674, -2 ), Map.Trammel );

			trigger = new KRStartingQuestTrigger( 4 );
			trigger.MoveToWorld( new Point3D( 3647, 2666, -3 ), Map.Trammel );
			trigger = new KRStartingQuestTrigger( 4 );
			trigger.MoveToWorld( new Point3D( 3648, 2666, -3 ), Map.Trammel );
			trigger = new KRStartingQuestTrigger( 4 );
			trigger.MoveToWorld( new Point3D( 3649, 2666, -2 ), Map.Trammel );

			trigger = new KRStartingQuestTrigger( 5 );
			trigger.MoveToWorld( new Point3D( 3646, 2656, -2 ), Map.Trammel );
			trigger = new KRStartingQuestTrigger( 5 );
			trigger.MoveToWorld( new Point3D( 3647, 2656, -4 ), Map.Trammel );
			trigger = new KRStartingQuestTrigger( 5 );
			trigger.MoveToWorld( new Point3D( 3648, 2656, -3 ), Map.Trammel );

			for ( int i = 0; i < 12; i++ )
			{
				trigger = new KRStartingQuestTrigger( 5 );
				trigger.MoveToWorld( new Point3D( 3649 + i, 2656, -2 ), Map.Trammel );
			}

			container = new KRStartingQuestContainer( 0x9A9 );
			container.MoveToWorld( new Point3D( 3646, 2652, -3 ), Map.Trammel );
			container = new KRStartingQuestContainer( 0xE3E );
			container.MoveToWorld( new Point3D( 3649, 2650, 2 ), Map.Trammel );
			container = new KRStartingQuestContainer( 0xE3D );
			container.MoveToWorld( new Point3D( 3650, 2650, 2 ), Map.Trammel );
			container = new KRStartingQuestContainer( 0xE3D );
			container.MoveToWorld( new Point3D( 3651, 2650, 2 ), Map.Trammel );

			container = new KRStartingQuestContainer( 0xE3D );
			container.MoveToWorld( new Point3D( 3643, 2649, 0 ), Map.Trammel );
			container = new KRStartingQuestContainer( 0xE3D );
			container.MoveToWorld( new Point3D( 3644, 2648, 3 ), Map.Trammel );
			container = new KRStartingQuestContainer( 0xE3D );
			container.MoveToWorld( new Point3D( 3645, 2647, 2 ), Map.Trammel );
			container = new KRStartingQuestContainer( 0xE3D );
			container.MoveToWorld( new Point3D( 3645, 2648, -1 ), Map.Trammel );
			container = new KRStartingQuestContainer( 0xE3D );
			container.MoveToWorld( new Point3D( 3648, 2644, 0 ), Map.Trammel );
			container = new KRStartingQuestContainer( 0xE3D );
			container.MoveToWorld( new Point3D( 3650, 2642, 2 ), Map.Trammel );
			container = new KRStartingQuestContainer( 0xE3D );
			container.MoveToWorld( new Point3D( 3648, 2642, 0 ), Map.Trammel );
			container = new KRStartingQuestContainer( 0xE3D );
			container.MoveToWorld( new Point3D( 3648, 2641, 2 ), Map.Trammel );
			container = new KRStartingQuestContainer( 0xE3D );
			container.MoveToWorld( new Point3D( 3652, 2642, 3 ), Map.Trammel );
			container = new KRStartingQuestContainer( 0xE3D );
			container.MoveToWorld( new Point3D( 3653, 2642, 6 ), Map.Trammel );
			container = new KRStartingQuestContainer( 0xE3D );
			container.MoveToWorld( new Point3D( 3653, 2643, 0 ), Map.Trammel );
			container = new KRStartingQuestContainer( 0xE3D );
			container.MoveToWorld( new Point3D( 3657, 2641, 3 ), Map.Trammel );
			container = new KRStartingQuestContainer( 0xE3D );
			container.MoveToWorld( new Point3D( 3657, 2642, 0 ), Map.Trammel );
			container = new KRStartingQuestContainer( 0xE3D );
			container.MoveToWorld( new Point3D( 3657, 2643, 0 ), Map.Trammel );
			container = new KRStartingQuestContainer( 0xE3D );
			container.MoveToWorld( new Point3D( 3659, 2644, 7 ), Map.Trammel );
			container = new KRStartingQuestContainer( 0xE3D );
			container.MoveToWorld( new Point3D( 3660, 2644, 8 ), Map.Trammel );
			container = new KRStartingQuestContainer( 0xE3D );
			container.MoveToWorld( new Point3D( 3659, 2645, 7 ), Map.Trammel );
			container = new KRStartingQuestContainer( 0xE3D );
			container.MoveToWorld( new Point3D( 3660, 2645, 7 ), Map.Trammel );

			for ( int i = 0; i < 6; i++ )
			{
				trigger = new KRStartingQuestTrigger( 9 );
				trigger.MoveToWorld( new Point3D( 3672, 2654 - i, 0 ), Map.Trammel );
			}

			spawner = new CreatureSpawner( "HogarthTheKeeperOfOldHaven", 1, 300, 600, 0, 0 );
			spawner.MoveToWorld( new Point3D( 3672, 2653, 0 ), Map.Trammel );
			spawner.Active = true;

			Static sta;

			sta = new Static( 0xB8D );
			sta.Movable = false;
			sta.MoveToWorld( new Point3D( 3668, 2647, 0 ), Map.Trammel );
			sta = new Static( 0xB8C );
			sta.Movable = false;
			sta.MoveToWorld( new Point3D( 3668, 2648, 0 ), Map.Trammel );
			sta = new Static( 0xB8D );
			sta.Movable = false;
			sta.MoveToWorld( new Point3D( 3669, 2647, 0 ), Map.Trammel );
			sta = new Static( 0xB8D );
			sta.Movable = false;
			sta.MoveToWorld( new Point3D( 3669, 2648, 0 ), Map.Trammel );
			sta = new Static( 0xB8A );
			sta.Movable = false;
			sta.MoveToWorld( new Point3D( 3670, 2647, 0 ), Map.Trammel );
			sta = new Static( 0xB8B );
			sta.Movable = false;
			sta.MoveToWorld( new Point3D( 3670, 2648, 0 ), Map.Trammel );

			sta = new Static( 0xB8D );
			sta.Movable = false;
			sta.MoveToWorld( new Point3D( 3673, 2652, 0 ), Map.Trammel );
			sta = new Static( 0xB8C );
			sta.Movable = false;
			sta.MoveToWorld( new Point3D( 3673, 2653, 0 ), Map.Trammel );
			sta = new Static( 0xB8D );
			sta.Movable = false;
			sta.MoveToWorld( new Point3D( 3674, 2652, 0 ), Map.Trammel );
			sta = new Static( 0xB8D );
			sta.Movable = false;
			sta.MoveToWorld( new Point3D( 3674, 2653, 0 ), Map.Trammel );
			sta = new Static( 0xB8D );
			sta.Movable = false;
			sta.MoveToWorld( new Point3D( 3675, 2652, 0 ), Map.Trammel );
			sta = new Static( 0xB8D );
			sta.Movable = false;
			sta.MoveToWorld( new Point3D( 3675, 2653, 0 ), Map.Trammel );
			sta = new Static( 0xB8D );
			sta.Movable = false;
			sta.MoveToWorld( new Point3D( 3676, 2652, 0 ), Map.Trammel );
			sta = new Static( 0xB8D );
			sta.Movable = false;
			sta.MoveToWorld( new Point3D( 3676, 2653, 0 ), Map.Trammel );
			sta = new Static( 0xB8A );
			sta.Movable = false;
			sta.MoveToWorld( new Point3D( 3677, 2652, 0 ), Map.Trammel );
			sta = new Static( 0xB8B );
			sta.Movable = false;
			sta.MoveToWorld( new Point3D( 3677, 2653, 0 ), Map.Trammel );

			remover = new KRWaypointRemover( 15 );
			remover.MoveToWorld( new Point3D( 3665, 2631, 0 ), Map.Trammel );
			remover = new KRWaypointRemover( 15 );
			remover.MoveToWorld( new Point3D( 3665, 2630, 0 ), Map.Trammel );
			remover = new KRWaypointRemover( 15 );
			remover.MoveToWorld( new Point3D( 3665, 2629, 0 ), Map.Trammel );
			remover = new KRWaypointRemover( 15 );
			remover.MoveToWorld( new Point3D( 3665, 2628, 0 ), Map.Trammel );

			gate = new KRStartingQuestGate( 16, new Point3D( 3663, 2629, 0 ) );
			gate.ItemID = 0x830;
			gate.MoveToWorld( new Point3D( 3664, 2630, 0 ), Map.Trammel );
			gate = new KRStartingQuestGate( 16, new Point3D( 3663, 2629, 0 ) );
			gate.ItemID = 0x832;
			gate.MoveToWorld( new Point3D( 3664, 2629, 0 ), Map.Trammel );

			spawner = new CreatureSpawner( "WeakSkeleton", 10, 300, 600, 20, 10 );
			spawner.MoveToWorld( new Point3D( 3649, 2623, 0 ), Map.Trammel );
			spawner.Active = true;

			remover = new KRWaypointRemover( 23 );
			remover.MoveToWorld( new Point3D( 3653, 2604, 0 ), Map.Trammel );
			remover = new KRWaypointRemover( 23 );
			remover.MoveToWorld( new Point3D( 3654, 2604, 0 ), Map.Trammel );
			remover = new KRWaypointRemover( 23 );
			remover.MoveToWorld( new Point3D( 3655, 2604, 0 ), Map.Trammel );
			remover = new KRWaypointRemover( 23 );
			remover.MoveToWorld( new Point3D( 3656, 2604, 0 ), Map.Trammel );

			gate = new KRStartingQuestGate( 24, new Point3D( 3654, 2602, 0 ) );
			gate.ItemID = 0x82F;
			gate.MoveToWorld( new Point3D( 3654, 2603, 0 ), Map.Trammel );
			gate = new KRStartingQuestGate( 24, new Point3D( 3654, 2602, 0 ) );
			gate.ItemID = 0x833;
			gate.MoveToWorld( new Point3D( 3655, 2603, 0 ), Map.Trammel );

			spawner = new CreatureSpawner( "Zombie", 20, 300, 600, 20, 10 );
			spawner.MoveToWorld( new Point3D( 3648, 2589, 0 ), Map.Trammel );
			spawner.Active = true;

			remover = new KRWaypointRemover( 25 );
			remover.MoveToWorld( new Point3D( 3623, 2611, 0 ), Map.Trammel );
			remover = new KRWaypointRemover( 25 );
			remover.MoveToWorld( new Point3D( 3623, 2612, 0 ), Map.Trammel );

			spawner = new CreatureSpawner( "Healer", 1, 300, 600, 5, 1 );
			spawner.MoveToWorld( new Point3D( 3619, 2618, 0 ), Map.Trammel );
			spawner.Active = true;

			for ( int i = 0; i < 7; i++ )
			{
				remover = new KRWaypointRemover( 27 );
				remover.MoveToWorld( new Point3D( 3629 + i, 2578, 0 ), Map.Trammel );
			}

			teleporter = new KRStartingQuestTeleporter( 28, new Point3D( 3631, 2573, 0 ) );
			teleporter.AdvanceLevel = true;
			teleporter.MoveToWorld( new Point3D( 3631, 2577, 0 ), Map.Trammel );
			teleporter = new KRStartingQuestTeleporter( 28, new Point3D( 3631, 2573, 0 ) );
			teleporter.AdvanceLevel = true;
			teleporter.MoveToWorld( new Point3D( 3632, 2577, 0 ), Map.Trammel );

			DarkKnight dk = new DarkKnight();
			dk.MoveToWorld( new Point3D( 3631, 2568, 0 ), Map.Trammel );
			dk.Direction = Direction.South;

			Blocker b = new Blocker();
			b.MoveToWorld( new Point3D( 3631, 2576, 0 ), Map.Trammel );
			b = new Blocker();
			b.MoveToWorld( new Point3D( 3632, 2576, 0 ), Map.Trammel );

			Static st = new Static( 0x3946 );
			st.MoveToWorld( new Point3D( 3631, 2576, 0 ), Map.Trammel );
			st = new Static( 0x3946 );
			st.MoveToWorld( new Point3D( 3632, 2576, 0 ), Map.Trammel );

			st = new Static( 0x3946 );
			st.MoveToWorld( new Point3D( 3631, 2565, 0 ), Map.Trammel );
			st = new Static( 0x3946 );
			st.MoveToWorld( new Point3D( 3632, 2565, 0 ), Map.Trammel );

			trigger = new KRStartingQuestTrigger( 31 );
			trigger.MoveToWorld( new Point3D( 3540, 2570, -1 ), Map.Trammel );
			trigger = new KRStartingQuestTrigger( 31 );
			trigger.MoveToWorld( new Point3D( 3540, 2569, 0 ), Map.Trammel );
			trigger = new KRStartingQuestTrigger( 31 );
			trigger.MoveToWorld( new Point3D( 3540, 2568, 0 ), Map.Trammel );
			trigger = new KRStartingQuestTrigger( 31 );
			trigger.MoveToWorld( new Point3D( 3540, 2567, 0 ), Map.Trammel );

			teleporter = new KRStartingQuestTeleporter( 31, new Point3D( 3631, 2561, 0 ) );
			teleporter.MoveToWorld( new Point3D( 3631, 2566, 0 ), Map.Trammel );
			teleporter = new KRStartingQuestTeleporter( 31, new Point3D( 3631, 2561, 0 ) );
			teleporter.MoveToWorld( new Point3D( 3632, 2566, 0 ), Map.Trammel );

			e.Mobile.SendMessage( "Generation completed!" );
		}
		#endregion

		public static void DoStep( PlayerMobile pm )
		{
			switch ( pm.KRStartingQuestStep )
			{
				case 0:
					{
						pm.MoveToWorld( new Point3D( 3503, 2574, 14 ), Map.Trammel );

						break;
					}
				case 1:
					{
						pm.MoveToWorld( new Point3D( 3648, 2678, -2 ), Map.Trammel );

						/*
						 * Introduction
						 *
						 * Greetings, Traveler! Welcome to
						 * the world of Ultima Online.
						 *
						 * I am Gwen. I am here to help
						 * you learn the basic skills
						 * needed to thrive in your travels
						 * to come.
						 *
						 * Left click on the Next button
						 * to continue.
						 */
						pm.SendGump( new KRStartingQuestGump( 1078606, 1077642, true ) );

						pm.PlaySound( 0x505 );

						break;
					}
				case 2:
					{
						/*
						 * Movement
						 *
						 * Movement is controlled with the mouse.
						 *
						 * Do you see the glowing light?
						 *
						 * Right click and hold over it
						 * to walk in that direction.
						 */
						pm.SendGump( new KRStartingQuestGump( 1078235, 1077643, false ) );

						pm.PlaySound( 0x505 );

						pm.Send( new DisplayWaypoint( Serial.MinusOne, new Point3D( 3648, 2674, 0 ), Map.Trammel, WaypointType.QuestDestination, true, 1078266 ) ); // Walk Toward Here

						break;
					}
				case 3:
					{
						pm.Send( new RemoveWaypoint( Serial.MinusOne ) );

						pm.Send( new DisplayWaypoint( Serial.MinusOne, new Point3D( 3648, 2666, 0 ), Map.Trammel, WaypointType.QuestDestination, true, 1078267 ) ); // Run Toward Here

						/*
						 * Movement
						 *
						 * Very good!
						 *
						 * Right click and hold, further
						 * away, to run to the glowing light.
						 */
						pm.SendGump( new KRStartingQuestGump( 1078235, 1077644, false ) );

						pm.PlaySound( 0x505 );

						break;
					}
				case 4:
					{
						pm.Send( new RemoveWaypoint( Serial.MinusOne ) );

						pm.Send( new DisplayWaypoint( Serial.MinusOne, new Point3D( 3648, 2655, 0 ), Map.Trammel, WaypointType.QuestDestination, true, 1078268 ) ); // Pathfind Toward Here

						/*
						 * Movement
						 *
						 * Very good!
						 *
						 * Good!
						 *
						 * Do you see the glowing light?
						 *
						 * Right click and hold over it to
						 * run in that direction.
						 */
						pm.SendGump( new KRStartingQuestGump( 1078235, 1077645, false ) );

						pm.PlaySound( 0x505 );

						break;
					}
				case 5:
					{
						pm.Send( new RemoveWaypoint( Serial.MinusOne ) );

						/*
						 * Inventory
						 *
						 * Alright! Now you have your footing.
						 *
						 * Many things you find in the world,
						 * you will be able to pick up and carry.
						 *
						 * You have been given a backpack to carry
						 * all your belongings in.
						 *
						 * Left click on the Backpack Icon in
						 * the lower right corner of your screen.
						 */
						pm.SendGump( new KRStartingQuestGump( 1078236, 1077646, false ) );

						pm.PlaySound( 0x505 );

						pm.Send( new ToggleHighlightKRUIElement( pm.Serial, 31006, "ToggleInventory", 100000 ) );

						pm.Send( new ContinueHighlightKRUIElement( pm.Serial, 0x7919, pm.Backpack.Serial ) );

						break;
					}
				case 6:
					{
						pm.Send( new ToggleHighlightKRUIElement( pm.Serial, 31006, "ToggleInventory", 0 ) );

						/*
						 * Inventory
						 *
						 * This is where you will carry items.
						 *
						 * Up ahead are some crates. There are
						 * supplies in them you may need.
						 *
						 * To open the crates, mouse over them
						 * until you see their item properties.
						 *
						 * Double Left click on it to see
						 * what's inside.
						 */
						pm.SendGump( new KRStartingQuestGump( 1078236, 1077647, false ) );

						pm.PlaySound( 0x505 );

						break;
					}
				case 7:
					{
						/*
						 * Inventory
						 *
						 * To pick up an object, Left click
						 * and drag it into your backpack
						 * window.
						 *
						 * There are other goodies in the
						 * crates and barrels, go ahead and
						 * grab some for yourself.
						 */
						pm.SendGump( new KRStartingQuestGump( 1078236, 1077648, false ) );

						pm.PlaySound( 0x505 );

						break;
					}
				case 8:
					{
						pm.Send( new DisplayWaypoint( Serial.MinusOne, new Point3D( 3672, 2653, 0 ), Map.Trammel, WaypointType.QuestDestination, true, 1078266 ) ); // Walk Toward Here

						/*
						 * Equipment
						 *
						 * Excellent!
						 *
						 * Now approach Hogarth the Keeper
						 * of Old Haven in the building to
						 * the East of you. He is the Keeper
						 * of Old Haven, and he has a task
						 * for you.
						 *
						 * A glowing light toward the East
						 * marks his location.
						 */
						pm.SendGump( new KRStartingQuestGump( 1078237, 1077649, false ) );

						pm.PlaySound( 0x505 );

						break;
					}
				case 9:
					{
						pm.Send( new RemoveWaypoint( Serial.MinusOne ) );

						/*
						 * Equipment
						 *
						 * To speak with the Keeper, Double
						 * Left click on him.
						 *
						 * Please read the quest text and
						 * accept the quest to proceed.
						 */
						pm.SendGump( new KRStartingQuestGump( 1078237, 1077650, false ) );

						pm.PlaySound( 0x505 );

						break;
					}
				case 10:
					{
						/*
						 * Equipment
						 *
						 * Great! You are on your first quest.
						 *
						 * It's a small one, but I'm sure you
						 * will be rewarded nonetheless.
						 *
						 * Remember to pick up the Worn Katana
						 * and then talk to the Keeper to
						 * complete the quest.
						 */
						pm.SendGump( new KRStartingQuestGump( 1078237, 1077651, false ) );

						pm.PlaySound( 0x505 );

						break;
					}
				case 11:
					{
						/*
						 * Equipment
						 *
						 * You found the Worn Katana.
						 * You need to toggle it as a
						 * quest item.
						 *
						 * Hold down the Shift key, Right
						 * click on yourself, and select
						 * 'Toggle Quest Item' from the
						 * context menu. A 'Targeting
						 * Cursor' will appear.
						 *
						 * Left click on the Worn Katana,
						 * press the Esc key, and then
						 * talk to Hogarth.
						 */
						pm.SendGump( new KRStartingQuestGump( 1078237, 1077652, false ) );

						pm.PlaySound( 0x505 );

						break;
					}
				case 12:
					{
						/*
						 * Equipment
						 *
						 * Great! The Keeper has given you
						 * a weapon. You need to equip it.
						 *
						 * Left click on the Paperdoll icon
						 * on the lower right of your screen.
						 *
						 * Drag the Worn Katana from your
						 * backpack to your hand slot in the
						 * Paperdoll.
						 */
						pm.SendGump( new KRStartingQuestGump( 1078237, 1077653, false ) );

						pm.PlaySound( 0x505 );

						pm.Send( new ToggleHighlightKRUIElement( pm.Serial, 31006, "TogglePaperdoll", 100000 ) );

						break;
					}
				case 13:
					{
						/*
						 * Equipment
						 *
						 * You will use this same method to
						 * put on weapons, armor, and
						 * clothing. The slot has to be
						 * clear of any other items. So if
						 * you are wearing a hat, and want
						 * to put on a helmet, you will
						 * have to take off the hat, first.
						 *
						 * Left click the Next button to
						 * continue.
						 */
						pm.SendGump( new KRStartingQuestGump( 1078237, 1077654, true ) );

						pm.PlaySound( 0x505 );

						pm.Send( new ToggleHighlightKRUIElement( pm.Serial, 31006, "TogglePaperdoll", 0 ) );

						break;
					}
				case 14:
					{
						/*
						 * Navigation
						 *
						 * Alright, looks like you're ready
						 * to go.
						 *
						 * Before you can start your
						 * adventures, you'll have to complete
						 * a few challenges.
						 *
						 * Open your map at the bottom of
						 * the screen.
						 */
						pm.SendGump( new KRStartingQuestGump( 1078238, 1078246, false ) );

						pm.PlaySound( 0x505 );

						pm.Send( new DisplayWaypoint( Serial.MinusOne, new Point3D( 3665, 2630, 0 ), Map.Trammel, WaypointType.QuestDestination, true, 1078266 ) ); // Walk Toward Here

						pm.Send( new ToggleHighlightKRUIElement( pm.Serial, 31006, "ToggleMap", 100000 ) );
						pm.Send( new ContinueHighlightKRUIElement( pm.Serial, 31007, pm.Serial ) );

						break;
					}
				case 15:
					{
						pm.Send( new ToggleHighlightKRUIElement( pm.Serial, 31006, "ToggleMap", 0 ) );

						/*
						 * Navigation
						 *
						 * You'll notice a little marker
						 * on the map. That's a waypoint.
						 * This waypoint shows you where
						 * a gate is.
						 *
						 * Follow the alley towards the
						 * marker, and head through the
						 * gate.
						 *
						 * Once you arrive at the gate,
						 * Double Left click on it.
						 */
						pm.SendGump( new KRStartingQuestGump( 1078238, 1078247, false ) );

						pm.PlaySound( 0x505 );

						break;
					}
				case 16:
					{
						pm.Send( new RemoveWaypoint( Serial.MinusOne ) );

						/*
						 * First Combat
						 *
						 * This is the town of Old Haven,
						 * or what remains of it. The
						 * former residents have been
						 * reanimated from the dead.
						 *
						 * Your first challenge is to
						 * return just one of these
						 * skeletons to the grave.
						 *
						 * Left click on the War/Peace
						 * button on the bar at the
						 * bottom left of your screen.
						 */
						pm.SendGump( new KRStartingQuestGump( 1078239, 1078248, false ) );

						pm.PlaySound( 0x505 );

						pm.Send( new ToggleHighlightKRUIElement( pm.Serial, 31009, "", 100000 ) );

						break;
					}
				case 17:
					{
						pm.Send( new ToggleHighlightKRUIElement( pm.Serial, 31009, "", 0 ) );

						/*
						 * First Combat
						 *
						 * Great, now you are in an
						 * aggressive stance. Perfect
						 * for fighting.
						 *
						 * Now remember to toggle back
						 * into Peace mode before you
						 * try to interact with friendly
						 * townsfolk. They don't usually
						 * take kindly to being attacked.
						 *
						 * Left click on the Next button
						 * to continue.
						 */
						pm.SendGump( new KRStartingQuestGump( 1078239, 1078249, true ) );

						pm.PlaySound( 0x505 );

						break;
					}
				case 18:
					{
						/*
						 * First Combat
						 *
						 * Now, let's slay a skeleton.
						 *
						 * Left click on the skeleton,
						 * and move within reach.
						 *
						 * Once you begin attacking, you
						 * will continue to do so until
						 * the skeleton is laid to rest.
						 *
						 * You may need to kill more than
						 * one skeleton if another
						 * adventurer is also attacking it.
						 */
						pm.SendGump( new KRStartingQuestGump( 1078239, 1078320, false ) );

						pm.PlaySound( 0x505 );

						break;
					}
				case 19:
					{
						/*
						 * First Loot
						 *
						 * You destroyed the skeleton!
						 * Nice work!
						 *
						 * Now you can check it to see if
						 * it was carrying anything of
						 * value.
						 *
						 * Double Left click on its corpse.
						 */
						pm.SendGump( new KRStartingQuestGump( 1078240, 1078251, false ) );

						pm.PlaySound( 0x505 );

						break;
					}
				case 20:
					{
						/*
						 * First Loot
						 *
						 * This is what the poor thing was
						 * carrying, remnants of its life
						 * long past. It's sad, really.
						 *
						 * Anyway, drag all of these items
						 * to your inventory. You can right
						 * click on each item to loot as well.
						 */
						pm.SendGump( new KRStartingQuestGump( 1078240, 1078252, false ) );

						pm.PlaySound( 0x505 );

						break;
					}
				case 21:
					{
						/*
						 * First Loot
						 *
						 * Well done! When you have taken
						 * any damage, it's a good idea to
						 * heal up before going into battle
						 * again.
						 *
						 * You have a Potion of Healing in
						 * your inventory. To drink the
						 * potion, Double Left click on it
						 * and you will drink it.
						 *
						 * Even if you are not injured, try
						 * and drink the potion now.
						 */
						pm.SendGump( new KRStartingQuestGump( 1078240, 1078253, false ) );

						pm.PlaySound( 0x505 );

						break;
					}
				case 22:
					{
						/*
						 * First Loot
						 *
						 * Very good!
						 *
						 * You should have a scroll of
						 * Magic Arrow in your backpack.
						 *
						 * Scrolls, and other items, like
						 * the healing potion or bandages,
						 * can be put into your hotbar on
						 * the bottom left of your screen.
						 *
						 * Drag the scroll of Magic Arrow
						 * from your inventory to an open
						 * slot on the hotbar.
						 */
						pm.SendGump( new KRStartingQuestGump( 1078240, 1078254, false ) );

						pm.PlaySound( 0x505 );

						pm.Send( new ToggleHighlightKRUIElement( pm.Serial, 31005, "", 100000 ) );

						pm.Send( EnableKRHotbar.Enable );

						break;
					}
				case 23:
					{
						/*
						 * First Loot
						 *
						 * Don't use that scroll yet. You
						 * will need it against your next
						 * opponent.
						 *
						 * Let's head to the next waypoint
						 * on your map. Head Northwest,
						 * then North, and follow path to
						 * the East.
						 *
						 * I'm waiting for you to approach
						 * the next gate.
						 */
						pm.SendGump( new KRStartingQuestGump( 1078240, 1078255, false ) );

						pm.PlaySound( 0x505 );

						pm.Send( new DisplayWaypoint( Serial.MinusOne, new Point3D( 3655, 2604, 0 ), Map.Trammel, WaypointType.QuestDestination, true, 1078266 ) ); // Walk Toward Here

						pm.Send( new ToggleHighlightKRUIElement( pm.Serial, 31005, "", 0 ) );

						pm.Send( EnableKRHotbar.Enable );

						break;
					}
				case 24:
					{
						/*
						 * Zombies
						 *
						 * Alright, the zombies beyond this
						 * gate are tough, but you have
						 * proven to be pretty good with a
						 * weapon thus far.
						 *
						 * Head through the gate and take
						 * one of those zombies out.
						 *
						 * When you have a zombie targeted,
						 * Left click on the Magic Arrow
						 * scroll icon in the hotbar to cast
						 * the spell.
						 */
						pm.SendGump( new KRStartingQuestGump( 1078241, 1078256, false ) );

						pm.PlaySound( 0x505 );

						pm.Send( new RemoveWaypoint( Serial.MinusOne ) );

						break;
					}
				case 25:
					{
						/*
						 * The Healer
						 *
						 * Oh No! They got you.
						 *
						 * Death in the world of Sosaria
						 * is not a trivial thing, but
						 * neither is it the end of your
						 * adventures.
						 *
						 * You now exist in a ghostly state,
						 * and cannot interact with the
						 * world of the living.
						 *
						 * The location of the nearest
						 * Healer appears on your map.
						 * Go to the location marked
						 * Resurrection on your map.
						 */
						pm.SendGump( new KRStartingQuestGump( 1078242, 1078257, false ) );

						pm.PlaySound( 0x505 );

						pm.Send( new DisplayWaypoint( Serial.MinusOne, new Point3D( 3623, 2611, 0 ), Map.Trammel, WaypointType.QuestDestination, true, 1078266 ) ); // Walk Toward Here

						break;
					}
				case 26:
					{
						/*
						 * Your Corpse
						 *
						 * Good! You are back among the
						 * living.
						 *
						 * Once you are resurrected you
						 * will have only 15 minutes to
						 * return to your body and collect
						 * your equipment from your corpse.
						 * Your corpse location is now
						 * marked on the map.
						 *
						 * Double Left click on your corpse
						 * to retrieve your items.
						 */
						pm.SendGump( new KRStartingQuestGump( 1078243, 1078258, false ) );

						pm.PlaySound( 0x505 );

						break;
					}
				case 27:
					{
						/*
						 * Your Corpse
						 *
						 * Good job! You have retrieved
						 * your items. Take a look in your
						 * backpack and re-equip your
						 * weapon. You will also find a
						 * small gift from me.
						 *
						 * The Token of Passage will allow
						 * you to get through a gate
						 * protected by an energy field.
						 * I have marked the gate on your
						 * map.
						 *
						 * Head to the gate and pass through
						 * the energy field.
						 */
						pm.SendGump( new KRStartingQuestGump( 1078243, 1078259, false ) );

						pm.PlaySound( 0x505 );

						pm.Send( new DisplayWaypoint( Serial.MinusOne, new Point3D( 3632, 2578, 0 ), Map.Trammel, WaypointType.QuestDestination, true, 1078266 ) ); // Walk Toward Here

						pm.Backpack.DropItem( new TokenOfPassage() );

						break;
					}
				case 28:
					{
						/*
						 * A Challenge
						 *
						 * Uh oh. It's the Dark Knight.
						 * I don't see a way to slip
						 * past him.
						 *
						 * So, it looks like you're
						 * going to have to talk to him.
						 *
						 * Double Left click on the Dark
						 * Knight to speak with him, and
						 * let's hope he's in a good mood.
						 */
						pm.SendGump( new KRStartingQuestGump( 1078244, 1078260, false ) );

						pm.PlaySound( 0x505 );

						break;
					}
				case 29:
					{
						pm.SendGump( new DarkKnightGump() );

						pm.AddToBackpack( new DarkKnightRune() );

						break;
					}
				case 30:
					{
						/*
						 * To New Haven
						 *
						 * This gate will lead you out of
						 * here, and on the road to New
						 * Haven. It's a town of learning,
						 * a place where you will get trained
						 * and learn how to advance in all
						 * of the skills vital to your
						 * prosperity in the world of
						 * Sosaria.
						 *
						 * Follow the path to the West.
						 */
						pm.SendGump( new KRStartingQuestGump( 1078608, 1078261, false ) );

						pm.PlaySound( 0x505 );

						pm.Send( new DisplayWaypoint( Serial.MinusOne, new Point3D( 3540, 2568, 0 ), Map.Trammel, WaypointType.QuestDestination, true, 1078266 ) ); // Walk Toward Here

						break;
					}
				case 31:
					{
						pm.SendGump( new KRStartingQuestGumpGoodbye() );

						pm.PlaySound( 0x505 );

						pm.Send( new RemoveWaypoint( Serial.MinusOne ) );

						pm.FinishKRStartingQuest();

						break;
					}
			}
		}
	}

	#region Gumps
	public class KRStartingQuestGump : Gump
	{
		public override int TypeID { get { return 0xF3E63; } }

		private bool m_NextButton;

		public KRStartingQuestGump( int title, int text, bool nextbutton )
			: base( 150, 50 )
		{
			m_NextButton = nextbutton;

			Intern( "" );

			AddBackground( 0, 0, 2600, 500, 0x1AE );

			AddPage( 0 );

			AddKRHtmlLocalized( 0, 0, 0, 0, title, false, false );
			AddKRHtmlLocalized( 0, 0, 0, 0, 1015313, false, false ); // <center></center>
			AddHtmlLocalized( 30, 70, 390, 210, text, true, true );

			AddButton( 110, 355, 0xFA5, 0xFA7, 0, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 145, 358, 100, 36, 1045007, false, false );

			if ( nextbutton )
			{
				AddButton( 110, 355, 0xFA5, 0xFA7, 1, GumpButtonType.Reply, 0 );
				AddHtmlLocalized( 145, 358, 100, 36, 1045007, false, false );
			}
		}

		public override void OnResponse( NetState sender, RelayInfo info )
		{
			PlayerMobile pm = sender.Mobile as PlayerMobile;

			if ( pm == null )
				return;

			switch ( info.ButtonID )
			{
				case 0:
					{
						pm.SendGump( new KRStartingQuestCancelGump() );

						break;
					}
				case 1:
					{
						if ( m_NextButton )
							pm.KRStartingQuestStep++;

						break;
					}
			}
		}
	}

	public class KRStartingQuestGumpGoodbye : Gump
	{
		public override int TypeID { get { return 0xF3E63; } }

		public KRStartingQuestGumpGoodbye()
			: base( 150, 50 )
		{
			Intern( "" );

			AddBackground( 0, 0, 2600, 500, 0x1AE );

			AddPage( 0 );

			AddKRHtmlLocalized( 0, 0, 0, 0, 1078607, false, false );
			AddKRHtmlLocalized( 0, 0, 0, 0, 1015313, false, false );
			AddHtmlLocalized( 30, 70, 390, 210, 1078262, true, true );
			AddButton( 110, 355, 0xFA5, 0xFA7, 0, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 145, 358, 100, 36, 1045007, false, false );
			AddButton( 110, 355, 0xFA5, 0xFA7, 2, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 145, 358, 100, 36, 1045007, false, false );
		}

		public override void OnResponse( NetState sender, RelayInfo info )
		{
		}
	}

	public class KRStartingQuestCancelGump : Gump
	{
		public override int TypeID { get { return 0xF3E84; } }

		public KRStartingQuestCancelGump()
			: base( 120, 50 )
		{
			Intern( "" );

			AddPage( 0 );

			Closable = false;
			AddImageTiled( 0, 0, 348, 262, 0xA8E );
			AddAlphaRegion( 0, 0, 348, 262 );
			AddImage( 0, 15, 0x27A8 );
			AddImageTiled( 0, 30, 17, 200, 0x27A7 );
			AddImage( 0, 230, 0x27AA );
			AddImage( 15, 0, 0x280C );
			AddImageTiled( 30, 0, 300, 17, 0x280A );
			AddImage( 315, 0, 0x280E );
			AddImage( 15, 244, 0x280C );
			AddImageTiled( 30, 244, 300, 17, 0x280A );
			AddImage( 315, 244, 0x280E );
			AddImage( 330, 15, 0x27A8 );
			AddImageTiled( 330, 30, 17, 200, 0x27A7 );
			AddImage( 330, 230, 0x27AA );
			AddImage( 333, 2, 0x2716 );
			AddImage( 333, 248, 0x2716 );
			AddImage( 2, 248, 0x2716 );
			AddImage( 2, 2, 0x2716 );
			AddHtmlLocalized( 25, 22, 200, 20, 1049004, 0x7D00, false, false );
			AddImage( 25, 40, 0xBBF );
			AddHtmlLocalized( 25, 55, 300, 120, 1078579, 0xFFFFFF, false, false );
			AddRadio( 25, 175, 0x25F8, 0x25FB, true, 1 );
			AddRadio( 25, 210, 0x25F8, 0x25FB, false, 0 );
			AddHtmlLocalized( 60, 180, 280, 20, 1074976, 0xFFFFFF, false, false );
			AddHtmlLocalized( 60, 215, 280, 20, 1074977, 0xFFFFFF, false, false );
			AddButton( 265, 220, 0xF7, 0xF8, 7, GumpButtonType.Reply, 0 );
		}

		public override void OnResponse( NetState sender, RelayInfo info )
		{
			PlayerMobile pm = sender.Mobile as PlayerMobile;

			if ( pm == null )
				return;

			if ( info.ButtonID == 1 )
				pm.KRStartingQuestStep = 0;
			else
				KRStartingQuest.DoStep( pm );
		}
	}
	#endregion

	public class KRStartingQuestContainer : Container
	{
		private const int ItemsMax = 20;

		[Constructable]
		public KRStartingQuestContainer()
			: this( 0x9A9 )
		{
		}

		public KRStartingQuestContainer( int itemID )
			: base( itemID )
		{
			Weight = 2.0;
			Movable = false;

			Timer.DelayCall( TimeSpan.FromSeconds( 30.0 ), TimeSpan.FromSeconds( 30.0 ), new TimerCallback( Restock_Callback ) );
		}

		private void Restock_Callback()
		{
			if ( TotalItems < ItemsMax )
			{
				for ( int i = TotalItems; i < ItemsMax; i++ )
				{
					Item item = null;

					switch ( Utility.RandomMinMax( 1, 11 ) )
					{
						default:
						case 1: item = new Grapes(); break;
						case 2: item = new Ham(); break;
						case 3: item = new CheeseWedge(); break;
						case 4: item = new Muffins(); break;
						case 5: item = new FishSteak(); break;
						case 6: item = new Ribs(); break;
						case 7: item = new CookedBird(); break;
						case 8: item = new Sausage(); break;
						case 9: item = new Apple(); break;
						case 10: item = new Peach(); break;
						case 11: item = new Bandage(); break;
					}

					if ( item != null )
						DropItem( item );
				}
			}
		}

		public override bool IsDecoContainer { get { return false; } }

		public override void OnDoubleClick( Mobile from )
		{
			base.OnDoubleClick( from );

			PlayerMobile pm = (PlayerMobile) from;

			if ( pm == null )
				return;

			pm.CheckKRStartingQuestStep( 7 );
		}

		public KRStartingQuestContainer( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();

			Timer.DelayCall( TimeSpan.FromSeconds( 30.0 ), TimeSpan.FromSeconds( 30.0 ), new TimerCallback( Restock_Callback ) );
		}
	}
}
