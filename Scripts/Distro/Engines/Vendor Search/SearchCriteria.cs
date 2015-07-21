using System;
using System.Collections.Generic;
using System.Linq;
using Server;
using Server.Mobiles;

namespace Server.Engines.VendorSearch
{
	public class SearchCriteria
	{
		public static readonly int DefaultMinPrice = 0;
		public static readonly int DefaultMaxPrice = 175000000;

		public static readonly int MaxCount = 20;

		private Dictionary<string, SearchCriterion> m_Criteria;
		private string m_Name;
		private int m_MinPrice, m_MaxPrice;

		public string Name
		{
			get
			{
				return m_Name;
			}
			set
			{
				if ( value != null )
					m_Name = value.Trim().ToLower();
				else
					m_Name = "";
			}
		}

		public int MinPrice
		{
			get
			{
				return m_MinPrice;
			}
			set
			{
				m_MinPrice = value;
				Utility.FixMinMax( ref m_MinPrice, DefaultMinPrice, DefaultMaxPrice );
			}
		}

		public int MaxPrice
		{
			get
			{
				return m_MaxPrice;
			}
			set
			{
				m_MaxPrice = value;
				Utility.FixMinMax( ref m_MaxPrice, DefaultMinPrice, DefaultMaxPrice );
			}
		}

		public int Count
		{
			get
			{
				int count = m_Criteria.Count;

				if ( HasName )
					count++;

				if ( HasPrice )
					count++;

				return count;
			}
		}

		public bool HasName
		{
			get { return !string.IsNullOrEmpty( m_Name ); }
		}

		public bool HasPrice
		{
			get { return m_MinPrice != DefaultMinPrice || m_MaxPrice != DefaultMaxPrice; }
		}

		public SearchCriteria()
		{
			m_Criteria = new Dictionary<string, SearchCriterion>();
			m_Name = "";
			m_MinPrice = DefaultMinPrice;
			m_MaxPrice = DefaultMaxPrice;
		}

		public void Add( SearchCriterion sc )
		{
			String key = sc.ReplacementKey;

			if ( m_Criteria.ContainsKey( key ) )
				m_Criteria.Remove( key );

			m_Criteria.Add( key, sc );
		}

		public void Remove( SearchCriterion sc )
		{
			String key = sc.ReplacementKey;

			if ( m_Criteria.ContainsKey( key ) )
				m_Criteria.Remove( key );
		}

		public bool Matches( IVendorSearchItem item )
		{
			if ( item.Price < m_MinPrice || item.Price > m_MaxPrice )
				return false;

			Item i = item.Item;

			if ( !string.IsNullOrEmpty( m_Name ) )
			{
				string name = i.Name ?? i.GetNameProperty().Delocalize().ToLower();

				if ( !name.Contains( m_Name ) )
					return false;
			}

			return m_Criteria.Values.All( c => c.Matches( item ) );
		}

		public void Clear()
		{
			m_Criteria.Clear();
			m_Name = "";
			m_MinPrice = DefaultMinPrice;
			m_MaxPrice = DefaultMaxPrice;
		}

		public IEnumerable<SearchCriterion> GetSelectedCriteria()
		{
			return m_Criteria.Values;
		}

		public static Dictionary<Mobile, SearchCriteria> m_PlayerCriteria = new Dictionary<Mobile, SearchCriteria>();

		public static SearchCriteria GetSearchCriteriaForPlayer( Mobile m )
		{
			SearchCriteria criteria;

			if ( m_PlayerCriteria.ContainsKey( m ) )
				criteria = m_PlayerCriteria[m];
			else
				criteria = m_PlayerCriteria[m] = new SearchCriteria();

			return criteria;
		}

		public string GetSelectedValue( SearchCriterion sc )
		{
			if ( !( sc is ValuedSearchCriterion ) )
				return null;

			SearchCriterion criterion;

			if ( !m_Criteria.TryGetValue( sc.ReplacementKey, out criterion ) || sc.TypeObjectKey != criterion.TypeObjectKey )
				return string.Empty;

			ValuedSearchCriterion valuedCriterion = (ValuedSearchCriterion) criterion;
			return valuedCriterion.Value.ToString();
		}
	}
}
