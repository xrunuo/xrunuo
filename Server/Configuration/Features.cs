using System;
using System.Collections;

namespace Server.Configuration
{
	public class Features
	{
		private Hashtable m_Table = new Hashtable();

		public bool this[string name]
		{
			get
			{
				return m_Table.Contains( name );
			}
			set
			{
				if ( value )
					m_Table[name] = true;
				else
					m_Table.Remove( name );
			}
		}
	}
}