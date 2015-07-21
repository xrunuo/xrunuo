using System;
using Server;
using Server.Items;
using Server.Misc;
using Server.Engines.Loyalty;

namespace Server.Mobiles
{
	[CorpseName( "Tikitavi's corpse" )]
	public class TikitaviRenowned : BaseCreature, IRenowned
	{
		public override InhumanSpeech SpeechType { get { return InhumanSpeech.Ratman; } }

		private static readonly Type[] m_Rewards = new Type[]
			{
				typeof( BasiliskHideBreastplate ),
				typeof( LegacyOfDespair ),
				typeof( MysticsGarb )
			};

		public Type[] Rewards { get { return m_Rewards; } }

		public override bool AlwaysMurderer { get { return true; } }

		[Constructable]
		public TikitaviRenowned()
			: base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Tikitavi [Renowned]";
			Body = 42;
			BaseSoundID = 437;

			Hue = 32905;

			SetStr( 307, 373 );
			SetDex( 106, 171 );
			SetInt( 253, 297 );

			SetHits( 40000 );

			SetDamage( 7, 9 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 25, 30 );
			SetResistance( ResistanceType.Fire, 20, 30 );
			SetResistance( ResistanceType.Cold, 30, 50 );
			SetResistance( ResistanceType.Poison, 10, 20 );
			SetResistance( ResistanceType.Energy, 10, 20 );

			SetSkill( SkillName.MagicResist, 40.6, 57.8 );
			SetSkill( SkillName.Tactics, 65.3, 83.3 );
			SetSkill( SkillName.Wrestling, 60.4, 84.2 );

			Fame = 6000;
			Karma = -6000;

			PackGold( 1500 );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.UltraRich, 2 );
			AddLoot( LootPack.RareGems );

			if ( 0.3 > Utility.RandomDouble() )
				AddLoot( LootPack.CavernIngredients );
		}

		public override LoyaltyGroup LoyaltyGroupEnemy { get { return LoyaltyGroup.GargoyleQueen; } }
		public override int LoyaltyPointsAward { get { return 50; } }

		public override bool CanRummageCorpses { get { return true; } }
		public override int Hides { get { return 8; } }
		public override int Meat { get { return 1; } }

		public TikitaviRenowned( Serial serial )
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
