using System;
using System.Collections.Generic;

namespace Server.Persistence
{
	public abstract class LoadStrategy
	{
		public static LoadStrategy Acquire()
		{
			return new StandardLoadStrategy();
		}

		public abstract string Name { get; }
		public abstract void LoadEntities( IEnumerable<IEntityRepository> repositories );
	}
}