using System;
using Server.Items;
using System.Collections;

namespace Server.Mobiles
{
	[CorpseName( "the remains of Tempest" )]
	public class Tempest : AirElemental
	{
		private bool m_SpawnedFriends = false;
		private int MaxFriends = 5;

		[Constructable]
		public Tempest()
			: base()
		{
			Name = "Tempest";
			Body = 13;
			Hue = 1175;
			BaseSoundID = 263;

			SetStr( 116, 135 );
			SetDex( 166, 185 );
			SetInt( 101, 125 );

			SetHits( 602 );

			SetDamage( 18, 20 );  // not sure if this is right or not?

			SetDamageType( ResistanceType.Energy, 80 );
			SetDamageType( ResistanceType.Cold, 20 );

			SetResistance( ResistanceType.Physical, 40, 50 );
			SetResistance( ResistanceType.Fire, 30, 40 );
			SetResistance( ResistanceType.Cold, 30, 40 );
			SetResistance( ResistanceType.Poison, 30, 40 );
			SetResistance( ResistanceType.Energy, 50, 60 );

			SetSkill( SkillName.EvalInt, 95, 105 );
			SetSkill( SkillName.Magery, 95, 105 );
			SetSkill( SkillName.MagicResist, 100, 110 );
			SetSkill( SkillName.Tactics, 105, 115 );
			SetSkill( SkillName.Wrestling, 110, 120 );

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

		public Tempest( Serial serial )
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