using System;
using Server;
using Server.Items;
using Server.Misc;
using Server.Engines.Loyalty;

namespace Server.Mobiles
{
	[CorpseName( "Vitavi's corpse" )]
	public class VitaviRenowned : BaseCreature, IRenowned
	{
		public override InhumanSpeech SpeechType { get { return InhumanSpeech.Ratman; } }

		private static readonly Type[] m_Rewards = new Type[]
			{
				typeof( AxeOfAbandon ),
				typeof( DemonBridleRing ),
				typeof( VoidInfusedKilt )
			};

		public Type[] Rewards { get { return m_Rewards; } }

		public override bool AlwaysMurderer { get { return true; } }

		[Constructable]
		public VitaviRenowned()
			: base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Vitavi [Renowned]";
			Body = 143;
			BaseSoundID = 437;

			Hue = 32905;

			SetStr( 310, 360 );
			SetDex( 241, 278 );
			SetInt( 318, 373 );

			SetHits( 40000 );

			SetDamage( 7, 14 );

			SetDamageType( ResistanceType.Physical, 80 );
			SetDamageType( ResistanceType.Cold, 20 );

			SetResistance( ResistanceType.Physical, 50, 60 );
			SetResistance( ResistanceType.Fire, 30, 50 );
			SetResistance( ResistanceType.Cold, 60, 80 );
			SetResistance( ResistanceType.Poison, 20, 30 );
			SetResistance( ResistanceType.Energy, 30, 50 );

			SetSkill( SkillName.MagicResist, 80.3, 98.4 );
			SetSkill( SkillName.Tactics, 62.1, 76.1 );
			SetSkill( SkillName.Wrestling, 61.1, 78.6 );

			Fame = 8000;
			Karma = -8000;

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
		public override Poison HitPoison { get { return Poison.Lethal; } }

		public VitaviRenowned( Serial serial )
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