using System;
using Server;
using Server.Engines.Loyalty;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a Clan Ribbon Supplicant's corpse" )]
	public class ClanRibbonSupplicant : BaseCreature
	{
		[Constructable]
		public ClanRibbonSupplicant()
			: base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Clan Ribbon Supplicant";
			Body = 42;
			BaseSoundID = 437;

			Hue = 2967;

			SetStr( 147, 175 );
			SetDex( 107, 129 );
			SetInt( 187, 207 );

			SetHits( 103, 140 );

			SetDamage( 7, 14 );

			SetDamageType( ResistanceType.Physical, 80 );
			SetDamageType( ResistanceType.Cold, 20 );

			SetResistance( ResistanceType.Physical, 50, 60 );
			SetResistance( ResistanceType.Fire, 30, 50 );
			SetResistance( ResistanceType.Cold, 60, 80 );
			SetResistance( ResistanceType.Poison, 30, 50 );
			SetResistance( ResistanceType.Energy, 20, 30 );

			SetSkill( SkillName.MagicResist, 72, 89 );
			SetSkill( SkillName.Tactics, 54.4, 72.9 );
			SetSkill( SkillName.Wrestling, 53.2, 69.9 );

			Fame = 7500;
			Karma = -7500;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Average );

			if ( 0.1 > Utility.RandomDouble() )
				AddLoot( LootPack.CavernIngredients );
		}

		public override LoyaltyGroup LoyaltyGroupEnemy { get { return LoyaltyGroup.GargoyleQueen; } }
		public override int LoyaltyPointsAward { get { return 2; } }

		public override int Meat { get { return 1; } }
		public override int Hides { get { return 8; } }
		public override int TreasureMapLevel { get { return 2; } }
		public override SlayerName SlayerGroup { get { return SlayerName.Repond; } }

		public ClanRibbonSupplicant( Serial serial )
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
