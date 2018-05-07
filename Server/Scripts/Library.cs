using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Server
{
	public class Library
	{
		private static readonly ILog log = LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

		private bool m_Configured, m_Initialized;

		public Library( Configuration.Library libConfig, Assembly assembly )
		{
			Name = libConfig.Name;
			Assembly = assembly;

			Assembly.GetTypes();

			Types = Assembly.GetTypes().Where( type => !libConfig.GetIgnoreType( type ) ).ToArray();

			TypesByName = new TypeTable( Types.Length );
			TypesByFullName = new TypeTable( Types.Length );

			foreach ( var type in Types )
			{
				TypesByName.Add( type.Name, type );
				TypesByFullName.Add( type.FullName, type );

				if ( type.IsDefined( typeof( TypeAliasAttribute ), false ) )
				{
					var attrs = type.GetCustomAttributes( typeof( TypeAliasAttribute ), false );

					if ( attrs.Length > 0 && attrs[0] != null )
					{
						var attr = attrs[0] as TypeAliasAttribute;

						foreach ( var alias in attr.Aliases )
							TypesByFullName.Add( alias, type );
					}
				}
			}

			TypeCache = new TypeCache( Types, TypesByName, TypesByFullName );
		}

		public string Name { get; }

		public Assembly Assembly { get; }

		public TypeCache TypeCache { get; }

		public Type[] Types { get; }

		public TypeTable TypesByName { get; }

		public TypeTable TypesByFullName { get; }

		public void Verify( ref int itemCount, ref int mobileCount )
		{
			Type[] ctorTypes = { typeof( Serial ) };

			foreach ( Type t in Types )
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
							log.Warning( "{0} has no serialization constructor", t );

						if ( t.GetMethod( "Serialize", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly ) == null )
							log.Warning( "{0} has no Serialize() method", t );

						if ( t.GetMethod( "Deserialize", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly ) == null )
							log.Warning( "{0} has no Deserialize() method", t );
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

			foreach ( Type type in Types )
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
			if ( Name == "Core" )
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
			if ( Name == "Core" )
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
			return TypesByName.Get( name, ignoreCase );
		}

		public Type GetTypeByFullName( string fullName, bool ignoreCase )
		{
			return TypesByFullName.Get( fullName, ignoreCase );
		}

		public IEnumerable<Type> FindTypesByAttribute<T>() where T : Attribute
		{
			return Types.Where( type => type.GetCustomAttribute<T>() != null );
		}
	}
}
