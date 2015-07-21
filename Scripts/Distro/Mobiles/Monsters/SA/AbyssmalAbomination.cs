using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "an abyssmal abomination's corpse" )]
	public class AbyssmalAbomination : BaseCreature
	{
		public override WeaponAbility GetWeaponAbility()
		{
			return WeaponAbility.MortalStrike;
		}

		[Constructable]
		public AbyssmalAbomination()
			: base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "an abyssmal abomination";
			Body = 312;
			BaseSoundID = 0x451;

			Hue = 1159;

			SetStr( 203, 218 );
			SetDex( 81, 90 );
			SetInt( 203, 220 );

			SetHits( 608, 792 );

			SetDamage( 13, 17 );

			SetDamageType( ResistanceType.Physical, 50 );
			SetDamageType( ResistanceType.Poison, 50 );

			SetResistance( ResistanceType.Physical, 30, 35 );
			SetResistance( ResistanceType.Fire, 100 );
			SetResistance( ResistanceType.Cold, 50, 55 );
			SetResistance( ResistanceType.Poison, 60, 65 );
			SetResistance( ResistanceType.Energy, 75, 80 );

			SetSkill( SkillName.MagicResist, 87.8, 90.0 );
			SetSkill( SkillName.Tactics, 100.0 );
			SetSkill( SkillName.Wrestling, 84.3, 87.7 );
			SetSkill( SkillName.EvalInt, 108.2, 125.8 );
			SetSkill( SkillName.Magery, 104.4, 126.1 );
			SetSkill( SkillName.Meditation, 100.0 );

			Fame = 18000;
			Karma = -18000;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.UltraRich );
			AddLoot( LootPack.FilthyRich );
		}

		public override Poison PoisonImmune { get { return Poison.Lethal; } }

		public AbyssmalAbomination( Serial serial )
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
