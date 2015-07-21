using Server.Engines.Housing.Items;
using Server.Engines.Housing.Multis;

namespace Server.Engines.Housing.Deeds.Items
{
	[TypeAlias( "Server.Multis.Deeds.StonePlasterHouseDeed" )]
	public class StonePlasterHouseDeed : HouseDeed
	{
		[Constructable]
		public StonePlasterHouseDeed()
			: base( 0x64, new Point3D( 0, 4, 0 ) )
		{
		}

		public StonePlasterHouseDeed( Serial serial )
			: base( serial )
		{
		}

		public override BaseHouse GetHouse( Mobile owner )
		{
			return new SmallOldHouse( owner, 0x64 );
		}

		public override int LabelNumber { get { return 1041211; } }
		public override Rectangle2D[] Area { get { return SmallOldHouse.AreaArray; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}

	[TypeAlias( "Server.Multis.Deeds.FieldStoneHouseDeed" )]
	public class FieldStoneHouseDeed : HouseDeed
	{
		[Constructable]
		public FieldStoneHouseDeed()
			: base( 0x66, new Point3D( 0, 4, 0 ) )
		{
		}

		public FieldStoneHouseDeed( Serial serial )
			: base( serial )
		{
		}

		public override BaseHouse GetHouse( Mobile owner )
		{
			return new SmallOldHouse( owner, 0x66 );
		}

		public override int LabelNumber { get { return 1041212; } }
		public override Rectangle2D[] Area { get { return SmallOldHouse.AreaArray; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}

	[TypeAlias( "Server.Multis.Deeds.SmallBrickHouseDeed" )]
	public class SmallBrickHouseDeed : HouseDeed
	{
		[Constructable]
		public SmallBrickHouseDeed()
			: base( 0x68, new Point3D( 0, 4, 0 ) )
		{
		}

		public SmallBrickHouseDeed( Serial serial )
			: base( serial )
		{
		}

		public override BaseHouse GetHouse( Mobile owner )
		{
			return new SmallOldHouse( owner, 0x68 );
		}

		public override int LabelNumber { get { return 1041213; } }
		public override Rectangle2D[] Area { get { return SmallOldHouse.AreaArray; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}

	[TypeAlias( "Server.Multis.Deeds.WoodHouseDeed" )]
	public class WoodHouseDeed : HouseDeed
	{
		[Constructable]
		public WoodHouseDeed()
			: base( 0x6A, new Point3D( 0, 4, 0 ) )
		{
		}

		public WoodHouseDeed( Serial serial )
			: base( serial )
		{
		}

		public override BaseHouse GetHouse( Mobile owner )
		{
			return new SmallOldHouse( owner, 0x6A );
		}

		public override int LabelNumber { get { return 1041214; } }
		public override Rectangle2D[] Area { get { return SmallOldHouse.AreaArray; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}

	[TypeAlias( "Server.Multis.Deeds.WoodPlasterHouseDeed" )]
	public class WoodPlasterHouseDeed : HouseDeed
	{
		[Constructable]
		public WoodPlasterHouseDeed()
			: base( 0x6C, new Point3D( 0, 4, 0 ) )
		{
		}

		public WoodPlasterHouseDeed( Serial serial )
			: base( serial )
		{
		}

		public override BaseHouse GetHouse( Mobile owner )
		{
			return new SmallOldHouse( owner, 0x6C );
		}

		public override int LabelNumber { get { return 1041215; } }
		public override Rectangle2D[] Area { get { return SmallOldHouse.AreaArray; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}

	[TypeAlias( "Server.Multis.Deeds.ThatchedRoofCottageDeed" )]
	public class ThatchedRoofCottageDeed : HouseDeed
	{
		[Constructable]
		public ThatchedRoofCottageDeed()
			: base( 0x6E, new Point3D( 0, 4, 0 ) )
		{
		}

		public ThatchedRoofCottageDeed( Serial serial )
			: base( serial )
		{
		}

		public override BaseHouse GetHouse( Mobile owner )
		{
			return new SmallOldHouse( owner, 0x6E );
		}

		public override int LabelNumber { get { return 1041216; } }
		public override Rectangle2D[] Area { get { return SmallOldHouse.AreaArray; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}

	[TypeAlias( "Server.Multis.Deeds.BrickHouseDeed" )]
	public class BrickHouseDeed : HouseDeed
	{
		[Constructable]
		public BrickHouseDeed()
			: base( 0x74, new Point3D( -1, 7, 0 ) )
		{
		}

		public BrickHouseDeed( Serial serial )
			: base( serial )
		{
		}

		public override BaseHouse GetHouse( Mobile owner )
		{
			return new GuildHouse( owner );
		}

		public override int LabelNumber { get { return 1041219; } }
		public override Rectangle2D[] Area { get { return GuildHouse.AreaArray; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}

	[TypeAlias( "Server.Multis.Deeds.TwoStoryWoodPlasterHouseDeed" )]
	public class TwoStoryWoodPlasterHouseDeed : HouseDeed
	{
		[Constructable]
		public TwoStoryWoodPlasterHouseDeed()
			: base( 0x76, new Point3D( -3, 7, 0 ) )
		{
		}

		public TwoStoryWoodPlasterHouseDeed( Serial serial )
			: base( serial )
		{
		}

		public override BaseHouse GetHouse( Mobile owner )
		{
			return new TwoStoryHouse( owner, 0x76 );
		}

		public override int LabelNumber { get { return 1041220; } }
		public override Rectangle2D[] Area { get { return TwoStoryHouse.AreaArray; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}

	[TypeAlias( "Server.Multis.Deeds.TwoStoryStonePlasterHouseDeed" )]
	public class TwoStoryStonePlasterHouseDeed : HouseDeed
	{
		[Constructable]
		public TwoStoryStonePlasterHouseDeed()
			: base( 0x78, new Point3D( -3, 7, 0 ) )
		{
		}

		public TwoStoryStonePlasterHouseDeed( Serial serial )
			: base( serial )
		{
		}

		public override BaseHouse GetHouse( Mobile owner )
		{
			return new TwoStoryHouse( owner, 0x78 );
		}

		public override int LabelNumber { get { return 1041221; } }
		public override Rectangle2D[] Area { get { return TwoStoryHouse.AreaArray; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}

	[TypeAlias( "Server.Multis.Deeds.TowerDeed" )]
	public class TowerDeed : HouseDeed
	{
		[Constructable]
		public TowerDeed()
			: base( 0x7A, new Point3D( 0, 7, 0 ) )
		{
		}

		public TowerDeed( Serial serial )
			: base( serial )
		{
		}

		public override BaseHouse GetHouse( Mobile owner )
		{
			return new Tower( owner );
		}

		public override int LabelNumber { get { return 1041222; } }
		public override Rectangle2D[] Area { get { return Tower.AreaArray; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}

	[TypeAlias( "Server.Multis.Deeds.KeepDeed" )]
	public class KeepDeed : HouseDeed
	{
		[Constructable]
		public KeepDeed()
			: base( 0x7C, new Point3D( 0, 11, 0 ) )
		{
		}

		public KeepDeed( Serial serial )
			: base( serial )
		{
		}

		public override BaseHouse GetHouse( Mobile owner )
		{
			return new Keep( owner );
		}

		public override int LabelNumber { get { return 1041223; } }
		public override Rectangle2D[] Area { get { return Keep.AreaArray; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}

	[TypeAlias( "Server.Multis.Deeds.CastleDeed" )]
	public class CastleDeed : HouseDeed
	{
		[Constructable]
		public CastleDeed()
			: base( 0x7E, new Point3D( 0, 16, 0 ) )
		{
		}

		public CastleDeed( Serial serial )
			: base( serial )
		{
		}

		public override BaseHouse GetHouse( Mobile owner )
		{
			return new Castle( owner );
		}

		public override int LabelNumber { get { return 1041224; } }
		public override Rectangle2D[] Area { get { return Castle.AreaArray; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}

	[TypeAlias( "Server.Multis.Deeds.LargePatioDeed" )]
	public class LargePatioDeed : HouseDeed
	{
		[Constructable]
		public LargePatioDeed()
			: base( 0x8C, new Point3D( -4, 7, 0 ) )
		{
		}

		public LargePatioDeed( Serial serial )
			: base( serial )
		{
		}

		public override BaseHouse GetHouse( Mobile owner )
		{
			return new LargePatioHouse( owner );
		}

		public override int LabelNumber { get { return 1041231; } }
		public override Rectangle2D[] Area { get { return LargePatioHouse.AreaArray; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}

	[TypeAlias( "Server.Multis.Deeds.LargeMarbleDeed" )]
	public class LargeMarbleDeed : HouseDeed
	{
		[Constructable]
		public LargeMarbleDeed()
			: base( 0x96, new Point3D( -4, 7, 0 ) )
		{
		}

		public LargeMarbleDeed( Serial serial )
			: base( serial )
		{
		}

		public override BaseHouse GetHouse( Mobile owner )
		{
			return new LargeMarbleHouse( owner );
		}

		public override int LabelNumber { get { return 1041236; } }
		public override Rectangle2D[] Area { get { return LargeMarbleHouse.AreaArray; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}

	[TypeAlias( "Server.Multis.Deeds.SmallTowerDeed" )]
	public class SmallTowerDeed : HouseDeed
	{
		[Constructable]
		public SmallTowerDeed()
			: base( 0x98, new Point3D( 3, 4, 0 ) )
		{
		}

		public SmallTowerDeed( Serial serial )
			: base( serial )
		{
		}

		public override BaseHouse GetHouse( Mobile owner )
		{
			return new SmallTower( owner );
		}

		public override int LabelNumber { get { return 1041237; } }
		public override Rectangle2D[] Area { get { return SmallTower.AreaArray; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}

	[TypeAlias( "Server.Multis.Deeds.LogCabinDeed" )]
	public class LogCabinDeed : HouseDeed
	{
		[Constructable]
		public LogCabinDeed()
			: base( 0x9A, new Point3D( 1, 6, 0 ) )
		{
		}

		public LogCabinDeed( Serial serial )
			: base( serial )
		{
		}

		public override BaseHouse GetHouse( Mobile owner )
		{
			return new LogCabin( owner );
		}

		public override int LabelNumber { get { return 1041238; } }
		public override Rectangle2D[] Area { get { return LogCabin.AreaArray; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}

	[TypeAlias( "Server.Multis.Deeds.SandstonePatioDeed" )]
	public class SandstonePatioDeed : HouseDeed
	{
		[Constructable]
		public SandstonePatioDeed()
			: base( 0x9C, new Point3D( -1, 4, 0 ) )
		{
		}

		public SandstonePatioDeed( Serial serial )
			: base( serial )
		{
		}

		public override BaseHouse GetHouse( Mobile owner )
		{
			return new SandStonePatio( owner );
		}

		public override int LabelNumber { get { return 1041239; } }
		public override Rectangle2D[] Area { get { return SandStonePatio.AreaArray; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}

	[TypeAlias( "Server.Multis.Deeds.VillaDeed" )]
	public class VillaDeed : HouseDeed
	{
		[Constructable]
		public VillaDeed()
			: base( 0x9E, new Point3D( 3, 6, 0 ) )
		{
		}

		public VillaDeed( Serial serial )
			: base( serial )
		{
		}

		public override BaseHouse GetHouse( Mobile owner )
		{
			return new TwoStoryVilla( owner );
		}

		public override int LabelNumber { get { return 1041240; } }
		public override Rectangle2D[] Area { get { return TwoStoryVilla.AreaArray; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}

	[TypeAlias( "Server.Multis.Deeds.StoneWorkshopDeed" )]
	public class StoneWorkshopDeed : HouseDeed
	{
		[Constructable]
		public StoneWorkshopDeed()
			: base( 0xA0, new Point3D( -1, 4, 0 ) )
		{
		}

		public StoneWorkshopDeed( Serial serial )
			: base( serial )
		{
		}

		public override BaseHouse GetHouse( Mobile owner )
		{
			return new SmallShop( owner, 0xA0 );
		}

		public override int LabelNumber { get { return 1041241; } }
		public override Rectangle2D[] Area { get { return SmallShop.AreaArray2; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}

	[TypeAlias( "Server.Multis.Deeds.MarbleWorkshopDeed" )]
	public class MarbleWorkshopDeed : HouseDeed
	{
		[Constructable]
		public MarbleWorkshopDeed()
			: base( 0xA2, new Point3D( -1, 4, 0 ) )
		{
		}

		public MarbleWorkshopDeed( Serial serial )
			: base( serial )
		{
		}

		public override BaseHouse GetHouse( Mobile owner )
		{
			return new SmallShop( owner, 0xA2 );
		}

		public override int LabelNumber { get { return 1041242; } }
		public override Rectangle2D[] Area { get { return SmallShop.AreaArray1; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}
}