using System;
using System.Collections.Generic;
using System.Linq;

using Server;
using Server.Items;
using Server.Events;

namespace Server.Factions
{
	public static class Moonbind
	{
		public static void Initialize()
		{
			EventSink.ServerStarted += new ServerStartedEventHandler( EventSink_ServerStarted );
			EventSink.MapChanged += new MapChangedEventHandler( EventSink_MapChanged );
		}

		private static Dictionary<Item, MoonbindTimer> m_Timers = new Dictionary<Item, MoonbindTimer>();

		private static void EventSink_ServerStarted()
		{
			foreach ( var item in World.Instance.Items )
			{
				if ( item is IFactionArtifact )
					CheckItem( item, null );
			}
		}

		private static void EventSink_MapChanged( MapChangedEventArgs e )
		{
			Item item = e.Entity as Item;

			if ( item == null || !( item is IFactionArtifact ) )
				return;

			CheckItem( item, e.OldMap );
		}

		private static void CheckItem( Item item, Map oldMap )
		{
			if ( item.Map == null || item.Map == Map.Internal )
				return;

			if ( !m_Timers.ContainsKey( item ) )
			{
				MoonbindTimer timer = new MoonbindTimer( item );
				timer.Start();

				m_Timers.Add( item, timer );
			}

			Mobile parent = item.Parent as Mobile;

			if ( parent != null )
			{
				bool sendMessage = oldMap != null && oldMap != Map.Internal && ( oldMap == Map.Felucca || item.Map == Map.Felucca );

				if ( sendMessage )
				{
					if ( item.IsEphemeral() )
						parent.SendLocalizedMessage( 1153088 ); // The power of the moon Felucca no longer strengthens your Faction gear.
					else
						parent.SendLocalizedMessage( 1153087 ); // The power of the moon Felucca strengthens your Faction gear.
				}
			}

			item.InvalidateProperties();
		}

		public static void GetProperties( Item item, ObjectPropertyList list )
		{
			if ( item.IsEphemeral() )
				list.Add( 1153085 ); // Moonbound: Ephemeral
		}

		public static bool IsEphemeral( this Item item )
		{
			return item.Map != Map.Felucca;
		}

		private class MoonbindTimer : Timer
		{
			private static readonly TimeSpan BreakInterval = TimeSpan.FromMinutes( 6.0 );

			private Item m_Item;
			private int m_BreakCounter;

			public MoonbindTimer( Item item )
				: base( TimeSpan.FromSeconds( 1.0 ), TimeSpan.FromSeconds( 1.0 ) )
			{
				m_Item = item;
			}

			protected override void OnTick()
			{
				IDurability d = m_Item as IDurability;

				if ( m_Item.Deleted || d == null || !d.CanLoseDurability )
				{
					m_Timers.Remove( m_Item );
					Stop();
					return;
				}

				if ( m_Item.IsEphemeral() )
				{
					Mobile parent = m_Item.Parent as Mobile;

					if ( parent != null && parent.Combatant != null )
					{
						m_BreakCounter = ( m_BreakCounter + 1 ) % (int) BreakInterval.TotalSeconds;

						if ( m_BreakCounter == 0 )
						{
							d.MaxHitPoints--;

							if ( d.MaxHitPoints == 0 )
								m_Item.Delete();
							if ( d.HitPoints > d.MaxHitPoints )
								d.HitPoints = d.MaxHitPoints;
						}
					}
				}
			}
		}
	}
}
