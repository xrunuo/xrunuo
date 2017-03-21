using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Regions;
using Server.Gumps;
using Server.Network;

namespace Server.Items
{
	public class SilverSaplingSeed : Item
	{
		public override int LabelNumber { get { return 1113053; } } // a seed of the Silver Sapling

		public SilverSaplingSeed()
			: base( 0xDCF )
		{
			Weight = 1.0;
			Movable = false;
			Stackable = true;
			Hue = 0x3DF;
		}

		public SilverSaplingSeed( Serial serial )
			: base( serial )
		{
		}

		private static Dictionary<Mobile, Point3D> m_ResLocations = new Dictionary<Mobile, Point3D>();

		public static bool CanBeResurrected( Mobile from )
		{
			return from.Map == Map.TerMur && from.Region.IsPartOf( typeof( DungeonRegion ) ) && m_ResLocations.ContainsKey( from );
		}

		public static void OfferResurrection( Mobile from )
		{
			from.SendGump( new SilverSaplingResGump( from ) );
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( from.Map == Map.TerMur && from.Region.IsPartOf( typeof( DungeonRegion ) ) )
			{
				/* The seed disappears into the earth and for a brief moment you see
				 * a vision of a small sapling growing before you. Should you perish
				 * in your adventures in the Abyss, you shall be restored to this
				 * place with your possessions. */
				from.SendLocalizedMessage( 1113056, "", 0x3C );

				m_ResLocations[from] = from.Location;

				Consume();
			}
			else
			{
				// The seed of the Silver Sapling can only be planted within the Stygian Abyss...
				from.SendLocalizedMessage( 1113055, "", 0x23 );
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

			/*int version =*/
			reader.ReadInt();
		}

		public class SilverSaplingResGump : ResurrectGump
		{
			public SilverSaplingResGump( Mobile m )
				: base( m, ResurrectMessage.SilverSapling )
			{
			}

			public override void OnResponse( NetState state, RelayInfo info )
			{
				Mobile from = state.Mobile;
				from.CloseGump( typeof( ResurrectGump ) );

				if ( info.ButtonID == 2 )
				{
					if ( !m_ResLocations.ContainsKey( from ) )
						return; // sanity

					from.MoveToWorld( m_ResLocations[from], Map.TerMur );

					from.PlaySound( 0x214 );
					from.Resurrect();

					m_ResLocations.Remove( from );

					if ( from.Corpse != null )
						from.Corpse.MoveToWorld( from.Location, from.Map );
				}
			}
		}
	}
}