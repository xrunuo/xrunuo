using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a crane corpse" )]
	public class Crane : BaseCreature
	{
		[Constructable]
		public Crane()
			: base( AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			Name = "a crane";
			Body = 254;

			SetStr( 25, 35 );
			SetDex( 15, 25 );
			SetInt( 10, 15 );

			SetHits( 25, 35 );
			SetMana( 0 );

			SetDamage( 1 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 5 );

			SetSkill( SkillName.MagicResist, 4.1, 5.0 );
			SetSkill( SkillName.Tactics, 10.1, 11.0 );
			SetSkill( SkillName.Wrestling, 10.1, 11.0 );

			Fame = 15;
			Karma = 0;

			Tamable = false;
		}

		public override int GetAngerSound()
		{
			return 0x4DA;
		}

		public override int GetIdleSound()
		{
			return 0x4D9;
		}

		public override int GetAttackSound()
		{
			return 0x4D8;
		}

		public override int GetHurtSound()
		{
			return 0x4DB;
		}

		public override int GetDeathSound()
		{
			return 0x4D7;
		}

		public override int Meat { get { return 1; } }
		public override MeatType MeatType { get { return MeatType.Bird; } }
		public override int Feathers { get { return 25; } }

		public Crane( Serial serial )
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