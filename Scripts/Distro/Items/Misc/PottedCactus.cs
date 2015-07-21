using System;
using Server;
using Server.Engines.VeteranRewards;

namespace Server.Items
{
	public class RewardPottedCactus : Item, IRewardItem
	{
		private bool m_IsRewardItem;

		[CommandProperty( AccessLevel.GameMaster )]
		public bool IsRewardItem
		{
			get { return m_IsRewardItem; }
			set { m_IsRewardItem = value; }
		}

		[Constructable]
		public RewardPottedCactus()
			: base( Utility.RandomMinMax( 0x1E0F, 0x1E14 ) )
		{
			Weight = 5.0;
		}

		public RewardPottedCactus( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
		}
	}

	public class PottedCactusDeed : Item, IRewardItem
	{
		public override int LabelNumber { get { return 1080407; } } // Potted Cactus Deed

		private bool m_IsRewardItem;

		[CommandProperty( AccessLevel.GameMaster )]
		public bool IsRewardItem
		{
			get { return m_IsRewardItem; }
			set { m_IsRewardItem = value; InvalidateProperties(); }
		}

		[Constructable]
		public PottedCactusDeed()
			: base( 0x14F0 )
		{
			LootType = LootType.Blessed;
			Weight = 1.0;
		}

		public PottedCactusDeed( Serial serial )
			: base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( m_IsRewardItem && !RewardSystem.CheckIsUsableBy( from, this, null ) )
				return;

			if ( IsChildOf( from.Backpack ) )
			{
				RewardPottedCactus cactus = new RewardPottedCactus();
				cactus.IsRewardItem = m_IsRewardItem;

				if ( !from.PlaceInBackpack( cactus ) )
				{
					cactus.Delete();
					from.SendLocalizedMessage( 1078837 ); // Your backpack is full! Please make room and try again.
				}
				else
					Delete();
			}
			else
				from.SendLocalizedMessage( 1062334 ); // This item must be in your backpack to be used.    
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			if ( m_IsRewardItem )
				list.Add( 1076219 ); // 3rd Year Veteran Reward
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

			int version = reader.ReadEncodedInt();

			m_IsRewardItem = reader.ReadBool();
		}
	}
}