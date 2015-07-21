using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Regions;

namespace Server.Engines.CannedEvil
{
	public class PitChampionSpawn : ChampionSpawn
	{
		private delegate Item CreateDecoItem();

		private List<Item> m_DecoItems;

		[Constructable]
		public PitChampionSpawn()
		{
			Type = ChampionSpawnType.Pit;
			RandomizeType = false;

			m_DecoItems = new List<Item>();
		}

		public override void OnSpawnActivated()
		{
			SpawnDeco( CreateBlood, 200 );
		}

		public override void OnSpawnDeactivated()
		{
			DeleteDeco();
		}

		public override void OnSubLevelAdvanced()
		{
			if ( GetSubLevel() == 1 )
			{
				SpawnDeco( CreateCrystal, 200 );
			}
		}

		private void SpawnDeco( CreateDecoItem creator, int amount )
		{
			for ( int i = 0; i < amount; i++ )
			{
				Item item = creator();

				item.MoveToWorld( GetSpawnLocation(), Map );

				m_DecoItems.Add( item );
			}
		}

		private void DeleteDeco()
		{
			for ( int i = 0; i < m_DecoItems.Count; i++ )
			{
				Item item = m_DecoItems[i];

				if ( item != null && !item.Deleted )
					item.Delete();
			}

			m_DecoItems.Clear();
		}

		private Item CreateBlood()
		{
			Item item = new Item( Utility.Random( 0x122A, 6 ) );
			item.Hue = Utility.RandomList( 0, 0x479, 0x47A, 0x47B, 0x485 );
			item.Movable = false;

			return item;
		}

		private Item CreateCrystal()
		{
			Item item = new Item( Utility.RandomList( 0x2206, 0x2207, 0x220A, 0x220D, 0x2211, 0x221E, 0x2220, 0x2226, 0x2229, 0x222A ) );
			item.Hue = Utility.RandomList( 0x497, 0x803, 0x804, 0x805, 0x806, 0x807, 0x808, 0x81B );
			item.Movable = false;

			return item;
		}

		public override void OnAfterDelete()
		{
			DeleteDeco();

			base.OnAfterDelete();
		}

		public PitChampionSpawn( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.WriteItemList( m_DecoItems );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();

			m_DecoItems = reader.ReadStrongItemList();
		}
	}
}