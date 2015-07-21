using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests.SE
{
	public class YellowPathBarrier : Item
	{
		[Constructable]
		public YellowPathBarrier()
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
				QuestObjective obj = qs.FindObjective( typeof( FollowYellowPathObjective ) );

				if ( obj != null )
				{
					return true;
				}
			}

			m.SendLocalizedMessage( 1063163 ); // You may not enter this area unless directed to do so by Daimyo Haochi.

			return false;
		}

		public YellowPathBarrier( Serial serial )
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
