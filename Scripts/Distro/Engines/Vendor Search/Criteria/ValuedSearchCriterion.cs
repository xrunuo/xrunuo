using System;
using Server;
using Server.Mobiles;

namespace Server.Engines.VendorSearch
{
	public abstract class ValuedSearchCriterion : SearchCriterion
	{
		private int m_Value;

		/// <summary>
		/// Gets or sets the value of this criterion
		/// </summary>
		public int Value
		{
			get { return m_Value; }
			set { m_Value = value; }
		}

		public override string LabelArgs { get { return Value.ToString(); } }
	}
}
