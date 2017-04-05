using System;
using System.Linq;

using Server.Items;
using Server.Network;
using Server.Targeting;
using Server.Mobiles;
using Server.Spells.Second;
using Server.Spells.Necromancy;
using Server.Spells.Bushido;
using Server.Spells.Ninjitsu;
using Server.Spells.Spellweaving;
using Server.Spells.Mysticism;
using Server.Misc;

namespace Server.Spells
{
	public abstract class Spell : ISpell
	{
		public SpellState State { get; set; }
		public Mobile Caster { get; }
		public IEntity MacroTarget { get; set; }
		public SpellInfo Info { get; }
		public Item Scroll { get; }

		public string Name => Info.Name;
		public string Mantra => Info.Mantra;
		public Type[] Reagents => Info.Reagents;

		private static TimeSpan AnimateDelay = TimeSpan.FromSeconds( 1.5 );

		public virtual SkillName CastSkill => SkillName.Magery;
		public virtual SkillName DamageSkill => SkillName.EvalInt;

		public virtual bool RevealOnCast => true;
		public virtual bool ClearHandsOnCast => Caster.Player;
		public virtual bool ShowHandMovement => true;

		public virtual bool DelayedDamage => false;

		protected Spell( Mobile caster, Item scroll, SpellInfo info )
		{
			Caster = caster;
			Scroll = scroll;
			Info = info;
		}

		public virtual int GetNewAosDamage( int bonus, int dice, int sides, Mobile singleTarget )
		{
			if ( singleTarget != null )
				return GetNewAosDamage( bonus, dice, sides, ( Caster.Player && singleTarget.Player ), GetDamageScalar( singleTarget ) );
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
			int inscribeSkill = GetInscribeFixed( Caster );
			int inscribeBonus = ( inscribeSkill + ( 1000 * ( inscribeSkill / 1000 ) ) ) / 200;
			damageBonus += inscribeBonus;
			#endregion

			#region Intelligence Bonus
			int intBonus = Caster.Int / 10;
			damageBonus += intBonus;
			#endregion

			#region SDI Bonus
			damageBonus += SpellHelper.GetSpellDamage( Caster, playerVsPlayer );
			#endregion

			#region EvalInt Bonus
			int evalSkill = GetDamageFixed( Caster );
			int evalScale = 30 + ( ( 9 * evalSkill ) / 100 );
			#endregion

			damage = AOS.Scale( damage, 100 + damageBonus );
			damage = AOS.Scale( damage, evalScale );
			damage = AOS.Scale( damage, (int) ( scalar * 100 ) );

			return damage / 100;
		}

		public bool Resonates { get; set; }

		public virtual bool IsCasting => State == SpellState.Casting;

		public virtual void OnCasterHurt()
		{
			if ( !Caster.Player )
				return;

			if ( IsCasting )
			{
				object o = ProtectionSpell.Registry[Caster];
				bool disturb = true;

				if ( o != null && o is double )
				{
					if ( ( (double) o ) > Utility.RandomDouble() * 100.0 )
						disturb = false;
				}

				if ( Resonates )
				{
					Caster.SendLocalizedMessage( 1113689 ); // Your equipment resonates with the damage you receive preventing you from being interrupted during casting.
					disturb = false;
				}

				Resonates = false;

				int castingFocus = Caster.GetMagicalAttribute( AosAttribute.CastingFocus );

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
				Caster.SendLocalizedMessage( 500111 ); // You are frozen and can not move.
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
			if ( State == SpellState.Sequencing )
				Disturb( DisturbType.UseRequest );

			return true;
		}

		public virtual bool OnCastInTown( Region r )
		{
			return Info.AllowTown;
		}

		public virtual bool ConsumeReagents()
		{
			if ( ( Scroll != null && !( Scroll is SpellStone ) ) || !Caster.Player )
				return true;

			// Staff members do not consume reagents
			if ( Caster.AccessLevel >= AccessLevel.GameMaster )
				return true;

			if ( Caster.GetMagicalAttribute( AosAttribute.LowerRegCost ) > Utility.Random( 100 ) )
				return true;

			Container pack = Caster.Backpack;

			if ( pack == null )
				return false;

			if ( pack.ConsumeTotal( Info.Reagents, Info.Amounts ) == -1 )
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
				( (BaseCreature) target ).AlterDamageScalarFrom( Caster, ref scalar );

			if ( Caster is BaseCreature )
				( (BaseCreature) Caster ).AlterDamageScalarTo( target, ref scalar );

			scalar *= GetSlayerDamageScalar( target );

			target.Region.SpellDamageScalar( Caster, target, ref scalar );

			if ( RunedSashOfWarding.IsUnderWardingEffect( target, WardingEffect.SpellDamage ) )
				scalar *= 0.9;

			if ( Evasion.CheckSpellEvasion( target ) )	//Only single target spells an be evaded
				scalar = 0;

			return scalar;
		}

		public virtual double GetSlayerDamageScalar( Mobile defender )
		{
			Spellbook atkBook = Spellbook.FindEquippedSpellbook( Caster );

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
			Caster.LocalOverheadMessage( MessageType.Regular, 0x3B2, 502632 ); // The spell fizzles.

			if ( Caster.Player )
			{
				Caster.FixedParticles( 0x3735, 1, 30, 9503, EffectLayer.Waist );
				Caster.PlaySound( 0x5C );
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
			if ( resistable && Scroll is BaseWand )
				return false;

			return true;
		}

		public void Disturb( DisturbType type, bool firstCircle, bool resistable )
		{
			if ( !CheckDisturb( type, firstCircle, resistable ) )
				return;

			if ( State == SpellState.Casting )
			{
				State = SpellState.None;
				Caster.Spell = null;

				OnDisturb( type, true );

				if ( m_CastTimer != null )
					m_CastTimer.Stop();

				if ( m_AnimTimer != null )
					m_AnimTimer.Stop();

				if ( Caster.Player && type == DisturbType.Hurt )
					DoHurtFizzle();

				Caster.NextSpellTime = DateTime.UtcNow + GetDisturbRecovery();
			}
			else if ( State == SpellState.Sequencing )
			{
				State = SpellState.None;
				Caster.Spell = null;

				OnDisturb( type, false );

				Targeting.Target.Cancel( Caster );

				if ( Caster.Player && type == DisturbType.Hurt )
					DoHurtFizzle();
			}
		}

		public virtual void DoHurtFizzle()
		{
			Caster.FixedEffect( 0x3735, 6, 30 );
			Caster.PlaySound( 0x5C );
		}

		public virtual void OnDisturb( DisturbType type, bool message )
		{
			if ( message )
				Caster.SendLocalizedMessage( 500641 ); // Your concentration is disturbed, thus ruining thy spell.
		}

		public virtual bool CheckCast()
		{
			return true;
		}

		public virtual void SayMantra()
		{
			if ( Scroll is SpellStone )
				return;

			if ( Scroll is BaseWand )
				return;

			// Monsters in debug mode say Mantra too
			if ( Info.Mantra != null && Info.Mantra.Length > 0 && ( Caster.Player || Caster is BaseCreature && ( (BaseCreature) Caster ).Debug ) )
				Caster.PublicOverheadMessage( MessageType.Spell, Caster.SpeechHue, true, Info.Mantra, false );
		}

		public virtual bool BlockedByHorrificBeast { get { return true; } }
		public virtual bool BlockedByAnimalForm { get { return true; } }
		public virtual bool BlocksMovement { get { return true; } }

		public virtual bool CheckNextSpellTime { get { return !( Scroll is BaseWand ); } }

		public bool Cast()
		{
			if ( Caster.Spell is Spell && ( (Spell) Caster.Spell ).State == SpellState.Sequencing )
				( (Spell) Caster.Spell ).Disturb( DisturbType.NewCast );

			if ( !Caster.CheckAlive() )
			{
				return false;
			}
			else if ( Caster.Spell != null && Caster.Spell.IsCasting )
			{
				Caster.SendLocalizedMessage( 502642 ); // You are already casting a spell.
			}
			else if ( BlockedByHorrificBeast && TransformationSpell.UnderTransformation( Caster, typeof( HorrificBeastSpell ) ) )
			{
				Caster.SendLocalizedMessage( 1061091 ); // You cannot cast that spell in this form.
			}
			else if ( BlockedByAnimalForm && AnimalForm.UnderTransformation( Caster ) )
			{
				Caster.SendLocalizedMessage( 1061091 ); // You cannot cast that spell in this form.
			}
			else if ( !( Scroll is BaseWand ) && ( Caster.Paralyzed || Caster.Frozen ) )
			{
				Caster.SendLocalizedMessage( 502643 ); // You can not cast a spell while frozen.
			}
			else if ( CheckNextSpellTime && DateTime.UtcNow < Caster.NextSpellTime )
			{
				Caster.SendLocalizedMessage( 502644 ); // You have not yet recovered from casting a spell.
			}
			else if ( BaseBardCreature.IsCalmed( Caster ) )
			{
				Caster.SendLocalizedMessage( 1072060 ); // You cannot cast a spell while calmed.
			}
			else if ( Caster.Mana >= ScaleMana( GetMana() ) )
			{
				if ( Caster.Spell == null && Caster.CheckSpellCast( this ) && CheckCast() && Caster.Region.OnBeginSpellCast( Caster, this ) )
				{
					State = SpellState.Casting;
					Caster.Spell = this;

					if ( RevealOnCast )
						Caster.RevealingAction();

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

					if ( ShowHandMovement && Caster.Body.IsHuman && !( Scroll is SpellStone ) )
					{
						int count = (int) Math.Ceiling( castDelay.TotalSeconds / AnimateDelay.TotalSeconds );

						if ( count != 0 )
						{
							m_AnimTimer = new AnimTimer( this, count );
							m_AnimTimer.Start();
						}

						if ( Info.LeftHandEffect > 0 )
							Caster.FixedParticles( 0, 10, 5, Info.LeftHandEffect, EffectLayer.LeftHand );

						if ( Info.RightHandEffect > 0 )
							Caster.FixedParticles( 0, 10, 5, Info.RightHandEffect, EffectLayer.RightHand );
					}

					if ( ClearHandsOnCast )
						Caster.ClearHands();

					m_CastTimer = new CastTimer( this, castDelay );
					m_CastTimer.Start();

					SpecialMove.ClearCurrentMove( Caster );
					WeaponAbility.ClearCurrentAbility( Caster );

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
				Caster.LocalOverheadMessage( MessageType.Regular, 0x22, 502625 ); // Insufficient mana
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
			if ( Scroll is BaseWand )
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

			int lmc = Caster.GetMagicalAttribute( AosAttribute.LowerManaCost );

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
			if ( Caster.AccessLevel >= AccessLevel.GameMaster )
				return TimeSpan.Zero;

			if ( Caster is BaseCreature && ( (BaseCreature) Caster ).InstantCast )
				return TimeSpan.Zero;

			int fcr = Caster.GetMagicalAttribute( AosAttribute.CastRecovery );

			if ( Caster is BaseCreature && !( (BaseCreature) Caster ).Controlled && !( (BaseCreature) Caster ).Summoned )
				fcr = 4;

			fcr -= ThunderstormSpell.GetCastRecoveryMalus( Caster );

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

			if ( Scroll is SpellStone )
				return TimeSpan.Zero;

			if ( Caster is BaseCreature && ( (BaseCreature) Caster ).InstantCast )
				return TimeSpan.Zero;

			int fcMax = FasterCastingCap;
			int fc = Caster.GetMagicalAttribute( AosAttribute.CastSpeed );

			if ( Caster is BaseCreature && !( (BaseCreature) Caster ).Controlled && !( (BaseCreature) Caster ).Summoned )
				fc = 2;

			if ( fc > fcMax )
				fc = fcMax;

			if ( ProtectionSpell.Registry.Contains( Caster ) )
				fc -= 2;

			if ( EssenceOfWindSpell.IsDebuffed( Caster ) )
				fc -= EssenceOfWindSpell.GetFCMalus( Caster );

			if ( StoneFormSpell.UnderEffect( Caster ) )
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
			State = SpellState.None;

			if ( Caster.Spell == this )
				Caster.Spell = null;
		}

		public virtual int ComputeKarmaAward()
		{
			return 0;
		}

		public virtual bool CheckSequence()
		{
			int mana = ScaleMana( GetMana() );

			if ( Caster.Deleted || !Caster.Alive || Caster.Spell != this || State != SpellState.Sequencing )
			{
				DoFizzle();
			}
			else if ( Scroll != null && !( Scroll is Runebook ) && ( Scroll.Amount <= 0 || Scroll.Deleted || Scroll.RootParent != Caster || ( Scroll is BaseWand && ( ( (BaseWand) Scroll ).Charges <= 0 || Scroll.Parent != Caster ) ) ) )
			{
				DoFizzle();
			}
			else if ( !ConsumeReagents() )
			{
				Caster.LocalOverheadMessage( MessageType.Regular, 0x22, 502630 ); // More reagents are needed for this spell.
			}
			else if ( Caster.Mana < mana )
			{
				Caster.LocalOverheadMessage( MessageType.Regular, 0x22, 502625 ); // Insufficient mana for this spell.
			}
			else if ( Caster.Frozen || Caster.Paralyzed )
			{
				Caster.SendLocalizedMessage( 502646 ); // You cannot cast a spell while frozen.
				DoFizzle();
			}
			else if ( CheckFizzle() )
			{
				Caster.Mana -= mana;

				#region ManaPhase
				if ( ManaPhase.UnderEffect( Caster ) )
					ManaPhase.OnManaConsumed( Caster );
				#endregion

				if ( Scroll is SpellStone )
					( (SpellStone) Scroll ).Use( Caster );
				if ( Scroll is SpellScroll )
					Scroll.Consume();
				else if ( Scroll is BaseWand )
					( (BaseWand) Scroll ).ConsumeCharge( Caster );

				if ( Scroll is BaseWand )
				{
					bool m = Scroll.Movable;

					Scroll.Movable = false;

					if ( ClearHandsOnCast )
						Caster.ClearHands();

					Scroll.Movable = m;
				}
				else
				{
					if ( ClearHandsOnCast )
						Caster.ClearHands();
				}

				int karma = ComputeKarmaAward();

				if ( karma != 0 )
					Misc.Titles.AwardKarma( Caster, karma, true );

				if ( TransformationSpell.UnderTransformation( Caster, typeof( VampiricEmbraceSpell ) ) )
				{
					bool garlic = false;

					for ( int i = 0; !garlic && i < Info.Reagents.Length; ++i )
						garlic = ( Info.Reagents[i] == Reagent.Garlic );

					if ( garlic )
					{
						Caster.SendLocalizedMessage( 1061651 ); // The garlic burns you!
						AOS.Damage( Caster, Utility.RandomMinMax( 17, 23 ), 100, 0, 0, 0, 0 );
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
				Caster.SendLocalizedMessage( 501857 ); // This spell won't work on that!
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
				Caster.SendLocalizedMessage( 501857 ); // This spell won't work on that!
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
				if ( m_Spell.State != SpellState.Casting || m_Spell.Caster.Spell != m_Spell )
				{
					Stop();
					return;
				}

				if ( !m_Spell.Caster.Mounted && m_Spell.Caster.Body.IsHuman && m_Spell.Info.Action >= 0 )
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
				if ( m_Spell.State == SpellState.Casting && m_Spell.Caster.Spell == m_Spell )
				{
					m_Spell.State = SpellState.Sequencing;
					m_Spell.m_CastTimer = null;
					m_Spell.Caster.OnSpellCast( m_Spell );
					m_Spell.Caster.Region.OnSpellCast( m_Spell.Caster, m_Spell );
					m_Spell.Caster.NextSpellTime = DateTime.UtcNow + m_Spell.GetCastRecovery();

					Target originalTarget = m_Spell.Caster.Target;

					m_Spell.Caster.TargetLocked = true;
					m_Spell.OnCast();

					if ( m_Spell.Caster.Player && m_Spell.Caster.Target != originalTarget && m_Spell.Caster.Target != null )
					{
						if ( m_Spell.MacroTarget != null )
							m_Spell.Caster.Target.Invoke( m_Spell.Caster, m_Spell.MacroTarget );
						else
							m_Spell.Caster.Target.BeginTimeout( m_Spell.Caster, TimeSpan.FromSeconds( 30.0 ) );
					}

					m_Spell.Caster.TargetLocked = false;
					m_Spell.m_CastTimer = null;
				}
			}
		}
	}
}
