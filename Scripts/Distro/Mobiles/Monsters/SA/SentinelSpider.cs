using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a sentinel spider corpse" )]
	public class SentinelSpider : BaseCreature
	{
		public override WeaponAbility GetWeaponAbility() { return WeaponAbility.ArmorIgnore; }

		[Constructable]
		public SentinelSpider()
			: base( AIType.AI_Melee, FightMode.Weakest, 10, 1, 0.2, 0.4 )
		{
			Name = "sentinel spider";
			Body = 157;

			Hue = 1141;

			SetStr( 93 );
			SetDex( 135 );
			SetInt( 87 );

			SetHits( 374 );

			SetDamage( 15, 22 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 40, 50 );
			SetResistance( ResistanceType.Fire, 30, 40 );
			SetResistance( ResistanceType.Cold, 30, 40 );
			SetResistance( ResistanceType.Poison, 70, 80 );
			SetResistance( ResistanceType.Energy, 30, 40 );

			SetSkill( SkillName.Anatomy, 69.5, 90.2 );
			SetSkill( SkillName.Healing, 109.9, 118.9 );
			SetSkill( SkillName.MagicResist, 80.0, 91.5 );
			SetSkill( SkillName.Tactics, 110.5, 117.9 );
			SetSkill( SkillName.Wrestling, 108.6, 122.0 );

			Fame = 18500;
			Karma = -18500;

			PackItem( new SpidersSilk( 8 ) );
			PackItem( new LesserPoisonPotion() );
			PackItem( new LesserPoisonPotion() );

			if ( 0.15 > Utility.RandomDouble() )
				PackItem( new BottleOfIchor() );
		}

		public override int GetAttackSound() { return 0x601; }
		public override int GetDeathSound() { return 0x602; }
		public override int GetHurtSound() { return 0x603; }
		public override int GetIdleSound() { return 0x604; }

		public override Poison PoisonImmune { get { return Poison.Lethal; } }
		public override Poison HitPoison { get { return Poison.Lethal; } }

		public override void GenerateLoot()
		{
			AddLoot( LootPack.UltraRich );
		}

		public SentinelSpider( Serial serial )
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
