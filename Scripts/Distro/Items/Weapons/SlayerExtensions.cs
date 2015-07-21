using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.Items
{
	public static class SlayerExtensions
	{
		public static IEnumerable<ISlayer> GetEquippedSlayers( this Mobile m )
		{
			return m.GetEquippedItems().OfType<ISlayer>();
		}

		public static IEnumerable<SlayerEntry> GetSlayerEntries( this Mobile m )
		{
			return m.GetEquippedSlayers().SelectMany( ( slayer ) => slayer.GetSlayerEntries() );
		}

		public static IEnumerable<SlayerEntry> GetSlayerEntries( this ISlayer slayer )
		{
			return new[] { slayer.Slayer, slayer.Slayer2 }
				.Select( name => SlayerGroup.GetEntryByName( name ) )
				.Where( entry => entry != null );
		}
	}
}
