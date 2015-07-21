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

using Server;
using Server.Items;

namespace Server
{
	public delegate void AnimatedEventHandler( Mobile m, AnimatedEventArgs args );
	public delegate void DamagedEventHandler( Mobile m, DamagedEventArgs args );
	public delegate void DeadEventHandler( Mobile m, DeadEventArgs args );
	public delegate void ItemLiftedEventHandler( Mobile m, ItemLiftedEventArgs args );
	public delegate void ItemDroppedEventHandler( Mobile m, ItemDroppedEventArgs args );
	public delegate void LocationChangedEventHandler( Mobile m, LocationChangedEventArgs args );

	public class AnimatedEventArgs : EventArgs
	{
		public int Action { get; private set; }
		public int SubAction { get; private set; }

		public AnimatedEventArgs( int action, int subAction )
		{
			Action = action;
			SubAction = subAction;
		}
	}

	public class DamagedEventArgs : EventArgs
	{
		public int Amount { get; private set; }
		public Mobile From { get; private set; }

		public DamagedEventArgs( int amount, Mobile from )
		{
			Amount = amount;
			From = from;
		}
	}

	public class DeadEventArgs : EventArgs
	{
		public Container Corpse { get; private set; }

		public DeadEventArgs( Container corpse )
		{
			Corpse = corpse;
		}
	}

	public class ItemLiftedEventArgs : EventArgs
	{
		public Item Item { get; private set; }
		public int Amount { get; private set; }

		public ItemLiftedEventArgs( Item item, int amount )
		{
			Item = item;
			Amount = amount;
		}
	}

	public class ItemDroppedEventArgs : EventArgs
	{
		public Item Item { get; private set; }
		public bool Bounced { get; private set; }

		public ItemDroppedEventArgs( Item item, bool bounced )
		{
			Item = item;
			Bounced = bounced;
		}
	}

	public class LocationChangedEventArgs : EventArgs
	{
		public Point3D OldLocation { get; private set; }
		public Point3D NewLocation { get; private set; }
		public bool IsTeleport { get; private set; }

		public LocationChangedEventArgs( Point3D oldLocation, Point3D newLocation, bool isTeleport )
		{
			OldLocation = oldLocation;
			NewLocation = newLocation;
			IsTeleport = isTeleport;
		}
	}

	public partial class Mobile
	{
		public static event AnimatedEventHandler Animated;
		public static event DamagedEventHandler Damaged;
		public static event DeadEventHandler Dead;
		public static event ItemLiftedEventHandler ItemLifted;
		public static event ItemDroppedEventHandler ItemDropped;
		public static event LocationChangedEventHandler LocationChanged;

		protected void InvokeAnimated( AnimatedEventArgs args )
		{
			if ( Animated != null )
				Animated( this, args );
		}

		protected void InvokeDamaged( DamagedEventArgs args )
		{
			if ( Damaged != null )
				Damaged( this, args );
		}

		protected void InvokeDead( DeadEventArgs args )
		{
			if ( Dead != null )
				Dead( this, args );
		}

		protected void InvokeItemLifted( ItemLiftedEventArgs args )
		{
			if ( ItemLifted != null )
				ItemLifted( this, args );
		}

		protected void InvokeItemDropped( ItemDroppedEventArgs args )
		{
			if ( ItemDropped != null )
				ItemDropped( this, args );
		}

		protected void InvokeLocationChanged( LocationChangedEventArgs args )
		{
			if ( LocationChanged != null )
				LocationChanged( this, args );
		}
	}
}
