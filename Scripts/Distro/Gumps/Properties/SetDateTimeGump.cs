using System;
using System.Reflection;
using System.Collections;
using Server;
using Server.Network;

namespace Server.Gumps
{
	public class SetDateTimeGump : Gump
	{
		private DateTime m_OldDateTime;
		private PropertyInfo m_Property;
		private Mobile m_Mobile;
		private object m_Object;
		private Stack m_Stack;
		private int m_Page;
		private ArrayList m_List;

		public static readonly bool OldStyle = PropsConfig.OldStyle;

		public static readonly int GumpOffsetX = PropsConfig.GumpOffsetX;
		public static readonly int GumpOffsetY = PropsConfig.GumpOffsetY;

		public static readonly int TextHue = PropsConfig.TextHue;
		public static readonly int TextOffsetX = PropsConfig.TextOffsetX;

		public static readonly int OffsetGumpID = PropsConfig.OffsetGumpID;
		public static readonly int HeaderGumpID = PropsConfig.HeaderGumpID;
		public static readonly int EntryGumpID = PropsConfig.EntryGumpID;
		public static readonly int BackGumpID = PropsConfig.BackGumpID;
		public static readonly int SetGumpID = PropsConfig.SetGumpID;

		public static readonly int SetWidth = PropsConfig.SetWidth;
		public static readonly int SetOffsetX = PropsConfig.SetOffsetX, SetOffsetY = PropsConfig.SetOffsetY;
		public static readonly int SetButtonID1 = PropsConfig.SetButtonID1;
		public static readonly int SetButtonID2 = PropsConfig.SetButtonID2;

		public static readonly int PrevWidth = PropsConfig.PrevWidth;
		public static readonly int PrevOffsetX = PropsConfig.PrevOffsetX, PrevOffsetY = PropsConfig.PrevOffsetY;
		public static readonly int PrevButtonID1 = PropsConfig.PrevButtonID1;
		public static readonly int PrevButtonID2 = PropsConfig.PrevButtonID2;

		public static readonly int NextWidth = PropsConfig.NextWidth;
		public static readonly int NextOffsetX = PropsConfig.NextOffsetX, NextOffsetY = PropsConfig.NextOffsetY;
		public static readonly int NextButtonID1 = PropsConfig.NextButtonID1;
		public static readonly int NextButtonID2 = PropsConfig.NextButtonID2;

		public static readonly int OffsetSize = PropsConfig.OffsetSize;

		public static readonly int EntryHeight = PropsConfig.EntryHeight;
		public static readonly int BorderSize = PropsConfig.BorderSize;

		private static readonly int EntryWidth = 212;

		private static readonly int TotalWidth = OffsetSize + EntryWidth + OffsetSize + SetWidth + OffsetSize;
		private static readonly int TotalHeight = OffsetSize + ( 12 * ( EntryHeight + OffsetSize ) );

		private static readonly int BackWidth = BorderSize + TotalWidth + BorderSize;
		private static readonly int BackHeight = BorderSize + TotalHeight + BorderSize;

		public SetDateTimeGump( PropertyInfo prop, Mobile mobile, object o, Stack stack, int page, ArrayList list )
			: base( GumpOffsetX, GumpOffsetY )
		{
			m_Property = prop;
			m_Mobile = mobile;
			m_Object = o;
			m_Stack = stack;
			m_Page = page;
			m_List = list;

			m_OldDateTime = (DateTime) prop.GetValue( o, null );

			AddPage( 0 );

			AddBackground( 0, 0, BackWidth, BackHeight, BackGumpID );
			AddImageTiled( BorderSize, BorderSize, TotalWidth - ( OldStyle ? SetWidth + OffsetSize : 0 ), TotalHeight, OffsetGumpID );

			AddRect( 0, prop.Name, 0, -1 );
			AddRect( 1, String.Format( "{0:u}", m_OldDateTime ), 0, -1 );
			AddRect( 2, "MinValue", 1, -1 );
			AddRect( 3, "From YYYY:MM:DD hh:mm", 2, -1 );
			AddRect( 4, "From YYYY:MM:DD (old hour)", 3, -1 );
			AddRect( 5, "From hh:mm (old date)", 4, -1 );
			AddRect( 6, "Year", 5, 0 );
			AddRect( 7, "Month", 6, 1 );
			AddRect( 8, "Day", 7, 2 );
			AddRect( 9, "Hour", 8, 3 );
			AddRect( 10, "Minute", 9, 4 );
			AddRect( 11, "MaxValue", 10, -1 );
		}

		private void AddRect( int index, string str, int button, int text )
		{
			int x = BorderSize + OffsetSize;
			int y = BorderSize + OffsetSize + ( index * ( EntryHeight + OffsetSize ) );

			AddImageTiled( x, y, EntryWidth, EntryHeight, EntryGumpID );
			AddLabelCropped( x + TextOffsetX, y, EntryWidth - TextOffsetX, EntryHeight, TextHue, str );

			if ( text != -1 )
				AddTextEntry( x + 40 + TextOffsetX, y, EntryWidth - TextOffsetX - 16, EntryHeight, TextHue, text, "" );

			x += EntryWidth + OffsetSize;

			if ( SetGumpID != 0 )
				AddImageTiled( x, y, SetWidth, EntryHeight, SetGumpID );

			if ( button != 0 )
				AddButton( x + SetOffsetX, y + SetOffsetY, SetButtonID1, SetButtonID2, button, GumpButtonType.Reply, 0 );
		}

		public override void OnResponse( GameClient sender, RelayInfo info )
		{
			DateTime toSet;
			bool shouldSet, shouldSend;

			string Y = "";
			if ( info.ButtonID == 2 || info.ButtonID == 3 || info.ButtonID == 5 )
				Y = info.GetTextEntry( 0 ).Text;

			string M = "";
			if ( info.ButtonID == 2 || info.ButtonID == 3 || info.ButtonID == 6 )
				M = info.GetTextEntry( 1 ).Text;

			string D = "";
			if ( info.ButtonID == 2 || info.ButtonID == 3 || info.ButtonID == 7 )
				D = info.GetTextEntry( 2 ).Text;

			string h = "";
			if ( info.ButtonID == 2 || info.ButtonID == 4 || info.ButtonID == 8 )
				h = info.GetTextEntry( 3 ).Text;

			string m = "";
			if ( info.ButtonID == 2 || info.ButtonID == 4 || info.ButtonID == 9 )
				m = info.GetTextEntry( 4 ).Text;

			switch ( info.ButtonID )
			{
				case 1: // MinValue
					{
						toSet = DateTime.MinValue;
						shouldSet = true;
						shouldSend = true;

						break;
					}
				case 2: // From YYYY MM DD H:M
					{
						string toApply = String.Format( "{0}/{1}/{2} {3}:{4}:00",
							( Y != string.Empty ? Y : String.Format( "{0:yyyy}", m_OldDateTime ) ),
							( M != string.Empty ? M : String.Format( "{0:MM}", m_OldDateTime ) ), ( D != string.Empty ? D : String.Format( "{0:dd}", m_OldDateTime ) ),
							( h != string.Empty ? h : String.Format( "{0:HH}", m_OldDateTime ) ), ( m != string.Empty ? m : String.Format( "{0:mm}", m_OldDateTime ) ) );

						bool successfulParse = DateTime.TryParse( toApply, out toSet );
						shouldSet = shouldSend = successfulParse;

						break;
					}
				case 3:
				case 4:
				case 5:
				case 6:
				case 7:
				case 8:
				case 9:
					{
						goto case 2;
					}
				case 10:
					{
						toSet = DateTime.MaxValue;
						shouldSet = true;
						shouldSend = true;

						break;
					}
				default:
					{
						toSet = DateTime.MinValue;
						shouldSet = false;
						shouldSend = true;

						break;
					}
			}

			if ( shouldSet )
			{
				try
				{
					Server.Scripts.Commands.CommandLogging.LogChangeProperty( m_Mobile, m_Object, m_Property.Name, toSet.ToString() );
					m_Property.SetValue( m_Object, toSet, null );
					PropertiesGump.OnValueChanged( m_Object, m_Property, m_Stack );
				}
				catch
				{
					m_Mobile.SendMessage( "An exception was caught. The property may not have changed." );
				}
			}

			if ( shouldSend )
				m_Mobile.SendGump( new PropertiesGump( m_Mobile, m_Object, m_Stack, m_List, m_Page ) );
		}
	}
}
