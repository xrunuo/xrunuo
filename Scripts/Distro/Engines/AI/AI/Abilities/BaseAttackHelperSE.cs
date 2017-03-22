using System;
using System.Collections;
using Server.Items;
using Server.Mobiles;

namespace Server
{
	public class BaseAttackHelperSE
	{
		public static Hashtable m_FanDancerRMT = new Hashtable();
		public static Hashtable m_RuneBeetleRMT = new Hashtable();
		public static Hashtable m_HiryuRMT = new Hashtable();
		public static Hashtable m_KazeKemonoRMT = new Hashtable();

		public static Hashtable m_RageTable = new Hashtable();

		public static ArrayList GetAllAttackers( Mobile m, int range )
		{
			ArrayList targets = new ArrayList();

			if ( m.Combatant != null && m.InLOS( m.Combatant ) )
			{
				if ( m.GetDistanceToSqrt( m.Combatant ) <= range )
				{
					targets.Add( m.Combatant );
				}
			}

			// Current combatant has double chance to get an attack because of the above code

			foreach ( Mobile t in m.GetMobilesInRange( range ) )
			{
				if ( t.Combatant == m && m.InLOS( t ) && !t.Hidden )
				{
					targets.Add( t );
				}
			}

			return targets;
		}

		public static Mobile GetRandomAttacker( Mobile m, int range )
		{
			ArrayList targets = GetAllAttackers( m, range );

			if ( targets.Count != 0 )
			{
				return (Mobile) targets[Utility.Random( targets.Count )];
			}
			else
			{
				return null;
			}
		}

		public static bool IsUnderEffect( Mobile target, Hashtable hashtable )
		{
			Timer timer = (Timer) hashtable[target];

			if ( timer != null )
			{
				return true;
			}

			return false;
		}

		public static void LowerResistanceAttack( Mobile from, ref ExpireTimer timer, TimeSpan duration, Mobile target, ResistanceMod[] mod, Hashtable hashtbl )
		{
			target.PlaySound( 0x1E9 );
			target.FixedParticles( 0x376A, 9, 32, 5008, EffectLayer.Waist );

			timer = new ExpireTimer( target, mod, duration, hashtbl );
			timer.Start();

			hashtbl[target] = timer;

			for ( int i = 0; i < mod.Length; i++ )
			{
				target.AddResistanceMod( mod[i] );
			}

			if ( hashtbl == m_RuneBeetleRMT )
			{
				target.SendLocalizedMessage( 1070845 ); // The creature continues to corrupt your armor!
			}

			if ( hashtbl == m_FanDancerRMT )
			{
				target.SendLocalizedMessage( 1070835 ); // The creature surrounds you with fire, reducing your resistance to fire attacks.
			}

			from.DoHarmful( target );

			return;
		}

		public static void DismountAttack( Mobile from, Mobile target )
		{
			target.Damage( 1, from );

			IMount mt = target.Mount;

			if ( mt != null )
			{
				mt.Rider = null;
			}

			if ( target.Player )
			{
				target.BeginAction( typeof( BaseMount ) );

				target.SendLocalizedMessage( 1040023 ); // You have been knocked off of your mount!

				target.EndAction( typeof( BaseMount ) );
			}

			from.DoHarmful( target );
		}

		public static void HiryuAbilitiesAttack( Mobile from, ref ExpireTimer timer )
		{
			Mobile target = GetRandomAttacker( from, 1 );

			if ( target == null )
			{
				return;
			}

			if ( Utility.RandomBool() )
			{
				if ( IsUnderEffect( target, BaseAttackHelperSE.m_HiryuRMT ) )
					return;

				ResistanceMod[] mod = new ResistanceMod[1]
				{
					new ResistanceMod( ResistanceType.Physical, -15 )
				};

				LowerResistanceAttack( from, ref timer, TimeSpan.FromSeconds( 5.0 ), target, mod, m_HiryuRMT );
			}
			else
			{
				if ( !target.Mounted )
					return;

				DismountAttack( from, target );
			}
		}

		public static void IcyWindAttack( Mobile from, Mobile target )
		{
			Effects.PlaySound( from.Location, from.Map, 0x1FB );
			Effects.PlaySound( from.Location, from.Map, 0x10B );
			Effects.SendLocationParticles( EffectItem.Create( from.Location, from.Map, EffectItem.DefaultDuration ), 0x37CC, 1, 40, 97, 3, 9917, 0 );

			target.FixedParticles( 0x374A, 1, 15, 9502, 97, 3, (EffectLayer) 255 );

			target.SendLocalizedMessage( 1070832 ); // An icy wind surrounds you, freezing your lungs as you breathe!

			int duration = 3 + Utility.Random( 3 );

			target.Paralyze( TimeSpan.FromSeconds( duration ) );

			from.DoHarmful( target );
		}

		public static void LifeforceDrainAttack( Mobile from, Mobile target )
		{
			int toDrain = 6 + (int) ( ( 120 - target.Skills[SkillName.MagicResist].Value ) / 10 );

			target.FixedParticles( 0x3789, 10, 25, 5032, EffectLayer.Head );
			target.PlaySound( 0x1F8 );

			target.SendLocalizedMessage( 1070848 ); // You feel your life force being stolen away!

			target.Damage( toDrain, from );

			from.FixedParticles( 0x376A, 9, 32, 5030, EffectLayer.Waist );
			from.PlaySound( 0x202 );

			from.Heal( toDrain );

			from.DoHarmful( target );

			return;
		}

		public static void SpillAcid( Mobile from, bool yamandon )
		{
			ArrayList targets = GetAllAttackers( from, 2 );

			if ( yamandon )
			{
				targets.Add( from );
			}

			Effects.SendLocationParticles( EffectItem.Create( from.Location, from.Map, EffectItem.DefaultDuration ), 0x3728, 8, 20, 76, 3, 5042, 0 );

			if ( targets.Count == 0 )
			{
				return;
			}

			foreach ( Mobile m in targets )
			{
				m.FixedParticles( 0x374A, 10, 15, 5021, EffectLayer.Waist );
				m.PlaySound( 0x474 );

				if ( yamandon )
				{
					m.ApplyPoison( from, Poison.GetPoison( 4 ) );
				}
				else
				{
					m.ApplyPoison( from, Poison.GetPoison( Utility.Random( 5 ) - ( from.GetDistanceToSqrt( m ) > 1 ? 1 : 0 ) ) );
				}

				m.SendLocalizedMessage( 1070820 ); // The creature spills a pool of acidic slime!
			}
		}

		public static void RageAttack( Mobile from, Mobile target, ref RageTimer timer )
		{
			target.PlaySound( 0x133 );
			target.FixedParticles( 0x377A, 244, 25, 9950, 31, 0, EffectLayer.Waist );

			target.SendLocalizedMessage( 1070825 ); // The creature goes into a rage, inflicting heavy damage!

			timer = new RageTimer( from, target );

			timer.Start();

			m_RageTable[target] = timer;
		}

		public static void CorruptArmorAttack( Mobile from, ref ExpireTimer timer, Mobile target )
		{
			ResistanceMod[] mod = new ResistanceMod[5]
			{
				new ResistanceMod( ResistanceType.Fire, -target.FireResistance / 2 ),
				new ResistanceMod( ResistanceType.Poison, -target.PoisonResistance / 2 ),
				new ResistanceMod( ResistanceType.Cold, -target.ColdResistance / 2 ),
				new ResistanceMod( ResistanceType.Energy, -target.EnergyResistance / 2 ),
				new ResistanceMod( ResistanceType.Physical, -target.PhysicalResistance / 2 )
			};

			target.SendLocalizedMessage( 1070846 ); // The creature magically corrupts your armor!

			TimeSpan duration = TimeSpan.FromSeconds( 2 + Utility.Random( 3 ) );

			LowerResistanceAttack( from, ref timer, duration, target, mod, m_RuneBeetleRMT );

			return;
		}

		public static void AngryFireAttack( Mobile from, Mobile target )
		{
			int damage = target.Hits / 2;

			AOS.Damage( target, from, damage, 0, 100, 0, 0, 0 );

			target.FixedParticles( 0x3709, 10, 30, 5052, EffectLayer.LeftFoot );
			target.PlaySound( 0x208 );

			target.SendLocalizedMessage( 1070823 ); // The creature hits you with its Angry Fire.
		}
	}
}