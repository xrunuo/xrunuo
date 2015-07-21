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

namespace Server.Persistence
{
	public class RepositoryLoadException : Exception
	{
		private readonly IEntityRepositoryLoad m_RepositoryLoad;
		private readonly Serial m_FailedSerial;
		private readonly Type m_FailedType;
		private readonly int m_FailedTypeId;

		public IEntityRepositoryLoad RepositoryLoad { get { return m_RepositoryLoad; } }
		public Serial FailedSerial { get { return m_FailedSerial; } }
		public Type FailedType { get { return m_FailedType; } }
		public int FailedTypeId { get { return m_FailedTypeId; } }

		public RepositoryLoadException( IEntityRepositoryLoad repositoryLoad, Exception innerException, Serial failedSerial, Type failedType, int failedTypeId )
			: base( String.Format( "Load failed (repository={0}, type={1}, serial={2})", repositoryLoad, failedType, failedSerial ), innerException )
		{
			m_RepositoryLoad = repositoryLoad;
			m_FailedSerial = failedSerial;
			m_FailedType = failedType;
			m_FailedTypeId = failedTypeId;
		}
	}
}
