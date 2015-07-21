using System;
using Server;

namespace Server.Accounting
{
	public class AccountPrompt
	{
		public static void Initialize()
		{
			if ( Accounts.Count == 0 && !Environment.Service )
			{
				Console.WriteLine( "Warning: This server has no accounts." );
				Console.Write( "Do you want to create an owner account now? (y/n)" );

				if ( Console.ReadKey().Key == ConsoleKey.Y )
				{
					Console.Write( "Username: " );
					string username = Console.ReadLine();

					Console.Write( "Password: " );
					string password = Console.ReadLine();

					Account a = Accounts.CreateAccount( username, password );

					a.AccessLevel = AccessLevel.Owner;

					Console.WriteLine( "Account created, continuing..." );
				}
				else
				{
					Console.WriteLine( "Account not created, continuing..." );
				}
			}
		}
	}
}
