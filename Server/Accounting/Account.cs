using System;
using System.Collections;
using System.Xml;

namespace Server.Accounting
{
	[Flags]
	public enum AccountFeature
	{
		None = 0x00,
		BetaTester = 0x01,
		TheEightAge = 0x02,
		NinthAnniversary = 0x04,
		NoExpire = 0x08,
		SeventhCharacter = 0x10
	}

	public interface IAccount
	{
		int Length { get; }
		int Limit { get; }
		int Count { get; }
		bool FeatureEnabled( AccountFeature feature );
		Mobile this[int index] { get; set; }
	}
}
