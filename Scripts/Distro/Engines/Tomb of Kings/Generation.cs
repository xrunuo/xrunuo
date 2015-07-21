using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Gumps;

namespace Server.Engines.TombOfKings
{
	public class Generator
	{
		#region Generation
		public static void Initialize()
		{
			CommandSystem.Register( "GenToK", AccessLevel.Developer, new CommandEventHandler( GenToK_Command ) );
		}

		[Usage( "GenToK" )]
		private static void GenToK_Command( CommandEventArgs e )
		{
			e.Mobile.SendMessage( "Creating Tomb of Kings..." );

			// Serpent's Breath
			new FlameOfOrder( new Point3D( 28, 212, 3 ), Map.TerMur );
			new FlameOfChaos( new Point3D( 43, 212, 3 ), Map.TerMur );

			// Kings' Chambers
			ChamberLever.Generate();
			Chamber.Generate();
			ChamberSpawner.Generate();

			e.Mobile.SendMessage( "Generation completed!" );
		}
		#endregion
	}
}