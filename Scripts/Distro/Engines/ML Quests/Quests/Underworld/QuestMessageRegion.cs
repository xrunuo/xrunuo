using System;
using System.Xml;
using Server;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Regions
{
	public abstract class QuestMessageRegion<T> : BaseRegion where T : BaseQuest
	{
		public abstract int Message { get; }

		public QuestMessageRegion( XmlElement xml, Map map, Region parent )
			: base( xml, map, parent )
		{
		}

		public override void OnEnter( Mobile m )
		{
			if ( m is PlayerMobile )
			{
				PlayerMobile pm = m as PlayerMobile;

				if ( QuestHelper.HasQuest<T>( pm ) )
					pm.SendLocalizedMessage( Message );
			}
		}
	}
}