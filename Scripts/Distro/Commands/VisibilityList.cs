using System;
using System.Collections;
using Server;
using Server.Mobiles;
using Server.Targeting;
using Server.Network;
using Server.Events;

namespace Server.Scripts.Commands
{
	public class VisibilityList
	{
		public static void Initialize()
		{
			EventSink.Instance.Login += new LoginEventHandler( OnLogin );
			CommandSystem.Register( "Vis", AccessLevel.Counselor, new CommandEventHandler( Vis_OnCommand ) );
			CommandSystem.Register( "VisList", AccessLevel.Counselor, new CommandEventHandler( VisList_OnCommand ) );
			CommandSystem.Register( "VisClear", AccessLevel.Counselor, new CommandEventHandler( VisClear_OnCommand ) );
		}

		public static void OnLogin( LoginEventArgs e )
		{
			if ( e.Mobile is PlayerMobile )
			{
				PlayerMobile pm = (PlayerMobile) e.Mobile;

				pm.VisibilityList.Clear();
			}
		}

		[Usage( "Vis" )]
		[Description( "Adds or removes a targeted player from your visibility list.  Anyone on your visibility list will be able to see you at all times, even when you're hidden." )]
		public static void Vis_OnCommand( CommandEventArgs e )
		{
			if ( e.Mobile is PlayerMobile )
			{
				e.Mobile.Target = new VisTarget();
				e.Mobile.SendMessage( "Select person to add or remove from your visibility list." );
			}
		}

		[Usage( "VisList" )]
		[Description( "Shows the names of everyone in your visibility list." )]
		public static void VisList_OnCommand( CommandEventArgs e )
		{
			if ( e.Mobile is PlayerMobile )
			{
				PlayerMobile pm = (PlayerMobile) e.Mobile;
				ArrayList list = pm.VisibilityList;

				if ( list.Count > 0 )
				{
					pm.SendMessage( "You are visible to {0} mobile{1}:", list.Count, list.Count == 1 ? "" : "s" );

					for ( int i = 0; i < list.Count; ++i )
					{
						pm.SendMessage( "#{0}: {1}", i + 1, ( (Mobile) list[i] ).Name );
					}
				}
				else
				{
					pm.SendMessage( "Your visibility list is empty." );
				}
			}
		}

		[Usage( "VisClear" )]
		[Description( "Removes everyone from your visibility list." )]
		public static void VisClear_OnCommand( CommandEventArgs e )
		{
			if ( e.Mobile is PlayerMobile )
			{
				PlayerMobile pm = (PlayerMobile) e.Mobile;
				ArrayList list = new ArrayList( pm.VisibilityList );

				pm.VisibilityList.Clear();
				pm.SendMessage( "Your visibility list has been cleared." );

				for ( int i = 0; i < list.Count; ++i )
				{
					Mobile m = (Mobile) list[i];

					if ( !m.CanSee( pm ) && m.InUpdateRange( pm ) )
					{
						m.Send( pm.RemovePacket );
					}
				}
			}
		}

		private class VisTarget : Target
		{
			public VisTarget()
				: base( -1, false, TargetFlags.None )
			{
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				if ( from is PlayerMobile && targeted is Mobile )
				{
					PlayerMobile pm = (PlayerMobile) from;
					Mobile targ = (Mobile) targeted;

					if ( targ.AccessLevel <= from.AccessLevel )
					{
						ArrayList list = pm.VisibilityList;

						if ( list.Contains( targ ) )
						{
							list.Remove( targ );
							from.SendMessage( "{0} has been removed from your visibility list.", targ.Name );
						}
						else
						{
							list.Add( targeted );
							from.SendMessage( "{0} has been added to your visibility list.", targ.Name );
						}

						if ( targ.InUpdateRange( from ) )
						{
							if ( targ.CanSee( from ) )
							{
								targ.Send( new Network.MobileIncoming( targ, from ) );

								if ( ObjectPropertyListPacket.Enabled )
								{
									targ.Send( from.OPLPacket );

									foreach ( var item in from.GetEquippedItems() )
									{
										targ.Send( item.OPLPacket );
									}
								}
							}
							else
							{
								targ.Send( from.RemovePacket );
							}
						}
					}
					else
					{
						from.SendMessage( "They can already see you!" );
					}
				}
				else
				{
					from.SendMessage( "Add only mobiles to your visibility list." );
				}
			}
		}
	}
}