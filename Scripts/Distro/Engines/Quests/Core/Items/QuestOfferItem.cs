using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using Server.Network;

namespace Server.Engines.Quests.SE
{
	public class QuestOfferItem : Item
	{
		[Constructable]
		public QuestOfferItem()
			: base( 0x49E )
		{
			Movable = false;
			Visible = false;
		}

		public override bool OnMoveOver( Mobile m )
		{
			PlayerMobile pm = m as PlayerMobile;

			if ( pm != null && pm.Profession == 6 && pm.AccessLevel == AccessLevel.Player )
			{
				QuestSystem qs = pm.Quest;

				if ( qs == null )
				{
					QuestSystem newQuest = new HaochisTrialsQuest( pm );

					bool inRestartPeriod = false;

					if ( QuestSystem.CanOfferQuest( pm, typeof( HaochisTrialsQuest ), out inRestartPeriod ) )
					{
						newQuest.SendOffer();
					}
				}
			}
			else if ( pm != null && pm.Profession == 7 && pm.AccessLevel == AccessLevel.Player )
			{
				QuestSystem qs = pm.Quest;

				if ( qs == null )
				{
					QuestSystem newQuest = new EminosUndertakingQuest( pm );

					bool inRestartPeriod = false;

					if ( QuestSystem.CanOfferQuest( pm, typeof( EminosUndertakingQuest ), out inRestartPeriod ) )
					{
						newQuest.SendOffer();
					}
				}
			}

			return base.OnMoveOver( m );
		}

		public QuestOfferItem( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}
}
