using System;

using Server;
using Server.Items;

namespace Server.Network
{
	public static class PacketDispatcher
	{
		#region Event Handlers
		public static void Initialize()
		{
			Mobile.Animated += Mobile_Animated;
			Mobile.Damaged += Mobile_Damaged;
			Mobile.Dead += Mobile_Dead;
			Mobile.ItemLifted += Mobile_ItemLifted;
			Mobile.ItemDropped += Mobile_ItemDropped;
			Mobile.LocationChanged += Mobile_LocationChanged;
		}

		private static void Mobile_Animated( Mobile m, AnimatedEventArgs args )
		{
			var action = args.Action;
			var subAction = args.SubAction;

			var map = m.Map;

			if ( map != null )
			{
				m.ProcessDelta();

				Packet p = null;

				foreach ( var state in map.GetClientsInRange( m.Location ) )
				{
					if ( state.Mobile.CanSee( m ) )
					{
						state.Mobile.ProcessDelta();

						if ( p == null )
							p = Packet.Acquire( new MobileAnimation( m, action, subAction ) );

						state.Send( p );
					}
				}

				if ( p != null )
					p.Release();
			}
		}

		private static void Mobile_Damaged( Mobile m, DamagedEventArgs args )
		{
			var amount = args.Amount;
			var from = args.From;

			NetState ourState = m.NetState, theirState = from?.NetState;

			if ( ourState == null )
			{
				var master = m.GetDamageMaster( from );

				if ( master != null )
					ourState = master.NetState;
			}

			if ( theirState == null && from != null )
			{
				var master = from.GetDamageMaster( m );

				if ( master != null )
					theirState = master.NetState;
			}

			if ( amount > 0 && ( ourState != null || theirState != null ) )
			{
				var p = Packet.Acquire( new DamagePacket( m, amount ) );

				if ( ourState != null )
					ourState.Send( p );

				if ( theirState != null && theirState != ourState )
					theirState.Send( p );

				Packet.Release( p );
			}
		}

		private static void Mobile_Dead( Mobile m, DeadEventArgs args )
		{
			var c = args.Corpse;

			var map = m.Map;

			if ( map != null )
			{
				Packet animPacket = null;
				Packet remPacket = null;

				foreach ( var state in map.GetClientsInRange( m.Location ) )
				{
					if ( state != m.NetState )
					{
						if ( animPacket == null )
							animPacket = Packet.Acquire( new DeathAnimation( m, c ) );

						state.Send( animPacket );

						if ( !state.Mobile.CanSee( m ) )
						{
							if ( remPacket == null )
								remPacket = m.RemovePacket;

							state.Send( remPacket );
						}
					}
				}

				Packet.Release( animPacket );
			}
		}

		private static void Mobile_ItemLifted( Mobile m, ItemLiftedEventArgs args )
		{
			var item = args.Item;
			var amount = args.Amount;
			var map = m.Map;
			var root = item.RootParent;

			if ( map != null && ( root == null || root is Item ) )
			{
				Packet p = null;

				foreach ( var ns in map.GetClientsInRange( m.Location ) )
				{
					if ( ns.Mobile != m && ns.Mobile.CanSee( m ) )
					{
						if ( p == null )
						{
							IEntity src;

							if ( root == null )
								src = new DummyEntity( Serial.Zero, item.Location, map );
							else
								src = new DummyEntity( ( (Item) root ).Serial, ( (Item) root ).Location, map );

							p = Packet.Acquire( new DragEffect( src, m, item.ItemID, item.Hue, amount ) );
						}

						ns.Send( p );
					}
				}

				Packet.Release( p );
			}

			var liftSound = item.GetLiftSound( m );

			if ( liftSound != -1 )
				m.Send( GenericPackets.PlaySound( liftSound, m ) );
		}

		private static void Mobile_ItemDropped( Mobile m, ItemDroppedEventArgs args )
		{
			var item = args.Item;
			var bounced = args.Bounced;

			if ( bounced )
				return;

			var map = m.Map;
			var root = item.RootParent;

			if ( map != null && ( root == null || root is Item ) )
			{
				Packet p = null;

				foreach ( var ns in map.GetClientsInRange( m.Location ) )
				{
					if ( ns.Mobile != m && ns.Mobile.CanSee( m ) )
					{
						if ( p == null )
						{
							IEntity trg;

							if ( root == null )
								trg = new DummyEntity( Serial.Zero, item.Location, map );
							else
								trg = new DummyEntity( ( (Item) root ).Serial, ( (Item) root ).Location, map );

							p = Packet.Acquire( new DragEffect( m, trg, item.ItemID, item.Hue, item.Amount ) );
						}

						ns.Send( p );
					}
				}

				Packet.Release( p );
			}
		}

		private static void Mobile_LocationChanged( Mobile from, LocationChangedEventArgs args )
		{
			var oldLocation = args.OldLocation;
			var newLocation = args.NewLocation;
			var isTeleport = args.IsTeleport;

			var map = from.Map;

			if ( map != null )
			{
				// First, send a remove message to everyone who can no longer see us. (inOldRange && !inNewRange)
				Packet removeThis = null;

				foreach ( var ns in map.GetClientsInRange( oldLocation ) )
				{
					if ( ns != from.NetState && !ns.Mobile.InUpdateRange( newLocation ) )
					{
						if ( removeThis == null )
							removeThis = from.RemovePacket;

						ns.Send( removeThis );
					}
				}

				var ourState = from.NetState;

				// Check to see if we are attached to a client
				if ( ourState != null )
				{
					// We are attached to a client, so it's a bit more complex. We need to send new items and people to ourself, and ourself to other clients
					foreach ( var o in map.GetObjectsInRange( newLocation, Mobile.GlobalMaxUpdateRange ) )
					{
						if ( o is Item )
						{
							var item = (Item) o;

							if ( !item.InUpdateRange( oldLocation ) && item.InUpdateRange( newLocation ) && from.CanSee( item ) )
								item.SendInfoTo( ourState );
						}
						else if ( o != from && o is Mobile )
						{
							var m = (Mobile) o;

							if ( !m.InUpdateRange( newLocation ) )
								continue;

							var inOldRange = m.InUpdateRange( oldLocation );

							if ( ( isTeleport || !inOldRange ) && m.NetState != null && m.CanSee( from ) )
							{
								m.NetState.Send( new MobileIncoming( m, from ) );

								if ( from.IsDeadBondedPet )
									m.NetState.Send( new BondedStatus( 0, from.Serial, 1 ) );

								if ( ObjectPropertyListPacket.Enabled )
									m.NetState.Send( from.OPLPacket );
							}

							if ( !inOldRange && from.CanSee( m ) )
							{
								ourState.Send( new MobileIncoming( from, m ) );

								if ( m.IsDeadBondedPet )
									ourState.Send( new BondedStatus( 0, m.Serial, 1 ) );

								if ( ObjectPropertyListPacket.Enabled )
									ourState.Send( m.OPLPacket );
							}
						}
					}
				}
				else
				{
					// We're not attached to a client, so simply send an Incoming
					foreach ( var ns in map.GetClientsInRange( newLocation ) )
					{
						if ( ( isTeleport || !ns.Mobile.InUpdateRange( oldLocation ) ) && ns.Mobile.CanSee( from ) )
						{
							ns.Send( new MobileIncoming( ns.Mobile, from ) );

							if ( from.IsDeadBondedPet )
								ns.Send( new BondedStatus( 0, from.Serial, 1 ) );

							if ( ObjectPropertyListPacket.Enabled )
								ns.Send( from.OPLPacket );
						}
					}
				}
			}
		}
		#endregion

		#region Extension Methods
		public static void SendDamageToAll( this Mobile m, int amount )
		{
			if ( amount < 0 )
				return;

			var map = m.Map;

			if ( map == null )
				return;

			var p = Packet.Acquire( new DamagePacket( m, amount ) );

			foreach ( var ns in map.GetClientsInRange( m.Location ) )
			{
				if ( ns.Mobile.CanSee( m ) )
					ns.Send( p );
			}

			Packet.Release( p );
		}

		public static void ClearScreen( this Mobile from )
		{
			var ns = from.NetState;

			if ( from.Map != null && ns != null )
			{
				foreach ( var o in from.Map.GetObjectsInRange( from.Location, Mobile.GlobalMaxUpdateRange ) )
				{
					if ( o is Mobile )
					{
						var m = (Mobile) o;

						if ( m != from && m.InUpdateRange( from ) )
							ns.Send( m.RemovePacket );
					}
					else if ( o is Item )
					{
						var item = (Item) o;

						if ( item.InUpdateRange( from ) )
							ns.Send( item.RemovePacket );
					}
				}
			}
		}

		public static void SendIncomingPacket( this Mobile m )
		{
			if ( m.Map != null )
			{
				foreach ( var state in m.Map.GetClientsInRange( m.Location ) )
				{
					if ( state.Mobile.CanSee( m ) )
					{
						state.Send( new MobileIncoming( state.Mobile, m ) );

						if ( m.IsDeadBondedPet )
							state.Send( new BondedStatus( 0, m.Serial, 1 ) );

						if ( ObjectPropertyListPacket.Enabled )
							state.Send( m.OPLPacket );
					}
				}
			}
		}

		public static void SendRemovePacket( this Mobile m, bool everyone = true )
		{
			if ( m.Map != null )
			{
				Packet p = null;

				foreach ( var state in m.Map.GetClientsInRange( m.Location ) )
				{
					if ( state != m.NetState && ( everyone || !state.Mobile.CanSee( m ) ) )
					{
						if ( p == null )
							p = m.RemovePacket;

						state.Send( p );
					}
				}
			}
		}

		public static void SendEverything( this Mobile from )
		{
			var ns = from.NetState;

			if ( from.Map != null && ns != null )
			{
				foreach ( var o in from.Map.GetObjectsInRange( from.Location, Mobile.GlobalMaxUpdateRange ) )
				{
					if ( o is Item )
					{
						var item = (Item) o;

						if ( from.CanSee( item ) && from.InRange( item.Location, item.GetUpdateRange( from ) ) )
							item.SendInfoTo( ns );
					}
					else if ( o is Mobile )
					{
						var m = (Mobile) o;

						if ( from.CanSee( m ) && from.InUpdateRange( m ) )
						{
							ns.Send( new MobileIncoming( from, m ) );

							if ( m.IsDeadBondedPet )
								ns.Send( new BondedStatus( 0, m.Serial, 1 ) );

							if ( ObjectPropertyListPacket.Enabled )
								ns.Send( m.OPLPacket );
						}
					}
				}
			}
		}
		#endregion
	}
}
