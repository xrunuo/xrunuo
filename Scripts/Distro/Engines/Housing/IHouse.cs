using System.Collections;
using Server.Engines.Housing.Items;
using Server.Engines.Housing.Multis;
using Server.Multis;

namespace Server.Engines.Housing
{
	public interface IHouse : IEntity
	{
		bool IsOwner( Mobile m );
		bool IsCoOwner( Mobile m );
		bool IsSecure( Item item );
		bool IsLockedDown( Item item );
		bool CheckAosStorage( int need );
		SecureAccessResult CheckSecureAccess( Mobile m, Item item );
		bool Public { get; set; }
		Point3D BanLocation { get; set; }
		DecayType DecayType { get; }
		ArrayList PlayerVendors { get; }
		ArrayList Addons { get; }
		ArrayList Doors { get; }
		ArrayList VendorRentalContracts { get; }
		int Visits { get; set; }
		MovingCrate MovingCrate { get; set; }
		ArrayList PlayerBarkeepers { get; }
		Mobile Owner { get; }
		ArrayList VendorInventories { get; }
		ArrayList InternalizedVendors { get; }
		ArrayList Bans { get; }
		bool Deleted { get; }
		HouseSign Sign { get; }
		ArrayList Secures { get; }
		bool IsFriend( Mobile m );
		bool IsBanned( Mobile m );
		bool HasAccess( Mobile m );
		bool HasSecureAccess( Mobile m, SecureLevel level );
		bool LockDown( Mobile m, Item item );
		bool LockDown( Mobile m, Item item, bool checkIsInside );
		bool CanPlaceNewBarkeep();
		bool CanPlaceNewVendor();
		bool IsInside( Mobile m );
		bool IsInside( Item item );
		bool RefreshDecay();
		void DropToMovingCrate( Item item );
		new Map Map { get; }
	}
}
