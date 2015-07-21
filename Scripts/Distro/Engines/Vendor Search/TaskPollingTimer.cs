using System;
using System.Linq;
using System.Threading.Tasks;
using Server;

namespace Server.Engines.VendorSearch
{
	public class TaskPollingTimer<T> : Timer
	{
		private Task<T> m_Task;
		private Action<T> m_Callback;

		public TaskPollingTimer( Task<T> task, Action<T> callback )
			: base( TimeSpan.FromSeconds( 0.1 ), TimeSpan.FromSeconds( 0.1 ) )
		{
			m_Task = task;
			m_Callback = callback;
		}

		protected override void OnTick()
		{
			if ( m_Task.IsCompleted )
			{
				m_Callback( m_Task.Result );
				Stop();
			}
		}
	}
}
