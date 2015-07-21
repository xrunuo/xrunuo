using System;
using Server;
using Server.Items;
using Server.Engines.Loyalty;

namespace Server.Mobiles
{
	[CorpseName( "a toxic slith corpse" )]
	public class ToxicSlith : BaseCreature
	{
		[Constructable]
		public ToxicSlith()
			: base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a toxic slith";
			Body = 734;
			Hue = 476;

			SetStr( 207, 356 );
			SetDex( 215, 284 );
			SetInt( 22, 38 );

			SetHits( 180, 210 );
			SetMana( 0, 5 );

			SetDamage( 6, 24 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 35, 45 );
			SetResistance( ResistanceType.Fire, 5, 10 );
			SetResistance( ResistanceType.Cold, 5, 10 );
			SetResistance( ResistanceType.Poison, 100 );
			SetResistance( ResistanceType.Energy, 5, 10 );

			SetSkill( SkillName.Poisoning, 90.0, 110.0 );
			SetSkill( SkillName.MagicResist, 95.0, 100.0 );
			SetSkill( SkillName.Tactics, 80.0, 95.0 );
			SetSkill( SkillName.Wrestling, 85.0, 100.0 );

			Fame = 6000;
			Karma = -6000;
		}

		public override int GetAngerSound() { return 0x639; }
		public override int GetIdleSound() { return 0x289; }
		public override int GetAttackSound() { return 0x636; }
		public override int GetHurtSound() { return 0x638; }
		public override int GetDeathSound() { return 0x637; }

		public override LoyaltyGroup LoyaltyGroupEnemy { get { return LoyaltyGroup.GargoyleQueen; } }
		public override int LoyaltyPointsAward { get { return 30; } }

		public override Poison HitPoison { get { return Poison.Lethal; } }
		public override Poison PoisonImmune { get { return Poison.Lethal; } }
		public override HideType HideType { get { return HideType.Horned; } }
		public override int Hides { get { return 10; } }
		public override int Blood { get { return 6; } }
		public override int Meat { get { return 6; } }

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich, 2 );
		}

		protected override void OnAfterDeath( Container c )
		{
			base.OnAfterDeath( c );

			if ( 0.5 > Utility.RandomDouble() )
				c.DropItem( new SlithTongue() );

			if ( 0.5 > Utility.RandomDouble() )
				c.DropItem( new ToxicVenomSac() );
		}

		public ToxicSlith( Serial serial )
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
