using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "an effete undead gargoyle corpse" )]
	public class EffeteUndeadGargoyle : BaseCreature
	{
		[Constructable]
		public EffeteUndeadGargoyle()
			: base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "an effete undead gargoyle";
			Body = 722;

			SetStr( 62, 65 );
			SetDex( 56, 64 );
			SetInt( 17, 37 );

			SetHits( 67, 77 );

			SetDamage( 3, 7 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 15, 20 );
			SetResistance( ResistanceType.Fire, 5, 10 );
			SetResistance( ResistanceType.Cold, 25, 35 );
			SetResistance( ResistanceType.Poison, 25, 35 );
			SetResistance( ResistanceType.Energy, 5, 15 );

			SetSkill( SkillName.MagicResist, 45.1, 55.0 );
			SetSkill( SkillName.Tactics, 45.1, 55.0 );
			SetSkill( SkillName.Wrestling, 45.1, 55.0 );

			Fame = 500;
			Karma = -500;
		}

		public override int GetAngerSound() { return 0x175; }
		public override int GetIdleSound() { return 0x19D; }
		public override int GetAttackSound() { return 0xE2; }
		public override int GetHurtSound() { return 0x28B; }
		public override int GetDeathSound() { return 0x108; }

		public override SlayerName SlayerGroup { get { return SlayerName.Undead; } }

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Meager );
		}

		protected override void OnAfterDeath( Container c )
		{
			base.OnAfterDeath( c );

			if ( 0.1 > Utility.RandomDouble() )
				c.DropItem( new UndyingFlesh() );
		}

		public EffeteUndeadGargoyle( Serial serial )
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
