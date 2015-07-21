using System;
using System.Collections;
using Server.Items;

namespace Server.Engines.Craft
{
	public enum CraftECA
	{
		ChanceMinusSixty,
		FiftyPercentChanceMinusTenPercent,
		ChanceMinusSixtyToFourtyFive
	}

	public abstract class CraftSystem
	{
		private int m_MinCraftEffect;
		private int m_MaxCraftEffect;

		private double m_Delay;

		private bool m_Resmelt;
		private bool m_Repair;
		private bool m_MarkOption;
		private bool m_CanEnhance;
		private bool m_Alter;

		private CraftItemCol m_CraftItems;
		private CraftGroupCol m_CraftGroups;
		private CraftSubResCol m_CraftSubRes;
		private CraftSubResCol m_CraftSubRes2;

		public int MinCraftEffect { get { return m_MinCraftEffect; } }
		public int MaxCraftEffect { get { return m_MaxCraftEffect; } }

		public double Delay { get { return m_Delay; } }

		public CraftItemCol CraftItems { get { return m_CraftItems; } }
		public CraftGroupCol CraftGroups { get { return m_CraftGroups; } }
		public CraftSubResCol CraftSubRes { get { return m_CraftSubRes; } }
		public CraftSubResCol CraftSubRes2 { get { return m_CraftSubRes2; } }

		public abstract SkillName MainSkill { get; }

		public virtual int GumpTitleNumber { get { return 0; } }
		public virtual string GumpTitleString { get { return ""; } }

		public virtual CraftECA ECA { get { return CraftECA.ChanceMinusSixty; } }

		private Hashtable m_ContextTable = new Hashtable();

		public abstract double DefaultChanceAtMin { get; }

		public virtual double GetChanceAtMin( CraftItem item )
		{
			if ( !item.OverridesChanceAtMin )
				return DefaultChanceAtMin;

			return item.ChanceAtMin;
		}

		public virtual bool RetainsColorFrom( CraftItem item, Type type )
		{
			return false;
		}

		public CraftContext GetContext( Mobile m )
		{
			if ( m == null )
				return null;

			if ( m.Deleted )
			{
				m_ContextTable.Remove( m );
				return null;
			}

			CraftContext c = (CraftContext) m_ContextTable[m];

			if ( c == null )
				m_ContextTable[m] = c = new CraftContext();

			return c;
		}

		public void OnMade( Mobile m, CraftItem item )
		{
			CraftContext c = GetContext( m );

			if ( c != null )
				c.OnMade( item );
		}

		public bool Resmelt { get { return m_Resmelt; } set { m_Resmelt = value; } }
		public bool Repair { get { return m_Repair; } set { m_Repair = value; } }
		public bool MarkOption { get { return m_MarkOption; } set { m_MarkOption = value; } }
		public bool CanEnhance { get { return m_CanEnhance; } set { m_CanEnhance = value; } }
		public bool Alter { get { return m_Alter; } set { m_Alter = value; } }

		public CraftSystem( int minCraftEffect, int maxCraftEffect, double delay )
		{
			m_MinCraftEffect = minCraftEffect;
			m_MaxCraftEffect = maxCraftEffect;
			m_Delay = delay;

			m_CraftItems = new CraftItemCol();
			m_CraftGroups = new CraftGroupCol();
			m_CraftSubRes = new CraftSubResCol();
			m_CraftSubRes2 = new CraftSubResCol();

			InitCraftList();
		}

		public virtual bool ConsumeOnFailure( Mobile from, Type resourceType, CraftItem craftItem )
		{
			return true;
		}

		public void CreateItem( Mobile from, Type type, Type typeRes, BaseTool tool, CraftItem realCraftItem )
		{
			// Verify if the type is in the list of the craftable item
			CraftItem craftItem = m_CraftItems.SearchFor( type );
			if ( craftItem != null )
			{
				// The item is in the list, try to create it
				// Test code: items like sextant parts can be crafted either directly from ingots, or from different parts
				realCraftItem.Craft( from, this, typeRes, tool );
				//craftItem.Craft( from, this, typeRes, tool );
			}
		}

		public CraftItem AddCraft( int index, Type typeItem, TextDefinition group, TextDefinition name, double minSkill, double maxSkill, Type typeRes, TextDefinition nameRes, int amount )
		{
			return AddCraft( index, typeItem, group, name, MainSkill, minSkill, maxSkill, typeRes, nameRes, amount, "" );
		}

		public CraftItem AddCraft( int index, Type typeItem, TextDefinition group, TextDefinition name, double minSkill, double maxSkill, Type typeRes, TextDefinition nameRes, int amount, TextDefinition message )
		{
			return AddCraft( index, typeItem, group, name, MainSkill, minSkill, maxSkill, typeRes, nameRes, amount, message );
		}

		public CraftItem AddCraft( int index, Type typeItem, TextDefinition group, TextDefinition name, SkillName skillToMake, double minSkill, double maxSkill, Type typeRes, TextDefinition nameRes, int amount )
		{
			return AddCraft( index, typeItem, group, name, skillToMake, minSkill, maxSkill, typeRes, nameRes, amount, "" );
		}

		public CraftItem AddCraft( int index, Type typeItem, TextDefinition group, TextDefinition name, SkillName skillToMake, double minSkill, double maxSkill, Type typeRes, TextDefinition nameRes, int amount, TextDefinition message )
		{
			CraftItem craftItem = new CraftItem( typeItem, group, name );

			craftItem.AddRes( typeRes, nameRes, amount, message );
			craftItem.AddSkill( skillToMake, minSkill, maxSkill );
			craftItem.CraftId = index;

			if ( typeItem != null )
				DoGroup( group, index, craftItem );

			m_CraftItems.Add( index, craftItem );

			return craftItem;
		}

		private void DoGroup( TextDefinition groupName, int craftIndex, CraftItem craftItem )
		{
			int index = m_CraftGroups.SearchFor( groupName );

			if ( index == -1 )
			{
				CraftGroup craftGroup = new CraftGroup( groupName );
				craftGroup.AddCraftItem( craftIndex, craftItem );
				m_CraftGroups.Add( craftGroup );
			}
			else
			{
				m_CraftGroups.GetAt( index ).AddCraftItem( craftIndex, craftItem );
			}
		}

		public void SetSubRes( Type type, string name )
		{
			m_CraftSubRes.ResType = type;
			m_CraftSubRes.NameString = name;
			m_CraftSubRes.Init = true;
		}

		public void SetSubRes( Type type, int name )
		{
			m_CraftSubRes.ResType = type;
			m_CraftSubRes.NameNumber = name;
			m_CraftSubRes.Init = true;
		}

		public void AddSubRes( Type type, int name, double reqSkill, object message )
		{
			CraftSubRes craftSubRes = new CraftSubRes( type, name, reqSkill, message );
			m_CraftSubRes.Add( craftSubRes );
		}

		public void AddSubRes( Type type, int name, double reqSkill, int genericName, object message )
		{
			CraftSubRes craftSubRes = new CraftSubRes( type, name, reqSkill, genericName, message );
			m_CraftSubRes.Add( craftSubRes );
		}

		public void AddSubRes( Type type, string name, double reqSkill, object message )
		{
			CraftSubRes craftSubRes = new CraftSubRes( type, name, reqSkill, message );
			m_CraftSubRes.Add( craftSubRes );
		}

		public void SetSubRes2( Type type, string name )
		{
			m_CraftSubRes2.ResType = type;
			m_CraftSubRes2.NameString = name;
			m_CraftSubRes2.Init = true;
		}

		public void SetSubRes2( Type type, int name )
		{
			m_CraftSubRes2.ResType = type;
			m_CraftSubRes2.NameNumber = name;
			m_CraftSubRes2.Init = true;
		}

		public void AddSubRes2( Type type, int name, double reqSkill, object message )
		{
			CraftSubRes craftSubRes = new CraftSubRes( type, name, reqSkill, message );
			m_CraftSubRes2.Add( craftSubRes );
		}

		public void AddSubRes2( Type type, int name, double reqSkill, int genericName, object message )
		{
			CraftSubRes craftSubRes = new CraftSubRes( type, name, reqSkill, genericName, message );
			m_CraftSubRes2.Add( craftSubRes );
		}

		public void AddSubRes2( Type type, string name, double reqSkill, object message )
		{
			CraftSubRes craftSubRes = new CraftSubRes( type, name, reqSkill, message );
			m_CraftSubRes2.Add( craftSubRes );
		}

		public abstract void InitCraftList();

		public abstract void PlayCraftEffect( Mobile from );
		public abstract int PlayEndingEffect( Mobile from, bool failed, bool lostMaterial, bool toolBroken, bool exceptional, bool makersMark, CraftItem item );

		public abstract int CanCraft( Mobile from, BaseTool tool, Type itemType );
	}
}