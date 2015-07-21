using System;
using Server;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Items
{
	public class TwistedWealdTeleport : Teleporter
	{
		[Constructable]
		public TwistedWealdTeleport()
		{
			MapDest = Map.Ilshenar;
			PointDest = new Point3D( 2189, 1253, 0 );
		}

		public TwistedWealdTeleport( Serial serial )
			: base( serial )
		{
		}

		public override bool OnMoveOver( Mobile m )
		{
			if ( m is PlayerMobile )
			{
				PlayerMobile player = (PlayerMobile) m;

				if ( QuestHelper.HasQuest<DreadhornQuest>( player ) )
					return base.OnMoveOver( m );

				player.SendLocalizedMessage( 1074274 ); // You dance in the fairy ring, but nothing happens.
			}

			return true;
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