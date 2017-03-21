using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;

namespace Server.Engines.Collections
{
	public class CollectionTransferPetGump : Gump
	{
		public override int TypeID { get { return 0x25B; } }

		private CollectionController m_Collection;
		private BaseCreature m_Pet;
		private Timer m_ExpireTimer;

		public CollectionTransferPetGump( CollectionController collection, BaseCreature pet )
			: base( 50, 50 )
		{
			m_Collection = collection;
			m_Pet = pet;

			AddBackground( 0, 0, 270, 120, 0x13BE );
			AddHtmlLocalized( 10, 10, 250, 75, 1073105, true, false ); // <div align=center>Are you sure you wish to transfer this pet away, with no possibility of recovery?</div>
			AddHtmlLocalized( 55, 90, 75, 20, 1011011, false, false ); // CONTINUE
			AddButton( 20, 90, 0xFA5, 0xFA7, 2, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 170, 90, 75, 20, 1011012, false, false ); // CANCEL
			AddButton( 135, 90, 0xFA5, 0xFA7, 1, GumpButtonType.Reply, 0 );

			m_ExpireTimer = Timer.DelayCall( TimeSpan.FromSeconds( 30.0 ), new TimerStateCallback( Expire_Callback ), pet.ControlMaster );
		}

		private static void Expire_Callback( object state )
		{
			if ( state is Mobile )
			{
				Mobile from = (Mobile) state;

				if ( from.HasGump( typeof( CollectionTransferPetGump ) ) )
				{
					from.CloseGump( typeof( CollectionTransferPetGump ) );
					from.SendLocalizedMessage( 1073114 ); // You decide to not transfer this follower.
				}
			}
		}

		public override void OnResponse( NetState sender, RelayInfo info )
		{
			if ( m_ExpireTimer != null )
				m_ExpireTimer.Stop();

			if ( info.ButtonID == 2 )
			{
				double award = 0.0;

				if ( m_Pet.ControlMaster != sender.Mobile )
					return;

				if ( !m_Collection.CheckType( m_Pet, out award ) )
				{
					sender.Mobile.SendLocalizedMessage( 1073113 ); // This Collection is not accepting that type of creature.
					return;
				}

				/*if ( ( m_Collection.Points + award ) > m_Collection.PointsPerTier && m_Collection.Tier >= m_Collection.MaxTiers )
				{
					sender.Mobile.SendLocalizedMessage( 1072815 ); // This Collection is too full to accept this item right now.
					return;
				}*/

				m_Pet.Delete();
				m_Collection.Award( sender.Mobile, (int) award );
				sender.Mobile.SendGump( new CollectionDonateGump( m_Collection, sender.Mobile ) );
			}
			else
			{
				sender.Mobile.SendLocalizedMessage( 1073114 ); // You decide to not transfer this follower.
			}
		}
	}
}