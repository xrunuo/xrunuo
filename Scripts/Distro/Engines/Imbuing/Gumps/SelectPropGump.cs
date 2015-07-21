using System;
using System.Collections.Generic;
using Server;
using Server.Gumps;
using Server.Network;

namespace Server.Engines.Imbuing
{
	public class SelectPropGump : Gump
	{
		private Item m_Item;
		private List<int> m_Categories;
		private List<BaseAttrInfo> m_AllProperties, m_Properties;

		public SelectPropGump( Item item )
			: this( item, Imbuing.GetValidProperties( item ) )
		{
		}

		private SelectPropGump( Item item, List<BaseAttrInfo> props )
			: this( item, props, Categories( props ), 0 )
		{
		}

		private SelectPropGump( Item item, List<BaseAttrInfo> props, List<int> categories, int category )
			: base( 50, 50 )
		{
			m_Item = item;
			m_Categories = categories;
			m_AllProperties = props;
			m_Properties = Filter( props, categories[category] );

			AddPage( 0 );

			AddBackground( 0, 0, 520, 520, 0x13BE );
			AddImageTiled( 10, 10, 500, 20, 0xA40 );
			AddImageTiled( 10, 40, 220, 440, 0xA40 );
			AddImageTiled( 240, 40, 270, 440, 0xA40 );
			AddImageTiled( 10, 490, 500, 20, 0xA40 );
			AddAlphaRegion( 10, 10, 500, 520 );

			AddHtmlLocalized( 10, 12, 500, 20, 1079588, 0x7FFF, false, false ); // <CENTER>IMBUING MENU</CENTER>

			/*********** Categories *************/
			AddHtmlLocalized( 10, 60, 220, 20, 1044010, 0x7FFF, false, false ); // <CENTER>CATEGORIES</CENTER>

			for ( int i = 0, yOffset = 0; i < m_Categories.Count; i++, yOffset += 25 )
			{
				AddButton( 15, 90 + yOffset, 0xFA5, 0xFA6, 101 + i, GumpButtonType.Reply, 0 );
				AddHtmlLocalized( 50, 92 + yOffset, 450, 20, m_Categories[i], 0x7FFF, false, false );
			}
			/************************************/

			/*********** Selections *************/
			AddHtmlLocalized( 240, 60, 270, 20, 1044011, 0x7FFF, false, false ); // <CENTER>SELECTIONS</CENTER>

			for ( int i = 0, yOffset = 0; i < m_Properties.Count; i++, yOffset += 20 )
			{
				BaseAttrInfo prop = m_Properties[i];

				AddButton( 250, 90 + yOffset, 0xFA5, 0xFA6, 201 + i, GumpButtonType.Reply, 0 );
				AddHtmlLocalized( 295, 92 + yOffset, 200, 20, prop.Name, 0x7FFF, false, false );
			}
			/************************************/

			AddButton( 10, 490, 0xFB1, 0xFB2, 0, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 45, 492, 450, 20, 1060051, 0x7FFF, false, false ); // CANCEL
		}

		private static List<int> Categories( List<BaseAttrInfo> props )
		{
			List<int> categories = new List<int>();

			foreach ( BaseAttrInfo info in props )
			{
				if ( !categories.Contains( info.Category ) )
					categories.Add( info.Category );
			}

			return categories;
		}

		private static List<BaseAttrInfo> Filter( List<BaseAttrInfo> all, int category )
		{
			List<BaseAttrInfo> filtered = new List<BaseAttrInfo>();

			foreach ( BaseAttrInfo info in all )
			{
				if ( info.Category == category )
					filtered.Add( info );
			}

			return filtered;
		}

		public override void OnResponse( GameClient sender, RelayInfo info )
		{
			Mobile from = sender.Mobile;

			int mode = info.ButtonID / 100;

			switch ( mode )
			{
				case 1:
					{
						int categoryId = info.ButtonID - 101;

						if ( categoryId >= 0 && categoryId < m_Categories.Count )
							from.SendGump( new SelectPropGump( m_Item, m_AllProperties, m_Categories, categoryId ) );
						else
							from.EndAction( typeof( Imbuing ) );

						break;
					}
				case 2:
					{
						int propId = info.ButtonID - 201;

						if ( propId >= 0 && propId < m_Properties.Count )
						{
							Gump confirm = SelectProp( from, m_Item, m_Properties[propId] );

							if ( confirm != null )
								from.SendGump( confirm );
						}
						else
							from.EndAction( typeof( Imbuing ) );

						break;
					}
				default:
					{
						from.EndAction( typeof( Imbuing ) );

						break;
					}
			}
		}

		public static ConfirmationGump SelectProp( Mobile from, Item item, BaseAttrInfo propToImbue )
		{
			PropCollection props = new PropCollection( item );
			BaseAttrInfo propToReplace = Imbuing.GetReplaced( propToImbue, props );

			int totalProperties = props.Count;
			int totalIntensity = props.WeightedIntensity;

			if ( propToReplace == null )
				totalProperties++;
			else
				totalIntensity -= (int) ( Imbuing.ComputeIntensity( item, propToReplace ) * propToReplace.Weight );

			if ( totalProperties > 5 || totalIntensity > ( (IImbuable) item ).MaxIntensity )
			{
				from.SendLocalizedMessage( 1079772 ); // You cannot imbue this item with any more item properties.
				from.EndAction( typeof( Imbuing ) );
				return null;
			}
			else
				return new ConfirmationGump( from, item, props, propToImbue, propToReplace, 1, Imbuing.GetIntensitySteps( propToImbue ) );
		}
	}
}