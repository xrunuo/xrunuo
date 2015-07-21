using System;
using Server;
using Server.Misc;
using Server.Engines.Loyalty;

namespace Server.Mobiles
{
	[CorpseName("a Clan Scratch Henchrat's corpse" )]
	public class ClanScratchHenchrat : BaseCreature
	{
		public override InhumanSpeech SpeechType { get { return InhumanSpeech.Ratman; } }

		[Constructable]
		public ClanScratchHenchrat()
			: base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Clan Scratch Henchrat";
			Body = 42;
			BaseSoundID = 437;

			SetStr( 235, 249 );
			SetDex( 168, 198 );
			SetInt( 73, 115 );

			SetHits( 2048, 2106 );

			SetDamage( 5, 7 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 25, 30 );
			SetResistance( ResistanceType.Fire, 20, 30 );
			SetResistance( ResistanceType.Cold, 30, 50 );
			SetResistance( ResistanceType.Poison, 10, 20 );
			SetResistance( ResistanceType.Energy, 10, 20 );

			SetSkill( SkillName.Anatomy, 50.4, 63.7 );
			SetSkill( SkillName.MagicResist, 38.0, 58.4 );
			SetSkill( SkillName.Tactics, 50.5, 62.1 );
			SetSkill( SkillName.Wrestling, 51.5, 63.4 );

			Fame = 4000;
			Karma = -4000;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Average );

			if ( 0.2 > Utility.RandomDouble() )
				AddLoot( LootPack.CavernIngredients );
		}

		public override LoyaltyGroup LoyaltyGroupEnemy { get { return LoyaltyGroup.GargoyleQueen; } }
		public override int LoyaltyPointsAward { get { return 5; } }

		public override bool CanRummageCorpses { get { return true; } }
		public override int Meat { get { return 1; } }
		public override int Hides { get { return 8; } }
		public override int TreasureMapLevel { get { return 2; } }		

		public ClanScratchHenchrat( Serial serial )
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
