using System;
using Server;
using Server.Misc;
using Server.Multis;
using Server.Network;

namespace Server.Items
{
	public class GuardianTreasureChest : BaseTreasureChest
	{
		private static Rectangle2D m_Rect = new Rectangle2D( 356, 6, 19, 19 );

		private InternalTimer m_Timer;

		public int[] ItemIDs = new int[]
			{
				0xE41, 0xE43, 0x9AB
			};

		public void Fill()
		{
			Item item = Loot.RandomArmorOrShieldOrWeaponOrJewelry();

			if ( item is BaseArmor )
				BaseRunicTool.ApplyAttributesTo( (BaseArmor) item, 1, 10, 30 );
			else if ( item is BaseWeapon )
				BaseRunicTool.ApplyAttributesTo( (BaseWeapon) item, 1, 10, 30 );
			else if ( item is BaseJewel )
				BaseRunicTool.ApplyAttributesTo( (BaseJewel) item, 1, 10, 30 );

			DropItem( item );
		}

		[Constructable]
		public GuardianTreasureChest( int itemID )
			: base( itemID )
		{
			for ( int i = 0; i < 5; i++ )
				Fill();

			m_Timer = new InternalTimer( this );
			m_Timer.Start();
		}

		public GuardianTreasureChest( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version 
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();

			m_Timer = new InternalTimer( this );
			m_Timer.Start();
		}

		public override void OnAfterDelete()
		{
			if ( m_Timer != null )
				m_Timer.Stop();

			base.OnAfterDelete();
		}

		public class InternalTimer : Timer
		{
			public GuardianTreasureChest m_Chest;

			public InternalTimer( GuardianTreasureChest chest )
				: base( TimeSpan.FromMinutes( 1.0 ) )
			{
				m_Chest = chest;
			}

			protected override void OnTick()
			{
				if ( m_Chest != null )
					m_Chest.Delete();

				int x = Utility.Random( m_Rect.X, m_Rect.Width );
				int y = Utility.Random( m_Rect.Y, m_Rect.Height );
				int itemID = Utility.RandomList( m_Chest.ItemIDs );

				GuardianTreasureChest chest = new GuardianTreasureChest( itemID );
				chest.MoveToWorld( new Point3D( x, y, -1 ), Map.Malas );
			}
		}
	}
}