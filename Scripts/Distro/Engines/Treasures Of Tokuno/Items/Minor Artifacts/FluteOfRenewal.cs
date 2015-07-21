using System;
using Server;

namespace Server.Items
{
	public class FluteOfRenewal : BambooFlute
	{
		public override int LabelNumber { get { return 1070927; } } // Flute of Renewal

		public override int InitMinUses { get { return 300; } }
		public override int InitMaxUses { get { return 300; } }

		private static int[] m_SuperSlayers = new int[]
			{
				(int)SlayerName.Demon,
				(int)SlayerName.Undead,
				(int)SlayerName.Repond,
				(int)SlayerName.Arachnid,
				(int)SlayerName.Reptile
			};

		private static Timer m_ReplenishTimer;

		[Constructable]
		public FluteOfRenewal()
		{
			Slayer = (SlayerName) Utility.RandomList( m_SuperSlayers );

			BeginTimer();
		}

		public void BeginTimer()
		{
			m_ReplenishTimer = new ReplenishTimer( this );

			m_ReplenishTimer.Start();
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			list.Add( 1070928 ); // Replenish Charges
		}

		public FluteOfRenewal( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();

			BeginTimer();
		}

		private class ReplenishTimer : Timer
		{
			private FluteOfRenewal m_flute;

			public ReplenishTimer( FluteOfRenewal flute )
				: base( TimeSpan.FromMinutes( 5.0 ) )
			{
				m_flute = flute;
			}

			protected override void OnTick()
			{
				if ( m_flute.UsesRemaining < 300 )
				{
					m_flute.UsesRemaining++;
				}

				m_flute.BeginTimer();

				Stop();
			}
		}
	}
}
