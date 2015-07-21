using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests.SE
{
	public class WhiteTeleporter : SETeleporter
	{
		[Constructable]
		public WhiteTeleporter()
		{
			Hue = 0x47E;
		}

		public override bool GetDestination( PlayerMobile player, ref Point3D loc, ref Map map )
		{
			QuestSystem qs = player.Quest;

			if ( qs is EminosUndertakingQuest )
			{
				QuestObjective obj = qs.FindObjective( typeof( TakeWhiteTeleporterObjective ) );

				if ( obj != null )
				{
					if ( X == 392 && Y == 802 && Z == -1 )
					{
						loc = new Point3D( 411, 1085, 0 );

						obj.Complete();

						QuestObjective obj1 = qs.FindObjective( typeof( WalkThroughHallwayObjective ) );

						if ( obj1 == null )
						{
							qs.AddConversation( new NarrowsConversation() );

							qs.AddObjective( new WalkThroughHallwayObjective() );
						}
					}

					if ( X == 412 && Y == 1086 && Z == 0 )
					{
						loc = new Point3D( 391, 803, 0 );
					}

					map = Map.Malas;
					return true;
				}
			}

			return false;
		}

		public WhiteTeleporter( Serial serial )
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
