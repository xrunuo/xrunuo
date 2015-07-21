using System;
using System.Collections;
using Server.Network;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Spells.Ninjitsu
{
	public class Shadowjump : NinjaSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Shadowjump", null,
				-1,
				9002
			);

		public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds( 1.0 ); } }

		public override bool BlockedByAnimalForm { get { return false; } }

		public override double RequiredSkill { get { return 50.0; } }
		public override int RequiredMana { get { return 15; } }

		public Shadowjump( Mobile caster, Item scroll )
			: base( caster, scroll, m_Info )
		{
		}

		public override bool CheckCast()
		{
			if ( !Caster.IsStealthing )
			{
				Caster.SendLocalizedMessage( 1063087 ); // You must be in stealth mode to use this ability.
				return false;
			}

			if ( !base.CheckCast() )
				return false;

			Caster.SendLocalizedMessage( 1063088 ); // You prepare to perform a Shadowjump.
			return true;
		}

		public override void OnCast()
		{
			Caster.Target = new InternalTarget( this );
		}

		public void Target( IPoint3D p )
		{
			IPoint3D orig = p;
			Map map = Caster.Map;

			SpellHelper.GetSurfaceTop( ref p );

			if ( !Caster.IsStealthing )
			{
				Caster.SendLocalizedMessage( 1063087 ); // You must be in stealth mode to use this ability.
			}
			else if ( Factions.Sigil.ExistsOn( Caster ) )
			{
				Caster.SendLocalizedMessage( 1061632 ); // You can't do that while carrying the sigil.
			}
			else if ( Server.Misc.WeightOverloading.IsOverloaded( Caster ) )
			{
				Caster.SendLocalizedMessage( 502359, "", 0x22 ); // Thou art too encumbered to move.
			}
			else if ( !SpellHelper.CheckTravel( Caster, TravelCheckType.TeleportFrom ) )
			{
			}
			else if ( !SpellHelper.CheckTravel( Caster, map, new Point3D( p ), TravelCheckType.TeleportTo ) )
			{
			}
			else if ( map == null || !map.CanSpawnMobile( p.X, p.Y, p.Z ) )
			{
				Caster.SendLocalizedMessage( 502831 ); // Cannot teleport to that spot.
			}
			else if ( SpellHelper.CheckMulti( new Point3D( p ), map ) )
			{
				Caster.SendLocalizedMessage( 502831 ); // Cannot teleport to that spot.
			}
			else if ( CheckSequence() )
			{
				SpellHelper.Turn( Caster, orig );

				Mobile m = Caster;

				Point3D from = m.Location;
				Point3D to = new Point3D( p );

				m.Location = to;
				m.ProcessDelta();

				Effects.SendLocationParticles( EffectItem.Create( from, m.Map, EffectItem.DefaultDuration ), 0x3728, 10, 10, 2023 );

				m.PlaySound( 0x512 );

				Server.SkillHandlers.Stealth.OnUse( m ); // stealth check after the a jump
			}

			FinishSequence();
		}

		public class InternalTarget : Target
		{
			private Shadowjump m_Owner;

			public InternalTarget( Shadowjump owner )
				: base( 12, true, TargetFlags.None )
			{
				m_Owner = owner;
			}

			protected override void OnTarget( Mobile from, object o )
			{
				IPoint3D p = o as IPoint3D;

				if ( p != null )
				{
					double distance = from.GetDistanceToSqrt( p );

					//					double skillvalue = from.Skills[ SkillName.Ninjitsu ].Value/10.0; // is this correct?
					double skillvalue = 11; // Xeor: tested on OSI

					if ( distance <= skillvalue )
					{
						m_Owner.Target( p );
					}
					else
					{
						from.SendLocalizedMessage( 502481 ); // That location is too far away.
					}
				}
			}

			protected override void OnTargetFinish( Mobile from )
			{
				int armorRating = SkillHandlers.Stealth.GetArmorRating( from );

				// TODO: Should stealth be gained here?
				if ( !from.CheckSkill( SkillName.Stealth, -20.0 + ( armorRating * 2 ), 80.0 + ( armorRating * 2 ) ) )
				{
					from.SendLocalizedMessage( 502731 ); // You fail in your attempt to move unnoticed.
					from.RevealingAction();
				}

				m_Owner.FinishSequence();
			}
		}
	}
}