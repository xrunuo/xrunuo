using System;
using System.Collections.Generic;

using Server.Guilds;

namespace Server.Persistence
{
	public class StandardSaveStrategy : SaveStrategy
	{
		public override string Name => "Standard";

		private readonly Queue<Item> m_DecayQueue;
		private readonly Queue<IVendor> m_RestockQueue;

		public StandardSaveStrategy()
		{
			m_DecayQueue = new Queue<Item>();
			m_RestockQueue = new Queue<IVendor>();
		}

		public override void Save( bool permitBackgroundWrite )
		{
			SaveMobiles();
			SaveItems();
			SaveGuilds();

			if ( permitBackgroundWrite )
				World.NotifyDiskWriteComplete();
		}

		protected void SaveMobiles()
		{
			var mobiles = World.m_Mobiles;

			GenericWriter idx = new BinaryFileWriter( World.MobileIndexPath, false );
			GenericWriter tdb = new BinaryFileWriter( World.MobileTypesPath, false );
			GenericWriter bin = new BinaryFileWriter( World.MobileDataPath, true );

			idx.Write( mobiles.Count );

			foreach ( var m in mobiles.Values )
			{
				var start = bin.Position;

				idx.Write( m.m_TypeRef );
				idx.Write( m.Serial );
				idx.Write( start );

				m.Serialize( bin );

				idx.Write( (int) ( bin.Position - start ) );

				if ( m is IVendor )
				{
					var vendor = m as IVendor;

					if ( ( vendor.LastRestock + vendor.RestockDelay ) < DateTime.UtcNow )
						m_RestockQueue.Enqueue( vendor );
				}

				m.FreeCache();
			}

			tdb.Write( World.m_MobileTypes.Count );

			for ( var i = 0; i < World.m_MobileTypes.Count; ++i )
				tdb.Write( World.m_MobileTypes[i].FullName );

			idx.Close();
			tdb.Close();
			bin.Close();
		}

		protected void SaveItems()
		{
			var items = World.m_Items;

			GenericWriter idx = new BinaryFileWriter( World.ItemIndexPath, false );
			GenericWriter tdb = new BinaryFileWriter( World.ItemTypesPath, false );
			GenericWriter bin = new BinaryFileWriter( World.ItemDataPath, true );

			idx.Write( items.Count );
			foreach ( var item in items.Values )
			{
				if ( item.Decays && item.Parent == null && item.Map != Map.Internal && ( item.LastMoved + item.DecayTime ) <= DateTime.UtcNow )
					m_DecayQueue.Enqueue( item );

				var start = bin.Position;

				idx.Write( item.m_TypeRef );
				idx.Write( item.Serial );
				idx.Write( start );

				item.Serialize( bin );

				idx.Write( (int) ( bin.Position - start ) );

				item.FreeCache();
			}

			tdb.Write( World.m_ItemTypes.Count );

			for ( var i = 0; i < World.m_ItemTypes.Count; ++i )
				tdb.Write( World.m_ItemTypes[i].FullName );

			idx.Close();
			tdb.Close();
			bin.Close();
		}

		protected void SaveGuilds()
		{
			GenericWriter idx = new BinaryFileWriter( World.GuildIndexPath, false );
			GenericWriter tdb = new BinaryFileWriter( World.GuildTypesPath, false );
			GenericWriter bin = new BinaryFileWriter( World.GuildDataPath, true );

			idx.Write( BaseGuild.List.Count );
			foreach ( var guild in BaseGuild.List.Values )
			{
				var start = bin.Position;

				idx.Write( guild.m_TypeRef );
				idx.Write( guild.Serial );
				idx.Write( start );

				guild.Serialize( bin );

				idx.Write( (int) ( bin.Position - start ) );
			}

			tdb.Write( World.m_GuildTypes.Count );

			for ( var i = 0; i < World.m_GuildTypes.Count; ++i )
				tdb.Write( World.m_GuildTypes[i].FullName );

			idx.Close();
			tdb.Close();
			bin.Close();
		}

		public override void OnFinished()
		{
			ProcessDecay();
			ProcessRestock();
		}

		public void ProcessDecay()
		{
			while ( m_DecayQueue.Count > 0 )
			{
				var item = m_DecayQueue.Dequeue();

				if ( item.OnDecay() )
					item.Delete();
			}
		}

		public void ProcessRestock()
		{
			while ( m_RestockQueue.Count > 0 )
			{
				var vendor = m_RestockQueue.Dequeue();
				vendor.Restock();
				vendor.LastRestock = DateTime.UtcNow;
			}
		}
	}
}
