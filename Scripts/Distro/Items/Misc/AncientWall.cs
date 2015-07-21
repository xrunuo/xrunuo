using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
	public class AncientWall : Item
	{
		[Constructable]
		public AncientWall()
			: base( 0x175 )
		{
			Movable = false;
		}

		public bool IsInsidePyramid( Mobile from )
		{
			Rectangle2D rect = new Rectangle2D( 1808, 1784, 31, 31 );

			foreach ( object obj in Map.Malas.GetMobilesInBounds( rect ) )
			{
				if ( obj is Mobile )
				{
					Mobile m = obj as Mobile;

					if ( m == from )
					{
						return true;
					}
				}
			}

			return false;
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( IsInsidePyramid( from ) )
			{
				from.SendLocalizedMessage( 1061603 ); // You hear a metallic click, and the ancient stone block rises up into the ceiling.

				InternalTimer timer = new InternalTimer( this );

				timer.Start();

				Z -= 50;
			}
			else
			{
				from.SendLocalizedMessage( 501287 ); // That is locked, but is usable from the inside.
			}
		}

		public AncientWall( Serial serial )
			: base( serial )
		{
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

			if ( Z != -110 )
			{
				Z = -110;
			}
		}

		private class InternalTimer : Timer
		{
			private AncientWall wall;

			public InternalTimer( AncientWall m_wall )
				: base( TimeSpan.FromMinutes( 1.0 ) )
			{
				wall = m_wall;
			}

			protected override void OnTick()
			{
				wall.Z += 50;
			}
		}
	}
}