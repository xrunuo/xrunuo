using System;
using Server;
using Server.Network;

namespace Server.Items
{
	public class MagicVines : Item, ICarvable
	{
		public override int LabelNumber { get { return 1111655; } } // magic vines

		private VinesLockedSecretDoor m_Door;
		private Timer m_ResetTimer;

		public MagicVines( VinesLockedSecretDoor door )
			: base( 0xCF1 )
		{
			m_Door = door;

			Weight = 1.0;
			Movable = false;
		}

		public MagicVines( Serial serial )
			: base( serial )
		{
		}

		public override bool HandlesOnMovement { get { return true; } }

		public override void OnMovement( Mobile m, Point3D oldLocation )
		{
			if ( Visible && m.X > this.X && this.InRange( m, 4 ) && !this.InRange( oldLocation, 4 ) )
			{
				// You notice something odd about the vines covering the wall.
				m.SendLocalizedMessage( 1111665 );
			}
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( !from.InRange( this, 1 ) )
				return;

			if ( Utility.RandomBool() )
			{
				// The vines tighten their grip, stopping you from opening the door.
				from.SendLocalizedMessage( 1111658 );
			}
			else
			{
				// You try to examine the strange wall but the vines get in your way.
				from.SendLocalizedMessage( 1111659 );
			}
		}

		public void Carve( Mobile from, Item item )
		{
			// Your blade slips helplessly through the magic vines, leaving them unscathed.
			from.SendLocalizedMessage( 1111660 );
		}

		public void OnFlameStrike( Mobile from )
		{
			// The vines seem immune to the flames. Could a different substance burn them?
			from.SendLocalizedMessage( 1111661 );
		}

		public void OnAcidSac( Mobile from )
		{
			Effects.PlaySound( Location, Map, 0x18D );

			// The acid quickly burns through the writhing vines, revealing the strange wall.
			PublicOverheadMessage( MessageType.Regular, 0x2A, 1111662 );

			Visible = false;
			BeginReset();
		}

		public void OnOpenDoor()
		{
			Visible = false;
			BeginReset();
		}

		public void BeginReset()
		{
			if ( m_ResetTimer != null )
				m_ResetTimer.Stop();

			m_ResetTimer = Timer.DelayCall( TimeSpan.FromSeconds( 15.0 ), new TimerCallback(
				delegate
				{
					Visible = true;

					// The vines recover from the acid and, spreading like tentacles, reclaim their grip over the wall. 
					PublicOverheadMessage( MessageType.Regular, 0x25, 1111663 );

					Effects.PlaySound( Location, Map, 0x55 );
				}
			) );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.Write( (Item) m_Door );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version =*/
			reader.ReadInt();

			m_Door = reader.ReadItem() as VinesLockedSecretDoor;

			Visible = true;
		}
	}

	public class VinesLockedSecretDoor : UnderworldSecretDoor
	{
		private MagicVines m_Vines;

		[Constructable]
		public VinesLockedSecretDoor( int closedId, int mediumId )
			: base( closedId, mediumId )
		{
			Movable = false;

			m_Vines = new MagicVines( this );
		}

		public override void OnLocationChange( Point3D oldLocation )
		{
			if ( m_Vines != null )
				Timer.DelayCall( TimeSpan.FromSeconds( 0.5 ), new TimerCallback( delegate { m_Vines.MoveToWorld( new Point3D( Location.X + 1, Location.Y, Location.Z ), Map ); } ) );
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( !from.InRange( this, 1 ) )
				return;

			if ( from.Location.X >= m_Vines.Location.X && m_Vines.Visible )
				m_Vines.OnDoubleClick( from );
			else
			{
				m_Vines.OnOpenDoor();
				base.OnDoubleClick( from );
			}
		}

		public VinesLockedSecretDoor( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.Write( (Item) m_Vines );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version =*/
			reader.ReadInt();

			m_Vines = reader.ReadItem() as MagicVines;
		}
	}
}