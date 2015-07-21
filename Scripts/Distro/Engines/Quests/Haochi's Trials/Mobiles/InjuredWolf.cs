using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Engines.Quests.SE
{
	public class InjuredWolf : BaseCreature
	{
		[Constructable]
		public InjuredWolf()
			: base( AIType.AI_Melee, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			Body = 0xE1;
			Name = "an injured wolf";

			SetStr( 15, 30 );
			SetDex( 45, 65 );
			SetInt( 15, 30 );

			SetHits( 1 );

			SetDamage( 1, 3 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 18 );
			SetResistance( ResistanceType.Fire, 10 );

			SetSkill( SkillName.Magery, 120.0 );
			SetSkill( SkillName.Meditation, 25.0 );
			SetSkill( SkillName.MagicResist, 10.0 );
			SetSkill( SkillName.Tactics, 0.0, 5.0 );
			SetSkill( SkillName.Wrestling, 20.0, 30.0 );

			Fame = 1000;
			Karma = 1000;
		}

		public override bool PlayerRangeSensitive { get { return false; } }

		public InjuredWolf( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();

			Delete();
		}
	}
}
