using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "Coils corpse" )]
	public class Coil : BaseCreature
	{
		[Constructable]
		public Coil()
			: base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 ) // TODO: Verify
		{
			Name = "Coil";
			Body = 92;
			BaseSoundID = 219; // TODO: Verify
			Hue = 1370; // TODO: Verify

			SetStr( 335, 340 ); // TODO: All Values are taken from Stratics and will change along with the info taken from stratics/OSI.
			SetDex( 130, 135 );
			SetInt( 125, 130 );

			SetHits( 1165, 1170 );

			SetDamage( 5, 21 ); // TODO: Correct

			SetDamageType( ResistanceType.Physical, 50 );
			SetDamageType( ResistanceType.Poison, 50 );

			SetResistance( ResistanceType.Physical, 55, 60 );
			SetResistance( ResistanceType.Fire, 25, 30 );
			SetResistance( ResistanceType.Cold, 25, 30 );
			SetResistance( ResistanceType.Poison, 100, 100 );
			SetResistance( ResistanceType.Energy, 25, 30 );

			SetSkill( SkillName.Poisoning, 115, 120 );
			SetSkill( SkillName.Anatomy, 130, 135 );
			SetSkill( SkillName.MagicResist, 110, 115 );
			SetSkill( SkillName.Tactics, 135, 140 );
			SetSkill( SkillName.Wrestling, 130, 135 );

			Fame = 10000; // TODO: Correct
			Karma = -10000; // TODO: Correct

			if ( 0.01 > Utility.RandomDouble() )
				PackItem( new PetParrot() );
		}

		public override bool DeathAdderCharmable { get { return true; } } // Silverserpent

		public override int Meat { get { return 1; } } // Silverserpent
		public override Poison PoisonImmune { get { return Poison.Lethal; } } // Silverserpent
		public override Poison HitPoison { get { return Poison.Lethal; } }

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich, 2 );
		}

		protected override void OnAfterDeath(Server.Items.Container c)
		{
			if ( Utility.RandomBool() )
				c.DropItem( new Engines.MLQuests.CoilsFang() );

            if (2000 > Utility.Random(100000))
            {
                c.DropItem(SetItemsHelper.GetRandomSetItem());
            }

			base.OnAfterDeath (c);
		}


		public Coil( Serial serial )
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
