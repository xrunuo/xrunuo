using System;
using Server;

namespace Server.Mobiles
{
	[CorpseName( "a familiar corpse" )]
	public class Familiar : BaseCreature
	{
		[Constructable]
		public Familiar()
			: base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a familiar";
			Body = 776;

			Hue = 2101;

			SetStr( 49, 69 );
			SetDex( 53, 77 );
			SetInt( 22, 34 );

			SetHits( 51, 63 );

			SetDamage( 5, 10 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 15, 20 );
			SetResistance( ResistanceType.Fire, 5, 10 );

			SetSkill( SkillName.MagicResist, 20, 20 );
			SetSkill( SkillName.Tactics, 0.6, 13.5 );
			SetSkill( SkillName.Wrestling, 38.9, 48.9 );

			Fame = 500;
			Karma = -500;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Meager );
		}

		public override int GetIdleSound() { return 338; }
		public override int GetAngerSound() { return 338; }
		public override int GetDeathSound() { return 338; }
		public override int GetAttackSound() { return 406; }
		public override int GetHurtSound() { return 194; }

		public Familiar( Serial serial )
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

			/*int version = */
			reader.ReadInt();
		}
	}
}
