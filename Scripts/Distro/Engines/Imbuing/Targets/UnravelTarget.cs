using System;
using Server;
using Server.Targeting;

namespace Server.Engines.Imbuing
{
	public class UnravelTarget : Target
	{
		public UnravelTarget()
			: base( -1, false, TargetFlags.None )
		{
		}

		protected override void OnTarget( Mobile from, object targeted )
		{
			Item item = targeted as Item;
			IImbuable toUnravel = targeted as IImbuable;

			from.EndAction( typeof( Imbuing ) );

			if ( !Soulforge.CheckProximity( from, 2 ) )
				from.SendLocalizedMessage( 1080433 ); // You must be near a soulforge to magically unravel an item.
			else if ( !Soulforge.CheckQueen( from ) )
				from.SendLocalizedMessage( 1113736 ); // You must rise to the rank of noble in the eyes of the Gargoyle Queen before her majesty will allow you to use this soulforge.
			else if ( item == null || !item.IsChildOf( from.Backpack ) )
				from.SendLocalizedMessage( 1080424 ); // The item must be in your backpack to magically unravel it.
			else
				Unraveling.TryUnravel( from, item, true );
		}

		protected override void OnTargetCancel( Mobile from, TargetCancelType cancelType )
		{
			from.EndAction( typeof( Imbuing ) );
		}
	}
}