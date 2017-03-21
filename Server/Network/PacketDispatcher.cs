//
//  X-RunUO - Ultima Online Server Emulator
//  Copyright (C) 2015 Pedro Pardal
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//

using System;
using System.Linq;

using Server;
using Server.Items;

namespace Server.Network
{
	public static class PacketDispatcher
	{
		#region Event Handlers
		public static void Initialize()
		{
			Mobile.Animated += new AnimatedEventHandler( Mobile_Animated );
			Mobile.Damaged += new DamagedEventHandler( Mobile_Damaged );
			Mobile.Dead += new DeadEventHandler( Mobile_Dead );
			Mobile.ItemLifted += new ItemLiftedEventHandler( Mobile_ItemLifted );
			Mobile.ItemDropped += new ItemDroppedEventHandler( Mobile_ItemDropped );
			Mobile.LocationChanged += new LocationChangedEventHandler( Mobile_LocationChanged );
		}

		private static void Mobile_Animated( Mobile m, AnimatedEventArgs args )
		{
			int action = args.Action;
			int subAction = args.SubAction;

			Map map = m.Map;

			if ( map != null )
			{
				m.ProcessDelta();

				Packet p = null;

				foreach ( NetState state in map.GetClientsInRange( m.Location ) )
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
			int amount = args.Amount;
			Mobile from = args.From;

			NetState ourState = m.NetState, theirState = ( from == null ? null : from.NetState );

			if ( ourState == null )
			{
				Mobile master = m.GetDamageMaster( from );

				if ( master != null )
					ourState = master.NetState;
			}

			if ( theirState == null && from != null )
			{
				Mobile master = from.GetDamageMaster( m );

				if ( master != null )
					theirState = master.NetState;
			}

			if ( amount > 0 && ( ourState != null || theirState != null ) )
			{
				Packet p = Packet.Acquire( new DamagePacket( m, amount ) );

				if ( ourState != null )
					ourState.Send( p );

				if ( theirState != null && theirState != ourState )
					theirState.Send( p );

				Packet.Release( p );
			}
		}

		private static void Mobile_Dead( Mobile m, DeadEventArgs args )
		{
			Container c = args.Corpse;

			Map map = m.Map;

			if ( map != null )
			{
				Packet animPacket = null;
				Packet remPacket = null;

				foreach ( NetState state in map.GetClientsInRange( m.Location ) )
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
			Item item = args.Item;
			int amount = args.Amount;
			Map map = m.Map;
			object root = item.RootParent;

			if ( map != null && ( root == null || root is Item ) )
			{
				Packet p = null;

				foreach ( NetState ns in map.GetClientsInRange( m.Location ) )
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

			int liftSound = item.GetLiftSound( m );

			if ( liftSound != -1 )
				m.Send( GenericPackets.PlaySound( liftSound, m ) );
		}

		private static void Mobile_ItemDropped( Mobile m, ItemDroppedEventArgs args )
		{
			Item item = args.Item;
			bool bounced = args.Bounced;

			if ( bounced )
				return;

			Map map = m.Map;
			object root = item.RootParent;

			if ( map != null && ( root == null || root is Item ) )
			{
				Packet p = null;

				foreach ( NetState ns in map.GetClientsInRange( m.Location ) )
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
			Point3D oldLocation = args.OldLocation;
			Point3D newLocation = args.NewLocation;
			bool isTeleport = args.IsTeleport;

			Map map = from.Map;

			if ( map != null )
			{
				// First, send a remove message to everyone who can no longer see us. (inOldRange && !inNewRange)
				Packet removeThis = null;

				foreach ( NetState ns in map.GetClientsInRange( oldLocation ) )
				{
					if ( ns != from.NetState && !ns.Mobile.InUpdateRange( newLocation ) )
					{
						if ( removeThis == null )
							removeThis = from.RemovePacket;

						ns.Send( removeThis );
					}
				}

				NetState ourState = from.NetState;

				// Check to see if we are attached to a client
				if ( ourState != null )
				{
					// We are attached to a client, so it's a bit more complex. We need to send new items and people to ourself, and ourself to other clients
					foreach ( object o in map.GetObjectsInRange( newLocation, Mobile.GlobalMaxUpdateRange ) )
					{
						if ( o is Item )
						{
							Item item = (Item) o;

							if ( !item.InUpdateRange( oldLocation ) && item.InUpdateRange( newLocation ) && from.CanSee( item ) )
								item.SendInfoTo( ourState );
						}
						else if ( o != from && o is Mobile )
						{
							Mobile m = (Mobile) o;

							if ( !m.InUpdateRange( newLocation ) )
								continue;

							bool inOldRange = m.InUpdateRange( oldLocation );

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
					foreach ( NetState ns in map.GetClientsInRange( newLocation ) )
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

			Map map = m.Map;

			if ( map == null )
				return;

			Packet p = Packet.Acquire( new DamagePacket( m, amount ) );

			foreach ( NetState ns in map.GetClientsInRange( m.Location ) )
			{
				if ( ns.Mobile.CanSee( m ) )
					ns.Send( p );
			}

			Packet.Release( p );
		}

		public static void ClearScreen( this Mobile from )
		{
			NetState ns = from.NetState;

			if ( from.Map != null && ns != null )
			{
				foreach ( object o in from.Map.GetObjectsInRange( from.Location, Mobile.GlobalMaxUpdateRange ) )
				{
					if ( o is Mobile )
					{
						Mobile m = (Mobile) o;

						if ( m != from && m.InUpdateRange( from ) )
							ns.Send( m.RemovePacket );
					}
					else if ( o is Item )
					{
						Item item = (Item) o;

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
				foreach ( NetState state in m.Map.GetClientsInRange( m.Location ) )
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

				foreach ( NetState state in m.Map.GetClientsInRange( m.Location ) )
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
			NetState ns = from.NetState;

			if ( from.Map != null && ns != null )
			{
				foreach ( object o in from.Map.GetObjectsInRange( from.Location, Mobile.GlobalMaxUpdateRange ) )
				{
					if ( o is Item )
					{
						Item item = (Item) o;

						if ( from.CanSee( item ) && from.InRange( item.Location, item.GetUpdateRange( from ) ) )
							item.SendInfoTo( ns );
					}
					else if ( o is Mobile )
					{
						Mobile m = (Mobile) o;

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
