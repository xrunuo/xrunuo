using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "Grobus corpse" )]
	public class Grobu : BaseCreature
	{
		[Constructable]
		public Grobu()
			: base( AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			Name = "Grobu";
			Body = 211;
			BaseSoundID = 0xA3;
			Hue = 1175;

			SetStr( 180, 210 );
			SetDex( 125, 150 );
			SetInt( 45, 55 );

			SetHits( 1235, 1310 );
			SetMana( 0 );

			SetDamage( 15, 18 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 40, 45 );
			SetResistance( ResistanceType.Fire, 20, 40 );
			SetResistance( ResistanceType.Cold, 30, 35 );
			SetResistance( ResistanceType.Poison, 25, 30 );
			SetResistance( ResistanceType.Energy, 20, 40 );

			SetSkill( SkillName.MagicResist, 60, 85 );
			SetSkill( SkillName.Tactics, 95, 120 );
			SetSkill( SkillName.Wrestling, 95, 120 );

			Fame = 450;
			Karma = 0;

			if ( 0.01 > Utility.RandomDouble() )
				PackItem( new PetParrot() );
		}

		protected override void OnAfterDeath( Container c )
		{
			base.OnAfterDeath( c );
			c.AddItem( new Engines.MLQuests.GrobusFur() );

            if (1000 > Utility.Random(100000))
            {
                c.DropItem(SetItemsHelper.GetRandomSetItem());
            }
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.AosFilthyRich, 3 );
		}

		public Grobu( Serial serial )
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