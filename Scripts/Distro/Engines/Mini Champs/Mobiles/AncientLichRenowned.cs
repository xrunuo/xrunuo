using System;
using Server;
using Server.Items;
using Server.Engines.Loyalty;

namespace Server.Mobiles
{
	[CorpseName( "an ancient lich corpse" )]
	public class AncientLichRenowned : BaseCreature, IRenowned
	{
		private static readonly Type[] m_Rewards = new Type[]
			{
				typeof( DefenderOfTheMagus ),
				typeof( SpinedBloodwormBracers ),
				typeof( SummonersKilt )
			};

		public Type[] Rewards { get { return m_Rewards; } }

		[Constructable]
		public AncientLichRenowned()
			: base( AIType.AI_Necromancer, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "an ancient lich [Renowned]";
			Body = 78;
			BaseSoundID = 412;

			SetStr( 298, 299 );
			SetDex( 108, 113 );
			SetInt( 994, 1010 );

			SetHits( 2176, 2242 );

			SetDamage( 15, 27 );

			SetDamageType( ResistanceType.Physical, 20 );
			SetDamageType( ResistanceType.Cold, 40 );
			SetDamageType( ResistanceType.Energy, 40 );

			SetResistance( ResistanceType.Physical, 60, 65 );
			SetResistance( ResistanceType.Fire, 25, 30 );
			SetResistance( ResistanceType.Cold, 50, 60 );
			SetResistance( ResistanceType.Poison, 55, 60 );
			SetResistance( ResistanceType.Energy, 25, 30 );

			SetSkill( SkillName.MagicResist, 180.0, 190.0 );
			SetSkill( SkillName.Tactics, 90.0, 95.0 );
			SetSkill( SkillName.Wrestling, 95.0, 100.0 );
			SetSkill( SkillName.EvalInt, 120.0, 130.0 );
			SetSkill( SkillName.Magery, 120.0, 130.0 );
			SetSkill( SkillName.Necromancy, 120.0, 130.0 );
			SetSkill( SkillName.SpiritSpeak, 120.0, 130.0 );
			SetSkill( SkillName.Meditation, 100.0, 110.0 );

			Fame = 24000;
			Karma = -24000;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.UltraRich );
			AddLoot( LootPack.FilthyRich );
			AddLoot( LootPack.RareGems );
		}

		public override SlayerName SlayerGroup { get { return SlayerName.Undead; } }
		public override LoyaltyGroup LoyaltyGroupEnemy { get { return LoyaltyGroup.GargoyleQueen; } }
		public override int LoyaltyPointsAward { get { return 50; } }

		public AncientLichRenowned( Serial serial )
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
