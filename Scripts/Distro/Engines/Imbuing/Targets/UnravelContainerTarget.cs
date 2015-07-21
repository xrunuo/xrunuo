using System;
using System.Collections.Generic;
using Server;
using Server.Targeting;
using Server.Items;

namespace Server.Engines.Imbuing
{
	public class UnravelContainerTarget : Target
	{
		public UnravelContainerTarget()
			: base( -1, false, TargetFlags.None )
		{
		}

		protected override void OnTarget( Mobile from, object targeted )
		{
			Container cont = targeted as Container;

			from.EndAction( typeof( Imbuing ) );

			if ( !Soulforge.CheckProximity( from, 2 ) )
				from.SendLocalizedMessage( 1080433 ); // You must be near a soulforge to magically unravel an item.
			else if ( !Soulforge.CheckQueen( from ) )
				from.SendLocalizedMessage( 1113736 ); // You must rise to the rank of noble in the eyes of the Gargoyle Queen before her majesty will allow you to use this soulforge.
			else if ( cont == null )
				from.SendLocalizedMessage( 1111814, "0\t0" ); // Unraveled: ~1_COUNT~/~2_NUM~ items
			else if ( !cont.IsChildOf( from.Backpack ) )
				from.SendLocalizedMessage( 1062334 ); // This item must be in your backpack to be used.
			else if ( HasSpecialMaterialItem( cont ) )
			{
				from.BeginAction( typeof( Imbuing ) );
				from.SendGump( new ConfirmUnravelContainerGump( cont ) );
			}
			else
			{
				Unraveling.UnravelContainer( from, cont );
			}
		}

		private static bool HasSpecialMaterialItem( Container c )
		{
			for ( int i = 0; i < c.Items.Count; i++ )
			{
				IImbuable item = c.Items[i] as IImbuable;

				if ( item != null && item.IsSpecialMaterial )
					return true;
			}

			return false;
		}

		protected override void OnTargetCancel( Mobile from, TargetCancelType cancelType )
		{
			from.EndAction( typeof( Imbuing ) );
		}
	}
}