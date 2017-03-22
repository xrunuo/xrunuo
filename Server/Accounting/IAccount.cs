using System;

namespace Server.Accounting
{
	public interface IAccount
	{
		int Length { get; }
		int Limit { get; }
		int Count { get; }
		bool FeatureEnabled( AccountFeature feature );
		Mobile this[int index] { get; set; }
	}
}
