using System;
using System.Xml;
using Server;
using Server.Mobiles;
using Server.Regions;
using Server.Spells;
using Server.Spells.Seventh;
using Server.Spells.Fourth;
using Server.Spells.Sixth;
using Server.Spells.Chivalry;

namespace Server.Regions
{
	public class MondainRegion : BaseRegion
	{
		public MondainRegion( XmlElement xml, Map map, Region parent )
			: base( xml, map, parent )
		{
		}

		public override bool AllowHousing( Mobile from, Point3D p )
		{
			return false;
		}

		public override bool OnBeginSpellCast( Mobile m, ISpell s )
		{
			if ( ( s is GateTravelSpell || s is RecallSpell || s is MarkSpell || s is SacredJourneySpell ) && m.AccessLevel == AccessLevel.Player )
			{
				m.SendLocalizedMessage( 501802 ); // Thy spell doth not appear to work...

				return false;
			}

			return base.OnBeginSpellCast( m, s );
		}
	}
}