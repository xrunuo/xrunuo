using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Events;

namespace Server.Misc
{
	public class EquipMacro
	{
		public static void Initialize()
		{
			EventSink.EquipMacro += new EquipMacroEventHandler( EquipMacro_Handler );
			EventSink.UnequipMacro += new UnequipMacroEventHandler( UnequipMacro_Handler );
		}

		private static void Expire_Callback( object state )
		{
			Mobile m = (Mobile) state;

			m.EndAction( typeof( EquipMacro ) );
		}

		private static void UnequipMacro_Handler( UnequipMacroEventArgs e )
		{
			Mobile m = e.NetState.Mobile;

			if ( m != null && e.List != null )
			{
				if ( !m.CanBeginAction( typeof( EquipMacro ) ) || m.Backpack == null )
				{
					m.SendLocalizedMessage( 500119 ); // You must wait to perform another action.
					return;
				}

				m.BeginAction( typeof( EquipMacro ) );
				Timer.DelayCall( TimeSpan.FromSeconds( 1.0 ), new TimerStateCallback( Expire_Callback ), m );

				Layer layer;
				for ( int i = 0; i < e.List.Count; ++i )
				{
					try
					{
						layer = (Layer) e.List[i];
					}
					catch { continue; }

					Item item = m.FindItemOnLayer( layer );

					if ( item != null )
						m.Backpack.DropItem( item );
				}
			}
		}

		private static void EquipMacro_Handler( EquipMacroEventArgs e )
		{
			Mobile m = e.NetState.Mobile;

			if ( m != null && e.List != null )
			{
				if ( !m.CanBeginAction( typeof( EquipMacro ) ) )
				{
					m.SendLocalizedMessage( 500119 ); // You must wait to perform another action.
					return;
				}

				m.BeginAction( typeof( EquipMacro ) );
				Timer.DelayCall( TimeSpan.FromSeconds( 1.0 ), new TimerStateCallback( Expire_Callback ), m );

				for ( int i = 0; i < e.List.Count; ++i )
				{
					Item item = World.FindItem( e.List[i] );

					if ( item != null && item.IsChildOf( m ) )
						m.EquipItem( item );
				}
			}
		}
	}
}