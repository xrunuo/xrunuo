using Server.Engines.Housing.Multis;
using Server.Misc;
using Server.Regions;

namespace Server.Engines.Housing.Regions
{
	public class TempNoHousingRegion : BaseRegion
	{
		private Mobile m_RegionOwner;

		public TempNoHousingRegion( BaseHouse house, Mobile regionowner )
			: base( null, house.Map, Region.DefaultPriority, house.Region.Area )
		{
			Register();

			m_RegionOwner = regionowner;

			Timer.DelayCall( house.RestrictedPlacingTime, Unregister );
		}

		public override bool AllowHousing( Mobile from, Point3D p )
		{
			return ( from == m_RegionOwner || AccountHandler.CheckAccount( from, m_RegionOwner ) );
		}
	}
}
