using System;
using System.Collections;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Targeting;
using Server.Targets;

namespace Server.Scripts.Commands
{
	public class InterfaceCommand : BaseCommand
	{
		public InterfaceCommand()
		{
			AccessLevel = AccessLevel.GameMaster;
			Supports = CommandSupport.Complex;
			Commands = new string[] { "Interface" };
			ObjectTypes = ObjectTypes.Both;
			Usage = "Interface";
			Description = "Opens an interface to interact with matched objects. Generally used with condition arguments.";
			ListOptimized = true;
		}

		public override void ExecuteList( CommandEventArgs e, ArrayList list )
		{
			if ( list.Count > 0 )
			{
				e.Mobile.SendGump( new InterfaceGump( e.Mobile, list, 0 ) );
			}
			else
			{
				AddResponse( "No matching objects found." );
			}
		}
	}

	public class InterfaceGump : BaseGridGump
	{
		private Mobile m_From;
		private ArrayList m_List;
		private int m_Page;

		private const int EntriesPerPage = 15;

		public InterfaceGump( Mobile from, ArrayList list, int page )
			: base( 30, 30 )
		{
			m_From = from;
			m_List = list;
			m_Page = page;

			Render();
		}

		public void Render()
		{
			AddNewPage();

			if ( m_Page > 0 )
			{
				AddEntryButton( 20, ArrowLeftID1, ArrowLeftID2, 1, ArrowLeftWidth, ArrowLeftHeight );
			}
			else
			{
				AddEntryHeader( 20 );
			}

			AddEntryHtml( 160, Center( String.Format( "Page {0} of {1}", m_Page + 1, ( m_List.Count + EntriesPerPage - 1 ) / EntriesPerPage ) ) );

			if ( ( m_Page + 1 ) * EntriesPerPage < m_List.Count )
			{
				AddEntryButton( 20, ArrowRightID1, ArrowRightID2, 2, ArrowRightWidth, ArrowRightHeight );
			}
			else
			{
				AddEntryHeader( 20 );
			}

			for ( int i = m_Page * EntriesPerPage, line = 0; line < EntriesPerPage && i < m_List.Count; ++i, ++line )
			{
				AddNewLine();

				object obj = m_List[i];
				bool isDeleted = false;

				if ( obj is Item )
				{
					Item item = (Item) obj;

					if ( !( isDeleted = item.Deleted ) )
					{
						AddEntryHtml( 20 + OffsetSize + 160, item.GetType().Name );
					}
				}
				else if ( obj is Mobile )
				{
					Mobile mob = (Mobile) obj;

					if ( !( isDeleted = mob.Deleted ) )
					{
						AddEntryHtml( 20 + OffsetSize + 160, mob.Name );
					}
				}

				if ( isDeleted )
				{
					AddEntryHtml( 20 + OffsetSize + 160, "(deleted)" );
					AddEntryHeader( 20 );
				}
				else
				{
					AddEntryButton( 20, ArrowRightID1, ArrowRightID2, 3 + i, ArrowRightWidth, ArrowRightHeight );
				}
			}

			FinishPage();
		}

		public override void OnResponse( GameClient sender, RelayInfo info )
		{
			switch ( info.ButtonID )
			{
				case 1:
					{
						if ( m_Page > 0 )
						{
							m_From.SendGump( new InterfaceGump( m_From, m_List, m_Page - 1 ) );
						}

						break;
					}
				case 2:
					{
						if ( ( m_Page + 1 ) * EntriesPerPage < m_List.Count )
						{
							m_From.SendGump( new InterfaceGump( m_From, m_List, m_Page + 1 ) );
						}

						break;
					}
				default:
					{
						int v = info.ButtonID - 3;

						if ( v >= 0 && v < m_List.Count )
						{
							object obj = m_List[v];

							if ( !BaseCommand.IsAccessible( m_From, obj ) )
							{
								m_From.SendMessage( "That is not accessible." );
								m_From.SendGump( new InterfaceGump( m_From, m_List, m_Page ) );
								break;
							}

							if ( obj is Item && !( (Item) obj ).Deleted )
							{
								m_From.SendGump( new InterfaceItemGump( m_From, m_List, m_Page, (Item) obj ) );
							}
							else if ( obj is Mobile && !( (Mobile) obj ).Deleted )
							{
								m_From.SendGump( new InterfaceMobileGump( m_From, m_List, m_Page, (Mobile) obj ) );
							}
							else
							{
								m_From.SendGump( new InterfaceGump( m_From, m_List, m_Page ) );
							}
						}

						break;
					}
			}
		}
	}

	public class InterfaceItemGump : BaseGridGump
	{
		private Mobile m_From;
		private ArrayList m_List;
		private int m_Page;
		private Item m_Item;

		public InterfaceItemGump( Mobile from, ArrayList list, int page, Item item )
			: base( 30, 30 )
		{
			m_From = from;
			m_List = list;
			m_Page = page;
			m_Item = item;

			Render();
		}

		public void Render()
		{
			AddNewPage();

			AddEntryButton( 20, ArrowLeftID1, ArrowLeftID2, 1, ArrowLeftWidth, ArrowLeftHeight );
			AddEntryHtml( 160, m_Item.GetType().Name );
			AddEntryHeader( 20 );

			AddNewLine();
			AddEntryHtml( 20 + OffsetSize + 160, "Properties" );
			AddEntryButton( 20, ArrowRightID1, ArrowRightID2, 2, ArrowRightWidth, ArrowRightHeight );

			AddNewLine();
			AddEntryHtml( 20 + OffsetSize + 160, "Delete" );
			AddEntryButton( 20, ArrowRightID1, ArrowRightID2, 3, ArrowRightWidth, ArrowRightHeight );

			AddNewLine();
			AddEntryHtml( 20 + OffsetSize + 160, "Go there" );
			AddEntryButton( 20, ArrowRightID1, ArrowRightID2, 4, ArrowRightWidth, ArrowRightHeight );

			AddNewLine();
			AddEntryHtml( 20 + OffsetSize + 160, "Move to target" );
			AddEntryButton( 20, ArrowRightID1, ArrowRightID2, 5, ArrowRightWidth, ArrowRightHeight );

			AddNewLine();
			AddEntryHtml( 20 + OffsetSize + 160, "Bring to pack" );
			AddEntryButton( 20, ArrowRightID1, ArrowRightID2, 6, ArrowRightWidth, ArrowRightHeight );

			FinishPage();
		}

		private void InvokeCommand( string ip )
		{
			Server.CommandSystem.Handle( m_From, String.Format( "{0}{1}", Server.CommandSystem.CommandPrefix, ip ) );
		}

		public override void OnResponse( GameClient sender, RelayInfo info )
		{
			if ( m_Item.Deleted )
			{
				m_From.SendGump( new InterfaceGump( m_From, m_List, m_Page ) );
				return;
			}
			else if ( !BaseCommand.IsAccessible( m_From, m_Item ) )
			{
				m_From.SendMessage( "That is no longer accessible." );
				m_From.SendGump( new InterfaceGump( m_From, m_List, m_Page ) );
				return;
			}

			switch ( info.ButtonID )
			{
				case 0:
				case 1:
					{
						m_From.SendGump( new InterfaceGump( m_From, m_List, m_Page ) );
						break;
					}
				case 2: // Properties
					{
						m_From.SendGump( new InterfaceItemGump( m_From, m_List, m_Page, m_Item ) );
						m_From.SendGump( new PropertiesGump( m_From, m_Item ) );
						break;
					}
				case 3: // Delete
					{
						CommandLogging.WriteLine( m_From, "{0} {1} deleting {2}", m_From.AccessLevel, CommandLogging.Format( m_From ), CommandLogging.Format( m_Item ) );
						m_Item.Delete();
						m_From.SendGump( new InterfaceGump( m_From, m_List, m_Page ) );
						break;
					}
				case 4: // Go there
					{
						m_From.SendGump( new InterfaceItemGump( m_From, m_List, m_Page, m_Item ) );
						InvokeCommand( String.Format( "Go {0}", m_Item.Serial.Value ) );
						break;
					}
				case 5: // Move to target
					{
						m_From.SendGump( new InterfaceItemGump( m_From, m_List, m_Page, m_Item ) );
						m_From.Target = new MoveTarget( m_Item );
						break;
					}
				case 6: // Bring to pack
					{
						Mobile owner = m_Item.RootParent as Mobile;

						if ( owner != null && ( owner.Map != null && owner.Map != Map.Internal ) && !m_From.CanSee( owner ) )
						{
							m_From.SendMessage( "You can not get what you can not see." );
						}
						else if ( owner != null && ( owner.Map == null || owner.Map == Map.Internal ) && owner.Hidden && owner.AccessLevel >= m_From.AccessLevel )
						{
							m_From.SendMessage( "You can not get what you can not see." );
						}
						else
						{
							m_From.SendGump( new InterfaceItemGump( m_From, m_List, m_Page, m_Item ) );
							m_From.AddToBackpack( m_Item );
						}

						break;
					}
			}
		}
	}

	public class InterfaceMobileGump : BaseGridGump
	{
		private Mobile m_From;
		private ArrayList m_List;
		private int m_Page;
		private Mobile m_Mobile;

		public InterfaceMobileGump( Mobile from, ArrayList list, int page, Mobile mob )
			: base( 30, 30 )
		{
			m_From = from;
			m_List = list;
			m_Page = page;
			m_Mobile = mob;

			Render();
		}

		public void Render()
		{
			AddNewPage();

			AddEntryButton( 20, ArrowLeftID1, ArrowLeftID2, 1, ArrowLeftWidth, ArrowLeftHeight );
			AddEntryHtml( 160, m_Mobile.Name );
			AddEntryHeader( 20 );

			AddNewLine();
			AddEntryHtml( 20 + OffsetSize + 160, "Properties" );
			AddEntryButton( 20, ArrowRightID1, ArrowRightID2, 2, ArrowRightWidth, ArrowRightHeight );

			if ( !m_Mobile.IsPlayer )
			{
				AddNewLine();
				AddEntryHtml( 20 + OffsetSize + 160, "Delete" );
				AddEntryButton( 20, ArrowRightID1, ArrowRightID2, 3, ArrowRightWidth, ArrowRightHeight );
			}

			if ( m_Mobile != m_From )
			{
				AddNewLine();
				AddEntryHtml( 20 + OffsetSize + 160, "Go to there" );
				AddEntryButton( 20, ArrowRightID1, ArrowRightID2, 4, ArrowRightWidth, ArrowRightHeight );

				AddNewLine();
				AddEntryHtml( 20 + OffsetSize + 160, "Bring them here" );
				AddEntryButton( 20, ArrowRightID1, ArrowRightID2, 5, ArrowRightWidth, ArrowRightHeight );
			}

			AddNewLine();
			AddEntryHtml( 20 + OffsetSize + 160, "Move to target" );
			AddEntryButton( 20, ArrowRightID1, ArrowRightID2, 6, ArrowRightWidth, ArrowRightHeight );

			if ( m_From == m_Mobile || m_From.AccessLevel > m_Mobile.AccessLevel )
			{
				AddNewLine();
				if ( m_Mobile.Alive )
				{
					AddEntryHtml( 20 + OffsetSize + 160, "Kill" );
					AddEntryButton( 20, ArrowRightID1, ArrowRightID2, 7, ArrowRightWidth, ArrowRightHeight );
				}
				else
				{
					AddEntryHtml( 20 + OffsetSize + 160, "Resurrect" );
					AddEntryButton( 20, ArrowRightID1, ArrowRightID2, 8, ArrowRightWidth, ArrowRightHeight );
				}
			}

			if ( m_Mobile.Client != null )
			{
				AddNewLine();
				AddEntryHtml( 20 + OffsetSize + 160, "Client" );
				AddEntryButton( 20, ArrowRightID1, ArrowRightID2, 9, ArrowRightWidth, ArrowRightHeight );
			}

			FinishPage();
		}

		private void InvokeCommand( string ip )
		{
			Server.CommandSystem.Handle( m_From, String.Format( "{0}{1}", Server.CommandSystem.CommandPrefix, ip ) );
		}

		public override void OnResponse( GameClient sender, RelayInfo info )
		{
			if ( m_Mobile.Deleted )
			{
				m_From.SendGump( new InterfaceGump( m_From, m_List, m_Page ) );
				return;
			}
			else if ( !BaseCommand.IsAccessible( m_From, m_Mobile ) )
			{
				m_From.SendMessage( "That is no longer accessible." );
				m_From.SendGump( new InterfaceGump( m_From, m_List, m_Page ) );
				return;
			}

			switch ( info.ButtonID )
			{
				case 0:
				case 1:
					{
						m_From.SendGump( new InterfaceGump( m_From, m_List, m_Page ) );
						break;
					}
				case 2: // Properties
					{
						m_From.SendGump( new InterfaceMobileGump( m_From, m_List, m_Page, m_Mobile ) );
						m_From.SendGump( new PropertiesGump( m_From, m_Mobile ) );
						break;
					}
				case 3: // Delete
					{
						if ( !m_Mobile.IsPlayer )
						{
							CommandLogging.WriteLine( m_From, "{0} {1} deleting {2}", m_From.AccessLevel, CommandLogging.Format( m_From ), CommandLogging.Format( m_Mobile ) );
							m_Mobile.Delete();
							m_From.SendGump( new InterfaceGump( m_From, m_List, m_Page ) );
						}

						break;
					}
				case 4: // Go there
					{
						m_From.SendGump( new InterfaceMobileGump( m_From, m_List, m_Page, m_Mobile ) );
						InvokeCommand( String.Format( "Go {0}", m_Mobile.Serial.Value ) );
						break;
					}
				case 5: // Bring them here
					{
						if ( m_From.Map == null || m_From.Map == Map.Internal )
						{
							m_From.SendMessage( "You cannot bring that person here." );
						}
						else
						{
							m_From.SendGump( new InterfaceMobileGump( m_From, m_List, m_Page, m_Mobile ) );
							m_Mobile.MoveToWorld( m_From.Location, m_From.Map );
						}

						break;
					}
				case 6: // Move to target
					{
						m_From.SendGump( new InterfaceMobileGump( m_From, m_List, m_Page, m_Mobile ) );
						m_From.Target = new MoveTarget( m_Mobile );
						break;
					}
				case 7: // Kill
					{
						if ( m_From == m_Mobile || m_From.AccessLevel > m_Mobile.AccessLevel )
						{
							m_Mobile.Kill();
						}

						m_From.SendGump( new InterfaceMobileGump( m_From, m_List, m_Page, m_Mobile ) );

						break;
					}
				case 8: // Res
					{
						if ( m_From == m_Mobile || m_From.AccessLevel > m_Mobile.AccessLevel )
						{
							m_Mobile.PlaySound( 0x214 );
							m_Mobile.FixedEffect( 0x376A, 10, 16 );

							m_Mobile.Resurrect();
						}

						m_From.SendGump( new InterfaceMobileGump( m_From, m_List, m_Page, m_Mobile ) );

						break;
					}
				case 9: // Client
					{
						m_From.SendGump( new InterfaceMobileGump( m_From, m_List, m_Page, m_Mobile ) );

						if ( m_Mobile.Client != null )
						{
							m_From.SendGump( new ClientGump( m_From, m_Mobile.Client ) );
						}

						break;
					}
			}
		}
	}
}