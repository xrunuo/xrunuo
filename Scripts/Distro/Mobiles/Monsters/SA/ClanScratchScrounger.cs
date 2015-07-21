using System;
using Server;
using Server.Misc;
using Server.Engines.Loyalty;

namespace Server.Mobiles
{
	[CorpseName( "a Clan Scratch Scrounger's corpse" )]
	public class ClanScratchScrounger : BaseCreature
	{
		public override InhumanSpeech SpeechType { get { return InhumanSpeech.Ratman; } }

		[Constructable]
		public ClanScratchScrounger()
			: base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Clan Scratch Scrounger";
			Body = 42;
			BaseSoundID = 437;

			SetStr( 99, 118 );
			SetDex( 84, 92 );
			SetInt( 37, 59 );

			SetHits( 117, 155 );

			SetDamage( 4, 5 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 25, 30 );
			SetResistance( ResistanceType.Fire, 20, 30 );
			SetResistance( ResistanceType.Cold, 35, 45 );
			SetResistance( ResistanceType.Poison, 10, 20 );
			SetResistance( ResistanceType.Energy, 10, 20 );

			SetSkill( SkillName.Anatomy, 51.1, 71.6 );
			SetSkill( SkillName.MagicResist, 35.4, 55.9 );
			SetSkill( SkillName.Tactics, 56.1, 70.9 );
			SetSkill( SkillName.Wrestling, 50.5, 69.4 );

			Fame = 1500;
			Karma = -1500;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Average );

			if ( 0.1 > Utility.RandomDouble() )
				AddLoot( LootPack.CavernIngredients );
		}

		public override LoyaltyGroup LoyaltyGroupEnemy { get { return LoyaltyGroup.GargoyleQueen; } }
		public override int LoyaltyPointsAward { get { return 3; } }

		public override bool CanRummageCorpses { get { return true; } }
		public override int Meat { get { return 1; } }
		public override int Hides { get { return 8; } }
		public override int TreasureMapLevel { get { return 2; } }		

		public ClanScratchScrounger( Serial serial )
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
