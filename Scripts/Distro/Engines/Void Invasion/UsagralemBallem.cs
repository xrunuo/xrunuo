using System;
using Server;
using Server.Items;
using Server.Engines.Loyalty;

namespace Server.Mobiles
{
	[CorpseName( "an Usagralem Ballem's corpse" )]
	public class UsagralemBallem : VoidCreature
	{
		public override WeaponAbility GetWeaponAbility()
		{
			return WeaponAbility.WhirlwindAttack;
		}

		[Constructable]
		public UsagralemBallem()
			: base( AIType.AI_Mystic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Usagralem Ballem";
			Body = 318;
			BaseSoundID = 0x165;

			Hue = 2069;

			SetStr( 963, 983 );
			SetDex( 1028, 1088 );
			SetInt( 1014, 1042 );

			SetHits( 2099, 2125 );
			SetMana( 5000 );

			SetDamage( 17, 21 );

			SetDamageType( ResistanceType.Physical, 20 );
			SetDamageType( ResistanceType.Fire, 20 );
			SetDamageType( ResistanceType.Cold, 20 );
			SetDamageType( ResistanceType.Poison, 20 );
			SetDamageType( ResistanceType.Energy, 20 );

			SetResistance( ResistanceType.Physical, 30, 60 );
			SetResistance( ResistanceType.Fire, 30, 60 );
			SetResistance( ResistanceType.Cold, 30, 60 );
			SetResistance( ResistanceType.Poison, 30, 60 );
			SetResistance( ResistanceType.Energy, 30, 60 );

			SetSkill( SkillName.MagicResist, 80.1, 90.0 );
			SetSkill( SkillName.Tactics, 85.1, 95.0 );
			SetSkill( SkillName.Wrestling, 80.1, 90.0 );
			SetSkill( SkillName.Mysticism, 90.1, 100.0 );

			Fame = 2500;
			Karma = -2500;

			PackItem( new DaemonBone( 30 ) );
			PackItem( new VoidEssence() );
			PackItem( new VoidOrb() );

			for ( int i = 0; i < 3; i++ )
				PackMysticScroll( Utility.Random( 10, 6 ) ); // 6th - 8th circle
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.UltraRich, 2 );
		}

		public override LoyaltyGroup LoyaltyGroupEnemy { get { return LoyaltyGroup.GargoyleQueen; } }
		public override int LoyaltyPointsAward { get { return 50; } }

		public override bool PlayerRangeSensitive { get { return false; } }

		public UsagralemBallem( Serial serial )
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
