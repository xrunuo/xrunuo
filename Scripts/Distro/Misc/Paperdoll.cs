using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Network;
using Server.Multis;
using Server.Mobiles;
using Server.Events;

namespace Server.Misc
{
	public class Paperdoll
	{
		public static void Initialize()
		{
			EventSink.Instance.PaperdollRequest += new PaperdollRequestEventHandler( EventSink_PaperdollRequest );
		}

		public static void EventSink_PaperdollRequest( PaperdollRequestEventArgs e )
		{
			Mobile beholder = e.Beholder;
			Mobile beheld = e.Beheld;

			beholder.Send( new DisplayPaperdoll( beheld, Titles.ComputeTitle( beholder, beheld ), beheld.AllowEquipFrom( beholder ) ) );

			if ( ObjectPropertyListPacket.Enabled )
			{
				foreach ( var item in beheld.GetEquippedItems() )
					beholder.Send( item.OPLPacket );

				// NOTE: OSI sends MobileUpdate when opening your own paperdoll.
				// It has a very bad rubber-banding affect. What positive affects does it have?
			}
		}
	}
}