using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using Server.Guilds;

namespace Server.Persistence
{
	public sealed class DynamicSaveStrategy : SaveStrategy
	{
		public override string Name => "Dynamic";

		private FileStream m_ItemData, m_ItemIndex;
		private FileStream m_MobileData, m_MobileIndex;
		private FileStream m_GuildData, m_GuildIndex;

		private readonly ConcurrentBag<Item> m_DecayBag;
		private readonly ConcurrentBag<IVendor> m_RestockBag;

		private readonly BlockingCollection<QueuedMemoryWriter> m_ItemThreadWriters;
		private readonly BlockingCollection<QueuedMemoryWriter> m_MobileThreadWriters;
		private readonly BlockingCollection<QueuedMemoryWriter> m_GuildThreadWriters;

		public DynamicSaveStrategy()
		{
			m_DecayBag = new ConcurrentBag<Item>();
			m_RestockBag = new ConcurrentBag<IVendor>();

			m_ItemThreadWriters = new BlockingCollection<QueuedMemoryWriter>();
			m_MobileThreadWriters = new BlockingCollection<QueuedMemoryWriter>();
			m_GuildThreadWriters = new BlockingCollection<QueuedMemoryWriter>();
		}

		public override void Save( bool permitBackgroundWrite )
		{
			OpenFiles();

			var saveTasks = new Task[3];

			saveTasks[0] = SaveItems();
			saveTasks[1] = SaveMobiles();
			saveTasks[2] = SaveGuilds();

			SaveTypeDatabases();

			if ( permitBackgroundWrite )
			{
				// This option makes it finish the writing to disk in the background, continuing even after Save() returns.
				Task.Factory.ContinueWhenAll( saveTasks, _ =>
					{
						CloseFiles();

						World.NotifyDiskWriteComplete();
					} );
			}
			else
			{
				Task.WaitAll( saveTasks ); // Waits for the completion of all of the tasks (committing to disk)
				CloseFiles();
			}
		}

		private static Task StartCommitTask( BlockingCollection<QueuedMemoryWriter> threadWriter, FileStream data, FileStream index )
		{
			var commitTask = Task.Factory.StartNew( () =>
			{
				while ( !threadWriter.IsCompleted )
				{
					QueuedMemoryWriter writer;

					try
					{
						writer = threadWriter.Take();
					}
					catch ( InvalidOperationException )
					{
						// Per MSDN, it's fine if we're here, successful completion of adding can rarely put us into this state.
						break;
					}

					writer.CommitTo( data, index );
				}
			} );

			return commitTask;
		}

		private Task SaveItems()
		{
			// Start the blocking consumer; this runs in background.
			var commitTask = StartCommitTask( m_ItemThreadWriters, m_ItemData, m_ItemIndex );

			var items = World.Items;

			// Start the producer.
			Parallel.ForEach( items, () => new QueuedMemoryWriter(),
				( item, state, writer ) =>
				{
					var startPosition = writer.Position;

					item.Serialize( writer );

					var size = (int) ( writer.Position - startPosition );

					writer.QueueForIndex( item, size );

					if ( item.Decays && item.Parent == null && item.Map != Map.Internal && DateTime.UtcNow > ( item.LastMoved + item.DecayTime ) )
						m_DecayBag.Add( item );

					return writer;
				}, writer =>
				{
					writer.Flush();

					m_ItemThreadWriters.Add( writer );
				} );

			m_ItemThreadWriters.CompleteAdding(); // We only get here after the Parallel.ForEach completes.  Lets our task

			return commitTask;
		}

		private Task SaveMobiles()
		{
			// Start the blocking consumer; this runs in background.
			var commitTask = StartCommitTask( m_MobileThreadWriters, m_MobileData, m_MobileIndex );

			var mobiles = World.Mobiles;

			// Start the producer.
			Parallel.ForEach( mobiles, () => new QueuedMemoryWriter(),
				( mobile, state, writer ) =>
				{
					var startPosition = writer.Position;

					mobile.Serialize( writer );

					var size = (int) ( writer.Position - startPosition );

					writer.QueueForIndex( mobile, size );

					if ( mobile is IVendor )
					{
						var vendor = mobile as IVendor;

						if ( ( vendor.LastRestock + vendor.RestockDelay < DateTime.UtcNow ) )
							m_RestockBag.Add( vendor );
					}

					return writer;
				}, writer =>
				{
					writer.Flush();

					m_MobileThreadWriters.Add( writer );
				} );

			m_MobileThreadWriters.CompleteAdding();	// We only get here after the Parallel.ForEach completes.  Lets our task tell the consumer that we're done

			return commitTask;
		}

		private Task SaveGuilds()
		{
			// Start the blocking consumer; this runs in background.
			var commitTask = StartCommitTask( m_GuildThreadWriters, m_GuildData, m_GuildIndex );

			IEnumerable<BaseGuild> guilds = BaseGuild.List.Values;

			// Start the producer.
			Parallel.ForEach( guilds, () => new QueuedMemoryWriter(),
				( guild, state, writer ) =>
				{
					var startPosition = writer.Position;

					guild.Serialize( writer );

					var size = (int) ( writer.Position - startPosition );

					writer.QueueForIndex( guild, size );

					return writer;
				}, writer =>
				{
					writer.Flush();

					m_GuildThreadWriters.Add( writer );
				} );

			m_GuildThreadWriters.CompleteAdding(); // We only get here after the Parallel.ForEach completes.  Lets our task

			return commitTask;
		}

		public override void OnFinished()
		{
			ProcessDecay();
			ProcessRestock();
		}

		private void ProcessDecay()
		{
			Item item;

			while ( m_DecayBag.TryTake( out item ) )
			{
				if ( item.OnDecay() )
					item.Delete();
			}
		}

		private void ProcessRestock()
		{
			IVendor vendor;

			while ( m_RestockBag.TryTake( out vendor ) )
			{
				vendor.Restock();
				vendor.LastRestock = DateTime.UtcNow;
			}
		}

		private void OpenFiles()
		{
			m_ItemData = new FileStream( World.ItemDataPath, FileMode.Create );
			m_ItemIndex = new FileStream( World.ItemIndexPath, FileMode.Create );

			m_MobileData = new FileStream( World.MobileDataPath, FileMode.Create );
			m_MobileIndex = new FileStream( World.MobileIndexPath, FileMode.Create );

			m_GuildData = new FileStream( World.GuildDataPath, FileMode.Create );
			m_GuildIndex = new FileStream( World.GuildIndexPath, FileMode.Create );

			WriteCount( m_ItemIndex, World.m_Items.Count );
			WriteCount( m_MobileIndex, World.m_Mobiles.Count );
			WriteCount( m_GuildIndex, BaseGuild.List.Count );
		}

		private void CloseFiles()
		{
			m_ItemData.Close();
			m_ItemIndex.Close();

			m_MobileData.Close();
			m_MobileIndex.Close();

			m_GuildData.Close();
			m_GuildIndex.Close();
		}

		private void WriteCount( FileStream indexFile, int count )
		{
			// Equiv to GenericWriter.Write( (int)count );
			var buffer = new byte[4];

			buffer[0] = (byte) ( count );
			buffer[1] = (byte) ( count >> 8 );
			buffer[2] = (byte) ( count >> 16 );
			buffer[3] = (byte) ( count >> 24 );

			indexFile.Write( buffer, 0, buffer.Length );
		}

		private void SaveTypeDatabases()
		{
			SaveTypeDatabase( World.ItemTypesPath, World.m_ItemTypes );
			SaveTypeDatabase( World.MobileTypesPath, World.m_MobileTypes );
			SaveTypeDatabase( World.GuildTypesPath, World.m_GuildTypes );
		}

		private void SaveTypeDatabase( string path, List<Type> types )
		{
			var bfw = new BinaryFileWriter( path, false );

			bfw.Write( types.Count );

			foreach ( var type in types )
			{
				bfw.Write( type.FullName );
			}

			bfw.Flush();

			bfw.Close();
		}
	}
}
