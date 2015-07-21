using System;
using Server;

namespace Server.Mobiles
{
	[CorpseName( "a clockwork scorpion corpse" )]
	public class ClockworkScorpion : BaseCreature
	{
		[Constructable]
		public ClockworkScorpion()
			: base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a clockwork scorpion";
			Body = 717;
			BaseSoundID = 397;

			SetStr( 220, 266 );
			SetDex( 77, 92 );
			SetInt( 20, 28 );

			SetHits( 73, 81 );
			SetMana( 0 );

			SetDamage( 5, 10 );

			SetDamageType( ResistanceType.Physical, 60 );
			SetDamageType( ResistanceType.Poison, 40 );

			SetResistance( ResistanceType.Physical, 80, 90 );
			SetResistance( ResistanceType.Fire, 10, 30 );
			SetResistance( ResistanceType.Cold, 60, 70 );
			SetResistance( ResistanceType.Poison, 100 );
			SetResistance( ResistanceType.Energy, 10, 15 );

			SetSkill( SkillName.Poisoning, 80.0, 95.0 );
			SetSkill( SkillName.MagicResist, 30.0, 35.0 );
			SetSkill( SkillName.Tactics, 60.0, 70.0 );
			SetSkill( SkillName.Wrestling, 50.0, 65.0 );

			Fame = 2000;
			Karma = -2000;

			ControlSlots = 1;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Average );
		}

		public override int Meat { get { return 1; } }
		public override Poison HitPoison { get { return Poison.Greater; } }

		public ClockworkScorpion( Serial serial )
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