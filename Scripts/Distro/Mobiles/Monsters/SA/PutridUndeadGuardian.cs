using System;
using Server;
using Server.Items;
using Server.Engines.Loyalty;

namespace Server.Mobiles
{
	[CorpseName( "a Putrid Undead Guardian corpse" )]
	public class PutridUndeadGuardian : BaseCreature
	{
		[Constructable]
		public PutridUndeadGuardian()
			: base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Putrid Undead Guardian";
			Body = 722;

			SetStr( 76, 98 );
			SetDex( 56, 76 );
			SetInt( 187, 209 );

			SetHits( 47, 60 );

			SetDamage( 3, 7 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 35, 40 );
			SetResistance( ResistanceType.Fire, 20, 30 );
			SetResistance( ResistanceType.Cold, 50, 60 );
			SetResistance( ResistanceType.Poison, 20, 30 );
			SetResistance( ResistanceType.Energy, 30, 40 );

			SetSkill( SkillName.MagicResist, 60.1, 70.0 );
			SetSkill( SkillName.Tactics, 45.1, 60.0 );
			SetSkill( SkillName.Wrestling, 45.1, 55.0 );

			Fame = 1000; // TODO: Verify
			Karma = -1000;

			PackNecroReg( 50, 150 );

			PackItem( new UndyingFlesh() );
		}

		public PutridUndeadGuardian( Serial serial )
			: base( serial )
		{
		}

		public override int GetAngerSound() { return 0x175; }
		public override int GetIdleSound() { return 0x19D; }
		public override int GetAttackSound() { return 0xE2; }
		public override int GetHurtSound() { return 0x28B; }
		public override int GetDeathSound() { return 0x108; }

		public override LoyaltyGroup LoyaltyGroupEnemy { get { return LoyaltyGroup.GargoyleQueen; } }
		public override int LoyaltyPointsAward { get { return 15; } }
		public override SlayerName SlayerGroup { get { return SlayerName.Undead; } }

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
