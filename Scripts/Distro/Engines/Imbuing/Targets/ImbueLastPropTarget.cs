using System;
using Server;
using Server.Targeting;
using Server.Items;
using Server.Gumps;

namespace Server.Engines.Imbuing
{
	public class ImbueLastPropTarget : Target
	{
		private BaseAttrInfo m_Attribute;

		public ImbueLastPropTarget( BaseAttrInfo attribute )
			: base( -1, false, TargetFlags.None )
		{
			m_Attribute = attribute;
		}

		protected override void OnTarget( Mobile from, object targeted )
		{
			Item item = targeted as Item;

			if ( Imbuing.Check( from, item ) )
			{
				IImbuable imbuable = item as IImbuable;

				if ( imbuable != null && m_Attribute.CanHold( item ) && Imbuing.ValidateFlags( imbuable.ImbuingFlags, m_Attribute.Flags ) && m_Attribute.Validate( item ) )
				{
					Gump confirm = SelectPropGump.SelectProp( from, item, m_Attribute );

					if ( confirm != null )
						from.SendGump( confirm );
				}
				else
				{
					from.SendLocalizedMessage( 1114291 ); // You cannot imbue the last property on that item.
					from.EndAction( typeof( Imbuing ) );
				}
			}
			else
				from.EndAction( typeof( Imbuing ) );
		}

		protected override void OnTargetCancel( Mobile from, TargetCancelType cancelType )
		{
			from.EndAction( typeof( Imbuing ) );
		}
	}
}