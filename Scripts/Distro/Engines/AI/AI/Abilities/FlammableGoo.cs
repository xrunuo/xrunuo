using System;
using Server.Network;

namespace Server.Mobiles
{
	public class FlammableGoo : SpecialAbility
	{
		public override void OnGaveMeleeAttack( Mobile attacker, Mobile defender )
		{
			Effects.PlaySound( attacker.Location, attacker.Map, 0x349 );
			Effects.SendPacket( attacker.Location, attacker.Map, new LocationEffect( attacker.Location, 0x3709, 14, 14, 0x5DE, 0 ) );

			defender.SendLocalizedMessage( 1112365 ); // Flammable goo sprays into the air

			Timer.DelayCall( TimeSpan.FromSeconds( Utility.RandomMinMax( 2, 3 ) ), () =>
			{
				for ( var i = -1; i <= 1; i++ )
				{
					for ( var j = -1; j <= 1; j++ )
					{
						var loc = new Point3D( defender.X + i, defender.Y + j, defender.Z );

						Effects.SendPacket( attacker.Location, attacker.Map, new LocationEffect( loc, 0x36BD, 14, 14, 0x5DE, 5 ) );
					}
				}

				AOS.Damage( defender, Utility.RandomMinMax( 60, 80 ), 0, 100, 0, 0, 0 );

				defender.SendLocalizedMessage( 1112366 ); // The flammable goo covering you bursts into flame!
			} );
		}
	}
}
