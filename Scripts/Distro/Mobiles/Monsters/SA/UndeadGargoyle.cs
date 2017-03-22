using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "an undead gargoyle corpse" )]
	public class UndeadGargoyle : BaseCreature
	{
		[Constructable]
		public UndeadGargoyle()
			: base( AIType.AI_Necromancer, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "an undead gargoyle";
			Body = 722;

			SetStr( 250, 350 );
			SetDex( 120, 140 );
			SetInt( 250, 350 );

			SetHits( 200, 300 );

			SetDamage( 15, 27 );

			SetDamageType( ResistanceType.Cold, 60 );
			SetDamageType( ResistanceType.Energy, 40 );

			SetResistance( ResistanceType.Physical, 45, 55 );
			SetResistance( ResistanceType.Fire, 30, 40 );
			SetResistance( ResistanceType.Cold, 45, 55 );
			SetResistance( ResistanceType.Poison, 55, 65 );
			SetResistance( ResistanceType.Energy, 40, 50 );

			SetSkill( SkillName.MagicResist, 100.1, 120.0 );
			SetSkill( SkillName.Tactics, 60.1, 70.0 );
			SetSkill( SkillName.Wrestling, 60.1, 70.0 );
			SetSkill( SkillName.Magery, 120.0 );
			SetSkill( SkillName.EvalInt, 90.1, 110.0 );
			SetSkill( SkillName.Necromancy, 90.1, 110.0 );
			SetSkill( SkillName.SpiritSpeak, 90.1, 110.0 );

			Fame = 18000;
			Karma = -18000;

			PackMysticScroll( Utility.Random( 10, 6 ) );
			PackMysticScroll( Utility.Random( 8, 6 ) );
			PackMysticScroll( Utility.Random( 8, 6 ) );

			PackItem( new IronIngot( 30 ) );
			PackItem( new Bone() );
			PackItem( new BonePile() );
		}

		public override bool Unprovokable { get { return true; } }
		public override bool Uncalmable { get { return true; } }

		public override bool CanRummageCorpses { get { return true; } }
		public override bool BleedImmune { get { return true; } }
		public override Poison PoisonImmune { get { return Poison.Lethal; } }
		public override int TreasureMapLevel { get { return 3; } }
		public override SlayerName SlayerGroup { get { return SlayerName.Undead; } }

		public override int GetAttackSound() { return 0x646; }
		public override int GetDeathSound() { return 0x647; }
		public override int GetHurtSound() { return 0x648; }
		public override int GetIdleSound() { return 0x649; }

		public override void GenerateLoot()
		{
			AddLoot( LootPack.UltraRich );
		}

		protected override void OnAfterDeath( Container c )
		{
			base.OnAfterDeath( c );

			if ( 0.4 > Utility.RandomDouble() )
				c.DropItem( new UndamagedUndeadGargoyleHorns() );

			if ( 0.1 > Utility.RandomDouble() )
				c.DropItem( new UndeadGargoyleMedallion() );
		}

		public UndeadGargoyle( Serial serial )
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
