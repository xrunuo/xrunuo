using System;
using Server.Items;
using System.Collections;

namespace Server.Mobiles
{
	[CorpseName( "the remains of Meraktus" )]
	public class Meraktus : TormentedMinotaur
	{
		private bool m_SpawnedFriends = false;
		private int MaxFriends = 4;

		public override bool BardImmune { get { return true; } }

		[Constructable]
		public Meraktus()
			: base()
		{
			Name = "Meraktus";
			Title = "the Tormented";

			BaseSoundID = 680;
			Body = 263;
			Hue = 0x835;
		}

		public override void OnGaveMeleeAttack( Mobile defender )
		{
			base.OnGaveMeleeAttack( defender );

			if ( 0.3 >= Utility.RandomDouble() )
			{
				GroundSlap();
			}
		}

		protected override void OnAfterDeath( Container c )
		{
			base.OnAfterDeath( c );

			double random = Utility.RandomDouble();

			Item artifact = null;

			if ( 0.05 > random ) // 5% of getting a unique artifact
				artifact = new Subdue();
			else if ( 0.15 > random ) // 10% of getting a shared artifact
				switch ( Utility.Random( 2 ) )
				{
					case 0:
						artifact = new TheMostKnowledgePerson();
						break;
					case 1:
						artifact = new OblivionsNeedle();
						break;
					case 2:
						artifact = new RoyalGuardSurvivalKnife();
						break;
				}
			else if ( 0.30 > random ) // 15% of getting a decorative item
				switch ( Utility.Random( 2 ) )
				{
					case 0:
						artifact = new ArtifactLargeVase();
						break;
					case 1:
						artifact = new ArtifactVase();
						break;
					case 2:
						artifact = new MonsterStatuette( MonsterStatuetteType.TormentedMinotaur );
						break;
				}

			if ( artifact != null )
			{
				Mobile m = MonsterHelper.GetTopAttacker( this );

				if ( m != null )
					MonsterHelper.GiveArtifactTo( m, artifact );
				else
					artifact.Delete();
			}
		}

		public override void OnCombatantChange()
		{
			if ( m_SpawnedFriends )
				return;

			for ( int i = 1; i < MaxFriends + 1; i++ )
			{
				// spawn new friends

				BaseCreature friend = new TormentedMinotaur();

				friend.MoveToWorld( Map.GetSpawnPosition( Location, 3 ), Map );

				m_SpawnedFriends = true;
			}
		}

		public Meraktus( Serial serial )
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
