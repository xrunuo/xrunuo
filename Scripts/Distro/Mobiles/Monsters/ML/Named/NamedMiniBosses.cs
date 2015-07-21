using System;
using System.Linq;

using Server;
using Server.Items;

namespace Server.Mobiles
{
	public class NamedMiniBosses
	{
		public static Type[] m_NamedMiniBosses = new Type[]
		{
			typeof( Abscess ),				typeof( Coil ),			typeof( Gnaw ),
			typeof( Grobu ),				typeof( Guile ),		typeof( Irk ),
			typeof( LadyJennifyr ),			typeof( LadyLissith ),	typeof( LadyMarai ),
			typeof( LadySabrix ),			typeof( Lurg ),			typeof( Malefic ),
			typeof( MasterJonath ),			typeof( MasterMikael ),	typeof( MasterTheophilus ),
			typeof( Miasma ),				typeof( Pyre ),			typeof( RedDeath ),
			typeof( Rend ),					typeof( Saliva ),		typeof( Silk ),
			typeof( SirPatrick ),			typeof( Swoop ),		typeof( Tangle ),
			typeof( Thrasher ),				typeof( Virulent ),		typeof( Putrefier ),
			typeof( BlackOrderGrandMage ),	typeof( BlackOrderMasterThief ), typeof( BlackOrderHighExecutioner ),
			typeof( Spite ),				typeof( Flurry ),		typeof( Grim ),
			typeof( Meraktus ),				typeof( Mistral ),		typeof( Tempest )
	    };

		public static Type[] Artifacts = new Type[] 
		{
			typeof( AegisOfGrace ), typeof( BladeDance ), typeof( Bonesmasher ),
			typeof( Boomstick ), typeof( BrightsightLenses ), typeof( FeyLeggings ),
			typeof( FleshRipper ), typeof( HelmOfSwiftness ), typeof( PadsOfTheCuSidhe ),
			typeof( RaedsGlory ), typeof( RighteousAnger ), typeof( RobeOfTheEclipse ),
			typeof( RobeOfTheEquinox ), typeof( SoulSeeker ), typeof( TalonBite ),
			typeof( WildfireBow ), typeof( Windsong ),
			typeof( QuiverOfTheElements ), typeof( QuiverOfRage ),
			typeof( TotemOfTheVoid ), typeof( BloodwoodSpirit )
		};

		public static void Initialize()
		{
			BaseCreature.KilledBy += new BaseCreature.KilledByHandler( OnKilledBy );
		}

		private static void OnKilledBy( BaseCreature bc, Mobile killer )
		{
			if ( IsBoss( bc ) && CheckArtifactChance( killer, bc ) )
				GiveArtifactTo( killer );
		}

		public static bool IsBoss( Mobile m )
		{
			return m_NamedMiniBosses.Contains( m.GetType() );
		}

		public static bool CheckArtifactChance( Mobile m, BaseCreature bc )
		{
			double fame = (double) bc.Fame;

			if ( fame > 32000 )
				fame = 32000;

			double chance = 1 / ( Math.Max( 10, 100 * ( 0.83 - Math.Round( Math.Log( Math.Round( fame / 6000, 3 ) + 0.001, 10 ), 3 ) ) ) * ( 100 - Math.Sqrt( m.Luck ) ) / 100.0 );

			return chance > Utility.RandomDouble();
		}

		public static void GiveArtifactTo( Mobile m )
		{
			Item item = (Item) Activator.CreateInstance( Artifacts[Utility.Random( Artifacts.Length )] );

			if ( m.AddToBackpack( item ) )
			{
				m.SendLocalizedMessage( 1062317, "", 64 ); // For your valor in combating the fallen beast, a special artifact has been bestowed on you.
			}
			else
			{
				Container bank = m.BankBox;

				if ( bank != null && bank.TryDropItem( m, item, false ) )
				{
					m.SendLocalizedMessage( 1062317, "", 64 ); // For your valor in combating the fallen beast, a special artifact has been bestowed on you.
				}
				else
				{
					m.SendLocalizedMessage( 1072523, "", 64 ); // You find an artifact, but your backpack and bank are too full to hold it.

					item.MoveToWorld( m.Location, m.Map );
				}
			}

			Effects.SendLocationParticles( EffectItem.Create( m.Location, m.Map, EffectItem.DefaultDuration ), 0, 0, 0, 0, 0, 5060, 0 );
			Effects.PlaySound( m.Location, m.Map, 0x243 );

			Effects.SendMovingParticles( new DummyEntity( Serial.Zero, new Point3D( m.X - 6, m.Y - 6, m.Z + 15 ), m.Map ), m, 0x36D4, 7, 0, false, true, 0x497, 0, 9502, 1, 0, (EffectLayer) 255, 0x100 );
			Effects.SendMovingParticles( new DummyEntity( Serial.Zero, new Point3D( m.X - 4, m.Y - 6, m.Z + 15 ), m.Map ), m, 0x36D4, 7, 0, false, true, 0x497, 0, 9502, 1, 0, (EffectLayer) 255, 0x100 );
			Effects.SendMovingParticles( new DummyEntity( Serial.Zero, new Point3D( m.X - 6, m.Y - 4, m.Z + 15 ), m.Map ), m, 0x36D4, 7, 0, false, true, 0x497, 0, 9502, 1, 0, (EffectLayer) 255, 0x100 );

			Effects.SendTargetParticles( m, 0x375A, 35, 90, 0x00, 0x00, 9502, (EffectLayer) 255, 0x100 );
		}
	}
}