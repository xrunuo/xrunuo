//
//  X-RunUO - Ultima Online Server Emulator
//  Copyright (C) 2015 Pedro Pardal
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//

using System;
using Server;
using Server.Network;

namespace Server
{
	public abstract class BaseHairInfo
	{
		private int m_ItemID;
		private int m_Hue;

		[CommandProperty( AccessLevel.GameMaster )]
		public int ItemID { get { return m_ItemID; } set { m_ItemID = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int Hue { get { return m_Hue; } set { m_Hue = value; } }

		public BaseHairInfo( int itemid )
			: this( itemid, 0 )
		{
		}

		public BaseHairInfo( int itemid, int hue )
		{
			m_ItemID = itemid;
			m_Hue = hue;
		}

		public BaseHairInfo( GenericReader reader )
		{
			int version = reader.ReadInt();

			switch ( version )
			{
				case 0:
					{
						m_ItemID = reader.ReadInt();
						m_Hue = reader.ReadInt();
						break;
					}
			}
		}

		public virtual void Serialize( GenericWriter writer )
		{
			writer.Write( (int) 0 ); //version
			writer.Write( (int) m_ItemID );
			writer.Write( (int) m_Hue );
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