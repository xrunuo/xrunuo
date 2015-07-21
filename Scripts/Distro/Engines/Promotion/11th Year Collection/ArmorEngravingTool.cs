using System;
using Server;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.Items
{
	public class ArmorEngravingTool : Item, IUsesRemaining
	{
		public override int LabelNumber { get { return 1080547; } } // Armor Engraving Tool

		private int m_UsesRemaining;

		[CommandProperty( AccessLevel.GameMaster )]
		public int UsesRemaining
		{
			get { return m_UsesRemaining; }
			set { m_UsesRemaining = value; InvalidateProperties(); }
		}

		public bool ShowUsesRemaining { get { return true; } set { } }

		[Constructable]
		public ArmorEngravingTool()
			: this( 30 )
		{
		}

		[Constructable]
		public ArmorEngravingTool( int uses )
			: base( 0x32F8 )
		{
			LootType = LootType.Blessed;
			Weight = 1.0;
			Hue = 1170;

			m_UsesRemaining = uses;
		}

		public ArmorEngravingTool( Serial serial )
			: base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( m_UsesRemaining > 0 )
			{
				from.SendLocalizedMessage( 1072357 ); // Select an object to engrave.
				from.Target = new InternalTarget( this );
			}
			else
			{
				from.SendLocalizedMessage( 1076163 ); // There are no charges left on this engraving tool.
			}
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			if ( ShowUsesRemaining )
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

		private class InternalTarget : Target
		{
			private ArmorEngravingTool m_Tool;

			public InternalTarget( ArmorEngravingTool tool )
				: base( -1, true, TargetFlags.None )
			{
				m_Tool = tool;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				if ( m_Tool == null || m_Tool.Deleted )
					return;

				if ( targeted is BaseArmor && !( targeted is BaseShield ) )
				{
					BaseArmor item = (BaseArmor) targeted;

					from.CloseGump( typeof( InternalGump ) );
					from.SendGump( new InternalGump( m_Tool, item ) );
				}
				else
					from.SendLocalizedMessage( 1072309 ); // The selected item cannot be engraved by this engraving tool.
			}
		}

		private class InternalGump : Gump
		{
			public override int TypeID { get { return 0x3E80; } }

			private ArmorEngravingTool m_Tool;
			private BaseArmor m_Target;

			public InternalGump( ArmorEngravingTool tool, BaseArmor target )
				: base( 0, 0 )
			{
				m_Tool = tool;
				m_Target = target;

				Closable = true;
				Disposable = true;
				Dragable = true;
				Resizable = false;

				AddBackground( 50, 50, 400, 300, 0xA28 );

				AddPage( 0 );

				AddHtmlLocalized( 50, 70, 400, 20, 1072359, 0x0, false, false ); // <CENTER>Engraving Tool</CENTER>
				AddHtmlLocalized( 75, 95, 350, 145, 1076229, 0x0, true, true ); // Please enter the text to add to the selected object. Leave the text area blank to remove any existing text.  Removing text does not use a charge.
				AddButton( 125, 300, 0x81A, 0x81B, 1, GumpButtonType.Reply, 0 );
				AddButton( 320, 300, 0x819, 0x818, 0, GumpButtonType.Reply, 0 );
				AddImageTiled( 75, 245, 350, 40, 0xDB0 );
				AddImageTiled( 76, 245, 350, 2, 0x23C5 );
				AddImageTiled( 75, 245, 2, 40, 0x23C3 );
				AddImageTiled( 75, 285, 350, 2, 0x23C5 );
				AddImageTiled( 425, 245, 2, 42, 0x23C3 );

				AddTextEntry( 78, 246, 343, 37, 0xA28, 15, "", 64 );
			}

			public override void OnResponse( GameClient state, RelayInfo info )
			{
				if ( m_Tool == null || m_Tool.Deleted )
					return;

				if ( m_Target == null || m_Target.Deleted )
					return;

				if ( info.ButtonID == 1 )
				{
					String text = Utility.RemoveHtml( info.GetTextEntry( 15 ).Text );

					if ( String.IsNullOrEmpty( text ) )
					{
						m_Target.EngravedText = null;
						state.Mobile.SendLocalizedMessage( 1072362 ); // You remove the engraving from the object.
					}
					else
					{
						if ( text.Length > 64 )
							m_Target.EngravedText = text.Substring( 0, 64 );
						else
							m_Target.EngravedText = text;

						state.Mobile.SendLocalizedMessage( 1072361 ); // You engraved the object.	

						m_Target.InvalidateProperties();

						m_Tool.UsesRemaining -= 1;
						m_Tool.InvalidateProperties();
					}
				}
				else
					state.Mobile.SendLocalizedMessage( 1072363 ); // The object was not engraved.
			}
		}
	}
}
