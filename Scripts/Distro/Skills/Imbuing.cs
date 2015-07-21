using System;
using Server;

namespace Server.SkillHandlers
{
	public class Imbuing
	{
		public static void Initialize()
		{
			SkillInfo.Table[(int) SkillName.Imbuing].Callback = new SkillUseCallback( OnUse );
		}

		public static TimeSpan OnUse( Mobile m )
		{
			m.BeginAction( typeof( Engines.Imbuing.Imbuing ) );
			m.SendGump( new Engines.Imbuing.ImbuingMainGump() );

			return TimeSpan.FromSeconds( 1.0 );
		}
	}
}