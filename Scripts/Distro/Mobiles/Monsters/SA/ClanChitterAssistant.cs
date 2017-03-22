using System;
using Server;
using Server.Misc;
using Server.Engines.Loyalty;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a Clan Chitter Assistant's corpse" )]
	public class ClanChitterAssistant : BaseCreature
	{
		public override InhumanSpeech SpeechType { get { return InhumanSpeech.Ratman; } }

		[Constructable]
		public ClanChitterAssistant()
			: base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Clan Chitter Assistant";
			Body = 142;
			BaseSoundID = 437;

			SetStr( 148, 177 );
			SetDex( 101, 129 );
			SetInt( 119, 138 );

			SetHits( 103, 149 );

			SetDamage( 4, 10 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 21, 35 );
			SetResistance( ResistanceType.Fire, 22, 30 );
			SetResistance( ResistanceType.Cold, 32, 49 );
			SetResistance( ResistanceType.Poison, 10, 19 );
			SetResistance( ResistanceType.Energy, 11, 19 );

			SetSkill( SkillName.MagicResist, 68.1, 86.3 );
			SetSkill( SkillName.Tactics, 54.5, 72.4 );
			SetSkill( SkillName.Wrestling, 56, 73.9 );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Average );

			if ( 0.1 > Utility.RandomDouble() )
				AddLoot( LootPack.CavernIngredients );
		}

		public override LoyaltyGroup LoyaltyGroupEnemy { get { return LoyaltyGroup.GargoyleQueen; } }
		public override int LoyaltyPointsAward { get { return 2; } }

		public override bool CanRummageCorpses { get { return true; } }
		public override int Meat { get { return 1; } }
		public override int Hides { get { return 8; } }
		public override int TreasureMapLevel { get { return 2; } }
		public override SlayerName SlayerGroup { get { return SlayerName.Repond; } }

		public ClanChitterAssistant( Serial serial )
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
