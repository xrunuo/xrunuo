using System;
using System.Collections;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "Miasmas corpse" )]
	public class Miasma : BaseCreature
	{
		public override WeaponAbility GetWeaponAbility()
		{
			return WeaponAbility.MortalStrike;
		}

		//private DateTime m_Delay = DateTime.Now;

		public override int TreasureMapLevel { get { return 5; } }

		[Constructable]
		public Miasma()
			: base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.1, 0.4 )
		{
			Name = "Miasma";
			Body = 48;
			BaseSoundID = 397;
			Hue = 2301;

			SetStr( 170, 834 );
			SetDex( 100, 450 );
			SetInt( 40, 330 );

			SetHits( 272, 2000 );

			SetDamage( 20, 30 );

			SetDamageType( ResistanceType.Physical, 60 );
			SetDamageType( ResistanceType.Poison, 40 );

			SetResistance( ResistanceType.Physical, 50, 60 );
			SetResistance( ResistanceType.Fire, 40, 50 );
			SetResistance( ResistanceType.Cold, 50, 55 );
			SetResistance( ResistanceType.Poison, 70, 80 );
			SetResistance( ResistanceType.Energy, 40, 50 );

			SetSkill( SkillName.Poisoning, 120, 145 );
			SetSkill( SkillName.MagicResist, 70, 80 );
			SetSkill( SkillName.Tactics, 105, 115 );
			SetSkill( SkillName.Wrestling, 65, 75 );

			Fame = 21000;
			Karma = -21000;

			if ( 0.1 > Utility.RandomDouble() )
				PackItem( new ParagonChest( this.Name, 5 ) );

			if ( 0.1 > Utility.RandomDouble() )
				PackItem( new PetParrot() );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich );
		}

		protected override void OnAfterDeath(Container c)
		{
			base.OnAfterDeath (c);

            if (3000 > Utility.Random(100000))
            {
                c.DropItem(SetItemsHelper.GetRandomSetItem());
            }
			
		}

		public override PackInstinct PackInstinct { get { return PackInstinct.Arachnid; } }
		public override Poison PoisonImmune { get { return Poison.Greater; } }
		public override Poison HitPoison { get { return Poison.Deadly; } }

		public Miasma( Serial serial )
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
