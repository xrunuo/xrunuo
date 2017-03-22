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
