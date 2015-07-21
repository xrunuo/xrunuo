using System;
using System.Collections.Generic;
using System.Linq;

using Server;
using Server.Targeting;

namespace Server.Items
{
	public class DarkGlassFloor : Item
	{
		private Timer m_Timer;

		[Constructable]
		public DarkGlassFloor()
			: base( 0x2E46 )
		{
			Visible = false;
		}

		public override bool HandlesOnMovement { get { return true; } }

		public override void OnMovement( Mobile m, Point3D oldLocation )
		{
			if ( Visible )
				return;

			if ( m.InRange( this, 1 ) )
			{
				Visible = true;

				if ( m_Timer == null )
					m_Timer = Timer.DelayCall( TimeSpan.Zero, TimeSpan.FromSeconds( 2.0 ), new TimerCallback( Check ) );
				else
					m_Timer.Start();
			}
		}

		public void Check()
		{
			if ( !GetMobilesInRange( 1 ).Any() )
				Visible = false;
		}

		public DarkGlassFloor( Serial serial )
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

			Visible = false;
		}
	}
}
