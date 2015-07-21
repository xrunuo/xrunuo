using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a crystal daemon corpse" )]
	public class CrystalDaemon : BaseCreature
	{
		[Constructable]
		public CrystalDaemon()
			: base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 ) // TODO: Verify Fight Mode
		{
			Name = "a crystal daemon";
			Body = 0x310;
			BaseSoundID = 0x47D; // TODO: Correct
			Hue = 1000;

			SetStr( 150, 155 ); // All Values are taken from Stratics and will change along with the info taken from stratics/OSI.
			SetDex( 130, 150 );
			SetInt( 800, 840 );

			SetHits( 200, 215 );

			SetDamage( 6, 8 ); // Arcane Daemon Damage / 2

			SetDamageType( ResistanceType.Physical, 0 );
			SetDamageType( ResistanceType.Cold, 40 );
			SetDamageType( ResistanceType.Energy, 60 );

			SetResistance( ResistanceType.Physical, 20, 40 );
			SetResistance( ResistanceType.Fire, 5, 20 );
			SetResistance( ResistanceType.Cold, 60, 80 );
			SetResistance( ResistanceType.Poison, 20, 40 );
			SetResistance( ResistanceType.Energy, 65, 75 );

			SetSkill( SkillName.EvalInt, 100, 110 );
			SetSkill( SkillName.Magery, 120, 130 );
			SetSkill( SkillName.Meditation, 100, 110 );
			SetSkill( SkillName.MagicResist, 100, 110 );
			SetSkill( SkillName.Tactics, 70, 80 );
			SetSkill( SkillName.Wrestling, 60, 80 );

			Fame = 7000; // Arcane Deamon
			Karma = -10000; // Arcane Deamon

			PackSpellweavingScroll();
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Average ); // Arcane Deamon
		}

        protected override void OnAfterDeath(Container c)
        {
            base.OnAfterDeath(c);

            if (0.2 > Utility.RandomDouble())
                c.DropItem(new ScatteredCrystals());

        }

		public CrystalDaemon( Serial serial )
			: base( serial )
		{
		}

		public override Poison PoisonImmune { get { return Poison.Deadly; } } // Arcane Deamon

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
