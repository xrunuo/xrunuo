using System;
using Server;
using Server.Items;
using Server.Engines.Loyalty;

namespace Server.Mobiles
{
	[CorpseName( "a slith corpse" )]
	public class Slith : BaseCreature
	{
		[Constructable]
		public Slith()
			: base( AIType.AI_Animal, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a slith";
			Body = 734;

			SetStr( 125, 150 );
			SetDex( 55, 75 );
			SetInt( 10, 20 );

			SetHits( 75, 95 );
			SetMana( 0 );

			SetDamage( 6, 24 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 35, 45 );
			SetResistance( ResistanceType.Fire, 30, 45 );
			SetResistance( ResistanceType.Poison, 25, 35 );
			SetResistance( ResistanceType.Energy, 25, 35 );

			SetSkill( SkillName.MagicResist, 55.1, 70.0 );
			SetSkill( SkillName.Tactics, 60.1, 80.0 );
			SetSkill( SkillName.Wrestling, 60.1, 80.0 );

			Fame = 1500;
			Karma = -1500;

			Tamable = true;
			MinTameSkill = 80.7;
			ControlSlots = 1;
		}

		public override int GetAngerSound() { return 0x639; }
		public override int GetIdleSound() { return 0x289; }
		public override int GetAttackSound() { return 0x636; }
		public override int GetHurtSound() { return 0x638; }
		public override int GetDeathSound() { return 0x637; }

		public override LoyaltyGroup LoyaltyGroupEnemy { get { return LoyaltyGroup.GargoyleQueen; } }
		public override int LoyaltyPointsAward { get { return 15; } }

		public override int Meat { get { return 6; } }
		public override int Hides { get { return 10; } }
		public override int Blood { get { return 6; } }
		public override int TreasureMapLevel { get { return 2; } }		

		public override bool HasBreath { get { return true; } } // fire breath enabled

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Average, 2 );
		}

		protected override void OnAfterDeath( Container c )
		{
			base.OnAfterDeath( c );

			if ( 0.5 > Utility.RandomDouble() )
				c.DropItem( new SlithTongue() );
		}

		public Slith( Serial serial )
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
