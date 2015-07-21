namespace Server.Mobiles
{
	[CorpseName( "a minotaur captain corpse" )]
	public class MinotaurCaptain : BaseCreature
	{
		public override int GetDeathSound()
		{
			return 0x596;
		}
		public override int GetAttackSound()
		{
			return 0x597;
		}
		public override int GetIdleSound()
		{
			return 0x598;
		}
		public override int GetAngerSound()
		{
			return 0x599;
		}
		public override int GetHurtSound()
		{
			return 0x59A;
		}

		[Constructable]
		public MinotaurCaptain()
			: base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a minotaur captain";
			Body = 280;

			SetStr( 405, 430 );
			SetDex( 90, 110 );
			SetInt( 30, 50 );

			SetHits( 400, 450 );

			SetDamage( 11, 20 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 65, 75 );
			SetResistance( ResistanceType.Fire, 35, 45 );
			SetResistance( ResistanceType.Cold, 40, 50 );
			SetResistance( ResistanceType.Poison, 40, 50 );
			SetResistance( ResistanceType.Energy, 40, 50 );

			SetSkill( SkillName.MagicResist, 65, 75 );
			SetSkill( SkillName.Tactics, 90, 110 );
			SetSkill( SkillName.Wrestling, 90, 110 );

			Fame = 10000;
			Karma = -10000;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich );
		}

		public MinotaurCaptain( Serial serial )
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
