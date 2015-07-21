using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a cursed soul corpse" )]
	public class CursedSoul : BaseCreature
	{
		[Constructable]
		public CursedSoul()
			: base( AIType.AI_Melee, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			Name = "a cursed soul";
			Body = 3;
			BaseSoundID = 471;

			SetStr( 17, 40 );
			SetDex( 31, 50 );
			SetInt( 11, 21 );

			SetHits( 11 );

			SetDamage( 3, 7 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 12, 21 );
			SetResistance( ResistanceType.Fire, 5, 9 );

			SetSkill( SkillName.MagicResist, 10.1, 25.0 );
			SetSkill( SkillName.Wrestling, 35.1, 50.0 );

			Fame = 600;
			Karma = -600;
		}

		public override Poison PoisonImmune { get { return Poison.Regular; } }
		public override bool PlayerRangeSensitive { get { return false; } }

		public CursedSoul( Serial serial )
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
