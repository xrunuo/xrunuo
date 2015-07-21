using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Server.Accounting
{
	public static class Accounts
	{
		private static Dictionary<string, Account> m_Accounts;

		static Accounts()
		{
			m_Accounts = new Dictionary<string, Account>( 32, StringComparer.OrdinalIgnoreCase );
		}

		public static int Count { get { return m_Accounts.Count; } }

		public static IEnumerable<Account> GetAccounts()
		{
			return m_Accounts.Values;
		}

		public static Account GetAccount( string username )
		{
			if ( !m_Accounts.ContainsKey( username ) )
				return null;

			return m_Accounts[username];
		}

		public static Account AddAccount( Account account )
		{
			m_Accounts[account.Username] = account;

			return account;
		}

		public static Account CreateAccount( string username, string password, string linkedEmail = null )
		{
			Account account = new Account( username, password, linkedEmail );

			if ( m_Accounts.Count == 0 )
				account.AccessLevel = AccessLevel.Owner;

			return AddAccount( account );
		}

		public static void RemoveAccount( string username )
		{
			m_Accounts.Remove( username );
		}
	}
}