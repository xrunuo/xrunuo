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
using Server.Guilds;

namespace Server.Persistence
{
	public class StandardSaveStrategy : SaveStrategy
	{
		public override string Name
		{
			get { return "Standard"; }
		}

		private Queue<Item> m_DecayQueue;
		private Queue<IVendor> m_RestockQueue;

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
				World.Instance.NotifyDiskWriteComplete();
		}

		protected void SaveMobiles()
		{
			Dictionary<Serial, Mobile> mobiles = World.Instance.m_Mobiles;

			GenericWriter idx = new BinaryFileWriter( World.MobileIndexPath, false );
			GenericWriter tdb = new BinaryFileWriter( World.MobileTypesPath, false );
			GenericWriter bin = new BinaryFileWriter( World.MobileDataPath, true );

			idx.Write( (int) mobiles.Count );

			foreach ( Mobile m in mobiles.Values )
			{
				long start = bin.Position;

				idx.Write( (int) m.m_TypeRef );
				idx.Write( (int) m.Serial );
				idx.Write( (long) start );

				m.Serialize( bin );

				idx.Write( (int) ( bin.Position - start ) );

				if ( m is IVendor )
				{
					IVendor vendor = m as IVendor;

					if ( ( vendor.LastRestock + vendor.RestockDelay ) < DateTime.UtcNow )
						m_RestockQueue.Enqueue( vendor );
				}

				m.FreeCache();
			}

			tdb.Write( (int) World.m_MobileTypes.Count );

			for ( int i = 0; i < World.m_MobileTypes.Count; ++i )
				tdb.Write( World.m_MobileTypes[i].FullName );

			idx.Close();
			tdb.Close();
			bin.Close();
		}

		protected void SaveItems()
		{
			Dictionary<Serial, Item> items = World.Instance.m_Items;

			GenericWriter idx = new BinaryFileWriter( World.ItemIndexPath, false );
			GenericWriter tdb = new BinaryFileWriter( World.ItemTypesPath, false );
			GenericWriter bin = new BinaryFileWriter( World.ItemDataPath, true );

			idx.Write( (int) items.Count );
			foreach ( Item item in items.Values )
			{
				if ( item.Decays && item.Parent == null && item.Map != Map.Internal && ( item.LastMoved + item.DecayTime ) <= DateTime.UtcNow )
					m_DecayQueue.Enqueue( item );

				long start = bin.Position;

				idx.Write( (int) item.m_TypeRef );
				idx.Write( (int) item.Serial );
				idx.Write( (long) start );

				item.Serialize( bin );

				idx.Write( (int) ( bin.Position - start ) );

				item.FreeCache();
			}

			tdb.Write( (int) World.m_ItemTypes.Count );

			for ( int i = 0; i < World.m_ItemTypes.Count; ++i )
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

			idx.Write( (int) BaseGuild.List.Count );
			foreach ( BaseGuild guild in BaseGuild.List.Values )
			{
				long start = bin.Position;

				idx.Write( (int) guild.m_TypeRef );
				idx.Write( (int) guild.Serial );
				idx.Write( (long) start );

				guild.Serialize( bin );

				idx.Write( (int) ( bin.Position - start ) );
			}

			tdb.Write( (int) World.m_GuildTypes.Count );

			for ( int i = 0; i < World.m_GuildTypes.Count; ++i )
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
				Item item = m_DecayQueue.Dequeue();

				if ( item.OnDecay() )
					item.Delete();
			}
		}

		public void ProcessRestock()
		{
			while ( m_RestockQueue.Count > 0 )
			{
				IVendor vendor = m_RestockQueue.Dequeue();
				vendor.Restock();
				vendor.LastRestock = DateTime.UtcNow;
			}
		}
	}
}