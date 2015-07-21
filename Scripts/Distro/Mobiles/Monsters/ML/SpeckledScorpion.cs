using System;
using System.Collections;
using Server.Items;
using Server.Targeting;
using Server.Engines.MLQuests;

namespace Server.Mobiles
{
	[CorpseName( "a speckled scorpion corpse" )]
	public class SpeckledScorpion : BaseCreature
	{
		[Constructable]
		public SpeckledScorpion()
			: base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a speckled scorpion";
			Body = 48;
			BaseSoundID = 397;

			SetStr( 73, 115 );
			SetDex( 76, 95 );
			SetInt( 16, 30 );

			SetHits( 50, 63 );
			SetMana( 0 );

			SetDamage( 5, 10 );

			SetDamageType( ResistanceType.Physical, 60 );
			SetDamageType( ResistanceType.Poison, 40 );

			SetResistance( ResistanceType.Physical, 20, 25 );
			SetResistance( ResistanceType.Fire, 10, 15 );
			SetResistance( ResistanceType.Cold, 20, 25 );
			SetResistance( ResistanceType.Poison, 40, 50 );
			SetResistance( ResistanceType.Energy, 10, 15 );

			SetSkill( SkillName.Poisoning, 80.1, 100.0 );
			SetSkill( SkillName.MagicResist, 30.1, 35.0 );
			SetSkill( SkillName.Tactics, 60.3, 75.0 );
			SetSkill( SkillName.Wrestling, 50.3, 65.0 );

			Fame = 2000;
			Karma = -2000;

			if ( Utility.RandomBool() )
				PackItem( new LesserPoisonPotion() );
		}

		protected override void OnAfterDeath(Container c)
		{
			base.OnAfterDeath( c );

			if ( 0.25 > Utility.RandomDouble() )
				c.DropItem( new SpeckledPoisonSac() );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Meager, 2 );
		}

		public override int Meat { get { return 1; } }
		public override FoodType FavoriteFood { get { return FoodType.Meat; } }
		public override PackInstinct PackInstinct { get { return PackInstinct.Arachnid; } }
		public override Poison PoisonImmune { get { return Poison.Greater; } }
		public override Poison HitPoison { get { return ( 0.8 >= Utility.RandomDouble() ? Poison.Greater : Poison.Deadly ); } }

		public SpeckledScorpion( Serial serial )
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
			/*int version = */reader.ReadInt();
		}
	}
}
