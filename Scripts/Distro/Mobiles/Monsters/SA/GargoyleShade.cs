using System;
using Server;

namespace Server.Mobiles
{
	[CorpseName( "a gargoyle shade corpse" )]
	public class GargoyleShade : BaseCreature
	{
		[Constructable]
		public GargoyleShade()
			: base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a gargoyle shade";
			Body = 4;
			BaseSoundID = 372;

			Hue = 0x4001;

			SetStr( 76, 110 );
			SetDex( 76, 100 );
			SetInt( 41, 65 );

			SetHits( 61, 81 );

			SetDamage( 7, 14 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 40, 55 );
			SetResistance( ResistanceType.Fire, 35, 45 );
			SetResistance( ResistanceType.Cold, 15, 20 );
			SetResistance( ResistanceType.Poison, 25, 35 );

			SetSkill( SkillName.MagicResist, 50.1, 65.0 );
			SetSkill( SkillName.Tactics, 40.1, 60.0 );
			SetSkill( SkillName.Wrestling, 40.1, 50.0 );
			SetSkill( SkillName.EvalInt, 50.1, 65.0 );
			SetSkill( SkillName.Magery, 50.1, 65.0 );

			Fame = 2500; // TODO: Verify
			Karma = -2500;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Average );
			AddLoot( LootPack.MedScrolls );
			AddLoot( LootPack.Gems, Utility.RandomMinMax( 1, 4 ) );
		}

		public override int TreasureMapLevel { get { return 1; } }
		public override int Meat { get { return 1; } }

		public GargoyleShade( Serial serial )
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
