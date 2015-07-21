using System;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Mobiles;
using Server.Items;
using Server.Engines.Quests;
using Server.Events;

namespace Server.Engines.Quests.HumilityCloak
{
	public class HumilityProofGump : GenericQuestGump
	{
		public static void Initialize()
		{
			EventSink.Instance.Login += new LoginEventHandler( OnLogin );
		}

		private static void OnLogin( LoginEventArgs e )
		{
			PlayerMobile pm = e.Mobile as PlayerMobile;

			if ( pm != null && pm.HumilityQuestStatus == HumilityQuestStatus.RewardPending )
				pm.SendGump( new HumilityProofGump() );
		}

		public HumilityProofGump()
			: base( 1075782, GenericQuestGumpButton.Accept | GenericQuestGumpButton.Refuse, OnAccepted, OnCanceled )
		{
			/* Noble friend, thou hast performed tremendously! On behalf of the Rise
			 * of Britannia I wish to reward thee with this golden shield, a symbol
			 * of accomplishment and pride for the many things that thou hast done
			 * for our people.
			 * 
			 * Dost thou accept? */
		}

		public static void OnAccepted( Mobile from )
		{
			if ( from.Backpack == null )
				return;

			PlainGreyCloak cloak = from.FindItemOnLayer( Layer.Cloak ) as PlainGreyCloak;

			if ( cloak == null )
			{
				from.DropHolding();
				cloak = from.Backpack.FindItemByType<PlainGreyCloak>();
			}

			if ( cloak == null )
				return;

			cloak.Delete();
			from.PlaceInBackpack( new ShieldOfRecognition() );

			/* *smiles* Surely thy deeds will be recognized by those who see thee
			 * wearing this shield! It shall serve as a reminder of the exalted
			 * path that thou hast journeyed upon, and I wish to thank thee on
			 * behalf of all whom thine efforts shall benefit. Ah, let us not
			 * forget that old cloak I gavest thee - I shall take it back now and
			 * give thee thine reward. */
			from.SendGump( new GenericQuestGump( 1075783 ) );

			if ( from is PlayerMobile )
				( (PlayerMobile) from ).HumilityQuestStatus = HumilityQuestStatus.RewardAccepted;
		}

		public static void OnCanceled( Mobile from )
		{
			/* *smiles* I understandeth thy feelings, friend. Ye shall remain
			 * anonymous then, to all those who shall benefit from thine efforts.
			 * 
			 * Yet, through all these trials, perhaps thou hast come a little
			 * closer to understanding the true nature of Humility.
			 * Thine efforts might seem small compared to the great world in
			 * which we live, but as more of our people work together, stronger
			 * shall our people be.
			 * 
			 * I wish for ye to keep the cloak I gave thee earlier. Thou canst
			 * do with it what thou will, but I hope that it shall serve as a
			 * reminder of the days ye spent engaged in our simple cause.
			 * And although I have nothing more for thee, I wouldest exhort ye
			 * to continue upon this path, always seeking opportunities to
			 * humble thyself to the demands of the world.
			 * There is a small island to the south upon which lies the Shrine
			 * of Humility. Seek solace there, and perhaps the answers to thine
			 * questions will become clear. */
			from.SendGump( new GenericQuestGump( 1075784 ) );

			if ( from is PlayerMobile )
				( (PlayerMobile) from ).HumilityQuestStatus = HumilityQuestStatus.RewardRefused;
		}
	}
}