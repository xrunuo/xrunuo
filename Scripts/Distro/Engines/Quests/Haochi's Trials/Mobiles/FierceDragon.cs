using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests.SE
{
	[CorpseName( "a fierce dragon corpse" )]
	public class FierceDragon : BaseCreature
	{
		[Constructable]
		public FierceDragon()
			: base( AIType.AI_Mage, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			Name = "a fierce dragon";
			Body = 103;
			BaseSoundID = 362;

			SetStr( 6012 );
			SetDex( 0 );
			SetInt( 862 );

			SetDamage( 1000 );

			SetDamageType( ResistanceType.Fire, 100 );

			SetResistance( ResistanceType.Physical, 98 );
			SetResistance( ResistanceType.Fire, 95 );
			SetResistance( ResistanceType.Cold, 98 );
			SetResistance( ResistanceType.Poison, 98 );
			SetResistance( ResistanceType.Energy, 98 );

			Frozen = true;

			SetSkill( SkillName.Magery, 120.0 );
			SetSkill( SkillName.Meditation, 25.0 );
			SetSkill( SkillName.Tactics, 120.0 );
			SetSkill( SkillName.Wrestling, 120.0 );

			Fame = 15000;
			Karma = 15000;

			CantWalk = true;
		}

		public override int GetIdleSound()
		{
			return 0x2C4;
		}

		public override int GetAttackSound()
		{
			return 0x2C0;
		}

		public override int GetDeathSound()
		{
			return 0x2C1;
		}

		public override int GetAngerSound()
		{
			return 0x2C4;
		}

		public override int GetHurtSound()
		{
			return 0x2C3;
		}

		public override bool HasBreath { get { return true; } } // fire breath enabled
		public override bool AutoDispel { get { return true; } }
		public override HideType HideType { get { return HideType.Barbed; } }
		public override int Hides { get { return 20; } }
		public override int Meat { get { return 19; } }
		public override int Scales { get { return 6; } }
		public override ScaleType ScaleType { get { return ( Utility.RandomBool() ? ScaleType.Black : ScaleType.White ); } }
		public override int TreasureMapLevel { get { return 4; } }
		public override bool PlayerRangeSensitive { get { return false; } }

		public FierceDragon( Serial serial )
			: base( serial )
		{
		}

		public override void OnGotMeleeAttack( Mobile attacker )
		{
			attacker.Kill();
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
