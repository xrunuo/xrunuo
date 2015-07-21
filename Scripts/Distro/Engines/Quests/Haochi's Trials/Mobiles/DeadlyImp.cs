using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Engines.Quests.SE
{
	public class DeadlyImp : BaseCreature
	{
		[Constructable]
		public DeadlyImp()
			: base( AIType.AI_Mage, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			Body = 0x4A;
			Name = "a deadly imp";

			Hue = 0x66A;

			Frozen = true;

			SetStr( 75 );
			SetDex( 0 );
			SetInt( 94 );

			SetDamage( 1000 );

			SetDamageType( ResistanceType.Fire, 100 );

			SetResistance( ResistanceType.Physical, 99 );
			SetResistance( ResistanceType.Fire, 99 );
			SetResistance( ResistanceType.Cold, 96 );
			SetResistance( ResistanceType.Poison, 98 );
			SetResistance( ResistanceType.Energy, 98 );

			SetSkill( SkillName.Magery, 120.0 );
			SetSkill( SkillName.Meditation, 25.0 );
			SetSkill( SkillName.Tactics, 120.0 );
			SetSkill( SkillName.Wrestling, 120.0 );

			Fame = 15000;
			Karma = 15000;

			CantWalk = true;
		}

		public override bool PlayerRangeSensitive { get { return false; } }

		public DeadlyImp( Serial serial )
			: base( serial )
		{
		}

		public override void OnGotMeleeAttack( Mobile attacker )
		{
			attacker.Kill();
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
