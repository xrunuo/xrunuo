//
//  X-RunUO - Ultima Online Server Emulator
//  Copyright (C) 2015 Pedro Pardal
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Server
{
	public class Library
	{
		private string m_Name;
		private Assembly m_Assembly;
		private Type[] m_Types;
		private TypeTable m_TypesByName, m_TypesByFullName;
		private bool m_Configured, m_Initialized;
		private TypeCache m_TypeCache;

		public Library( Configuration.Library libConfig, Assembly assembly )
		{
			m_Name = libConfig.Name;
			m_Assembly = assembly;

			m_Assembly.GetTypes();

			List<Type> typeList = new List<Type>();

			foreach ( Type type in m_Assembly.GetTypes() )
			{
				if ( libConfig == null || !libConfig.GetIgnoreType( type ) )
					typeList.Add( type );
			}

			m_Types = typeList.ToArray();

			m_TypesByName = new TypeTable( m_Types.Length );
			m_TypesByFullName = new TypeTable( m_Types.Length );

			foreach ( Type type in m_Types )
			{
				m_TypesByName.Add( type.Name, type );
				m_TypesByFullName.Add( type.FullName, type );

				if ( type.IsDefined( typeof( TypeAliasAttribute ), false ) )
				{
					object[] attrs = type.GetCustomAttributes( typeof( TypeAliasAttribute ), false );

					if ( attrs != null && attrs.Length > 0 && attrs[0] != null )
					{
						TypeAliasAttribute attr = attrs[0] as TypeAliasAttribute;

						foreach ( string alias in attr.Aliases )
							m_TypesByFullName.Add( alias, type );
					}
				}
			}

			m_TypeCache = new TypeCache( m_Types, m_TypesByName, m_TypesByFullName );
		}

		public string Name
		{
			get { return m_Name; }
		}

		public Assembly Assembly
		{
			get { return m_Assembly; }
		}

		public TypeCache TypeCache
		{
			get { return m_TypeCache; }
		}

		public Type[] Types
		{
			get { return m_Types; }
		}

		public TypeTable TypesByName
		{
			get { return m_TypesByName; }
		}

		public TypeTable TypesByFullName
		{
			get { return m_TypesByFullName; }
		}

		public void Verify( ref int itemCount, ref int mobileCount )
		{
			Type[] ctorTypes = new Type[] { typeof( Serial ) };

			foreach ( Type t in m_Types )
			{
				bool isItem = t.IsSubclassOf( typeof( Item ) );
				bool isMobile = t.IsSubclassOf( typeof( Mobile ) );

				if ( isItem || isMobile )
				{
					if ( isItem )
						++itemCount;
					else
						++mobileCount;

					try
					{
						if ( t.GetConstructor( ctorTypes ) == null )
							Console.WriteLine( "Warning: {0} has no serialization constructor", t );

						if ( t.GetMethod( "Serialize", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly ) == null )
							Console.WriteLine( "Warning: {0} has no Serialize() method", t );

						if ( t.GetMethod( "Deserialize", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly ) == null )
							Console.WriteLine( "Warning: {0} has no Deserialize() method", t );
					}
					catch
					{
					}
				}
			}
		}

		private void InvokeAll( string methodName )
		{
			List<MethodInfo> invoke = new List<MethodInfo>();

			foreach ( Type type in m_Types )
			{
				MethodInfo m = type.GetMethod( methodName, BindingFlags.Static | BindingFlags.Public );

				if ( m != null )
					invoke.Add( m );
			}

			invoke.Sort( CallPriorityComparer.Instance );

			foreach ( MethodInfo m in invoke )
				m.Invoke( null, null );
		}

		public void Configure()
		{
			if ( m_Name == "Core" )
			{
				m_Configured = true;
				return;
			}

			if ( m_Configured )
				throw new ApplicationException( "already configured" );

			InvokeAll( "Configure" );

			m_Configured = true;
		}

		public void Initialize()
		{
			if ( m_Name == "Core" )
			{
				m_Initialized = true;
				return;
			}

			if ( !m_Configured )
				throw new ApplicationException( "not configured yet" );
			if ( m_Initialized )
				throw new ApplicationException( "already initialized" );

			InvokeAll( "Initialize" );

			m_Initialized = true;
		}

		public Type GetTypeByName( string name, bool ignoreCase )
		{
			return m_TypesByName.Get( name, ignoreCase );
		}

		public Type GetTypeByFullName( string fullName, bool ignoreCase )
		{
			return m_TypesByFullName.Get( fullName, ignoreCase );
		}
	}
}