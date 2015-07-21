using System;
using Server;
using Server.Items;
using Server.Engines.Loyalty;

namespace Server.Mobiles
{
	[CorpseName( "a fire elemental corpse" )]
	public class FireElementalRenowned : BaseCreature, IRenowned
	{
		public override bool AlwaysMurderer { get { return true; } }

		private static readonly Type[] m_Rewards = new Type[]
			{
				typeof( JadeWarAxe ),
				typeof( TokenOfHolyFavor ),
				typeof( SwordOfShatteredHopes )
			};

		public Type[] Rewards { get { return m_Rewards; } }

		[Constructable]
		public FireElementalRenowned()
			: base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "fire elemental [Renowned]";
			Body = 15;
			BaseSoundID = 838;

			Hue = 1161;

			SetStr( 443, 490 );
			SetDex( 172, 203 );
			SetInt( 318, 334 );

			SetHits( 1360, 1408 );

			SetDamage( 7, 9 );

			SetDamageType( ResistanceType.Physical, 25 );
			SetDamageType( ResistanceType.Fire, 75 );

			SetResistance( ResistanceType.Physical, 50, 55 );
			SetResistance( ResistanceType.Fire, 80, 85 );
			SetResistance( ResistanceType.Cold, 5, 10 );
			SetResistance( ResistanceType.Poison, 45, 50 );
			SetResistance( ResistanceType.Energy, 45, 50 );

			SetSkill( SkillName.MagicResist, 105.0, 125.0 );
			SetSkill( SkillName.Tactics, 100.0, 105.0 );
			SetSkill( SkillName.Wrestling, 95.0, 100.0 );
			SetSkill( SkillName.EvalInt, 105.0, 125.0 );
			SetSkill( SkillName.Magery, 105.0, 110.0 );

			Fame = 18000;
			Karma = -18000;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.UltraRich );
			AddLoot( LootPack.RareGems, 1 );
		}

		public override LoyaltyGroup LoyaltyGroupEnemy { get { return LoyaltyGroup.GargoyleQueen; } }
		public override int LoyaltyPointsAward { get { return 50; } }

		public override bool HasBreath { get { return true; } }

		public FireElementalRenowned( Serial serial )
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
