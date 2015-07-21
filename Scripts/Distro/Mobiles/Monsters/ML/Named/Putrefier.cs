using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	public class Putrefier : Balron
	{
		[Constructable]
		public Putrefier()
		{
			Name = "Putrefier";
			Hue = 63;

			ActiveSpeed = 0.1;

			SetStr( 1050, 1350 );
			SetDex( 250, 450 );
			SetInt( 201, 415 );

			SetHits( 3000, 4000 );

			SetDamage( 26, 34 );

			SetDamageType( ResistanceType.Physical, 50 );
			SetDamageType( ResistanceType.Poison, 50 );
			SetDamageType( ResistanceType.Cold, 0 );
			SetDamageType( ResistanceType.Energy, 0 );
			SetDamageType( ResistanceType.Fire, 0 );

			SetResistance( ResistanceType.Physical, 65, 80 );
			SetResistance( ResistanceType.Fire, 60, 75 );
			SetResistance( ResistanceType.Cold, 50, 60 );
			SetResistance( ResistanceType.Poison, 100 );
			SetResistance( ResistanceType.Energy, 40, 50 );

			SetSkill( SkillName.Anatomy, 25.1, 50.0 );
			SetSkill( SkillName.EvalInt, 90.1, 100.0 );
			SetSkill( SkillName.Magery, 95.5, 100.0 );
			SetSkill( SkillName.Meditation, 25.1, 50.0 );
			SetSkill( SkillName.MagicResist, 100.5, 150.0 );
			SetSkill( SkillName.Tactics, 90.1, 100.0 );
			SetSkill( SkillName.Wrestling, 90.1, 100.0 );

			Fame = 32000;
			Karma = -32000;

			PackSpellweavingScroll();
			PackSpellweavingScroll();

			if ( 0.01 > Utility.RandomDouble() )
				PackItem( new PetParrot() );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.UltraRich, 4 );
			AddLoot( LootPack.MedScrolls, 2 );
		}
		
		public override int TreasureMapLevel { get { return 5; } }		

		protected override void OnAfterDeath(Container c)
		{
			base.OnAfterDeath (c);

			if ( Utility.RandomBool() )
				c.DropItem( new SpleenOfThePutrefier() );

            if (2000 > Utility.Random(100000))
            {
                c.DropItem(SetItemsHelper.GetRandomSetItem());
            }
		}

		public Putrefier( Serial serial )
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