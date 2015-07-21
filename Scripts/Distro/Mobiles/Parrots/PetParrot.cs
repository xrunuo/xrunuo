using System;
using Server;
using Server.Engines.Housing.Multis;
using Server.Items;
using Server.Multis;
using Server.Targeting;

namespace Server.Mobiles
{
	public class PetParrot : Item
	{
		public override int LabelNumber { get { return 1074281; } } // pet parrot

		[Constructable]
		public PetParrot()
			: base( 0x20EE )
		{
			Weight = 1.0;
			Hue = Utility.RandomDyedHue();
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 1072612 ); // Target the Parrot Perch you wish to place this Parrot upon.
				from.Target = new InternalTarget( this );
			}
			else
			{
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
			}
		}

		private class InternalTarget : Target
		{
			private PetParrot m_Parrot;

			public InternalTarget( PetParrot parrot )
				: base( 3, true, TargetFlags.None )
			{
				m_Parrot = parrot;

				CheckLOS = false;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				if ( m_Parrot == null || m_Parrot.Deleted )
					return;

				ParrotPerchAddon perch = null;

				if ( targeted is ParrotPerchAddon )
					perch = targeted as ParrotPerchAddon;
				else if ( targeted is ParrotPerchAddonComponent )
					perch = ( (ParrotPerchAddonComponent) targeted ).Addon as ParrotPerchAddon;

				if ( perch != null )
				{
					BaseHouse h = perch.MyHouse;

					if ( h == null || ( h.Owner != from && !h.CoOwners.Contains( from ) ) )
					{
						from.SendLocalizedMessage( 1072618 ); // Parrots can only be placed on Parrot Perches in houses where you are an owner or co-owner.
					}
					else if ( perch.Parrot != null )
					{
						from.SendLocalizedMessage( 1072616 ); // That Parrot Perch already has a Parrot.
					}
					else
					{
						Parrot parrot = new Parrot( m_Parrot.Hue );

						parrot.MoveToWorld( new Point3D( perch.Location.X, perch.Location.Y, perch.Location.Z + 11 ), perch.Map );

						perch.Parrot = parrot;
						parrot.Perch = perch;

						m_Parrot.Delete();
					}
				}
				else
				{
					from.SendLocalizedMessage( 1072614 ); // You must place the Parrot on a Parrot Perch.
				}
			}

			protected override void OnTargetOutOfRange( Mobile from, object targeted )
			{
				from.SendLocalizedMessage( 1072613 ); // You must be closer to the Parrot Perch to place the Parrot upon it.
			}
		}

		public PetParrot( Serial serial )
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
	}
}
