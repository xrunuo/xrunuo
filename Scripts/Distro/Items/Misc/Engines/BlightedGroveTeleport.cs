using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Engines.MLQuests
{
	public class BlightedGroveTeleport : Teleporter
	{
		[Constructable]
		public BlightedGroveTeleport()
		{
			MapDest = Map.Felucca;
			PointDest = new Point3D( 6478, 861, 11 );
		}

		public BlightedGroveTeleport( Serial serial )
			: base( serial )
		{
		}

		public override bool OnMoveOver( Mobile m )
		{
			Container pack = m.Backpack;
			bool found = false;
			BoneMachete machete = null;

			if ( pack != null )
			{
				foreach ( Item item in pack.Items )
				{
					if ( item is BoneMachete )
					{
						machete = item as BoneMachete;
						found = true;
					}
				}
			}

			if ( found )
			{
				if ( machete != null )
				{
					if ( Utility.RandomBool() )
					{
						m.SendLocalizedMessage( 1075007 ); // Your bone handled machete snaps in half as you force your way through the poisonous undergrowth.
						machete.Delete();
					}
					else
					{
						m.SendLocalizedMessage( 1075008 ); // Your bone handled machete has grown dull but you still manage to force your way past the venomous branches.
					}
				}

				return base.OnMoveOver( m );
			}
			else
			{
				m.SendLocalizedMessage( 1074275 ); // You are unable to push your way through the tangling roots of the mighty tree.
				return true;
			}
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