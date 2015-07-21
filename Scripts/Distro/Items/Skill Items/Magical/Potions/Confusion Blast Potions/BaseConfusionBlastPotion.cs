using System;
using Server;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Mobiles;

namespace Server.Items
{
	public abstract class BaseConfusionBlastPotion : BasePotion
	{
		public abstract int AreaSize { get; }

		public BaseConfusionBlastPotion( PotionEffect effect )
			: base( 0xF06, effect )
		{
			Hue = 0x48D;
		}

		public BaseConfusionBlastPotion( Serial serial )
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
				return ( (PlayerMobile) from ).NextDrinkConfusionPotion;

			return TimeSpan.Zero;
		}

		public override void SetNextDrinkTime( Mobile from )
		{
			if ( from is PlayerMobile )
				( (PlayerMobile) from ).NextDrinkConfusionPotion = TimeSpan.FromMinutes( 1.0 );
		}

		public override void Drink( Mobile from )
		{
			ThrowTarget targ = from.Target as ThrowTarget;

			if ( targ != null && targ.Potion == this )
				return;

			from.RevealingAction();

			from.Target = new ThrowTarget( this );
		}

		private class OffsetEntry
		{
			private int m_X, m_Y;

			public OffsetEntry( int X, int Y )
			{
				m_X = X;
				m_Y = Y;
			}

			public int X { get { return m_X; } }
			public int Y { get { return m_Y; } }
		}

		private class ThrowTarget : Target
		{
			private static OffsetEntry[] Offsets = new OffsetEntry[]
			{
				// size = 1
				new OffsetEntry( -1,  0 ), new OffsetEntry(  0, -1 ),
				new OffsetEntry(  0,  1 ), new OffsetEntry(  1,  0 ),
				// size = 2
				new OffsetEntry( -2, -1 ), new OffsetEntry( -2,  0 ),
				new OffsetEntry( -2,  1 ), new OffsetEntry( -1, -2 ),
				new OffsetEntry( -1,  2 ), new OffsetEntry(  0, -2 ),
				new OffsetEntry(  0,  2 ), new OffsetEntry(  1, -2 ),
				new OffsetEntry(  1,  2 ), new OffsetEntry(  2, -1 ),
				new OffsetEntry(  2,  0 ), new OffsetEntry(  2,  1 ),
				// size = 3
				new OffsetEntry( -3, -1 ), new OffsetEntry( -3,  0 ),
				new OffsetEntry( -3,  1 ), new OffsetEntry( -2, -2 ),
				new OffsetEntry( -2,  2 ), new OffsetEntry( -1, -3 ),
				new OffsetEntry( -1,  3 ), new OffsetEntry(  0, -3 ),
				new OffsetEntry(  0,  3 ), new OffsetEntry(  1, -3 ),
				new OffsetEntry(  1,  3 ), new OffsetEntry(  2, -2 ),
				new OffsetEntry(  2,  2 ), new OffsetEntry(  3, -1 ),
				new OffsetEntry(  3,  0 ), new OffsetEntry(  3,  1 ),
				// size = 4
				new OffsetEntry( -4, -1 ), new OffsetEntry( -4,  0 ),
				new OffsetEntry( -4,  1 ), new OffsetEntry( -3, -3 ),
				new OffsetEntry( -3, -2 ), new OffsetEntry( -3,  2 ),
				new OffsetEntry( -3,  3 ), new OffsetEntry( -2, -3 ),
				new OffsetEntry( -2,  3 ), new OffsetEntry( -1, -4 ),
				new OffsetEntry( -1,  4 ), new OffsetEntry(  0, -4 ),
				new OffsetEntry(  0,  4 ), new OffsetEntry(  1, -4 ),
				new OffsetEntry(  1,  4 ), new OffsetEntry(  2, -3 ),
				new OffsetEntry(  2,  3 ), new OffsetEntry(  3, -3 ),
				new OffsetEntry(  3, -2 ), new OffsetEntry(  3,  2 ),
				new OffsetEntry(  3,  3 ), new OffsetEntry(  4, -1 ),
				new OffsetEntry(  4,  0 ), new OffsetEntry(  4,  1 ),
				// size = 5
				new OffsetEntry( -5, -2 ), new OffsetEntry( -5, -1 ),
				new OffsetEntry( -5,  0 ), new OffsetEntry( -5,  1 ),
				new OffsetEntry( -5,  2 ), new OffsetEntry( -4, -3 ),
				new OffsetEntry( -4,  3 ), new OffsetEntry( -3, -4 ),
				new OffsetEntry( -3,  4 ), new OffsetEntry( -2, -5 ),
				new OffsetEntry( -2,  5 ), new OffsetEntry( -1, -5 ),
				new OffsetEntry( -1,  5 ), new OffsetEntry(  0, -5 ),
				new OffsetEntry(  0,  5 ), new OffsetEntry(  1, -5 ),
				new OffsetEntry(  1,  5 ), new OffsetEntry(  2, -5 ),
				new OffsetEntry(  2,  5 ), new OffsetEntry(  3, -4 ),
				new OffsetEntry(  3,  4 ), new OffsetEntry(  4, -3 ),
				new OffsetEntry(  4,  3 ), new OffsetEntry(  5, -2 ),
				new OffsetEntry(  5, -1 ), new OffsetEntry(  5,  0 ),
				new OffsetEntry(  5,  1 ), new OffsetEntry(  5,  2 ),
				// size = 6
				new OffsetEntry( -6, -2 ), new OffsetEntry( -6, -1 ),
				new OffsetEntry( -6,  0 ), new OffsetEntry( -6,  1 ),
				new OffsetEntry( -6,  2 ), new OffsetEntry( -5, -4 ),
				new OffsetEntry( -5, -3 ), new OffsetEntry( -5,  3 ),
				new OffsetEntry( -5,  4 ), new OffsetEntry( -4, -5 ),
				new OffsetEntry( -4, -4 ), new OffsetEntry( -4,  4 ),
				new OffsetEntry( -4,  5 ), new OffsetEntry( -3, -5 ),
				new OffsetEntry( -3,  5 ), new OffsetEntry( -2, -6 ),
				new OffsetEntry( -2,  6 ), new OffsetEntry( -1, -6 ),
				new OffsetEntry( -1,  6 ), new OffsetEntry(  0, -6 ),
				new OffsetEntry(  0,  6 ), new OffsetEntry(  1, -6 ),
				new OffsetEntry(  1,  6 ), new OffsetEntry(  2, -6 ),
				new OffsetEntry(  2,  6 ), new OffsetEntry(  3, -5 ),
				new OffsetEntry(  3,  5 ), new OffsetEntry(  4, -5 ),
				new OffsetEntry(  4, -4 ), new OffsetEntry(  4,  4 ),
				new OffsetEntry(  4,  5 ), new OffsetEntry(  5, -4 ),
				new OffsetEntry(  5, -3 ), new OffsetEntry(  5,  3 ),
				new OffsetEntry(  5,  4 ), new OffsetEntry(  6, -2 ),
				new OffsetEntry(  6, -1 ), new OffsetEntry(  6,  0 ),
				new OffsetEntry(  6,  1 ), new OffsetEntry(  6,  2 ),
				// size = 7
				new OffsetEntry( -7, -2 ), new OffsetEntry( -7, -1 ),
				new OffsetEntry( -7,  0 ), new OffsetEntry( -7,  1 ),
				new OffsetEntry( -7,  2 ), new OffsetEntry( -6, -4 ),
				new OffsetEntry( -6, -3 ), new OffsetEntry( -6,  3 ),
				new OffsetEntry( -6,  4 ), new OffsetEntry( -5, -5 ),
				new OffsetEntry( -5,  5 ), new OffsetEntry( -4, -6 ),
				new OffsetEntry( -4,  6 ), new OffsetEntry( -3, -6 ),
				new OffsetEntry( -3,  6 ), new OffsetEntry( -2, -7 ),
				new OffsetEntry( -2,  7 ), new OffsetEntry( -1, -7 ),
				new OffsetEntry( -1,  7 ), new OffsetEntry(  0, -7 ),
				new OffsetEntry(  0,  7 ), new OffsetEntry(  1, -7 ),
				new OffsetEntry(  1,  7 ), new OffsetEntry(  2, -7 ),
				new OffsetEntry(  2,  7 ), new OffsetEntry(  3, -6 ),
				new OffsetEntry(  3,  6 ), new OffsetEntry(  4, -6 ),
				new OffsetEntry(  4,  6 ), new OffsetEntry(  5, -5 ),
				new OffsetEntry(  5,  5 ), new OffsetEntry(  6, -4 ),
				new OffsetEntry(  6, -3 ), new OffsetEntry(  6,  3 ),
				new OffsetEntry(  6,  4 ), new OffsetEntry(  7, -2 ),
				new OffsetEntry(  7, -1 ), new OffsetEntry(  7,  0 ),
				new OffsetEntry(  7,  1 ), new OffsetEntry(  7,  2 )
			};

			private class InternalTimer : Timer
			{
				private Point3D p;
				private int range;
				private Mobile m;

				public InternalTimer( Point3D loc, int areasize, Mobile from )
					: base( TimeSpan.FromSeconds( 10.0 ) )
				{
					p = loc;
					range = areasize;
					m = from;
				}

				protected override void OnTick()
				{
					var eable = m.Map.GetMobilesInRange( p, range );

					foreach ( object obj in eable )
					{
						if ( obj is BaseCreature )
						{
							BaseCreature mobile = obj as BaseCreature;

							if ( mobile != null && mobile.Frozen )
							{
								mobile.Frozen = false;
							}
						}
					}

				}
			}

			private BaseConfusionBlastPotion m_Potion;

			public BaseConfusionBlastPotion Potion
			{
				get { return m_Potion; }
			}

			public ThrowTarget( BaseConfusionBlastPotion potion )
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

				IEntity to;

				if ( p is Mobile )
					to = (Mobile) p;
				else
					to = new DummyEntity( Serial.Zero, new Point3D( p ), map );

				Effects.SendMovingEffect( from, to, m_Potion.ItemID, 7, 0, false, false, m_Potion.Hue, 0 );

				Effects.SendLocationParticles( EffectItem.Create( new Point3D( p.X, p.Y, p.Z ), from.Map, EffectItem.DefaultDuration ), 0x376A, 9, 4, 0, 0, 0x13AE, 0 );

				int length = 0;

				switch ( m_Potion.AreaSize )
				{
					case 1: length = 3; break;
					case 2: length = 15; break;
					case 3: length = 31; break;
					case 4: length = 55; break;
					case 5: length = 83; break;
					case 6: length = 123; break;
					case 7: length = 163; break;
				}

				for ( int i = 0; i <= length; ++i )
				{
					OffsetEntry oe = Offsets[i] as OffsetEntry;

					Effects.SendLocationParticles( EffectItem.Create( new Point3D( p.X + oe.X, p.Y + oe.Y, p.Z ), from.Map, EffectItem.DefaultDuration ), 0x376A, 9, 4, 0, 0, 0x13AE, 0 );
				}

				var eable = from.Map.GetMobilesInRange( new Point3D( p.X, p.Y, p.Z ), m_Potion.AreaSize );

				foreach ( object obj in eable )
				{
					if ( obj is BaseCreature )
					{
						BaseCreature m = obj as BaseCreature;

						if ( m != null && !m.Controlled )
						{
							m.Frozen = true;

							InternalTimer timer = new InternalTimer( new Point3D( p.X, p.Y, p.Z ), m_Potion.AreaSize, from );

							timer.Start();
						}
					}
				}


				m_Potion.SetNextDrinkTime( from );

				m_Potion.Consume();
			}
		}
	}
}