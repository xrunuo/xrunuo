using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests.SE
{
	public class FacingBarrier : Item
	{
		[Constructable]
		public FacingBarrier()
			: base( 0x49E )
		{
			Movable = false;
			Visible = false;
		}

		public override bool OnMoveOver( Mobile m )
		{
			PlayerMobile pm = m as PlayerMobile;

			if ( pm == null )
			{
				return false;
			}

			QuestSystem qs = pm.Quest;

			if ( qs != null && qs is EminosUndertakingQuest )
			{
				EminosUndertakingQuest euq = qs as EminosUndertakingQuest;

				if ( qs.IsObjectiveInProgress( typeof( EnterTheCaveObjective ) ) )
				{
					QuestObjective obj = qs.FindObjective( typeof( EnterTheCaveObjective ) );

					if ( obj != null )
					{
						obj.Complete();
					}

					qs.AddConversation( new StandsConversation() );

					qs.AddObjective( new UseNinjaTrainingsObjective() );
				}

				if ( qs.IsObjectiveInProgress( typeof( UseNinjaTrainingsObjective ) ) && !euq.AlreadySendWarning && euq.CanSendWarning )
				{
					qs.AddConversation( new StandsWarningConversation() );

					euq.AlreadySendWarning = true;
				}
			}

			if ( m.Hidden )
			{
				return true;
			}

			return false;
		}

		public FacingBarrier( Serial serial )
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
