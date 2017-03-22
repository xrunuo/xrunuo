using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "Lady Marais corpse" )]
	public class LadyMarai : BaseCreature
	{
		[Constructable]
		public LadyMarai()
			: base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Lady Marai";
			Body = 147;
			BaseSoundID = 451;
			Hue = 2118;

			SetStr( 230, 235 );
			SetDex( 90, 130 );
			SetInt( 55, 100 );

			SetHits( 625, 930 );

			SetDamage( 8, 18 );

			SetDamageType( ResistanceType.Physical, 40 );
			SetDamageType( ResistanceType.Cold, 60 );

			SetResistance( ResistanceType.Physical, 55, 65 );
			SetResistance( ResistanceType.Fire, 40, 50 );
			SetResistance( ResistanceType.Cold, 70, 80 );
			SetResistance( ResistanceType.Poison, 40, 50 );
			SetResistance( ResistanceType.Energy, 50, 60 );

			SetSkill( SkillName.Anatomy, 125, 140 );
			SetSkill( SkillName.MagicResist, 100, 120 );
			SetSkill( SkillName.Tactics, 125, 145 );
			SetSkill( SkillName.Wrestling, 130, 140 );

			Fame = 3000;
			Karma = -3000;

			if ( 0.01 > Utility.RandomDouble() )
				PackItem( new PetParrot() );
		}

		public override SlayerName SlayerGroup { get { return SlayerName.Undead; } }

		public override void GenerateLoot()
		{
			AddLoot( LootPack.UltraRich );
		}

		protected override void OnAfterDeath(Container c)
		{
			base.OnAfterDeath (c);

			if ( 0.15 > Utility.RandomDouble() )
				c.DropItem( new Engines.MLQuests.DisintegratingThesisNotes() );

            if (2000 > Utility.Random(100000))
            {
                c.DropItem(SetItemsHelper.GetRandomSetItem());
            }
		}

		public LadyMarai( Serial serial )
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
