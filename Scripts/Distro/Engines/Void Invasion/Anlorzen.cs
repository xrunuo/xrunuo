using System;
using Server;
using Server.Items;
using Server.Engines.Loyalty;

namespace Server.Mobiles
{
	[CorpseName( "an Anlorzen's corpse" )]
	public class Anlorzen : VoidCreature, IGatherer
	{
		protected override void CreateEvolutionHandlers()
		{
			AddEvolutionHandler( new GroupingPathHandler( this, typeof( Anlorlem ), 500 ) );
		}

		[Constructable]
		public Anlorzen()
			: base( AIType.AI_Arcanist, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Anlorzen";
			Body = 11;
			BaseSoundID = 1170;

			Hue = 2071;

			SetStr( 617, 786 );
			SetDex( 617, 791 );
			SetInt( 843, 987 );

			SetHits( 309, 391 );

			SetDamage( 15, 18 );

			SetDamageType( ResistanceType.Physical, 20 );
			SetDamageType( ResistanceType.Fire, 20 );
			SetDamageType( ResistanceType.Cold, 20 );
			SetDamageType( ResistanceType.Poison, 20 );
			SetDamageType( ResistanceType.Energy, 20 );

			SetResistance( ResistanceType.Physical, 45, 55 );
			SetResistance( ResistanceType.Fire, 35, 45 );
			SetResistance( ResistanceType.Cold, 35, 44 );
			SetResistance( ResistanceType.Poison, 100, 100 );
			SetResistance( ResistanceType.Energy, 35, 45 );

			SetSkill( SkillName.MagicResist, 12.1, 54.6 );
			SetSkill( SkillName.Tactics, 38.3, 62.2 );
			SetSkill( SkillName.Wrestling, 43.2, 63.9 );

			Fame = 1800;
			Karma = -1800;

			PackItem( new DaemonBone( 5 ) );

			if ( 0.1 > Utility.RandomDouble() )
				PackItem( new VoidOrb() );

			if ( 0.1 > Utility.RandomDouble() )
				PackItem( new VoidEssence() );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Average );
			AddLoot( LootPack.Meager );
		}

		public override FoodType FavoriteFood { get { return FoodType.Meat; } }
		public override MeatType MeatType { get { return MeatType.Ribs; } }
		public override int Meat { get { return 2; } }

		public override LoyaltyGroup LoyaltyGroupEnemy { get { return LoyaltyGroup.GargoyleQueen; } }
		public override int LoyaltyPointsAward { get { return 10; } }

		public Anlorzen( Serial serial )
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
