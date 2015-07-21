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
	public class TwauloOfTheGlade : BaseChampion
	{
		public override ChampionSkullType SkullType { get { return ChampionSkullType.Power; } }

		public override Type[] UniqueList { get { return new Type[] { typeof( Quell ) }; } }
		public override Type[] SharedList { get { return new Type[] { typeof( TheMostKnowledgePerson ), typeof( OblivionsNeedle ) }; } }
		public override Type[] DecorativeList { get { return new Type[] { typeof( Pier ), typeof( MonsterStatuette ) }; } }

		public override MonsterStatuetteType[] StatueTypes { get { return new MonsterStatuetteType[] { MonsterStatuetteType.DreadHorn }; } }

		[Constructable]
		public TwauloOfTheGlade()
			: base( AIType.AI_Melee )
		{
			Name = "Twaulo";
			Title = "of the Glade";

			Body = 101;
			BaseSoundID = 679;

			Hue = 1109;

			SetStr( 1200, 1300 );
			SetDex( 100, 150 );
			SetInt( 600, 650 );
			SetHits( 9000, 10000 );

			SetDamage( 29, 35 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 55, 65 );
			SetResistance( ResistanceType.Fire, 45, 55 );
			SetResistance( ResistanceType.Cold, 35, 45 );
			SetResistance( ResistanceType.Poison, 55, 65 );
			SetResistance( ResistanceType.Energy, 45, 55 );

			SetSkill( SkillName.Anatomy, 105.1, 125.0 );
			SetSkill( SkillName.Archery, 105.1, 110.0 );
			SetSkill( SkillName.MagicResist, 110.0, 120.0 );
			SetSkill( SkillName.Tactics, 110.1, 120.0 );
			SetSkill( SkillName.Wrestling, 105.1, 115.0 );

			Fame = 24000;
			Karma = -24000;

			AddItem( new Gold( 2000, 2500 ) );

			AddItem( new Bow() );
			PackItem( new Arrow( Utility.RandomMinMax( 80, 90 ) ) ); // OSI it is different: in a sub backpack, this is probably just a limitation of their engine
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.UltraRich, 4 );
			AddLoot( LootPack.FilthyRich );
		}

		public override int TreasureMapLevel { get { return 5; } }
		public override bool Unprovokable { get { return true; } }
		public override bool BardImmune { get { return true; } }
		public override int Hides { get { return 30; } }
		public override bool ShowFameTitle { get { return false; } }
		public override bool ClickTitle { get { return false; } }
		public override int Meat { get { return 3; } }
		public override HideType HideType { get { return HideType.Spined; } }

		public override OppositionGroup OppositionGroup
		{
			get { return OppositionGroup.FeyAndUndead; }
		}

		public TwauloOfTheGlade( Serial serial )
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
