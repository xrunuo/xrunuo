using System;
using Server.Items;
using Server.Engines.MLQuests;

namespace Server.Mobiles
{
	[CorpseName( "a troglodyte corpse" )]
	public class Troglodyte : BaseCreature
	{
		public override int GetDeathSound() { return 0x59D; }
		public override int GetAttackSound() { return 0x59E; }
		public override int GetIdleSound() { return 0x59F; }
		public override int GetAngerSound() { return 0x5A0; }
		public override int GetHurtSound() { return 0x5A1; }

		public override WeaponAbility GetWeaponAbility()
		{
			return WeaponAbility.CrushingBlow;
		}

		[Constructable]
		public Troglodyte()
			: base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a troglodyte";
			Body = 267;

			SetStr( 160, 210 );
			SetDex( 90, 120 );
			SetInt( 50, 70 );

			SetHits( 300, 340 );

			SetDamage( 11, 14 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 30, 35 );
			SetResistance( ResistanceType.Fire, 20, 30 );
			SetResistance( ResistanceType.Cold, 35, 40 );
			SetResistance( ResistanceType.Poison, 30, 40 );
			SetResistance( ResistanceType.Energy, 30, 40 );

			SetSkill( SkillName.Anatomy, 70.0, 95.0 );
			SetSkill( SkillName.MagicResist, 50.0, 65.0 );
			SetSkill( SkillName.Tactics, 80.0, 95.0 );
			SetSkill( SkillName.Wrestling, 70.0, 95.0 );

			Fame = 500;
			Karma = -500;

			PackItem( new Bandage( Utility.Random( 10 ) + 10 ) );
			PackGold( 415, 490 );
		}

		// TODO: Self heal with bandages

		public override int TreasureMapLevel { get { return 2; } }
		public override SlayerName SlayerGroup { get { return SlayerName.Repond; } }

		protected override void OnAfterDeath( Container c )
		{
			base.OnAfterDeath( c );

			if ( 0.2 > Utility.RandomDouble() )
				c.DropItem( new PrimitiveFetish() );
		}

		public Troglodyte( Serial serial )
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
