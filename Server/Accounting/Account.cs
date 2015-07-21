//
//  X-RunUO - Ultima Online Server Emulator
//  Copyright (C) 2015 Pedro Pardal
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//

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
