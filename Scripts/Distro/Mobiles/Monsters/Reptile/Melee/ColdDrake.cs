using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a cold drake corpse" )]
	public class ColdDrake : BaseCreature
	{
		[Constructable]
		public ColdDrake()
			: base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a cold drake";
			Body = 60;
			Hue = Utility.RandomBlueHue();
			BaseSoundID = 362;

			SetStr( 617, 669 );
			SetDex( 133, 152 );
			SetInt( 152, 189 );

			SetHits( 461, 495 );
			SetMana( 309, 340 );

			SetDamage( 17, 20 );

			SetDamageType( ResistanceType.Physical, 50 );
			SetDamageType( ResistanceType.Cold, 50 );

			SetResistance( ResistanceType.Physical, 55, 65 );
			SetResistance( ResistanceType.Fire, 30, 40 );
			SetResistance( ResistanceType.Cold, 75, 90 );
			SetResistance( ResistanceType.Poison, 45, 60 );
			SetResistance( ResistanceType.Energy, 40, 50 );

			SetSkill( SkillName.MagicResist, 96.7, 109.8 );
			SetSkill( SkillName.Tactics, 116.2, 139.5 );
			SetSkill( SkillName.Wrestling, 115.4, 125.4 );

			Fame = 7500;
			Karma = -7500;

			PackReg( 3 );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich );
			AddLoot( LootPack.MedScrolls, 2 );
		}

		public override bool HasBreath { get { return true; } } // fire breath enabled
		public override int BreathFireDamage { get { return 0; } }
		public override int BreathColdDamage { get { return 100; } }
		public override int BreathEffectHue { get { return 0x480; } }
		public override int TreasureMapLevel { get { return 3; } }
		public override int Meat { get { return 10; } }
		public override int Hides { get { return 22; } }
		public override HideType HideType { get { return HideType.Horned; } }
		public override int Scales { get { return 8; } }
		public override int Blood { get { return 8; } }
		public override ScaleType ScaleType { get { return ScaleType.Yellow; } }
		public override FoodType FavoriteFood { get { return FoodType.Fish; } }
		public override bool HasAura { get { return true; } }
		public override TimeSpan AuraInterval { get { return TimeSpan.FromSeconds( 10.0 ); } }
		public override int AuraFireDamage { get { return 0; } }
		public override int AuraColdDamage { get { return 100; } }

		public override void AuraEffect( Mobile m )
		{
			m.SendLocalizedMessage( 1041522, String.Format( "*{0}\t#1008111\t*", Name ) ); // : The intense cold is damaging you!
		}

		public ColdDrake( Serial serial )
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