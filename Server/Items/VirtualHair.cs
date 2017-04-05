using System;
using Server;
using Server.Network;

namespace Server
{
	public abstract class BaseHairInfo
	{
		[CommandProperty( AccessLevel.GameMaster )]
		public int ItemID { get; set; }

		[CommandProperty( AccessLevel.GameMaster )]
		public int Hue { get; set; }

		public BaseHairInfo( int itemid )
			: this( itemid, 0 )
		{
		}

		public BaseHairInfo( int itemid, int hue )
		{
			ItemID = itemid;
			Hue = hue;
		}

		public BaseHairInfo( GenericReader reader )
		{
			int version = reader.ReadInt();

			switch ( version )
			{
				case 0:
					{
						ItemID = reader.ReadInt();
						Hue = reader.ReadInt();
						break;
					}
			}
		}

		public virtual void Serialize( GenericWriter writer )
		{
			writer.Write( (int) 0 ); //version
			writer.Write( (int) ItemID );
			writer.Write( (int) Hue );
		}
	}

	public class HairInfo : BaseHairInfo
	{
		public HairInfo( int itemid )
			: base( itemid, 0 )
		{
		}

		public HairInfo( int itemid, int hue )
			: base( itemid, hue )
		{
		}

		public HairInfo( GenericReader reader )
			: base( reader )
		{
		}

		public static int FakeSerial( Mobile parent )
		{
			return ( 0x7FFFFFFF - 0x400 - ( parent.Serial * 4 ) );
		}
	}

	public class FacialHairInfo : BaseHairInfo
	{
		public FacialHairInfo( int itemid )
			: base( itemid, 0 )
		{
		}

		public FacialHairInfo( int itemid, int hue )
			: base( itemid, hue )
		{
		}

		public FacialHairInfo( GenericReader reader )
			: base( reader )
		{
		}

		public static int FakeSerial( Mobile parent )
		{
			return ( 0x7FFFFFFF - 0x400 - 1 - ( parent.Serial * 4 ) );
		}
	}

	public sealed class HairEquipUpdate : Packet
	{
		public HairEquipUpdate( Mobile parent )
			: base( 0x2E, 15 )
		{
			int hue = parent.HairHue;

			if ( parent.SolidHueOverride >= 0 )
				hue = parent.SolidHueOverride;

			int hairSerial = HairInfo.FakeSerial( parent );

			m_Stream.Write( (int) hairSerial );
			m_Stream.Write( (short) parent.HairItemID );
			m_Stream.Write( (byte) 0 );
			m_Stream.Write( (byte) Layer.Hair );
			m_Stream.Write( (int) parent.Serial );
			m_Stream.Write( (short) hue );
		}
	}

	public sealed class FacialHairEquipUpdate : Packet
	{
		public FacialHairEquipUpdate( Mobile parent )
			: base( 0x2E, 15 )
		{
			int hue = parent.FacialHairHue;

			if ( parent.SolidHueOverride >= 0 )
				hue = parent.SolidHueOverride;

			int hairSerial = FacialHairInfo.FakeSerial( parent );

			m_Stream.Write( (int) hairSerial );
			m_Stream.Write( (short) parent.FacialHairItemID );
			m_Stream.Write( (byte) 0 );
			m_Stream.Write( (byte) Layer.FacialHair );
			m_Stream.Write( (int) parent.Serial );
			m_Stream.Write( (short) hue );
		}
	}

	public sealed class RemoveHair : Packet
	{
		public RemoveHair( Mobile parent )
			: base( 0x1D, 5 )
		{
			m_Stream.Write( (int) HairInfo.FakeSerial( parent ) );
		}
	}

	public sealed class RemoveFacialHair : Packet
	{
		public RemoveFacialHair( Mobile parent )
			: base( 0x1D, 5 )
		{
			m_Stream.Write( (int) FacialHairInfo.FakeSerial( parent ) );
		}
	}
}