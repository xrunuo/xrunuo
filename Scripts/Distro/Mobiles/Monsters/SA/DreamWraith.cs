using System;
using Server;
using Server.Items;
using Server.Engines.Loyalty;

namespace Server.Mobiles
{
	[CorpseName( "a dream wraith corpse" )]
	public class DreamWraith : BaseCreature
	{
		[Constructable]
		public DreamWraith()
			: base( AIType.AI_Necromancer, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a dream wraith";
			Body = 740;

			SetStr( 183, 264 );
			SetDex( 81, 101 );
			SetInt( 551, 596 );

			SetHits( 502, 592 );

			SetDamage( 18, 25 );

			SetDamageType( ResistanceType.Physical, 10 );
			SetDamageType( ResistanceType.Cold, 45 );
			SetDamageType( ResistanceType.Energy, 45 );

			SetResistance( ResistanceType.Physical, 50, 60 );
			SetResistance( ResistanceType.Fire, 40, 50 );
			SetResistance( ResistanceType.Cold, 40, 50 );
			SetResistance( ResistanceType.Poison, 40, 50 );
			SetResistance( ResistanceType.Energy, 20, 30 );

			SetSkill( SkillName.MagicResist, 120.1, 150.0 );
			SetSkill( SkillName.Tactics, 70.1, 80.0 );
			SetSkill( SkillName.Wrestling, 90.1, 100.0 );
			SetSkill( SkillName.Magery, 100.1, 120.0 );
			SetSkill( SkillName.EvalInt, 100.1, 120.0 );
			SetSkill( SkillName.Meditation, 100.1, 110.0 );
			SetSkill( SkillName.Necromancy, 100.1, 120.0 );
			SetSkill( SkillName.SpiritSpeak, 100.1, 120.0 );

			Fame = 18000;
			Karma = -18000;

			PackMysticScroll( Utility.Random( 10, 6 ) );
			PackMysticScroll( Utility.Random( 10, 6 ) );
		}

		public override bool BardImmune { get { return true; } }
		public override bool BleedImmune { get { return true; } }
		public override bool CanRummageCorpses { get { return true; } }

		public override Poison HitPoison { get { return Poison.Lethal; } }
		public override Poison PoisonImmune { get { return Poison.Lethal; } }
		public override int TreasureMapLevel { get { return 4; } }

		public override LoyaltyGroup LoyaltyGroupEnemy { get { return LoyaltyGroup.GargoyleQueen; } }
		public override int LoyaltyPointsAward { get { return 5; } }
		public override SlayerName SlayerGroup { get { return SlayerName.Undead; } }

		public override int GetAttackSound() { return 0x646; }
		public override int GetDeathSound() { return 0x647; }
		public override int GetHurtSound() { return 0x648; }
		public override int GetIdleSound() { return 0x649; }

		public override void GenerateLoot()
		{
			AddLoot( LootPack.UltraRich, 2 );
		}

		public DreamWraith( Serial serial )
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
