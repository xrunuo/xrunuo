using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a greater poison elemental corpse" )]
	public class GreaterPoisonElemental : BaseCreature
	{
		[Constructable]
		public GreaterPoisonElemental()
			: base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "greater poison elemental";
			Body = 162;
			BaseSoundID = 263;

			Hue = 667;

			SetStr( 656, 699 );
			SetDex( 184, 198 );
			SetInt( 563, 578 );

			SetHits( 662, 700 );
			SetStam( 307, 350 );

			SetDamage( 12, 18 );

			SetDamageType( ResistanceType.Physical, 10 );
			SetDamageType( ResistanceType.Poison, 90 );

			SetResistance( ResistanceType.Physical, 60, 70 );
			SetResistance( ResistanceType.Fire, 20, 30 );
			SetResistance( ResistanceType.Cold, 20, 30 );
			SetResistance( ResistanceType.Poison, 100 );
			SetResistance( ResistanceType.Energy, 40, 50 );

			SetSkill( SkillName.Poisoning, 105.1, 115.0 );
			SetSkill( SkillName.MagicResist, 90.1, 110.0 );
			SetSkill( SkillName.Tactics, 85.1, 90.0 );
			SetSkill( SkillName.Wrestling, 70.1, 90.0 );
			SetSkill( SkillName.EvalInt, 110.1, 120.0 );
			SetSkill( SkillName.Magery, 95.1, 105.0 );
			SetSkill( SkillName.Meditation, 95.1, 105.0 );

			Fame = 12500;
			Karma = -12500;

			PackItem( new Nightshade( 4 ) );
			PackItem( new LesserPoisonPotion() );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich );
			AddLoot( LootPack.Rich );
			AddLoot( LootPack.MedScrolls );
		}

		public override bool BleedImmune { get { return true; } }
		public override Poison PoisonImmune { get { return Poison.Lethal; } }

		public override Poison HitPoison { get { return Poison.Lethal; } }
		public override double HitPoisonChance { get { return 0.75; } }

		public override int TreasureMapLevel { get { return 5; } }

		public GreaterPoisonElemental( Serial serial )
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
