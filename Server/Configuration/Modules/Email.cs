using System;

namespace Server.Configuration
{
	[ConfigModule]
	public class Email
	{
		public string SmtpServer { get; set; }
		public string SmtpUser { get; set; }
		public string SmtpPassword { get; set; }
		public int SmtpPort { get; set; }
		public bool EnableSsl { get; set; }
	}
}
