using System;
using Server;

namespace Server.Mobiles
{
	[CorpseName( "a slimy corpse" )]
	public abstract class UnderworldSlime : BaseCreature
	{
		[Constructable]
		public UnderworldSlime()
			: base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.25, 0.5 )
		{
			Name = "a slime";
			Body = 51;
			BaseSoundID = 456;

			SetMana( 0 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetSkill( SkillName.Poisoning, 30.1, 80 );
			SetSkill( SkillName.MagicResist, 15.1, 20.0 );
			SetSkill( SkillName.Tactics, 20.1, 35.0 );
			SetSkill( SkillName.Wrestling, 25.1, 30.0 );

			Fame = 300;
			Karma = -300;
		}

		public override bool CheckMovement( Direction d, out int newZ )
		{
			if ( !base.CheckMovement( d, out newZ ) )
				return false;

			if ( newZ > Location.Z )
				return false;

			return true;
		}

		public UnderworldSlime( Serial serial )
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

	public class LesserSlime : UnderworldSlime
	{
		[Constructable]
		public LesserSlime()
		{
			Hue = 2209;

			SetHits( 91, 110 );

			SetDamage( 6, 11 );
		}

		public LesserSlime( Serial serial )
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

	public class GreaterSlime : UnderworldSlime
	{
		[Constructable]
		public GreaterSlime()
		{
			Hue = Utility.Random( 2207, 3 );

			SetHits( 181, 213 );

			SetDamage( 16, 21 );
		}

		public GreaterSlime( Serial serial )
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
