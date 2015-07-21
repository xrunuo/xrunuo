using System;
using Server;

namespace Server.Mobiles
{
	[CorpseName( "a doppelgangers corpse" )]
	public class Doppelganger : BaseCreature
	{
		[Constructable]
		public Doppelganger()
			: base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Body = 777;
			Name = "a doppelganger";

			SetStr( 81, 110 );
			SetDex( 56, 75 );
			SetInt( 81, 105 );

			SetHits( 101, 120 );

			SetDamage( 11, 18 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 50, 60 );
			SetResistance( ResistanceType.Fire, 10, 20 );
			SetResistance( ResistanceType.Cold, 40, 50 );
			SetResistance( ResistanceType.Poison, 50, 60 );
			SetResistance( ResistanceType.Energy, 30, 40 );

			SetSkill( SkillName.Wrestling, 80.1, 90 );
			SetSkill( SkillName.Tactics, 70.1, 80 );
			SetSkill( SkillName.MagicResist, 75.1, 85 );

			SetFameLevel( 1 );
			SetKarmaLevel( 1 );
		}

		public Doppelganger( Serial serial )
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