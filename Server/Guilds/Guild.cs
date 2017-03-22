using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.Guilds
{
	public enum GuildType
	{
		Regular,
		Chaos,
		Order
	}

	public abstract class BaseGuild : ISerializable
	{
		private readonly int m_Id;

		internal int m_TypeRef;

		public BaseGuild( int id ) // serialization ctor
		{
			m_Id = id;
			m_GuildList.Add( m_Id, this );

			if ( m_Id + 1 > m_NextId )
				m_NextId = m_Id + 1;

			RegisterType();
		}

		public BaseGuild()
		{
			m_Id = m_NextId++;
			m_GuildList.Add( m_Id, this );

			RegisterType();
		}

		private void RegisterType()
		{
			Type ourType = this.GetType();
			m_TypeRef = World.m_GuildTypes.IndexOf( ourType );

			if ( m_TypeRef == -1 )
			{
				World.m_GuildTypes.Add( ourType );
				m_TypeRef = World.m_GuildTypes.Count - 1;
			}
		}

		public int Serial { get { return m_Id; } }

		int ISerializable.TypeReference
		{
			get { return m_TypeRef; }
		}

		Serial ISerializable.SerialIdentity
		{
			get { return m_Id; }
		}

		public abstract void Deserialize( GenericReader reader );
		public abstract void Serialize( GenericWriter writer );

		public abstract string Abbreviation { get; set; }
		public abstract string Name { get; set; }
		public abstract GuildType Type { get; set; }
		public abstract bool Disbanded { get; }
		public abstract void OnDelete( Mobile mob );

		private static Dictionary<int, BaseGuild> m_GuildList = new Dictionary<int, BaseGuild>();
		private static int m_NextId = 1;

		public static Dictionary<int, BaseGuild> List
		{
			get
			{
				return m_GuildList;
			}
		}

		public static BaseGuild Find( int id )
		{
			BaseGuild g;

			m_GuildList.TryGetValue( id, out g );

			return g;
		}

		public static BaseGuild FindByName( string name )
		{
			return m_GuildList.Values.FirstOrDefault( g => g.Name == name );
		}

		public static BaseGuild FindByAbbrev( string abbr )
		{
			return m_GuildList.Values.FirstOrDefault( g => g.Abbreviation == abbr );
		}

		public static BaseGuild[] Search( string find )
		{
			string[] words = find.ToLower().Split( ' ' );
			List<BaseGuild> results = new List<BaseGuild>();

			foreach ( BaseGuild g in m_GuildList.Values )
			{
				bool match = true;
				string name = g.Name.ToLower();
				for ( int i = 0; i < words.Length; i++ )
				{
					if ( name.IndexOf( words[i] ) == -1 )
					{
						match = false;
						break;
					}
				}

				if ( match )
					results.Add( g );
			}

			return results.ToArray();
		}

		public override string ToString()
		{
			return String.Format( "0x{0:X} \"{1} [{2}]\"", m_Id, Name, Abbreviation );
		}
	}
}