using System;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName( "an undead horse corpse" )]
	public class GrizzledMare : BaseMount
	{
		[Constructable]
		public GrizzledMare()
			: this( "a grizzled mare" )
		{
		}

		[Constructable]
		public GrizzledMare( string name )
			: base( name, 793, 0x3EBB, AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			SetStr( 91, 100 );
			SetDex( 46, 55 );
			SetInt( 46, 60 );

			SetHits( 41, 50 );

			SetDamage( 5, 12 );

			SetDamageType( ResistanceType.Physical, 50 );
			SetDamageType( ResistanceType.Cold, 50 );

			SetResistance( ResistanceType.Physical, 50, 60 );
			SetResistance( ResistanceType.Cold, 90, 95 );
			SetResistance( ResistanceType.Poison, 100 );
			SetResistance( ResistanceType.Energy, 10, 15 );

			SetSkill( SkillName.MagicResist, 100 );
			SetSkill( SkillName.Tactics, 100 );
			SetSkill( SkillName.Wrestling, 100 );
			SetSkill( SkillName.Anatomy, 100 );

			Fame = 0;
			Karma = 0;

			Tamable = true; // so we can stable them
			ControlSlots = 1;
		}

		public override Poison PoisonImmune { get { return Poison.Lethal; } }
		public override bool BleedImmune { get { return true; } }

		public override bool DeleteOnRelease { get { return true; } }

		public GrizzledMare( Serial serial )
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

			/*int version = */
			reader.ReadInt();
		}
	}
}