using System;
using System.Xml;
using Server;
using Server.Accounting;
using Server.Mobiles;
using Server.Gumps;
using Server.Spells;

namespace Server.Regions
{
	public class DungeonRegion : BaseRegion
	{
		private Point3D m_EntranceLocation;
		private Map m_EntranceMap;

		public Point3D EntranceLocation { get { return m_EntranceLocation; } set { m_EntranceLocation = value; } }
		public Map EntranceMap { get { return m_EntranceMap; } set { m_EntranceMap = value; } }

		public DungeonRegion( XmlElement xml, Map map, Region parent )
			: base( xml, map, parent )
		{
			XmlElement entrEl = xml["entrance"];

			Map entrMap = map;
			ReadMap( entrEl, "map", ref entrMap, false );

			if ( ReadPoint3D( entrEl, entrMap, ref m_EntranceLocation, false ) )
				m_EntranceMap = entrMap;
		}

		public override bool AllowHousing( Mobile from, Point3D p )
		{
			return false;
		}

		public override void OnEnter( Mobile m )
		{
			Account acc = m.Account as Account;

			if ( m.Player && ( acc == null || acc.Trial ) )
			{
				m.SendMessage( "Trial account players may not travel into dungeons." );

				m.MoveToWorld( new Point3D( 3651, 2554, 0 ), Map.Trammel );
			}
			else if ( m is PlayerMobile && ( (PlayerMobile) m ).Young )
				m.SendGump( new YoungDungeonWarning() );
		}

		public override void AlterLightLevel( Mobile m, ref int global, ref int personal )
		{
			global = LightCycle.DungeonLevel;
		}

		public override bool CanUseStuckMenu( Mobile m )
		{
			if ( this.Map == Map.Felucca )
				return false;

			return base.CanUseStuckMenu( m );
		}
	}
}