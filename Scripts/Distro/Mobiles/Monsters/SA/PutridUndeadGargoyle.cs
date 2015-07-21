using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a putrid undead gargoyle corpse" )]
	public class PutridUndeadGargoyle : BaseCreature
	{
		[Constructable]
		public PutridUndeadGargoyle()
			: base( AIType.AI_Mystic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a putrid undead gargoyle";
			Body = 722;
			Hue = 1157;

			SetStr( 514, 536 );
			SetDex( 117, 135 );
			SetInt( 1131, 1154 );

			SetHits( 609, 641 );

			SetDamage( 21, 30 );

			SetDamageType( ResistanceType.Physical, 20 );
			SetDamageType( ResistanceType.Cold, 40 );
			SetDamageType( ResistanceType.Energy, 40 );

			SetResistance( ResistanceType.Physical, 55, 65 );
			SetResistance( ResistanceType.Fire, 30, 40 );
			SetResistance( ResistanceType.Cold, 50, 60 );
			SetResistance( ResistanceType.Poison, 50, 60 );
			SetResistance( ResistanceType.Energy, 40, 50 );

			SetSkill( SkillName.MagicResist, 170.0, 200.0 );
			SetSkill( SkillName.Tactics, 100.0, 120.0 );
			SetSkill( SkillName.Wrestling, 100.0, 120.0 );
			SetSkill( SkillName.Magery, 120.0, 135.0 );
			SetSkill( SkillName.EvalInt, 120.0, 135.0 );
			SetSkill( SkillName.Mysticism, 120.0, 135.0 );
			SetSkill( SkillName.Meditation, 100.0, 120.0 );
			SetSkill( SkillName.Focus, 100.0, 120.0 );

			Fame = 23000;
			Karma = -23000;

			for ( int i = 0; i < 4; i++ )
				PackMysticScroll( Utility.Random( 8, 6 ) ); // 5th - 7th circle
		}

		public override int GetAngerSound() { return 0x175; }
		public override int GetIdleSound() { return 0x19D; }
		public override int GetAttackSound() { return 0xE2; }
		public override int GetHurtSound() { return 0x28B; }
		public override int GetDeathSound() { return 0x108; }

		public override void GenerateLoot()
		{
			AddLoot( LootPack.UltraRich, 2 );
		}

		protected override void OnAfterDeath( Container c )
		{
			base.OnAfterDeath( c );

			c.DropItem( new UndyingFlesh() );

			if ( 0.3 > Utility.RandomDouble() )
				c.DropItem( new InfusedGlassStave() );
		}

		public PutridUndeadGargoyle( Serial serial )
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
