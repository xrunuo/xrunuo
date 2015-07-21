using System;
using Server;
using Server.Items;
using Server.Spells;
using Server.Spells.Seventh;
using Server.Spells.Fifth;
using Server.Engines.CannedEvil;
using Server.Engines.Plants;

namespace Server.Mobiles
{
	public class IlhenirTheStained : BaseChampion
	{
		public override Type[] UniqueList
		{
			get
			{
				return new Type[] { };
			}
		}

		public override Type[] SharedList
		{
			get
			{
				return new Type[]
					{
						typeof( ANecromancerShroud ),
						typeof( LieutenantOfTheBritannianRoyalGuard ),
						typeof( OblivionsNeedle ),
						typeof( TheRobeOfBritanniaAri )
					};
			}
		}
		public override Type[] DecorativeList
		{
			get
			{
				return new Type[]
					{
						typeof( MonsterStatuette )
					};
			}
		}

		public override MonsterStatuetteType[] StatueTypes
		{
			get
			{
				return new MonsterStatuetteType[]
					{
						MonsterStatuetteType.PlagueBeast,
						MonsterStatuetteType.RedDeath
					};
			}
		}

		public override int GetDeathSound()
		{
			return 0x57F;
		}
		public override int GetAttackSound()
		{
			return 0x580;
		}
		public override int GetIdleSound()
		{
			return 0x581;
		}
		public override int GetAngerSound()
		{
			return 0x582;
		}
		public override int GetHurtSound()
		{
			return 0x583;
		}

		public override ChampionSkullType SkullType { get { return ChampionSkullType.Power; } }

		[Constructable]
		public IlhenirTheStained()
			: base( AIType.AI_Melee )
		{
			Name = "Ilhenir";
			Title = "the Stained";

			Body = 259;

			Tamable = false;

			Hue = 1164;

			SetStr( 1200, 1300 );
			SetDex( 100, 150 );
			SetInt( 600, 650 );
			SetHits( 9000, 10000 );

			SetDamage( 29, 35 );

			SetDamageType( ResistanceType.Physical, 25 );
			SetDamageType( ResistanceType.Energy, 75 );

			SetResistance( ResistanceType.Physical, 50, 70 );
			SetResistance( ResistanceType.Fire, 50, 70 );
			SetResistance( ResistanceType.Cold, 40, 50 );
			SetResistance( ResistanceType.Poison, 50, 60 );
			SetResistance( ResistanceType.Energy, 50, 60 );

			SetSkill( SkillName.EvalInt, 90.1, 100.0 );
			SetSkill( SkillName.Magery, 99.1, 105.0 );
			SetSkill( SkillName.Meditation, 90.1, 100.0 );
			SetSkill( SkillName.MagicResist, 115.0, 125.0 );
			SetSkill( SkillName.Tactics, 115.0, 125.0 );
			SetSkill( SkillName.Wrestling, 115.0, 125.0 );

			Fame = 24000;
			Karma = -24000;

			AddItem( new Gold( 2000, 2500 ) );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.UltraRich, 4 );
			AddLoot( LootPack.FilthyRich );
		}

		public override FoodType FavoriteFood { get { return FoodType.Fish; } }

		public override int TreasureMapLevel { get { return 5; } }

		public override bool Unprovokable { get { return true; } }

		public override Poison HitPoison { get { return Poison.Lethal; } }
		public override double HitPoisonChance { get { return 1.0; } }
		public override bool BardImmune { get { return true; } }
		public override Poison PoisonImmune { get { return Poison.Lethal; } }

		public override int Hides { get { return 30; } }

		public override bool ShowFameTitle { get { return false; } }
		public override bool ClickTitle { get { return false; } }

		public IlhenirTheStained( Serial serial )
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