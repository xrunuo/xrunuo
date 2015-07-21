using System;
using Server;
using Server.Engines.Housing.Regions;
using Server.Items;

namespace Server.Mobiles
{
	public class TokunoTreasures
	{
		public static bool Enabled = true;

		public static readonly double MempoChance = 0.005; // 0.5%
		public static readonly double PigmentsChance = 0.05; // 5.0%

		public static Type[] MinorArtifacts = new Type[]
			{
				typeof( AncientFarmersKasa ),		typeof( AncientSamuraiDo ),
				typeof( AncientUrn ),				typeof( ArmsOfTacticalExcellence ),
				typeof( BlackLotusHood ),			typeof( ChestOfHeirlooms ),
				typeof( DaimyosHelm ),				typeof( DemonForks ),
				typeof( DragonNunchaku ),			typeof( Exiler ),
				typeof( FluteOfRenewal ),			typeof( GlovesOfTheSun ),
				typeof( HanzosBow ),				typeof( HonorableSwords ),
				typeof( LegsOfStability ),			typeof( PeasantsBokuto ),
				typeof( PigmentsOfTokuno ),			typeof( PilferedDancerFans ),
				typeof( TheDestroyer ),				typeof( TomeOfEnlightenment )
			};

		public static PigmentsType[] MinorPigmentsType = new PigmentsType[]
			{
				PigmentsType.FreshPlum,
				PigmentsType.Silver,
				PigmentsType.DeepBrown,
				PigmentsType.BurntBrown,
				PigmentsType.LightGreen,
				PigmentsType.FreshRose,
				PigmentsType.PaleBlue,
				PigmentsType.NobleGold,
				PigmentsType.PaleOrange,
				PigmentsType.ChaosBlue
			};

		public static Type[] MajorArtifacts = new Type[]
			{
				typeof( DarkenedSky ),				typeof( KasaOfRajin ),
				typeof( PigmentsOfTokunoMajor ),	typeof( RuneBeetleCarapace ),
				typeof( Stormgrip ),				typeof( SwordOfStampede ),
				typeof( SwordsOfProsperity ),		typeof( TheHorselord ),
				typeof( TomeOfLostKnowledge ),		typeof( WindsEdge )
			};

		public static void Initialize()
		{
			BaseCreature.KilledBy += new BaseCreature.KilledByHandler( OnKilledBy );
		}

		private static void OnKilledBy( BaseCreature bc, Mobile killer )
		{
			if ( Enabled && CheckArtifactChance( killer, bc ) )
				GiveArtifactTo( killer );
		}

		public static bool CheckArtifactChance( Mobile from, BaseCreature bc )
		{
			if ( !from.Alive )
				return false;

			Region r = from.Region;

			if ( r is HouseRegion || Server.Multis.BaseBoat.FindBoatAt( from, from.Map ) != null )
				return false;

			if ( from.Map != bc.Map || !from.InRange( bc, 90 ) )
				return false;

			if ( bc.Map != Map.Malas && bc.Map != Map.Tokuno )
				return false;

			if ( bc is Crane )
				return false;

			if ( bc.Map == Map.Malas )
			{
				if ( bc.X > 250 || bc.Y > 720 )
					return false;
			}

			double fame = (double) bc.Fame;

			PlayerMobile pm = from as PlayerMobile;

			int luck = pm.Luck;

			pm.ToTTotalMonsterFame += fame / 100000000;

			double chance = pm.ToTTotalMonsterFame;

			if ( luck > 0 )
			{
				double luckmodifier = ( (double) luck / 600 );

				if ( luckmodifier < 1 )
					luckmodifier = 1;

				chance *= luckmodifier;
			}

			return chance > Utility.RandomDouble();
		}

		public static void GiveArtifactTo( Mobile m )
		{
			double random = Utility.RandomDouble();

			Item item;

			if ( MempoChance > random )
				item = new LeurociansMempoOfFortune();
			else if ( PigmentsChance > random )
				item = new PigmentsOfTokunoMajor( MinorPigmentsType[Utility.Random( MinorPigmentsType.Length )], 1 );
			else
				item = (Item) Activator.CreateInstance( MinorArtifacts[Utility.Random( MinorArtifacts.Length )] );

			bool message = true;

			if ( !m.AddToBackpack( item ) )
			{
				Container bank = m.BankBox;

				if ( !( bank != null && bank.TryDropItem( m, item, false ) ) )
				{
					m.SendLocalizedMessage( 1072523, "", 64 ); // You find an artifact, but your backpack and bank are too full to hold it.

					message = false;

					item.MoveToWorld( m.Location, m.Map );
				}
			}

			if ( message )
				m.SendLocalizedMessage( 1062317, "", 64 ); // For your valor in combating the fallen beast, a special artifact has been bestowed on you.

			Effects.SendLocationParticles( EffectItem.Create( m.Location, m.Map, EffectItem.DefaultDuration ), 0, 0, 0, 0, 0, 5060, 0 );
			Effects.PlaySound( m.Location, m.Map, 0x243 );

			Effects.SendMovingParticles( new DummyEntity( Serial.Zero, new Point3D( m.X - 6, m.Y - 6, m.Z + 15 ), m.Map ), m, 0x36D4, 7, 0, false, true, 0x497, 0, 9502, 1, 0, (EffectLayer) 255, 0x100 );
			Effects.SendMovingParticles( new DummyEntity( Serial.Zero, new Point3D( m.X - 4, m.Y - 6, m.Z + 15 ), m.Map ), m, 0x36D4, 7, 0, false, true, 0x497, 0, 9502, 1, 0, (EffectLayer) 255, 0x100 );
			Effects.SendMovingParticles( new DummyEntity( Serial.Zero, new Point3D( m.X - 6, m.Y - 4, m.Z + 15 ), m.Map ), m, 0x36D4, 7, 0, false, true, 0x497, 0, 9502, 1, 0, (EffectLayer) 255, 0x100 );

			Effects.SendTargetParticles( m, 0x375A, 35, 90, 0x00, 0x00, 9502, (EffectLayer) 255, 0x100 );

			PlayerMobile pm = m as PlayerMobile;

			pm.ToTTotalMonsterFame = 0;
		}
	}
}