using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Events;

namespace Server.Factions
{
	public class Reflector
	{
		private static List<Type> m_Types = new List<Type>();

		private static TownCollection m_Towns;

		public static TownCollection Towns
		{
			get
			{
				if ( m_Towns == null )
					ProcessTypes();

				return m_Towns;
			}
		}

		private static FactionCollection m_Factions;

		public static FactionCollection Factions
		{
			get
			{
				if ( m_Factions == null )
					ProcessTypes();

				return m_Factions;
			}
		}

		public static void Configure()
		{
			EventSink.Instance.WorldSave += new WorldSaveEventHandler( EventSink_WorldSave );
		}

		private static void EventSink_WorldSave( WorldSaveEventArgs e )
		{
			m_Types.Clear();
		}

		public static void Serialize( GenericWriter writer, Type type )
		{
			int index = m_Types.IndexOf( type );

			writer.WriteEncodedInt( (int) ( index + 1 ) );

			if ( index == -1 )
			{
				writer.Write( type == null ? null : type.FullName );
				m_Types.Add( type );
			}
		}

		public static Type Deserialize( GenericReader reader )
		{
			int index = reader.ReadEncodedInt();

			if ( index == 0 )
			{
				string typeName = reader.ReadString();

				if ( typeName == null )
					m_Types.Add( null );
				else
					m_Types.Add( ScriptCompiler.FindTypeByFullName( typeName, false ) );

				return m_Types[m_Types.Count - 1];
			}
			else
			{
				return m_Types[index - 1];
			}
		}

		private static void ProcessTypes()
		{
			m_Factions = new FactionCollection();

			m_Factions.Add( new CouncilOfMages() );
			m_Factions.Add( new Minax() );
			m_Factions.Add( new Shadowlords() );
			m_Factions.Add( new TrueBritannians() );

			m_Towns = new TownCollection();

			m_Towns.Add( new Britain() );
			m_Towns.Add( new Magincia() );
			m_Towns.Add( new Minoc() );
			m_Towns.Add( new Moonglow() );
			m_Towns.Add( new SkaraBrae() );
			m_Towns.Add( new Trinsic() );
			m_Towns.Add( new Vesper() );
			m_Towns.Add( new Yew() );
		}
	}
}