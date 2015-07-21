using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "Sir Patricks corpse" )]
	public class SirPatrick : BaseCreature
	{
		[Constructable]
		public SirPatrick()
			: base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.1, 0.4 )
		{
			Name = "Sir Patrick";
			Body = 147;
			BaseSoundID = 451;
			Hue = 1150;

			SetStr( 255, 320 );
			SetDex( 105, 135 );
			SetInt( 55, 100 );

			SetHits( 710, 880 );

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
			SetSkill( SkillName.Tactics, 130, 140 );
			SetSkill( SkillName.Wrestling, 130, 135 );

			Fame = 3000;
			Karma = -3000;

			if ( 0.01 > Utility.RandomDouble() )
				PackItem( new PetParrot() );
		}

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

		public SirPatrick( Serial serial )
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
