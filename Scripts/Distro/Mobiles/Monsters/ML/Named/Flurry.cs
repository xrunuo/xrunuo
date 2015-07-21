using System;
using Server.Items;
using System.Collections;

namespace Server.Mobiles
{
	[CorpseName( "the remains of Flurry" )]
	public class Flurry : AirElemental
	{
		private bool m_SpawnedFriends = false;
		private int MaxFriends = 5;

		[Constructable]
		public Flurry()
			: base()
		{
			Name = "Flurry";
			Body = 13;
			Hue = 3;
			BaseSoundID = 263;

			SetStr( 149, 195 );
			SetDex( 218, 264 );
			SetInt( 130, 199 );

			SetHits( 474, 477 );

			SetDamage( 10, 15 );  // not sure if this is right or not?

			SetDamageType( ResistanceType.Energy, 20 );
			SetDamageType( ResistanceType.Cold, 80 );

			SetResistance( ResistanceType.Physical, 56, 57 );
			SetResistance( ResistanceType.Fire, 38, 44 );
			SetResistance( ResistanceType.Cold, 40, 45 );
			SetResistance( ResistanceType.Poison, 31, 37 );
			SetResistance( ResistanceType.Energy, 39, 41 );

			SetSkill( SkillName.EvalInt, 99.1, 100.2 );
			SetSkill( SkillName.Magery, 105.1, 108.8 );
			SetSkill( SkillName.MagicResist, 104.0, 112.8 );
			SetSkill( SkillName.Tactics, 113.1, 119.8 );
			SetSkill( SkillName.Wrestling, 105.6, 106.4 );

			Fame = 4500;
			Karma = -4500;

			if ( 0.1 > Utility.RandomDouble() )
				PackItem( new PetParrot() );
		}

		public override void OnCombatantChange()
		{
			if ( m_SpawnedFriends )
				return;

			for ( int i = 1; i < MaxFriends + 1; i++ )
			{
				// spawn new friends

				BaseCreature friend = new AirElemental();

				friend.MoveToWorld( Map.GetSpawnPosition( Location, 3 ), Map );

				m_SpawnedFriends = true;
			}
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich, 4 );
		}

		protected override void OnAfterDeath( Container c )
		{
			base.OnAfterDeath( c );

			if ( 3000 > Utility.Random( 100000 ) )
			{
				c.DropItem( SetItemsHelper.GetRandomSetItem() );
			}

		}

		public Flurry( Serial serial )
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