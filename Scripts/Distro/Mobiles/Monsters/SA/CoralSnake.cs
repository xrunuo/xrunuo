using System;
using Server;
using Server.Engines.Loyalty;

namespace Server.Mobiles
{
	[CorpseName( "a coral snake's corpse" )]
	public class CoralSnake : BaseCreature
	{
		[Constructable]
		public CoralSnake()
			: base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a coral snake";
			Body = 52;
			BaseSoundID = 0xDB;

			Hue = 33;

			SetStr( 199, 265 );
			SetDex( 198, 289 );
			SetInt( 22, 37 );

			SetHits( 102, 136 );

			SetDamage( 5, 21 );

			SetDamageType( ResistanceType.Physical, 50 );
			SetDamageType( ResistanceType.Poison, 50 );

			SetResistance( ResistanceType.Physical, 35, 45 );
			SetResistance( ResistanceType.Fire, 6, 10 );
			SetResistance( ResistanceType.Cold, 5, 10 );
			SetResistance( ResistanceType.Poison, 100, 100 );
			SetResistance( ResistanceType.Energy, 5, 9 );

			SetSkill( SkillName.Healing, 92.8, 100 );
			SetSkill( SkillName.MagicResist, 95.9, 99.6 );
			SetSkill( SkillName.Tactics, 83.3, 94.2 );
			SetSkill( SkillName.Wrestling, 90.9, 98.1 );

			Fame = 5000;
			Karma = -5000;
		}

		public override LoyaltyGroup LoyaltyGroupEnemy { get { return LoyaltyGroup.GargoyleQueen; } }
		public override int LoyaltyPointsAward { get { return 3; } }

		public override Poison HitPoison { get { return Poison.Lethal; } }

		public override void GenerateLoot()
		{
			base.GenerateLoot();

			AddLoot( LootPack.Rich );
		}

		public CoralSnake( Serial serial )
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
