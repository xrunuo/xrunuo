using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "Red Deaths corpse" )]
	public class RedDeath : BaseCreature
	{
		public override WeaponAbility GetWeaponAbility()
		{
			return WeaponAbility.WhirlwindAttack;
		}

		[Constructable]
		public RedDeath()
			: base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.1, 0.4 )
		{
			Name = "Red Death";
			Body = 793;
			Hue = 38;

			SetStr( 315, 325 );
			SetDex( 240, 255 );
			SetInt( 240, 255 );

			SetHits( 1520, 1615 );

			SetDamage( 20, 24 );

			SetDamageType( ResistanceType.Physical, 25 );
			SetDamageType( ResistanceType.Fire, 75 );

			SetResistance( ResistanceType.Physical, 60, 70 );
			SetResistance( ResistanceType.Fire, 90, 90 );
			SetResistance( ResistanceType.Poison, 100, 100 );

			SetSkill( SkillName.Anatomy, 120, 145 );
			SetSkill( SkillName.MagicResist, 120, 145 );
			SetSkill( SkillName.Tactics, 120, 145 );
			SetSkill( SkillName.Wrestling, 120, 145 );

			// TODO: Check
			Fame = 12000;
			Karma = -12000;

			if ( 0.01 > Utility.RandomDouble() )
				PackItem( new PetParrot() );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.UltraRich, 2 );
		}

		protected override void OnAfterDeath( Container c )
		{
			base.OnAfterDeath( c );		
			
			c.DropItem( new ResolvesBridle() );

            if (2000 > Utility.Random(100000))
            {
                c.DropItem(SetItemsHelper.GetRandomSetItem());
            }
		}

		public override bool AlwaysMurderer { get { return true; } }

		public RedDeath( Serial serial )
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
