using System;
using Server;
using Server.Engines.Housing;
using Server.Multis;
using Server.Targeting;

namespace Server.Items
{
	[Flipable( 0x14F0, 0x14EF )]
	public abstract class BaseAddonContainerDeed : Item
	{
		public abstract BaseAddonContainer Addon { get; }

		public BaseAddonContainerDeed()
			: base( 0x14F0 )
		{
			Weight = 1.0;
		}

		public BaseAddonContainerDeed( Serial serial )
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
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( IsChildOf( from.Backpack ) )
				from.Target = new InternalTarget( this );
			else
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
		}

		private class InternalTarget : Target
		{
			private BaseAddonContainerDeed m_Deed;

			public InternalTarget( BaseAddonContainerDeed deed )
				: base( -1, true, TargetFlags.None )
			{
				m_Deed = deed;

				CheckLOS = false;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				IPoint3D p = targeted as IPoint3D;
				Map map = from.Map;

				if ( p == null || map == null || m_Deed.Deleted )
					return;

				if ( m_Deed.IsChildOf( from.Backpack ) )
				{
					BaseAddonContainer addon = m_Deed.Addon;

					Spells.SpellHelper.GetSurfaceTop( ref p );

					IHouse house = null;

					AddonFitResult res = addon.CouldFit( p, map, from, ref house );

					if ( res == AddonFitResult.Valid )
						addon.MoveToWorld( new Point3D( p ), map );
					else if ( res == AddonFitResult.Blocked )
						from.SendLocalizedMessage( 500269 ); // You cannot build that there.
					else if ( res == AddonFitResult.NotInHouse )
						from.SendLocalizedMessage( 500274 ); // You can only place this in a house that you own!
					else if ( res == AddonFitResult.DoorsNotClosed )
						from.SendMessage( "You must close all house doors before placing this." );
					else if ( res == AddonFitResult.DoorTooClose )
						from.SendLocalizedMessage( 500271 ); // You cannot build near the door.
					else if ( res == AddonFitResult.NoWall )
						from.SendLocalizedMessage( 500268 ); // This object needs to be mounted on something.

					if ( res == AddonFitResult.Valid )
					{
						m_Deed.Delete();
						house.Addons.Add( addon );
					}
					else
					{
						addon.Delete();
					}
				}
				else
				{
					from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
				}
			}
		}
	}
}