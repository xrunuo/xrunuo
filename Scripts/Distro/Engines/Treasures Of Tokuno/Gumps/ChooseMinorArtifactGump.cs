using System;
using System.Collections;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Mobiles;
using Server.Items;

namespace Server.Gumps
{
	public class ChooseMinorArtifactGump : Gump
	{
		public override int TypeID { get { return 0x2336; } }

		private Mobile from;
		private IharaSoko minister;
		private ArrayList list;

		public void GetSizes( int i, int number, out int offset, out int width, out int height )
		{
			offset = ( i % 2 == 0 ) ? i / 2 : ( i / 2 ) + 1;

			offset--;

			switch ( number )
			{
				case 1070912:
					width = 0;
					height = 14;
					break;
				case 1070913:
					width = 2;
					height = 18;
					break;
				case 1070914:
					width = 18;
					height = 19;
					break;
				case 1070915:
					width = 5;
					height = 10;
					break;
				case 1070916:
					width = 17;
					height = 10;
					break;
				case 1070917:
					width = 19;
					height = 11;
					break;
				case 1070918:
					width = 2;
					height = 10;
					break;
				case 1070919:
					width = 18;
					height = 19;
					break;
				case 1070920:
					width = 18;
					height = 19;
					break;
				case 1070921:
					width = 19;
					height = 15;
					break;
				case 1070922:
					width = 17;
					height = 16;
					break;
				case 1070923:
					width = 15;
					height = 17;
					break;
				case 1070924:
					width = 16;
					height = 17;
					break;
				case 1070926:
					width = 17;
					height = 12;
					break;
				case 1070927:
					width = 18;
					height = 15;
					break;
				case 1070933:
					width = 11;
					height = 19;
					break;
				case 1070934:
					width = 17;
					height = 19;
					break;
				case 1071014:
					width = 17;
					height = 17;
					break;
				case 1071015:
					width = -2;
					height = -13;
					break;
				case 1070937:
					width = 10;
					height = -7;
					break;
				default:
					width = 0;
					height = 0;
					break;
			}
		}

		public int GetLabel( Item item )
		{
			if ( item is AncientUrn )
			{
				return 1071014;
			}

			if ( item is HonorableSwords )
			{
				return 1071015;
			}

			return item.LabelNumber;
		}

		public ChooseMinorArtifactGump( Mobile m, IharaSoko min, ArrayList l )
			: base( 60, 36 )
		{
			from = m;

			minister = min;

			list = l;

			AddPage( 0 );

			AddBackground( 0, 0, 520, 404, 0x13BE );

			AddImageTiled( 10, 10, 500, 20, 0xA40 );
			AddImageTiled( 10, 40, 500, 324, 0xA40 );
			AddImageTiled( 10, 374, 500, 20, 0xA40 );

			AddAlphaRegion( 10, 10, 500, 384 );

			AddButton( 10, 374, 0xFB1, 0xFB2, 0, GumpButtonType.Reply, 0 );

			AddHtmlLocalized( 45, 376, 450, 20, 1060051, 0x7FFF, false, false ); // CANCEL

			AddHtmlLocalized( 14, 12, 500, 20, 1071012, 0x7FFF, false, false ); // Click a minor artifact to give it to Ihara Soko.

			int pages = (int) ( list.Count / 10 ) + 1;

			if ( list.Count % 10 == 0 )
			{
				pages--;
			}

			if ( pages > 4 ) // at OSI you cannot turn-in more than 40 artifacts per gump
			{
				pages = 4;
			}

			for ( int j = 1; j <= pages; j++ )
			{
				AddPage( j );

				if ( j > 1 )
				{
					AddButton( 300, 374, 4014, 4016, 0, GumpButtonType.Page, j - 1 );

					AddHtmlLocalized( 340, 376, 60, 20, 1011393, 0x7FFF, false, false ); // Back
				}

				int start = ( j - 1 ) * 10;

				for ( int i = start; i < list.Count; i++ )
				{
					Item item = list[i] as Item;

					if ( item != null && i <= ( 10 * j - 1 ) )
					{
						int offset, width, height;

						int k = i - ( j - 1 ) * 10;

						GetSizes( k + 1, GetLabel( item ), out offset, out width, out height );

						AddButtonTileArt( ( ( k + 1 ) % 2 != 0 ) ? 14 : 264, 44 + offset * 64, 0x918, 0x919, GumpButtonType.Reply, 0, 100 + i, item.ItemID, item.Hue, width, height );

						AddHtmlLocalized( ( ( k + 1 ) % 2 != 0 ) ? 98 : 348, 44 + offset * 64, 250, 60, GetLabel( item ), 0x7FFF, false, false );
					}
				}

				if ( j < pages )
				{
					AddButton( 400, 374, 4005, 4007, 0, GumpButtonType.Page, j + 1 );

					AddHtmlLocalized( 440, 376, 60, 20, 1043353, 0x7FFF, false, false ); // Next
				}
			}
		}

		public override void OnResponse( GameClient sender, RelayInfo info )
		{
			PlayerMobile pm = from as PlayerMobile;

			if ( info.ButtonID > 0 )
			{
				int i = info.ButtonID - 100;

				Item item = list[i] as Item;

				from.CloseGump( typeof( ChooseMinorArtifactGump ) );

				if ( item != null && item.IsChildOf( from.Backpack ) )
				{
					item.Delete();

					if ( pm.ToTItemsTurnedIn < 10 )
					{
						pm.ToTItemsTurnedIn++;

						if ( pm.ToTItemsTurnedIn != 10 )
						{
							string args = String.Format( "{0}\t{1}\t ", pm.ToTItemsTurnedIn.ToString(), ( 10 - pm.ToTItemsTurnedIn ).ToString() );

							minister.Say( 1070981, args ); // You have turned in ~1_COUNT~ minor artifacts. Turn in ~2_NUM~ to receive a reward.
						}
						else
						{
							minister.Say( 1070980 ); // Congratulations! You have turned in enough minor treasures to earn a greater reward.

							from.CloseGump( typeof( ChooseRewardGump ) );
							from.CloseGump( typeof( ChoosePigmentGump ) );

							from.SendGump( new ChooseRewardGump( from, minister ) );
						}
					}
				}

				list = minister.FindMinorArtifacts( from );

				if ( list.Count != 0 && pm.ToTItemsTurnedIn != 10 )
				{
					from.SendGump( new ChooseMinorArtifactGump( from, minister, list ) );
				}
				else
				{
					string args = String.Format( "{0}\t{1}\t ", pm.ToTItemsTurnedIn.ToString(), ( 10 - pm.ToTItemsTurnedIn ).ToString() );

					minister.Say( 1070981, args ); // You have turned in ~1_COUNT~ minor artifacts. Turn in ~2_NUM~ to receive a reward.

					minister.Say( 1071013 ); // Bring me 10 of the lost treasures of Tokuno and I will reward you with a valuable item.
				}
			}
			else
			{
				string args = String.Format( "{0}\t{1}\t ", pm.ToTItemsTurnedIn.ToString(), ( 10 - pm.ToTItemsTurnedIn ).ToString() );

				minister.Say( 1070981, args ); // You have turned in ~1_COUNT~ minor artifacts. Turn in ~2_NUM~ to receive a reward.
			}
		}
	}
}