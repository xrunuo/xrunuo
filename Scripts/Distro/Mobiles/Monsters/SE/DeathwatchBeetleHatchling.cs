using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a deathWatch beetle hatchling corpse" )]
	public class DeathWatchBeetleHatchling : BaseCreature
	{
		[Constructable]
		public DeathWatchBeetleHatchling()
			: base( AIType.AI_Melee, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			Name = "a deathwatch beetle hatchling";
			Body = 242;

			SetStr( 25, 50 );
			SetDex( 40, 55 );
			SetInt( 20, 30 );

			SetHits( 50, 60 );
			SetMana( 20 );

			SetDamage( 2, 5 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 35, 40 );
			SetResistance( ResistanceType.Fire, 15, 30 );
			SetResistance( ResistanceType.Cold, 15, 30 );
			SetResistance( ResistanceType.Poison, 20, 40 );
			SetResistance( ResistanceType.Energy, 20, 35 );

			SetSkill( SkillName.Anatomy, 20.1, 25.0 );
			SetSkill( SkillName.MagicResist, 30.1, 40.0 );
			SetSkill( SkillName.Tactics, 45.1, 60.0 );
			SetSkill( SkillName.Wrestling, 50.1, 60.0 );

			Fame = 150;
			Karma = 0;

			Tamable = false;

			PackReg( 3 );
			PackItem( Loot.RandomArmor() );
		}

		public override int GetAngerSound()
		{
			return 0x4F4;
		}

		public override int GetIdleSound()
		{
			return 0x4F3;
		}

		public override int GetAttackSound()
		{
			return 0x4F2;
		}

		public override int GetHurtSound()
		{
			return 0x4F5;
		}

		public override int GetDeathSound()
		{
			return 0x4F1;
		}

		public override int Hides { get { return 8; } }
		public override HideType HideType { get { return HideType.Regular; } }

		public DeathWatchBeetleHatchling( Serial serial )
			: base( serial )
		{
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Potions );
			AddLoot( LootPack.LowScrolls );
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