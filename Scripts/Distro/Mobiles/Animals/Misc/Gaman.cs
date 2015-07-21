using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a gaman corpse" )]
	public class Gaman : BaseCreature
	{
		[Constructable]
		public Gaman()
			: base( AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			Name = "a gaman";
			Body = 248;

			SetStr( 145, 175 );
			SetDex( 115, 145 );
			SetInt( 45, 60 );

			SetHits( 130, 160 );
			SetMana( 0 );

			SetDamage( 7, 11 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 50, 70 );
			SetResistance( ResistanceType.Fire, 30, 50 );
			SetResistance( ResistanceType.Cold, 30, 50 );
			SetResistance( ResistanceType.Poison, 40, 60 );
			SetResistance( ResistanceType.Energy, 30, 50 );

			SetSkill( SkillName.MagicResist, 35.1, 45.0 );
			SetSkill( SkillName.Tactics, 70.1, 85.0 );
			SetSkill( SkillName.Wrestling, 50.1, 60.0 );

			Fame = 900;
			Karma = 0;

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 68.7;
		}

		protected override void OnAfterDeath( Container c )
		{
			if ( Utility.RandomBool() )
				c.DropItem( new Engines.MLQuests.GamanHorns( Utility.RandomMinMax( 1, 2 ) ) );

			base.OnAfterDeath( c );
		}

		public override int GetAngerSound()
		{
			return 0x4FA;
		}

		public override int GetIdleSound()
		{
			return 0x4F9;
		}

		public override int GetAttackSound()
		{
			return 0x4F7;
		}

		public override int GetHurtSound()
		{
			return 0x4FB;
		}

		public override int GetDeathSound()
		{
			return 0x4F6;
		}

		public override int Hides { get { return 15; } }
		public override HideType HideType { get { return HideType.Regular; } }
		public override int Meat { get { return 10; } }
		public override FoodType FavoriteFood { get { return FoodType.GrainsAndHay; } }

		public Gaman( Serial serial )
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