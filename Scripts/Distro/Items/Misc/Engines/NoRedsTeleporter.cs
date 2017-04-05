using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
	[TypeAlias( "Server.Items.HeartwoodTeleport" )]
	public class NoRedsTeleporter : Teleporter
	{
		[Constructable]
		public NoRedsTeleporter()
		{
		}

		public NoRedsTeleporter( Serial serial )
			: base( serial )
		{
		}

		public override bool OnMoveOver( Mobile m )
		{
			if ( m.Murderer )
			{
				m.SendLocalizedMessage( 1074569 ); // Slayers of innocent souls are not welcome to enter here!
				return true;
			}

			return base.OnMoveOver( m );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}
}