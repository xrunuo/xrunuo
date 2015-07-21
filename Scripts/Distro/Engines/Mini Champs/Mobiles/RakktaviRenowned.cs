using System;
using Server;
using Server.Items;
using Server.Misc;
using Server.Engines.Loyalty;

namespace Server.Mobiles
{
	[CorpseName( "Rakktavi's corpse" )]
	public class RakktaviRenowned : BaseCreature, IRenowned
	{
		public override InhumanSpeech SpeechType { get { return InhumanSpeech.Ratman; } }

		private static readonly Type[] m_Rewards = new Type[]
			{
				typeof( CavalrysFolly ),
				typeof( TorcOfTheGuardians ),
				typeof( GiantSteps )
			};

		public Type[] Rewards { get { return m_Rewards; } }

		public override bool AlwaysMurderer { get { return true; } }

		[Constructable]
		public RakktaviRenowned()
			: base( AIType.AI_Archer, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Rakktavi [Renowned]";
			Body = 142;
			BaseSoundID = 437;

			Hue = 32905;

			SetStr( 127, 175 );
			SetDex( 244, 285 );
			SetInt( 305, 347 );

			SetHits( 40000 );

			SetDamage( 8, 10 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 20, 50 );
			SetResistance( ResistanceType.Fire, 20, 30 );
			SetResistance( ResistanceType.Cold, 30, 40 );
			SetResistance( ResistanceType.Poison, 10, 20 );
			SetResistance( ResistanceType.Energy, 10, 20 );

			SetSkill( SkillName.MagicResist, 68.2, 87.8 );
			SetSkill( SkillName.Tactics, 60.3, 78.6 );
			SetSkill( SkillName.Wrestling, 65.3, 79.0 );
			SetSkill( SkillName.Archery, 63.4, 78.8 );

			Fame = 8000;
			Karma = -8000;

			PackGold( 1500 );

			AddItem( new Bow() );
			PackItem( new Arrow( Utility.RandomMinMax( 10, 30 ) ) );
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

		public RakktaviRenowned( Serial serial )
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
