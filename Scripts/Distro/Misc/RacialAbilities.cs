using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Spells;
using Server.Spells.Necromancy;
using Server.Events;

namespace Server.Mobiles
{
	public class RacialAbilities
	{
		public static void Initialize()
		{
			EventSink.RacialAbilityRequest += new RacialAbilityRequestEventHandler( EventSink_RacialAbilityUsed );
		}

		private static void EventSink_RacialAbilityUsed( RacialAbilityRequestEventArgs e )
		{
			PlayerMobile from = e.Mobile as PlayerMobile;

			if ( from == null )
				return;

			switch ( e.AbilityID )
			{
				case 1:
					{
						/* 
						 * TODO (SA): ¿Dónde van estos CliLocs?
						 * - You must heal before flying.	1112454
						 * - You can't use this while flying!	1113414
						 * - You may not continue while flying.	1113589
						 */

						if ( from.Race == Race.Gargoyle )
						{
							if ( !from.Flying )
							{
								if ( from.Spell == null )
								{
									Spell spell = new FlySpell( from );

									spell.Cast();
								}
							}
							else if ( IsValidLandLocation( from.Location, from.Map ) )
							{
								from.Animate( 0xA );
								from.Flying = false;
							}
							else
								from.LocalOverheadMessage( MessageType.Regular, 0x3B2, 1113081 ); // You may not land here.
						}

						break;
					}
			}
		}

		public static bool IsValidLandLocation( Point3D p, Map map )
		{
			return map.CanFit( p.X, p.Y, p.Z, 16, false, false );
		}

		public static bool CheckFlyingAllowed( Mobile mob, bool message )
		{
			if ( mob.Region != null && !mob.Region.AllowFlying( mob ) )
			{
				mob.SendMessage( "You may not fly here." );
				return false;
			}

			BlockMountType type = BaseMount.GetMountPrevention( mob );

			if ( type == BlockMountType.None )
				return true;

			if ( message )
			{
				switch ( type )
				{
					case BlockMountType.Dazed:
						{
							mob.SendLocalizedMessage( 1112457 ); // You are still too dazed to fly.
							break;
						}
					case BlockMountType.BolaRecovery:
						{
							mob.SendLocalizedMessage( 1112455 ); // You cannot fly while recovering from a bola throw.
							break;
						}
					case BlockMountType.DismountRecovery:
						{
							mob.SendLocalizedMessage( 1112456 ); // You cannot fly while recovering from a dismount maneuver.
							break;
						}
				}
			}

			return false;
		}

		public class FlySpell : Spell
		{
			private static SpellInfo m_Info = new SpellInfo( "Fly", "", -1 );

			public FlySpell( Mobile caster )
				: base( caster, null, m_Info )
			{
			}

			public override bool ClearHandsOnCast { get { return false; } }
			public override bool RevealOnCast { get { return false; } }

			public override double CastDelayFastScalar { get { return 0; } }
			public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds( 1.0 ); } }

			public override int GetMana()
			{
				return 0;
			}

			public override void OnCasterHurt()
			{
				if ( IsCasting )
					Disturb( DisturbType.Hurt, false, true );
			}

			public override bool ConsumeReagents()
			{
				return true;
			}

			public override bool CheckFizzle()
			{
				return true;
			}

			public override bool CheckNextSpellTime { get { return false; } }

			public override bool CheckDisturb( DisturbType type, bool checkFirst, bool resistable )
			{
				if ( type == DisturbType.EquipRequest || type == DisturbType.UseRequest )
					return false;

				return true;
			}

			public override void SayMantra()
			{
			}

			public override void OnDisturb( DisturbType type, bool message )
			{
				if ( Caster is PlayerMobile )
					( (PlayerMobile) Caster ).Flying = false;

				if ( message )
					Caster.SendLocalizedMessage( 1113192 ); // You have been disrupted while attempting to fly!
			}

			public override bool CheckCast()
			{
				if ( !CheckFlyingAllowed( Caster, true ) )
					return false;
				else if ( !Caster.Alive )
					Caster.LocalOverheadMessage( MessageType.Regular, 0x3B2, 1113082 ); // You may not fly while dead.
				else if ( Caster.IsBodyMod && !VampiricEmbraceSpell.UnderEffect( Caster ) )
					Caster.LocalOverheadMessage( MessageType.Regular, 0x3B2, 1112453 ); // You can't fly in your current form!
				else
				{
					if ( Caster is PlayerMobile )
						( (PlayerMobile) Caster ).Flying = true;

					Caster.Animate( 0x9 );
					return true;
				}

				return false;
			}

			public override void OnCast()
			{
				FinishSequence();
			}
		}
	}
}