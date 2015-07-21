using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "an Undead Guardian corpse" )]
	public class UndeadGuardian : BaseCreature
	{
		[Constructable]
		public UndeadGuardian()
			: base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.25, 0.5 )
		{
			Name = "Undead Guardian";
			Body = 722;

			SetStr( 209, 243 );
			SetDex( 77, 93 );
			SetInt( 36, 58 );

			SetHits( 118, 148 );

			SetDamage( 8, 18 );

			SetDamageType( ResistanceType.Physical, 40 );
			SetDamageType( ResistanceType.Cold, 60 );

			SetResistance( ResistanceType.Physical, 35, 45 );
			SetResistance( ResistanceType.Fire, 20, 30 );
			SetResistance( ResistanceType.Cold, 50, 60 );
			SetResistance( ResistanceType.Poison, 20, 30 );
			SetResistance( ResistanceType.Energy, 30, 40 );

			SetSkill( SkillName.MagicResist, 65.1, 80.0 );
			SetSkill( SkillName.Tactics, 85.1, 100.0 );
			SetSkill( SkillName.Wrestling, 85.1, 95.1 );

			Fame = 5000; // TODO: Verify
			Karma = -5000;

			PackNecroReg( 50, 150 );

			PackItem( new UndyingFlesh() );
		}

		public UndeadGuardian( Serial serial )
			: base( serial )
		{
		}

		public override int GetAngerSound() { return 0x175; }
		public override int GetIdleSound() { return 0x19D; }
		public override int GetAttackSound() { return 0xE2; }
		public override int GetHurtSound() { return 0x28B; }
		public override int GetDeathSound() { return 0x108; }

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich, 3 );
			AddLoot( LootPack.Gems, 2 );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}
}
