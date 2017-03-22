using System;
using System.Collections.Generic;
using System.Linq;
using Server.Network;
using Server.Gumps;
using Server.Items;
using Server.Targeting;
using Server.Mobiles;
using Server.Spells.First;
using Server.Spells.Second;
using Server.Spells.Fifth;
using Server.Spells.Chivalry;
using Server.Spells.Necromancy;
using Server.Spells.Ninjitsu;
using Server.Spells.Spellweaving;

namespace Server.Spells.Mysticism
{
	public class PurgeMagicSpell : MysticSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Purge Magic", "An Ort Sanct",
				-1,
				9002,
				Reagent.FertileDirt,
				Reagent.Garlic,
				Reagent.MandrakeRoot,
				Reagent.SulfurousAsh
			);

		public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds( 1.0 ); } }

		public override double RequiredSkill { get { return 8.0; } }
		public override int RequiredMana { get { return 6; } }

		public PurgeMagicSpell( Mobile caster, Item scroll )
			: base( caster, scroll, m_Info )
		{
		}

		public override bool CheckCast()
		{
			if ( !base.CheckCast() )
				return false;

			if ( AnimalForm.UnderTransformation( Caster ) )
			{
				Caster.SendLocalizedMessage( 1063218 ); // You cannot use that ability in this form.
				return false;
			}

			return true;
		}

		private BeneficalWard[] m_Entries = new BeneficalWard[]
			{
				//new BeneficalWard( "Agility",			null, null ),
				//new BeneficalWard( "Cunning",			null, null ),
				//new BeneficalWard( "Strength",			null, null ),
				//new BeneficalWard( "Bless",				null, null ),
				new BeneficalWard( "Magic Reflection",	MagicReflectSpell.UnderEffect,		MagicReflectSpell.RemoveWard, new MagicReflectSpell(null, null).MinimumCastSkill ),
				new BeneficalWard( "Curse Weapon",		CurseWeaponSpell.UnderEffect,		CurseWeaponSpell.RemoveEffect, new CurseWeaponSpell(null, null).RequiredSkill ),
				new BeneficalWard( "Horrific Beast",	HorrificBeastSpell.UnderEffect,		HorrificBeastSpell.RemoveEffect, new HorrificBeastSpell(null, null).RequiredSkill ),
				new BeneficalWard( "Lich Form",			LichFormSpell.UnderEffect,			LichFormSpell.RemoveEffect, new LichFormSpell(null, null).RequiredSkill ),
				new BeneficalWard( "Vampiric Embrace",	VampiricEmbraceSpell.UnderEffect,	VampiricEmbraceSpell.RemoveEffect, new VampiricEmbraceSpell(null, null).RequiredSkill ),
				new BeneficalWard( "Wraith Form",		WraithFormSpell.UnderEffect,		WraithFormSpell.RemoveEffect, new WraithFormSpell(null, null).RequiredSkill ),
				new BeneficalWard( "Protection",		ProtectionSpell.UnderEffect,		ProtectionSpell.RemoveWard, new ProtectionSpell(null, null).MinimumCastSkill ),
				new BeneficalWard( "Consecrate Weapon",	ConsecrateWeaponSpell.UnderEffect,	ConsecrateWeaponSpell.RemoveEffect, new ConsecrateWeaponSpell(null, null).RequiredSkill ),
				new BeneficalWard( "Divine Fury",		DivineFurySpell.UnderEffect,		DivineFurySpell.RemoveEffect, new DivineFurySpell(null, null).RequiredSkill ),
				new BeneficalWard( "Enemy of One",		EnemyOfOneSpell.UnderEffect,		EnemyOfOneSpell.RemoveEffect, new EnemyOfOneSpell(null, null).RequiredSkill ),
				new BeneficalWard( "Reactive Armor",	ReactiveArmorSpell.UnderEffect,		ReactiveArmorSpell.RemoveWard, new ReactiveArmorSpell(null, null).MinimumCastSkill ),
				//new BeneficalWard( "Confidence",		null, null ),
				new BeneficalWard( "Arcane Empowerment",ArcaneEmpowermentSpell.IsBuffed,	ArcaneEmpowermentSpell.RemoveBuff, new ArcaneEmpowermentSpell(null, null).RequiredSkill ),
				new BeneficalWard( "Attune Weapon",		AttunementSpell.IsAbsorbing,		AttunementSpell.StopAbsorbing, new AttunementSpell(null, null).RequiredSkill ),
				new BeneficalWard( "Gift of Life",		GiftOfLifeSpell.UnderEffect,		GiftOfLifeSpell.RemoveEffect, new GiftOfLifeSpell(null, null).RequiredSkill ),
				new BeneficalWard( "Gift of Renewal",	GiftOfRenewalSpell.UnderEffect,		GiftOfRenewalSpell.RemoveEffect, new GiftOfRenewalSpell(null, null).RequiredSkill ),
				new BeneficalWard( "Immolating Weapon",	ImmolatingWeaponSpell.IsImmolating,	ImmolatingWeaponSpell.EndImmolating, new ImmolatingWeaponSpell(null, null).RequiredSkill ),
				new BeneficalWard( "Reaper Form",		ReaperFormSpell.UnderEffect,		ReaperFormSpell.RemoveEffects, new ReaperFormSpell(null, null).RequiredSkill ),
				new BeneficalWard( "Enchant",			EnchantSpell.UnderEffect,			EnchantSpell.RemoveEffect, new EnchantSpell(null, null).RequiredSkill ),
				//new BeneficalWard( "Stone Form",		null, null ),
			};

		public BeneficalWard[] Entries { get { return m_Entries; } }

		private List<BeneficalWard> GetCurrentWardsFor( Mobile m )
		{
			return m_Entries.Where( ward => ward.UnderEffect != null && ward.UnderEffect( m ) ).ToList();
		}

		public override void OnCast()
		{
			Caster.Target = new InternalTarget( this );
		}

		public void Target( Mobile m )
		{
			var wards = GetCurrentWardsFor( m );

			if ( wards.Count == 0 )
			{
				Caster.SendLocalizedMessage( 1080120 ); // Your target has no magic that can be purged.
			}
			else if ( CheckHSequence( m ) )
			{
				/* Attempts to remove a beneficial ward from the Target,
				 * chosen randomly. The chance to successfully Purge Magic
				 * is determined by a comparison between the Caster's
				 * Focus and Mysticism skills and the
				 * Target's Resisting Spells skill.
				 */

				var ward = wards[Utility.Random( wards.Count )];

				//First check, Focus + Mysticism / 2 must be greater than target's Resist - 10
				//Second check, Base Focus + Base Mysticism must be greater than spell's minimum required skill
				if ( ( ( Caster.Skills.Focus.Value + Caster.Skills.Mysticism.Value ) * 0.5 ) >= m.Skills.MagicResist.Value - 10 &&
					Caster.Skills.Focus.Base + Caster.Skills.Mysticism.Base >= ward.RequiredSkill )
				{
					ward.RemoveEffect( m );

					m.PlaySound( 0x655 );
					Effects.SendTargetParticles( m, 0x3728, 1, 13, 0x834, 3, 0x13B2, EffectLayer.Head, 0 );

					m.SendLocalizedMessage( 1080117, ward.Name ); // Your ~1_ABILITY_NAME~ has been purged.

					if ( Caster != m )
						Caster.SendLocalizedMessage( 1080118, ward.Name ); // Your target's ~1_ABILITY_NAME~ has been purged.
				}
				else
				{
					Caster.SendLocalizedMessage( 1080119 ); // Your Purge Magic has been resisted!
				}
			}

			FinishSequence();
		}

		private class InternalTarget : Target
		{
			private PurgeMagicSpell m_Owner;

			public InternalTarget( PurgeMagicSpell owner )
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

		public delegate bool UnderEffect( Mobile m );
		public delegate void RemoveEffect( Mobile m );

		public class BeneficalWard
		{
			public string Name { get; }
			public UnderEffect UnderEffect { get; }
			public RemoveEffect RemoveEffect { get; }
			public double RequiredSkill { get; }

			public BeneficalWard( string name, UnderEffect underEffect, RemoveEffect removeEffect, double requiredSkill )
			{
				Name = name;
				UnderEffect = underEffect;
				RemoveEffect = removeEffect;
				RequiredSkill = requiredSkill;
			}
		}
	}
}
