using System;

using Server;
using Server.Engines.Housing;
using Server.Multis;
using Server.Engines.VeteranRewards;

namespace Server.Items
{
	public class RewardBrazier : Item, IRewardItem
	{
		public override bool ForceShowProperties { get { return ObjectPropertyListPacket.Enabled; } }

		private bool m_IsRewardItem;

		[CommandProperty( AccessLevel.GameMaster )]
		public bool IsRewardItem
		{
			get { return m_IsRewardItem; }
			set { m_IsRewardItem = value; InvalidateProperties(); }
		}

		private Item m_Fire;

		private static int[] m_Art = new int[]
		{
			0x19AA, 0x19BB
		};

		[Constructable]
		public RewardBrazier()
			: base( Utility.RandomList( m_Art ) )
		{
			LootType = LootType.Blessed;
			Weight = 10.0;
		}

		public RewardBrazier( Serial serial )
			: base( serial )
		{
		}

		public void TurnOff()
		{
			if ( m_Fire != null )
			{
				m_Fire.Delete();
				m_Fire = null;
			}
		}

		public void TurnOn()
		{
			if ( m_Fire == null )
				m_Fire = new Item();

			m_Fire.ItemID = 0x19AB;
			m_Fire.Movable = false;
			m_Fire.MoveToWorld( new Point3D( X, Y, Z + ItemData.Height ), Map );
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( IsLockedDown )
			{
				IHouse house = HousingHelper.FindHouseAt( from );

				if ( house != null && house.IsCoOwner( from ) )
				{
					if ( m_Fire != null )
						TurnOff();
					else
						TurnOn();
				}
				else
					from.SendLocalizedMessage( 502436 ); // That is not accessible.
			}
			else
				from.SendLocalizedMessage( 502692 ); // This must be in a house and be locked down to work.
		}

		public override void OnLocationChange( Point3D old )
		{
			if ( m_Fire != null )
				m_Fire.MoveToWorld( new Point3D( X, Y, Z + ItemData.Height ), Map );
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			if ( m_IsRewardItem )
				list.Add( 1076222 ); // 6th Year Veteran Reward
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version

			writer.Write( (bool) m_IsRewardItem );
			writer.Write( (Item) m_Fire );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();

			m_IsRewardItem = reader.ReadBool();
			m_Fire = reader.ReadItem();
		}
	}

	public class RewardBrazierDeed : Item, IRewardItem
	{
		public override int LabelNumber { get { return 1080527; } } // Brazier Deed

		private bool m_IsRewardItem;

		[CommandProperty( AccessLevel.GameMaster )]
		public bool IsRewardItem
		{
			get { return m_IsRewardItem; }
			set { m_IsRewardItem = value; InvalidateProperties(); }
		}

		[Constructable]
		public RewardBrazierDeed()
			: base( 0x14F0 )
		{
			LootType = LootType.Blessed;
			Weight = 1.0;
		}

		public RewardBrazierDeed( Serial serial )
			: base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( m_IsRewardItem && !RewardSystem.CheckIsUsableBy( from, this, null ) )
				return;

			if ( IsChildOf( from.Backpack ) )
			{
				RewardBrazier brazier = new RewardBrazier();
				brazier.IsRewardItem = m_IsRewardItem;

				if ( !from.PlaceInBackpack( brazier ) )
				{
					brazier.Delete();
					from.SendLocalizedMessage( 500722 ); // You don't have enough room in your backpack!
				}
				else
					Delete();
			}
			else
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			if ( m_IsRewardItem )
				list.Add( 1076222 ); // 6th Year Veteran Reward
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version

			writer.Write( (bool) m_IsRewardItem );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadEncodedInt();

			m_IsRewardItem = reader.ReadBool();
		}
	}
}
