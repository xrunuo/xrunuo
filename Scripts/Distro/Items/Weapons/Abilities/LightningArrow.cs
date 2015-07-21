using System;
using System.Collections;
using Server;
using Server.Spells;

namespace Server.Items
{
	/// <summary>
	/// A charged arrow that arcs lightning into its target's allies. 
	/// </summary>
	public class LightningArrow : WeaponAbility
	{
		public LightningArrow()
		{
		}

		public override int BaseMana { get { return 20; } }

		public override void OnHit( Mobile attacker, Mobile defender, int damage )
		{
			if ( !Validate( attacker ) || !CheckMana( attacker, true ) )
				return;

			ClearCurrentAbility( attacker );

			Map map = attacker.Map;

			if ( map != null )
			{
				defender.PlaySound( 0x5BF );

				ArrayList targets = new ArrayList();

				foreach ( Mobile m in defender.GetMobilesInRange( 5 ) )
				{
					if ( SpellHelper.ValidIndirectTarget( attacker, m ) && attacker.CanBeHarmful( m, false ) && defender.InLOS( m ) && defender.CanSee( m ) )
						targets.Add( m );
				}

				double dm;

				for ( int i = 0; i < targets.Count; ++i )
				{
					Mobile m = (Mobile) targets[i];

					if ( attacker.CanBeHarmful( m ) && attacker != m )
					{
						attacker.DoHarmful( m );

						Effects.SendBoltEffect( m, false, 0 );

						// TODO: Revisar formula del daño

						dm = Utility.RandomMinMax( 25, 30 );

						SpellHelper.Damage( TimeSpan.Zero, m, attacker, dm, 0, 0, 0, 0, 100 );
					}
				}
			}
		}
	}
}