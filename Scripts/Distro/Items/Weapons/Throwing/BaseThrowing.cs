using System;
using Server.Network;
using Server.Spells;
using Server.Mobiles;
using Server.Engines.Imbuing;

namespace Server.Items
{
	public abstract class BaseThrowing : BaseMeleeWeapon
	{
		private static readonly ILog log = LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

		public override int HitSound { get { return 0x5D2; } }
		public override int MissSound { get { return 0x5D3; } }

		public override Race RequiredRace { get { return Race.Gargoyle; } }

		public override SkillName Skill { get { return SkillName.Throwing; } }
		public override WeaponType Type { get { return WeaponType.Ranged; } }

		public override WeaponAnimation Animation { get { return WeaponAnimation.Wrestle; } }

		public BaseThrowing( int itemID )
			: base( itemID )
		{
		}

		public BaseThrowing( Serial serial )
			: base( serial )
		{
		}

		public override TimeSpan OnSwing( Mobile attacker, Mobile defender )
		{
			// Make sure we've been standing still for 0.5 seconds
			if ( DateTime.UtcNow > ( attacker.LastMoveTime + TimeSpan.FromSeconds( 0.5 ) ) || ( WeaponAbility.GetCurrentAbility( attacker ) is MovingShot ) )
			{
				if ( CanSwing( attacker ) && attacker.HarmfulCheck( defender ) )
				{
					attacker.DisruptiveAction();
					attacker.Send( new Swing( 0, attacker, defender ) );

					Effects.SendPacket( attacker, attacker.Map, new GraphicalEffect( EffectType.Moving, attacker.Serial, defender.Serial, ItemID, attacker.Location, defender.Location, 18, 0, false, 2 ) );

					if ( CheckHit( attacker, defender ) )
						OnHit( attacker, defender );
					else
						OnMiss( attacker, defender );
				}

				return GetDelay( attacker );
			}
			else
			{
				return TimeSpan.FromSeconds( 0.5 + ( 0.25 * Utility.RandomDouble() ) );
			}
		}

		private int GetAlteredMaxRange()
		{
			int baseRange = MaxRange;

			var attacker = Parent as Mobile;
			if ( attacker != null )
			{
				/*
				 * Each weapon has a base and max range available to it, where the base
				 * range is modified by the player’s strength to determine the actual range.
				 *
				 * Determining the maximum range of each weapon while in use:
				 * - Range = BaseRange + ((PlayerStrength - MinWeaponStrReq) / ((150 - MinWeaponStrReq) / 3))
				 * - The absolute maximum range is capped at 11 tiles.
				 *
				 * As per OSI tests: with 140 Strength you achieve max range for all throwing weapons.
				 */

				return ( baseRange - 3 ) + ( ( attacker.Str - StrengthReq ) / ( ( 140 - StrengthReq ) / 3 ) );
			}
			else
			{
				return baseRange;
			}
		}

		public override int GetMaxRange( Mobile attacker, Mobile defender )
		{
			return GetAlteredMaxRange();
		}

		private static double[][] m_AccuracyPenalties = new double[][]
			{
				new double[] { 0.31, 0.31, 0.00, 0.00, 0.00 },
				new double[] { 0.40, 0.40, 0.03, 0.00, 0.00, 0.00 },
				new double[] { 0.47, 0.47, 0.12, 0.00, 0.00, 0.00, 0.00 },
				new double[] { 0.52, 0.52, 0.19, 0.00, 0.00, 0.00, 0.00, 0.00 },
				new double[] { 0.56, 0.56, 0.26, 0.03, 0.00, 0.00, 0.00, 0.00, 0.00 },
				new double[] { 0.59, 0.59, 0.31, 0.09, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00 },
				new double[] { 0.62, 0.62, 0.36, 0.14, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00 },
				new double[] { 0.64, 0.64, 0.40, 0.19, 0.03, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00 }
			};

		public override double GetAccuracyScalar( Mobile attacker, Mobile defender, int maxRange )
		{
			int dist = (int) Math.Min( attacker.GetDistanceToSqrt( defender ), maxRange );
			double scalar = 0.0;
			try
			{
				//TODO - Revisar, da un index fuera de rango a veces
				scalar = m_AccuracyPenalties[maxRange - 4][dist];
			}
			catch ( Exception ex )
			{
				log.Error( "Exception in Throwing code, maxRange={0}, dist={1}, attacker={2}, defender={3}, exception={4}", maxRange, dist, attacker, defender, ex );
			}

			if ( dist < 2 )
				scalar *= 1.0 - Math.Min( 1.0, ( ( attacker.Skills[SkillName.Swords].Value / 240.0 ) + ( attacker.Dex / 300.0 ) ) );

			if ( m_Debug )
				attacker.SendMessage( "Your hit chance scalar due to distance is x{0:0.00}", 1.0 - scalar );

			return 1.0 - scalar;
		}

		private static double[][] m_DamagePenalties = new double[][]
			{
				new double[] { 0.00, 0.00, 0.00, 0.00, 0.31 },
				new double[] { 0.00, 0.00, 0.00, 0.00, 0.03, 0.40 },
				new double[] { 0.00, 0.00, 0.00, 0.00, 0.00, 0.12, 0.47 },
				new double[] { 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.19, 0.52 },
				new double[] { 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.03, 0.26, 0.56 },
				new double[] { 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.09, 0.31, 0.59 },
				new double[] { 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.14, 0.36, 0.62 },
				new double[] { 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.03, 0.19, 0.40, 0.64 }
			};

		public override double GetDamageScalar( Mobile attacker, Mobile defender, int maxRange )
		{
			int dist = (int) Math.Min( attacker.GetDistanceToSqrt( defender ), maxRange );
			double scalar = 0.0;
			try
			{
				scalar = m_DamagePenalties[maxRange - 4][dist];
				//TODO - Revisar, da un index fuera de rango a veces
			}
			catch ( Exception ex )
			{
				log.Error( "Exception in Throwing code, maxRange={0}, dist={1}, attacker={2}, defender={3}, exception={4}", maxRange, dist, attacker, defender, ex );
			}

			if ( m_Debug )
				attacker.SendMessage( "Your damage scalar due to distance is x{0:0.00}", 1.0 - scalar );

			return 1.0 - scalar;
		}

		public static void ApplyShieldPenalties( Mobile attacker, Mobile defender, ref double chance )
		{
			if ( attacker.Weapon is BaseThrowing && attacker.FindItemOnLayer( Layer.TwoHanded ) is BaseShield )
			{
				double malus = ( 100.0 - ( ( attacker.Skills[SkillName.Parry].Value / 80.0 ) * 40.0 ) ) / 100.0;

				chance -= chance * malus;

				if ( ( (BaseThrowing) attacker.Weapon ).Debug )
					attacker.SendMessage( "Your hit chance scalar for wearing a shield is x{0:0.00}", 1.0 - malus );
			}

			if ( defender.Weapon is BaseThrowing && defender.FindItemOnLayer( Layer.TwoHanded ) is BaseShield )
			{
				double malus = ( 100.0 - ( ( attacker.Skills[SkillName.Parry].Value / 100.0 ) * 60.0 ) ) / 100.0;

				chance += ( 1.0 - chance ) * malus;

				if ( ( (BaseThrowing) defender.Weapon ).Debug )
					defender.SendMessage( "Your defense chance scalar for wearing a shield is x{0:0.00}", 1.0 - malus );
			}
		}

		public override void AddRangeProperty( ObjectPropertyList list )
		{
			int maxRange = GetAlteredMaxRange();

			if ( maxRange > 1 )
				list.Add( 1061169, maxRange.ToString() ); // range ~1_val~
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			if ( m_Debug )
				list.Add( "<center><basefont color=#009900>(Debug)</basefont></center>" );
		}

		private bool m_Debug;

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Debug
		{
			get { return m_Debug; }
			set
			{
				m_Debug = value;
				InvalidateProperties();
			}
		}

		public override ImbuingFlag ImbuingFlags { get { return ImbuingFlag.Throwing; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}
}
