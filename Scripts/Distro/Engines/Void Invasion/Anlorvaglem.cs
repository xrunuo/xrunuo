using System;
using Server;
using Server.Items;
using Server.Engines.Loyalty;

namespace Server.Mobiles
{
	[CorpseName( "an Anlorvaglem's corpse" )]
	public class Anlorvaglem : VoidCreature
	{
		[Constructable]
		public Anlorvaglem()
			: base( AIType.AI_Arcanist, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Anlorvaglem";
			Body = 316;
			BaseSoundID = 377;

			Hue = 2071;

			SetStr( 1026, 1187 );
			SetDex( 1014, 1165 );
			SetInt( 1016, 1193 );

			SetHits( 3031, 3334 );

			SetDamage( 11, 13 );

			SetDamageType( ResistanceType.Physical, 20 );
			SetDamageType( ResistanceType.Fire, 20 );
			SetDamageType( ResistanceType.Cold, 20 );
			SetDamageType( ResistanceType.Poison, 20 );
			SetDamageType( ResistanceType.Energy, 20 );

			SetResistance( ResistanceType.Physical, 30, 60 );
			SetResistance( ResistanceType.Fire, 30, 60 );
			SetResistance( ResistanceType.Cold, 30, 60 );
			SetResistance( ResistanceType.Poison, 100 );
			SetResistance( ResistanceType.Energy, 30, 60 );

			SetSkill( SkillName.MagicResist, 50.1, 100.0 );
			SetSkill( SkillName.Tactics, 50.1, 100.0 );
			SetSkill( SkillName.Wrestling, 50.1, 100.0 );
			SetSkill( SkillName.Mysticism, 50.1, 100.0 );

			Fame = 2500;
			Karma = -2500;

			PackItem( new DaemonBone( 30 ) );
			PackItem( new VoidEssence() );
			PackItem( new VoidOrb() );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.UltraRich );
		}

		public override LoyaltyGroup LoyaltyGroupEnemy { get { return LoyaltyGroup.GargoyleQueen; } }
		public override int LoyaltyPointsAward { get { return 50; } }

		public override bool PlayerRangeSensitive { get { return false; } }

		public Anlorvaglem( Serial serial )
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
