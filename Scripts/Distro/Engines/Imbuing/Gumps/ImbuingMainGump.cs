using System;
using Server;
using Server.Gumps;
using Server.Network;
using Server.SkillHandlers;

namespace Server.Engines.Imbuing
{
	public class ImbuingMainGump : Gump
	{
		public override int TypeID { get { return 0xF3E93; } }

		public ImbuingMainGump()
			: base( 25, 50 )
		{
			AddPage( 0 );

			AddBackground( 0, 0, 520, 310, 0x13BE );
			AddImageTiled( 10, 10, 500, 20, 0xA40 );
			AddImageTiled( 10, 40, 500, 230, 0xA40 );
			AddImageTiled( 10, 280, 500, 20, 0xA40 );
			AddAlphaRegion( 10, 10, 500, 310 );

			// <CENTER>IMBUING MENU</CENTER>
			AddHtmlLocalized( 10, 12, 500, 20, 1079588, 0x7FFF, false, false );

			// Imbue Item - Adds or modifies an item property on an item
			AddHtmlLocalized( 50, 62, 500, 20, 1080432, 0x7FFF, false, false );
			AddButton( 15, 60, 0xFA5, 0xFA7, 1, GumpButtonType.Reply, 0 );

			// Reimbue Last - Repeats the last imbuing attempt
			AddHtmlLocalized( 50, 92, 500, 20, 1113622, 0x7FFF, false, false );
			AddButton( 15, 90, 0xFA5, 0xFA7, 4, GumpButtonType.Reply, 0 );

			// Imbue Last Item - Auto targets the last imbued item
			AddHtmlLocalized( 50, 122, 500, 20, 1113571, 0x7FFF, false, false );
			AddButton( 15, 120, 0xFA5, 0xFA7, 3, GumpButtonType.Reply, 0 );

			// Imbue Last Property - Imbues a new item with the last property
			AddHtmlLocalized( 50, 152, 500, 20, 1114274, 0x7FFF, false, false );
			AddButton( 15, 150, 0xFA5, 0xFA7, 5, GumpButtonType.Reply, 0 );

			// Unravel Item - Extracts magical ingredients from an item, destroying it
			AddHtmlLocalized( 50, 182, 500, 20, 1080431, 0x7FFF, false, false );
			AddButton( 15, 180, 0xFA5, 0xFA7, 2, GumpButtonType.Reply, 0 );

			// Unravel Container - Unravels all items in a container
			AddHtmlLocalized( 50, 212, 500, 20, 1114275, 0x7FFF, false, false );
			AddButton( 15, 210, 0xFA5, 0xFA7, 6, GumpButtonType.Reply, 0 );

			// CANCEL
			AddHtmlLocalized( 50, 282, 500, 20, 1060051, 0x7FFF, false, false );
			AddButton( 15, 280, 0xFB1, 0xFB2, 0, GumpButtonType.Reply, 0 );
		}

		public override void OnResponse( GameClient sender, RelayInfo info )
		{
			Mobile from = sender.Mobile;

			switch ( info.ButtonID )
			{
				default:
				case 0: // Cancel
					{
						from.EndAction( typeof( Imbuing ) );

						break;
					}
				case 1: // Imbue Item
					{
						from.SendLocalizedMessage( 1079589 ); // Target an item you wish to imbue.

						from.Target = new ImbueTarget();
						from.Target.BeginTimeout( from, TimeSpan.FromSeconds( 10.0 ) );

						break;
					}
				case 2: // Unravel Item
					{
						from.SendLocalizedMessage( 1080422 ); // Target an item you wish to magically unravel.

						from.Target = new UnravelTarget();
						from.Target.BeginTimeout( from, TimeSpan.FromSeconds( 10.0 ) );

						break;
					}
				case 3: // Imbue Last Item
					{
						Item item = ImbuingContext.GetLastItem( from );

						if ( item == null )
						{
							from.SendLocalizedMessage( 1113572 ); // You haven't imbued anything yet!
							from.EndAction( typeof( Imbuing ) );
						}
						else if ( Imbuing.Check( from, item ) )
						{
							from.SendGump( new SelectPropGump( item ) );
						}

						break;
					}
				case 4: // Reimbue Last
					{
						ImbuingContext context = ImbuingContext.GetContext( from );

						if ( context == null )
						{
							from.SendLocalizedMessage( 1113572 ); // You haven't imbued anything yet!
							from.EndAction( typeof( Imbuing ) );
						}
						else
						{
							ConfirmationGump confirm = SelectPropGump.SelectProp( from, context.Item, context.Property );

							if ( confirm != null )
							{
								confirm.ChangeIntensity( from, context.Intensity );
								confirm.OnResponse( sender, new RelayInfo( 302, new int[0], new TextRelay[0] ) );
							}
						}

						break;
					}
				case 5: // Imbue Last Property
					{
						BaseAttrInfo attribute = ImbuingContext.GetLastProperty( from );

						if ( attribute == null )
						{
							from.SendLocalizedMessage( 1113572 ); // You haven't imbued anything yet!
							from.EndAction( typeof( Imbuing ) );
						}
						else
						{
							from.SendLocalizedMessage( 1079589 ); // Target an item you wish to imbue.

							from.Target = new ImbueLastPropTarget( attribute );
							from.Target.BeginTimeout( from, TimeSpan.FromSeconds( 10.0 ) );
						}

						break;
					}
				case 6: // Unravel Container
					{
						from.SendLocalizedMessage( 1080422 ); // Target an item you wish to magically unravel.

						from.Target = new UnravelContainerTarget();
						from.Target.BeginTimeout( from, TimeSpan.FromSeconds( 10.0 ) );

						break;
					}
			}
		}
	}
}