using System;
using Server;
using Server.Prompts;

namespace Server.Multis
{
	public class RenameBoatPrompt : Prompt
	{
		// What dost thou wish to name thy ship?
		public override int MessageCliloc { get { return 502580; } }

		private BaseBoat m_Boat;

		public RenameBoatPrompt( BaseBoat boat )
			: base( boat )
		{
			m_Boat = boat;
		}

		public override void OnResponse( Mobile from, string text )
		{
			m_Boat.EndRename( from, text );
		}
	}
}