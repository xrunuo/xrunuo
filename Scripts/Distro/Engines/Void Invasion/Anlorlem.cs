using System;
using Server;
using Server.Items;
using Server.Engines.Loyalty;

namespace Server.Mobiles
{
	[CorpseName( "an Anlorlem's corpse" )]
	public class Anlorlem : VoidCreature, IGatherer
	{
		protected override void CreateEvolutionHandlers()
		{
			AddEvolutionHandler( new GroupingPathHandler( this, typeof( Anlorvaglem ), 2000 ) );
		}

		[Constructable]
		public Anlorlem()
			: base( AIType.AI_Arcanist, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Anlorlem";
			Body = 152;
			BaseSoundID = 0x24D;

			Hue = 2071;

			SetStr( 906, 989 );
			SetDex( 1030, 1181 );
			SetInt( 925, 989 );

			SetHits( 504, 600 );

			SetDamage( 18, 22 );

			SetDamageType( ResistanceType.Physical, 50 );
			SetDamageType( ResistanceType.Poison, 50 );

			SetResistance( ResistanceType.Physical, 40, 50 );
			SetResistance( ResistanceType.Fire, 30, 40 );
			SetResistance( ResistanceType.Cold, 35, 45 );
			SetResistance( ResistanceType.Poison, 90, 100 );
			SetResistance( ResistanceType.Energy, 35, 45 );

			SetSkill( SkillName.MagicResist, 32, 69.7 );
			SetSkill( SkillName.Tactics, 90.9, 99.7 );
			SetSkill( SkillName.Wrestling, 90.1, 99.1 );
			SetSkill( SkillName.Spellweaving, 90.1, 99.1 );

			Fame = 2000;
			Karma = -2000;

			PackItem( new DaemonBone( 15 ) );

			if ( 0.2 > Utility.RandomDouble() )
				PackItem( new VoidOrb() );

			if ( 0.2 > Utility.RandomDouble() )
				PackItem( new VoidEssence() );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich );
		}

		public override FoodType FavoriteFood { get { return FoodType.Meat; } }
		public override MeatType MeatType { get { return MeatType.Ribs; } }
		public override int Meat { get { return 2; } }

		public override LoyaltyGroup LoyaltyGroupEnemy { get { return LoyaltyGroup.GargoyleQueen; } }
		public override int LoyaltyPointsAward { get { return 20; } }

		public Anlorlem( Serial serial )
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
