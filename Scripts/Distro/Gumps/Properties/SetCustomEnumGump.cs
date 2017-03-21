using System;
using System.Reflection;
using System.Collections;
using Server;
using Server.Network;
using Server.Misc;

namespace Server.Gumps
{
	public class SetCustomEnumGump : SetListOptionGump
	{
		private string[] m_Names;

		public SetCustomEnumGump( PropertyInfo prop, Mobile mobile, object o, Stack stack, int propspage, ArrayList list, string[] names )
			: base( prop, mobile, o, stack, propspage, list, names, null )
		{
			m_Names = names;
		}

		public override void OnResponse( NetState sender, RelayInfo relayInfo )
		{
			int index = relayInfo.ButtonID - 1;

			if ( index >= 0 && index < m_Names.Length )
			{
				try
				{
					MethodInfo info = m_Property.PropertyType.GetMethod( "Parse", new Type[] { typeof( string ) } );

					Server.Scripts.Commands.CommandLogging.LogChangeProperty( m_Mobile, m_Object, m_Property.Name, m_Names[index] );

					if ( info != null )
						m_Property.SetValue( m_Object, info.Invoke( null, new object[] { m_Names[index] } ), null );
					else if ( m_Property.PropertyType == typeof( Enum ) || m_Property.PropertyType.IsSubclassOf( typeof( Enum ) ) )
						m_Property.SetValue( m_Object, Enum.Parse( m_Property.PropertyType, m_Names[index], false ), null );

					PropertiesGump.OnValueChanged( m_Object, m_Property, m_Stack );
				}
				catch
				{
					m_Mobile.SendMessage( "An exception was caught. The property may not have changed." );
				}
			}

			m_Mobile.SendGump( new PropertiesGump( m_Mobile, m_Object, m_Stack, m_List, m_Page ) );
		}
	}
}
