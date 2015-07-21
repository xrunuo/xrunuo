using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests.SE
{
	public class GreenTeleporter : SETeleporter
	{
		[Constructable]
		public GreenTeleporter()
		{
			Hue = 0x17E;
		}

		public override bool GetDestination( PlayerMobile player, ref Point3D loc, ref Map map )
		{
			QuestSystem qs = player.Quest;

			if ( qs is EminosUndertakingQuest )
			{
				QuestObjective obj = qs.FindObjective( typeof( TakeGreenTeleporterObjective ) );

				if ( obj != null )
				{
					if ( X == 412 && Y == 1123 && Z == 0 )
					{
						loc = new Point3D( 417, 806, 0 );
					}

					if ( X == 418 && Y == 804 && Z == -1 )
					{
						loc = new Point3D( 410, 1125, 0 );
					}

					map = Map.Malas;
					return true;
				}
			}

			return false;
		}

		public GreenTeleporter( Serial serial )
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
