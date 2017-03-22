using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

using Server.Events;
using Server.Network;
using Server.Persistence;

namespace Server
{
	public class World
	{
		private static World m_Instance;

		public static World Instance
		{
			get
			{
				if ( m_Instance == null )
					m_Instance = new World();

				return m_Instance;
			}
		}

		private World()
		{
		}

		public static bool ManualGC;
		public static bool DualSave;

		private List<IEntityRepository> m_Repositories = new List<IEntityRepository>
			{
				new MobileRepository(),
				new ItemRepository(),
				new GuildRepository(),
			}; 

		internal Dictionary<Serial, Mobile> m_Mobiles;
		internal Dictionary<Serial, Item> m_Items;
		
		private bool m_Loading;
		private bool m_Loaded;
		private bool m_Saving;

		public bool Saving { get { return m_Saving; } }
		public bool Loaded { get { return m_Loaded; } }
		public bool Loading { get { return m_Loading; } }

		private ManualResetEvent m_DiskWriteHandle = new ManualResetEvent( true );

		private Queue<IEntity> m_AddQueue, m_DeleteQueue;

		public readonly static string MobileBasePath = Path.Combine( "Saves", "Mobiles" );
		public readonly static string MobileIndexPath = Path.Combine( MobileBasePath, "Mobiles.idx" );
		public readonly static string MobileTypesPath = Path.Combine( MobileBasePath, "Mobiles.tdb" );
		public readonly static string MobileDataPath = Path.Combine( MobileBasePath, "Mobiles.bin" );

		public readonly static string ItemBasePath = Path.Combine( "Saves", "Items" );
		public readonly static string ItemIndexPath = Path.Combine( ItemBasePath, "Items.idx" );
		public readonly static string ItemTypesPath = Path.Combine( ItemBasePath, "Items.tdb" );
		public readonly static string ItemDataPath = Path.Combine( ItemBasePath, "Items.bin" );

		public readonly static string GuildBasePath = Path.Combine( "Saves", "Guilds" );
		public readonly static string GuildIndexPath = Path.Combine( GuildBasePath, "Guilds.idx" );
		public readonly static string GuildTypesPath = Path.Combine( GuildBasePath, "Guilds.tdb" );
		public readonly static string GuildDataPath = Path.Combine( GuildBasePath, "Guilds.bin" );
		
		public void NotifyDiskWriteComplete()
		{
			if ( m_DiskWriteHandle.Set() )
				Console.WriteLine( "World: Closing Save Files..." );
		}

		public void WaitForWriteCompletion()
		{
			m_DiskWriteHandle.WaitOne();
		}

		public int MobileCount
		{
			get { return m_Mobiles.Count; }
		}

		public int ItemCount
		{
			get { return m_Items.Count; }
		}

		public IEnumerable<Mobile> Mobiles
		{
			get { return m_Mobiles.Values; }
		}

		public IEnumerable<Item> Items
		{
			get { return m_Items.Values; }
		}

		public bool OnDelete( IEntity entity )
		{
			if ( m_Saving || m_Loading )
			{
				m_DeleteQueue.Enqueue( entity );

				return false;
			}

			return true;
		}

		public static void Broadcast( int hue, bool ascii, string text )
		{
			Packet p;

			if ( ascii )
				p = new AsciiMessage( Serial.MinusOne, -1, MessageType.Regular, hue, 3, "System", text );
			else
				p = new UnicodeMessage( Serial.MinusOne, -1, MessageType.Regular, hue, 3, "ENU", "System", text );
			
			p.Acquire();

			foreach ( var client in GameServer.Instance.Clients )
			{
				if ( client.Mobile != null )
				{
					client.Send( p );
					client.Flush();
				}
			}

			p.Release();
		}

		public static void Broadcast( int hue, bool ascii, string format, params object[] args )
		{
			Broadcast( hue, ascii, String.Format( format, args ) );
		}

		public void Load()
		{
			if ( m_Loaded )
				return;

			m_Loaded = true;

			Console.Write( "World: Loading..." );

			DateTime start = DateTime.UtcNow;

			m_Loading = true;

			m_AddQueue = new Queue<IEntity>();
			m_DeleteQueue = new Queue<IEntity>();

			LoadStrategy strategy = LoadStrategy.Acquire();
			strategy.LoadEntities( m_Repositories );

			EventSink.InvokeWorldLoad();

			m_Loading = false;

			ProcessSafetyQueues();

			foreach ( Item item in m_Items.Values )
			{
				if ( item.Parent == null )
					item.UpdateTotals();

				item.ClearProperties();
			}

			foreach ( Mobile m in m_Mobiles.Values )
			{
				m.UpdateRegion(); // Is this really needed?
				m.UpdateTotals();

				m.ClearProperties();
			}

			if ( ManualGC )
				System.GC.Collect();

			Console.WriteLine( String.Format( "done: {1} items, {2} mobiles ({0:F1} seconds)", ( DateTime.UtcNow - start ).TotalSeconds, m_Items.Count, m_Mobiles.Count ) );
		}

		private void ProcessSafetyQueues()
		{
			while ( m_AddQueue.Count > 0 )
			{
				IEntity entity = m_AddQueue.Dequeue();

				Item item = entity as Item;

				if ( item != null )
				{
					AddItem( item );
				}
				else
				{
					Mobile mob = entity as Mobile;

					if ( mob != null )
						AddMobile( mob );
				}
			}

			while ( m_DeleteQueue.Count > 0 )
			{
				IEntity entity = m_DeleteQueue.Dequeue();

				Item item = entity as Item;

				if ( item != null )
				{
					item.Delete();
				}
				else
				{
					Mobile mob = entity as Mobile;

					if ( mob != null )
						mob.Delete();
				}
			}
		}

		public void Save( bool message = true, bool permitBackgroundWrite = false )
		{
			if ( m_Saving )
				return;

			GameServer.Instance.Clients.Each( c => c.Flush() );

			WaitForWriteCompletion(); // Blocks Save until current disk flush is done.

			m_Saving = true;

			m_DiskWriteHandle.Reset();

			if ( message )
				Broadcast( 0x35, true, "The world is saving, please wait." );

			Console.WriteLine( "World: Save started" );

			SaveStrategy strategy = SaveStrategy.Acquire();
			Console.WriteLine( "Core: Using {0} save strategy", strategy.Name.ToLowerInvariant() );

			Stopwatch watch = Stopwatch.StartNew();

			try
			{
				EventSink.InvokeWorldBeforeSave();
			}
			catch ( Exception e )
			{
				throw new Exception( "World Before Save event threw an exception. Save failed!", e );
			}

			if ( !Directory.Exists( MobileBasePath ) )
				Directory.CreateDirectory( MobileBasePath );
			if ( !Directory.Exists( ItemBasePath ) )
				Directory.CreateDirectory( ItemBasePath );
			if ( !Directory.Exists( GuildBasePath ) )
				Directory.CreateDirectory( GuildBasePath );

			strategy.Save( permitBackgroundWrite );

			Console.WriteLine( "World: Entities saved in {0:F2} seconds.", watch.Elapsed.TotalSeconds );

			try
			{
				EventSink.InvokeWorldSave( new WorldSaveEventArgs( message ) );
			}
			catch ( Exception e )
			{
				throw new Exception( "World Save event threw an exception. Save failed!", e );
			}

			if ( ManualGC )
				System.GC.Collect();

			watch.Stop();

			m_Saving = false;

			if ( !permitBackgroundWrite )
				NotifyDiskWriteComplete(); // Sets the DiskWriteHandle. If we allow background writes, we leave this upto the individual save strategies.

			ProcessSafetyQueues();

			strategy.OnFinished();

			Console.WriteLine( "World: Save done in {0:F2} seconds.", watch.Elapsed.TotalSeconds );

			if ( message )
				Broadcast( 0x35, true, "World save complete. The entire process took {0:F2} seconds.", watch.Elapsed.TotalSeconds );
		}

		internal static List<Type> m_ItemTypes = new List<Type>();
		internal static List<Type> m_MobileTypes = new List<Type>();
		internal static List<Type> m_GuildTypes = new List<Type>();

		public IEntity FindEntity( Serial serial )
		{
			if ( serial.IsItem )
				return FindItem( serial );
			else if ( serial.IsMobile )
				return FindMobile( serial );

			return null;
		}

		public Mobile FindMobile( Serial serial )
		{
			Mobile mob;

			m_Mobiles.TryGetValue( serial, out mob );

			return mob;
		}

		public void AddMobile( Mobile m )
		{
			if ( m_Saving )
				m_AddQueue.Enqueue( m );
			else
				m_Mobiles[m.Serial] = m;
		}

		public Item FindItem( Serial serial )
		{
			Item item;

			m_Items.TryGetValue( serial, out item );

			return item;
		}

		public void AddItem( Item item )
		{
			if ( m_Saving )
				m_AddQueue.Enqueue( item );
			else
				m_Items[item.Serial] = item;
		}

		public void RemoveMobile( Mobile m )
		{
			m_Mobiles.Remove( m.Serial );
		}

		public void RemoveItem( Item item )
		{
			m_Items.Remove( item.Serial );
		}
	}
}