using System;
using System.Collections;
using Server;
using Server.Items;
using Server.SkillHandlers;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a satyr corpse" )]
	public class Satyr : BaseBardCreature
	{
		public override int GetDeathSound() { return 0x585; }
		public override int GetAttackSound() { return 0x586; }
		public override int GetIdleSound() { return 0x587; }
		public override int GetAngerSound() { return 0x588; }
		public override int GetHurtSound() { return 0x589; }

		[Constructable]
		public Satyr()
			: base( AIType.AI_Melee, FightMode.Evil, 10, 1, 0.2, 0.4 )
		{
			Name = "a satyr";
			Body = 271;

			SetStr( 175, 795 );
			SetDex( 250, 270 );
			SetInt( 150, 170 );

			SetHits( 350, 400 );

			SetDamage( 13, 24 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 55, 60 );
			SetResistance( ResistanceType.Fire, 25, 35 );
			SetResistance( ResistanceType.Cold, 30, 40 );
			SetResistance( ResistanceType.Poison, 30, 40 );
			SetResistance( ResistanceType.Energy, 30, 40 );

			SetSkill( SkillName.Musicianship, 100, 100 );
			SetSkill( SkillName.Peacemaking, 100, 100 );
			SetSkill( SkillName.Discordance, 100, 100 );
			SetSkill( SkillName.Provocation, 100, 100 );
			SetSkill( SkillName.MagicResist, 55, 65 );
			SetSkill( SkillName.Tactics, 80, 100 );
			SetSkill( SkillName.Wrestling, 80, 100 );

			Fame = 5000;
			Karma = -5000;

			PackSpellweavingScroll();
		}

		public Satyr( Serial serial )
			: base( serial )
		{
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich, 2 );
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