using System;
using Server;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;

namespace Server.Factions
{
	public class JoinStoneGump : FactionGump
	{
		public override int TypeID { get { return 0x2328; } }

		private PlayerMobile m_From;
		private Faction m_Faction;

		public JoinStoneGump( PlayerMobile from, Faction faction )
			: base( 20, 30 )
		{
			m_From = from;
			m_Faction = faction;

			/*0*/
			Intern( faction.Commander != null ? faction.Commander.Name : "" );
			/*1*/
			Intern( from.Name );
			/*2*/
			Intern( "0" );
			/*3*/
			Intern( "0" );
			/*4*/
			Intern( faction.Tithe.ToString() );
			/*5*/
			Intern( "0" );
			/*6*/
			Intern( "0" );

			AddPage( 0 );

			AddBackground( 0, 0, 550, 440, 5054 );
			AddBackground( 10, 10, 530, 420, 3000 );

			AddPage( 1 );

			AddHtmlLocalized( 20, 30, 510, 20, faction.Definition.Header.Number, false, false );

			AddHtmlLocalized( 20, 60, 100, 20, 1011429, false, false ); // Led By : 
			if ( faction.Commander != null )
				AddHtmlIntern( 125, 60, 200, 20, 0, false, false );
			else
				AddHtmlLocalized( 125, 60, 200, 20, 1011051, false, false ); // None

			AddHtmlLocalized( 20, 80, 100, 20, 1011457, false, false ); // Tithe rate : 
			AddHtmlLocalized( 125, 80, 350, 20, 1011480 + ( faction.Tithe / 10 ), false, false );

			AddHtmlLocalized( 20, 130, 510, 100, faction.Definition.About.Number, true, true );

			AddHtmlLocalized( 55, 400, 200, 20, 1011425, false, false ); // JOIN THIS FACTION
			AddButton( 20, 400, 4005, 4007, 1, GumpButtonType.Reply, 0 );

			AddHtmlLocalized( 335, 400, 200, 20, 1011012, false, false ); // CANCEL
			AddButton( 300, 400, 4005, 4007, 0, GumpButtonType.Reply, 0 );
		}

		public override void OnResponse( NetState sender, RelayInfo info )
		{
			if ( info.ButtonID == 1 )
				m_Faction.OnJoinAccepted( m_From );
		}
	}
}