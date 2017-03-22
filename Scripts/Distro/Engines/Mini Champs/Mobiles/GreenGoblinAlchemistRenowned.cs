using System;
using Server;
using Server.Items;
using Server.Engines.Loyalty;

namespace Server.Mobiles
{
	[CorpseName( "a green goblin alchemist corpse" )]
	public class GreenGoblinAlchemistRenowned : GreenGoblinAlchemist, IRenowned
	{
		private static readonly Type[] m_Rewards = new Type[]
			{
				typeof( ObsidianEarrings ),
				typeof( TheImpalersPick )
			};

		public Type[] Rewards { get { return m_Rewards; } }

		[Constructable]
		public GreenGoblinAlchemistRenowned()
		{
			Name = "green goblin alchemist [Renowned]";
			Body = 723;

			SetStr( 585 );
			SetDex( 64 );
			SetInt( 117 );

			SetHits( 1160, 1246 );

			SetDamage( 5, 7 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 50, 60 );
			SetResistance( ResistanceType.Fire, 55, 65 );
			SetResistance( ResistanceType.Cold, 40, 50 );
			SetResistance( ResistanceType.Poison, 45, 55 );
			SetResistance( ResistanceType.Energy, 20, 30 );

			SetSkill( SkillName.MagicResist, 124.3, 129.6 );
			SetSkill( SkillName.Tactics, 97.9, 99.9 );
			SetSkill( SkillName.Wrestling, 105.7, 109.6 );

			Fame = 25000;
			Karma = -25000;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.UltraRich );
			AddLoot( LootPack.Gems );
			AddLoot( LootPack.RareGems );
		}

		public override LoyaltyGroup LoyaltyGroupEnemy { get { return LoyaltyGroup.GargoyleQueen; } }
		public override int LoyaltyPointsAward { get { return 50; } }
		public override SlayerName SlayerGroup { get { return SlayerName.Repond; } }

		public GreenGoblinAlchemistRenowned( Serial serial )
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
