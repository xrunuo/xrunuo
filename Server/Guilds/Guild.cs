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
		internal int m_TypeRef;

		public BaseGuild( int id ) // serialization ctor
		{
			Serial = id;
			List.Add( Serial, this );

			if ( Serial + 1 > m_NextId )
				m_NextId = Serial + 1;

			RegisterType();
		}

		public BaseGuild()
		{
			Serial = m_NextId++;
			List.Add( Serial, this );

			RegisterType();
		}

		private void RegisterType()
		{
			var ourType = this.GetType();
			m_TypeRef = World.m_GuildTypes.IndexOf( ourType );

			if ( m_TypeRef == -1 )
			{
				World.m_GuildTypes.Add( ourType );
				m_TypeRef = World.m_GuildTypes.Count - 1;
			}
		}

		public int Serial { get; }

		int ISerializable.TypeReference => m_TypeRef;

		Serial ISerializable.SerialIdentity => Serial;

		public abstract void Deserialize( GenericReader reader );
		public abstract void Serialize( GenericWriter writer );

		public abstract string Abbreviation { get; set; }
		public abstract string Name { get; set; }
		public abstract GuildType Type { get; set; }
		public abstract bool Disbanded { get; }
		public abstract void OnDelete( Mobile mob );

		private static int m_NextId = 1;

		public static Dictionary<int, BaseGuild> List { get; } = new Dictionary<int, BaseGuild>();

		public static BaseGuild Find( int id )
		{
			BaseGuild g;

			List.TryGetValue( id, out g );

			return g;
		}

		public static BaseGuild FindByName( string name )
		{
			return List.Values.FirstOrDefault( g => g.Name == name );
		}

		public static BaseGuild FindByAbbrev( string abbr )
		{
			return List.Values.FirstOrDefault( g => g.Abbreviation == abbr );
		}

		public static BaseGuild[] Search( string find )
		{
			var words = find.ToLower().Split( ' ' );
			var results = new List<BaseGuild>();

			foreach ( var g in List.Values )
			{
				var match = true;
				var name = g.Name.ToLower();
				for ( var i = 0; i < words.Length; i++ )
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
			return $"0x{Serial:X} \"{Name} [{Abbreviation}]\"";
		}
	}
}
