using System;

namespace Server.Items
{
	public enum DoorFacing
	{
		WestCW,
		EastCCW,
		WestCCW,
		EastCW,
		SouthCW,
		NorthCCW,
		SouthCCW,
		NorthCW,
		//Sliding Doors
		SouthSW,
		SouthSE,
		WestSS,
		WestSN
	}

	public class IronGateShort : BaseDoor
	{
		[Constructable]
		public IronGateShort( DoorFacing facing )
			: base( 0x84c + ( 2 * (int) facing ), 0x84d + ( 2 * (int) facing ), 0xEC, 0xF3, BaseDoor.GetOffset( facing ) )
		{
		}

		public IronGateShort( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer ) // Default Serialize method
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader ) // Default Deserialize method
		{
			base.Deserialize( reader );

			/*int version =*/
			reader.ReadInt();
		}
	}

	public class IronGate : BaseDoor
	{
		[Constructable]
		public IronGate( DoorFacing facing )
			: base( 0x824 + ( 2 * (int) facing ), 0x825 + ( 2 * (int) facing ), 0xEC, 0xF3, BaseDoor.GetOffset( facing ) )
		{
		}

		public IronGate( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer ) // Default Serialize method
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader ) // Default Deserialize method
		{
			base.Deserialize( reader );

			/*int version =*/
			reader.ReadInt();
		}
	}

	public class LightWoodGate : BaseDoor
	{
		[Constructable]
		public LightWoodGate( DoorFacing facing )
			: base( 0x839 + ( 2 * (int) facing ), 0x83A + ( 2 * (int) facing ), 0xEB, 0xF2, BaseDoor.GetOffset( facing ) )
		{
		}

		public LightWoodGate( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer ) // Default Serialize method
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader ) // Default Deserialize method
		{
			base.Deserialize( reader );

			/*int version =*/
			reader.ReadInt();
		}
	}

	public class DarkWoodGate : BaseDoor
	{
		[Constructable]
		public DarkWoodGate( DoorFacing facing )
			: base( 0x866 + ( 2 * (int) facing ), 0x867 + ( 2 * (int) facing ), 0xEB, 0xF2, BaseDoor.GetOffset( facing ) )
		{
		}

		public DarkWoodGate( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer ) // Default Serialize method
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader ) // Default Deserialize method
		{
			base.Deserialize( reader );

			/*int version =*/
			reader.ReadInt();
		}
	}

	public class MetalDoor : BaseDoor
	{
		[Constructable]
		public MetalDoor( DoorFacing facing )
			: base( 0x675 + ( 2 * (int) facing ), 0x676 + ( 2 * (int) facing ), 0xEC, 0xF3, BaseDoor.GetOffset( facing ) )
		{
		}

		public MetalDoor( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer ) // Default Serialize method
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader ) // Default Deserialize method
		{
			base.Deserialize( reader );

			/*int version =*/
			reader.ReadInt();
		}
	}

	public class BarredMetalDoor : BaseDoor
	{
		[Constructable]
		public BarredMetalDoor( DoorFacing facing )
			: base( 0x685 + ( 2 * (int) facing ), 0x686 + ( 2 * (int) facing ), 0xEC, 0xF3, BaseDoor.GetOffset( facing ) )
		{
		}

		public BarredMetalDoor( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer ) // Default Serialize method
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader ) // Default Deserialize method
		{
			base.Deserialize( reader );

			/*int version =*/
			reader.ReadInt();
		}
	}

	public class BarredMetalDoor2 : BaseDoor
	{
		[Constructable]
		public BarredMetalDoor2( DoorFacing facing )
			: base( 0x1FED + ( 2 * (int) facing ), 0x1FEE + ( 2 * (int) facing ), 0xEC, 0xF3, BaseDoor.GetOffset( facing ) )
		{
		}

		public BarredMetalDoor2( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer ) // Default Serialize method
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader ) // Default Deserialize method
		{
			base.Deserialize( reader );

			/*int version =*/
			reader.ReadInt();
		}
	}

	public class RattanDoor : BaseDoor
	{
		[Constructable]
		public RattanDoor( DoorFacing facing )
			: base( 0x695 + ( 2 * (int) facing ), 0x696 + ( 2 * (int) facing ), 0xEB, 0xF2, BaseDoor.GetOffset( facing ) )
		{
		}

		public RattanDoor( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer ) // Default Serialize method
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader ) // Default Deserialize method
		{
			base.Deserialize( reader );

			/*int version =*/
			reader.ReadInt();
		}
	}

	public class DarkWoodDoor : BaseDoor
	{
		[Constructable]
		public DarkWoodDoor( DoorFacing facing )
			: base( 0x6A5 + ( 2 * (int) facing ), 0x6A6 + ( 2 * (int) facing ), 0xEA, 0xF1, BaseDoor.GetOffset( facing ) )
		{
		}

		public DarkWoodDoor( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer ) // Default Serialize method
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader ) // Default Deserialize method
		{
			base.Deserialize( reader );

			/*int version =*/
			reader.ReadInt();
		}
	}

	public class MediumWoodDoor : BaseDoor
	{
		[Constructable]
		public MediumWoodDoor( DoorFacing facing )
			: base( 0x6B5 + ( 2 * (int) facing ), 0x6B6 + ( 2 * (int) facing ), 0xEA, 0xF1, BaseDoor.GetOffset( facing ) )
		{
		}

		public MediumWoodDoor( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer ) // Default Serialize method
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader ) // Default Deserialize method
		{
			base.Deserialize( reader );

			/*int version =*/
			reader.ReadInt();
		}
	}

	public class MetalDoor2 : BaseDoor
	{
		[Constructable]
		public MetalDoor2( DoorFacing facing )
			: base( 0x6C5 + ( 2 * (int) facing ), 0x6C6 + ( 2 * (int) facing ), 0xEC, 0xF3, BaseDoor.GetOffset( facing ) )
		{
		}

		public MetalDoor2( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer ) // Default Serialize method
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader ) // Default Deserialize method
		{
			base.Deserialize( reader );

			/*int version =*/
			reader.ReadInt();
		}
	}

	public class LightWoodDoor : BaseDoor
	{
		[Constructable]
		public LightWoodDoor( DoorFacing facing )
			: base( 0x6D5 + ( 2 * (int) facing ), 0x6D6 + ( 2 * (int) facing ), 0xEA, 0xF1, BaseDoor.GetOffset( facing ) )
		{
		}

		public LightWoodDoor( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer ) // Default Serialize method
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader ) // Default Deserialize method
		{
			base.Deserialize( reader );

			/*int version =*/
			reader.ReadInt();
		}
	}

	public class StrongWoodDoor : BaseDoor
	{
		[Constructable]
		public StrongWoodDoor( DoorFacing facing )
			: base( 0x6E5 + ( 2 * (int) facing ), 0x6E6 + ( 2 * (int) facing ), 0xEA, 0xF1, BaseDoor.GetOffset( facing ) )
		{
		}

		public StrongWoodDoor( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer ) // Default Serialize method
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader ) // Default Deserialize method
		{
			base.Deserialize( reader );

			/*int version =*/
			reader.ReadInt();
		}
	}

	public class GargishTraditionalDoor : BaseDoor
	{
		public static int GetIdOffset( DoorFacing facing )
		{
			switch ( facing )
			{
				case DoorFacing.WestCCW: return 0;
				case DoorFacing.EastCW: return 2;
				case DoorFacing.SouthCCW: return 4;
				case DoorFacing.NorthCW: return 6;
			}

			return 0;
		}

		[Constructable]
		public GargishTraditionalDoor( DoorFacing facing )
			: base( 0x409B + GetIdOffset( facing ), 0x409C + GetIdOffset( facing ), 0xEA, 0xF1, BaseDoor.GetOffset( facing ) )
		{
		}

		public GargishTraditionalDoor( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer ) // Default Serialize method
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader ) // Default Deserialize method
		{
			base.Deserialize( reader );

			/*int version =*/
			reader.ReadInt();
		}
	}

	public class GargishProgressiveDoor : BaseDoor
	{
		public static int GetIdOffset( DoorFacing facing )
		{
			switch ( facing )
			{
				case DoorFacing.WestCCW: return 0;
				case DoorFacing.EastCW: return 2;
				case DoorFacing.SouthCCW: return 4;
				case DoorFacing.NorthCW: return 6;
			}

			return 0;
		}

		[Constructable]
		public GargishProgressiveDoor( DoorFacing facing )
			: base( 0x41CF + GetIdOffset( facing ), 0x41D0 + GetIdOffset( facing ), 0xEA, 0xF1, BaseDoor.GetOffset( facing ) )
		{
		}

		public GargishProgressiveDoor( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer ) // Default Serialize method
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader ) // Default Deserialize method
		{
			base.Deserialize( reader );

			/*int version =*/
			reader.ReadInt();
		}
	}

	public class LargeGargishQueenDoor : BaseDoor
	{
		public static int GetIdOffset( DoorFacing facing )
		{
			switch ( facing )
			{
				case DoorFacing.WestCCW: return 0;
				case DoorFacing.EastCW: return 2;
				case DoorFacing.SouthCCW: return 4;
				case DoorFacing.NorthCW: return 6;
			}

			return 0;
		}

		[Constructable]
		public LargeGargishQueenDoor( DoorFacing facing )
			: base( 0x4D1A + GetIdOffset( facing ), 0x4D1B + GetIdOffset( facing ), -1, -1, BaseDoor.GetOffset( facing ) )
		{
		}

		public LargeGargishQueenDoor( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer ) // Default Serialize method
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader ) // Default Deserialize method
		{
			base.Deserialize( reader );

			/*int version =*/
			reader.ReadInt();
		}
	}

	public class GargishBentasDoor : BaseDoor
	{
		public static int GetIdOffset( DoorFacing facing )
		{
			switch ( facing )
			{

				case DoorFacing.WestCCW: return 0;
				case DoorFacing.EastCW: return 2;
				case DoorFacing.SouthCCW: return 4;
				case DoorFacing.NorthCW: return 6;
			}

			return 0;
		}

		[Constructable]
		public GargishBentasDoor( DoorFacing facing )
			: base( 0x50D0 + GetIdOffset( facing ), 0x50D1 + GetIdOffset( facing ), 0xEA, 0xF1, BaseDoor.GetOffset( facing ) )
		{
		}

		public GargishBentasDoor( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer ) // Default Serialize method
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader ) // Default Deserialize method
		{
			base.Deserialize( reader );

			/*int version =*/
			reader.ReadInt();
		}
	}

	public class GargishPrisonDoor : BaseDoor
	{
		public static int GetIdOffset( DoorFacing facing )
		{
			switch ( facing )
			{
				case DoorFacing.WestCCW: return 0;
				case DoorFacing.EastCW: return 2;
				case DoorFacing.SouthCCW: return 4;
				case DoorFacing.NorthCW: return 6;
			}

			return 0;
		}

		[Constructable]
		public GargishPrisonDoor( DoorFacing facing )
			: base( 0x5142 + GetIdOffset( facing ), 0x5143 + GetIdOffset( facing ), 0xF0, 0xEF, BaseDoor.GetOffset( facing ) )
		{
		}

		public GargishPrisonDoor( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer ) // Default Serialize method
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader ) // Default Deserialize method
		{
			base.Deserialize( reader );

			/*int version =*/
			reader.ReadInt();
		}
	}
}