using System;
using System.Collections;
using Server.Mobiles;
using Server.Network;
using Server.Items;
using Server.Targeting;

namespace Server.Spells.Spellweaving
{
	public class NaturesFurySpell : SpellweavingSpell
	{
		private static SpellInfo m_Info = new SpellInfo( "Natures Fury", "Rauvvrae", -1, 9002 );

		public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds( 1.25 ); } }

		public override double RequiredSkill { get { return 0.0; } }
		public override int RequiredMana { get { return 24; } }

		public override bool CheckCast()
		{
			if ( !base.CheckCast() )
			{
				return false;
			}

			if ( ( Caster.Followers + 1 ) > Caster.FollowersMax )
			{
				Caster.SendLocalizedMessage( 1049645 ); // You have too many followers to summon that creature.
				return false;
			}

			return true;
		}

		public NaturesFurySpell( Mobile caster, Item scroll )
			: base( caster, scroll, m_Info )
		{
		}

		public override void OnCast()
		{
			Caster.Target = new InternalTarget( this );
		}

		public void Target( IPoint3D p )
		{
			Map map = Caster.Map;

			SpellHelper.GetSurfaceTop( ref p );

			if ( map == null || !map.CanSpawnMobile( p.X, p.Y, p.Z ) )
			{
				Caster.SendLocalizedMessage( 501942 ); // That location is blocked.
			}
			else if ( SpellHelper.CheckTown( p, Caster ) && CheckSequence() )
			{
				NaturesFury fury = new NaturesFury();

				BaseCreature.Summon( fury, false, Caster, new Point3D( p ), 0x212, TimeSpan.FromSeconds( 25 + ( Caster.Skills[SkillName.Spellweaving].Fixed / 240 ) + ( GetFocusLevel( Caster ) * 2 ) ) );

				fury.FixedParticles( 0x91C, 10, 0xFF, 9539, EffectLayer.Waist );
			}

			FinishSequence();
		}

		private class InternalTarget : Target
		{
			private NaturesFurySpell m_Owner;

			public InternalTarget( NaturesFurySpell owner )
				: base( 12, true, TargetFlags.None )
			{
				m_Owner = owner;
			}

			protected override void OnTarget( Mobile from, object o )
			{
				if ( o is IPoint3D )
				{
					m_Owner.Target( (IPoint3D) o );
				}
			}

			protected override void OnTargetOutOfLOS( Mobile from, object o )
			{
				from.SendLocalizedMessage( 501943 ); // Target cannot be seen. Try again.
				from.Target = new InternalTarget( m_Owner );
				from.Target.BeginTimeout( from, TimeoutTime - DateTime.Now );
				m_Owner = null;
			}

			protected override void OnTargetFinish( Mobile from )
			{
				if ( m_Owner != null )
				{
					m_Owner.FinishSequence();
				}
			}
		}
	}
}