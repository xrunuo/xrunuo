using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Server.Persistence
{
	public interface IEntityRepositoryLoad
	{
		void LoadEntityIndex();
		void DeserializeEntities();
		void SaveIndex();
		void OnError( int failedTypeId );
	}

	public class EntityRepositoryLoad<T> : IEntityRepositoryLoad
		where T : class, ISerializable
	{
		private readonly EntityRepository<T> m_Repository;

		private IEntityEntry[] m_EntityEntries;

		public EntityRepositoryLoad( EntityRepository<T> repository )
		{
			m_Repository = repository;
		}

		public void LoadEntityIndex()
		{
			if ( File.Exists( m_Repository.IndexPath ) && File.Exists( m_Repository.TypesPath ) )
			{
				var types = LoadEntityTypes( m_Repository.TypesPath );
				m_EntityEntries = LoadEntityIndex( types );
			}
			else
			{
				m_Repository.Initialize();
			}
		}

		private EntityType[] LoadEntityTypes( string path )
		{
			using ( var tdb = new FileStream( path, FileMode.Open, FileAccess.Read, FileShare.Read ) )
			{
				using ( var tdbReader = new BinaryReader( tdb ) )
				{
					return LoadEntityTypes( tdbReader );
				}
			}
		}

		private static readonly Type[] m_CtorTypes = { typeof( Serial ) };

		private EntityType[] LoadEntityTypes( BinaryReader tdbReader )
		{
			var count = tdbReader.ReadInt32();

			var types = new EntityType[count];

			for ( var i = 0; i < count; ++i )
			{
				var typeName = tdbReader.ReadString();

				if ( string.IsNullOrEmpty( typeName ) )
					continue;

				typeName = string.Intern( typeName );

				var t = ScriptCompiler.FindTypeByFullName( typeName );

				if ( t == null )
				{
					Console.WriteLine( "failed" );
					Console.WriteLine( "Error: Type '{0}' was not found. Delete all of those types? (y/n)", typeName );

					if ( Console.ReadKey().Key == ConsoleKey.Y )
					{
						Console.Write( "World: Loading..." );
						continue;
					}

					Console.WriteLine( "Types will not be deleted. An exception will be thrown when you press return" );
					throw new Exception( $"Bad type '{typeName}'" );
				}

				var ctor = t.GetConstructor( m_CtorTypes );

				if ( ctor != null )
					types[i] = new EntityType( typeName, ctor );
				else
					throw new Exception( $"Type '{t}' does not have a serialization constructor" );
			}

			return types;
		}

		private IEntityEntry[] LoadEntityIndex( EntityType[] ctors )
		{
			using ( var idxReader = new BinaryReader( new FileStream( m_Repository.IndexPath, FileMode.Open, FileAccess.Read, FileShare.Read ) ) )
			{
				return LoadEntityIndex( idxReader, ctors );
			}
		}

		private IEntityEntry[] LoadEntityIndex( BinaryReader idxReader, EntityType[] types )
		{
			var count = idxReader.ReadInt32();
			m_Repository.Initialize( count );

			var entries = new HashSet<IEntityEntry>();

			for ( var i = 0; i < count; ++i )
			{
				var typeId = idxReader.ReadInt32();
				var serial = idxReader.ReadInt32();
				var pos = idxReader.ReadInt64();
				var length = idxReader.ReadInt32();

				var type = types[typeId];

				if ( type.Constructor == null )
					continue;

				T entity = null;

				try
				{
					entity = (T) type.CtorDelegate( serial );
				}
				catch
				{
				}

				if ( entity != null )
				{
					entries.Add( new EntityEntry( entity, typeId, type.Name, pos, length ) );
					m_Repository.AddObject( entity );
				}
			}

			return entries.ToArray();
		}

		public void DeserializeEntities()
		{
			if ( !File.Exists( m_Repository.DataPath ) )
				return;

			using ( var bin = new FileStream( m_Repository.DataPath, FileMode.Open, FileAccess.Read, FileShare.Read ) )
			{
				var reader = new BinaryFileReader( new BinaryReader( bin ) );

				foreach ( var entry in m_EntityEntries )
				{
					var obj = entry.Object;

					if ( obj != null )
					{
						reader.Seek( entry.Position, SeekOrigin.Begin );

						try
						{
							obj.Deserialize( reader );

							if ( reader.Position != ( entry.Position + entry.Length ) )
								throw new Exception( $"***** Bad serialize on {obj.GetType()} *****" );
						}
						catch ( Exception e )
						{
							entry.Clear();
							throw new RepositoryLoadException( this, e, obj.SerialIdentity, obj.GetType(), entry.TypeId );
						}
					}
				}

				reader.Close();
			}

			m_EntityEntries = null;
		}

		public void SaveIndex()
		{
			if ( m_EntityEntries == null )
				return;

			if ( !Directory.Exists( m_Repository.BasePath ) )
				Directory.CreateDirectory( m_Repository.BasePath );

			using ( var idx = new FileStream( m_Repository.IndexPath, FileMode.Create, FileAccess.Write, FileShare.None ) )
			{
				var idxWriter = new BinaryWriter( idx );

				idxWriter.Write( m_EntityEntries.Length );

				foreach ( var obj in m_EntityEntries )
				{
					idxWriter.Write( obj.TypeId );
					idxWriter.Write( obj.Serial );
					idxWriter.Write( obj.Position );
					idxWriter.Write( obj.Length );
				}

				idxWriter.Close();
			}
		}

		public void OnError( int failedTypeId )
		{
			Console.WriteLine( "Delete all objects of that type? (y/n)" );

			if ( Console.ReadKey().Key == ConsoleKey.Y )
			{
				for ( var i = 0; i < m_EntityEntries.Length; ++i )
				{
					if ( m_EntityEntries[i].TypeId == failedTypeId )
						m_EntityEntries[i].Clear();
				}
			}
		}
	}
}
