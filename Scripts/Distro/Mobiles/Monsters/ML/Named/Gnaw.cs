using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "Gnaws corpse" )]
	public class Gnaw : BaseCreature
	{
		[Constructable]
		public Gnaw()
			: base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Gnaw";
			Body = 23;
			BaseSoundID = 0xE5;
			Hue = 304;

			SetStr( 165, 170 );
			SetDex( 145, 145 );
			SetInt( 60, 60 );

			SetHits( 790, 795 );

			SetDamage( 16, 22 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 60, 60 );
			SetResistance( ResistanceType.Fire, 50, 50 );
			SetResistance( ResistanceType.Cold, 25, 30 );
			SetResistance( ResistanceType.Poison, 25, 30 );
			SetResistance( ResistanceType.Energy, 20, 25 );

			SetSkill( SkillName.MagicResist, 110, 115 );
			SetSkill( SkillName.Tactics, 105, 110 );
			SetSkill( SkillName.Wrestling, 115, 120 );

			Fame = 2500;
			Karma = -2500;

			if ( 0.01 > Utility.RandomDouble() )
				PackItem( new PetParrot() );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich );
		}

		protected override void OnAfterDeath(Container c)
		{
			base.OnAfterDeath( c );

			if ( 0.2 > Utility.RandomDouble() )
				c.DropItem( new Items.GnawsFang() );

            if (2000 > Utility.Random(100000))
            {
                c.DropItem(SetItemsHelper.GetRandomSetItem());
            }
		}

		public override int Meat { get { return 1; } }
		public override int Hides { get { return 7; } }
		public override HideType HideType { get { return HideType.Spined; } }
		public override PackInstinct PackInstinct { get { return PackInstinct.Canine; } }

		public Gnaw( Serial serial )
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
