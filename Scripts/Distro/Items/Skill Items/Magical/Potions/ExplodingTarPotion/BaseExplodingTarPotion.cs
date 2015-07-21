using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using Server;
using Server.Engines.BuffIcons;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;

namespace Server.Items
{
	public abstract class BaseExplodingTarPotion : BasePotion
	{
		private const int ExplosionRange = 2; // How long is the blast radius?

		public BaseExplodingTarPotion( PotionEffect effect )
			: base( 0xF0D, effect )
		{
			Hue = 0x455;
		}

		public BaseExplodingTarPotion( Serial serial )
			: base( serial )
		{
		}

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

		public override TimeSpan GetNextDrinkTime( Mobile from )
		{
			if ( from is PlayerMobile )
				return ( (PlayerMobile) from ).NextDrinkExplodingTarPotion;

			return TimeSpan.Zero;
		}

		public override void SetNextDrinkTime( Mobile from )
		{
			if ( from is PlayerMobile )
				( (PlayerMobile) from ).NextDrinkExplodingTarPotion = TimeSpan.FromMinutes( 2.0 );
		}

		public override void Drink( Mobile from )
		{
			ThrowTarget targ = from.Target as ThrowTarget;

			if ( targ != null && targ.Potion == this )
				return;

			from.RevealingAction();

			from.Target = new ThrowTarget( this );
		}

		private class ThrowTarget : Target
		{
			private BaseExplodingTarPotion m_Potion;

			public BaseExplodingTarPotion Potion
			{
				get { return m_Potion; }
			}

			public ThrowTarget( BaseExplodingTarPotion potion )
				: base( 12, true, TargetFlags.None )
			{
				m_Potion = potion;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				if ( m_Potion.Deleted || m_Potion.Map == Map.Internal )
					return;

				IPoint3D p = targeted as IPoint3D;

				if ( p == null )
					return;

				Map map = from.Map;

				if ( map == null )
					return;

				SpellHelper.GetSurfaceTop( ref p );

				from.RevealingAction();

				IEntity to = new DummyEntity( Serial.Zero, new Point3D( p ), map );

				Effects.SendMovingEffect( from, to, m_Potion.ItemID, 7, 0, false, false, m_Potion.Hue, 0 );

				Timer.DelayCall( TimeSpan.FromSeconds( 1.0 ), new TimerStateCallback( ExplodingTarPotionEffect_Callback ), new object[] { p, from, map } );

				m_Potion.SetNextDrinkTime( from );
			}

			public virtual void ExplodingTarPotionEffect_Callback( object state )
			{
				object[] states = (object[]) state;
				Point3D p = new Point3D( (IPoint3D) states[0] );
				Mobile from = (Mobile) states[1];
				Map map = (Map) states[2];

				var eable = map.GetMobilesInRange( p, ExplosionRange );

				int toAffect = 0;
				List<Mobile> toSleep = new List<Mobile>();

				foreach ( Mobile m in eable )
				{
					if ( m.Alive && m != from && Notoriety.Compute( from, m ) != Notoriety.Innocent )
					{
						toSleep.Add( m );
						++toAffect;
					}
				}


				if ( toAffect > 0 )
				{
					Effects.PlaySound( p, map, 0x207 );
					Effects.SendLocationParticles( EffectItem.Create( new Point3D( p.X, p.Y, p.Z ), from.Map, EffectItem.DefaultDuration ), 0x36B0, 1, 14, 0x455, 0x7, 0x26BB, 0 );

					foreach ( Mobile m in toSleep )
					{
						if ( CheckSleep( from, m ) )
						{
							SpellHelper.Turn( from, m );
							DoSleep( from, m );
						}
					}
				}
				else
				{
					if ( from is PlayerMobile )
						( (PlayerMobile) from ).NextDrinkExplodingTarPotion = TimeSpan.Zero;
				}

				m_Potion.Consume();
			}
		}

		public static bool CheckSleep( Mobile from, Mobile m )
		{
			if ( IsSlept( m ) )
			{
				from.SendLocalizedMessage( 1080134 ); // Your target is already immobilized and cannot be slept.
				return false;
			}
			else if ( !m.Alive )
			{
				from.SendLocalizedMessage( 1080135 ); // Your target cannot be put to sleep
				return false;
			}

			return true;
		}

		private static Dictionary<Mobile, Timer> m_SleptTable = new Dictionary<Mobile, Timer>();

		public static bool IsSlept( Mobile m )
		{
			return m_SleptTable.ContainsKey( m );
		}

		public static void DoSleep( Mobile from, Mobile m )
		{
			double alchemy = from.Skills.Alchemy.Value;
			double resist = m.Skills.MagicResist.Value;

			double enhancePotions = GetEnhancePotions( from );

			int seconds = (int) ( alchemy + ( enhancePotions / 2 ) - ( resist + 40 ) ) / 10;

			if ( seconds > 0 )
			{
				BuffInfo.AddBuff( m, new BuffInfo( BuffIcon.Paralyze, 1095150, 1095151, TimeSpan.FromSeconds( seconds ), m, seconds.ToString() ) );
				Timer t = new InternalTimer( m, DateTime.Now + TimeSpan.FromSeconds( seconds ) );
				t.Start();

				m_SleptTable[m] = t;

				m.ForcedWalk = true;
			}
			else
			{
				from.SendLocalizedMessage( 1095149 ); // Your target resists the exploding tar potion.
				m.SendLocalizedMessage( 1095148 ); // You resist the exploding tar potion.
			}
		}

		private class InternalTimer : Timer
		{
			private Mobile m_Target;
			private DateTime m_End;

			public InternalTimer( Mobile target, DateTime end )
				: base( TimeSpan.Zero, TimeSpan.FromSeconds( 0.5 ) )
			{

				m_Target = target;
				m_End = end;
			}

			protected override void OnTick()
			{
				if ( m_Target.Deleted || !m_Target.Alive || DateTime.Now > m_End )
				{
					RemoveEffect( m_Target );
				}
				else
				{
					Effects.SendTargetParticles( m_Target, 0x3779, 1, 32, 0x13BA, EffectLayer.Head );

					/* OSI manda un paquete 0xDE aquí, probablemente para actualizar la
					 * información de los Buff Icons. ¿Realmente es necesario?
					 */
				}
			}
		}

		public static void RemoveEffect( Mobile m )
		{
			if ( !m_SleptTable.ContainsKey( m ) )
				return;

			Timer t = m_SleptTable[m];
			t.Stop();

			m_SleptTable.Remove( m );

			BuffInfo.RemoveBuff( m, BuffIcon.Paralyze );
			m.ForcedWalk = false;
		}
	}
}
