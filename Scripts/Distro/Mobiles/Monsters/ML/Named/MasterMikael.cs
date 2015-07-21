using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "Master Mikaels corpse" )]
	public class MasterMikael : BaseCreature
	{
		[Constructable]
		public MasterMikael()
			: base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.1, 0.4 )
		{
			Name = "Master Mikael";
			Body = 148;
			BaseSoundID = 451;
			Hue = 2102;

			SetStr( 80, 130 );
			SetDex( 60, 105 );
			SetInt( 225, 255 );

			SetHits( 780, 1015 );

			SetDamage( 3, 7 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 55, 60 );
			SetResistance( ResistanceType.Fire, 40, 50 );
			SetResistance( ResistanceType.Cold, 70, 80 );
			SetResistance( ResistanceType.Poison, 40, 50 );
			SetResistance( ResistanceType.Energy, 50, 60 );

			SetSkill( SkillName.EvalInt, 95, 110 );
			SetSkill( SkillName.Magery, 95, 110 );
			SetSkill( SkillName.MagicResist, 90, 110 );
			SetSkill( SkillName.Tactics, 75, 95 );
			SetSkill( SkillName.Wrestling, 75, 90 );

			Fame = 3000;
			Karma = -3000;

			if ( 0.01 > Utility.RandomDouble() )
				PackItem( new PetParrot() );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.UltraRich, 1 );
		}

		protected override void OnAfterDeath(Container c)
		{
			base.OnAfterDeath (c);

			if ( 0.15 > Utility.RandomDouble() )
				c.DropItem( new Engines.MLQuests.DisintegratingThesisNotes() );

            if (3000 > Utility.Random(100000))
            {
                c.DropItem(SetItemsHelper.GetRandomSetItem());
            }
		}

		public override Poison PoisonImmune { get { return Poison.Regular; } }

		public MasterMikael( Serial serial )
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
