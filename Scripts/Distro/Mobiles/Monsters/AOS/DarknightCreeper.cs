using System;
using Server;
using Server.Items;
using Server.Engines.Doom;

namespace Server.Mobiles
{
	[CorpseName( "a darknight creeper corpse" )]
	public class DarknightCreeper : BaseCreature
	{
		[Constructable]
		public DarknightCreeper()
			: base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = NameList.RandomName( "darknight creeper" );
			Body = 313;
			BaseSoundID = 0xE0;

			SetStr( 301, 330 );
			SetDex( 101, 110 );
			SetInt( 301, 330 );

			SetHits( 4000 );

			SetDamage( 22, 26 );

			SetDamageType( ResistanceType.Physical, 85 );
			SetDamageType( ResistanceType.Poison, 15 );

			SetResistance( ResistanceType.Physical, 60 );
			SetResistance( ResistanceType.Fire, 60 );
			SetResistance( ResistanceType.Cold, 100 );
			SetResistance( ResistanceType.Poison, 90 );
			SetResistance( ResistanceType.Energy, 75 );

			SetSkill( SkillName.EvalInt, 118.1, 120.0 );
			SetSkill( SkillName.Magery, 112.6, 120.0 );
			SetSkill( SkillName.Meditation, 150.0 );
			SetSkill( SkillName.Poisoning, 120.0 );
			SetSkill( SkillName.MagicResist, 90.1, 90.9 );
			SetSkill( SkillName.Tactics, 100.0 );
			SetSkill( SkillName.Wrestling, 90.1, 90.9 );

			Fame = 22000;
			Karma = -22000;
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

		public override bool BardImmune { get { return false; } }
		public override bool Unprovokable { get { return true; } }
		public override bool Uncalmable { get { return true; } }
		public override bool BleedImmune { get { return true; } }
		public override Poison PoisonImmune { get { return Poison.Lethal; } }
		public override Poison HitPoison { get { return Poison.Lethal; } }

		public override int TreasureMapLevel { get { return 1; } }

		public DarknightCreeper( Serial serial )
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
