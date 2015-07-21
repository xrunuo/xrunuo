// TODO: Check this creature
using System;
using Server.Items;
using System.Collections;

namespace Server.Mobiles
{
	[CorpseName( "Rends corpse" )] // TODO: Verify
	public class Rend : BaseCreature
	{
		public override WeaponAbility GetWeaponAbility()
		{
			return ( Utility.RandomBool() ? WeaponAbility.ParalyzingBlow : WeaponAbility.BleedAttack );
		}

		[Constructable]
		public Rend()
			: base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.1, 0.4 )
		{
			Name = "Rend";
			Body = 276;
			BaseSoundID = 362;
			Hue = 1109;

			SetStr( 1270, 1280 );
			SetDex( 365, 385 );
			SetInt( 600, 650 );

			SetHits( 5170, 5950 );

			SetDamage( 26, 33 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 75, 85 );
			SetResistance( ResistanceType.Fire, 80, 95 );
			SetResistance( ResistanceType.Cold, 45, 55 );
			SetResistance( ResistanceType.Poison, 35, 45 );
			SetResistance( ResistanceType.Energy, 45, 55 );

			SetSkill( SkillName.Anatomy, 65.0, 75.0 );
			SetSkill( SkillName.MagicResist, 90.0, 110.0 );
			SetSkill( SkillName.Tactics, 130.0, 145.0 );
			SetSkill( SkillName.Wrestling, 135.0, 155.0 );

			Fame = 20000; // TODO: Verify
			Karma = -20000;  // TODO: Verify

			if ( 0.01 > Utility.RandomDouble() )
				PackItem( new PetParrot() );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.UltraRich, 2 );
		}

        protected override void OnAfterDeath(Container c)
        {
            if (5000 > Utility.Random(100000))
            {
                c.DropItem(SetItemsHelper.GetRandomSetItem());
            }

            base.OnAfterDeath(c);
        }

		public override bool HasBreath { get { return true; } } // fire breath enabled

		public Rend( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */reader.ReadInt();
		}
	}
}
