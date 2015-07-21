using System;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName( "a raging grizzly bear corpse" )]
	public class RagingGrizzlyBear : BaseCreature
	{
		[Constructable]
		public RagingGrizzlyBear()
			: base( AIType.AI_Melee, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			Name = "a raging grizzly bear";
			Body = 212;
			BaseSoundID = 0xA3;

			SetStr( 626, 655 );
			SetDex( 581, 605 );
			SetInt( 16, 40 );

			SetHits( 576, 593 );

			SetDamage( 16, 24 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 55, 65 );
			SetResistance( ResistanceType.Fire, 25, 35 );
			SetResistance( ResistanceType.Cold, 45, 55 );
			SetResistance( ResistanceType.Poison, 35, 40 );
			SetResistance( ResistanceType.Energy, 35, 40 );

			SetSkill( SkillName.MagicResist, 100.1, 105.0 );
			SetSkill( SkillName.Tactics, 105.1, 120.0 );
			SetSkill( SkillName.Wrestling, 105.1, 125.0 );

			Fame = 10000;
			Karma = 0;
		}

		public override int Meat { get { return 2; } }
		public override int Hides { get { return 16; } }
		public override FoodType FavoriteFood { get { return FoodType.Fish | FoodType.FruitsAndVegies | FoodType.Meat; } }
		public override PackInstinct PackInstinct { get { return PackInstinct.Bear; } }

		public RagingGrizzlyBear( Serial serial )
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