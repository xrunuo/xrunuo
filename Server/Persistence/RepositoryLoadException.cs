using System;

namespace Server.Persistence
{
	public class RepositoryLoadException : Exception
	{
		public IEntityRepositoryLoad RepositoryLoad { get; }

		public Serial FailedSerial { get; }

		public Type FailedType { get; }

		public int FailedTypeId { get; }

		public RepositoryLoadException( IEntityRepositoryLoad repositoryLoad, Exception innerException, Serial failedSerial, Type failedType, int failedTypeId )
			: base( $"Load failed (repository={repositoryLoad}, type={failedType}, serial={failedSerial})", innerException )
		{
			RepositoryLoad = repositoryLoad;
			FailedSerial = failedSerial;
			FailedType = failedType;
			FailedTypeId = failedTypeId;
		}
	}
}
