namespace Server.Configuration
{
	[ConfigModule]
	public class Login
	{
		public bool IgnoreAuthID { get; set; }
		public bool AutoCreateAccounts { get; set; }
		public int MaxAccountsPerIP { get; set; }
		public int MaxLoginsPerIP { get; set; }
		public int MaxLoginsPerPC { get; set; }
		public bool ProtectPasswords { get; set; }
	}
}