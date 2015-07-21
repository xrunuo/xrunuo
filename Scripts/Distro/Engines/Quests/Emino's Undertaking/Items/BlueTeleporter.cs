using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests.SE
{
	public class BlueTeleporter : SETeleporter
	{
		[Constructable]
		public BlueTeleporter()
		{
			Hue = 0x2;
		}

		public override bool GetDestination( PlayerMobile player, ref Point3D loc, ref Map map )
		{
			QuestSystem qs = player.Quest;

			if ( qs is EminosUndertakingQuest )
			{
				QuestObjective obj = qs.FindObjective( typeof( TakeBlueTeleporterObjective ) );

				if ( obj != null )
				{
					if ( X == 423 && Y == 805 && Z == -1 )
					{
						loc = new Point3D( 411, 1116, 0 );
					}

					if ( X == 411 && Y == 1117 && Z == 0 )
					{
						loc = new Point3D( 424, 807, 0 );
					}

					map = Map.Malas;
					return true;
				}
			}

			return false;
		}

		public BlueTeleporter( Serial serial )
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
