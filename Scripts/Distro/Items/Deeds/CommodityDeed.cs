using System;
using Server.Targeting;
using Server.Network;

namespace Server.Items
{
	public interface ICommodity
	{
	}

	public class CommodityDeed : Item
	{
		private Item m_Commodity;

		[CommandProperty( AccessLevel.GameMaster )]
		public Item Commodity { get { return m_Commodity; } }

		public bool SetCommodity( Item item )
		{
			InvalidateProperties();

			if ( m_Commodity == null && item is ICommodity )
			{
				m_Commodity = item;
				m_Commodity.Internalize();

				Hue = 0x592;

				InvalidateProperties();

				return true;
			}
			else
			{
				return false;
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.Write( m_Commodity );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 0:
					{
						m_Commodity = reader.ReadItem();

						break;
					}
			}
		}

		public CommodityDeed( Item commodity )
			: base( 0x14F0 )
		{
			Weight = 1.0;
			Hue = 0x47;

			m_Commodity = commodity;

			LootType = LootType.Blessed;
		}

		[Constructable]
		public CommodityDeed()
			: this( null )
		{
		}

		public CommodityDeed( Serial serial )
			: base( serial )
		{
		}

		public override void OnDelete()
		{
			if ( m_Commodity != null )
				m_Commodity.Delete();

			base.OnDelete();
		}

		public override LocalizedText GetNameProperty()
		{
			if ( m_Commodity != null )
			{
				return new LocalizedText( 1115599, "{0}\t#{1}", m_Commodity.Amount, m_Commodity.LabelNumber ); // commodity deed worth ~1_quantity~ ~2_item~
			}
			else
			{
				return new LocalizedText( 1047016 ); // commodity deed
			}
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			if ( m_Commodity != null )
			{
				list.Add( 1060747 ); // filled
			}
			else
			{
				list.Add( 1060748 ); // unfilled
			}
		}

		public override void OnDoubleClick( Mobile from )
		{
			int number;

			BankBox box = from.FindBankNoCreate();
			CommodityDeedBox cox = CommodityDeedBox.Find( this );

			// Veteran Rewards mods
			if ( m_Commodity != null )
			{
				if ( box != null && IsChildOf( box ) )
				{
					number = 1047031; // The commodity has been redeemed.


					box.DropItem( m_Commodity );

					m_Commodity = null;
					Delete();
				}
				else if ( cox != null )
				{
					if ( cox.IsSecure )
					{
						number = 1047031; // The commodity has been redeemed.
						m_Commodity.InstanceID = cox.InstanceID;
						cox.DropItem( m_Commodity );

						m_Commodity = null;
						Delete();
					}
					else
						number = 1080525; // The commodity deed box must be secured before you can use it.
				}
				else
				{
					number = 1080526; // That must be in your bank box or commodity deed box to use it.
				}
			}
			else if ( cox != null && !cox.IsSecure )
			{
				number = 1080525; // The commodity deed box must be secured before you can use it.
			}
			else if ( ( box == null || !IsChildOf( box ) ) && cox == null )
			{
				number = 1080526; // That must be in your bank box or commodity deed box to use it.
			}
			else
			{
				number = 1047029; // Target the commodity to fill this deed with.

				from.Target = new InternalTarget( this );
			}

			from.SendLocalizedMessage( number );
		}
		private class InternalTarget : Target
		{
			private CommodityDeed m_Deed;

			public InternalTarget( CommodityDeed deed )
				: base( 3, false, TargetFlags.None )
			{
				m_Deed = deed;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				if ( m_Deed.Deleted )
					return;

				int number;

				if ( m_Deed.Commodity != null )
				{
					number = 1047028; // The commodity deed has already been filled.
				}
				else if ( targeted is Item )
				{
					Container box = m_Deed.Parent as Container;

					if ( box == null || ( (Item) targeted ).Parent != box )
					{
						number = 1080526; // That must be in your bank box or commodity deed box to use it.
					}
					else
					{
						if ( m_Deed.SetCommodity( (Item) targeted ) )
							number = 1047030; // The commodity deed has been filled.
						else
							number = 1047027; // That is not a commodity the bankers will fill a commodity deed with.
					}
				}
				else
				{
					number = 1047027; // That is not a commodity the bankers will fill a commodity deed with.
				}

				from.SendLocalizedMessage( number );
			}
		}
	}
}
