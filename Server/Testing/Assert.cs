using System;

namespace Server.Testing
{
	public class Assert
	{
		public static void False(bool condition)
		{
			if (condition)
			{
				throw new Exception("Expected false but found true");
			}
		}

		public static void True(bool condition)
		{
			if (!condition)
			{
				throw new Exception("Expected true but found false");
			}
		}
	}
}
