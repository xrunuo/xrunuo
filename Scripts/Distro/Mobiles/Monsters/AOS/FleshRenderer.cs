using System;
using Server;
using Server.Items;
using Server.Engines.Doom;

namespace Server.Mobiles
{
	[CorpseName( "a fleshrenderer corpse" )]
	public class FleshRenderer : BaseCreature
	{
		public override WeaponAbility GetWeaponAbility()
		{
			return Utility.RandomBool() ? WeaponAbility.Dismount : WeaponAbility.ParalyzingBlow;
		}

		[Constructable]
		public FleshRenderer()
			: base( AIType.AI_BossMelee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a fleshrenderer";
			Body = 315;

			SetStr( 401, 460 );
			SetDex( 201, 210 );
			SetInt( 221, 260 );

			SetHits( 4500 );

			SetDamage( 16, 20 );

			SetDamageType( ResistanceType.Physical, 80 );
			SetDamageType( ResistanceType.Poison, 20 );

			SetResistance( ResistanceType.Physical, 80, 90 );
			SetResistance( ResistanceType.Fire, 50, 60 );
			SetResistance( ResistanceType.Cold, 50, 60 );
			SetResistance( ResistanceType.Poison, 100 );
			SetResistance( ResistanceType.Energy, 70, 80 );

			SetSkill( SkillName.MagicResist, 155.1, 160.0 );
			SetSkill( SkillName.Meditation, 100.0 );
			SetSkill( SkillName.Tactics, 100.0 );
			SetSkill( SkillName.Wrestling, 90.1, 100.0 );

			Fame = 23000;
			Karma = -23000;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.UltraRich, 2 );
		}

		protected override void OnAfterDeath( Container c )
		{
			base.OnAfterDeath( c );

			if ( !Summoned && !NoKillAwards )
				DoomArtifactGiver.CheckArtifactGiving( this );
		}

		public override bool AutoDispel { get { return true; } }
		public override bool Unprovokable { get { return true; } }
		public override bool Uncalmable { get { return true; } }
		public override Poison PoisonImmune { get { return Poison.Lethal; } }

		public override int TreasureMapLevel { get { return 1; } }

		public override int GetAttackSound() { return 0x34C; }
		public override int GetHurtSound() { return 0x354; }
		public override int GetAngerSound() { return 0x34C; }
		public override int GetIdleSound() { return 0x34C; }
		public override int GetDeathSound() { return 0x354; }

		public FleshRenderer( Serial serial )
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
