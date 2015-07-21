using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a greater dragon corpse" )]
	public class GreaterDragon : BaseCreature
	{
		[Constructable]
		public GreaterDragon()
			: base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a greater dragon";
			Body = Utility.RandomList( 12, 59 );
			BaseSoundID = 362;

			SetStr( 1126, 1322 );
			SetDex( 98, 132 );
			SetInt( 510, 604 );

			SetHits( 1076, 1732 );

			SetDamage( 24, 33 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 60, 85 );
			SetResistance( ResistanceType.Fire, 65, 90 );
			SetResistance( ResistanceType.Cold, 40, 55 );
			SetResistance( ResistanceType.Poison, 40, 60 );
			SetResistance( ResistanceType.Energy, 50, 75 );

			SetSkill( SkillName.EvalInt, 46.1, 57.5 );
			SetSkill( SkillName.Magery, 110.1, 140.0 );
			SetSkill( SkillName.MagicResist, 110.1, 140.0 );
			SetSkill( SkillName.Tactics, 110.1, 140.0 );
			SetSkill( SkillName.Wrestling, 115.1, 145.0 );

			Fame = 25000;
			Karma = -25000;

			Tamable = true;
			ControlSlots = 5;
			MinTameSkill = 104.7;

			this.Skills[SkillName.Wrestling].Cap = 120;
			this.Skills[SkillName.Tactics].Cap = 120;
			this.Skills[SkillName.Magery].Cap = 110;
			this.Skills[SkillName.EvalInt].Cap = 110;
			this.Skills[SkillName.MagicResist].Cap = 120;

			if ( 0.025 > Utility.RandomDouble() )
			{
				Name = "a black dragon";
				Hue = 1175;
			}
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich, 2 );
			AddLoot( LootPack.Gems, 4 );
			AddLoot( LootPack.Gems, 4 );
			AddLoot( LootPack.Gems, 4 );
			AddLoot( LootPack.Gems, 4 );
		}

		public override bool HasBreath { get { return true; } } // fire breath enabled
		public override bool AutoDispel { get { return !Controlled; } }
		public override int TreasureMapLevel { get { return 5; } }
		public override int Meat { get { return 19; } }
		public override int Hides { get { return 30; } }
		public override HideType HideType { get { return HideType.Barbed; } }
		public override int Scales { get { return 7; } }
		public override ScaleType ScaleType { get { return ( Body == 12 ? ScaleType.Yellow : ScaleType.Red ); } }
		public override FoodType FavoriteFood { get { return FoodType.Meat; } }
		public override bool CanAngerOnTame { get { return true; } }
		public override Poison PoisonImmune { get { return Poison.Regular; } }

		public override WeaponAbility GetWeaponAbility()
		{
			return WeaponAbility.BleedAttack;
		}

		public override bool StatLossAfterTame { get { return true; } }

		public GreaterDragon( Serial serial )
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