using System;

namespace Server.Engines.Collections
{
	public class CollectionDecayTimer : Timer
	{
		// Hora del día a la que se produce el decay de puntos
		private static readonly TimeSpan DecayHour = TimeSpan.FromHours( 6.0 );

		// Rate de decay de puntos de las colecciones
		private static readonly double DecayRate = 0.005;

		private static CollectionDecayTimer m_Timer;
		private static DateTime m_NextDecayTime;

		public static void Initialize()
		{
			m_NextDecayTime = DateTime.Now.Date + DecayHour;

			if ( m_NextDecayTime < DateTime.Now )
				m_NextDecayTime += TimeSpan.FromDays( 1.0 );

			m_Timer = new CollectionDecayTimer( m_NextDecayTime - DateTime.Now );
			m_Timer.Start();
		}

		public CollectionDecayTimer( TimeSpan delay )
			: base( delay )
		{
		}

		protected override void OnTick()
		{
			foreach ( CollectionController collection in CollectionController.WorldCollections )
				collection.Points -= (int) ( DecayRate * ( collection.PointsPerTier ) );

			Initialize(); // Crea un nuevo timer para el siguiente decay
		}
	}
}