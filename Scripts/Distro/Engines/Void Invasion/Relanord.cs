using System;
using Server;
using Server.Items;
using Server.Engines.Loyalty;

namespace Server.Mobiles
{
	[CorpseName( "a Relanord's corpse" )]
	public class Relanord : VoidCreature
	{
		protected override void CreateEvolutionHandlers()
		{
			AddEvolutionHandler( new SurvivalPathHandler( this, typeof( Vasanord ), 50000 ) );
		}

		[Constructable]
		public Relanord()
			: base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Relanord";
			Body = 778;
			BaseSoundID = 377;

			Hue = 2069;

			SetStr( 758, 773 );
			SetDex( 53, 75 );
			SetInt( 58, 80 );

			SetHits( 403, 496 );

			SetDamage( 10, 15 );

			SetDamageType( ResistanceType.Physical, 20 );
			SetDamageType( ResistanceType.Fire, 20 );
			SetDamageType( ResistanceType.Cold, 20 );
			SetDamageType( ResistanceType.Poison, 20 );
			SetDamageType( ResistanceType.Energy, 20 );

			SetResistance( ResistanceType.Physical, 30, 60 );
			SetResistance( ResistanceType.Fire, 30, 60 );
			SetResistance( ResistanceType.Cold, 30, 60 );
			SetResistance( ResistanceType.Poison, 100 );
			SetResistance( ResistanceType.Energy, 60, 60 );

			SetSkill( SkillName.MagicResist, 26.5, 68.9 );
			SetSkill( SkillName.Tactics, 39.4, 60.3 );
			SetSkill( SkillName.Wrestling, 57.5, 72.2 );

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

		public override LoyaltyGroup LoyaltyGroupEnemy { get { return LoyaltyGroup.GargoyleQueen; } }
		public override int LoyaltyPointsAward { get { return 20; } }

		public override int Meat { get { return 1; } }

		public Relanord( Serial serial )
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
