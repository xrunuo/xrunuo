using System;
using Server.Network;
using Server.Items;
using Server.Targeting;
using Server.Mobiles;

namespace Server.Spells.Spellweaving
{
	public class WordOfDeathSpell : SpellweavingSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Word of Death", "Nyraxle",
				-1
			);

		public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds( 3.5 ); } }


		public override double RequiredSkill { get { return 80.0; } }
		public override int RequiredMana { get { return 50; } }

		public WordOfDeathSpell( Mobile caster, Item scroll )
			: base( caster, scroll, m_Info )
		{
		}

		public override void OnCast()
		{
			Caster.Target = new InternalTarget( this );
		}

		public void Target( Mobile m )
		{
			if ( !Caster.CanSee( m ) )
			{
				Caster.SendLocalizedMessage( 500237 ); // Target can not be seen.
			}
			else if ( CheckHSequence( m ) )
			{
				SpellHelper.Turn( Caster, m );

				// Sacamos el lvl del focus
				int focuslevel = GetFocusLevel( Caster );

				int umbral = m.HitsMax * focuslevel * 5 / 100;

				int damage = 0;

				bool pvp = m.Player && Caster.Player;

				if ( pvp || m.Hits > umbral || m is SlasherOfVeils )
					damage = Utility.RandomMinMax( 25, 35 );
				else
					damage = Utility.RandomMinMax( 480, 520 );

				// Spellweaving Bonus
				damage = AOS.Scale( damage, (int) Caster.Skills[SkillName.Spellweaving].Value );

				int damageBonus = 0;

				// Intelligence Bonus
				damageBonus += Caster.Int / 10;

				// Spell Damage Increase Bonus
				damageBonus += SpellHelper.GetSpellDamage( Caster, pvp );

				damage = AOS.Scale( damage, 100 + damageBonus );

				// Chaos Damage
				switch ( Utility.RandomMinMax( 1, 5 ) )
				{
					case 1: SpellHelper.Damage( this, m, damage, 100, 0, 0, 0, 0 ); break;
					case 2: SpellHelper.Damage( this, m, damage, 0, 100, 0, 0, 0 ); break;
					case 3: SpellHelper.Damage( this, m, damage, 0, 0, 100, 0, 0 ); break;
					case 4: SpellHelper.Damage( this, m, damage, 0, 0, 0, 100, 0 ); break;
					case 5: SpellHelper.Damage( this, m, damage, 0, 0, 0, 0, 100 ); break;
				}

				m.PlaySound( 0x211 );
				Effects.SendLocationParticles( new DummyEntity( Serial.Zero, new Point3D( m.X, m.Y, m.Z + 10 ), m.Map ), 0x3779, 1, 30, 0x3, 0x3F, 0x26EC, 0x100 );
				Effects.SendMovingParticles( new DummyEntity( Serial.Zero, new Point3D( m.X, m.Y, m.Z + 50 ), m.Map ), m, 0xF5F, 1, 0, false, false, 0x21, 0x3F, 0x251D, 0x1, 0x0, EffectLayer.Head, 0x100 );
			}

			FinishSequence();
		}

		private class InternalTarget : Target
		{
			private WordOfDeathSpell m_Owner;

			public InternalTarget( WordOfDeathSpell owner )
				: base( 12, false, TargetFlags.Harmful )
			{
				m_Owner = owner;
			}

			protected override void OnTarget( Mobile from, object o )
			{
				if ( o is Mobile )
					m_Owner.Target( (Mobile) o );
			}

			protected override void OnTargetFinish( Mobile from )
			{
				m_Owner.FinishSequence();
			}
		}
	}
}