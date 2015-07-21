using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
	public class ThreeFeetOfSteelQuest : BaseQuest
	{
		/* Three Feet of Steel */
		public override object Title { get { return 1077636; } }

		/* Pick up the Worn Katana and talk to the Keeper to complete
		 * the quest.<br><center>------</center><br>So they send ya ta
		 * me to get ya outfitted. Well then, ya need to get a weapon.
		 * I would recommend the katana, as there's nothing better than
		 * three feet of steel to make anything think twice 'bout
		 * attackin' ya.<br><br>Tell ya what. Get me that katana.
		 * It's over there on the table. I need to look at it to be
		 * sure it's fit to skewer the unnaturals up ahead. */
		public override object Description { get { return 1077637; } }

		/* Well then, ya ain't gettin' through that gate until you
		 * bring me that katana. */
		public override object Refuse { get { return 1077638; } }

		/* Try that there katana. The long, metal one with the sharp
		 * edge on it. */
		public override object Uncomplete { get { return 1077641; } }

		/* Here ya go. It looks plenty strong enough ta me.<br><br>
		 * Oh and best work on keeping yer eyes open. I slipped it
		 * back into yer bag when you wasn't lookin' Haha! */
		public override object Complete { get { return 1077640; } }

		public ThreeFeetOfSteelQuest()
		{
			AddObjective( new ObtainObjective( typeof( WornKatana ), "Worn Katana", 1 ) );

			AddReward( new BaseReward( typeof( WornKatana ), 1077958 ) );
		}

		public override void OnAccept()
		{
			base.OnAccept();

			Owner.CheckKRStartingQuestStep( 10 );
		}

		public override void GiveRewards()
		{
			base.GiveRewards();

			Owner.CheckKRStartingQuestStep( 12 );
		}
	}
}