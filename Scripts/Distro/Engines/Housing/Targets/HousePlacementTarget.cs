using Server.Engines.Housing.Items;
using Server.Engines.Housing.Regions;
using Server.Targeting;

namespace Server.Engines.Housing.Targets
{
	public class HousePlacementTarget : MultiTarget
	{
		private HouseDeed m_Deed;

		public HousePlacementTarget( HouseDeed deed )
			: base( deed.MultiID, deed.Offset )
		{
			m_Deed = deed;
		}

		protected override void OnTarget( Mobile from, object o )
		{
			IPoint3D ip = o as IPoint3D;

			if ( ip != null )
			{
				if ( ip is Item )
					ip = ( (Item) ip ).GetWorldTop();

				Point3D p = new Point3D( ip );

				Region reg = Region.Find( new Point3D( p ), from.Map );

				if ( from.AccessLevel >= AccessLevel.GameMaster || reg.AllowHousing( from, p ) )
					m_Deed.OnPlacement( from, p );
				else if ( reg.IsPartOf( typeof( TempNoHousingRegion ) ) )
					from.SendLocalizedMessage( 501270 ); // Lord British has decreed a 'no build' period, thus you cannot build this house at this time.
				else if ( reg.IsPartOf( typeof( TreasureRegion ) ) )
					from.SendLocalizedMessage( 1043287 ); // The house could not be created here.  Either something is blocking the house, or the house would not be on valid terrain.
				else
					from.SendLocalizedMessage( 501265 ); // Housing can not be created in this area.
			}
		}
	}
}