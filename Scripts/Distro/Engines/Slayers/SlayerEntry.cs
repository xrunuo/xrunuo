using System;
using System.Linq;
using Server;
using Server.Mobiles;

namespace Server.Items
{
	public class SlayerEntry
	{
		public SlayerGroup Group { get; set; }
		public SlayerName Name { get; }
		public Type[] Types { get; }

		private static int[] m_Titles = {
				1060479, // undead slayer
				1060470, // orc slayer
				1060480, // troll slayer
				1060468, // ogre slayer
				1060472, // repond slayer
				1060462, // dragon slayer
				1060478, // terathan slayer
				1060475, // snake slayer
				1060467, // lizardman slayer
				1060473, // reptile slayer
				     -1, // unused
				1060466, // gargoyle slayer
				     -1, // unused
				1060461, // demon slayer
				1060469, // ophidian slayer
				1060477, // spider slayer
				1060474, // scorpion slayer
				1060458, // arachnid slayer
				1060465, // fire elemental slayer
				1060481, // water elemental slayer
				1060457, // air elemental slayer
				1060471, // poison elemental slayer
				1060463, // earth elemental slayer
				1060459, // blood elemental slayer
				1060476, // snow elemental slayer
				1060464, // elemental slayer
				1070855  // fey slayer
			};

		public int Title
		{
			get
			{
				return m_Titles[(int) Name - 1];
			}
		}

		public SlayerEntry( SlayerName name, params Type[] types )
		{
			Name = name;
			Types = types;
		}

		public bool Slays( Mobile m )
		{
			return (m is BaseCreature && ((BaseCreature)m).SlayerGroup == Name);
		}
	}
}
