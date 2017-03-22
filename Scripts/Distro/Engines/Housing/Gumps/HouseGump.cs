using System;
using System.Collections;
using System.Collections.Generic;
using Server.Engines.Housing.Items;
using Server.Engines.Housing.Multis;
using Server.Engines.Housing.Prompts;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Multis;
using Server.Network;
using Server.Prompts;

namespace Server.Engines.Housing.Gumps
{
	public enum HouseGumpPage
	{
		Information,
		Security,
		Storage,
		Customize,
		Ownership,
		ChangeHanger,
		ChangeFoundation,
		ChangeSign,
		RemoveCoOwner,
		ListCoOwner,
		RemoveFriend,
		ListFriend,
		RemoveBan,
		ListBan,
		RemoveAccess,
		ListAccess,
		ChangePost,
		Vendors
	}

	public class HouseGump : Gump
	{
		private BaseHouse m_House;
		private HouseGumpPage m_Page;

		private const int LabelColor = 0x7FFF;
		private const int SelectedColor = 0x421F;
		private const int DisabledColor = 0x4210;

		private const int LabelHue = 0x481;
		private const int HighlightedLabelHue = 0x64;

		private ArrayList m_List;

		private string GetOwnerName()
		{
			Mobile m = m_House.Owner;

			if ( m == null || m.Deleted )
			{
				return "(unowned)";
			}

			string name;

			if ( ( name = m.Name ) == null || ( name = name.Trim() ).Length <= 0 )
			{
				name = "(no name)";
			}

			return name;
		}

		private string GetDateTime( DateTime val )
		{
			if ( val == DateTime.MinValue )
			{
				return "";
			}

			return val.ToString( "yyyy'-'MM'-'dd HH':'mm':'ss" );
		}

		public void AddPageButton( int x, int y, int buttonID, int number, HouseGumpPage page )
		{
			bool isSelection = ( m_Page == page );

			AddButton( x, y, isSelection ? 4006 : 4005, 4007, buttonID, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( x + 45, y, 200, 20, number, isSelection ? SelectedColor : LabelColor, false, false );
		}

		public void AddButtonLabeled( int x, int y, int buttonID, int number )
		{
			AddButtonLabeled( x, y, buttonID, number, true );
		}

		public void AddButtonLabeled( int x, int y, int buttonID, int number, bool enabled )
		{
			if ( enabled )
			{
				AddButton( x, y, 4005, 4007, buttonID, GumpButtonType.Reply, 0 );
			}

			AddHtmlLocalized( x + 35, y, 240, 20, number, enabled ? LabelColor : DisabledColor, false, false );
		}

		public void AddList( ArrayList list, int button, bool accountOf, bool leadingStar, Mobile from )
		{
			if ( list == null )
			{
				return;
			}

			m_List = new ArrayList( list );

			int lastPage = 0;
			int index = 0;

			for ( int i = 0; i < list.Count; ++i )
			{
				int offset = index % 10;
				int page = 1 + ( index / 10 );

				if ( page != lastPage )
				{
					if ( lastPage != 0 )
					{
						AddButton( 40, 360, 4005, 4007, 0, GumpButtonType.Page, page );
					}

					AddPage( page );

					if ( lastPage != 0 )
					{
						AddButton( 10, 360, 4014, 4016, 0, GumpButtonType.Page, lastPage );
					}

					lastPage = page;
				}


				Mobile m = (Mobile) list[i];

				string name;
				int labelHue = LabelHue;

				if ( m is PlayerVendor )
				{
					PlayerVendor vendor = (PlayerVendor) m;

					name = vendor.ShopName;

					if ( vendor.IsOwner( from ) )
					{
						labelHue = HighlightedLabelHue;
					}
				}
				else if ( m != null )
				{
					name = m.Name;
				}
				else
				{
					continue;
				}

				if ( ( name = name.Trim() ).Length <= 0 )
				{
					continue;
				}

				if ( button != -1 )
				{
					AddButton( 10, 150 + ( offset * 20 ), 4005, 4007, GetButtonID( button, i ), GumpButtonType.Reply, 0 );
				}

				if ( accountOf && m.Player && m.Account != null )
				{
					name = "Account of " + name;
				}

				if ( leadingStar )
				{
					name = "* " + name;
				}

				AddLabel( button > 0 ? 45 : 10, 150 + ( offset * 20 ), labelHue, name );
				++index;
			}
		}

		public static int GetButtonID( int type, int index )
		{
			return 1 + ( index * 15 ) + type;
		}

		private static int[] m_HangerNumbers = new int[]
			{
				2968, 2970, 2972,
				2974, 2976, 2978
			};

		private static int[] m_FoundationNumbers = new int[]
			{
				// Default
				20, 189, 765, 65, 101,

				// Mondain's Legacy
				11767, 11771, 11207, 11715,

				// 9th Anniversary
				13938, 13942,

				// Stygian Abyss
				16806, 16732
			};

		private static int[] m_PostNumbers = new int[]
			{
				9, 29, 54, 90, 147, 169,
				177, 204, 251, 257, 263,
				298, 347, 424, 441, 466,
				514, 600, 601, 602, 603,
				660, 666, 672, 898, 970,
				974, 982
			};

		public HouseGump( HouseGumpPage page, Mobile from, BaseHouse house )
			: base( 50, 40 )
		{
			m_House = house;
			m_Page = page;

			from.CloseGump( typeof( HouseGump ) );
			//from.CloseGump( typeof( HouseListGump ) );
			//from.CloseGump( typeof( HouseRemoveGump ) );

			bool isCombatRestricted = house.IsCombatRestricted( from );

			bool isOwner = house.IsOwner( from );
			bool isCoOwner = isOwner || house.IsCoOwner( from );
			bool isFriend = isCoOwner || house.IsFriend( from );

			if ( isCombatRestricted )
			{
				isFriend = isCoOwner = isOwner = false;
			}

			AddPage( 0 );

			if ( isFriend || page == HouseGumpPage.Vendors )
			{
				AddBackground( 0, 0, 420, page != HouseGumpPage.Vendors ? 440 : 420, 5054 );

				AddImageTiled( 10, 10, 400, 100, 2624 );
				AddAlphaRegion( 10, 10, 400, 100 );

				AddImageTiled( 10, 120, 400, 260, 2624 );
				AddAlphaRegion( 10, 120, 400, 260 );

				AddImageTiled( 10, 390, 400, page != HouseGumpPage.Vendors ? 40 : 20, 2624 );
				AddAlphaRegion( 10, 390, 400, page != HouseGumpPage.Vendors ? 40 : 20 );

				AddButtonLabeled( 250, page != HouseGumpPage.Vendors ? 410 : 390, 0, 1060675 ); // CLOSE
			}

			AddImage( 10, 10, 100 );

			if ( m_House.Sign != null )
			{
				ArrayList lines = Wrap( m_House.Sign.Name );

				if ( lines != null )
				{
					for ( int i = 0, y = ( 114 - ( lines.Count * 14 ) ) / 2; i < lines.Count; ++i, y += 14 )
					{
						string s = (string) lines[i];

						AddLabel( 10 + ( ( 160 - ( s.Length * 8 ) ) / 2 ), y, 0, s );
					}
				}
			}

			if ( page == HouseGumpPage.Vendors )
			{
				AddHtmlLocalized( 10, 120, 400, 20, 1062428, LabelColor, false, false ); // <CENTER>SHOPS</CENTER>

				AddList( house.AvailableVendorsFor( from ), 1, false, false, from );
				return;
			}

			if ( !isFriend )
			{
				return;
			}

			if ( house.Public )
			{
				AddButtonLabeled( 10, 390, GetButtonID( 0, 0 ), 1060674 ); // Banish
				AddButtonLabeled( 10, 410, GetButtonID( 0, 1 ), 1011261 ); // Lift a Ban
			}
			else
			{
				AddButtonLabeled( 10, 390, GetButtonID( 0, 2 ), 1060676 ); // Grant Access
				AddButtonLabeled( 10, 410, GetButtonID( 0, 3 ), 1060677 ); // Revoke Access
			}

			AddPageButton( 150, 10, GetButtonID( 1, 0 ), 1060668, HouseGumpPage.Information );
			AddPageButton( 150, 30, GetButtonID( 1, 1 ), 1060669, HouseGumpPage.Security );
			AddPageButton( 150, 50, GetButtonID( 1, 2 ), 1060670, HouseGumpPage.Storage );
			AddPageButton( 150, 70, GetButtonID( 1, 3 ), 1060671, HouseGumpPage.Customize );
			AddPageButton( 150, 90, GetButtonID( 1, 4 ), 1060672, HouseGumpPage.Ownership );

			switch ( page )
			{
				case HouseGumpPage.Information:
					{
						AddHtmlLocalized( 20, 130, 200, 20, 1011242, LabelColor, false, false ); // Owned By: 
						AddLabel( 210, 130, LabelHue, GetOwnerName() );

						AddHtmlLocalized( 20, 170, 380, 20, 1018032, SelectedColor, false, false ); // This house is properly placed.
						AddHtmlLocalized( 20, 190, 380, 20, 1018035, SelectedColor, false, false ); // This house is of modern design.
						AddHtmlLocalized( 20, 210, 380, 20, ( house is HouseFoundation ) ? 1060681 : 1060680, SelectedColor, false, false ); // This is a (pre | custom)-built house.
						AddHtmlLocalized( 20, 230, 380, 20, house.Public ? 1060678 : 1060679, SelectedColor, false, false ); // This house is (private | open to the public).

						switch ( house.DecayType )
						{
							case DecayType.Ageless:
							case DecayType.AutoRefresh:
								{
									AddHtmlLocalized( 20, 250, 380, 20, 1062209, SelectedColor, false, false ); // This house is <a href = "?ForceTopic97">Automatically</a> refreshed.
									break;
								}
							case DecayType.ManualRefresh:
								{
									AddHtmlLocalized( 20, 250, 380, 20, 1062208, SelectedColor, false, false ); // This house is <a href = "?ForceTopic97">Grandfathered</a>.
									break;
								}
							case DecayType.Condemned:
								{
									AddHtmlLocalized( 20, 250, 380, 20, 1062207, SelectedColor, false, false ); // This house is <a href = "?ForceTopic97">Condemned</a>.
									break;
								}
						}

						AddHtmlLocalized( 20, 290, 200, 20, 1060692, SelectedColor, false, false ); // Built On:
						AddLabel( 250, 290, LabelHue, GetDateTime( house.BuiltOn ) );

						AddHtmlLocalized( 20, 310, 200, 20, 1060693, SelectedColor, false, false ); // Last Traded:
						AddLabel( 250, 310, LabelHue, GetDateTime( house.LastTraded ) );

						AddHtmlLocalized( 20, 330, 200, 20, 1061793, SelectedColor, false, false ); // House Value
						AddLabel( 250, 330, LabelHue, house.Price.ToString() );

						AddHtmlLocalized( 20, 360, 300, 20, 1011241, SelectedColor, false, false ); // Number of visits this building has had: 
						AddLabel( 350, 360, LabelHue, house.Visits.ToString() );

						break;
					}
				case HouseGumpPage.Security:
					{
						AddButtonLabeled( 10, 130, GetButtonID( 3, 0 ), 1011266, isCoOwner ); // View Co-Owner List
						AddButtonLabeled( 10, 150, GetButtonID( 3, 1 ), 1011267, isOwner ); // Add a Co-Owner
						AddButtonLabeled( 10, 170, GetButtonID( 3, 2 ), 1018036, isOwner ); // Remove a Co-Owner
						AddButtonLabeled( 10, 190, GetButtonID( 3, 3 ), 1011268, isOwner ); // Clear Co-Owner List

						AddButtonLabeled( 10, 220, GetButtonID( 3, 4 ), 1011243 ); // View Friends List
						AddButtonLabeled( 10, 240, GetButtonID( 3, 5 ), 1011244, isCoOwner ); // Add a Friend
						AddButtonLabeled( 10, 260, GetButtonID( 3, 6 ), 1018037, isCoOwner ); // Remove a Friend
						AddButtonLabeled( 10, 280, GetButtonID( 3, 7 ), 1011245, isCoOwner ); // Clear Friend List

						if ( house.Public )
						{
							AddButtonLabeled( 10, 310, GetButtonID( 3, 8 ), 1011260 ); // View Ban List
							AddButtonLabeled( 10, 330, GetButtonID( 3, 9 ), 1060698 ); // Clear Ban List

							AddButtonLabeled( 210, 130, GetButtonID( 3, 12 ), 1060695, isOwner ); // Change to Private

							AddHtmlLocalized( 245, 150, 240, 20, 1060694, SelectedColor, false, false ); // Change to Public
						}
						else
						{
							AddButtonLabeled( 10, 310, GetButtonID( 3, 10 ), 1060699 ); // View Access List
							AddButtonLabeled( 10, 330, GetButtonID( 3, 11 ), 1060700 ); // Clear Access List

							AddHtmlLocalized( 245, 130, 240, 20, 1060695, SelectedColor, false, false ); // Change to Private

							AddButtonLabeled( 210, 150, GetButtonID( 3, 13 ), 1060694, isOwner ); // Change to Public
						}

						break;
					}
				case HouseGumpPage.Storage:
					{
						AddHtmlLocalized( 10, 130, 400, 20, 1060682, LabelColor, false, false ); // <CENTER>HOUSE STORAGE SUMMARY</CENTER>

						// This is not as OSI; storage changes not yet implemented

						/*
						AddHtmlLocalized( 10, 170, 275, 20, 1011237, LabelColor, false, false ); // Number of locked down items:
						AddLabel( 310, 170, LabelHue, m_House.LockDownCount.ToString() );

						AddHtmlLocalized( 10, 190, 275, 20, 1011238, LabelColor, false, false ); // Maximum locked down items:
						AddLabel( 310, 190, LabelHue, m_House.MaxLockDowns.ToString() );

						AddHtmlLocalized( 10, 210, 275, 20, 1011239, LabelColor, false, false ); // Number of secure containers:
						AddLabel( 310, 210, LabelHue, m_House.SecureCount.ToString() );

						AddHtmlLocalized( 10, 230, 275, 20, 1011240, LabelColor, false, false ); // Maximum number of secure containers:
						AddLabel( 310, 230, LabelHue, m_House.MaxSecures.ToString() );
						*/

						int fromSecures, fromVendors, fromLockdowns, fromMovingCrate;

						int maxSecures = house.GetAosMaxSecures();
						int curSecures = house.GetAosCurSecures( out fromSecures, out fromVendors, out fromLockdowns, out fromMovingCrate );

						int maxLockdowns = house.GetAosMaxLockdowns();
						int curLockdowns = house.GetAosCurLockdowns();

						AddHtmlLocalized( 10, 170, 300, 20, 1060683, LabelColor, false, false ); // Maximum Secure Storage
						AddLabel( 310, 170, LabelHue, maxSecures.ToString() );

						AddHtmlLocalized( 10, 190, 300, 20, 1060685, LabelColor, false, false ); // Used by Moving Crate
						AddLabel( 310, 190, LabelHue, fromMovingCrate.ToString() );

						AddHtmlLocalized( 10, 210, 300, 20, 1060686, LabelColor, false, false ); // Used by Lockdowns
						AddLabel( 310, 210, LabelHue, fromLockdowns.ToString() );

						AddHtmlLocalized( 10, 230, 300, 20, 1060688, LabelColor, false, false ); // Used by Secure Containers
						AddLabel( 310, 230, LabelHue, fromSecures.ToString() );

						AddHtmlLocalized( 10, 250, 300, 20, 1060689, LabelColor, false, false ); // Available Storage
						AddLabel( 310, 250, LabelHue, Math.Max( maxSecures - curSecures, 0 ).ToString() );

						AddHtmlLocalized( 10, 290, 300, 20, 1060690, LabelColor, false, false ); // Maximum Lockdowns
						AddLabel( 310, 290, LabelHue, maxLockdowns.ToString() );

						AddHtmlLocalized( 10, 310, 300, 20, 1060691, LabelColor, false, false ); // Available Lockdowns
						AddLabel( 310, 310, LabelHue, Math.Max( maxLockdowns - curLockdowns, 0 ).ToString() );

						int maxVendors = house.GetNewVendorSystemMaxVendors();
						int vendors = house.PlayerVendors.Count + house.VendorRentalContracts.Count;

						AddHtmlLocalized( 10, 350, 300, 20, 1062391, LabelColor, false, false ); // Vendor Count
						AddLabel( 310, 350, LabelHue, vendors.ToString() + " / " + maxVendors.ToString() );

						break;
					}
				case HouseGumpPage.Customize:
					{
						bool isCustomizable = isOwner && ( house is HouseFoundation );

						AddButtonLabeled( 10, 120, GetButtonID( 5, 0 ), 1060759, isOwner && !isCustomizable && ( house.ConvertEntry != null ) ); // Convert Into Customizable House
						AddButtonLabeled( 10, 160, GetButtonID( 5, 1 ), 1060765, isOwner && isCustomizable ); // Customize This House
						AddButtonLabeled( 10, 180, GetButtonID( 5, 2 ), 1060760, isOwner && house.MovingCrate != null ); // Relocate Moving Crate
						AddButtonLabeled( 10, 210, GetButtonID( 5, 3 ), 1060761, isOwner && house.Public ); // Change House Sign
						AddButtonLabeled( 10, 230, GetButtonID( 5, 4 ), 1060762, isOwner && isCustomizable ); // Change House Sign Hanger
						AddButtonLabeled( 10, 250, GetButtonID( 5, 5 ), 1060763, isOwner && isCustomizable && ( ( (HouseFoundation) house ).Signpost != null ) ); // Change Signpost
						AddButtonLabeled( 10, 280, GetButtonID( 5, 6 ), 1062004, isOwner && isCustomizable ); // Change Foundation Style
						AddButtonLabeled( 10, 310, GetButtonID( 5, 7 ), 1060764, isCoOwner ); // Rename House

						break;
					}
				case HouseGumpPage.Ownership:
					{
						bool isCustomizable = isOwner && ( house is HouseFoundation );

						AddButtonLabeled( 10, 130, GetButtonID( 6, 0 ), 1061794, isOwner && house.MovingCrate == null ); // Demolish House
						AddButtonLabeled( 10, 150, GetButtonID( 6, 1 ), 1061797, isOwner ); // Trade House
						AddButtonLabeled( 10, 190, GetButtonID( 6, 2 ), 1061798, false ); // Make Primary

						break;
					}
				case HouseGumpPage.ChangeHanger:
					{
						for ( int i = 0; i < m_HangerNumbers.Length; ++i )
						{
							int x = 50 + ( ( i % 3 ) * 100 );
							int y = 180 + ( ( i / 3 ) * 80 );

							AddButton( x, y, 4005, 4007, GetButtonID( 7, i ), GumpButtonType.Reply, 0 );
							AddItem( x + 20, y, m_HangerNumbers[i] );
						}

						break;
					}
				case HouseGumpPage.ChangeFoundation:
					{
						int[] foundationNumbers = m_FoundationNumbers;

						for ( int i = 0; i < foundationNumbers.Length; ++i )
						{
							int x = 15 + ( ( i % 5 ) * 80 );
							int y = 150 + ( ( i / 5 ) * 80 );

							AddButton( x, y, 4005, 4007, GetButtonID( 8, i ), GumpButtonType.Reply, 0 );
							AddItem( x + 25, y, foundationNumbers[i] );
						}

						break;
					}
				case HouseGumpPage.ChangeSign:
					{
						int index = 0;

						for ( int i = 0; i < 3; ++i )
						{
							AddPage( i + 1 );

							AddButton( 10, 360, 4005, 4007, 0, GumpButtonType.Page, ( ( i + 1 ) % 3 ) + 1 );

							for ( int j = 0; j < 18; ++j )
							{
								int x = 30 + ( ( j % 6 ) * 60 );
								int y = 130 + ( ( j / 6 ) * 60 );

								AddButton( x, y, 4005, 4007, GetButtonID( 9, index ), GumpButtonType.Reply, 0 );
								AddItem( x + 20, y, 2980 + ( index++ * 2 ) );
							}
						}

						break;
					}
				case HouseGumpPage.RemoveCoOwner:
					{
						AddHtmlLocalized( 10, 120, 400, 20, 1060730, LabelColor, false, false ); // <CENTER>CO-OWNER LIST</CENTER>
						AddList( house.CoOwners, 10, false, true, from );
						break;
					}
				case HouseGumpPage.ListCoOwner:
					{
						AddHtmlLocalized( 10, 120, 400, 20, 1060730, LabelColor, false, false ); // <CENTER>CO-OWNER LIST</CENTER>
						AddList( house.CoOwners, -1, false, true, from );
						break;
					}
				case HouseGumpPage.RemoveFriend:
					{
						AddHtmlLocalized( 10, 120, 400, 20, 1060731, LabelColor, false, false ); // <CENTER>FRIENDS LIST</CENTER>
						AddList( house.Friends, 11, false, true, from );
						break;
					}
				case HouseGumpPage.ListFriend:
					{
						AddHtmlLocalized( 10, 120, 400, 20, 1060731, LabelColor, false, false ); // <CENTER>FRIENDS LIST</CENTER>
						AddList( house.Friends, -1, false, true, from );
						break;
					}
				case HouseGumpPage.RemoveBan:
					{
						AddHtmlLocalized( 10, 120, 400, 20, 1060733, LabelColor, false, false ); // <CENTER>BAN LIST</CENTER>
						AddList( house.Bans, 12, true, true, from );
						break;
					}
				case HouseGumpPage.ListBan:
					{
						AddHtmlLocalized( 10, 120, 400, 20, 1060733, LabelColor, false, false ); // <CENTER>BAN LIST</CENTER>
						AddList( house.Bans, -1, true, true, from );
						break;
					}
				case HouseGumpPage.RemoveAccess:
					{
						AddHtmlLocalized( 10, 120, 400, 20, 1060732, LabelColor, false, false ); // <CENTER>ACCESS LIST</CENTER>
						AddList( house.Access, 13, false, true, from );
						break;
					}
				case HouseGumpPage.ListAccess:
					{
						AddHtmlLocalized( 10, 120, 400, 20, 1060732, LabelColor, false, false ); // <CENTER>ACCESS LIST</CENTER>
						AddList( house.Access, -1, false, true, from );
						break;
					}
				case HouseGumpPage.ChangePost:
					{
						int index = 0;

						for ( int i = 0; i < 2; ++i )
						{
							AddPage( i + 1 );

							AddButton( 10, 360, 4005, 4007, 0, GumpButtonType.Page, ( ( i + 1 ) % 2 ) + 1 );

							for ( int j = 0; j < 16 && index < m_PostNumbers.Length; ++j )
							{
								int x = 15 + ( ( j % 8 ) * 50 );
								int y = 130 + ( ( j / 8 ) * 110 );

								AddButton( x, y, 4005, 4007, GetButtonID( 14, index ), GumpButtonType.Reply, 0 );
								AddItem( x + 10, y, m_PostNumbers[index++] );
							}
						}

						break;
					}
			}
		}

		public static void PublicPrivateNotice_Callback( Mobile from, object state )
		{
			BaseHouse house = (BaseHouse) state;

			if ( !house.Deleted )
				from.SendGump( new HouseGump( HouseGumpPage.Security, from, house ) );
		}

		public static void CustomizeNotice_Callback( Mobile from, object state )
		{
			BaseHouse house = (BaseHouse) state;

			if ( !house.Deleted )
			{
				from.SendGump( new HouseGump( HouseGumpPage.Customize, from, house ) );
			}
		}

		public static void ClearCoOwners_Callback( Mobile from, bool okay, object state )
		{
			BaseHouse house = (BaseHouse) state;
			if ( house.Deleted )
			{
				return;
			}

			if ( okay && house.IsOwner( from ) )
			{
				if ( house.CoOwners != null )
				{
					house.CoOwners.Clear();
				}

				from.SendLocalizedMessage( 501333 ); // All co-owners have been removed from this house.
			}

			from.SendGump( new HouseGump( HouseGumpPage.Security, from, house ) );
		}

		public static void ClearFriends_Callback( Mobile from, bool okay, object state )
		{
			BaseHouse house = (BaseHouse) state;
			if ( house.Deleted )
			{
				return;
			}

			if ( okay && house.IsCoOwner( from ) )
			{
				if ( house.Friends != null )
				{
					house.Friends.Clear();
				}

				from.SendLocalizedMessage( 501332 ); // All friends have been removed from this house.
			}

			from.SendGump( new HouseGump( HouseGumpPage.Security, from, house ) );
		}

		public static void ClearBans_Callback( Mobile from, bool okay, object state )
		{
			BaseHouse house = (BaseHouse) state;
			if ( house.Deleted )
			{
				return;
			}

			if ( okay && house.IsFriend( from ) )
			{
				if ( house.Bans != null )
				{
					house.Bans.Clear();
				}

				from.SendLocalizedMessage( 1060754 ); // All bans for this house have been lifted.
			}

			from.SendGump( new HouseGump( HouseGumpPage.Security, from, house ) );
		}

		public static void ClearAccess_Callback( Mobile from, bool okay, object state )
		{
			BaseHouse house = (BaseHouse) state;
			if ( house.Deleted )
			{
				return;
			}

			if ( okay && house.IsFriend( from ) )
			{
				ArrayList list = new ArrayList( house.Access );

				if ( house.Access != null )
				{
					house.Access.Clear();
				}

				for ( int i = 0; i < list.Count; ++i )
				{
					Mobile m = (Mobile) list[i];

					if ( !house.HasAccess( m ) && house.IsInside( m ) )
					{
						m.Location = house.BanLocation;
						m.SendLocalizedMessage( 1060734 ); // Your access to this house has been revoked.
					}
				}

				from.SendLocalizedMessage( 1061843 ); // This house's Access List has been cleared.
			}

			from.SendGump( new HouseGump( HouseGumpPage.Security, from, house ) );
		}

		public static void ConvertHouse_Callback( Mobile from, bool okay, object state )
		{
			BaseHouse house = (BaseHouse) state;
			if ( house.Deleted )
			{
				return;
			}

			if ( okay && house.IsOwner( from ) && !house.HasRentedVendors )
			{
				HousePlacementEntry e = house.ConvertEntry;

				if ( e != null )
				{
					int cost = e.Cost - house.Price;

					if ( cost > 0 )
					{
						if ( Banker.Withdraw( from, cost ) )
						{
							from.SendLocalizedMessage( 1060398, cost.ToString() ); // ~1_AMOUNT~ gold has been withdrawn from your bank box.
						}
						else
						{
							from.SendLocalizedMessage( 1061624 ); // You do not have enough funds in your bank to cover the difference between your old house and your new one.
							return;
						}
					}
					else if ( cost < 0 )
					{
						if ( Banker.Deposit( from, -cost ) )
						{
							from.SendLocalizedMessage( 1060397, ( -cost ).ToString() ); // ~1_AMOUNT~ gold has been deposited into your bank box.
						}
						else
						{
							return;
						}
					}

					BaseHouse newHouse = e.ConstructHouse( from );

					if ( newHouse != null )
					{
						newHouse.Price = e.Cost;

						house.MoveAllToCrate();

						newHouse.Friends = new ArrayList( house.Friends );
						newHouse.CoOwners = new ArrayList( house.CoOwners );
						newHouse.Bans = new ArrayList( house.Bans );
						newHouse.Access = new ArrayList( house.Access );
						newHouse.BuiltOn = house.BuiltOn;
						newHouse.LastTraded = house.LastTraded;
						newHouse.Public = house.Public;

						newHouse.VendorInventories.AddRange( house.VendorInventories );
						house.VendorInventories.Clear();

						foreach ( VendorInventory inventory in newHouse.VendorInventories )
						{
							inventory.House = newHouse;
						}

						newHouse.InternalizedVendors.AddRange( house.InternalizedVendors );
						house.InternalizedVendors.Clear();

						foreach ( Mobile mobile in newHouse.InternalizedVendors )
						{
							if ( mobile is PlayerVendor )
							{
								( (PlayerVendor) mobile ).House = newHouse;
							}
							else if ( mobile is PlayerBarkeeper )
							{
								( (PlayerBarkeeper) mobile ).House = newHouse;
							}
						}

						if ( house.MovingCrate != null )
						{
							newHouse.MovingCrate = house.MovingCrate;
							newHouse.MovingCrate.House = newHouse;
							house.MovingCrate = null;
						}

						ArrayList items = house.GetItems();
						ArrayList mobiles = house.GetMobiles();

						newHouse.MoveToWorld( new Point3D( house.X + house.ConvertOffsetX, house.Y + house.ConvertOffsetY, house.Z + house.ConvertOffsetZ ), house.Map );
						house.Delete();

						foreach ( Item item in items )
						{
							item.Location = newHouse.BanLocation;
						}

						foreach ( Mobile mobile in mobiles )
						{
							mobile.Location = newHouse.BanLocation;
						}

						/* You have successfully replaced your original house with a new house.
						 * The value of the replaced house has been deposited into your bank box.
						 * All of the items in your original house have been relocated to a Moving Crate in the new house.
						 * Any deed-based house add-ons have been converted back into deeds.
						 * Vendors and barkeeps in the house, if any, have been stored in the Moving Crate as well.
						 * Use the <B>Get Vendor</B> context-sensitive menu option on your character to retrieve them.
						 * These containers can be used to re-create the vendor in a new location.
						 * Any barkeepers have been converted into deeds.
						 */
						from.SendGump( new NoticeGump( 1060637, 30720, 1060012, 32512, 420, 280, null, null ) );
						return;
					}
				}
			}

			from.SendGump( new HouseGump( HouseGumpPage.Security, from, house ) );
		}

		public override void OnResponse( NetState sender, RelayInfo info )
		{
			int val = info.ButtonID - 1;

			if ( val < 0 )
				return;

			ProcessResponse( sender.Mobile, m_House, val % 15, val / 15, m_List, m_Page );
		}

		public static void ProcessResponse( Mobile from, BaseHouse house, int type, int index, ArrayList vendorList, HouseGumpPage page )
		{
			if ( house.Deleted )
				return;

			bool isCombatRestricted = house.IsCombatRestricted( from );

			bool isOwner = house.IsOwner( from );
			bool isCoOwner = isOwner || house.IsCoOwner( from );
			bool isFriend = isCoOwner || house.IsFriend( from );

			if ( isCombatRestricted )
				isFriend = isCoOwner = isFriend = false;

			if ( !from.CheckAlive() )
				return;

			Item sign = house.Sign;

			if ( sign == null || from.Map != sign.Map || !from.InRange( sign.GetWorldLocation(), 18 ) )
				return;

			HouseFoundation foundation = house as HouseFoundation;
			bool isCustomizable = ( foundation != null );

			if ( page == HouseGumpPage.Vendors )
			{
				if ( index >= 0 && index < vendorList.Count )
				{
					PlayerVendor vendor = (PlayerVendor) vendorList[index];

					if ( !vendor.CanInteractWith( from, false ) )
						return;

					if ( from.Map != sign.Map || !from.InRange( sign, 5 ) )
						from.SendLocalizedMessage( 1062429 ); // You must be within five paces of the house sign to use this option.
					else if ( vendor.IsOwner( from ) )
						vendor.SendOwnerGump( from );
					else
						vendor.OpenBackpack( from );
				}

				return;
			}

			if ( !isFriend )
				return;

			switch ( type )
			{
				case 0:
					{
						switch ( index )
						{
							case 0: // Banish
								{
									if ( house.Public )
									{
										from.SendLocalizedMessage( 501325 ); // Target the individual to ban from this house.
										from.Target = new HouseBanTarget( true, house );
									}

									break;
								}
							case 1: // Lift Ban
								{
									if ( house.Public )
										from.SendGump( new HouseGump( HouseGumpPage.RemoveBan, from, house ) );

									break;
								}
							case 2: // Grant Access
								{
									if ( !house.Public )
									{
										from.SendLocalizedMessage( 1060711 ); // Target the person you would like to grant access to.
										from.Target = new HouseAccessTarget( house );
									}

									break;
								}
							case 3: // Revoke Access
								{
									if ( !house.Public )
										from.SendGump( new HouseGump( HouseGumpPage.RemoveAccess, from, house ) );

									break;
								}
						}

						break;
					}
				case 1:
					{
						HouseGumpPage gpage;

						switch ( index )
						{
							case 0:
								gpage = HouseGumpPage.Information;
								break;
							case 1:
								gpage = HouseGumpPage.Security;
								break;
							case 2:
								gpage = HouseGumpPage.Storage;
								break;
							case 3:
								gpage = HouseGumpPage.Customize;
								break;
							case 4:
								gpage = HouseGumpPage.Ownership;
								break;
							default:
								return;
						}

						from.SendGump( new HouseGump( gpage, from, house ) );
						break;
					}
				case 3:
					{
						switch ( index )
						{
							case 0: // View Co-Owner List
								{
									if ( isCoOwner )
										from.SendGump( new HouseGump( HouseGumpPage.ListCoOwner, from, house ) );

									break;
								}
							case 1: // Add a Co-Owner
								{
									if ( isOwner )
									{
										from.SendLocalizedMessage( 501328 ); // Target the person you wish to name a co-owner of your household.
										from.Target = new CoOwnerTarget( true, house );
									}

									break;
								}
							case 2: // Remove a Co-Owner
								{
									if ( isOwner )
										from.SendGump( new HouseGump( HouseGumpPage.RemoveCoOwner, from, house ) );

									break;
								}
							case 3: // Clear Co-Owner List
								{
									if ( isOwner )
										from.SendGump( new WarningGump( 1060635, 30720, 1060736, 32512, 420, 280, new WarningGumpCallback( ClearCoOwners_Callback ), house ) );

									break;
								}
							case 4: // View Friends List
								{
									from.SendGump( new HouseGump( HouseGumpPage.ListFriend, from, house ) );

									break;
								}
							case 5: // Add a Friend
								{
									if ( isCoOwner )
									{
										from.SendLocalizedMessage( 501317 ); // Target the person you wish to name a friend of your household.
										from.Target = new HouseFriendTarget( true, house );
									}

									break;
								}
							case 6: // Remove a Friend
								{
									if ( isCoOwner )
										from.SendGump( new HouseGump( HouseGumpPage.RemoveFriend, from, house ) );

									break;
								}
							case 7: // Clear Friend List
								{
									if ( isCoOwner )
										from.SendGump( new WarningGump( 1060635, 30720, 1018039, 32512, 420, 280, new WarningGumpCallback( ClearFriends_Callback ), house ) );

									break;
								}
							case 8: // View Ban List
								{
									from.SendGump( new HouseGump( HouseGumpPage.ListBan, from, house ) );

									break;
								}
							case 9: // Clear Ban List
								{
									from.SendGump( new WarningGump( 1060635, 30720, 1060753, 32512, 420, 280, new WarningGumpCallback( ClearBans_Callback ), house ) );

									break;
								}
							case 10: // View Access List
								{
									from.SendGump( new HouseGump( HouseGumpPage.ListAccess, from, house ) );

									break;
								}
							case 11: // Clear Access List
								{
									from.SendGump( new WarningGump( 1060635, 30720, 1061842, 32512, 420, 280, new WarningGumpCallback( ClearAccess_Callback ), house ) );

									break;
								}
							case 12: // Make Private
								{
									if ( isOwner )
									{
										if ( house.PlayerVendors.Count > 0 )
										{
											// You have vendors working out of this building. It cannot be declared private until there are no vendors in place.
											from.SendGump( new NoticeGump( 1060637, 30720, 501887, 32512, 320, 180, new NoticeGumpCallback( PublicPrivateNotice_Callback ), house ) );
											break;
										}

										if ( house.VendorRentalContracts.Count > 0 )
										{
											// You cannot currently take this action because you have vendor contracts locked down in your home.  You must remove them first.
											from.SendGump( new NoticeGump( 1060637, 30720, 1062351, 32512, 320, 180, new NoticeGumpCallback( PublicPrivateNotice_Callback ), house ) );
											break;
										}

										house.Public = false;

										house.ChangeLocks( from );

										// This house is now private.
										from.SendGump( new NoticeGump( 1060637, 30720, 501888, 32512, 320, 180, new NoticeGumpCallback( PublicPrivateNotice_Callback ), house ) );

										Region r = house.Region;
										List<Mobile> list = r.GetMobiles();

										for ( int i = 0; i < list.Count; ++i )
										{
											Mobile m = list[i];

											if ( !house.HasAccess( m ) && house.IsInside( m ) )
												m.Location = house.BanLocation;
										}
									}

									break;
								}
							case 13: // Make Public
								{
									if ( isOwner )
									{
										house.Public = true;

										house.RemoveKeys( from );
										house.RemoveLocks();

										// This house is now public. The owner may now place vendors and vendor rental contracts.
										from.SendGump( new NoticeGump( 1060637, 30720, 501886, 32512, 320, 180, new NoticeGumpCallback( PublicPrivateNotice_Callback ), house ) );

										Region r = house.Region;
										List<Mobile> list = r.GetMobiles();

										for ( int i = 0; i < list.Count; ++i )
										{
											Mobile m = list[i];

											if ( house.IsBanned( m ) && house.IsInside( m ) )
												m.Location = house.BanLocation;
										}
									}

									break;
								}
						}

						break;
					}
				case 5:
					{
						switch ( index )
						{
							case 0: // Convert Into Customizable House
								{
									if ( isOwner && !isCustomizable )
									{
										if ( house.HasRentedVendors )
										{
											// You cannot perform this action while you still have vendors rented out in this house.
											from.SendGump( new NoticeGump( 1060637, 30720, 1062395, 32512, 320, 180, new NoticeGumpCallback( CustomizeNotice_Callback ), house ) );
										}
										else
										{
											HousePlacementEntry e = house.ConvertEntry;

											if ( e != null )
											{
												/* You are about to turn your house into a customizable house.
												* You will be refunded the value of this house, and then be charged the cost of the equivalent customizable dirt lot.
												* All of your possessions in the house will be transported to a Moving Crate.
												* Deed-based house add-ons will be converted back into deeds.
												* Vendors and barkeeps will also be stored in the Moving Crate.
												* Your house will be leveled to its foundation, and you will be able to build new walls, windows, doors, and stairs.
												* Are you sure you wish to continue?
												*/
												from.SendGump( new WarningGump( 1060635, 30720, 1060013, 32512, 420, 280, new WarningGumpCallback( ConvertHouse_Callback ), house ) );
											}
										}
									}

									break;
								}
							case 1: // Customize This House
								{
									if ( isOwner && isCustomizable )
									{
										if ( house.HasRentedVendors )
										{
											// You cannot perform this action while you still have vendors rented out in this house.
											from.SendGump( new NoticeGump( 1060637, 30720, 1062395, 32512, 320, 180, new NoticeGumpCallback( CustomizeNotice_Callback ), house ) );
										}
										else
										{
											foundation.BeginCustomize( from );
										}
									}

									break;
								}
							case 2: // Relocate Moving Crate
								{
									MovingCrate crate = house.MovingCrate;

									if ( isOwner && crate != null )
									{
										if ( !house.IsInside( from ) )
										{
											from.SendLocalizedMessage( 502092 ); // You must be in your house to do this.
										}
										else
										{
											crate.MoveToWorld( from.Location, from.Map );
											crate.RestartTimer();
										}
									}

									break;
								}
							case 3: // Change House Sign
								{
									if ( isOwner && house.Public )
										from.SendGump( new HouseGump( HouseGumpPage.ChangeSign, from, house ) );

									break;
								}
							case 4: // Change House Sign Hanger
								{
									if ( isOwner && isCustomizable )
										from.SendGump( new HouseGump( HouseGumpPage.ChangeHanger, from, house ) );

									break;
								}
							case 5: // Change Signpost
								{
									if ( isOwner && isCustomizable && foundation.Signpost != null )
										from.SendGump( new HouseGump( HouseGumpPage.ChangePost, from, house ) );

									break;
								}
							case 6: // Change Foundation Style
								{
									if ( isOwner && isCustomizable )
										from.SendGump( new HouseGump( HouseGumpPage.ChangeFoundation, from, house ) );

									break;
								}
							case 7: // Rename House
								{
									if ( isCoOwner )
										from.Prompt = new RenamePrompt( house );

									break;
								}
						}

						break;
					}
				case 6:
					{
						switch ( index )
						{
							case 0: // Demolish
								{
									if ( isOwner && house.MovingCrate == null )
									{
										if ( house.FindGuildstone() != null )
											from.SendLocalizedMessage( 501389 ); // You cannot redeed a house with a guildstone inside.
										else
											from.SendGump( new HouseDemolishGump( from, house ) );
									}

									break;
								}
							case 1: // Trade House
								{
									if ( isOwner )
									{
										if ( house.HasPersonalVendors )
										{
											from.SendLocalizedMessage( 1062467 ); // You cannot trade this house while you still have personal vendors inside.
										}
										else if ( house.DecayLevel == DecayLevel.DemolitionPending )
										{
											from.SendLocalizedMessage( 1005321 ); // This house has been marked for demolition, and it cannot be transferred.
										}
										else
										{
											from.SendLocalizedMessage( 501309 ); // Target the person to whom you wish to give this house.
											from.Target = new HouseOwnerTarget( house );
										}
									}

									break;
								}
							case 2: // Make Primary
								{
									break;
								}
						}

						break;
					}
				case 7:
					{
						if ( isOwner && isCustomizable && index >= 0 && index < m_HangerNumbers.Length )
						{
							Item hanger = foundation.SignHanger;

							if ( hanger != null )
								hanger.ItemID = m_HangerNumbers[index];

							from.SendGump( new HouseGump( HouseGumpPage.Customize, from, house ) );
						}

						break;
					}
				case 8:
					{
						if ( isOwner && isCustomizable )
						{
							FoundationType newType;

							switch ( index )
							{
								case 0:
									newType = FoundationType.DarkWood;
									break;
								case 1:
									newType = FoundationType.LightWood;
									break;
								case 2:
									newType = FoundationType.Dungeon;
									break;
								case 3:
									newType = FoundationType.Brick;
									break;
								case 4:
									newType = FoundationType.Stone;
									break;
								case 5:
									newType = FoundationType.ElvenStone;
									break;
								case 6:
									newType = FoundationType.ElvenWood;
									break;
								case 7:
									newType = FoundationType.ElvenSimple;
									break;
								case 8:
									newType = FoundationType.ElvenPlain;
									break;
								case 9:
									newType = FoundationType.Crystal;
									break;
								case 10:
									newType = FoundationType.Shadow;
									break;
								case 11:
									newType = FoundationType.GargishGreenMarble;
									break;
								case 12:
									newType = FoundationType.GargishTwoToneStone;
									break;
								default:
									return;
							}

							foundation.Type = newType;

							DesignState state = foundation.BackupState;
							HouseFoundation.ApplyFoundation( newType, state.Components );
							state.OnRevised();

							state = foundation.DesignState;
							HouseFoundation.ApplyFoundation( newType, state.Components );
							state.OnRevised();

							state = foundation.CurrentState;
							HouseFoundation.ApplyFoundation( newType, state.Components );
							state.OnRevised();

							foundation.Delta( ItemDelta.Update );

							from.SendGump( new HouseGump( HouseGumpPage.Customize, from, house ) );
						}

						break;
					}
				case 9:
					{
						if ( isOwner && house.Public && index >= 0 && index < 54 )
						{
							house.ChangeSignType( 2980 + ( index * 2 ) );
							from.SendGump( new HouseGump( HouseGumpPage.Customize, from, house ) );
						}

						break;
					}
				case 10:
					{
						if ( isOwner && vendorList != null && index >= 0 && index < vendorList.Count )
						{
							house.RemoveCoOwner( from, (Mobile) vendorList[index] );

							if ( house.CoOwners.Count > 0 )
								from.SendGump( new HouseGump( HouseGumpPage.RemoveCoOwner, from, house ) );
							else
								from.SendGump( new HouseGump( HouseGumpPage.Security, from, house ) );
						}

						break;
					}
				case 11:
					{
						if ( isCoOwner && vendorList != null && index >= 0 && index < vendorList.Count )
						{
							house.RemoveFriend( from, (Mobile) vendorList[index] );

							if ( house.Friends.Count > 0 )
								from.SendGump( new HouseGump( HouseGumpPage.RemoveFriend, from, house ) );
							else
								from.SendGump( new HouseGump( HouseGumpPage.Security, from, house ) );
						}

						break;
					}
				case 12:
					{
						if ( vendorList != null && index >= 0 && index < vendorList.Count )
						{
							house.RemoveBan( from, (Mobile) vendorList[index] );

							if ( house.Bans.Count > 0 )
								from.SendGump( new HouseGump( HouseGumpPage.RemoveBan, from, house ) );
							else
								from.SendGump( new HouseGump( HouseGumpPage.Security, from, house ) );
						}

						break;
					}
				case 13:
					{
						if ( vendorList != null && index >= 0 && index < vendorList.Count )
						{
							house.RemoveAccess( from, (Mobile) vendorList[index] );

							if ( house.Access.Count > 0 )
								from.SendGump( new HouseGump( HouseGumpPage.RemoveAccess, from, house ) );
							else
								from.SendGump( new HouseGump( HouseGumpPage.Security, from, house ) );
						}

						break;
					}
				case 14:
					{
						if ( isOwner && isCustomizable && index >= 0 && index < m_PostNumbers.Length )
						{
							foundation.SignpostGraphic = m_PostNumbers[index];
							foundation.CheckSignpost();

							from.SendGump( new HouseGump( HouseGumpPage.Customize, from, house ) );
						}

						break;
					}
			}
		}

		private ArrayList Wrap( string value )
		{
			if ( value == null || ( value = value.Trim() ).Length <= 0 )
				return null;

			string[] values = value.Split( ' ' );
			ArrayList list = new ArrayList();
			string current = "";

			for ( int i = 0; i < values.Length; ++i )
			{
				string val = values[i];

				string v = current.Length == 0 ? val : current + ' ' + val;

				if ( v.Length < 10 )
				{
					current = v;
				}
				else if ( v.Length == 10 )
				{
					list.Add( v );

					if ( list.Count == 6 )
						return list;

					current = "";
				}
				else if ( val.Length <= 10 )
				{
					list.Add( current );

					if ( list.Count == 6 )
						return list;

					current = val;
				}
				else
				{
					while ( v.Length >= 10 )
					{
						list.Add( v.Substring( 0, 10 ) );

						if ( list.Count == 6 )
							return list;

						v = v.Substring( 10 );
					}

					current = v;
				}
			}

			if ( current.Length > 0 )
				list.Add( current );

			return list;
		}
	}
}

namespace Server.Engines.Housing.Prompts
{
	public class RenamePrompt : Prompt
	{
		// What dost thou wish the sign to say?
		public override int MessageCliloc { get { return 501302; } }

		private IHouse m_House;

		public RenamePrompt( IHouse house )
			: base( house )
		{
			m_House = house;
		}

		public override void OnResponse( Mobile from, string text )
		{
			if ( m_House.IsFriend( from ) )
			{
				if ( m_House.Sign != null )
					m_House.Sign.Name = text;

				from.SendMessage( "Sign changed." );
			}
		}
	}
}