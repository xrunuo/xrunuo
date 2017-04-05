namespace Server.Persistence
{
	public abstract class SaveStrategy
	{
		public static SaveStrategy Acquire()
		{
			if ( World.DualSave )
				return new DynamicSaveStrategy();

			return new StandardSaveStrategy();
		}

		public abstract string Name { get; }
		public abstract void Save( bool permitBackgroundWrite );

		public abstract void OnFinished();
	}
}