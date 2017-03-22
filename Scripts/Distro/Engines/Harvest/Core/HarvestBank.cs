using System;

using Server;
using Server.Regions;

namespace Server.Engines.Harvest
{
	public class HarvestBank
	{
		private int m_Current;
		private int m_Maximum;
		private DateTime m_NextRespawn;
		private HarvestVein m_Vein, m_DefaultVein;
		private double m_ChanceToFallback;

		private HarvestDefinition m_Definition;

		public HarvestDefinition Definition
		{
			get { return m_Definition; }
		}

		public int Current
		{
			get
			{
				CheckRespawn();
				return m_Current;
			}
		}

		public HarvestVein Vein
		{
			get
			{
				CheckRespawn();
				return m_Vein;
			}
			set
			{
				m_Vein = value;
			}
		}

		public HarvestVein DefaultVein
		{
			get
			{
				CheckRespawn();
				return m_DefaultVein;
			}
		}

		public double ChanceToFallback
		{
			get
			{
				return m_ChanceToFallback;
			}
		}

		public void CheckRespawn()
		{
			if ( m_Current == m_Maximum || m_NextRespawn > DateTime.UtcNow )
				return;

			m_Current = m_Maximum;

			if ( m_Definition.RandomizeVeins )
				m_DefaultVein = m_Definition.GetVeinFrom( Utility.RandomDouble() );

			m_Vein = m_DefaultVein;
		}

		public void Consume( Mobile from )
		{
			CheckRespawn();

			if ( m_Current == m_Maximum )
			{
				if ( from.Region is GuardedRegion )
					m_Current /= 2;

				m_Current--;

				double min = m_Definition.MinRespawn.TotalMinutes;
				double max = m_Definition.MaxRespawn.TotalMinutes;
				double rnd = Utility.RandomDouble();
				double minutes = min + ( rnd * ( max - min ) );

				if ( m_Definition.RaceBonus && from.Race == Race.Elf )
					minutes *= .75;	// 25% off the time.  

				m_NextRespawn = DateTime.UtcNow + TimeSpan.FromMinutes( minutes );
			}
			else
			{
				m_Current--;
			}

			if ( m_Current < 0 )
				m_Current = 0;
		}

		public HarvestBank( HarvestDefinition def, HarvestVein defaultVein, double chanceToFallback )
		{
			m_Maximum = Utility.RandomMinMax( def.MinTotal, def.MaxTotal );
			m_Current = m_Maximum;
			m_DefaultVein = defaultVein;
			m_Vein = m_DefaultVein;
			m_ChanceToFallback = chanceToFallback;

			m_Definition = def;
		}
	}
}