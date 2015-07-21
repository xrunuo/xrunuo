using System;
using Server;
using Server.Items;
using Server.Engines.Loyalty;

namespace Server.Mobiles
{
	[CorpseName( "a Betballem's corpse" )]
	public class Betballem : VoidCreature
	{
		protected override void CreateEvolutionHandlers()
		{
			AddEvolutionHandler( new KillingPathHandler( this, typeof( Ballem ), 5000 ) );
		}

		[Constructable]
		public Betballem()
			: base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Betballem";
			Body = 776;
			BaseSoundID = 357;

			Hue = 2069;

			SetStr( 228, 289 );
			SetDex( 809, 976 );
			SetInt( 53, 101 );

			SetHits( 93, 201 );

			SetDamage( 5, 10 );

			SetDamageType( ResistanceType.Physical, 20 );
			SetDamageType( ResistanceType.Fire, 20 );
			SetDamageType( ResistanceType.Cold, 20 );
			SetDamageType( ResistanceType.Poison, 20 );
			SetDamageType( ResistanceType.Energy, 20 );

			SetResistance( ResistanceType.Physical, 10, 40 );
			SetResistance( ResistanceType.Fire, 10, 40 );
			SetResistance( ResistanceType.Cold, 10, 40 );
			SetResistance( ResistanceType.Poison, 10, 40 );
			SetResistance( ResistanceType.Energy, 100 );

			SetSkill( SkillName.MagicResist, 14, 51.9 );
			SetSkill( SkillName.Tactics, 18.9, 31.8 );
			SetSkill( SkillName.Wrestling, 24.9, 40.7 );

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

		public override LoyaltyGroup LoyaltyGroupEnemy { get { return LoyaltyGroup.GargoyleQueen; } }
		public override int LoyaltyPointsAward { get { return 3; } }

		public Betballem( Serial serial )
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
