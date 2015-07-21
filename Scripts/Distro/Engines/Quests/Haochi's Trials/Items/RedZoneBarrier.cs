using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests.SE
{
	public class RedZoneBarrier : Item
	{
		[Constructable]
		public RedZoneBarrier()
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

			if ( qs != null && qs is HaochisTrialsQuest )
			{
				if ( qs.IsObjectiveInProgress( typeof( FollowRedPathObjective ) ) )
				{
					QuestObjective obj = qs.FindObjective( typeof( FollowRedPathObjective ) );

					if ( obj != null )
					{
						obj.Complete();
					}

					qs.AddConversation( new ApproachGypsyConversation() );

					qs.AddObjective( new GiveGypsyGoldOrHuntCatsObjective() );
				}
			}

			return true;
		}

		public RedZoneBarrier( Serial serial )
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
