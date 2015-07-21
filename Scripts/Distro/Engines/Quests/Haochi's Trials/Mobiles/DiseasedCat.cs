using System;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName( "a diseased cat corpse" )]
	[TypeAlias( "Server.Mobiles.Housecat" )]
	public class DiseasedCat : BaseCreature
	{
		[Constructable]
		public DiseasedCat()
			: base( AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			Name = "a diseased cat";
			Body = 0xC9;
			Hue = 0x8FE;
			BaseSoundID = 0x69;

			SetStr( 9 );
			SetDex( 35 );
			SetInt( 5 );

			SetHits( 6 );
			SetMana( 0 );

			SetDamage( 1 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 5, 10 );

			SetSkill( SkillName.MagicResist, 5.0 );
			SetSkill( SkillName.Tactics, 4.0 );
			SetSkill( SkillName.Wrestling, 5.0 );

			Fame = 0;
			Karma = 150;
		}

		public override int Meat { get { return 1; } }
		public override FoodType FavoriteFood { get { return FoodType.Meat | FoodType.Fish; } }
		public override PackInstinct PackInstinct { get { return PackInstinct.Feline; } }

		public override bool PlayerRangeSensitive { get { return false; } }
		public override bool AlwaysMurderer { get { return true; } }

		public DiseasedCat( Serial serial )
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
