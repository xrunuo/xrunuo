using System;
using Server;
using Server.Network;
using Server.Targeting;

namespace Server.Spells.Bard
{
	public abstract class TargetedSpellsong : Spellsong
	{
		private Mobile m_Target;
		private int m_LeftRounds;

		public Mobile Target { get { return m_Target; } }

		public TargetedSpellsong( Mobile caster, Item scroll, SpellInfo info )
			: base( caster, scroll, info )
		{
		}

		public override void OnCast()
		{
			Spellsong song = GetActiveSpellsong( Caster, this.GetType() );

			if ( song != null )
			{
				// You halt your spellsong.
				song.InterruptSong( 1115774 );
			}
			else
			{
				Caster.Target = new InternalTarget( this );
			}
		}

		private class InternalTarget : Target
		{
			private TargetedSpellsong m_Owner;

			public InternalTarget( TargetedSpellsong owner )
				: base( 12, false, TargetFlags.Harmful )
			{
				m_Owner = owner;
			}

			protected override void OnTarget( Mobile from, object o )
			{
				if ( o is Mobile )
					m_Owner.Target_Callback( (Mobile) o );
			}

			protected override void OnTargetFinish( Mobile from )
			{
				m_Owner.FinishSequence();
			}
		}

		private void Target_Callback( Mobile target )
		{
			if ( UnderEffect( target, this.GetType() ) )
			{
				Caster.SendLocalizedMessage( 1115772 ); // Your target is already under the effect of this spellsong.
				FinishSequence();
				return;
			}

			m_Target = target;

			OnTarget( target );
		}

		protected virtual void OnTarget( Mobile target )
		{
		}

		protected override void OnSongStarted()
		{
			AddTarget( m_Target );

			int music, peace, provo, disco;
			GetSkillBonus( out music, out peace, out provo, out disco );

			m_LeftRounds = 5 + ( music + disco ) + ( peace + provo ) / 2;
		}

		protected override void OnUpkeep()
		{
			Effects.SendTargetParticles( m_Target, 0x3779, 1, 32, 0x13BA, EffectLayer.Head );

			m_LeftRounds--;

			if ( m_LeftRounds == 0 )
				InterruptSong();
			else if ( !m_Target.Alive )
				InterruptSong( 1115773 ); // Your target is dead.
			else if ( !m_Target.InRange( Caster, 8 ) )
				InterruptSong( 1115771 ); // Your target is no longer in range of your spellsong.
		}

		protected override void OnSongInterrupted()
		{
			RemoveTarget( m_Target );
		}
	}
}