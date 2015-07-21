using System;
using Server;
using Server.Items;
using Server.Engines.Loyalty;

namespace Server.Mobiles
{
	[CorpseName( "a Ballem's corpse" )]
	public class Ballem : VoidCreature
	{
		protected override void CreateEvolutionHandlers()
		{
			AddEvolutionHandler( new KillingPathHandler( this, typeof( UsagralemBallem ), 20000 ) );
		}

		public override WeaponAbility GetWeaponAbility()
		{
			return WeaponAbility.CrushingBlow;
		}

		[Constructable]
		public Ballem()
			: base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Ballem";
			Body = 792;
			BaseSoundID = 0x3E9;

			Hue = 2070;

			SetStr( 1012, 1101 );
			SetDex( 1040, 1093 );
			SetInt( 246, 275 );

			SetHits( 430, 573 );

			SetDamage( 10, 15 );

			SetDamageType( ResistanceType.Physical, 20 );
			SetDamageType( ResistanceType.Fire, 20 );
			SetDamageType( ResistanceType.Cold, 20 );
			SetDamageType( ResistanceType.Poison, 20 );
			SetDamageType( ResistanceType.Energy, 20 );

			SetResistance( ResistanceType.Physical, 34, 47 );
			SetResistance( ResistanceType.Fire, 20, 48 );
			SetResistance( ResistanceType.Cold, 21, 49 );
			SetResistance( ResistanceType.Poison, 100, 100 );
			SetResistance( ResistanceType.Energy, 24, 48 );

			SetSkill( SkillName.MagicResist, 61.2, 71.2 );
			SetSkill( SkillName.Tactics, 53, 89.9 );
			SetSkill( SkillName.Wrestling, 58.7, 77.5 );

			Fame = 2000;
			Karma = -2000;

			PackItem( new DaemonBone( 15 ) );

			if ( 0.2 > Utility.RandomDouble() )
				PackItem( new VoidOrb() );

			if ( 0.2 > Utility.RandomDouble() )
				PackItem( new VoidEssence() );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich );
			AddLoot( LootPack.Rich );
		}

		public override LoyaltyGroup LoyaltyGroupEnemy { get { return LoyaltyGroup.GargoyleQueen; } }
		public override int LoyaltyPointsAward { get { return 20; } }

		public Ballem( Serial serial )
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
