using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "an unfrozen mummy corpse" )]
	public class UnfrozenMummy : BaseCreature // Take an existing mummy and make it 1337. Thanks EA! -.-
	{
		[Constructable]
		public UnfrozenMummy()
			: base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 ) // TODO: Verify Fight Mode
		{
			Name = "an unfrozen mummy";
			Body = 154;
			BaseSoundID = 471; // TODO: Verify
			Hue = 0x480; // TODO: Verify

			SetStr( 495, 500 ); // TODO: All Values are taken from Stratics and will change along with the info taken from stratics/OSI.
			SetDex( 150, 150 );
			SetInt( 845, 850 );

			SetHits( 1500, 1500 );

			SetDamage( 18, 21 ); // TODO: Correct

			SetDamageType( ResistanceType.Physical, 0 );
			SetDamageType( ResistanceType.Cold, 50 );
			SetDamageType( ResistanceType.Energy, 50 );

			SetResistance( ResistanceType.Physical, 35, 40 );
			SetResistance( ResistanceType.Fire, 20, 25 );
			SetResistance( ResistanceType.Cold, 65, 70 );
			SetResistance( ResistanceType.Poison, 25, 30 );
			SetResistance( ResistanceType.Energy, 60, 65 );

			SetSkill( SkillName.EvalInt, 50, 55 );
			SetSkill( SkillName.Magery, 50, 55 );
			SetSkill( SkillName.Meditation, 80, 80 );
			SetSkill( SkillName.MagicResist, 250, 250 );
			SetSkill( SkillName.Tactics, 100, 100 );
			SetSkill( SkillName.Wrestling, 95, 100 );

			Fame = 10000; // TODO: Correct
			Karma = -10000; // TODO: Correct

			PackSpellweavingScroll();
		}

		public override SlayerName SlayerGroup { get { return SlayerName.Undead; } }

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich );
		}

        protected override void OnAfterDeath(Container c)
        {
            base.OnAfterDeath(c);

            if (0.2 > Utility.RandomDouble())
                c.DropItem(new BrokenCrystals());
        }

		public UnfrozenMummy( Serial serial )
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
