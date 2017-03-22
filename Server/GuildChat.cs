using System;

namespace Server
{
	public abstract class GuildChat
	{
		private static GuildChat m_Handler;

		public static GuildChat Handler { get { return m_Handler; } set { m_Handler = value; } }

		public abstract void OnGuildMessage( Mobile from, string text );
		public abstract void OnAllianceMessage( Mobile from, string text );
	}
}