using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Engines.Collections
{
	public abstract class BaseLibraryRepresentative : Mobile
	{
		private BritainLibraryCollection m_Controller;

		[CommandProperty( AccessLevel.Developer )]
		public BritainLibraryCollection Controller
		{
			get { return m_Controller; }
			set { m_Controller = value; }
		}

		public BaseLibraryRepresentative()
		{
			Blessed = true;
			Hits = HitsMax;
			Direction = (Direction) Utility.RandomMinMax( 0x0, 0x7 );
		}

		public BaseLibraryRepresentative( Serial serial )
			: base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( !from.InRange( this.Location, 2 ) )
			{
				from.LocalOverheadMessage( MessageType.Regular, 0x3B2, 1019045 ); // I can't reach that.
				return;
			}

			if ( m_Controller != null )
			{
				from.CloseGump<CollectionDonateGump>();
				from.CloseGump<CollectionRewardGump>();
				from.CloseGump<CollectionConfirmReward>();
				from.CloseGump<CollectionSelectHueGump>();

				from.SendGump( new CollectionDonateGump( m_Controller, from ) );
			}
			else
			{
				base.OnDoubleClick( from );
			}
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			if ( m_Controller != null )
			{
				list.Add( 1072819, m_Controller.Tier.ToString() ); // Current Tier: ~1_TIER~
				list.Add( 1072820, m_Controller.Points.ToString() ); // Current Points: ~1_POINTS~
				list.Add( 1072821, m_Controller.PointsUntilNextTier.ToString() ); // Points until next tier: ~1_POINTS~
				list.Add( m_Controller.Section );
			}
			else
			{
				list.Add( "[No CollectionControler selected]" );
			}

		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.Write( m_Controller );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();

			m_Controller = reader.ReadItem() as BritainLibraryCollection;
		}
	}
}