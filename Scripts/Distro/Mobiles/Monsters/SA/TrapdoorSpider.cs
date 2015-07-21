using System;
using Server;
using Server.Items;
using Server.Engines.Loyalty;

namespace Server.Mobiles
{
	[CorpseName( "a trapdoor spider corpse" )]
	public class TrapdoorSpider : BaseCreature
	{
		[Constructable]
		public TrapdoorSpider()
			: base( AIType.AI_Ambusher, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a trapdoor spider";
			Body = 737;

			SetStr( 103, 122 );
			SetDex( 149, 165 );
			SetInt( 27, 47 );

			SetHits( 127, 147 );

			SetDamage( 15, 18 );

			SetDamageType( ResistanceType.Physical, 20 );
			SetDamageType( ResistanceType.Poison, 80 );

			SetResistance( ResistanceType.Physical, 25, 40 );
			SetResistance( ResistanceType.Fire, 20, 35 );
			SetResistance( ResistanceType.Cold, 35, 45 );
			SetResistance( ResistanceType.Poison, 35, 45 );
			SetResistance( ResistanceType.Energy, 95, 100 );

			SetSkill( SkillName.Healing, 81.1, 87 );
			SetSkill( SkillName.MagicResist, 47.6, 58.1 );
			SetSkill( SkillName.Tactics, 72.5, 84.2 );
			SetSkill( SkillName.Wrestling, 87, 92.8 );
			SetSkill( SkillName.Hiding, 88.6, 98.6 );
			SetSkill( SkillName.Stealth, 89.2, 99.2 );

			Fame = 2000;
			Karma = -2000;

			PackItem( new SpidersSilk( 8 ) );
		}

		public override int GetAttackSound() { return 0x642; }
		public override int GetDeathSound() { return 0x643; }
		public override int GetHurtSound() { return 0x644; }
		public override int GetIdleSound() { return 0x645; }

		public override LoyaltyGroup LoyaltyGroupEnemy { get { return LoyaltyGroup.GargoyleQueen; } }
		public override int LoyaltyPointsAward { get { return 5; } }

		public override Poison HitPoison { get { return Poison.Lethal; } }
		public override Poison PoisonImmune { get { return Poison.Lethal; } }
		public override int TreasureMapLevel { get { return 2; } }		

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich, 2 );
		}

		protected override void OnAfterDeath( Container c )
		{
			base.OnAfterDeath( c );

			if ( 0.15 > Utility.RandomDouble() )
				c.DropItem( new SpiderCarapace() );

			if ( 0.15 > Utility.RandomDouble() )
				c.DropItem( new BottleOfIchor() );
		}

		public TrapdoorSpider( Serial serial )
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
