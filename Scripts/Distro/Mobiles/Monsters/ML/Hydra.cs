namespace Server.Mobiles
{
	[CorpseName( "a hydra corpse" )]
	public class Hydra : BaseCreature
	{
		[Constructable]
		public Hydra()
			: base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 ) // TODO: Verify
		{
			Name = "a hydra";
			Body = 265;
			BaseSoundID = 219; // TODO: Verify

			SetStr( 810, 815 ); // TODO: All Values are taken from Stratics and will change along with the info taken from stratics/OSI.
			SetDex( 105, 110 );
			SetInt( 110, 115 );

			SetHits( 1400, 1500 );

			SetDamage( 15, 30 ); // TODO: Correct

			SetDamageType( ResistanceType.Physical, 60 );
			SetDamageType( ResistanceType.Fire, 10 );
			SetDamageType( ResistanceType.Cold, 10 );
			SetDamageType( ResistanceType.Poison, 10 );
			SetDamageType( ResistanceType.Energy, 10 );

			SetResistance( ResistanceType.Physical, 70, 75 );
			SetResistance( ResistanceType.Fire, 75, 80 );
			SetResistance( ResistanceType.Cold, 25, 30 );
			SetResistance( ResistanceType.Poison, 40, 45 );
			SetResistance( ResistanceType.Energy, 35, 40 );

			SetSkill( SkillName.Anatomy, 75, 80 );
			SetSkill( SkillName.MagicResist, 90, 95 );
			SetSkill( SkillName.Tactics, 105, 110 );
			SetSkill( SkillName.Wrestling, 110, 110 );

			Fame = 10000; // TODO: Correct
			Karma = -10000; // TODO: Correct

			PackSpellweavingScroll();
		}

		public override bool HasBreath { get { return true; } } // fire breath enabled
		public override int BreathEffectSound { get { return 0x56D; } }// TODO: Correct breath

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich ); // ArcticOgreLord loot
		}

		public Hydra( Serial serial )
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
