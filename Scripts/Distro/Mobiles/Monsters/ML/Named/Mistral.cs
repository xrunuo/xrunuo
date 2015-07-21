using System;
using Server.Items;
using System.Collections;

namespace Server.Mobiles
{
	[CorpseName( "the remains of Mistral" )]
	public class Mistral : AirElemental
	{
		private bool m_SpawnedFriends = false;
		private int MaxFriends = 5;

		[Constructable]
		public Mistral()
			: base()
		{
			Name = "Mistral";
			Body = 13;
			Hue = 924;
			BaseSoundID = 263;

			SetStr( 134, 201 );
			SetDex( 226, 238 );
			SetInt( 126, 134 );

			SetHits( 386, 609 );

			SetDamage( 17, 20 );  // not sure if this is right or not?

			SetDamageType( ResistanceType.Energy, 20 );
			SetDamageType( ResistanceType.Cold, 40 );
			SetDamageType( ResistanceType.Physical, 40 );

			SetResistance( ResistanceType.Physical, 55, 64 );
			SetResistance( ResistanceType.Fire, 36, 40 );
			SetResistance( ResistanceType.Cold, 33, 39 );
			SetResistance( ResistanceType.Poison, 30, 39 );
			SetResistance( ResistanceType.Energy, 49, 53 );

			SetSkill( SkillName.EvalInt, 96.2, 97.8 );
			SetSkill( SkillName.Magery, 100.8, 112.9 );
			SetSkill( SkillName.MagicResist, 106.2, 111.2 );
			SetSkill( SkillName.Tactics, 110.2, 117.1 );
			SetSkill( SkillName.Wrestling, 100.3, 104.0 );

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

		public Mistral( Serial serial )
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
