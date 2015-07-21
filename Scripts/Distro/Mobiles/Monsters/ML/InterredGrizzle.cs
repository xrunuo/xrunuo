using System;
using System.Collections;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "an interred grizzle corpse" )]
	public class InterredGrizzle : BaseCreature
	{
		public override int GetDeathSound()
		{
			return 0x57F;
		}
		public override int GetAttackSound()
		{
			return 0x580;
		}
		public override int GetIdleSound()
		{
			return 0x581;
		}
		public override int GetAngerSound()
		{
			return 0x582;
		}
		public override int GetHurtSound()
		{
			return 0x583;
		}

		[Constructable]
		public InterredGrizzle()
			: base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "an interred grizzle";
			Body = 259;

			SetStr( 488, 620 );
			SetDex( 121, 170 );
			SetInt( 498, 657 );

			SetHits( 312, 553 );

			SetDamage( 18, 23 );

			SetDamageType( ResistanceType.Physical, 25 );
			SetDamageType( ResistanceType.Energy, 75 );

			SetResistance( ResistanceType.Physical, 50, 70 );
			SetResistance( ResistanceType.Fire, 50, 70 );
			SetResistance( ResistanceType.Cold, 40, 50 );
			SetResistance( ResistanceType.Poison, 50, 60 );
			SetResistance( ResistanceType.Energy, 50, 60 );

			SetSkill( SkillName.EvalInt, 90.1, 100.0 );
			SetSkill( SkillName.Magery, 99.1, 105.0 );
			SetSkill( SkillName.Meditation, 90.1, 100.0 );
			SetSkill( SkillName.MagicResist, 110.5, 120.0 );
			SetSkill( SkillName.Tactics, 100.1, 110.0 );
			SetSkill( SkillName.Wrestling, 100.1, 110.0 );

			Fame = 18000;
			Karma = -18000;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich, 2 );
			AddLoot( LootPack.MedScrolls, 2 );
		}

		public override int Meat { get { return 1; } }
		public override int TreasureMapLevel { get { return 5; } }

		protected override void OnAfterDeath(Container c)
		{
			base.OnAfterDeath (c);

			if ( 5000 > Utility.Random( 100000 ) )
			{
				switch ( Utility.RandomMinMax( 1, 2 ) )
				{
					case 1:
						c.DropItem( new MalekisHonor() ); break;
					case 2:
						c.DropItem( new AcolyteTunic() ); break;
				}
			}
		}

		public InterredGrizzle( Serial serial )
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

			/*int version = */reader.ReadInt();
		}
	}
}