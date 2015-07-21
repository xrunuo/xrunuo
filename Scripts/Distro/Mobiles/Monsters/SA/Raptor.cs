using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Engines.Loyalty;

namespace Server.Mobiles
{
	[CorpseName( "a raptor corpse" )]
	public class Raptor : BaseCreature
	{
		private const int MaxFriends = 2;

		private bool m_IsFriend;
		private List<Mobile> m_Friends = new List<Mobile>();
		private InternalTimer m_FriendsTimer;

		[Constructable]
		public Raptor()
			: this( false )
		{
		}

		[Constructable]
		public Raptor( bool isFriend )
			: base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.175, 0.350 )
		{
			m_IsFriend = isFriend;

			Name = "a raptor";
			Body = 730;

			SetStr( 404, 471 );
			SetDex( 132, 155 );
			SetInt( 105, 145 );

			SetHits( 343, 400 );

			SetDamage( 11, 17 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 45, 50 );
			SetResistance( ResistanceType.Fire, 50, 60 );
			SetResistance( ResistanceType.Cold, 40, 50 );
			SetResistance( ResistanceType.Poison, 20, 30 );
			SetResistance( ResistanceType.Energy, 30, 40 );

			SetSkill( SkillName.MagicResist, 75.1, 90.0 );
			SetSkill( SkillName.Tactics, 75.1, 100.0 );
			SetSkill( SkillName.Wrestling, 70.1, 95.1 );

			Fame = 7500;
			Karma = -7500;

			Tamable = !isFriend; // not OSI accurate
			MinTameSkill = 107.1;
			ControlSlots = 2;
		}

		public override int GetAngerSound() { return 0x625; }
		public override int GetIdleSound() { return 0x625; }
		public override int GetAttackSound() { return 0x622; }
		public override int GetHurtSound() { return 0x624; }
		public override int GetDeathSound() { return 0x623; }

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich, 2 );
		}

		public override void OnCombatantChange()
		{
			if ( !m_IsFriend && !Controlled && Combatant != null && m_FriendsTimer == null )
			{
				m_FriendsTimer = new InternalTimer( this );
				m_FriendsTimer.Start();
			}
		}

		public void CheckFriends()
		{
			if ( !Alive || Combatant == null || Controlled || Map == null || Map == Map.Internal )
			{
				m_Friends.ForEach( f => f.Delete() );
				m_Friends.Clear();

				m_FriendsTimer.Stop();
				m_FriendsTimer = null;
			}
			else
			{
				int count = 0;

				for ( int i = 0; i < m_Friends.Count; i++ )
				{
					// remove dead friends

					Mobile friend = m_Friends[i];

					if ( friend == null || friend.Deleted )
						m_Friends.Remove( friend );
					else
						count++;
				}

				for ( int i = count; i < MaxFriends; i++ )
				{
					// spawn new friends

					BaseCreature friend = new Raptor( true );

					friend.MoveToWorld( Map.GetSpawnPosition( Location, 6 ), Map );
					friend.Combatant = Combatant;

					if ( friend.AIObject != null )
						friend.AIObject.Action = ActionType.Combat;

					m_Friends.Add( friend );
				}
			}
		}

		protected override void OnAfterDeath( Container c )
		{
			base.OnAfterDeath( c );

			if ( 0.1 > Utility.RandomDouble() )
				c.DropItem( new RaptorTeeth() );

			if ( 0.005 > Utility.RandomDouble() )
				c.DropItem( new RaptorClaw() );
		}

		public override LoyaltyGroup LoyaltyGroupEnemy { get { return LoyaltyGroup.GargoyleQueen; } }
		public override int LoyaltyPointsAward { get { return 20; } }

		public override int Meat { get { return 5; } }
		public override int Blood { get { return 8; } }
		public override int Hides { get { return 10; } }
		public override HideType HideType { get { return HideType.Horned; } }
		public override bool CanAngerOnTame { get { return true; } }
		public override PackInstinct PackInstinct { get { return PackInstinct.Raptor; } }
		public override FoodType FavoriteFood { get { return FoodType.Fish; } }
		public override int TreasureMapLevel { get { return 3; } }		

		public Raptor( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 );

			writer.Write( (bool) m_IsFriend );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			if ( version > 0 )
				m_IsFriend = reader.ReadBool();

			if ( m_IsFriend )
				Delete();
		}

		private class InternalTimer : Timer
		{
			private Raptor m_Owner;

			public InternalTimer( Raptor owner )
				: base( TimeSpan.Zero, TimeSpan.FromSeconds( 30.0 ) )
			{
				m_Owner = owner;
				}

			protected override void OnTick()
			{
				m_Owner.CheckFriends();
			}
		}
	}
}
