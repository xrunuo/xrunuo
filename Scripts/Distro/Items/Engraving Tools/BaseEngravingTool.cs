using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Targeting;

namespace Server.Items
{
	public abstract class BaseEngravingTool : Item
	{
		public abstract bool ValidateItem( Item item );

		public virtual int SelectTargetMessage { get { return 1072357; } } // Select an object to engrave.
		public virtual int EngraveMessage { get { return 1072361; } } // You engraved the object.

		public virtual int GumpTitle { get { return 1072359; } } // <CENTER>Engraving Tool</CENTER>

		private int m_UsesRemaining;

		[CommandProperty( AccessLevel.GameMaster )]
		public int UsesRemaining
		{
			get { return m_UsesRemaining; }
			set
			{
				m_UsesRemaining = value;
				InvalidateProperties();
			}
		}

		public BaseEngravingTool( int itemid )
			: base( itemid )
		{
			UsesRemaining = 1;
		}

		public BaseEngravingTool( Serial serial )
			: base( serial )
		{
		}

		public override void OnDoubleClick( Mobile m )
		{
			m.SendLocalizedMessage( SelectTargetMessage );

			m.BeginTarget( -1, false, TargetFlags.None, new TargetCallback(
				( mob, targeted ) =>
				{
					Item item = targeted as Item;

					if ( item == null )
						return;

					if ( !IsChildOf( m.Backpack ) )
						m.SendLocalizedMessage( 1072308 ); // You cannot access the engraving tool.
					else if ( !ValidateItem( item ) )
						m.SendLocalizedMessage( 1072309 ); // The selected item cannot be engraved by this engraving tool.
					else if ( !item.IsChildOf( m.Backpack ) )
						m.SendLocalizedMessage( 1072310 ); // The selected item is not accessible to engrave.
					else
						m.SendGump( new EngravingGump( item, this ) );
				} ) );
		}

		public void FinishEngrave( Mobile from, Item item, string text )
		{
			// Do the real thing.
			DoEngrave( item, text );

			UsesRemaining--;

			from.SendLocalizedMessage( EngraveMessage );

			if ( UsesRemaining <= 0 )
			{
				Delete();
				from.SendLocalizedMessage( 1044038 ); // You have worn out your tool!
			}
		}

		protected virtual void DoEngrave( Item item, string text )
		{
			if ( !string.IsNullOrEmpty( text ) )
				item.Label1 = "Engraved: " + text; // TODO: is there a better way to do this?
			else
				item.Label1 = null;
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			list.Add( 1060584, m_UsesRemaining.ToString() ); // uses remaining: ~1_val~
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.Write( (int) m_UsesRemaining );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();

			m_UsesRemaining = reader.ReadInt();
		}
	}

	public class EngravingGump : Gump
	{
		private Item m_Item;
		private BaseEngravingTool m_Tool;

		public EngravingGump( Item item, BaseEngravingTool tool )
			: base( 50, 50 )
		{
			m_Item = item;
			m_Tool = tool;

			Closable = true;
			Disposable = false;
			Dragable = true;
			Resizable = false;

			AddPage( 0 );

			AddBackground( 15, 15, 400, 300, 2600 );

			// <CENTER>Engraving Tool</CENTER>
			AddHtmlLocalized( 35, 35, 350, 20, m_Tool.GumpTitle, 0, false, false );

			// Please enter the text to add to the selected object.
			// Leave the text area blank to remove the text from the
			// object without using up the tool
			AddHtmlLocalized( 41, 60, 350, 150, 1072360, 0, true, true );

			AddBackground( 40, 215, 350, 40, 3000 );
			AddTextEntry( 40, 215, 350, 40, 0, 0, "" );

			AddButton( 90, 265, 2450, 2451, 1, GumpButtonType.Reply, 0 );
			AddButton( 280, 265, 2453, 248, 2, GumpButtonType.Reply, 0 );
		}

		public override void OnResponse( GameClient sender, RelayInfo info )
		{
			Mobile from = sender.Mobile;

			from.CloseGump( typeof( EngravingGump ) );

			if ( info.ButtonID == 1 ) // Okay button.
			{
				string text = (string) info.GetTextEntry( 0 ).Text;

				if ( text.Length > 40 )
				{
					// If text size is illegal, resend gump.
					from.SendGump( new EngravingGump( m_Item, m_Tool ) );
				}
				else
				{
					m_Tool.FinishEngrave( from, m_Item, text );
				}
			}
		}
	}
}