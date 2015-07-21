using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server;
using Server.Events;

namespace Server.Mobiles
{
	public class RespawnAtRestart
	{
		public static void Initialize()
		{
			EventSink.Instance.ServerStarted += new ServerStartedEventHandler( OnServerStarted );
		}

		private static void OnServerStarted()
		{
			Timer.DelayCall( TimeSpan.Zero, () =>
			{
				var spawners = World.Instance.Items.OfType<CreatureSpawner>().Where( spawner => spawner.RespawnAtRestart ).ToArray();
				spawners.Each( spawner => spawner.TotalRespawn() );
			} );
		}
	}
}
