using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "Silks corpse" )]
	public class Silk : BaseCreature
	{
		[Constructable]
		public Silk()
			: base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.1, 0.4 )
		{
			Name = "Silk";
			Body = 0x9D;
			BaseSoundID = 0x388;
			Hue = 1150;

			SetStr( 110, 115 );
			SetDex( 120, 125 );
			SetInt( 70, 70 );

			SetHits( 370, 370 );

			SetDamage( 5, 17 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 50, 50 );
			SetResistance( ResistanceType.Fire, 35, 40 );
			SetResistance( ResistanceType.Cold, 35, 40 );
			SetResistance( ResistanceType.Poison, 80, 80 );
			SetResistance( ResistanceType.Energy, 35, 40 );

			SetSkill( SkillName.Anatomy, 70, 75 );
			SetSkill( SkillName.Poisoning, 95, 100 );
			SetSkill( SkillName.MagicResist, 95, 100 );
			SetSkill( SkillName.Tactics, 100, 105 );
			SetSkill( SkillName.Wrestling, 110, 125 );

			Fame = 3500;
			Karma = -3500;

			if ( 0.01 > Utility.RandomDouble() )
				PackItem( new PetParrot() );
		}

		public override void GenerateLoot()
		{
		}

        protected override void OnAfterDeath(Container c)
        {
            if (2000 > Utility.Random(100000))
            {
                c.DropItem(SetItemsHelper.GetRandomSetItem());
            }

            base.OnAfterDeath(c);
        }

		public override Poison PoisonImmune { get { return Poison.Deadly; } }
		public override Poison HitPoison { get { return Poison.Deadly; } }

		public Silk( Serial serial )
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
