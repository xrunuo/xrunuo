using System;
using System.Collections.Generic;

namespace Server.Engines.Craft
{
	public enum CraftMarkOption
	{
		MarkItem,
		DoNotMark,
		PromptForMark
	}

	public class CraftContext
	{
		private List<CraftItem> m_Items;
		private int m_LastResourceIndex;
		private int m_LastResourceIndex2;
		private int m_LastGroupIndex;
		private bool m_DoNotColor;
		private CraftMarkOption m_MarkOption;
		private bool m_QuestItem;
		private bool m_MakeLast;
		private bool m_Success;
		private CraftItem m_Making;
		private int m_Done, m_Total = 1;

		public List<CraftItem> Items
		{
			get { return m_Items; }
		}

		public int LastResourceIndex
		{
			get { return m_LastResourceIndex; }
			set { m_LastResourceIndex = value; }
		}

		public int LastResourceIndex2
		{
			get { return m_LastResourceIndex2; }
			set { m_LastResourceIndex2 = value; }
		}

		public int LastGroupIndex
		{
			get { return m_LastGroupIndex; }
			set { m_LastGroupIndex = value; }
		}

		public bool DoNotColor
		{
			get { return m_DoNotColor; }
			set { m_DoNotColor = value; }
		}

		public CraftMarkOption MarkOption
		{
			get { return m_MarkOption; }
			set { m_MarkOption = value; }
		}

		public bool QuestItem
		{
			get { return m_QuestItem; }
			set { m_QuestItem = value; }
		}

		public bool MakeLast
		{
			get { return m_MakeLast; }
			set { m_MakeLast = value; }
		}

		public bool Success
		{
			get { return m_Success; }
			set { m_Success = value; }
		}

		public int Done
		{
			get { return m_Done; }
			set
			{
				m_Done = value;

				if ( m_Done >= m_Total )
					m_Making = null;
			}
		}

		public int Total
		{
			get { return m_Total; }
			set
			{
				m_Total = value;
				m_Done = 0;
			}
		}

		public CraftItem Making
		{
			get { return m_Making; }
			set
			{
				if ( m_Making != value )
					m_Done = 0;

				m_Making = value;
			}
		}

		public bool MakeMax { get { return m_Total == 9999; } }
		public bool MakeNumber { get { return !MakeMax && m_Total > 1; } }

		public CraftContext()
		{
			m_Items = new List<CraftItem>();
			m_LastResourceIndex = -1;
			m_LastResourceIndex2 = -1;
			m_LastGroupIndex = -1;
		}

		public CraftItem LastMade
		{
			get
			{
				if ( m_Items.Count > 0 )
					return m_Items[0];

				return null;
			}
		}

		public void OnMade( CraftItem item )
		{
			m_Items.Remove( item );

			if ( m_Items.Count == 10 )
				m_Items.RemoveAt( 9 );

			m_Items.Insert( 0, item );
		}
	}
}