using System;
using System.Collections;
using Server;
using Server.Mobiles;

namespace Server.Engines.Craft
{
	public class Recipe
	{
		private static Hashtable m_Recipes = new Hashtable();

		public static Hashtable Recipes { get { return m_Recipes; } }

		private static int m_LargestRecipeID;
		public static int LargestRecipeID { get { return m_LargestRecipeID; } }

		private CraftSystem m_System;

		public CraftSystem CraftSystem
		{
			get { return m_System; }
			set { m_System = value; }
		}

		private CraftItem m_CraftItem;

		public CraftItem CraftItem
		{
			get { return m_CraftItem; }
			set { m_CraftItem = value; }
		}

		private int m_ID;

		public int ID
		{
			get { return m_ID; }
		}

		private TextDefinition m_TD;
		public TextDefinition TextDefinition
		{
			get
			{
				if ( m_TD == null )
					m_TD = new TextDefinition( m_CraftItem.NameNumber, m_CraftItem.NameString );

				return m_TD;
			}
		}

		public Recipe( int id, CraftSystem system, CraftItem item )
		{
			m_ID = id;
			m_System = system;
			m_CraftItem = item;

			if ( m_Recipes.ContainsKey( id ) )
				throw new Exception( "Attempting to create recipe with preexisting ID." );

			m_Recipes.Add( id, this );
			m_LargestRecipeID = Math.Max( id, m_LargestRecipeID );
		}
	}
}
