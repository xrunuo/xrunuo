using System;
using System.Collections;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Spells.Fourth;
using Server.Targeting;

namespace Server.Items
{
	public abstract class BaseConflagrationPotion : BasePotion
	{
		public abstract int MinBaseDamage { get; }
		public abstract int MaxBaseDamage { get; }

		public BaseConflagrationPotion( PotionEffect effect )
			: base( 0xF06, effect )
		{
			Hue = 0x489;
		}

		public BaseConflagrationPotion( Serial serial )
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
				return ( (PlayerMobile) from ).NextDrinkConflagrationPotion;

			return TimeSpan.Zero;
		}

		public override void SetNextDrinkTime( Mobile from )
		{
			if ( from is PlayerMobile )
				( (PlayerMobile) from ).NextDrinkConflagrationPotion = TimeSpan.FromSeconds( 30.0 );
		}

		public override void Drink( Mobile from )
		{
			if ( from.Paralyzed || from.Frozen || ( from.Spell != null && from.Spell.IsCasting ) )
			{
				from.SendLocalizedMessage( 1062725 ); // You can not use that potion while paralyzed.
				return;
			}

			ThrowTarget targ = from.Target as ThrowTarget;

			if ( targ != null && targ.Potion == this )
				return;

			from.RevealingAction();

			from.Target = new ThrowTarget( this );
		}

		protected void ComputeDamage( Mobile from, out int damageMin, out int damageMax )
		{
			double alchemy = from.Skills[SkillName.Alchemy].Value;

			damageMin = (int) BasePotion.Scale( from, MinBaseDamage + alchemy / 11.0 );
			damageMax = (int) BasePotion.Scale( from, MaxBaseDamage + alchemy / 13.0 );
		}

		private class ThrowTarget : Target
		{
			private BaseConflagrationPotion m_Potion;

			public BaseConflagrationPotion Potion
			{
				get { return m_Potion; }
			}

			public ThrowTarget( BaseConflagrationPotion potion )
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
				{
					to = (Mobile) p;
					p = to.Location;
				}
				else
					to = new DummyEntity( Serial.Zero, new Point3D( p ), map );

				Effects.SendMovingEffect( from, to, m_Potion.ItemID, 7, 0, false, false, m_Potion.Hue, 0 );

				m_Potion.Consume();

				Effects.PlaySound( p, map, 0x20C );

				int damageMin, damageMax;
				m_Potion.ComputeDamage( from, out damageMin, out damageMax );

				Timer.DelayCall( TimeSpan.FromSeconds( 1.5 ), () =>
					{
						for ( int i = -2; i <= 2; i++ )
						{
							for ( int j = -2; j <= 2; j++ )
							{
								Point3D point = new Point3D( p.X + i, p.Y + j, p.Z );

								if ( map.CanFit( point, 12, true, false ) && from.InLOS( point ) )
									new InternalItem( from, point, map, damageMin, damageMax );
							}
						}
					} );

				m_Potion.SetNextDrinkTime( from );
			}
		}

		public class InternalItem : Item
		{
			private Mobile m_Caster;
			private DateTime m_End;
			private Timer m_Timer;
			private int m_DamageMin, m_DamageMax;

			public override bool BlocksFit { get { return true; } }

			public InternalItem( Mobile caster, Point3D loc, Map map, int damageMin, int damageMax )
				: base( 0x398C )
			{
				Movable = false;
				Light = LightType.Circle300;

				MoveToWorld( loc, map );

				m_Caster = caster;
				m_End = DateTime.Now + TimeSpan.FromSeconds( 10 );

				m_DamageMin = damageMin;
				m_DamageMax = damageMax;

				m_Timer = new InternalTimer( this );
				m_Timer.Start();
			}

			public override void OnAfterDelete()
			{
				base.OnAfterDelete();

				if ( m_Timer != null )
					m_Timer.Stop();
			}

			public InternalItem( Serial serial )
				: base( serial )
			{
			}

			public override void Serialize( GenericWriter writer )
			{
				base.Serialize( writer );

				writer.Write( (int) 1 ); // version

				writer.Write( (Mobile) m_Caster );
				writer.Write( (DateTime) m_End );
				writer.Write( (int) m_DamageMin );
				writer.Write( (int) m_DamageMax );
			}

			public override void Deserialize( GenericReader reader )
			{
				base.Deserialize( reader );

				int version = reader.ReadInt();

				m_Caster = reader.ReadMobile();
				m_End = reader.ReadDateTime();
				m_DamageMin = reader.ReadInt();
				m_DamageMax = reader.ReadInt();

				if ( version < 1 )
				{
					// Prior to version 1, damage max was actually an offset
					m_DamageMax = m_DamageMin + m_DamageMax;
				}

				m_Timer = new InternalTimer( this );
				m_Timer.Start();
			}

			public override bool OnMoveOver( Mobile m )
			{
				if ( m_Caster != null && m != m_Caster && SpellHelper.ValidIndirectTarget( m_Caster, m ) && m_Caster.CanBeHarmful( m, false ) )
					Damage( m );

				return true;
			}

			public void Damage( Mobile m )
			{
				m_Caster.DoHarmful( m );

				AOS.Damage( m, m_Caster, Utility.RandomMinMax( m_DamageMin, m_DamageMax ), 0, 100, 0, 0, 0 );
				m.PlaySound( 0x208 );
			}

			private class InternalTimer : Timer
			{
				private InternalItem m_Item;

				private static Queue m_Queue = new Queue();

				public InternalTimer( InternalItem item )
					: base( TimeSpan.Zero, TimeSpan.FromSeconds( 1.0 ) )
				{
					m_Item = item;

				}

				protected override void OnTick()
				{
					if ( m_Item.Deleted )
						return;

					if ( DateTime.Now > m_Item.m_End )
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
								if ( ( m.Z + 16 ) > m_Item.Z && ( m_Item.Z + 12 ) > m.Z && m != caster && SpellHelper.ValidIndirectTarget( caster, m ) && caster.CanBeHarmful( m, false ) )
									m_Queue.Enqueue( m );
							}

							while ( m_Queue.Count > 0 )
							{
								Mobile m = (Mobile) m_Queue.Dequeue();

								m_Item.Damage( m );
							}
						}
					}
				}
			}
		}
	}
}