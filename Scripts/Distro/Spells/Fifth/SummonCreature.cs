using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.Spells.Fifth
{
	public class SummonCreatureSpell : MagerySpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Summon Creature", "Kal Xen",
				266,
				9040,
				Reagent.Bloodmoss,
				Reagent.MandrakeRoot,
				Reagent.SpidersSilk
			);

		public override SpellCircle Circle { get { return SpellCircle.Fifth; } }

		public SummonCreatureSpell( Mobile caster, Item scroll )
			: base( caster, scroll, m_Info )
		{
		}

		private static Type[] m_Types = new Type[]
			{
				typeof( Scorpion ),
				typeof( GiantSerpent ),
				typeof( GreyWolf ),
				typeof( Chicken ),
				typeof( Hind ),
				typeof( Alligator ),
				typeof( SnowLeopard ),
				typeof( PolarBear ),
				typeof( Walrus ),
				typeof( GrizzlyBear ),
				typeof( BlackBear ),
				typeof( Llama ),
				typeof( Slime ),
				typeof( Pig ),
				typeof( Horse ),
				typeof( Eagle ),
				typeof( Gorilla )
			};

		public override bool CheckCast()
		{
			if ( !base.CheckCast() )
				return false;

			if ( SpellHelper.IsTown( Caster.Location, Caster ) )
			{
				Caster.SendLocalizedMessage( 502745 ); // You cannot summon in this area.
				return false;
			}
			else if ( ( Caster.Followers + 2 ) > Caster.FollowersMax )
			{
				Caster.SendLocalizedMessage( 1049645 ); // You have too many followers to summon that creature.
				return false;
			}

			return true;
		}

		public override void OnCast()
		{
			if ( CheckSequence() )
			{
				try
				{
					BaseCreature creature = (BaseCreature) Activator.CreateInstance( m_Types[Utility.Random( m_Types.Length )] );

					creature.ControlSlots = 2;

					TimeSpan duration = TimeSpan.FromSeconds( ( 2 * Caster.Skills.Magery.Fixed ) / 5 );

					SpellHelper.Summon( creature, Caster, 0x215, duration, false, false );
				}
				catch
				{
				}
			}

			FinishSequence();
		}

		public override TimeSpan GetCastDelay()
		{
			return TimeSpan.FromTicks( base.GetCastDelay().Ticks * 5 );
		}
	}
}