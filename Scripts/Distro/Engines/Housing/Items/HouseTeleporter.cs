using System;
using System.Collections.Generic;

using Server.ContextMenus;
using Server.Engines.Housing.Multis;
using Server.Gumps;
using Server.Items;
using Server.Multis;

namespace Server.Engines.Housing.Items
{
	[TypeAlias( "Server.Items.HouseTeleporter" )]
	public class HouseTeleporter : Item, ISecurable
	{
		public override int LabelNumber { get { return 1113857; } } // house teleporter

		private HouseTeleporter m_Target;
		private SecureLevel m_Level;

		[CommandProperty( AccessLevel.GameMaster )]
		public HouseTeleporter Target { get { return m_Target; } set { m_Target = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public SecureLevel Level { get { return m_Level; } set { m_Level = value; } }

		[Constructable]
		public HouseTeleporter( int itemId )
			: this( itemId, null )
		{
		}

		public HouseTeleporter( int itemId, HouseTeleporter target )
			: base( itemId )
		{
			LootType = LootType.Blessed;
			Movable = false;
			Weight = 1.0;

			m_Level = SecureLevel.Anyone;

			m_Target = target;
		}

		public bool CheckAccess( Mobile m )
		{
			var house = HousingHelper.FindHouseAt( this );

			if ( house != null && ( house.Public ? house.IsBanned( m ) : !house.HasAccess( m ) ) )
				return false;

			return ( house != null && house.HasSecureAccess( m, m_Level ) );
		}

		public override bool OnMoveOver( Mobile m )
		{
			if ( m_Target != null && !m_Target.Deleted && !Movable && !m_Target.Movable )
			{
				if ( m.Player && m.Kills >= 5 && m_Target.Map != Map.Felucca )
				{
					m.SendLocalizedMessage( 1019004 ); // You are not allowed to travel there.
				}
				else if ( CheckAccess( m ) && m_Target.CheckAccess( m ) )
				{
					if ( !m.Hidden || m.AccessLevel == AccessLevel.Player )
						new EffectTimer( Location, Map, 2023, 0x1F0, TimeSpan.FromSeconds( 0.4 ) ).Start();

					new DelayTimer( this, m ).Start();
				}
				else
				{
					m.SendLocalizedMessage( 1061637 ); // You are not allowed to access this.
				}
			}

			return true;
		}

		public override void GetContextMenuEntries( Mobile from, List<ContextMenuEntry> list )
		{
			base.GetContextMenuEntries( from, list );

			SetSecureLevelEntry.AddTo( from, this, list );
		}

		public HouseTeleporter( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version

			writer.Write( (int) m_Level );

			writer.Write( (HouseTeleporter) m_Target );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 1:
					{
						m_Level = (SecureLevel) reader.ReadInt();
						goto case 0;
					}
				case 0:
					{
						m_Target = reader.ReadItem<HouseTeleporter>();

						if ( version < 0 )
							m_Level = SecureLevel.Anyone;

						break;
					}
			}
		}

		private class EffectTimer : Timer
		{
			private Point3D m_Location;
			private Map m_Map;
			private int m_EffectId;
			private int m_SoundId;

			public EffectTimer( Point3D p, Map map, int effectId, int soundId, TimeSpan delay )
				: base( delay )
			{
				m_Location = p;
				m_Map = map;
				m_EffectId = effectId;
				m_SoundId = soundId;
			}

			protected override void OnTick()
			{
				Effects.SendLocationParticles( EffectItem.Create( m_Location, m_Map, EffectItem.DefaultDuration ), 0x3728, 10, 10, m_EffectId, 0 );

				if ( m_SoundId != -1 )
					Effects.PlaySound( m_Location, m_Map, m_SoundId );
			}
		}

		private class DelayTimer : Timer
		{
			private HouseTeleporter m_Teleporter;
			private Mobile m_Mobile;

			public DelayTimer( HouseTeleporter tp, Mobile m )
				: base( TimeSpan.FromSeconds( 1.0 ) )
			{
				m_Teleporter = tp;
				m_Mobile = m;
			}

			protected override void OnTick()
			{
				HouseTeleporter target = m_Teleporter.Target;

				if ( target != null && !target.Deleted )
				{
					Mobile m = m_Mobile;

					if ( m.X == m_Teleporter.X && m.Y == m_Teleporter.Y && m.Map == m_Teleporter.Map )
					{
						Point3D p = target.GetWorldTop();
						Map map = target.Map;

						Server.Mobiles.BaseCreature.TeleportPets( m, p, map );

						m.MoveToWorld( p, map );

						if ( !m.Hidden || m.AccessLevel == AccessLevel.Player )
						{
							Effects.PlaySound( target.Location, target.Map, 0x1FE );

							Effects.SendLocationParticles( EffectItem.Create( m_Teleporter.Location, m_Teleporter.Map, EffectItem.DefaultDuration ), 0x3728, 10, 10, 2023, 0 );
							Effects.SendLocationParticles( EffectItem.Create( target.Location, target.Map, EffectItem.DefaultDuration ), 0x3728, 10, 10, 5023, 0 );

							new EffectTimer( target.Location, target.Map, 2023, -1, TimeSpan.FromSeconds( 0.4 ) ).Start();
						}
					}
				}
			}
		}
	}
}