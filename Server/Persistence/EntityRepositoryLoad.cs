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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

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
				EntityType[] types = LoadEntityTypes( m_Repository.TypesPath );
				m_EntityEntries = LoadEntityIndex( types );
			}
			else
			{
				m_Repository.Initialize();
			}
		}

		private EntityType[] LoadEntityTypes( string path )
		{
			using ( FileStream tdb = new FileStream( path, FileMode.Open, FileAccess.Read, FileShare.Read ) )
			{
				using ( BinaryReader tdbReader = new BinaryReader( tdb ) )
				{
					return LoadEntityTypes( tdbReader );
				}
			}
		}

		private static readonly Type[] m_CtorTypes = new[] { typeof( Serial ) };

		private EntityType[] LoadEntityTypes( BinaryReader tdbReader )
		{
			int count = tdbReader.ReadInt32();

			EntityType[] types = new EntityType[count];

			for ( int i = 0; i < count; ++i )
			{
				string typeName = tdbReader.ReadString();

				if ( string.IsNullOrEmpty( typeName ) )
					continue;

				typeName = string.Intern( typeName );

				Type t = ScriptCompiler.FindTypeByFullName( typeName );

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
					throw new Exception( String.Format( "Bad type '{0}'", typeName ) );
				}

				ConstructorInfo ctor = t.GetConstructor( m_CtorTypes );

				if ( ctor != null )
					types[i] = new EntityType( typeName, ctor );
				else
					throw new Exception( String.Format( "Type '{0}' does not have a serialization constructor", t ) );
			}

			return types;
		}

		private IEntityEntry[] LoadEntityIndex( EntityType[] ctors )
		{
			using ( BinaryReader idxReader = new BinaryReader( new FileStream( m_Repository.IndexPath, FileMode.Open, FileAccess.Read, FileShare.Read ) ) )
			{
				return LoadEntityIndex( idxReader, ctors );
			}
		}

		private IEntityEntry[] LoadEntityIndex( BinaryReader idxReader, EntityType[] types )
		{
			int count = idxReader.ReadInt32();
			m_Repository.Initialize( count );
			
			var entries = new HashSet<IEntityEntry>();

			for ( int i = 0; i < count; ++i )
			{
				int typeId = idxReader.ReadInt32();
				int serial = idxReader.ReadInt32();
				long pos = idxReader.ReadInt64();
				int length = idxReader.ReadInt32();

				EntityType type = types[typeId];

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
								throw new Exception( String.Format( "***** Bad serialize on {0} *****", obj.GetType() ) );
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
				for ( int i = 0; i < m_EntityEntries.Length; ++i )
				{
					if ( m_EntityEntries[i].TypeId == failedTypeId )
						m_EntityEntries[i].Clear();
				}
			}
		}
	}
}
