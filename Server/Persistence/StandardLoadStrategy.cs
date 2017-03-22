using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.Persistence
{
	public class StandardLoadStrategy : LoadStrategy
	{
		public override string Name
		{
			get { return "Standard"; }
		}

		public override void LoadEntities( IEnumerable<IEntityRepository> repositories )
		{
			var repositoryLoads = repositories.Select( repository => repository.CreateEntityRepositoryLoad() ).ToArray();

			foreach ( var repositoryLoad in repositoryLoads )
			{
				repositoryLoad.LoadEntityIndex();
			}

			try
			{
				foreach ( var repositoryLoad in repositoryLoads )
				{
					repositoryLoad.DeserializeEntities();
				}
			}
			catch ( RepositoryLoadException e )
			{
				Console.WriteLine( e.InnerException );

				Console.WriteLine( "An error was encountered while loading a saved object" );

				Console.WriteLine( " - Type: {0}", e.FailedType );
				Console.WriteLine( " - Serial: {0}", e.FailedSerial );

				Console.WriteLine( "Delete the object? (y/n)" );

				if ( Console.ReadKey().Key == ConsoleKey.Y )
				{
					e.RepositoryLoad.OnError( e.FailedTypeId );

					foreach ( var repositoryLoad in repositoryLoads )
					{
						repositoryLoad.SaveIndex();
					}
				}

				Console.WriteLine( "After pressing return an exception will be thrown and the server will terminate" );
				Console.ReadLine();

				throw;
			}
		}
	}
}
