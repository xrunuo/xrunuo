using System;
using Server;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Items
{
	public class RunedSwitch : Item
	{
		public override int LabelNumber { get { return 1032129; } } // Runed Switch

		[Constructable]
		public RunedSwitch()
			: base( 0x2F61 )
		{
			Weight = 1.0;
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( !IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it. 
				return;
			}

			from.SendLocalizedMessage( 1075101 ); // Please select an item to recharge.
			from.Target = new InternalTarget( this );
		}

		public RunedSwitch( Serial serial )
			: base( serial )
		{
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

			Weight = 1.0;
		}

		private class InternalTarget : Target
		{
			private RunedSwitch m_Item;

			public InternalTarget( RunedSwitch item )
				: base( 0, false, TargetFlags.None )
			{
				m_Item = item;
			}

			protected override void OnTarget( Mobile from, object o )
			{
				BaseTalisman talisman = o as BaseTalisman;

				if ( talisman == null )
				{
					from.SendLocalizedMessage( 1046439 ); // That is not a valid target.
				}
				else if ( !talisman.IsChildOf( from.Backpack ) )
				{
					from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it. 
				}
				else if ( talisman.Charges == -1 || talisman.TalismanType == TalismanType.None )
				{
					from.SendLocalizedMessage( 1046439 ); // That is not a valid target.
				}
				else if ( talisman.Charges > 0 )
				{
					from.SendLocalizedMessage( 1075099 ); // You cannot recharge that item until all of its current charges have been used.
				}
				else
				{
					from.SendLocalizedMessage( 1075100 ); // The item has been recharged.
					talisman.Charges = 49;

					if ( m_Item != null && !m_Item.Deleted )
						m_Item.Delete();
				}
			}
		}
	}
}
