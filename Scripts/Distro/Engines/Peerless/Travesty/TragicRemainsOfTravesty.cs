using System;
using Server;

namespace Server.Items
{
	public class TragicRemainsOfTravesty : MonsterStatuette
	{
		public override int LabelNumber { get { return 1074500; } } // Tragic Remains of the Travesty

		[Constructable]
		public TragicRemainsOfTravesty()
			: base( Utility.Random( 0x122A, 6 ) )
		{
			Weight = 1.0;
			Hue = Utility.RandomList( 0x11E, 0x846 );
		}

		public TragicRemainsOfTravesty( Serial serial )
			: base( serial )
		{
		}

		private static int[] m_Sounds = new int[]
		{
			0x314, 0x315, 0x316, 0x317  // TODO check
		};

		public override void OnMovement( Mobile m, Point3D oldLocation )
		{
			if ( TurnedOn && IsLockedDown && ( !m.Hidden || m.AccessLevel == AccessLevel.Player ) && this.InRange( m, 2 ) && !this.InRange( oldLocation, 2 ) )
				Effects.PlaySound( Location, Map, m_Sounds[Utility.Random( m_Sounds.Length )] );

			base.OnMovement( m, oldLocation );
		}

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

