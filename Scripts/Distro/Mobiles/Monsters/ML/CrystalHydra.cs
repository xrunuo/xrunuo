using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a crystal hydra corpse" )]
	public class CrystalHydra : BaseCreature
	{
		[Constructable]
		public CrystalHydra()
			: base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 ) // TODO: Verify
		{
			Name = "a crystal hydra";
			Body = 265;
			BaseSoundID = 219; // TODO: Verify
			Hue = 0x47E; // TODO: Correct

			SetStr( 800, 830 ); // TODO: All Values are taken from Stratics and will change along with the info taken from stratics/OSI.
			SetDex( 100, 120 );
			SetInt( 100, 110 );

			SetHits( 1400, 1500 );

			SetDamage( 15, 30 ); // TODO: Correct

			SetDamageType( ResistanceType.Physical, 5 );
			SetDamageType( ResistanceType.Fire, 5 );
			SetDamageType( ResistanceType.Cold, 80 );
			SetDamageType( ResistanceType.Poison, 5 );
			SetDamageType( ResistanceType.Energy, 5 );

			SetResistance( ResistanceType.Physical, 65, 75 );
			SetResistance( ResistanceType.Fire, 20, 30 );
			SetResistance( ResistanceType.Cold, 85, 100 );
			SetResistance( ResistanceType.Poison, 35, 45 );
			SetResistance( ResistanceType.Energy, 80, 100 );

			SetSkill( SkillName.Anatomy, 75, 80 );
			SetSkill( SkillName.MagicResist, 85, 100 );
			SetSkill( SkillName.Tactics, 100, 110 );
			SetSkill( SkillName.Wrestling, 100, 115 );

			Fame = 10000; // TODO: Correct
			Karma = -10000; // TODO: Correct

			PackSpellweavingScroll();
		}

		public override bool HasBreath { get { return true; } } // ice breath enabled
		public override int BreathEffectHue { get { return 0x47E; } } // TODO: Correct breath
		public override int BreathEffectSound { get { return 0x56D; } }
		public override int BreathFireDamage { get { return 0; } }
		public override int BreathColdDamage { get { return 100; } }

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich ); // ArcticOgreLord loot
		}

		protected override void OnAfterDeath(Container c)
		{
			base.OnAfterDeath (c);

			if ( 0.2 > Utility.RandomDouble() )
				c.DropItem( new LuckyDagger() );
            if (0.2 > Utility.RandomDouble())
                c.DropItem(new ShatteredCrystals());
		}


		public CrystalHydra( Serial serial )
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
