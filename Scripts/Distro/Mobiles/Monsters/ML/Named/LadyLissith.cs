using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "Lady Lissiths corpse" )]
	public class LadyLissith : BaseCreature
	{
		[Constructable]
		public LadyLissith()
			: base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Lady Lissith";
			Body = 0x9D;
			BaseSoundID = 0x388;
			Hue = 1106;

			SetStr( 80, 80 );
			SetDex( 115, 120 );
			SetInt( 70, 80 );

			SetHits( 255, 260 );

			SetDamage( 15, 22 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 50, 50 );
			SetResistance( ResistanceType.Fire, 35, 40 );
			SetResistance( ResistanceType.Cold, 30, 35 );
			SetResistance( ResistanceType.Poison, 75, 80 );
			SetResistance( ResistanceType.Energy, 35, 40 );

			SetSkill( SkillName.Anatomy, 75, 80 );
			SetSkill( SkillName.Poisoning, 115, 120 );
			SetSkill( SkillName.MagicResist, 85, 90 );
			SetSkill( SkillName.Tactics, 100, 105 );
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
				c.DropItem( new LissithsSilk() );
			
			if ( 0.05 > Utility.RandomDouble() )
				c.DropItem( new AcolyteTunic() );

            if (2000 > Utility.Random(100000))
            {
                c.DropItem(SetItemsHelper.GetRandomSetItem());
            }
		}

		public override Poison PoisonImmune { get { return Poison.Deadly; } }
		public override Poison HitPoison { get { return Poison.Deadly; } }

		public LadyLissith( Serial serial )
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
