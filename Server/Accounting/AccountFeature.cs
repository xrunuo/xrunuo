using System;

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
}
