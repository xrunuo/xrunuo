using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
	public enum NavreysPillarState
	{
		Off,
		On,
		Hot
	}

	public class NavreysPillar : Item
	{
		private NavreysPillarState m_State;
		private InternalTimer m_Timer;
		private NavreysController m_Controller;

		public NavreysPillarState State
		{
			get { return m_State; }
			set
			{
				m_State = value;

				if ( m_Timer != null )
				{
					m_Timer.Stop();
					m_Timer = null;
				}

				switch ( m_State )
				{
					case NavreysPillarState.Off:
						Hue = 0x456;
						break;

					case NavreysPillarState.On:
						Hue = 0;
						break;

					case NavreysPillarState.Hot:
						m_Timer = new InternalTimer( this );
						m_Timer.Start();
						break;
				}
			}
		}

		public NavreysPillar( NavreysController controller )
			: base( 0x3BF )
		{
			m_Controller = controller;
			Movable = false;
		}

		public override bool CheckLOSOnUse { get { return false; } }

		public override void OnDoubleClick( Mobile from )
		{
			if ( from.InRange( this, 3 ) && m_State == NavreysPillarState.On )
			{
				State = NavreysPillarState.Hot;
				m_Controller.CheckPillars();
			}
		}

		public NavreysPillar( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.Write( (int) m_State );
			writer.Write( (Item) m_Controller );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();

			m_State = (NavreysPillarState) reader.ReadInt();
			m_Controller = (NavreysController) reader.ReadItem();
		}

		private class InternalTimer : Timer
		{
			private NavreysPillar m_Pillar;
			private int m_Ticks;

			public InternalTimer( NavreysPillar pillar )
				: base( TimeSpan.Zero, TimeSpan.FromSeconds( 0.25 ) )
			{

				m_Pillar = pillar;
				m_Ticks = 20; // 5 seconds
			}

			protected override void OnTick()
			{
				m_Ticks--;

				m_Pillar.Hue = 0x461 + ( m_Ticks % 2 );

				if ( m_Ticks == 0 )
				{
					Stop();
					m_Pillar.State = NavreysPillarState.On;
				}
			}
		}
	}
}