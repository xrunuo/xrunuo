using System;
using Server;
using Server.Misc;
using Server.Engines.Loyalty;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a Clan Chitter Tinkerer's corpse" )]
	public class ClanChitterTinkerer : BaseCreature
	{
		public override InhumanSpeech SpeechType { get { return InhumanSpeech.Ratman; } }

		[Constructable]
		public ClanChitterTinkerer()
			: base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Clan Chitter Tinkerer";
			Body = 142;
			BaseSoundID = 437;

			SetStr( 298, 333 );
			SetDex( 227, 252 );
			SetInt( 237, 268 );

			SetHits( 2034, 2093 );

			SetDamage( 8, 10 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 20, 30 );
			SetResistance( ResistanceType.Fire, 20, 30 );
			SetResistance( ResistanceType.Cold, 30, 50 );
			SetResistance( ResistanceType.Poison, 10, 20 );
			SetResistance( ResistanceType.Energy, 10, 20 );

			SetSkill( SkillName.MagicResist, 75.0, 100.0 );
			SetSkill( SkillName.Tactics, 60.0, 85.0 );
			SetSkill( SkillName.Wrestling, 60.0, 85.0 );

			Fame = 7000;
			Karma = -7000;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich );

			if ( 0.2 > Utility.RandomDouble() )
				AddLoot( LootPack.CavernIngredients );
		}

		public override LoyaltyGroup LoyaltyGroupEnemy { get { return LoyaltyGroup.GargoyleQueen; } }
		public override int LoyaltyPointsAward { get { return 5; } }

		public override bool CanRummageCorpses { get { return true; } }
		public override int Meat { get { return 1; } }
		public override int Hides { get { return 8; } }
		public override int TreasureMapLevel { get { return 2; } }
		public override SlayerName SlayerGroup { get { return SlayerName.Repond; } }

		public ClanChitterTinkerer( Serial serial )
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
