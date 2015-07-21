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
