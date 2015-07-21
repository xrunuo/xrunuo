using System;
using Server;
using Server.Items;
using Server.Engines.Loyalty;

namespace Server.Mobiles
{
	[CorpseName( "an Anzuanord's corpse" )]
	public class Anzuanord : VoidCreature
	{
		protected override void CreateEvolutionHandlers()
		{
			AddEvolutionHandler( new SurvivalPathHandler( this, typeof( Relanord ), 10000 ) );
		}

		[Constructable]
		public Anzuanord()
			: base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Anzuanord";
			Body = 74;
			BaseSoundID = 422;

			Hue = 2069;

			SetStr( 582, 735 );
			SetDex( 904, 991 );
			SetInt( 910, 998 );

			SetHits( 135, 259 );

			SetDamage( 8, 10 );

			SetDamageType( ResistanceType.Physical, 20 );
			SetDamageType( ResistanceType.Fire, 20 );
			SetDamageType( ResistanceType.Cold, 20 );
			SetDamageType( ResistanceType.Poison, 20 );
			SetDamageType( ResistanceType.Energy, 20 );

			SetResistance( ResistanceType.Physical, 0, 25 );
			SetResistance( ResistanceType.Fire, 0, 25 );
			SetResistance( ResistanceType.Cold, 0, 30 );
			SetResistance( ResistanceType.Poison, 0, 30 );
			SetResistance( ResistanceType.Energy, 100 );

			SetSkill( SkillName.MagicResist, 30.2, 49.5 );
			SetSkill( SkillName.Tactics, 42.5, 49.3 );
			SetSkill( SkillName.Wrestling, 40.2, 43.9 );
			SetSkill( SkillName.EvalInt, 72.5, 87.7 );
			SetSkill( SkillName.Magery, 66.8, 85.2 );

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
			AddLoot( LootPack.Rich );
			AddLoot( LootPack.LowScrolls, 2 );
		}

		public override int Meat { get { return 1; } }
		public override int Hides { get { return 6; } }
		public override HideType HideType { get { return HideType.Spined; } }
		public override FoodType FavoriteFood { get { return FoodType.Meat; } }
		public override PackInstinct PackInstinct { get { return PackInstinct.Daemon; } }

		public override LoyaltyGroup LoyaltyGroupEnemy { get { return LoyaltyGroup.GargoyleQueen; } }
		public override int LoyaltyPointsAward { get { return 10; } }

		public Anzuanord( Serial serial )
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
