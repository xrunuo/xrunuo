using System;

namespace Server.Configuration
{
	[ConfigModule]
	public class Reports
	{
		public bool Enabled { get; set; }
		public string FtpHost { get; set; }
		public string FtpUsername { get; set; }
		public string FtpPassword { get; set; }
		public string FtpStatsDirectory { get; set; }
		public string FtpStaffDirectory { get; set; }
	}
}