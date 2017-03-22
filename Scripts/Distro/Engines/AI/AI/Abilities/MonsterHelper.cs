using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Network;
using Server.Misc;
using Server.Spells;

namespace Server.Mobiles
{
	public class MonsterHelper
	{
		public static Mobile GetTopAttacker( BaseCreature bc )
		{
			List<DamageStore> rights = BaseCreature.GetLootingRights( bc.DamageEntries, bc.HitsMax );

			for ( int i = 0; i < rights.Count; i++ )
			{
				DamageStore ds = rights[i];

				if ( ds.HasRight && ds.Mobile.Alive )
					return ds.Mobile;
			}

			return null;
		}

		public static void GiveArtifactTo( Mobile m, Item artifact )
		{
			bool message = true;

			if ( !m.AddToBackpack( artifact ) )
			{
				Container bank = m.BankBox;

				if ( !( bank != null && bank.TryDropItem( m, artifact, false ) ) )
				{
					m.SendLocalizedMessage( 1072523, "", 64 ); // You find an artifact, but your backpack and bank are too full to hold it.

					message = false;

					artifact.MoveToWorld( m.Location, m.Map );
				}
			}

			if ( message )
				m.SendLocalizedMessage( 1062317, "", 64 ); // For your valor in combating the fallen beast, a special artifact has been bestowed on you.

			EffectPool.ArtifactDrop( m );
		}

		[DispellableField]
		public class FireFieldItem : Item
		{
			private Timer m_Timer;
			private DateTime m_End;
			private Mobile m_Caster;

			public override bool BlocksFit { get { return true; } }

			public FireFieldItem( Mobile caster, Point3D loc, Map map, int itemId, TimeSpan duration )
				: base( itemId )
			{
				Movable = false;
				Light = LightType.Circle300;

				MoveToWorld( loc, map );

				m_Caster = caster;

				m_End = DateTime.UtcNow + duration;

				m_Timer = new InternalTimer( this );
				m_Timer.Start();
			}

			public override void OnAfterDelete()
			{
				base.OnAfterDelete();

				if ( m_Timer != null )
					m_Timer.Stop();
			}

			public FireFieldItem( Serial serial )
				: base( serial )
			{
			}

			public override void Serialize( GenericWriter writer )
			{
				base.Serialize( writer );

				writer.Write( (int) 0 ); // version

				writer.Write( m_Caster );
				writer.WriteDeltaTime( m_End );
			}

			public override void Deserialize( GenericReader reader )
			{
				base.Deserialize( reader );

				/*int version = */
				reader.ReadInt();

				m_Caster = reader.ReadMobile();
				m_End = reader.ReadDeltaTime();

				m_Timer = new InternalTimer( this );
				m_Timer.Start();
			}

			private class InternalTimer : Timer
			{
				private readonly FireFieldItem m_Item;

				private static readonly Queue<Mobile> m_Queue = new Queue<Mobile>();

				public InternalTimer( FireFieldItem item )
					: base( TimeSpan.FromSeconds( 0.5 ), TimeSpan.FromSeconds( 1.0 ) )
				{
					m_Item = item;
				}

				protected override void OnTick()
				{
					if ( m_Item.Deleted )
					{
						return;
					}
					else if ( DateTime.UtcNow > m_Item.m_End )
					{
						m_Item.Delete();
						Stop();
					}
					else
					{
						Map map = m_Item.Map;
						Mobile caster = m_Item.m_Caster;

						if ( map != null && caster != null )
						{
							foreach ( Mobile m in m_Item.GetMobilesInRange( 0 ) )
							{
								if ( m != caster && caster.CanBeHarmful( m, false ) )
									m_Queue.Enqueue( m );
							}

							while ( m_Queue.Count > 0 )
							{
								Mobile m = m_Queue.Dequeue();

								caster.DoHarmful( m );

								AOS.Damage( m, caster, Utility.RandomMinMax( 15, 20 ), 0, 100, 0, 0, 0 );
								m.PlaySound( 0x5CF );
							}
						}
					}
				}
			}
		}

		#region Fire Wall
		private static void GetDirectionOffset( Direction d, out int dx, out int dy, out bool diagonal )
		{
			switch ( d )
			{
				default:
				case Direction.West:
					dx = -1; dy = 0; diagonal = false; break;
				case Direction.Up:
					dx = -1; dy = -1; diagonal = true; break;
				case Direction.North:
					dx = 0; dy = -1; diagonal = false; break;
				case Direction.Right:
					dx = 1; dy = -1; diagonal = true; break;
				case Direction.East:
					dx = 1; dy = 0; diagonal = false; break;
				case Direction.Down:
					dx = 1; dy = 1; diagonal = true; break;
				case Direction.South:
					dx = 0; dy = 1; diagonal = false; break;
				case Direction.Left:
					dx = -1; dy = 1; diagonal = true; break;
			}
		}

		public static void FireWall( Mobile from, Mobile target )
		{
			Effects.SendPacket( from.Location, from.Map, new FlashEffect( FlashType.LightFlash ) );
			Effects.PlaySound( from.Location, from.Map, 0x44B );

			Direction d = from.GetDirectionTo( target );

			int dx, dy;
			bool diagonal;

			GetDirectionOffset( d, out dx, out dy, out diagonal );

			int length = 1 + (int) Math.Min( from.GetDistanceToSqrt( target ), 10 );

			for ( int i = 0; i < length; i++ )
			{
				int x = from.Location.X + ( dx * i );
				int y = from.Location.Y + ( dy * i );

				Point3D loc = new Point3D( x, y, from.Location.Z );

				TimeSpan duration = TimeSpan.FromSeconds( 100.0 - ( i * 9.0 ) );

				if ( d == Direction.West || d == Direction.East || diagonal )
					new FireFieldItem( from, loc, from.Map, 0x398C, duration );

				if ( d == Direction.North || d == Direction.South || diagonal )
					new FireFieldItem( from, loc, from.Map, 0x3996, duration );
			}
		}
		#endregion

		#region Stygian Fireball
		private static Point2D[] m_UlFlamColumnOffset = new Point2D[]
			{
				/*
				 *  X
				 * X X
				 *  X
				 */
				new Point2D(  0, -1 ),
				new Point2D( -1,  0 ),
				new Point2D(  1,  0 ),
				new Point2D(  0,  1 )
			};

		private static Point2D[] m_UlFlamBallOffset = new Point2D[]
			{
				/*
				 *   XXX
				 *  X	X
				 *  X   X
				 *  X   X
				 *   XXX
				 */

				new Point2D( -1, -2 ),
				new Point2D(  0, -2 ),
				new Point2D(  1, -2 ),
				new Point2D( -2, -1 ),
				new Point2D(  2, -1 ),
				new Point2D( -2,  0 ),
				new Point2D(  2,  0 ),
				new Point2D( -2,  1 ),
				new Point2D(  2,  1 ),
				new Point2D( -1,  2 ),
				new Point2D(  0,  2 ),
				new Point2D(  1,  2 ),
			};

		public static void StygianFireball( Mobile from, Mobile target, int hue, int itemId, int phys, int fire, int cold, int pois, int nrgy )
		{
			from.PlaySound( 0x349 );

			for ( int i = 0; i < m_UlFlamColumnOffset.Length; i++ )
			{
				Point2D offset = m_UlFlamColumnOffset[i];

				Point3D effectLocation = new Point3D( from.Location.X + offset.X, from.Location.Y + offset.Y, from.Location.Z );

				Effects.SendPacket( effectLocation, from.Map,
					new LocationEffect( effectLocation, 0x3709, 10, 30, 0x5DE, 4 ) );
			}

			Timer.DelayCall( TimeSpan.FromSeconds( 0.5 ), new TimerCallback(
				delegate
				{
					from.PlaySound( 0x44B );

					for ( int i = 0; i < m_UlFlamBallOffset.Length; i++ )
					{
						Point2D offset = m_UlFlamBallOffset[i];

						Point3D effectFrom = new Point3D( from.Location.X + offset.X, from.Location.Y + offset.Y, from.Location.Z + 10 );

						Effects.SendPacket( effectFrom, from.Map,
							new HuedEffect( EffectType.Moving, Serial.Zero, target.Serial, itemId, effectFrom, target.Location, 7, 0, false, true, hue, 4 ) );
					}

					AOS.Damage( target, Utility.RandomMinMax( 200, 250 ), phys, fire, cold, pois, nrgy );
				}
				) );
		}
		#endregion

		#region Crimson Meteor
		public class CrimsonMeteorTimer : Timer
		{
			private const int MaxMeteors = 50;

			private Mobile m_Owner;
			private int m_Meteors;

			public CrimsonMeteorTimer( Mobile owner )
				: base( TimeSpan.Zero, TimeSpan.FromSeconds( 0.25 ) )
			{
				m_Owner = owner;
				m_Meteors = MaxMeteors;
			}

			protected override void OnTick()
			{
				if ( --m_Meteors == 0 || m_Owner.Deleted )
					Stop();

				Point3D loc = m_Owner.Location;
				Map map = m_Owner.Map;

				if ( map != null )
				{
					Point2D pTarget2d = new Point2D( loc.X + Utility.RandomMinMax( -2, 2 ), loc.Y + Utility.RandomMinMax( -2, 2 ) );

					Point3D pTarget = new Point3D( pTarget2d, map.GetAverageZ( pTarget2d.X, pTarget2d.Y ) );
					Point3D pSource = new Point3D( pTarget.X - 3, pTarget.Y - Utility.RandomMinMax( 9, 10 ), pTarget.Z + Utility.RandomMinMax( 45, 50 ) );

					int speed = Utility.RandomMinMax( 5, 8 );

					Effects.SendPacket( loc, map, new HuedEffect( EffectType.Moving, Serial.Zero, Serial.Zero, 0x36D4, pSource, pTarget, speed, 0, false, false, 0x5DE, 4 ) );
					Effects.PlaySound( loc, map, Utility.Random( 0x15E, 3 ) );
					Effects.PlaySound( loc, map, Utility.Random( 0x305, 5 ) );

					if ( 0.2 > Utility.RandomDouble() )
						new FireFieldItem( m_Owner, loc, map, Utility.RandomBool() ? 0x398C : 0x3996, TimeSpan.FromMinutes( 1.0 ) );
				}
			}
		}

		public static void CrimsonMeteor( Mobile from )
		{
			new CrimsonMeteorTimer( from ).Start();
		}
		#endregion
	}
}
