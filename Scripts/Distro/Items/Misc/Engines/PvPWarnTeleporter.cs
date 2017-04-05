using System;
using Server;
using Server.Mobiles;
using Server.Gumps;

namespace Server.Items
{
	public class PvPWarnTeleporter : Teleporter
	{
		[Constructable]
		public PvPWarnTeleporter()
		{
		}

		public PvPWarnTeleporter( Serial serial )
			: base( serial )
		{
		}

		public override bool OnMoveOver( Mobile m )
		{
			PlayerMobile pm = m as PlayerMobile;

			if ( pm != null )
			{
				if ( pm.DisabledPvpWarning )
					return base.OnMoveOver( m );
				else if ( !pm.HasGump<PvpWarningGump>() )
					pm.SendGump( new PvpWarningGump( this ) );
			}

			return true;
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