using System;
using Server;

namespace Server.Mobiles
{
	[CorpseName( "a vollem's corpse" )]
	public class Vollem : BaseCreature
	{
		[Constructable]
		public Vollem()
			: base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a vollem";
			Body = 0x125;

			SetStr( 496, 524 );
			SetDex( 88, 105 );
			SetInt( 94, 117 );

			SetHits( 300, 315 );

			SetDamage( 16, 22 );

			SetDamageType( ResistanceType.Physical, 40 );
			SetDamageType( ResistanceType.Fire, 40 );
			SetDamageType( ResistanceType.Energy, 20 );

			SetResistance( ResistanceType.Physical, 55, 65 );
			SetResistance( ResistanceType.Fire, 30, 40 );
			SetResistance( ResistanceType.Cold, 30, 40 );
			SetResistance( ResistanceType.Poison, 30, 40 );
			SetResistance( ResistanceType.Energy, 30, 30 );

			SetSkill( SkillName.MagicResist, 90.4, 97.8 );
			SetSkill( SkillName.Tactics, 99.0, 99.4 );
			SetSkill( SkillName.Wrestling, 84.2, 87.7 );
			SetSkill( SkillName.EvalInt, 21.9, 43.3 );
			SetSkill( SkillName.Magery, 25.6, 38.6 );

			ControlSlots = 2;
		}

		public override int Meat { get { return 5; } }
		public override int Hides { get { return 10; } }

		public Vollem( Serial serial )
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
