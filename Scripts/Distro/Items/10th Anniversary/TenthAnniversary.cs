using System;
using Server;
using Server.Items;
using Server.Regions;

namespace Server.Mobiles
{
	public class TenthAnniversary
	{
		public static bool Enabled = true;

		public static Type[] Artifacts = new Type[]
			{
				typeof( JaanasStaff ),		typeof( DragonsEnd ),
				typeof( SentinelsGuard ),	typeof( LordBlackthornsExemplar ),
				typeof( ArmsOfCompassion ),	typeof( BreastplateOfJustice ),
				typeof( GauntletsOfValor ),	typeof( HelmOfSpirituality ),
				typeof( LegsOfHonor ),		typeof( SolleretsOfSacrifice ),
				typeof( GorgetOfHonesty ),  typeof( MapOfTheKnownWorld ),
                typeof( KatrinasCrook ),	typeof( TenthAnniversarySculpture ),
				typeof( AnkhPendant ),
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

		private static bool CheckArtifactChance( Mobile from, BaseCreature bc )
		{
			if ( !from.Alive )
				return false;

			Region r = from.Region;

			if ( r.Name != "Covetous" && r.Name != "Deceit" && r.Name != "Despise" && r.Name != "Destard" && r.Name != "Hythloth" && r.Name != "Shame" && r.Name != "Wrong" )
				return false;

			if ( from.GetDistanceToSqrt( bc ) > 16 )
				return false;

			double fame = (double) bc.Fame;

			PlayerMobile pm = from as PlayerMobile;

			int luck = pm.Luck;

			double chance = fame / 1000000000;

			if ( from.Hidden )
				chance /= 2;

			pm.TenthAnniversaryCredits += chance;

			chance = pm.TenthAnniversaryCredits;

			if ( luck > 0 )
			{
				double luckmodifier = ( (double) luck / 600 );

				if ( luckmodifier < 1 )
					luckmodifier = 1;

				chance *= luckmodifier;
			}

			return chance > Utility.RandomDouble();
		}

		private static void GiveArtifactTo( Mobile m )
		{
			Item item = (Item) Activator.CreateInstance( Artifacts[Utility.Random( Artifacts.Length )] );

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

			pm.TenthAnniversaryCredits = 0;
		}
	}
}