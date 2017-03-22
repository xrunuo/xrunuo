using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server
{
	public class AOS
	{
		private static bool m_ArmorIgnore;

		public static bool ArmorIgnore
		{
			get { return m_ArmorIgnore; }
			set { m_ArmorIgnore = value; }
		}

		private static bool m_ArmorPierce;

		public static bool ArmorPierce
		{
			get { return m_ArmorPierce; }
			set { m_ArmorPierce = value; }
		}

		public static int Damage( Mobile m, Mobile from, int damage, int phys, int fire, int cold, int pois, int nrgy )
		{
			return Damage( m, from, damage, phys, fire, cold, pois, nrgy, false );
		}

		public static int Damage( Mobile m, Mobile from, int damage, int phys, int fire, int cold, int pois, int nrgy, int chao )
		{
			return Damage( m, from, damage, phys, fire, cold, pois, nrgy, chao, false );
		}

		public static int Damage( Mobile m, Mobile from, int damage, int phys, int fire, int cold, int pois, int nrgy, bool keepAlive )
		{
			return Damage( false, m, from, damage, phys, fire, cold, pois, nrgy, 0, keepAlive );
		}

		public static int Damage( Mobile m, Mobile from, int damage, int phys, int fire, int cold, int pois, int nrgy, int chao, bool keepAlive )
		{
			return Damage( false, m, from, damage, phys, fire, cold, pois, nrgy, chao, keepAlive );
		}

		public static int Damage( bool directDamage, Mobile m, Mobile from, int damage, int phys, int fire, int cold, int pois, int nrgy )
		{
			return Damage( directDamage, m, from, damage, phys, fire, cold, pois, nrgy, 0 );
		}

		public static int Damage( bool directDamage, Mobile m, Mobile from, int damage, int phys, int fire, int cold, int pois, int nrgy, int chao )
		{
			return Damage( directDamage, m, from, damage, phys, fire, cold, pois, nrgy, chao, false );
		}

		public static int Damage( bool directDamage, Mobile m, Mobile from, int damage, int phys, int fire, int cold, int pois, int nrgy, bool keepAlive )
		{
			return Damage( directDamage, m, from, damage, phys, fire, cold, pois, nrgy, 0, keepAlive );
		}

		public static int Damage( bool directDamage, Mobile m, Mobile from, int damage, int phys, int fire, int cold, int pois, int nrgy, int chao, bool keepAlive )
		{
			if ( m == null || m.Deleted || !m.Alive || damage <= 0 )
				return 0;

			if ( phys == 0 && fire == 100 && cold == 0 && pois == 0 && nrgy == 0 )
				Mobiles.MeerMage.StopEffect( m, true );

			Fix( ref phys );
			Fix( ref fire );
			Fix( ref cold );
			Fix( ref pois );
			Fix( ref nrgy );
			Fix( ref chao );

			switch ( Utility.RandomMinMax( 1, 5 ) )
			{
				case 1: phys += chao; break;
				case 2: fire += chao; break;
				case 3: cold += chao; break;
				case 4: pois += chao; break;
				case 5: nrgy += chao; break;
			}

			chao = 0;

			int resPhys = m.PhysicalResistance;
			int resFire = m.FireResistance;
			int resCold = m.ColdResistance;
			int resPois = m.PoisonResistance;
			int resNrgy = m.EnergyResistance;

			if ( m_ArmorIgnore )
			{
				resPhys = fire = cold = pois = nrgy = 0;
				phys = 100;
			}

			if ( m_ArmorPierce )
			{
				double delta = 0.6; // is this correct?

				resPhys -= (int) ( delta * resPhys );
			}

			int totalDamage;

			totalDamage = damage * phys * ( 100 - resPhys );
			totalDamage += damage * fire * ( 100 - resFire );
			totalDamage += damage * cold * ( 100 - resCold );
			totalDamage += damage * pois * ( 100 - resPois );
			totalDamage += damage * nrgy * ( 100 - resNrgy );

			totalDamage /= 10000;

			if ( totalDamage < 1 )
				totalDamage = 1;

			int absorbed;

			#region Damage Eater
			int dmgEater = Math.Min( (int) AbsorptionAttributes.GetValue( m, AbsorptionAttribute.DamageEater ), 18 );

			int physEater = dmgEater;
			int fireEater = dmgEater;
			int coldEater = dmgEater;
			int poisEater = dmgEater;
			int nrgyEater = dmgEater;

			if ( !m_ArmorIgnore )
			{
				physEater = Math.Min( Math.Max( AbsorptionAttributes.GetValue( m, AbsorptionAttribute.KineticEater ), dmgEater ), 30 );
				fireEater = Math.Min( Math.Max( AbsorptionAttributes.GetValue( m, AbsorptionAttribute.FireEater ), dmgEater ), 30 );
				coldEater = Math.Min( Math.Max( AbsorptionAttributes.GetValue( m, AbsorptionAttribute.ColdEater ), dmgEater ), 30 );
				poisEater = Math.Min( Math.Max( AbsorptionAttributes.GetValue( m, AbsorptionAttribute.PoisonEater ), dmgEater ), 30 );
				nrgyEater = Math.Min( Math.Max( AbsorptionAttributes.GetValue( m, AbsorptionAttribute.EnergyEater ), dmgEater ), 30 );
			}

			absorbed = damage * phys * ( 100 - resPhys ) * physEater;
			absorbed += damage * fire * ( 100 - resFire ) * fireEater;
			absorbed += damage * cold * ( 100 - resCold ) * coldEater;
			absorbed += damage * pois * ( 100 - resPois ) * poisEater;
			absorbed += damage * nrgy * ( 100 - resNrgy ) * nrgyEater;

			absorbed /= 1000000;

			if ( m is PlayerMobile )
				( (PlayerMobile) m ).EatDamage( absorbed );
			#endregion

			if ( !m_ArmorIgnore )
			{
				#region Resonance
				if ( m.Spell is Spell && m.Spell.IsCasting )
				{
					for ( int i = 0; i < 5; i++ )
					{
						double chance = 0.0;

						switch ( i )
						{
							case 0: chance = (double) AbsorptionAttributes.GetValue( m, AbsorptionAttribute.KineticResonance ) / phys; break;
							case 1: chance = (double) AbsorptionAttributes.GetValue( m, AbsorptionAttribute.FireResonance ) / fire; break;
							case 2: chance = (double) AbsorptionAttributes.GetValue( m, AbsorptionAttribute.ColdResonance ) / cold; break;
							case 3: chance = (double) AbsorptionAttributes.GetValue( m, AbsorptionAttribute.PoisonResonance ) / pois; break;
							case 4: chance = (double) AbsorptionAttributes.GetValue( m, AbsorptionAttribute.EnergyResonance ) / nrgy; break;
						}

						if ( chance > Utility.RandomDouble() )
						{
							( (Spell) m.Spell ).Resonates = true;
							break;
						}
					}
				}
				#endregion
			}

			#region Dragon Barding
			if ( ( from == null || !from.Player ) && m.Player && m.Mount is SwampDragon )
			{
				SwampDragon pet = m.Mount as SwampDragon;

				if ( pet != null && pet.HasBarding )
				{
					int percent = ( pet.BardingExceptional ? 20 : 10 );
					absorbed = Scale( totalDamage, percent );

					totalDamage -= absorbed;
					if ( !( pet is ParoxysmusSwampDragon ) )
						pet.BardingHP -= absorbed;

					if ( pet.BardingHP < 0 )
					{
						pet.HasBarding = false;
						pet.BardingHP = 0;

						m.SendLocalizedMessage( 1053031 ); // Your dragon's barding has been destroyed!
					}
				}
			}
			#endregion

			if ( keepAlive && totalDamage > m.Hits )
				totalDamage = m.Hits;

			if ( from != null )
			{
				int reflectPhys = m.GetMagicalAttribute( AosAttribute.ReflectPhysical );

				if ( reflectPhys != 0 )
				{
					if ( from is ExodusMinion && ( (ExodusMinion) from ).FieldActive || from is ExodusOverseer && ( (ExodusOverseer) from ).FieldActive )
					{
						from.FixedParticles( 0x376A, 20, 10, 0x2530, EffectLayer.Waist );
						from.PlaySound( 0x2F4 );
						m.SendAsciiMessage( "Your weapon cannot penetrate the creature's magical barrier" );
					}
					else
					{
						int rpd_damage = Scale( ( damage * phys * ( 100 - from.PhysicalResistance ) ) / 10000, reflectPhys );
						if ( m is PlayerMobile && from is PlayerMobile && directDamage )
							rpd_damage = Math.Min( rpd_damage, 35 );
						from.Damage( rpd_damage, m );
					}
				}
			}

			if ( from is BaseCreature )
			{
				Mobile master = ( (BaseCreature) from ).ControlMaster;

				if ( master != null )
					master.RevealingAction();

				#region Talismans
				BaseTalisman talis = BaseTalisman.GetTalisman( m );

				if ( talis != null && talis.ProtectionTalis != NPC_Name.None && talis.ProtectionValue > 0 )
				{
					if ( ProtectionKillerEntry.IsProtectionKiller( talis.ProtectionTalis, from ) )
						totalDamage = totalDamage - ( ( totalDamage * talis.ProtectionValue ) / 100 );
				}
				#endregion
			}

			if ( from != null )
			{
				Item cloak = from.FindItemOnLayer( Layer.Cloak );
				Item twohanded = from.FindItemOnLayer( Layer.TwoHanded );

				if ( directDamage && cloak is BaseQuiver && twohanded is BaseRanged )
				{
					BaseQuiver quiver = cloak as BaseQuiver;
					totalDamage += (int) ( totalDamage * quiver.DamageModifier * 0.01 );
				}
			}

			if ( m is PlayerMobile && from is PlayerMobile && directDamage )
				totalDamage = Math.Min( totalDamage, 35 );

			/* TODO: El efecto del Attunement deber�a ir despues de aplicar el Blood Oath.
			 * Seg�n parece, en OSI los Necro-Arcanists lo usan mucho en PvP, de manera que
			 * el da�o recibido se refleja al atacante por Blood Oath, pero luego es
			 * absorbido por el Attunement en el defensor.
			 */
			if ( directDamage )
				Spells.Spellweaving.AttunementSpell.TryAbsorb( m, ref totalDamage );

			if ( from != null )
				SpellHelper.DoLeech( totalDamage, from, m );

			m.Damage( totalDamage, from );

			return totalDamage;
		}

		public static int Damage( Mobile m, int damage, int phys, int fire, int cold, int pois, int nrgy )
		{
			return Damage( m, null, damage, phys, fire, cold, pois, nrgy );
		}

		public static void Fix( ref int val )
		{
			if ( val < 0 )
				val = 0;
		}

		public static int Scale( int input, int percent )
		{
			return ( input * percent ) / 100;
		}
	}
}