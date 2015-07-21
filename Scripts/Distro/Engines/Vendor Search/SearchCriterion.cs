using System;
using Server;
using Server.Mobiles;

namespace Server.Engines.VendorSearch
{
	public abstract class SearchCriterion
	{
		/// <summary>
		/// The name that will be shown in the gump.
		/// </summary>
		public abstract int LabelNumber { get; }

		/// <summary>
		/// The args that will be shown in the gump.
		/// </summary>
		public virtual string LabelArgs { get { return null; } }

		/// <summary>
		/// Returns whether the given vendor item will appear in the search results filtered by this criterion.
		/// </summary>
		/// <param name="item">The item</param>
		/// <returns></returns>
		public abstract bool Matches( IVendorSearchItem item );

		public virtual string ReplacementKey
		{
			get { return GetType().FullName; }
		}

		public string TypeObjectKey
		{
			get { return GetType().FullName; }
		}

		public SearchCriterion Clone()
		{
			return (SearchCriterion) Activator.CreateInstance( this.GetType() );
		}
	}
}
