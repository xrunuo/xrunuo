using System;
using Server;
using Server.Targeting;

namespace Server.Items
{
	public class AcidSac : Item
	{
		public override int LabelNumber { get { return 1111654; } } // acid sac

		[Constructable]
		public AcidSac()
			: base( 0xC67 )
		{
			Weight = 0.1;
			Hue = 0x464;
			Stackable = true;
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( from.Backpack == null || !IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 1080063 ); // This must be in your backpack to use it.
				return;
			}

			from.SendLocalizedMessage( 1111656 ); // What do you wish to use the acid on?
			from.BeginTarget( -1, false, TargetFlags.None, new TargetCallback( OnTarget ) );
		}

		protected void OnTarget( Mobile from, object targeted )
		{
			if ( from.Backpack == null || !IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 1080063 ); // This must be in your backpack to use it.
				return;
			}

			if ( targeted is MagicVines )
			{
				MagicVines vines = targeted as MagicVines;

				if ( from.InRange( vines, 1 ) )
				{
					vines.OnAcidSac( from );
					Consume();
				}
				else
				{
					from.SendLocalizedMessage( 1076766 ); // That is too far away.
				}
			}
			else
			{
				from.SendLocalizedMessage( 1046439 ); // That is not a valid target.
			}
		}

		public AcidSac( Serial serial )
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
