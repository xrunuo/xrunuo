using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a corporeal brume corpse" )]
	public class CorporealBrume : BaseCreature
	{
		[Constructable]
		public CorporealBrume()
			: base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a corporeal brume";
			Body = 0x111;
			BaseSoundID = 0x56B;

			SetStr( 439, 442 );
			SetDex( 110, 140 );
			SetInt( 51, 58 );

			SetHits( 1185, 1235 );

			SetDamage( 14, 17 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 100 );
			SetResistance( ResistanceType.Fire, 43, 47 );
			SetResistance( ResistanceType.Cold, 43, 46 );
			SetResistance( ResistanceType.Poison, 50, 58 );
			SetResistance( ResistanceType.Energy, 35, 39 );

			SetSkill( SkillName.Wrestling, 110.3, 112.0 );
			SetSkill( SkillName.Tactics, 113.3, 114.3 );
			SetSkill( SkillName.MagicResist, 83.7, 93.3 );
			SetSkill( SkillName.Anatomy, 102.4, 108.3 );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.AosFilthyRich, 3 );
		}

		public void AreaDamageEffect( Mobile m )
		{
			m.FixedParticles( 0x374A, 10, 15, 5038, 1181, 2, EffectLayer.Head );
			m.PlaySound( 0x213 );
		}

		//TODO: Estos estaban como override todos, quizas falten cosas en el BaseCreature.cs ¿?¿?
		public bool CanAreaDamage { get { return true; } }
		public TimeSpan AreaDamageDelay { get { return TimeSpan.FromSeconds( 20 ); } }
		public double AreaDamageScalar { get { return 0.5; } }
		public int AreaFireDamage { get { return 0; } }
		public int AreaColdDamage { get { return 100; } }
		public override bool Unprovokable { get { return true; } }

		public CorporealBrume( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}
