using System;
using System.Collections.Generic;
using Server;
using Server.Engines.Imbuing;
using Server.Factions;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Gumps
{
	public class ItemInsuranceInfo
	{
		private Item m_Item;
		private bool m_Toggled;

		public Item Item { get { return m_Item; } }
		public bool Toggled { get { return m_Toggled; } }

		public void Toggle()
		{
			m_Toggled = !m_Toggled;
		}

		public bool NowInsured()
		{
			return m_Item.Insured ^ m_Toggled;
		}

		public int GetCost()
		{
			return ItemInsuranceHelper.GetInsuranceCost( m_Item );
		}

		public ItemInsuranceInfo( Item item )
		{
			m_Item = item;
			m_Toggled = false;
		}
	}

	public class ItemInsuranceMenu : Gump
	{
		public override int TypeID { get { return 0xF3E9F; } }

		private const int ItemsPerPage = 4;

		private PlayerMobile m_Owner;
		private ItemInsuranceInfo[] m_ItemInsuranceInfo;
		private int m_Page;

		public ItemInsuranceMenu( PlayerMobile pm, ItemInsuranceInfo[] infoCollection )
			: this( pm, infoCollection, 0 )
		{
		}

		public ItemInsuranceMenu( PlayerMobile pm, ItemInsuranceInfo[] infoCollection, int page )
			: base( 25, 50 )
		{
			if ( page < 0 )
				page = 0;

			m_Owner = pm;
			m_ItemInsuranceInfo = infoCollection;
			m_Page = page;

			int totalGold = Banker.GetBalance( pm );
			int totalCost = ComputeTotalCost();
			int totalDeaths = ( totalCost == 0 ? 0 : totalGold / totalCost );

			AddPage( 0 );

			AddBackground( 0, 0, 520, 510, 0x13BE );
			AddImageTiled( 10, 10, 500, 30, 0xA40 );
			AddImageTiled( 10, 50, 500, 355, 0xA40 );
			AddImageTiled( 10, 415, 500, 80, 0xA40 );
			AddAlphaRegion( 10, 10, 500, 485 );

			AddHtmlLocalized( 10, 14, 150, 20, 1114121, 0x7FFF, false, false ); // <CENTER>ITEM INSURANCE MENU</CENTER>

			AddHtmlLocalized( 15, 420, 300, 20, 1114310, 0x7FFF, false, false ); // GOLD AVAILABLE:
			AddLabel( 215, 420, 0x481, totalGold.ToString() );
			AddHtmlLocalized( 15, 435, 300, 20, 1114123, 0x7FFF, false, false ); // TOTAL COST OF INSURANCE:
			AddLabel( 215, 435, 0x481, totalCost.ToString() );
			AddHtmlLocalized( 15, 450, 300, 20, 1114125, 0x7FFF, false, false ); // NUMBER OF DEATHS PAYABLE:
			AddLabel( 215, 450, 0x481, totalDeaths.ToString() );

			AddHtmlLocalized( 395, 14, 105, 20, 1114122, 0x7FFF, false, false ); // AUTO REINSURE
			AddCheckButton( 360, 10, 1, pm.AutoRenewInsurance );

			AddHtmlLocalized( 45, 54, 70, 20, 1062214, 0x7FFF, false, false ); // Item
			AddHtmlLocalized( 250, 54, 70, 20, 1061038, 0x7FFF, false, false ); // Cost
			AddHtmlLocalized( 400, 54, 70, 20, 1114311, 0x7FFF, false, false ); // Insured

			int idx = page * ItemsPerPage;

			for ( int i = 0; i < ItemsPerPage && idx < m_ItemInsuranceInfo.Length; i++, idx++ )
			{
				ItemInsuranceInfo info = m_ItemInsuranceInfo[idx];

				AddButtonTileArt( 40, 72 + ( i * 75 ), 0x918, 0x918, GumpButtonType.Page, 0, 0, info.Item.ItemID, info.Item.Hue, 23, 5 );
				AddItemProperty( info.Item );
				AddCheckButton( 400, 72 + ( i * 75 ), 100 + idx, info.NowInsured() );
				AddLabel( 250, 72 + ( i * 75 ), info.NowInsured() ? 0x481 : 0x66C, info.GetCost().ToString() );
			}

			if ( page > 0 )
			{
				AddHtmlLocalized( 50, 380, 450, 20, 1044044, 0x7FFF, false, false ); // PREV PAGE
				AddButton( 15, 380, 0xFAE, 0xFAF, 503, GumpButtonType.Reply, 0 );
			}

			if ( idx < m_ItemInsuranceInfo.Length )
			{
				AddHtmlLocalized( 435, 380, 70, 20, 1044045, 0x7FFF, false, false ); // NEXT PAGE
				AddButton( 400, 380, 0xFA5, 0xFA7, 505, GumpButtonType.Reply, 0 );
			}

			AddHtmlLocalized( 430, 472, 50, 20, 1006044, 0x7FFF, false, false ); // OK
			AddButton( 395, 470, 0xFA5, 0xFA6, 2, GumpButtonType.Reply, 0 );

			AddHtmlLocalized( 50, 472, 80, 20, 1011012, 0x7FFF, false, false ); // CANCEL
			AddButton( 15, 470, 0xFB1, 0xFB2, 0, GumpButtonType.Reply, 0 );
		}

		private void AddCheckButton( int x, int y, int buttonId, bool checkd )
		{
			if ( checkd )
				AddButton( x, y, 0x25FB, 0x25FC, buttonId, GumpButtonType.Reply, 0 );
			else
				AddButton( x, y, 0x25F8, 0x25FA, buttonId, GumpButtonType.Reply, 0 );
		}

		public override void OnResponse( GameClient sender, RelayInfo info )
		{
			if ( !m_Owner.CheckAlive() )
				return;

			switch ( info.ButtonID )
			{
				case 0: // Cancel
					{
						break;
					}
				case 1: // Toggle Auto Renew
					{
						if ( m_Owner.AutoRenewInsurance )
						{
							m_Owner.SendGump( new ConfirmationCancelInsuranceGump( m_Owner, m_ItemInsuranceInfo ) );
						}
						else
						{
							m_Owner.SendLocalizedMessage( 1060881, String.Empty, 0x23 ); // You have selected to automatically reinsure all insured items upon death
							m_Owner.AutoRenewInsurance = true;

							m_Owner.SendGump( new ItemInsuranceMenu( m_Owner, m_ItemInsuranceInfo, m_Page ) );
						}

						break;
					}
				case 2: // OK
					{
						m_Owner.CloseGump( typeof( ConfirmInsureGump ) );
						m_Owner.SendGump( new ConfirmInsureGump( m_Owner, m_ItemInsuranceInfo ) );

						break;
					}
				case 503: // Previous page
					{
						m_Owner.CloseGump( typeof( ItemInsuranceMenu ) );
						m_Owner.SendGump( new ItemInsuranceMenu( m_Owner, m_ItemInsuranceInfo, m_Page - 1 ) );

						break;
					}
				case 505: // Next page
					{
						m_Owner.CloseGump( typeof( ItemInsuranceMenu ) );
						m_Owner.SendGump( new ItemInsuranceMenu( m_Owner, m_ItemInsuranceInfo, m_Page + 1 ) );

						break;
					}
				default:
					{
						int idx = info.ButtonID - 100;

						if ( idx >= 0 && idx < m_ItemInsuranceInfo.Length )
						{
							m_ItemInsuranceInfo[idx].Toggle();
							m_Owner.SendGump( new ItemInsuranceMenu( m_Owner, m_ItemInsuranceInfo, m_Page ) );
						}

						break;
					}
			}
		}

		private int ComputeTotalCost()
		{
			int total = 0;

			foreach ( ItemInsuranceInfo info in m_ItemInsuranceInfo )
			{
				if ( info.NowInsured() )
					total += info.GetCost();
			}

			return total;
		}

		private static void RecurseSelectItems( List<ItemInsuranceInfo> list, Container cont )
		{
			foreach ( Item item in cont.Items )
			{
				if ( item is Container && !( item is BaseQuiver ) )
					RecurseSelectItems( list, (Container) item );
				else if ( ItemInsuranceHelper.CanInsure( item ) )
					list.Add( new ItemInsuranceInfo( item ) );
			}
		}

		public static void SendGump( PlayerMobile pm )
		{
			List<ItemInsuranceInfo> list = new List<ItemInsuranceInfo>();

			foreach ( Item item in pm.GetEquippedItems() )
			{
				if ( ItemInsuranceHelper.CanInsure( item ) )
					list.Add( new ItemInsuranceInfo( item ) );
			}

			if ( pm.Backpack != null )
				RecurseSelectItems( list, pm.Backpack );

			if ( list.Count > 0 )
			{
				ItemInsuranceInfo[] col = list.ToArray();

				pm.CloseGump( typeof( ItemInsuranceMenu ) );
				pm.SendGump( new ItemInsuranceMenu( pm, col ) );
			}
			else
			{
				pm.SendLocalizedMessage( 1114915, String.Empty, 53 ); // None of your current items meet the requirements for insurance.
			}
		}
	}

	public class ItemInsuranceHelper
	{
		public static bool CanInsure( Item item )
		{
			if ( !item.CanInsure )
				return false;

			if ( item.Stackable )
				return false;

			if ( item.LootType != LootType.Regular )
				return false;

			if ( item.ItemID == 0x204E ) // death shroud
				return false;

			Container parent = item.Parent as Container;

			if ( parent != null && parent.IsLockedContainer )
				return false;

			return true;
		}

		public static int GetInsuranceCost( Item item )
		{
			int cost;

			if ( item is ICollectionItem )
				cost = 600;
			else if ( item is ISetItem )
				cost = 600;
			else if ( item is IFactionArtifact )
				cost = 800;
			else if ( item is IImbuable )
			{
				int intensity = new PropCollection( item ).WeightedIntensity;

				if ( item is BaseWeapon )
				{
					BaseWeapon weapon = item as BaseWeapon;

					cost = intensity;

					if ( weapon.Attributes.WeaponDamage <= 40 )
						cost -= ( weapon.Attributes.WeaponDamage * 2 );
				}
				else if ( item is BaseArmor )
				{
					BaseArmor armor = item as BaseArmor;

					int totalResist = armor.PhysicalResistance + armor.FireResistance + armor.ColdResistance + armor.PoisonResistance + armor.EnergyResistance;

					cost = (int) Math.Ceiling( intensity + ( totalResist * 0.1 ) );

					if ( intensity < 200 )
						cost += (int) Math.Ceiling( intensity * 0.35 );
				}
				else
				{
					cost = intensity;
				}
			}
			else
				cost = BaseVendor.GetVendorPrice( item.GetType() );

			Utility.FixMinMax( ref cost, 10, 800 );

			return cost;
		}
	}
}