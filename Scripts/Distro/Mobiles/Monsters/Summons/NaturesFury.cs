using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a nature's fury corpse" )]
	public class NaturesFury : BaseCreature
	{
		public override bool DeleteCorpseOnDeath { get { return Summoned; } }

		public override double DispelDifficulty { get { return 80.0; } }
		public override double DispelFocus { get { return 20.0; } }

		[Constructable]
		public NaturesFury()
			: base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.3, 0.6 )
		{
			Name = "a nature's fury";
			Body = 0x33;
			Hue = 0x4001;

			SetStr( 150 );
			SetDex( 150 );
			SetInt( 100 );

			SetHits( 80 );
			SetStam( 250 );
			SetMana( 0 );

			SetDamage( 14, 17 );

			SetDamageType( ResistanceType.Physical, 0 );
			SetDamageType( ResistanceType.Poison, 100 );

			SetResistance( ResistanceType.Physical, 90 );

			SetSkill( SkillName.MagicResist, 70.0 );
			SetSkill( SkillName.Tactics, 100.0 );
			SetSkill( SkillName.Wrestling, 90.0 );

			Fame = 0;
			Karma = 0;

			ControlSlots = 1;
		}

		public override void OnAttack( Mobile m )
		{
			this.FixedParticles( 0x91C, 10, 180, 9539, EffectLayer.Waist );
		}

		public override Poison PoisonImmune { get { return Poison.Lethal; } }

		public override int GetAngerSound()
		{
			return 0x00E;
		}

		public override int GetAttackSound()
		{
			return 0x1BC;
		}

		public NaturesFury( Serial serial )
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