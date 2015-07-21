using System;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "Swoops corpse" )]
	public class Swoop : BaseCreature
	{
		[Constructable]
		public Swoop()
			: base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.1, 0.4 )
		{
			Name = "Swoop";
			Body = 5;
			BaseSoundID = 0x2EE;
			Hue = 224;

			SetStr( 135, 140 );
			SetDex( 425, 450 );
			SetInt( 80, 85 );

			SetDamage( 20, 30 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetHits( 1350, 1550 );

			SetResistance( ResistanceType.Physical, 75, 90 );
			SetResistance( ResistanceType.Fire, 60, 75 );
			SetResistance( ResistanceType.Cold, 70, 75 );
			SetResistance( ResistanceType.Poison, 55, 65 );
			SetResistance( ResistanceType.Energy, 55, 60 );

			SetSkill( SkillName.Wrestling, 120.0, 140.0 );
			SetSkill( SkillName.Tactics, 120.0, 140.0 );
			SetSkill( SkillName.MagicResist, 100.0, 110.0 );

			Fame = 1000;
			Karma = -1000;

			if ( 0.01 > Utility.RandomDouble() )
				PackItem( new PetParrot() );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich, 4 );
		}

		protected override void OnAfterDeath( Container c )
		{
			base.OnAfterDeath( c );

			if (3000 > Utility.Random(100000))
            {
                c.DropItem(SetItemsHelper.GetRandomSetItem());
            }

		}

		// TODO: Check all of this
		public override int Meat { get { return 1; } }
		public override MeatType MeatType { get { return MeatType.Bird; } }
		public override int Feathers { get { return 36; } }

		public override FoodType FavoriteFood { get { return FoodType.Fish; } }

		public Swoop( Serial serial )
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
