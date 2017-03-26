using System;
using Server.Mobiles;
using Server.Targeting;
using Server.Items;
using Server.Engines.Quests;

namespace Server.Engines.MLQuests
{
	public class ParoxysmusLairTeleport : Teleporter
	{
		[Constructable]
		public ParoxysmusLairTeleport()
		{
			MapDest = Map.Felucca;
			PointDest = new Point3D( 6227, 334, 60 );
		}

		public ParoxysmusLairTeleport( Serial serial )
			: base( serial )
		{
		}

		public override bool OnMoveOver( Mobile m )
		{
			Container pack = m.Backpack;
			bool found = false;
			TransientItem rope = null;

			if ( pack != null )
			{
				foreach ( Item item in pack.Items )
				{
					if ( item is MagicalRope )
					{
						rope = item as TransientItem;
						found = true;
					}

					else if ( item is AcidProofRope )
					{
						rope = item as TransientItem;
						found = true;
					}
				}
			}

			if ( found )
			{
				if ( rope != null )
				{
					if ( rope is MagicalRope )
					{
						m.SendLocalizedMessage( 1075097 ); // Your rope is severely damaged by the acidic Core.  You're lucky to have made it safely to the ground.
						rope.Delete();
					}
					else
					{
						if ( Utility.RandomBool() )
						{
							m.SendLocalizedMessage( 1075098 ); // Your rope has been weakened by the acidic Core.
							if ( rope.LifeSpan != null && rope.LifeSpan.TotalSeconds == 0 )
								rope.LifeSpan = TimeSpan.FromSeconds( 600 );
						}
						else
						{
							m.SendLocalizedMessage( 1075097 ); // Your rope is severely damaged by the acidic Core.  You're lucky to have made it safely to the ground.
							rope.Delete();
						}
					}
				}

				return base.OnMoveOver( m );
			}
			else
			{
				m.SendLocalizedMessage( 1074272 ); // You have no way to lower yourself safely into the enormous sinkhole.
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