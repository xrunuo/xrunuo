using System;
using Server;
using Server.Engines.Loyalty;

namespace Server.Mobiles
{
	[CorpseName( "a skree corpse" )]
	public class Skree : BaseCreature
	{
		[Constructable]
		public Skree()
			: base( AIType.AI_Mystic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a skree";
			Body = 733;

			SetStr( 296, 328 );
			SetDex( 103, 121 );
			SetInt( 193, 260 );

			SetHits( 216, 293 );

			SetDamage( 5, 7 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 55, 65 );
			SetResistance( ResistanceType.Fire, 45, 55 );
			SetResistance( ResistanceType.Cold, 25, 40 );
			SetResistance( ResistanceType.Poison, 55, 65 );
			SetResistance( ResistanceType.Energy, 25, 40 );

			SetSkill( SkillName.MagicResist, 75.1, 80.0 );
			SetSkill( SkillName.Tactics, 20.1, 25.0 );
			SetSkill( SkillName.Wrestling, 20.1, 35.1 );
			SetSkill( SkillName.EvalInt, 90.1, 100.0 );
			SetSkill( SkillName.Magery, 90.1, 115.0 );
			SetSkill( SkillName.Meditation, 65.1, 75.0 );
			SetSkill( SkillName.Mysticism, 20.1, 35.0 );

			Fame = 2000;
			Karma = -2000;

			Tamable = true;
			MinTameSkill = 95.1;
			ControlSlots = 4;

			PackMysticReg( 4, 5 );

			if ( 0.5 > Utility.RandomDouble() )
				PackMysticScroll( Utility.Random( 8 ) ); // 1st - 4th circles
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Meager );
			AddLoot( LootPack.Poor );
		}

		public override int GetAngerSound() { return 0x631; }
		public override int GetIdleSound() { return 0x631; }
		public override int GetAttackSound() { return 0x62E; }
		public override int GetHurtSound() { return 0x630; }
		public override int GetDeathSound() { return 0x62F; }

		public override LoyaltyGroup LoyaltyGroupEnemy { get { return LoyaltyGroup.GargoyleQueen; } }
		public override int LoyaltyPointsAward { get { return 15; } }

		public override MeatType MeatType { get { return MeatType.Bird; } }
		public override int Meat { get { return 3; } }
		public override HideType HideType { get { return HideType.Regular; } }
		public override int Hides { get { return 5; } }
		public override bool CanAngerOnTame { get { return Owners.Count == 0; } }

		public override void OnTamed( Mobile master )
		{
			MinTameSkill = -24.9;
		}

		public Skree( Serial serial )
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
