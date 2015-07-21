using System;
using Server;

namespace Server.Items
{
	public class SAArtifacts
	{
		public static Type[] m_SAArtifacts = new Type[]
		{
			typeof( AbyssalBlade ),
			typeof( AnimatedLegsOfTheInsaneTinker ),
			typeof( AxeOfAbandon ),
			typeof( AxesOfFury ),
			typeof( BansheesCall ),
			typeof( BasiliskHideBreastplate ),
			typeof( BladeOfBattle ),
			typeof( BouraTailShield ),
			typeof( BreastplateOfTheBerserker ),
			typeof( BurningAmber ),
			typeof( CastOffZombieSkin ),
			typeof( CavalrysFolly ),
			typeof( ChannelersDefender ),
			typeof( ClawsOfTheBerserker ),
			typeof( DeathsHead ),
			typeof( DefenderOfTheMagus ),
			typeof( DemonBridleRing ),
			typeof( DemonHuntersStandard ),
			typeof( DraconisWrath ),
			typeof( DragonHideShield ),
            typeof( DragonJadeEarrings ),
			typeof( EternalGuardianStaff ),
			typeof( FallenMysticsSpellbook ),
			typeof( GiantSteps ),
			typeof( IronwoodCompositeBow ),
			typeof( JadeWarAxe ),
			typeof( Lavaliere ),
			typeof( LegacyOfDespair ),
			typeof( LifeSyphon ),
			typeof( Mangler ),
			typeof( MantleOfTheFallen ),
			typeof( MysticsGarb ),
			typeof( NightEyes ),
			typeof( ObsidianEarrings ),
			typeof( PetrifiedSnake ),
			typeof( PillarOfStrength ),
			typeof( ProtectorOfTheBattleMage ),
			typeof( RaptorClaw ),
			typeof( ResonantStaffOfEnlightenment ),
			typeof( SignOfChaos ),
			typeof( SignOfOrder ),
			typeof( GargishSignOfChaos ),
			typeof( GargishSignOfOrder ),
			typeof( Slither ),
			typeof( SpinedBloodwormBracers ),
			typeof( StaffOfShatteredDreams ),
			typeof( StandardOfChaos ),
            typeof( StoneDragonsTooth ),
			typeof( StoneSlithClaw ),
			typeof( StormCaller ),
			typeof( SummonersKilt ),
			typeof( SwordOfShatteredHopes ),
			typeof( TangleA ),
			typeof( TheImpalersPick ),
			typeof( TokenOfHolyFavor ),
			typeof( TorcOfTheGuardians ),
			typeof( VampiricEssence ),
			typeof( Venom ),
			typeof( VoidInfusedKilt ),
			typeof( WallOfHungryMouths ),
	    };

		public static bool IsSAArtifact( Item item )
		{
			for ( int i = 0; i < m_SAArtifacts.Length; ++i )
			{
				if ( item.GetType() == m_SAArtifacts[i] )
				{
					return true;
				}
			}

			return false;
		}
	}
}