using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a treefellow guardian's corpse" )]
	public class TreefellowGuardian : BaseCreature
	{
		public override WeaponAbility GetWeaponAbility()
		{
			return WeaponAbility.Dismount;
		}

		[Constructable]
		public TreefellowGuardian()
			: base( AIType.AI_Arcanist, FightMode.Aggressor, 10, 1, 0.2, 0.4 ) // TODO: Arcanist + Mystic
		{
			Name = "a treefellow guardian";
			Body = 301;

			SetStr( 500, 694 );
			SetDex( 34, 61 );
			SetInt( 405, 492 );

			SetHits( 709, 892 );

			SetDamage( 20, 25 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 30, 35 );
			SetResistance( ResistanceType.Fire, 0, 15 );
			SetResistance( ResistanceType.Cold, 50, 60 );
			SetResistance( ResistanceType.Poison, 20, 30 );
			SetResistance( ResistanceType.Energy, 80, 90 );

			SetSkill( SkillName.MagicResist, 110, 130.7 );
			SetSkill( SkillName.Tactics, 90.6, 119.8 );
			SetSkill( SkillName.Wrestling, 100.3, 118.7 );
			SetSkill( SkillName.Spellweaving, 105.6, 119.4 );

			Fame = 10000;
			Karma = 10000;

			PackItem( new Log( Utility.RandomMinMax( 25, 35 ) ) );

			// TODO: Mysticism scroll

			if ( 0.1 > Utility.RandomDouble() )
				PackItem( new TreefellowWood() );
		}

		public override OppositionGroup OppositionGroup
		{
			get { return OppositionGroup.FeyAndUndead; }
		}

		public override bool BleedImmune { get { return true; } }

		public override int GetIdleSound() { return 443; }
		public override int GetDeathSound() { return 31; }
		public override int GetAttackSound() { return 672; }

		public TreefellowGuardian( Serial serial )
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
