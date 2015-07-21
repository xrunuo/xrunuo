using System;
using Server;

namespace Server.Mobiles
{
	[CorpseName( "a chicken lizard corpse" )]
	public class ChickenLizard : BaseCreature
	{
		[Constructable]
		public ChickenLizard()
			: base( AIType.AI_Melee, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			Name = "a chicken lizard";
			Body = 716;

			SetStr( 75, 95 );
			SetDex( 75, 95 );
			SetInt( 6, 10 );

			SetHits( 75, 85 );

			SetDamage( 2, 5 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 15, 20 );
			SetResistance( ResistanceType.Fire, 5, 15 );

			SetSkill( SkillName.MagicResist, 25.0, 30.0 );
			SetSkill( SkillName.Tactics, 30.0, 45.0 );
			SetSkill( SkillName.Wrestling, 25.0, 40.0 );

			Fame = 300;
			Karma = -300;

			Tamable = true;
			MinTameSkill = 0.0;
			ControlSlots = 1;

			// TODO (SA): Chance to drop A chicken lizard egg
		}

		public override int GetAttackSound() { return 0x5DC; }
		public override int GetDeathSound() { return 0x5DD; }
		public override int GetHurtSound() { return 0x5DE; }
		public override int GetIdleSound() { return 0x5DF; }

		public override MeatType MeatType { get { return MeatType.Bird; } }
		public override int Meat { get { return 3; } }
		public override int Blood { get { return 4; } }

		public ChickenLizard( Serial serial )
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
