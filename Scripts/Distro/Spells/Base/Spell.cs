using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Server.Items;
using Server.Network;
using Server.Targeting;
using Server.Mobiles;
using Server.Spells.Second;
using Server.Spells.Necromancy;
using Server.Spells.Chivalry;
using Server.Spells.Bushido;
using Server.Spells.Ninjitsu;
using Server.Spells.Spellweaving;
using Server.Spells.Mysticism;
using Server.Misc;

namespace Server.Spells
{
	public abstract class Spell : ISpell
	{
		private Mobile m_Caster;
		private IEntity m_MacroTarget;
		private Item m_Scroll;
		private SpellInfo m_Info;
		private SpellState m_State;
		private DateTime m_StartCastTime;

		public SpellState State { get { return m_State; } set { m_State = value; } }
		public Mobile Caster { get { return m_Caster; } }
		public IEntity MacroTarget { get { return m_MacroTarget; } set { m_MacroTarget = value; } }
		public SpellInfo Info { get { return m_Info; } }
		public string Name { get { return m_Info.Name; } }
		public string Mantra { get { return m_Info.Mantra; } }
		public Type[] Reagents { get { return m_Info.Reagents; } }
		public Item Scroll { get { return m_Scroll; } }

		private static TimeSpan NextSpellDelay = TimeSpan.FromSeconds( 0.75 );
		private static TimeSpan AnimateDelay = TimeSpan.FromSeconds( 1.5 );

		public virtual SkillName CastSkill { get { return SkillName.Magery; } }
		public virtual SkillName DamageSkill { get { return SkillName.EvalInt; } }

		public virtual bool RevealOnCast { get { return true; } }
		public virtual bool ClearHandsOnCast { get { return m_Caster.IsPlayer; } }
		public virtual bool ShowHandMovement { get { return true; } }

		public virtual bool DelayedDamage { get { return false; } }

		public Spell( Mobile caster, Item scroll, SpellInfo info )
		{
			m_Caster = caster;
			m_Scroll = scroll;
			m_Info = info;
		}

		public virtual int GetNewAosDamage( int bonus, int dice, int sides, Mobile singleTarget )
		{
			if ( singleTarget != null )
				return GetNewAosDamage( bonus, dice, sides, ( Caster.IsPlayer && singleTarget.IsPlayer ), GetDamageScalar( singleTarget ) );
			else
				return GetNewAosDamage( bonus, dice, sides, false );
		}

		public virtual int GetNewAosDamage( int bonus, int dice, int sides, bool playerVsPlayer )
		{
			return GetNewAosDamage( bonus, dice, sides, playerVsPlayer, 1.0 );
		}

		public virtual int GetNewAosDamage( int bonus, int dice, int sides, bool playerVsPlayer, double scalar )
		{
			int damage = Utility.Dice( dice, sides, bonus ) * 100;
			int damageBonus = 0;

			#region Inscription Bonus
			int inscribeSkill = GetInscribeFixed( m_Caster );
			int inscribeBonus = ( inscribeSkill + ( 1000 * ( inscribeSkill / 1000 ) ) ) / 200;
			damageBonus += inscribeBonus;
			#endregion

			#region Intelligence Bonus
			int intBonus = Caster.Int / 10;
			damageBonus += intBonus;
			#endregion

			#region SDI Bonus
			damageBonus += SpellHelper.GetSpellDamage( m_Caster, playerVsPlayer );
			#endregion

			#region EvalInt Bonus
			int evalSkill = GetDamageFixed( m_Caster );
			int evalScale = 30 + ( ( 9 * evalSkill ) / 100 );
			#endregion

			damage = AOS.Scale( damage, 100 + damageBonus );
			damage = AOS.Scale( damage, evalScale );
			damage = AOS.Scale( damage, (int) ( scalar * 100 ) );

			return damage / 100;
		}

		private bool m_Resonates;

		public bool Resonates { get { return m_Resonates; } set { m_Resonates = value; } }

		public virtual bool IsCasting { get { return m_State == SpellState.Casting; } }

		public virtual void OnCasterHurt()
		{
			if ( !Caster.IsPlayer )
				return;

			if ( IsCasting )
			{
				object o = ProtectionSpell.Registry[m_Caster];
				bool disturb = true;

				if ( o != null && o is double )
				{
					if ( ( (double) o ) > Utility.RandomDouble() * 100.0 )
						disturb = false;
				}

				if ( m_Resonates )
				{
					Caster.SendLocalizedMessage( 1113689 ); // Your equipment resonates with the damage you receive preventing you from being interrupted during casting.
					disturb = false;
				}

				m_Resonates = false;

				int castingFocus = Caster.GetMagicalAttribute( MagicalAttribute.CastingFocus );

				castingFocus += (int) Math.Max( 0.0, ( Caster.Skills[SkillName.Inscribe].Value - 50.0 ) / 10.0 );

				if ( castingFocus > Utility.Random( 100 ) )
				{
					Caster.SendLocalizedMessage( 1113690 ); // You regain your focus and continue casting the spell.
					disturb = false;
				}

				if ( disturb )
					Disturb( DisturbType.Hurt, false, true );
			}
		}

		public virtual void OnCasterKilled()
		{
			Disturb( DisturbType.Kill );
		}

		public virtual void OnConnectionChanged()
		{
			FinishSequence();
		}

		public virtual bool OnCasterMoving( Direction d )
		{
			if ( IsCasting && BlocksMovement )
			{
				m_Caster.SendLocalizedMessage( 500111 ); // You are frozen and can not move.
				return false;
			}

			return true;
		}

		public virtual bool OnCasterEquiping( Item item )
		{
			if ( IsCasting )
				Disturb( DisturbType.EquipRequest );

			return true;
		}

		public virtual bool OnCasterUsingObject( object o )
		{
			if ( m_State == SpellState.Sequencing )
				Disturb( DisturbType.UseRequest );

			return true;
		}

		public virtual bool OnCastInTown( Region r )
		{
			return m_Info.AllowTown;
		}

		public virtual bool ConsumeReagents()
		{
			if ( ( m_Scroll != null && !( m_Scroll is SpellStone ) ) || !m_Caster.IsPlayer )
				return true;

			// Staff members do not consume reagents
			if ( m_Caster.AccessLevel >= AccessLevel.GameMaster )
				return true;

			if ( m_Caster.GetMagicalAttribute( MagicalAttribute.LowerRegCost ) > Utility.Random( 100 ) )
				return true;

			Container pack = m_Caster.Backpack;

			if ( pack == null )
				return false;

			if ( pack.ConsumeTotal( m_Info.Reagents, m_Info.Amounts ) == -1 )
				return true;

			return false;
		}

		public virtual double GetInscribeSkill( Mobile m )
		{
			return m.Skills[SkillName.Inscribe].Value;
		}

		public virtual int GetInscribeFixed( Mobile m )
		{
			return m.Skills[SkillName.Inscribe].Fixed;
		}

		public virtual int GetDamageFixed( Mobile m )
		{
			m.CheckSkill( DamageSkill, 0.0, 120.0 );

			return m.Skills[DamageSkill].Fixed;
		}

		public virtual double GetDamageSkill( Mobile m )
		{
			m.CheckSkill( DamageSkill, 0.0, 120.0 );

			return m.Skills[DamageSkill].Value;
		}

		public virtual double GetResistSkill( Mobile m )
		{
			return m.Skills[SkillName.MagicResist].Value;
		}

		public virtual double GetDamageScalar( Mobile target )
		{
			double scalar = 1.0;

			if ( target is BaseCreature )
				( (BaseCreature) target ).AlterDamageScalarFrom( m_Caster, ref scalar );

			if ( m_Caster is BaseCreature )
				( (BaseCreature) m_Caster ).AlterDamageScalarTo( target, ref scalar );

			scalar *= GetSlayerDamageScalar( target );

			target.Region.SpellDamageScalar( m_Caster, target, ref scalar );

			if ( RunedSashOfWarding.IsUnderWardingEffect( target, WardingEffect.SpellDamage ) )
				scalar *= 0.9;

			if ( Evasion.CheckSpellEvasion( target ) )	//Only single target spells an be evaded
				scalar = 0;

			return scalar;
		}

		public virtual double GetSlayerDamageScalar( Mobile defender )
		{
			Spellbook atkBook = Spellbook.FindEquippedSpellbook( m_Caster );

			double scalar = 1.0;
			if ( atkBook != null )
			{
				SlayerEntry atkSlayer = SlayerGroup.GetEntryByName( atkBook.Slayer );
				SlayerEntry atkSlayer2 = SlayerGroup.GetEntryByName( atkBook.Slayer2 );

				if ( atkSlayer != null && atkSlayer.Slays( defender ) )
				{
					defender.FixedEffect( 0x37B9, 10, 5 );
					scalar = Math.Max( scalar, SlayerGroup.IsSuperSlayer( atkSlayer.Name ) ? 2.0 : 3.0 );
				}

				if ( atkSlayer2 != null && atkSlayer2.Slays( defender ) )
				{
					defender.FixedEffect( 0x37B9, 10, 5 );
					scalar = Math.Max( scalar, SlayerGroup.IsSuperSlayer( atkSlayer2.Name ) ? 2.0 : 3.0 );
				}

				TransformContext context = TransformationSpell.GetContext( defender );

				if ( ( atkBook.Slayer == SlayerName.Undead || atkBook.Slayer2 == SlayerName.Undead ) && context != null && context.Type != typeof( HorrificBeastSpell ) )
					scalar += .25; // Every necromancer transformation other than horrific beast take an additional 25% damage

				if ( scalar != 1.0 )
					return scalar;
			}

			#region Opposite Slayer
			if ( defender.GetSlayerEntries().Any( e => e.Group.OppositionSuperSlays( Caster ) ) )
				scalar = 2.0;
			#endregion

			return scalar;
		}

		public virtual void DoFizzle()
		{
			m_Caster.LocalOverheadMessage( MessageType.Regular, 0x3B2, 502632 ); // The spell fizzles.

			if ( m_Caster.IsPlayer )
			{
				m_Caster.FixedParticles( 0x3735, 1, 30, 9503, EffectLayer.Waist );
				m_Caster.PlaySound( 0x5C );
			}
		}

		private CastTimer m_CastTimer;
		private AnimTimer m_AnimTimer;

		public void Disturb( DisturbType type )
		{
			Disturb( type, true, false );
		}

		public virtual bool CheckDisturb( DisturbType type, bool firstCircle, bool resistable )
		{
			if ( resistable && m_Scroll is BaseWand )
				return false;

			return true;
		}

		public void Disturb( DisturbType type, bool firstCircle, bool resistable )
		{
			if ( !CheckDisturb( type, firstCircle, resistable ) )
				return;

			if ( m_State == SpellState.Casting )
			{
				m_State = SpellState.None;
				m_Caster.Spell = null;

				OnDisturb( type, true );

				if ( m_CastTimer != null )
					m_CastTimer.Stop();

				if ( m_AnimTimer != null )
					m_AnimTimer.Stop();

				if ( m_Caster.IsPlayer && type == DisturbType.Hurt )
					DoHurtFizzle();

				m_Caster.NextSpellTime = DateTime.Now + GetDisturbRecovery();
			}
			else if ( m_State == SpellState.Sequencing )
			{
				m_State = SpellState.None;
				m_Caster.Spell = null;

				OnDisturb( type, false );

				Targeting.Target.Cancel( m_Caster );

				if ( m_Caster.IsPlayer && type == DisturbType.Hurt )
					DoHurtFizzle();
			}
		}

		public virtual void DoHurtFizzle()
		{
			m_Caster.FixedEffect( 0x3735, 6, 30 );
			m_Caster.PlaySound( 0x5C );
		}

		public virtual void OnDisturb( DisturbType type, bool message )
		{
			if ( message )
				m_Caster.SendLocalizedMessage( 500641 ); // Your concentration is disturbed, thus ruining thy spell.
		}

		public virtual bool CheckCast()
		{
			return true;
		}

		public virtual void SayMantra()
		{
			if ( m_Scroll is SpellStone )
				return;

			if ( m_Scroll is BaseWand )
				return;

			// Monsters in debug mode say Mantra too
			if ( m_Info.Mantra != null && m_Info.Mantra.Length > 0 && ( m_Caster.IsPlayer || m_Caster is BaseCreature && ( (BaseCreature) m_Caster ).Debug ) )
				m_Caster.PublicOverheadMessage( MessageType.Spell, m_Caster.SpeechHue, true, m_Info.Mantra, false );
		}

		public virtual bool BlockedByHorrificBeast { get { return true; } }
		public virtual bool BlockedByAnimalForm { get { return true; } }
		public virtual bool BlocksMovement { get { return true; } }

		public virtual bool CheckNextSpellTime { get { return !( m_Scroll is BaseWand ); } }

		public bool Cast()
		{
			m_StartCastTime = DateTime.Now;

			if ( m_Caster.Spell is Spell && ( (Spell) m_Caster.Spell ).State == SpellState.Sequencing )
				( (Spell) m_Caster.Spell ).Disturb( DisturbType.NewCast );

			if ( !m_Caster.CheckAlive() )
			{
				return false;
			}
			else if ( m_Caster.Spell != null && m_Caster.Spell.IsCasting )
			{
				m_Caster.SendLocalizedMessage( 502642 ); // You are already casting a spell.
			}
			else if ( BlockedByHorrificBeast && TransformationSpell.UnderTransformation( m_Caster, typeof( HorrificBeastSpell ) ) )
			{
				m_Caster.SendLocalizedMessage( 1061091 ); // You cannot cast that spell in this form.
			}
			else if ( BlockedByAnimalForm && AnimalForm.UnderTransformation( m_Caster ) )
			{
				m_Caster.SendLocalizedMessage( 1061091 ); // You cannot cast that spell in this form.
			}
			else if ( !( m_Scroll is BaseWand ) && ( m_Caster.Paralyzed || m_Caster.Frozen ) )
			{
				m_Caster.SendLocalizedMessage( 502643 ); // You can not cast a spell while frozen.
			}
			else if ( CheckNextSpellTime && DateTime.Now < m_Caster.NextSpellTime )
			{
				m_Caster.SendLocalizedMessage( 502644 ); // You have not yet recovered from casting a spell.
			}
			else if ( BaseBardCreature.IsCalmed( m_Caster ) )
			{
				m_Caster.SendLocalizedMessage( 1072060 ); // You cannot cast a spell while calmed.
			}
			else if ( m_Caster.Mana >= ScaleMana( GetMana() ) )
			{
				if ( m_Caster.Spell == null && m_Caster.CheckSpellCast( this ) && CheckCast() && m_Caster.Region.OnBeginSpellCast( m_Caster, this ) )
				{
					m_State = SpellState.Casting;
					m_Caster.Spell = this;

					if ( RevealOnCast )
						m_Caster.RevealingAction();

					SayMantra();

					TimeSpan castDelay = this.GetCastDelay();

					/*
					 * OSI cast delay is computed with a global timer based on ticks. There is
					 * one tick per 0.25 seconds, so every tick the timer computes all the
					 * spells ready to cast. This introduces a random additional delay of 0-0.25
					 * seconds due to fact that the first tick is always incomplete. We add this
					 * manually here to enhance the gameplay to get the real OSI feeling.
					 */
					castDelay += TimeSpan.FromSeconds( 0.25 * Utility.RandomDouble() );

					if ( ShowHandMovement && m_Caster.Body.IsHuman && !( m_Scroll is SpellStone ) )
					{
						int count = (int) Math.Ceiling( castDelay.TotalSeconds / AnimateDelay.TotalSeconds );

						if ( count != 0 )
						{
							m_AnimTimer = new AnimTimer( this, count );
							m_AnimTimer.Start();
						}

						if ( m_Info.LeftHandEffect > 0 )
							Caster.FixedParticles( 0, 10, 5, m_Info.LeftHandEffect, EffectLayer.LeftHand );

						if ( m_Info.RightHandEffect > 0 )
							Caster.FixedParticles( 0, 10, 5, m_Info.RightHandEffect, EffectLayer.RightHand );
					}

					if ( ClearHandsOnCast )
						m_Caster.ClearHands();

					m_CastTimer = new CastTimer( this, castDelay );
					m_CastTimer.Start();

					SpecialMove.ClearCurrentMove( m_Caster );
					WeaponAbility.ClearCurrentAbility( m_Caster );

					OnBeginCast();

					return true;
				}
				else
				{
					return false;
				}
			}
			else
			{
				m_Caster.LocalOverheadMessage( MessageType.Regular, 0x22, 502625 ); // Insufficient mana
			}

			return false;
		}

		public abstract void OnCast();

		public virtual void OnBeginCast()
		{
		}

		public virtual void GetCastSkills( out double min, out double max )
		{
			min = max = 0; // Intended but not required for overriding.
		}

		public virtual bool CheckFizzle()
		{
			if ( m_Scroll is BaseWand )
				return true;

			double minSkill, maxSkill;

			GetCastSkills( out minSkill, out maxSkill );

			return Caster.CheckSkill( CastSkill, minSkill, maxSkill );
		}

		public abstract int GetMana();

		public virtual int ScaleMana( int mana )
		{
			double scalar = 1.0;

			if ( !Necromancy.MindRotSpell.GetMindRotScalar( Caster, ref scalar ) )
				scalar = 1.0;

			int lmc = m_Caster.GetMagicalAttribute( MagicalAttribute.LowerManaCost );

			if ( lmc > 40 )
				lmc = 40;

			scalar -= (double) lmc / 100;

			#region Mana Phase
			if ( ManaPhase.UnderEffect( Caster ) )
				scalar = 0.0;
			#endregion

			return (int) ( mana * scalar );
		}

		public virtual TimeSpan GetDisturbRecovery()
		{
			return TimeSpan.Zero;
		}

		public virtual int CastRecoveryBase { get { return 6; } }
		public virtual int CastRecoveryFastScalar { get { return 1; } }
		public virtual int CastRecoveryPerSecond { get { return 4; } }
		public virtual int CastRecoveryMinimum { get { return 0; } }

		public virtual TimeSpan GetCastRecovery()
		{
			// Staff members have no recovery
			if ( m_Caster.AccessLevel >= AccessLevel.GameMaster )
				return TimeSpan.Zero;

			if ( Caster is BaseCreature && ( (BaseCreature) Caster ).InstantCast )
				return TimeSpan.Zero;

			int fcr = m_Caster.GetMagicalAttribute( MagicalAttribute.CastRecovery );

			if ( Caster is BaseCreature && !( (BaseCreature) Caster ).Controlled && !( (BaseCreature) Caster ).Summoned )
				fcr = 4;

			fcr -= ThunderstormSpell.GetCastRecoveryMalus( m_Caster );

			int fcrDelay = -( CastRecoveryFastScalar * fcr );

			int delay = CastRecoveryBase + fcrDelay;

			if ( delay < CastRecoveryMinimum )
				delay = CastRecoveryMinimum;

			return TimeSpan.FromSeconds( (double) delay / CastRecoveryPerSecond );
		}

		public abstract TimeSpan CastDelayBase { get; }

		public virtual int FasterCastingCap { get { return 2; } }
		public virtual double CastDelayFastScalar { get { return 1; } }
		public virtual double CastDelaySecondsPerTick { get { return 0.25; } }
		public virtual TimeSpan CastDelayMinimum { get { return TimeSpan.FromSeconds( 0.25 ); } }

		public virtual TimeSpan GetCastDelay()
		{
			// Staff members cast instantly
			if ( Caster.AccessLevel >= AccessLevel.GameMaster )
				return TimeSpan.Zero;

			if ( m_Scroll is SpellStone )
				return TimeSpan.Zero;

			if ( Caster is BaseCreature && ( (BaseCreature) Caster ).InstantCast )
				return TimeSpan.Zero;

			int fcMax = FasterCastingCap;
			int fc = m_Caster.GetMagicalAttribute( MagicalAttribute.CastSpeed );

			if ( Caster is BaseCreature && !( (BaseCreature) Caster ).Controlled && !( (BaseCreature) Caster ).Summoned )
				fc = 2;

			if ( fc > fcMax )
				fc = fcMax;

			if ( ProtectionSpell.Registry.Contains( m_Caster ) )
				fc -= 2;

			if ( EssenceOfWindSpell.IsDebuffed( m_Caster ) )
				fc -= EssenceOfWindSpell.GetFCMalus( m_Caster );

			if ( StoneFormSpell.UnderEffect( m_Caster ) )
				fc -= 2;

			TimeSpan baseDelay = CastDelayBase;

			TimeSpan fcDelay = TimeSpan.FromSeconds( -( CastDelayFastScalar * fc * CastDelaySecondsPerTick ) );

			TimeSpan delay = baseDelay + fcDelay;

			if ( delay < CastDelayMinimum )
				delay = CastDelayMinimum;

			return delay;
		}

		public virtual void FinishSequence()
		{
			m_State = SpellState.None;

			if ( m_Caster.Spell == this )
				m_Caster.Spell = null;
		}

		public virtual int ComputeKarmaAward()
		{
			return 0;
		}

		public virtual bool CheckSequence()
		{
			int mana = ScaleMana( GetMana() );

			if ( m_Caster.Deleted || !m_Caster.Alive || m_Caster.Spell != this || m_State != SpellState.Sequencing )
			{
				DoFizzle();
			}
			else if ( m_Scroll != null && !( m_Scroll is Runebook ) && ( m_Scroll.Amount <= 0 || m_Scroll.Deleted || m_Scroll.RootParent != m_Caster || ( m_Scroll is BaseWand && ( ( (BaseWand) m_Scroll ).Charges <= 0 || m_Scroll.Parent != m_Caster ) ) ) )
			{
				DoFizzle();
			}
			else if ( !ConsumeReagents() )
			{
				m_Caster.LocalOverheadMessage( MessageType.Regular, 0x22, 502630 ); // More reagents are needed for this spell.
			}
			else if ( m_Caster.Mana < mana )
			{
				m_Caster.LocalOverheadMessage( MessageType.Regular, 0x22, 502625 ); // Insufficient mana for this spell.
			}
			else if ( m_Caster.Frozen || m_Caster.Paralyzed )
			{
				m_Caster.SendLocalizedMessage( 502646 ); // You cannot cast a spell while frozen.
				DoFizzle();
			}
			else if ( CheckFizzle() )
			{
				m_Caster.Mana -= mana;

				#region ManaPhase
				if ( ManaPhase.UnderEffect( m_Caster ) )
					ManaPhase.OnManaConsumed( m_Caster );
				#endregion

				if ( m_Scroll is SpellStone )
					( (SpellStone) m_Scroll ).Use( m_Caster );
				if ( m_Scroll is SpellScroll )
					m_Scroll.Consume();
				else if ( m_Scroll is BaseWand )
					( (BaseWand) m_Scroll ).ConsumeCharge( m_Caster );

				if ( m_Scroll is BaseWand )
				{
					bool m = m_Scroll.Movable;

					m_Scroll.Movable = false;

					if ( ClearHandsOnCast )
						m_Caster.ClearHands();

					m_Scroll.Movable = m;
				}
				else
				{
					if ( ClearHandsOnCast )
						m_Caster.ClearHands();
				}

				int karma = ComputeKarmaAward();

				if ( karma != 0 )
					Misc.Titles.AwardKarma( Caster, karma, true );

				if ( TransformationSpell.UnderTransformation( m_Caster, typeof( VampiricEmbraceSpell ) ) )
				{
					bool garlic = false;

					for ( int i = 0; !garlic && i < m_Info.Reagents.Length; ++i )
						garlic = ( m_Info.Reagents[i] == Reagent.Garlic );

					if ( garlic )
					{
						m_Caster.SendLocalizedMessage( 1061651 ); // The garlic burns you!
						AOS.Damage( m_Caster, Utility.RandomMinMax( 17, 23 ), 100, 0, 0, 0, 0 );
					}
				}

				return true;
			}
			else
			{
				DoFizzle();
			}

			return false;
		}

		public bool CheckBSequence( Mobile target )
		{
			return CheckBSequence( target, false );
		}

		public bool CheckBSequence( Mobile target, bool allowDead )
		{
			if ( !target.Alive && !allowDead )
			{
				m_Caster.SendLocalizedMessage( 501857 ); // This spell won't work on that!
				return false;
			}
			else if ( Caster.CanBeBeneficial( target, true, allowDead ) && CheckSequence() )
			{
				Caster.DoBeneficial( target );
				return true;
			}
			else
			{
				return false;
			}
		}

		public bool CheckHSequence( Mobile target )
		{
			if ( !target.Alive )
			{
				m_Caster.SendLocalizedMessage( 501857 ); // This spell won't work on that!
				return false;
			}
			else if ( Caster.CanBeHarmful( target ) && CheckSequence() )
			{
				Caster.DoHarmful( target );
				return true;
			}
			else
			{
				return false;
			}
		}

		private class AnimTimer : Timer
		{
			private Spell m_Spell;

			public AnimTimer( Spell spell, int count )
				: base( TimeSpan.Zero, AnimateDelay, count )
			{
				m_Spell = spell;
			}

			protected override void OnTick()
			{
				if ( m_Spell.State != SpellState.Casting || m_Spell.m_Caster.Spell != m_Spell )
				{
					Stop();
					return;
				}

				if ( !m_Spell.Caster.Mounted && m_Spell.Caster.Body.IsHuman && m_Spell.m_Info.Action >= 0 )
					m_Spell.Caster.Animate( 0xB );

				if ( !Running )
					m_Spell.m_AnimTimer = null;
			}
		}

		private class CastTimer : Timer
		{
			private Spell m_Spell;

			public CastTimer( Spell spell, TimeSpan castDelay )
				: base( castDelay )
			{
				m_Spell = spell;

			}

			protected override void OnTick()
			{
				if ( m_Spell.m_State == SpellState.Casting && m_Spell.m_Caster.Spell == m_Spell )
				{
					m_Spell.m_State = SpellState.Sequencing;
					m_Spell.m_CastTimer = null;
					m_Spell.m_Caster.OnSpellCast( m_Spell );
					m_Spell.m_Caster.Region.OnSpellCast( m_Spell.m_Caster, m_Spell );
					m_Spell.m_Caster.NextSpellTime = DateTime.Now + m_Spell.GetCastRecovery();

					Target originalTarget = m_Spell.m_Caster.Target;

					m_Spell.m_Caster.TargetLocked = true;
					m_Spell.OnCast();

					if ( m_Spell.m_Caster.IsPlayer && m_Spell.m_Caster.Target != originalTarget && m_Spell.Caster.Target != null )
					{
						if ( m_Spell.MacroTarget != null )
							m_Spell.Caster.Target.Invoke( m_Spell.Caster, m_Spell.MacroTarget );
						else
							m_Spell.m_Caster.Target.BeginTimeout( m_Spell.m_Caster, TimeSpan.FromSeconds( 30.0 ) );
					}

					m_Spell.m_Caster.TargetLocked = false;
					m_Spell.m_CastTimer = null;
				}
			}
		}
	}
}