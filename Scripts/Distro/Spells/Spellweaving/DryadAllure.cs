using System;
using System.Collections;
using Server.Mobiles;
using Server.Network;
using Server.Items;
using Server.Targeting;

namespace Server.Spells.Spellweaving
{
	public class DryadAllureSpell : SpellweavingSpell
	{
		private static SpellInfo m_Info = new SpellInfo( "Dryad Allure", "Rathril", -1, 9002 );

		public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds( 2.0 ); } }

		public override double RequiredSkill { get { return 52.0; } }
		public override int RequiredMana { get { return 40; } }

		public DryadAllureSpell( Mobile caster, Item scroll )
			: base( caster, scroll, m_Info )
		{
		}

		public override void OnCast()
		{
			Caster.Target = new InternalTarget( this );
		}

		public override bool CheckCast()
		{
			if ( !base.CheckCast() )
				return false;

			if ( ( Caster.Followers + 3 ) > Caster.FollowersMax )
			{
				Caster.SendLocalizedMessage( 1049645 ); // You have too many followers to control that creature.
				return false;
			}

			return true;
		}

		public void Target( BaseCreature bc )
		{
			if ( !Caster.CanSee( bc ) )
			{
				Caster.SendLocalizedMessage( 500237 ); // Target can not be seen.
			}
			if ( !Caster.InRange( bc, 6 ) )
			{
				Caster.SendLocalizedMessage( 500643 ); // Target is too far away.
			}
			else if ( !Caster.CanBeHarmful( bc ) || !bc.CanBeDamaged() )
			{
				Caster.SendLocalizedMessage( 1074379 ); // You cannot charm that!
			}
			else if ( bc.Controlled || bc.Name == null )
			{
				Caster.SendLocalizedMessage( 1074379 ); // You cannot charm that!
			}
			else if ( bc is BaseChampion || bc.IsParagon || bc is Medusa || bc is Lurg || !SlayerGroup.GetEntryByName( SlayerName.Repond ).Slays( bc ) )
			{
				Caster.SendLocalizedMessage( 1074379 ); // You cannot charm that!
			}
			else if ( CheckSequence() )
			{
				double chance = Caster.Skills[SkillName.Spellweaving].Fixed / 1000;

				chance += ( SpellweavingSpell.GetFocusLevel( Caster ) * 2 ) / 100;

				if ( chance > Utility.RandomDouble() )
				{
					SpellHelper.Turn( Caster, bc );

					bc.ControlSlots = 3;

					bc.ActiveSpeed = 2;
					bc.PassiveSpeed = 2;

					bc.Owners.Add( Caster );

					bc.SetControlMaster( Caster );

					bc.IsBonded = false;

					Caster.SendLocalizedMessage( 1072527 ); // You allure the humanoid to follow and protect you.

					Caster.PlaySound( 0x5C4 );
				}
				else
				{
					bc.Combatant = Caster;

					Caster.SendLocalizedMessage( 1072528 ); // The humanoid becomes enraged by your charming attempt and attacks you.

					Caster.PlaySound( 0x5C5 );
				}
			}

			FinishSequence();
		}

		private class InternalTarget : Target
		{
			private DryadAllureSpell m_Owner;

			public InternalTarget( DryadAllureSpell owner )
				: base( 12, false, TargetFlags.None )
			{
				m_Owner = owner;
			}

			protected override void OnTarget( Mobile from, object o )
			{
				if ( o is BaseCreature )
					m_Owner.Target( (BaseCreature) o );
				else
					m_Owner.Caster.SendLocalizedMessage( 1074379 ); // You cannot charm that!
			}

			protected override void OnTargetFinish( Mobile from )
			{
				m_Owner.FinishSequence();
			}
		}
	}
}