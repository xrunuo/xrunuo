using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Engines.Loyalty;

namespace Server.Mobiles
{
	[CorpseName( "a leather wolf's corpse" )]
	public class LeatherWolf : BaseCreature
	{
		public override bool AlwaysMurderer { get { return !Controlled; } }

		public override WeaponAbility GetWeaponAbility()
		{
			return WeaponAbility.BleedAttack;
		}

		private const int MaxFellows = 3;

		private List<Mobile> m_Fellows = new List<Mobile>();
		private InternalTimer m_FellowsTimer;

		[Constructable]
		public LeatherWolf()
			: base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a leather wolf";
			Body = 739;
			BaseSoundID = 0xE5;

			SetStr( 104, 125 );
			SetDex( 102, 125 );
			SetInt( 20, 34 );

			SetHits( 291, 329 );

			SetDamage( 12, 23 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 40, 49 );
			SetResistance( ResistanceType.Fire, 20, 29 );
			SetResistance( ResistanceType.Cold, 30, 40 );
			SetResistance( ResistanceType.Poison, 21, 29 );
			SetResistance( ResistanceType.Energy, 20, 25 );

			SetSkill( SkillName.MagicResist, 79.5, 94.9 );
			SetSkill( SkillName.Tactics, 80.6, 89.4 );
			SetSkill( SkillName.Wrestling, 70.9, 88.4 );

			Fame = 4500;
			Karma = -4500;

			ControlSlots = 1;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Average );
			AddLoot( LootPack.Meager );
		}

		protected override void OnAfterDeath( Container c )
		{
			base.OnAfterDeath( c );

			if ( !Controlled )
			{
				if ( 0.2 > Utility.RandomDouble() )
					c.DropItem( new LeatherWolfSkin() );

				if ( 0.2 > Utility.RandomDouble() )
					c.DropItem( new ReflectiveWolfEye() );
			}
		}

		public override int Meat { get { return 1; } }
		public override int Hides { get { return 7; } }
		public override HideType HideType { get { return HideType.Spined; } }
		public override PackInstinct PackInstinct { get { return PackInstinct.Canine; } }

		public override LoyaltyGroup LoyaltyGroupEnemy { get { return LoyaltyGroup.GargoyleQueen; } }
		public override int LoyaltyPointsAward { get { return 30; } }

		public override void OnCombatantChange()
		{
			if ( !Controlled && Combatant != null && m_FellowsTimer == null )
			{
				m_FellowsTimer = new InternalTimer( this );
				m_FellowsTimer.Start();
			}
		}

		public void CheckFellows()
		{
			if ( !Alive || Combatant == null || Controlled || Map == null || Map == Map.Internal )
			{
				m_Fellows.ForEach( f => f.Delete() );
				m_Fellows.Clear();

				m_FellowsTimer.Stop();
				m_FellowsTimer = null;
			}
			else
			{
				for ( int i = 0; i < m_Fellows.Count; i++ )
				{
					Mobile friend = m_Fellows[i];

					if ( friend.Deleted )
						m_Fellows.Remove( friend );
				}

				bool spawned = false;

				for ( int i = m_Fellows.Count; i < MaxFellows; i++ )
				{
					BaseCreature friend = new LeatherWolfFellow();

					friend.MoveToWorld( Map.GetSpawnPosition( Location, 6 ), Map );
					friend.Combatant = Combatant;

					if ( friend.AIObject != null )
						friend.AIObject.Action = ActionType.Combat;

					m_Fellows.Add( friend );

					spawned = true;
				}

				if ( spawned )
				{
					Say( 1113132 ); // The leather wolf howls for help
					PlaySound( 0xE6 );
				}
			}
		}

		private class InternalTimer : Timer
		{
			private LeatherWolf m_Owner;

			public InternalTimer( LeatherWolf owner )
				: base( TimeSpan.Zero, TimeSpan.FromSeconds( 30.0 ) )
			{
				m_Owner = owner;
				}

			protected override void OnTick()
			{
				m_Owner.CheckFellows();
			}
		}

		public LeatherWolf( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}

	public class LeatherWolfFellow : BaseCreature
	{
		[Constructable]
		public LeatherWolfFellow()
			: base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a leather wolf";
			Body = 739;
			BaseSoundID = 0xE5;

			SetStr( 105, 115 );
			SetDex( 101, 114 );
			SetInt( 23, 34 );

			SetHits( 81, 110 );

			SetDamage( 9, 20 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 36, 50 );
			SetResistance( ResistanceType.Fire, 10, 18 );
			SetResistance( ResistanceType.Cold, 23, 29 );
			SetResistance( ResistanceType.Poison, 10, 17 );
			SetResistance( ResistanceType.Energy, 10, 15 );

			SetSkill( SkillName.MagicResist, 59.2, 75 );
			SetSkill( SkillName.Tactics, 53.3, 64.8 );
			SetSkill( SkillName.Wrestling, 64, 79 );

			Fame = 2500;
			Karma = -2500;
		}

		public override PackInstinct PackInstinct { get { return PackInstinct.Canine; } }

		public LeatherWolfFellow( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}
}
