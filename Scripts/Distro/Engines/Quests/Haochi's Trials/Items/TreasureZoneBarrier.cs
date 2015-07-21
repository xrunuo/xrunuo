using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests.SE
{
	public class TreasureZoneBarrier : Item
	{
		[Constructable]
		public TreasureZoneBarrier()
			: base( 0x49E )
		{
			Movable = false;
			Visible = false;
		}

		public override bool OnMoveOver( Mobile m )
		{
			PlayerMobile pm = m as PlayerMobile;

			if ( pm == null )
				return false;

			QuestSystem qs = pm.Quest;

			if ( qs != null && qs is HaochisTrialsQuest )
			{
				if ( qs.IsObjectiveInProgress( typeof( RetrieveKatanaObjective ) ) )
				{
					QuestObjective obj = qs.FindObjective( typeof( RetrieveKatanaObjective ) );

					if ( obj != null )
						obj.Complete();

					qs.AddConversation( new SpotSwordConversation() );

					m.AddToBackpack( new DaimyoHaochisKatana() );

					qs.AddObjective( new GiveSwordDaimyoObjective() );
				}

				if ( qs.IsObjectiveInProgress( typeof( GiveSwordDaimyoObjective ) ) )
				{
					List<Item> list = m.Backpack.Items;

					DaimyoHaochisKatana katana = null;

					for ( int i = 0; i < list.Count; i++ )
					{
						if ( list[i] is DaimyoHaochisKatana )
						{
							katana = list[i] as DaimyoHaochisKatana;

							break;
						}
					}

					if ( katana == null )
					{
						qs.AddConversation( new SpotSwordConversation() );

						m.AddToBackpack( new DaimyoHaochisKatana() );
					}
				}
			}

			return true;
		}

		public TreasureZoneBarrier( Serial serial )
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
