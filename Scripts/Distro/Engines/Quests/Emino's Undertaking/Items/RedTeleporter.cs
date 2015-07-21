using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests.SE
{
	public class RedTeleporter : SETeleporter
	{
		[Constructable]
		public RedTeleporter()
		{
			Hue = 0x21;
		}

		public override bool GetDestination( PlayerMobile player, ref Point3D loc, ref Map map )
		{
			QuestSystem qs = player.Quest;

			if ( qs is EminosUndertakingQuest )
			{
				QuestObjective obj = qs.FindObjective( typeof( TakeBlueTeleporterObjective ) );

				if ( obj != null )
				{
					loc = new Point3D( 391, 803, 0 );
					map = Map.Malas;
					return true;
				}
			}

			return false;
		}

		public RedTeleporter( Serial serial )
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
