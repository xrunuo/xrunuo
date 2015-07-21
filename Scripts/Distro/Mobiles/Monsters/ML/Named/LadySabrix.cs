using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "Lady Sabrix corpse" )]
	public class LadySabrix : BaseCreature
	{
		[Constructable]
		public LadySabrix()
			: base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Lady Sabrix";
			Body = 0x9D;
			BaseSoundID = 0x388;
			Hue = 1175;

			SetStr( 115, 120 );
			SetDex( 125, 130 );
			SetInt( 90, 90 );

			SetHits( 305, 305 );

			SetDamage( 15, 22 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 45, 45 );
			SetResistance( ResistanceType.Fire, 35, 40 );
			SetResistance( ResistanceType.Cold, 35, 40 );
			SetResistance( ResistanceType.Poison, 75, 80 );
			SetResistance( ResistanceType.Energy, 35, 40 );

			SetSkill( SkillName.Anatomy, 85, 90 );
			SetSkill( SkillName.Poisoning, 100, 105 );
			SetSkill( SkillName.MagicResist, 90, 95 );
			SetSkill( SkillName.Tactics, 105, 110 );
			SetSkill( SkillName.Wrestling, 110, 115 );

			Fame = 3500;
			Karma = -3500;		
	
			if ( 0.01 > Utility.RandomDouble() )
				PackItem( new PetParrot() );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich );
		}

		protected override void OnAfterDeath(Container c)
		{
			base.OnAfterDeath( c );

			if ( 0.2 > Utility.RandomDouble() )
				c.DropItem( new SabrixsEye() );

            if (2000 > Utility.Random(100000))
            {
                c.DropItem(SetItemsHelper.GetRandomSetItem());
            }
		}

		public override Poison PoisonImmune { get { return Poison.Deadly; } }
		public override Poison HitPoison { get { return Poison.Deadly; } }

		public LadySabrix( Serial serial )
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
