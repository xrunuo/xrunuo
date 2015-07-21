using System;
using Server;
using Server.Items;
using Server.Engines.Loyalty;

namespace Server.Mobiles
{
	[CorpseName( "a kepetch ambusher corpse" )]
	public class KepetchAmbusher : BaseCreature
	{
		[Constructable]
		public KepetchAmbusher()
			: base( AIType.AI_Ambusher, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a kepetch ambusher";
			Body = 726;

			Hue = 33018;

			SetStr( 437, 446 );
			SetDex( 231, 254 );
			SetInt( 40, 46 );

			SetHits( 524, 544 );

			SetDamage( 7, 17 );

			SetDamageType( ResistanceType.Physical, 80 );
			SetDamageType( ResistanceType.Poison, 20 );

			SetResistance( ResistanceType.Physical, 70, 75 );
			SetResistance( ResistanceType.Fire, 55, 60 );
			SetResistance( ResistanceType.Cold, 50, 55 );
			SetResistance( ResistanceType.Poison, 55, 60 );
			SetResistance( ResistanceType.Energy, 65, 70 );

			SetSkill( SkillName.Anatomy, 104.2, 108.1 );
			SetSkill( SkillName.MagicResist, 88.5, 91.4 );
			SetSkill( SkillName.Tactics, 102, 109.5 );
			SetSkill( SkillName.Wrestling, 103.2, 106.6 );
			SetSkill( SkillName.Stealth, 98.1, 102.5 );
			SetSkill( SkillName.Hiding, 99.5, 101.3 );

			Fame = 2500;
			Karma = -2500;
		}

		public override int GetAttackSound() { return 0x606; }
		public override int GetDeathSound() { return 0x607; }
		public override int GetHurtSound() { return 0x608; }
		public override int GetIdleSound() { return 0x609; }

		public override LoyaltyGroup LoyaltyGroupEnemy { get { return LoyaltyGroup.GargoyleQueen; } }
		public override int LoyaltyPointsAward { get { return 25; } }

		public override int Meat { get { return 5; } }
		public override int Blood { get { return 6; } }
		public override int Hides { get { return 12; } }
		public override HideType HideType { get { return HideType.Horned; } }

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich, 2 );
		}

		public KepetchAmbusher( Serial serial )
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
