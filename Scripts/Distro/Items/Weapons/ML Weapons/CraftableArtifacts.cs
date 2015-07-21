using System;
using Server;

namespace Server.Items
{
	public class CraftableArtifacts
	{
		public static Type[] m_CraftableArtifacts = new Type[]
		{
			typeof( BoneMachete ),
			typeof( ColdForgedBlade ),
			typeof( LuminousRuneBlade ),
			typeof( OverseerSunderedBlade ),
			typeof( RuneCarvingKnife ),
			typeof( ShardThrasher ),
			typeof( SongWovenMantle ),
			typeof( SpellWovenBritches ),
			typeof( StitchersMittens ),
			typeof( BlightGrippedLongbow ),
			typeof( FaerieFire ),
			typeof( MischiefMaker ),
			typeof( SilvanisFeywoodBow ),
			typeof( TheNightReaper ),
			typeof( EssenceOfBattle ),
			typeof( ResillientBracer ),
			typeof( PendantOfTheMagi ),
			typeof( PhantomStaff ),
			typeof( IronwoodCrown ),
			typeof( BrambleCoat ),
	    };

		public static bool IsCraftableArtifact( Item item )
		{
			for ( int i = 0; i < m_CraftableArtifacts.Length; ++i )
			{
				if ( item.GetType() == m_CraftableArtifacts[i] )
				{
					return true;
				}
			}

			return false;
		}
	}
}