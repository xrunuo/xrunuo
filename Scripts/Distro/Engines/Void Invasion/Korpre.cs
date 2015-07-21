using System;
using Server;
using Server.Engines.Loyalty;

namespace Server.Mobiles
{
	[CorpseName( "a Korpre's corpse" )]
	public class Korpre : VoidCreature, IGatherer
	{
		protected override void CreateEvolutionHandlers()
		{
			AddEvolutionHandler( new KillingPathHandler( this, typeof( Betballem ), 300 ) );
			AddEvolutionHandler( new GroupingPathHandler( this, typeof( Anlorzen ), 150 ) );
			AddEvolutionHandler( new SurvivalPathHandler( this, typeof( Anzuanord ), 1500 ) );
		}

		public override bool AlwaysMurderer { get { return true; } }

		[Constructable]
		public Korpre()
			: base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Korpre";
			Body = 51;
			BaseSoundID = 456;

			Hue = 2070;

			SetStr( 22, 34 );
			SetDex( 16, 23 );
			SetInt( 16, 23 );

			SetHits( 28, 379 );
			SetMana( 0, 4 );

			SetDamage( 1, 5 );

			SetDamageType( ResistanceType.Physical, 20 );
			SetDamageType( ResistanceType.Fire, 20 );
			SetDamageType( ResistanceType.Cold, 20 );
			SetDamageType( ResistanceType.Poison, 20 );
			SetDamageType( ResistanceType.Energy, 20 );

			SetResistance( ResistanceType.Physical, 0, 50 );
			SetResistance( ResistanceType.Fire, 0, 50 );
			SetResistance( ResistanceType.Cold, 0, 50 );
			SetResistance( ResistanceType.Poison, 100 );
			SetResistance( ResistanceType.Energy, 0, 50 );

			SetSkill( SkillName.Anatomy, 10.6, 29 );
			SetSkill( SkillName.MagicResist, 15.1, 20 );
			SetSkill( SkillName.Tactics, 11.7, 25.2 );
			SetSkill( SkillName.Wrestling, 12.5, 25.1 );

			Fame = 300;
			Karma = -300;
		}

		public override LoyaltyGroup LoyaltyGroupEnemy { get { return LoyaltyGroup.GargoyleQueen; } }
		public override int LoyaltyPointsAward { get { return 3; } }

		public Korpre( Serial serial )
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

		private Mobile m_GatherTarget;
		private DateTime m_NextGatherAttempt;

		public Mobile GatherTarget
		{
			get { return m_GatherTarget; }
			set { m_GatherTarget = value; }
		}

		public DateTime NextGatherAttempt
		{
			get { return m_NextGatherAttempt; }
			set { m_NextGatherAttempt = value; }
		}

		public bool DoesGather { get { return true; } }
	}
}
