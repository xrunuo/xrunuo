using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Engines.Loyalty;

namespace Server.Mobiles
{
	[CorpseName( "a fire ant corpse" )]
	public class FireAnt : BaseCreature
	{
		[Constructable]
		public FireAnt()
			: base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.175, 0.375 )
		{
			Name = "a fire ant";
			Body = 738;

			SetStr( 205, 249 );
			SetDex( 104, 130 );
			SetInt( 17, 30 );

			SetHits( 260, 294 );

			SetDamage( 15, 18 );

			SetDamageType( ResistanceType.Physical, 40 );
			SetDamageType( ResistanceType.Fire, 60 );

			SetResistance( ResistanceType.Physical, 45, 55 );
			SetResistance( ResistanceType.Fire, 95, 100 );
			SetResistance( ResistanceType.Cold, 35, 45 );
			SetResistance( ResistanceType.Poison, 35, 45 );
			SetResistance( ResistanceType.Energy, 35, 45 );

			SetSkill( SkillName.MagicResist, 45.4, 59.5 );
			SetSkill( SkillName.Tactics, 70.7, 81.4 );
			SetSkill( SkillName.Wrestling, 74, 82.3 );

			Fame = 5000;
			Karma = -5000;
		}

		public override int TreasureMapLevel { get { return 3; } }

		public override int GetDeathSound()
		{
			return 0x1BA;
		}

		public override int GetIdleSound()
		{
			return 0x5A;
		}

		public override int GetAttackSound()
		{
			return 0x164;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich );
		}

		protected override void OnAfterDeath( Container c )
		{
			base.OnAfterDeath( c );

			if ( 0.2 > Utility.RandomDouble() )
				c.DropItem( new SearedFireAntGoo() );
		}

		public override SpecialAbility GetSpecialAbility()
		{
			return SpecialAbility.FlammableGoo;
		}

		public override LoyaltyGroup LoyaltyGroupEnemy { get { return LoyaltyGroup.GargoyleQueen; } }
		public override int LoyaltyPointsAward { get { return 2; } }

		public FireAnt( Serial serial )
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
