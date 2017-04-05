using System;

namespace Server
{
	public abstract class GuildChat
	{
		public static GuildChat Handler { get; set; }

		public abstract void OnGuildMessage( Mobile from, string text );
		public abstract void OnAllianceMessage( Mobile from, string text );
	}
}