using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	public class GlacialStaff : BlackStaff
	{
		public override int LabelNumber { get { return 1017413; } } // Glacial Staff

		private int m_PhysicalResist;

		[Constructable]
		public GlacialStaff()
		{
			Hue = 0x480;

			WeaponAttributes.HitHarm = (short) Utility.RandomList( 5, 10, 15, 20 );
			WeaponAttributes.MageWeapon = (short) Utility.RandomMinMax( 5, 10 );

			m_PhysicalResist = Utility.RandomMinMax( 50, 100 ); // TODO: Verify
		}

		public GlacialStaff( Serial serial )
			: base( serial )
		{
		}

		public override void GetDamageTypes( Mobile wielder, out int phys, out int fire, out int cold, out int pois, out int nrgy )
		{
			fire = pois = nrgy = 0;

			phys = m_PhysicalResist;

			cold = 100 - phys;
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version

			writer.Write( m_PhysicalResist );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			if ( version < 1 )
				m_PhysicalResist = Utility.RandomMinMax( 50, 100 ); // TODO: Verify

			switch ( version )
			{
				case 1:
					{
						m_PhysicalResist = reader.ReadInt();

						goto case 0;
					}
				case 0:
					{
						break;
					}
			}
		}
	}
}