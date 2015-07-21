using System;
using System.Collections;
using Server.Items;

namespace Server.Mobiles
{
	public class BaseThrowableSE
	{
		protected Mobile m_Mobile;

		private Type m_Object;

		private int m_ThrowRange;

		public BaseThrowableSE( Mobile from, int range, Type obj )
		{
			m_Mobile = from;
			m_ThrowRange = range;
			m_Object = obj;
		}

		public void ThrowIt()
		{
			Mobile target = BaseAttackHelperSE.GetRandomAttacker( m_Mobile, m_ThrowRange );

			if ( target != null )
			{
				Throw( target );
			}
		}

		private void SpawnItem( Point3D loc, Map map, bool isEffect )
		{
			Item item = null;

			if ( m_Object == null )
			{
				return;
			}

			if ( !isEffect && Utility.RandomDouble() > 0.1 )
			{
				return;
			}

			try
			{
				item = Activator.CreateInstance( m_Object ) as Item;
			}
			catch
			{
			}

			if ( item == null )
			{
				return;
			}

			if ( isEffect )
			{
				DoEffect( null, item );
				item.Delete();
			}
			else
			{
				item.MoveToWorld( loc, map );
			}
		}

		private void Throw( Mobile m )
		{
			Direction to = m_Mobile.GetDirectionTo( m );

			m_Mobile.Direction = to;

			if ( Utility.RandomDouble() >= ( Math.Sqrt( m.Dex / 100.0 ) * 0.8 ) )
			{
				m_Mobile.DoHarmful( m );

				DoEffect( m, null );

				DoDamage( m );

				SpawnItem( m.Location, m.Map, false );
			}
			else
			{
				int x = 0, y = 0;

				switch ( to & Direction.Mask )
				{
					case Direction.North:
						--y;
						break;
					case Direction.South:
						++y;
						break;
					case Direction.West:
						--x;
						break;
					case Direction.East:
						++x;
						break;
					case Direction.Up:
						--x;
						--y;
						break;
					case Direction.Down:
						++x;
						++y;
						break;
					case Direction.Left:
						--x;
						++y;
						break;
					case Direction.Right:
						++x;
						--y;
						break;
				}

				x += Utility.Random( -1, 3 );
				y += Utility.Random( -1, 3 );

				x += m.X;
				y += m.Y;

				SpawnItem( new Point3D( x, y, m.Z ), m.Map, true );
			}
		}

		public virtual void DoEffect( Mobile m, Item i )
		{
			return;
		}

		public virtual void DoDamage( Mobile m )
		{
			AOS.Damage( m, m_Mobile, GetDamage(), 100, 0, 0, 0, 0 );
		}

		public virtual int GetDamage()
		{
			return 0;
		}
	}

	public class ThrowingTessenSE : BaseThrowableSE
	{
		public ThrowingTessenSE( Mobile from )
			: base( from, FanDancer.AbilityRange, typeof( Tessen ) )
		{
		}

		public override void DoEffect( Mobile m, Item i )
		{
			if ( m != null )
			{
				m_Mobile.MovingEffect( m, 0x27A3, 12, 1, false, false, 0x481, 0 );
			}
			else
			{
				m_Mobile.MovingEffect( i, 0x27A3, 12, 1, false, false, 0x481, 0 );
			}
		}

		public override int GetDamage()
		{
			return Utility.Random( 5, m_Mobile.Str / 7 );
		}
	}

	public class ThrowingDaggerSE : BaseThrowableSE
	{
		public ThrowingDaggerSE( Mobile from )
			: base( from, 12, typeof( ThrowingDagger ) )
		{
		}

		public override void DoEffect( Mobile m, Item i )
		{
			if ( m != null )
			{
				m_Mobile.MovingEffect( m, 0x1BFE, 8, 1, false, false, 0x481, 0 );
			}
			else
			{
				m_Mobile.MovingEffect( i, 0x1BFE, 8, 1, false, false, 0x481, 0 );
			}
		}

		public override int GetDamage()
		{
			return Utility.Random( 5, m_Mobile.Str / 10 );
			;
		}
	}

	public class ThrowingSnowballSE : BaseThrowableSE
	{
		public ThrowingSnowballSE( Mobile from )
			: base( from, LadyOfTheSnow.AbilityRange, null )
		{
		}

		public override void DoEffect( Mobile m, Item i )
		{
			if ( m != null )
			{
				m_Mobile.MovingEffect( m, 0x2808, 9, 1, false, false, 0x481, 0 );
			}
			else
			{
				m_Mobile.MovingEffect( i, 0x2808, 9, 1, false, false, 0x481, 0 );
			}
		}

		public override int GetDamage()
		{
			return Utility.Random( 15, m_Mobile.Str / 8 );
		}
	}

	public class PoisonSpitSE : BaseThrowableSE
	{
		public PoisonSpitSE( Mobile from )
			: base( from, DeathWatchBeetle.AbilityRange, null )
		{
		}

		public override void DoEffect( Mobile m, Item i )
		{
			if ( m != null )
			{
				m_Mobile.MovingParticles( m, 0x36D4, 5, 0, false, false, 0x3f, 0, 0x1F73, 1, 0, 0x100 );
			}
			else
			{
				m_Mobile.MovingParticles( i, 0x36D4, 5, 0, false, false, 0x3f, 0, 0x1F73, 1, 0, 0x100 );
			}
		}

		public override void DoDamage( Mobile m )
		{
			if ( m.IsPlayer )
			{
				m.SendLocalizedMessage( 1070821, m_Mobile.Name ); // ~1_CREATURE~ spits a poisonous substance at you!
			}

			m.FixedParticles( 0x374A, 10, 15, 5021, EffectLayer.Waist );
			m.PlaySound( 0x474 );
			m.ApplyPoison( m_Mobile, Poison.GetPoison( 2 + Utility.Random( 3 ) ) );
		}
	}

	public class ThrowingShurikenSE : BaseThrowableSE
	{
		public ThrowingShurikenSE( Mobile from )
			: base( from, EliteNinja.AbilityRange, null )
		{
		}

		public override void DoEffect( Mobile m, Item i )
		{
			if ( m_Mobile.Body == 0x190 || m_Mobile.Body == 0x191 )
			{
				m_Mobile.Animate( m_Mobile.Mounted ? 26 : 9, 7, 1, true, false, 0 );
			}

			if ( m != null )
			{
				m_Mobile.MovingEffect( m, 0x27AC, 7, 1, false, false, 0x23A, 0 );
			}
			else
			{
				m_Mobile.MovingEffect( i, 0x27AC, 7, 1, false, false, 0x23A, 0 );
			}
		}

		public override void DoDamage( Mobile m )
		{
			AOS.Damage( m, m_Mobile, Utility.Random( 5, m_Mobile.Str / 10 ), 100, 0, 0, 0, 0 );

			if ( Utility.RandomBool() )
			{
				m.ApplyPoison( m_Mobile, Poison.GetPoison( 2 + Utility.Random( 3 ) ) );
			}
		}
	}
}