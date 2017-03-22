using System;
using System.Xml;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Events;

namespace Server.Regions
{
	public class TwistedWealdDesert : MondainRegion
	{
		public static void Initialize()
		{
			EventSink.Login += new LoginEventHandler( Desert_OnLogin );
		}

		private static void Desert_OnLogin( LoginEventArgs e )
		{
			Mobile m = e.Mobile;

			if ( m.Region.IsPartOf( typeof( TwistedWealdDesert ) ) && m.AccessLevel < AccessLevel.GameMaster )
				m.ForcedWalk = true;
		}

		public TwistedWealdDesert( XmlElement xml, Map map, Region parent )
			: base( xml, map, parent )
		{
		}

		public override void OnEnter( Mobile m )
		{
			if ( m.Player )
				m.ForcedWalk = true;
		}

		public override void OnExit( Mobile m )
		{
			if ( m.Player )
				m.ForcedWalk = false;
		}
	}
}