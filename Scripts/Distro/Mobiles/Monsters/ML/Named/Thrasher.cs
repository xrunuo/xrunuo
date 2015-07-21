using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "Thrashers corpse" )]
	public class Thrasher : BaseCreature
	{
		[Constructable]
		public Thrasher()
			: base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.1, 0.4 )
		{
			Name = "Thrasher";
			Body = 0xCA;
			BaseSoundID = 660;
			Hue = 1;

			SetStr( 90, 100 );
			SetDex( 235, 235 );
			SetInt( 20, 20 );

			SetHits( 680, 685 );

			SetDamage( 5, 15 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 50, 50 );
			SetResistance( ResistanceType.Fire, 25, 30 );
			SetResistance( ResistanceType.Poison, 30, 30 );

			SetSkill( SkillName.MagicResist, 115, 120 );
			SetSkill( SkillName.Tactics, 100, 105 );
			SetSkill( SkillName.Wrestling, 95, 100 );

			Fame = 600;
			Karma = -600;

			if ( 0.01 > Utility.RandomDouble() )
				PackItem( new PetParrot() );
		}

		public override FoodType FavoriteFood { get { return FoodType.Fish; } }

		public Thrasher( Serial serial )
			: base( serial )
		{
		}

        protected override void OnAfterDeath(Container c)
        {
            if (1000 > Utility.Random(100000))
            {
                c.DropItem(SetItemsHelper.GetRandomSetItem());
            }

            base.OnAfterDeath(c);
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

			if ( BaseSoundID == 0x5A )
				BaseSoundID = 660;
		}
	}
}
