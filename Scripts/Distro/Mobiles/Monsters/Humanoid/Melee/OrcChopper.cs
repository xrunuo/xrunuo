using System;
using System.Collections;
using Server.Misc;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "an orcish corpse" )]
	public class OrcChopper : BaseCreature
	{
		public override WeaponAbility GetWeaponAbility()
		{
			return WeaponAbility.WhirlwindAttack;
		}

		public override InhumanSpeech SpeechType { get { return InhumanSpeech.Orc; } }

		[Constructable]
		public OrcChopper()
			: base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "an orc chopper";
			Body = 138;
			BaseSoundID = 0x45A;
			Hue = 0x96E;

			SetStr( 147, 245 );
			SetDex( 91, 115 );
			SetInt( 61, 85 );

			SetHits( 97, 139 );

			SetDamage( 4, 13 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 25, 35 );
			SetResistance( ResistanceType.Fire, 30, 40 );
			SetResistance( ResistanceType.Cold, 15, 25 );
			SetResistance( ResistanceType.Poison, 15, 20 );
			SetResistance( ResistanceType.Energy, 25, 30 );

			SetSkill( SkillName.MagicResist, 60.1, 85.0 );
			SetSkill( SkillName.Tactics, 75.1, 90.0 );
			SetSkill( SkillName.Wrestling, 60.1, 85.0 );

			SetSkill( SkillName.Swords, 60.1, 85.0 );
			SetSkill( SkillName.Parry, 60.1, 85.0 );

			Fame = 2500;
			Karma = -2500;

			switch ( Utility.Random( 3 ) )
			{
				case 0:
					PackItem( new Log( Utility.Random( 20 ) ) );
					break;
				case 1:
					PackItem( new Board( Utility.Random( 20 ) ) );
					break;
				case 2:
					PackItem( new LambLeg( Utility.Random( 3 ) ) );
					break;
			}

			PackItem( new RingmailChest() );
			AddItem( new ExecutionersAxe() );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Meager );
			AddLoot( LootPack.Average );
		}

		public override bool CanRummageCorpses { get { return true; } }
		public override int TreasureMapLevel { get { return 1; } }
		public override int Meat { get { return 1; } }

		public override OppositionGroup OppositionGroup { get { return OppositionGroup.SavagesAndOrcs; } }

		public override bool IsEnemy( Mobile m )
		{
			if ( m.Player && m.FindItemOnLayer( Layer.Helm ) is OrcishKinMask )
			{
				return false;
			}

			return base.IsEnemy( m );
		}

		public override void AggressiveAction( Mobile aggressor, bool criminal )
		{
			base.AggressiveAction( aggressor, criminal );

			Item item = aggressor.FindItemOnLayer( Layer.Helm );

			if ( item is OrcishKinMask )
			{
				AOS.Damage( aggressor, 50, 0, 100, 0, 0, 0 );
				item.Delete();
				aggressor.FixedParticles( 0x36BD, 20, 10, 5044, EffectLayer.Head );
				aggressor.PlaySound( 0x307 );
			}
		}

		public OrcChopper( Serial serial )
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