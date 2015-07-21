using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "Virulents corpse" )]
	public class Virulent : BaseCreature
	{
		[Constructable]
		public Virulent()
			: base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.1, 0.4 )
		{
			Name = "Virulent";
			Body = 11;
			BaseSoundID = 1170;
			Hue = 2301;

			SetStr( 245, 245 );
			SetDex( 195, 200 );
			SetInt( 350, 350 );

			SetHits( 745, 750 );

			SetDamage( 15, 22 );

			SetDamageType( ResistanceType.Physical, 20 );
			SetDamageType( ResistanceType.Poison, 80 );

			SetResistance( ResistanceType.Physical, 70, 70 );
			SetResistance( ResistanceType.Fire, 40, 40 );
			SetResistance( ResistanceType.Cold, 50, 50 );
			SetResistance( ResistanceType.Poison, 100, 100 );
			SetResistance( ResistanceType.Energy, 45, 50 );

			SetSkill( SkillName.EvalInt, 105, 110 );
			SetSkill( SkillName.Magery, 115, 120 );
			SetSkill( SkillName.Meditation, 10, 15 );
			SetSkill( SkillName.Poisoning, 120, 120 );
			SetSkill( SkillName.MagicResist, 95, 100 );
			SetSkill( SkillName.Tactics, 100, 105 );
			SetSkill( SkillName.Wrestling, 100, 105 );

			Fame = 5000;
			Karma = -5000;

			if ( 0.01 > Utility.RandomDouble() )
				PackItem( new PetParrot() );
		}

		public override void GenerateLoot()
		{
		}

		protected override void OnAfterDeath(Container c)
		{
			base.OnAfterDeath( c );

            if (4000 > Utility.Random(100000))
            {
                c.DropItem(SetItemsHelper.GetRandomSetItem());
            }
		}

		public override Poison PoisonImmune { get { return Poison.Lethal; } }
		public override Poison HitPoison { get { return Poison.Lethal; } }

		public Virulent( Serial serial )
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
