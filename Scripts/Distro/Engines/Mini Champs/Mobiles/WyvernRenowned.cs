using System;
using Server;
using Server.Items;
using Server.Engines.Loyalty;

namespace Server.Mobiles
{
	[CorpseName( "a wyvern corpse" )]
	public class WyvernRenowned : BaseCreature, IRenowned
	{
		private static readonly Type[] m_Rewards = new Type[]
			{
				typeof( AnimatedLegsOfTheInsaneTinker ),
				typeof( PillarOfStrength )
			};

		public Type[] Rewards { get { return m_Rewards; } }

		[Constructable]
		public WyvernRenowned()
			: base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a wyvern [Renowned]";
			Body = 62;
			BaseSoundID = 362;

			Hue = 1616;

			SetStr( 1316, 1422 );
			SetDex( 98, 170 );
			SetInt( 835, 1002 );

			SetHits( 2412, 2820 );

			SetDamage( 29, 35 );

			SetDamageType( ResistanceType.Physical, 75 );
			SetDamageType( ResistanceType.Fire, 25 );

			SetResistance( ResistanceType.Physical, 65, 75 );
			SetResistance( ResistanceType.Fire, 80, 85 );
			SetResistance( ResistanceType.Cold, 75, 80 );
			SetResistance( ResistanceType.Poison, 60, 70 );
			SetResistance( ResistanceType.Energy, 60, 70 );

			SetSkill( SkillName.MagicResist, 105.8, 130.1 );
			SetSkill( SkillName.Tactics, 130.9, 139.1 );
			SetSkill( SkillName.Wrestling, 109.7, 113.2 );
			SetSkill( SkillName.EvalInt, 111.2, 115.4 );
			SetSkill( SkillName.Magery, 109.4, 112.3 );
			SetSkill( SkillName.Meditation, 65.4, 69.3 );

			Fame = 24000;
			Karma = -24000;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.UltraRich, 2 );
			AddLoot( LootPack.RareGems );
		}

		public override bool HasBreath { get { return true; } }

		public override bool ReacquireOnMovement { get { return true; } }

		public override Poison PoisonImmune { get { return Poison.Deadly; } }
		public override Poison HitPoison { get { return Poison.Deadly; } }
		public override int TreasureMapLevel { get { return 2; } }

		public override int Meat { get { return 10; } }
		public override int Hides { get { return 20; } }
		public override HideType HideType { get { return HideType.Horned; } }

		public override LoyaltyGroup LoyaltyGroupEnemy { get { return LoyaltyGroup.GargoyleQueen; } }
		public override int LoyaltyPointsAward { get { return 50; } }

		public override int GetAttackSound() { return 713; }
		public override int GetAngerSound() { return 718; }
		public override int GetDeathSound() { return 716; }
		public override int GetHurtSound() { return 721; }
		public override int GetIdleSound() { return 725; }

		public WyvernRenowned( Serial serial )
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