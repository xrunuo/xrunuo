using System;
using System.Collections.Generic;
using Server.Network;
using Server.Items;
using Server.Targeting;
using Server.Engines.BuffIcons;

namespace Server.Spells.Chivalry
{
	public class ConsecrateWeaponSpell : PaladinSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Consecrate Weapon", "Consecrus Arma",
				-1,
				9002
			);

		public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds( 0.5 ); } }

		public override double RequiredSkill { get { return 15.0; } }
		public override int RequiredMana { get { return 10; } }
		public override int RequiredTithing { get { return 10; } }
		public override int MantraNumber { get { return 1060720; } } // Consecrus Arma
		public override bool BlocksMovement { get { return false; } }

		public ConsecrateWeaponSpell( Mobile caster, Item scroll )
			: base( caster, scroll, m_Info )
		{
		}

		public override void OnCast()
		{
			BaseWeapon weapon = Caster.Weapon as BaseWeapon;

			if ( weapon == null || weapon is Fists )
			{
				Caster.SendLocalizedMessage( 501078 ); // You must be holding a weapon.
			}
			else if ( CheckSequence() )
			{
				/* Temporarily enchants the weapon the caster is currently wielding.
				 * The type of damage the weapon inflicts when hitting a target will
				 * be converted to the target's worst Resistance type.
				 * Duration of the effect is affected by the caster's Karma and lasts for 3 to 11 seconds.
				 */

				int itemID, soundID;

				switch ( weapon.Skill )
				{
					case SkillName.Macing:
						itemID = 0xFB4;
						soundID = 0x232;
						break;
					case SkillName.Archery:
						itemID = 0x13B1;
						soundID = 0x145;
						break;
					default:
						itemID = 0xF5F;
						soundID = 0x56;
						break;
				}

				Caster.PlaySound( 0x20C );
				Caster.PlaySound( soundID );
				Caster.FixedParticles( 0x3779, 1, 30, 9964, 3, 3, EffectLayer.Waist );

				IEntity from = new DummyEntity( Serial.Zero, new Point3D( Caster.X, Caster.Y, Caster.Z ), Caster.Map );
				IEntity to = new DummyEntity( Serial.Zero, new Point3D( Caster.X, Caster.Y, Caster.Z + 50 ), Caster.Map );
				Effects.SendMovingParticles( from, to, itemID, 1, 0, false, false, 33, 3, 9501, 1, 0, EffectLayer.Head, 0x100 );

				double seconds = ComputePowerValue( 20 );

				// TODO: Should caps be applied?
				Utility.FixMinMax( ref seconds, 3.0, 11.0 );

				TimeSpan duration = TimeSpan.FromSeconds( seconds );

				if ( m_Table.ContainsKey( Caster ) )
				{
					Timer t = m_Table[Caster].Timer;

					if ( t != null )
						t.Stop();
				}

				double chivalry = Caster.Skills.Chivalry.Value;

				Timer expireTimer = new ExpireTimer( Caster, duration );
				int procChance = (int) ( ( chivalry - 20.0 ) / 0.6 );
				int bonusDamage = (int) ( ( chivalry - 90.0 ) / 2.0 );

				Utility.FixMinMax( ref procChance, 0, 100 );
				Utility.FixMinMax( ref bonusDamage, 0, 15 );

				BuffInfo.AddBuff( Caster, new BuffInfo( BuffIcon.ConsecrateWeapon, 1151385, 1151386, duration, Caster, String.Format( "{0}\t{1}", procChance, bonusDamage ) ) );

				ConsecrateContext context = new ConsecrateContext( expireTimer, procChance, bonusDamage );

				m_Table[Caster] = context;

				expireTimer.Start();
			}

			FinishSequence();
		}

		private static Dictionary<Mobile, ConsecrateContext> m_Table = new Dictionary<Mobile, ConsecrateContext>();

		public static ConsecrateContext GetContext( Mobile m )
		{
			if ( !m_Table.ContainsKey( m ) )
				return null;

			return m_Table[m];
		}

		public static bool UnderEffect( Mobile m )
		{
			return GetContext( m ) != null;
		}

		public static void RemoveEffect( Mobile m )
		{
			if ( m_Table.ContainsKey( m ) )
			{
				Timer t = m_Table[m].Timer;

				if ( t != null )
					t.Stop();

				m_Table.Remove( m );

				BuffInfo.RemoveBuff( m, BuffIcon.ConsecrateWeapon );
			}
		}

		public static void AlterDamageTypes( Mobile attacker, Mobile defender, ref int phys, ref int fire, ref int cold, ref int pois, ref int nrgy, ref int chao )
		{
			ConsecrateContext context = GetContext( attacker );

			if ( context != null && context.ProcChance > Utility.Random( 100 ) )
			{
				phys = defender.PhysicalResistance;
				fire = defender.FireResistance;
				cold = defender.ColdResistance;
				pois = defender.PoisonResistance;
				nrgy = defender.EnergyResistance;
				chao = 0;

				int low = phys, type = 0;

				if ( fire < low )
				{
					low = fire;
					type = 1;
				}
				if ( cold < low )
				{
					low = cold;
					type = 2;
				}
				if ( pois < low )
				{
					low = pois;
					type = 3;
				}
				if ( nrgy < low )
				{
					low = nrgy;
					type = 4;
				}

				phys = fire = cold = pois = nrgy = 0;

				if ( type == 0 )
					phys = 100;
				else if ( type == 1 )
					fire = 100;
				else if ( type == 2 )
					cold = 100;
				else if ( type == 3 )
					pois = 100;
				else if ( type == 4 )
					nrgy = 100;
			}
		}

		private class ExpireTimer : Timer
		{
			private Mobile m_Owner;

			public ExpireTimer( Mobile owner, TimeSpan delay )
				: base( delay )
			{
				m_Owner = owner;
			}

			protected override void OnTick()
			{
				Effects.PlaySound( m_Owner.Location, m_Owner.Map, 0x1F8 );

				RemoveEffect( m_Owner );
			}
		}
	}

	public class ConsecrateContext
	{
		private Timer m_Timer;
		private int m_ProcChance;
		private int m_BonusDamage;

		public Timer Timer { get { return m_Timer; } }
		public int ProcChance { get { return m_ProcChance; } }
		public int BonusDamage { get { return m_BonusDamage; } }

		public ConsecrateContext( Timer timer, int procChance, int bonusDamage )
		{
			m_Timer = timer;
			m_ProcChance = procChance;
			m_BonusDamage = bonusDamage;
		}
	}
}