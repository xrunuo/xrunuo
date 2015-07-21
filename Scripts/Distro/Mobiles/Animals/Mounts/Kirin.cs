using System;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a ki-rin corpse" )]
	public class Kirin : BaseMount
	{
		public override bool AllowFemaleRider { get { return false; } }
		public override bool AllowFemaleTamer { get { return false; } }

		public override bool InitialInnocent { get { return true; } }

		public override void OnDisallowedRider( Mobile m )
		{
			m.SendLocalizedMessage( 1042319 ); // The Ki-Rin refuses your attempts to mount it.
		}

		public DateTime m_NextAbilityTime;

		[Constructable]
		public Kirin()
			: this( "a ki-rin" )
		{
		}

		[Constructable]
		public Kirin( string name )
			: base( name, 132, 0x3EAD, AIType.AI_Mage, FightMode.Evil, 10, 1, 0.2, 0.4 )
		{
			BaseSoundID = 0x3C5;

			SetStr( 296, 325 );
			SetDex( 86, 105 );
			SetInt( 186, 225 );

			SetHits( 191, 210 );

			SetDamage( 16, 22 );

			SetDamageType( ResistanceType.Physical, 70 );
			SetDamageType( ResistanceType.Fire, 10 );
			SetDamageType( ResistanceType.Cold, 10 );
			SetDamageType( ResistanceType.Energy, 10 );

			SetResistance( ResistanceType.Physical, 55, 65 );
			SetResistance( ResistanceType.Fire, 35, 45 );
			SetResistance( ResistanceType.Cold, 25, 35 );
			SetResistance( ResistanceType.Poison, 25, 35 );
			SetResistance( ResistanceType.Energy, 25, 35 );

			SetSkill( SkillName.EvalInt, 80.1, 90.0 );
			SetSkill( SkillName.Magery, 60.4, 100.0 );
			SetSkill( SkillName.Meditation, 90.1, 100.0 );
			SetSkill( SkillName.MagicResist, 85.3, 100.0 );
			SetSkill( SkillName.Tactics, 20.1, 22.5 );
			SetSkill( SkillName.Wrestling, 80.5, 92.5 );

			Fame = 9000;
			Karma = 9000;

			Tamable = true;
			ControlSlots = 2;
			MinTameSkill = 95.1;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich );
			AddLoot( LootPack.LowScrolls );
			AddLoot( LootPack.Potions );
		}

		protected override void OnAfterDeath( Container c )
		{
			if ( 0.3 > Utility.RandomDouble() )
				c.DropItem( new KirinBrains() );

			base.OnAfterDeath( c );
		}

		public override int Meat { get { return 3; } }
		public override int Hides { get { return 10; } }
		public override HideType HideType { get { return HideType.Horned; } }
		public override FoodType FavoriteFood { get { return FoodType.FruitsAndVegies | FoodType.GrainsAndHay; } }

		public Kirin( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 2 ); // version

			writer.Write( (DateTime) m_NextAbilityTime );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 2:
					{
						m_NextAbilityTime = reader.ReadDateTime();
						goto case 0;
					}
				case 0:
					{
						break;
					}
			}

			if ( version == 0 )
				AI = AIType.AI_Mage;
		}
	}
}
