using System;
using Server.Items;
using System.Collections.Generic;

namespace Server.Mobiles
{
	[CorpseName( "the remains of Grim" )]
	public class Grim : Drake
	{
		private bool m_SpawnedFriends = false;

		private const int MaxFriends = 5;

		[Constructable]
		public Grim()
			: base()
		{
			Name = "Grim";
			Hue = 1744;
			Tamable = false;

			SetStr( 527, 580 );
			SetDex( 284, 322 );
			SetInt( 249, 386 );

			SetHits( 1762, 2502 );

			SetDamageType( ResistanceType.Physical, 80 );
			SetDamageType( ResistanceType.Fire, 20 );

			SetResistance( ResistanceType.Physical, 55, 60 );
			SetResistance( ResistanceType.Fire, 62, 68 );
			SetResistance( ResistanceType.Cold, 52, 57 );
			SetResistance( ResistanceType.Poison, 30, 40 );
			SetResistance( ResistanceType.Energy, 40, 44 );

			SetSkill( SkillName.MagicResist, 105.8, 115.6 );
			SetSkill( SkillName.Tactics, 102.8, 120.8 );
			SetSkill( SkillName.Wrestling, 111.7, 119.2 );
			SetSkill( SkillName.Anatomy, 105.0, 128.4 );

			if ( 0.1 > Utility.RandomDouble() )
				PackItem( new PetParrot() );
		}

		public override bool HasBreath { get { return true; } } // fire breath enabled

		public override WeaponAbility GetWeaponAbility()
		{
			return WeaponAbility.CrushingBlow;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich, 4 );
		}

		public override void OnCombatantChange()
		{
			if ( m_SpawnedFriends )
				return;

			for ( int i = 1; i < MaxFriends + 1; i++ )
			{
				// spawn new friends

				BaseCreature friend = new Drake();

				friend.MoveToWorld( Map.GetSpawnPosition( Location, 3 ), Map );

				m_SpawnedFriends = true;
			}
		}

		public Grim( Serial serial )
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
