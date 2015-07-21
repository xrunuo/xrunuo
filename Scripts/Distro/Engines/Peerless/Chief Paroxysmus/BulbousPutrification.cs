using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a bulbous putrification corpse" )]
	public class BulbousPutrification : BaseCreature
	{
		[Constructable]
		public BulbousPutrification()
			: base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a bulbous putrification";
			Body = 775;
			Hue = 0x55C;
			BaseSoundID = 0x165;

			SetStr( 750, 800 );
			SetDex( 50, 60 );
			SetInt( 50, 60 );

			SetHits( 1200, 1250 );

			SetDamage( 24, 28 );

			SetDamageType( ResistanceType.Physical, 60 );
			SetDamageType( ResistanceType.Poison, 40 );

			SetResistance( ResistanceType.Physical, 55, 70 );
			SetResistance( ResistanceType.Fire, 40, 50 );
			SetResistance( ResistanceType.Cold, 40, 50 );
			SetResistance( ResistanceType.Poison, 50, 70 );
			SetResistance( ResistanceType.Energy, 50, 60 );

			SetSkill( SkillName.MagicResist, 50.1, 65.0 );
			SetSkill( SkillName.Tactics, 110.1, 120.0 );
			SetSkill( SkillName.Wrestling, 100.1, 115.0 );
			SetSkill( SkillName.Anatomy, 110.0 );
			SetSkill( SkillName.Poisoning, 80.0 );

			Fame = 15000;
			Karma = -15000;
		}

		public override Poison HitPoison { get { return Poison.Lethal; } }
		public override Poison PoisonImmune { get { return Poison.Lethal; } }

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich, 4 );
		}

		public BulbousPutrification( Serial serial )
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