using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "Master Theophilus corpse" )]
	public class MasterTheophilus : BaseCreature
	{
		[Constructable]
		public MasterTheophilus()
			: base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.1, 0.4 )
		{
			Name = "Master Theophilus";
			Title = "the Necromancer";
			Body = 124;
			Hue = 0;

			SetStr( 160, 190 );
			SetDex( 260, 305 );
			SetInt( 410, 450 );

			SetHits( 825, 875 );

			SetDamage( 5, 10 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 55, 60 );
			SetResistance( ResistanceType.Fire, 50, 60 );
			SetResistance( ResistanceType.Cold, 50, 60 );
			SetResistance( ResistanceType.Poison, 50, 60 );
			SetResistance( ResistanceType.Energy, 50, 60 );

			SetSkill( SkillName.Meditation, 130, 135 );
			SetSkill( SkillName.EvalInt, 120, 135 );
			SetSkill( SkillName.Magery, 135, 145 );
			SetSkill( SkillName.MagicResist, 125, 135 );
			SetSkill( SkillName.Tactics, 110, 120 );
			SetSkill( SkillName.Wrestling, 65, 105 );

			Fame = 2500;
			Karma = -2500;

			if ( 0.01 > Utility.RandomDouble() )
				PackItem( new PetParrot() );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.UltraRich );
		}

        protected override void OnAfterDeath(Container c)
        {
            if (2000 > Utility.Random(100000))
            {
                c.DropItem(SetItemsHelper.GetRandomSetItem());
            }

            base.OnAfterDeath(c);
        }

		public override bool CanRummageCorpses { get { return true; } }
		public override bool AlwaysMurderer { get { return true; } }
		public override int TreasureMapLevel { get { return 5; } }		

		public MasterTheophilus( Serial serial )
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
