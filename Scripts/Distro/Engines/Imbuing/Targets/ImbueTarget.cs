using System;
using Server;
using Server.Targeting;
using Server.Items;

namespace Server.Engines.Imbuing
{
	public class ImbueTarget : Target
	{
		public ImbueTarget()
			: base( -1, false, TargetFlags.None )
		{
		}

		protected override void OnTarget( Mobile from, object targeted )
		{
			Item toImbue = targeted as Item;

			if ( Imbuing.Check( from, toImbue ) )
				from.SendGump( new SelectPropGump( toImbue ) );
			else
				from.EndAction( typeof( Imbuing ) );
		}

		protected override void OnTargetCancel( Mobile from, TargetCancelType cancelType )
		{
			from.EndAction( typeof( Imbuing ) );
		}
	}
}