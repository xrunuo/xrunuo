using Server.Network;

namespace Server.Admin
{
	public delegate bool IsAuth( NetState ns );

	public class Stub
	{
		public static event IsAuth m_IsAuth;

		public static bool IsAuth( NetState ns )
		{
			return m_IsAuth == null
				? false
				: m_IsAuth( ns );
		}
	}
}
