using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "Malefics corpse" )]
	public class Malefic : BaseCreature
	{
		[Constructable]
		public Malefic()
			: base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Malefic";
			Body = 11;
			BaseSoundID = 1170;
			Hue = 1175;

			SetStr( 220, 225 );
			SetDex( 175, 180 );
			SetInt( 365, 370 );

			SetHits( 655, 655 );

			SetDamage( 15, 22 );

			SetDamageType( ResistanceType.Physical, 20 );
			SetDamageType( ResistanceType.Poison, 80 );

			SetResistance( ResistanceType.Physical, 60, 60 );
			SetResistance( ResistanceType.Fire, 40, 45 );
			SetResistance( ResistanceType.Cold, 45, 50 );
			SetResistance( ResistanceType.Poison, 100, 100 );
			SetResistance( ResistanceType.Energy, 45, 50 );

			SetSkill( SkillName.EvalInt, 105, 110 );
			SetSkill( SkillName.Magery, 105, 110 );
			SetSkill( SkillName.Meditation, 15, 20 );
			SetSkill( SkillName.Poisoning, 120, 120 );
			SetSkill( SkillName.MagicResist, 85, 90 );
			SetSkill( SkillName.Tactics, 95, 100 );
			SetSkill( SkillName.Wrestling, 100, 105 );

			Fame = 5000;
			Karma = -5000;

			if ( 0.01 > Utility.RandomDouble() )
				PackItem( new PetParrot() );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich );
		}

        protected override void OnAfterDeath(Container c)
        {
            if (2000 > Utility.Random(100000))
            {
                c.DropItem(SetItemsHelper.GetRandomSetItem());
            }

            base.OnAfterDeath(c);
        }

		public override Poison PoisonImmune { get { return Poison.Lethal; } }
		public override Poison HitPoison { get { return Poison.Lethal; } }

		public Malefic( Serial serial )
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
