using System;
using Server;
using Server.Engines.Loyalty;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a Clan Ribbon Courtier's corpse" )]
	public class ClanRibbonCourtier : BaseCreature
	{
		[Constructable]
		public ClanRibbonCourtier()
			: base( AIType.AI_Arcanist, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Clan Ribbon Courtier";
			Body = 143;
			BaseSoundID = 437;

			Hue = 1196;

			SetStr( 205, 248 );
			SetDex( 236, 250 );
			SetInt( 123, 139 );

			SetHits( 2072, 2146 );

			SetDamage( 7, 14 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 40, 50 );
			SetResistance( ResistanceType.Fire, 10, 20 );
			SetResistance( ResistanceType.Cold, 10, 20 );
			SetResistance( ResistanceType.Poison, 10, 20 );
			SetResistance( ResistanceType.Energy, 10, 20 );

			SetSkill( SkillName.MagicResist, 76.1, 108.8 );
			SetSkill( SkillName.Tactics, 50.8, 62.4 );
			SetSkill( SkillName.Wrestling, 50.7, 70.2 );
			SetSkill( SkillName.Spellweaving, 71.8, 87.1 );

			Fame = 6000;
			Karma = -6000;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich );

			if ( 0.2 > Utility.RandomDouble() )
				AddLoot( LootPack.CavernIngredients );
		}

		public override int Meat { get { return 1; } }
		public override int Hides { get { return 8; } }
		public override int TreasureMapLevel { get { return 2; } }

		public override LoyaltyGroup LoyaltyGroupEnemy { get { return LoyaltyGroup.GargoyleQueen; } }
		public override int LoyaltyPointsAward { get { return 5; } }
		public override SlayerName SlayerGroup { get { return SlayerName.Repond; } }

		public ClanRibbonCourtier( Serial serial )
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
