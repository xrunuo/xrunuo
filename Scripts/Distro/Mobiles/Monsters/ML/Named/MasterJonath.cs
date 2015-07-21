using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "Master Jonaths corpse" )]
	public class MasterJonath : BaseCreature
	{
		[Constructable]
		public MasterJonath()
			: base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.1, 0.4 )
		{
			Name = "Master Jonath";
			Body = 148;
			BaseSoundID = 451;
			Hue = 1109;

			SetStr( 240, 260 );
			SetDex( 75, 100 );
			SetInt( 240, 260 );

			SetHits( 865, 970 );

			SetDamage( 3, 7 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 55, 60 );
			SetResistance( ResistanceType.Fire, 40, 50 );
			SetResistance( ResistanceType.Cold, 70, 80 );
			SetResistance( ResistanceType.Poison, 40, 50 );
			SetResistance( ResistanceType.Energy, 50, 60 );

			SetSkill( SkillName.EvalInt, 95, 105 );
			SetSkill( SkillName.Magery, 100, 105 );
			SetSkill( SkillName.MagicResist, 90, 110 );
			SetSkill( SkillName.Tactics, 80, 95 );
			SetSkill( SkillName.Wrestling, 80, 90 );

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
            if (2000 > Utility.Random(100000))
            {
                c.DropItem(SetItemsHelper.GetRandomSetItem());
            }

            base.OnAfterDeath(c);
        }

		public override Poison PoisonImmune { get { return Poison.Regular; } }

		public MasterJonath( Serial serial )
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
