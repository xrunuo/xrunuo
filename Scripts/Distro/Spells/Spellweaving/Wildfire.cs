using System;
using System.Collections;
using Server.Targeting;
using Server.Mobiles;
using Server.Network;
using Server.Items;
using Server.Misc;

namespace Server.Spells.Spellweaving
{
	public class WildfireSpell : SpellweavingSpell
	{
		private static SpellInfo m_Info = new SpellInfo( "Wildfire", "Haelyn", -1, 9002 );

		public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds( 1.75 ); } }

		public override double RequiredSkill { get { return 66.0; } }
		public override int RequiredMana { get { return 50; } }

		public WildfireSpell( Mobile caster, Item scroll )
			: base( caster, scroll, m_Info )
		{
		}

		public override void OnCast()
		{
			Caster.Target = new InternalTarget( this );
		}

		public void Target( IPoint3D p )
		{
			if ( !Caster.CanSee( p ) )
			{
				Caster.SendLocalizedMessage( 500237 ); // Target can not be seen.
			}
			else if ( SpellHelper.CheckTown( p, Caster ) && CheckSequence() )
			{
				SpellHelper.Turn( Caster, p );
				SpellHelper.GetSurfaceTop( ref p );

				Effects.PlaySound( p, Caster.Map, 0x5CF );

				int focuslevel = GetFocusLevel( Caster );

				int range = focuslevel + 1;

				bool visible;

				for ( int i = range * -1; i <= range; i++ )
				{
					for ( int j = range * -1; j <= range; j++ )
					{
						visible = false;

						if ( ( j == range && i == range ) || ( j == range * -1 && i == range * -1 ) || ( j == range && i == range * -1 ) || ( j == range * -1 && i == range ) )
							visible = true;

						if ( ( j == 0 && i == range ) || ( j == 0 && i == range * -1 ) || ( j == range && i == 0 ) || ( j == range * -1 && i == 0 ) )
							visible = true;

						Point3D loc = new Point3D( p.X + i, p.Y + j, p.Z );

						new InternalItem( loc, Caster, Caster.Map, TimeSpan.FromSeconds( ( Caster.Skills[SkillName.Spellweaving].Fixed / 240 ) + focuslevel ), visible );
					}
				}
			}

			FinishSequence();
		}

		[DispellableField]
		public class InternalItem : Item
		{
			private Timer m_Timer;
			private DateTime m_End;
			private Mobile m_Caster;

			public int Damage;

			public override bool BlocksFit { get { return true; } }

			public InternalItem( Point3D loc, Mobile caster, Map map, TimeSpan duration, bool visible )
				: base( Utility.RandomBool() ? 0x398C : 0x3996 )
			{
				bool canFit = SpellHelper.AdjustField( ref loc, map, 12, false );

				Movable = false;
				Visible = visible;
				Light = LightType.Circle300;

				MoveToWorld( loc, map );

				m_Caster = caster;

				Damage = 15 + GetFocusLevel( caster );

				m_End = DateTime.UtcNow + duration;

				m_Timer = new InternalTimer( this, TimeSpan.FromSeconds( 0.5 ), caster.InLOS( this ), canFit );
				m_Timer.Start();
			}

			public override void OnAfterDelete()
			{
				base.OnAfterDelete();

				if ( m_Timer != null )
				{
					m_Timer.Stop();
				}
			}

			public InternalItem( Serial serial )
				: base( serial )
			{
			}

			public override void Serialize( GenericWriter writer )
			{
				base.Serialize( writer );

				writer.Write( (int) 1 ); // version

				writer.Write( m_Caster );
				writer.WriteDeltaTime( m_End );
			}

			public override void Deserialize( GenericReader reader )
			{
				base.Deserialize( reader );

				int version = reader.ReadInt();

				switch ( version )
				{
					case 1:
						{
							m_Caster = reader.ReadMobile();

							goto case 0;
						}
					case 0:
						{
							m_End = reader.ReadDeltaTime();

							m_Timer = new InternalTimer( this, TimeSpan.Zero, true, true );
							m_Timer.Start();

							break;
						}
				}
			}

			private class InternalTimer : Timer
			{
				private InternalItem m_Item;

				private static Queue m_Queue = new Queue();

				public InternalTimer( InternalItem item, TimeSpan delay, bool inLOS, bool canFit )
					: base( delay, TimeSpan.FromSeconds( 1.0 ) )
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
								if ( ( m.Z + 16 ) > m_Item.Z && ( m_Item.Z + 12 ) > m.Z && m != caster && SpellHelper.ValidIndirectTarget( caster, m ) && caster.CanBeHarmful( m, false ) )
								{
									m_Queue.Enqueue( m );
								}
							}

							while ( m_Queue.Count > 0 )
							{
								Mobile m = (Mobile) m_Queue.Dequeue();

								if ( caster.CanSee( m ) && caster.InLOS( m ) && ( caster.IsPlayer || m.IsPlayer || ( m is BaseCreature && ( (BaseCreature) m ).Controlled ) ) )
								{
									caster.DoHarmful( m );

									if ( !m_Item.Visible )
										m_Item.Visible = true;

									AOS.Damage( m, caster, m_Item.Damage, 0, 100, 0, 0, 0 );
									m.PlaySound( 0x5CF );
								}
							}
						}
					}
				}
			}
		}

		public class InternalTarget : Target
		{
			private WildfireSpell m_Owner;

			public InternalTarget( WildfireSpell owner )
				: base( 12, true, TargetFlags.None )
			{
				m_Owner = owner;
			}

			protected override void OnTarget( Mobile from, object o )
			{
				if ( o is IPoint3D )
				{
					m_Owner.Target( (IPoint3D) o );
				}
			}

			protected override void OnTargetFinish( Mobile from )
			{
				m_Owner.FinishSequence();
			}
		}
	}
}
