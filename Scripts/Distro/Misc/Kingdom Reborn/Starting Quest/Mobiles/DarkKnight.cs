using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Gumps;

namespace Server.Mobiles
{
	public class DarkKnight : Mobile
	{
		[Constructable]
		public DarkKnight()
		{
			Name = "Dark Knight";
			Title = "the Challenger of Ol'Haven";
			Body = 311;
			Blessed = true;
			Hue = 33779;
		}

		public override void OnDoubleClick( Mobile from )
		{
			base.OnDoubleClick( from );

			if ( from is PlayerMobile )
				((PlayerMobile)from).CheckKRStartingQuestStep( 29 );
		}

		public DarkKnight( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			
			/*int version = */reader.ReadInt();
		}
	}

	public class DarkKnightGump : Gump
	{
		public override int TypeID { get { return 0x328; } }

		public DarkKnightGump() : base( 75, 25 )
		{
			Intern( "" );

			AddPage( 1 );

			Closable = false;
			
			AddImageTiled( 50, 20, 400, 400, 0x1404 );
			AddImageTiled( 50, 29, 30, 390, 0x28DC );
			AddImageTiled( 34, 140, 17, 279, 0x242F );
			AddImage( 48, 135, 0x28AB );
			AddImage( -16, 285, 0x28A2 );
			AddImage( 0, 10, 0x28B5 );
			AddImage( 25, 0, 0x28B4 );
			AddImageTiled( 83, 15, 350, 15, 0x280A );
			AddImage( 34, 419, 0x2842 );
			AddImage( 442, 419, 0x2840 );
			AddImageTiled( 51, 419, 392, 17, 0x2775 );
			AddImageTiled( 415, 29, 44, 390, 0xA2D );
			AddImageTiled( 415, 29, 30, 390, 0x28DC );
			AddLabelIntern( 100, 50, 0x481, 0 );
			AddImage( 370, 50, 0x589 );
			AddImage( 379, 60, 0x15A9 );
			AddImage( 425, 0, 0x28C9 );
			AddImage( 90, 33, 0x232D );
			AddHtmlLocalized( 130, 45, 270, 16, 1060668, 0xFFFFFF, false, false ); // INFORMATION
			AddImageTiled( 130, 65, 175, 1, 0x238D );
			AddButton( 95, 395, 0x2EE6, 0x2EE8, 3, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 160, 108, 250, 16, 1077712, 0x2710, false, false ); // A Dark Challenge

			/*
			 * Well, well. Another youngling. You are
			 * here to prove your mettle. I think you
			 * have none. But there is some persistence
			 * in you. You may learn to master that weapon
			 * someday.
			 * 
			 * When you get some training under your belt
			 * and feel you have mastered your arts, come
			 * find me, so I can eliminate those who seek
			 * to keep this world from dark clutches.
			 * 
			 * Take this Rune. When you feel you have
			 * mastered your skills, come and find me.
			 * I will be waiting.
			 */
			AddHtmlLocalized( 98, 156, 312, 180, 1077655, 0x15F90, false, true );
		}

		public override void OnResponse( GameClient sender, RelayInfo info )
		{
			if ( sender.Mobile is PlayerMobile )
				((PlayerMobile)sender.Mobile).CheckKRStartingQuestStep( 30 );
		}
	}
}