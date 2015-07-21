using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Misc;

namespace Server.Engines.Doom
{
	public class DoomArtifactGiver
	{
		public static Type[] Artifacts = new Type[]
			{
				typeof( TheTaskmaster ),			typeof( LegacyOfTheDreadLord ),
				typeof( TheDragonSlayer ),			typeof( ArmorOfFortune ),
				typeof( GauntletsOfNobility ),		typeof( HelmOfInsight ),
				typeof( HolyKnightsBreastplate ),	typeof( JackalsCollar ),
				typeof( LeggingsOfBane ),			typeof( MidnightBracers ),
				typeof( OrnateCrownOfTheHarrower ),	typeof( ShadowDancerLeggings ),
				typeof( TunicOfFire ),				typeof( VoiceOfTheFallenKing ),
				typeof( BraceletOfHealth ),			typeof( OrnamentOfTheMagician ),
				typeof( RingOfTheElements ),		typeof( RingOfTheVile ),
				typeof( Aegis ),					typeof( ArcaneShield ),
				typeof( AxeOfTheHeavens ),			typeof( BladeOfInsanity ),
				typeof( BoneCrusher ),				typeof( BreathOfTheDead ),
				typeof( Frostbringer ),				typeof( SerpentsFang ),
				typeof( StaffOfTheMagi ),			typeof( TheBeserkersMaul ),
				typeof( TheDryadBow ),				typeof( DivineCountenance ),
				typeof( HatOfTheMagi ),				typeof( HuntersHeaddress ),
				typeof( SpiritOfTheTotem )
			};

		public static void CheckArtifactGiving( BaseCreature boss )
		{
			int basePoints = ( boss is DemonKnight ) ? 500 : 100;

			List<DamageStore> rights = BaseCreature.GetLootingRights( boss.DamageEntries, boss.HitsMax );

			for ( int i = 0; i < rights.Count; i++ )
			{
				DamageStore store = rights[i];

				if ( !store.HasRight )
					continue;

				PlayerMobile pm = store.Mobile as PlayerMobile;

				if ( pm != null )
				{
					int awardPoints = (int) ( basePoints * ( 1.0 - Math.Ceiling( i / 2.0 ) * 0.02 ) );

					pm.DoomCredits += awardPoints;

					int chance = (int) ( pm.DoomCredits * ( 1.0 + LootPack.GetLuckChance( pm, boss ) / 10000.0 ) );

					if ( chance > Utility.Random( 1000000 ) )
						GiveArtifactTo( pm );
				}
			}
		}

		public static void GiveArtifactTo( PlayerMobile pm )
		{
			Item item = (Item) Activator.CreateInstance( Artifacts[Utility.Random( Artifacts.Length )] );

			bool message = true;

			if ( !pm.AddToBackpack( item ) )
			{
				Container bank = pm.BankBox;

				if ( !( bank != null && bank.TryDropItem( pm, item, false ) ) )
				{
					pm.SendLocalizedMessage( 1072523, "", 64 ); // You find an artifact, but your backpack and bank are too full to hold it.

					message = false;

					item.MoveToWorld( pm.Location, pm.Map );
				}
			}

			if ( message )
				pm.SendLocalizedMessage( 1062317, "", 64 ); // For your valor in combating the fallen beast, a special artifact has been bestowed on you.

			EffectPool.ArtifactDrop( pm );

			pm.DoomCredits = 0;
		}
	}
}