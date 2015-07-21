using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	public interface IRenowned
	{
		Type[] Rewards { get; }
	}

	public class Renowned
	{
		private static Item ConstructReward( Type[] types )
		{
			try
			{
				return (Item) Activator.CreateInstance( types[Utility.Random( types.Length )] );
			}
			catch
			{
			}

			return null;
		}

		public static void CheckDropReward( Mobile m, Container c )
		{
			if ( m is IRenowned )
			{
				IRenowned renowned = m as IRenowned;

				if ( 0.02 > Utility.RandomDouble() )
				{
					Item reward = ConstructReward( renowned.Rewards );

					if ( reward != null )
						c.DropItem( reward );
				}
			}
		}
	}
}
