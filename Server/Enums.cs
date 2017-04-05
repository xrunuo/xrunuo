using System;

namespace Server
{
	[CustomEnum( new string[] { "North", "Right", "East", "Down", "South", "Left", "West", "Up" } )]
	[Flags]
	public enum Direction : byte
	{
		North = 0x0,
		Right = 0x1,
		East = 0x2,
		Down = 0x3,
		South = 0x4,
		Left = 0x5,
		West = 0x6,
		Up = 0x7,

		Mask = 0x7,
		Running = 0x80,
		ValueMask = 0x87
	}

	/// <summary>
	/// Enumeration of item layer values.
	/// </summary>
	public enum Layer : byte
	{
		/// <summary>
		/// Invalid layer.
		/// </summary>
		Invalid = 0x00,
		/// <summary>
		/// First valid layer. Equivalent to <c>Layer.OneHanded</c>.
		/// </summary>
		FirstValid = 0x01,
		/// <summary>
		/// One handed weapon.
		/// </summary>
		OneHanded = 0x01,
		/// <summary>
		/// Two handed weapon or shield.
		/// </summary>
		TwoHanded = 0x02,
		/// <summary>
		/// Shoes.
		/// </summary>
		Shoes = 0x03,
		/// <summary>
		/// Pants.
		/// </summary>
		Pants = 0x04,
		/// <summary>
		/// Shirts.
		/// </summary>
		Shirt = 0x05,
		/// <summary>
		/// Helmets, hats, and masks.
		/// </summary>
		Helm = 0x06,
		/// <summary>
		/// Gloves.
		/// </summary>
		Gloves = 0x07,
		/// <summary>
		/// Rings.
		/// </summary>
		Ring = 0x08,
		/// <summary>
		/// Talismans.
		/// </summary>
		Talisman = 0x09,
		/// <summary>
		/// Gorgets and necklaces.
		/// </summary>
		Neck = 0x0A,
		/// <summary>
		/// Hair.
		/// </summary>
		Hair = 0x0B,
		/// <summary>
		/// Half aprons.
		/// </summary>
		Waist = 0x0C,
		/// <summary>
		/// Torso, inner layer.
		/// </summary>
		InnerTorso = 0x0D,
		/// <summary>
		/// Bracelets.
		/// </summary>
		Bracelet = 0x0E,
		/// <summary>
		/// Unused.
		/// </summary>
		Unused_xF = 0x0F,
		/// <summary>
		/// Beards and mustaches.
		/// </summary>
		FacialHair = 0x10,
		/// <summary>
		/// Torso, outer layer.
		/// </summary>
		MiddleTorso = 0x11,
		/// <summary>
		/// Earings.
		/// </summary>
		Earrings = 0x12,
		/// <summary>
		/// Arms and sleeves.
		/// </summary>
		Arms = 0x13,
		/// <summary>
		/// Cloaks.
		/// </summary>
		Cloak = 0x14,
		/// <summary>
		/// Backpacks.
		/// </summary>
		Backpack = 0x15,
		/// <summary>
		/// Torso, outer layer.
		/// </summary>
		OuterTorso = 0x16,
		/// <summary>
		/// Leggings, outer layer.
		/// </summary>
		OuterLegs = 0x17,
		/// <summary>
		/// Leggings, inner layer.
		/// </summary>
		InnerLegs = 0x18,
		/// <summary>
		/// Last valid non-internal layer. Equivalent to <c>Layer.InnerLegs</c>.
		/// </summary>
		LastUserValid = 0x18,
		/// <summary>
		/// Mount item layer.
		/// </summary>
		Mount = 0x19,
		/// <summary>
		/// Vendor 'buy pack' layer.
		/// </summary>
		ShopBuy = 0x1A,
		/// <summary>
		/// Vendor 'resale pack' layer.
		/// </summary>
		ShopResale = 0x1B,
		/// <summary>
		/// Vendor 'sell pack' layer.
		/// </summary>
		ShopSell = 0x1C,
		/// <summary>
		/// Bank box layer.
		/// </summary>
		Bank = 0x1D,
		/// <summary>
		/// Last valid layer. Equivalent to <c>Layer.Bank</c>.
		/// </summary>
		LastValid = 0x1D
	}

	/// <summary>
	/// Enumeration containing all possible light types. These are only applicable to light source items, like lanterns, candles, braziers, etc.
	/// </summary>
	public enum LightType
	{
		/// <summary>
		/// Window shape, arched, ray shining east.
		/// </summary>
		ArchedWindowEast,
		/// <summary>
		/// Medium circular shape.
		/// </summary>
		Circle225,
		/// <summary>
		/// Small circular shape.
		/// </summary>
		Circle150,
		/// <summary>
		/// Door shape, shining south.
		/// </summary>
		DoorSouth,
		/// <summary>
		/// Door shape, shining east.
		/// </summary>
		DoorEast,
		/// <summary>
		/// Large semicircular shape (180 degrees), north wall.
		/// </summary>
		NorthBig,
		/// <summary>
		/// Large pie shape (90 degrees), north-east corner.
		/// </summary>
		NorthEastBig,
		/// <summary>
		/// Large semicircular shape (180 degrees), east wall.
		/// </summary>
		EastBig,
		/// <summary>
		/// Large semicircular shape (180 degrees), west wall.
		/// </summary>
		WestBig,
		/// <summary>
		/// Large pie shape (90 degrees), south-west corner.
		/// </summary>
		SouthWestBig,
		/// <summary>
		/// Large semicircular shape (180 degrees), south wall.
		/// </summary>
		SouthBig,
		/// <summary>
		/// Medium semicircular shape (180 degrees), north wall.
		/// </summary>
		NorthSmall,
		/// <summary>
		/// Medium pie shape (90 degrees), north-east corner.
		/// </summary>
		NorthEastSmall,
		/// <summary>
		/// Medium semicircular shape (180 degrees), east wall.
		/// </summary>
		EastSmall,
		/// <summary>
		/// Medium semicircular shape (180 degrees), west wall.
		/// </summary>
		WestSmall,
		/// <summary>
		/// Medium semicircular shape (180 degrees), south wall.
		/// </summary>
		SouthSmall,
		/// <summary>
		/// Shaped like a wall decoration, north wall.
		/// </summary>
		DecorationNorth,
		/// <summary>
		/// Shaped like a wall decoration, north-east corner.
		/// </summary>
		DecorationNorthEast,
		/// <summary>
		/// Small semicircular shape (180 degrees), east wall.
		/// </summary>
		EastTiny,
		/// <summary>
		/// Shaped like a wall decoration, west wall.
		/// </summary>
		DecorationWest,
		/// <summary>
		/// Shaped like a wall decoration, south-west corner.
		/// </summary>
		DecorationSouthWest,
		/// <summary>
		/// Small semicircular shape (180 degrees), south wall.
		/// </summary>
		SouthTiny,
		/// <summary>
		/// Window shape, rectangular, no ray, shining south.
		/// </summary>
		RectWindowSouthNoRay,
		/// <summary>
		/// Window shape, rectangular, no ray, shining east.
		/// </summary>
		RectWindowEastNoRay,
		/// <summary>
		/// Window shape, rectangular, ray shining south.
		/// </summary>
		RectWindowSouth,
		/// <summary>
		/// Window shape, rectangular, ray shining east.
		/// </summary>
		RectWindowEast,
		/// <summary>
		/// Window shape, arched, no ray, shining south.
		/// </summary>
		ArchedWindowSouthNoRay,
		/// <summary>
		/// Window shape, arched, no ray, shining east.
		/// </summary>
		ArchedWindowEastNoRay,
		/// <summary>
		/// Window shape, arched, ray shining south.
		/// </summary>
		ArchedWindowSouth,
		/// <summary>
		/// Large circular shape.
		/// </summary>
		Circle300,
		/// <summary>
		/// Large pie shape (90 degrees), north-west corner.
		/// </summary>
		NorthWestBig,
		/// <summary>
		/// Negative light. Medium pie shape (90 degrees), south-east corner.
		/// </summary>
		DarkSouthEast,
		/// <summary>
		/// Negative light. Medium semicircular shape (180 degrees), south wall.
		/// </summary>
		DarkSouth,
		/// <summary>
		/// Negative light. Medium pie shape (90 degrees), north-west corner.
		/// </summary>
		DarkNorthWest,
		/// <summary>
		/// Negative light. Medium pie shape (90 degrees), south-east corner. Equivalent to <c>LightType.SouthEast</c>.
		/// </summary>
		DarkSouthEast2,
		/// <summary>
		/// Negative light. Medium circular shape (180 degrees), east wall.
		/// </summary>
		DarkEast,
		/// <summary>
		/// Negative light. Large circular shape.
		/// </summary>
		DarkCircle300,
		/// <summary>
		/// Opened door shape, shining south.
		/// </summary>
		DoorOpenSouth,
		/// <summary>
		/// Opened door shape, shining east.
		/// </summary>
		DoorOpenEast,
		/// <summary>
		/// Window shape, square, ray shining east.
		/// </summary>
		SquareWindowEast,
		/// <summary>
		/// Window shape, square, no ray, shining east.
		/// </summary>
		SquareWindowEastNoRay,
		/// <summary>
		/// Window shape, square, ray shining south.
		/// </summary>
		SquareWindowSouth,
		/// <summary>
		/// Window shape, square, no ray, shining south.
		/// </summary>
		SquareWindowSouthNoRay,
		/// <summary>
		/// Empty.
		/// </summary>
		Empty,
		/// <summary>
		/// Window shape, skinny, no ray, shining south.
		/// </summary>
		SkinnyWindowSouthNoRay,
		/// <summary>
		/// Window shape, skinny, ray shining east.
		/// </summary>
		SkinnyWindowEast,
		/// <summary>
		/// Window shape, skinny, no ray, shining east.
		/// </summary>
		SkinnyWindowEastNoRay,
		/// <summary>
		/// Shaped like a hole, shining south.
		/// </summary>
		HoleSouth,
		/// <summary>
		/// Shaped like a hole, shining south.
		/// </summary>
		HoleEast,
		/// <summary>
		/// Large circular shape with a moongate graphic embeded.
		/// </summary>
		Moongate,
		/// <summary>
		/// Unknown usage. Many rows of slightly angled lines.
		/// </summary>
		Strips,
		/// <summary>
		/// Shaped like a small hole, shining south.
		/// </summary>
		SmallHoleSouth,
		/// <summary>
		/// Shaped like a small hole, shining east.
		/// </summary>
		SmallHoleEast,
		/// <summary>
		/// Large semicircular shape (180 degrees), north wall. Identical graphic as <c>LightType.NorthBig</c>, but slightly different positioning.
		/// </summary>
		NorthBig2,
		/// <summary>
		/// Large semicircular shape (180 degrees), west wall. Identical graphic as <c>LightType.WestBig</c>, but slightly different positioning.
		/// </summary>
		WestBig2,
		/// <summary>
		/// Large pie shape (90 degrees), north-west corner. Equivalent to <c>LightType.NorthWestBig</c>.
		/// </summary>
		NorthWestBig2
	}

	public enum AccessLevel
	{
		Player,
		Counselor,
		GameMaster,
		Seer,
		Administrator,
		Developer,
		Owner
	}
}
