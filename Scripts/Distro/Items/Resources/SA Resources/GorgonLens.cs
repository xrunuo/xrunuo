using System;
using Server;
using Server.Mobiles;
using Server.Engines.Craft;
using Server.Targeting;
using Server.Gumps;
using Server.Network;

namespace Server.Items
{
	public enum GorgonQuality
	{
		Normal,
		Exceptional,
		Invulnerable
	}

	public interface IGorgonCharges
	{
		int GorgonCharges { get; set; }
		GorgonQuality GorgonQuality { get; set; }
	}

	public class GorgonLens : Item, ICraftable
	{
		public override int LabelNumber { get { return 1112625; } } // Gorgon Lens

		private GorgonQuality m_Quality;

		[CommandProperty( AccessLevel.GameMaster )]
		public GorgonQuality Quality { get { return m_Quality; } set { m_Quality = value; } }

		[Constructable]
		public GorgonLens()
			: base( 0x26B4 )
		{
			Weight = 1.0;
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( !IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
			}
			else
			{
				from.SendLocalizedMessage( 1112596 ); // Which item do you wish to enhance with Gorgon Lenses?
				from.BeginTarget( -1, false, TargetFlags.None, new TargetCallback( SelectItem_Callback ) );
			}
		}

		private void SelectItem_Callback( Mobile from, object o )
		{
			IGorgonCharges targeted = o as IGorgonCharges;

			if ( targeted != null )
			{
				if ( targeted.GorgonCharges > 0 )
				{
					from.CloseGump( typeof( InternalGump ) );
					from.SendGump( new InternalGump( this, targeted ) );
				}
				else
					ApplyTo( from, targeted );
			}
			else
			{
				from.SendLocalizedMessage( 1112594 ); // You cannot place gorgon lenses on this.
			}
		}

		public void ApplyTo( Mobile from, IGorgonCharges item )
		{
			if ( !IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
			}
			else
			{
				from.SendLocalizedMessage( 1112595 ); // You enhance the item with Gorgon Lenses!

				item.GorgonCharges = 26;
				item.GorgonQuality = m_Quality;

				Delete();
			}
		}

		private class InternalGump : Gump
		{
			public override int TypeID { get { return 0x237C; } }

			private GorgonLens m_Lens;
			private IGorgonCharges m_Item;

			public InternalGump( GorgonLens lens, IGorgonCharges item )
				: base( 340, 340 )
			{
				m_Lens = lens;
				m_Item = item;

				AddPage( 0 );

				AddBackground( 0, 0, 291, 99, 0x13BE );
				AddImageTiled( 5, 6, 280, 20, 0xA40 );

				// Replace active Gorgon Lenses
				AddHtmlLocalized( 9, 8, 280, 20, 1112597, 0x7FFF, false, false );

				AddImageTiled( 5, 31, 280, 40, 0xA40 );

				// The remaining charges of the active lenses will be lost. Do you wish to proceed?
				AddHtmlLocalized( 9, 35, 272, 40, 1112598, 0x7FFF, false, false );

				AddButton( 215, 73, 0xFB7, 0xFB8, 1, GumpButtonType.Reply, 0 );
				AddHtmlLocalized( 250, 75, 65, 20, 1006044, 0x7FFF, false, false ); // OK

				AddButton( 5, 73, 0xFB1, 0xFB2, 0, GumpButtonType.Reply, 0 );
				AddHtmlLocalized( 40, 75, 100, 20, 1060051, 0x7FFF, false, false ); // CANCEL
			}

			public override void OnResponse( GameClient sender, RelayInfo info )
			{
				Mobile from = sender.Mobile;

				if ( info.ButtonID == 1 )
					m_Lens.ApplyTo( from, m_Item );
			}
		}

		public GorgonLens( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.Write( (int) m_Quality );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();

			m_Quality = (GorgonQuality) reader.ReadInt();
		}

		public bool OnCraft( bool exceptional, bool makersMark, Mobile from, CraftSystem craftSystem, Type typeRes, BaseTool tool, CraftItem craftItem, int resHue )
		{
			Hue = resHue;

			switch ( resHue )
			{
				default:
					Quality = GorgonQuality.Normal; break;
				case 2223:
					Quality = GorgonQuality.Exceptional; break;
				case 1266:
					Quality = GorgonQuality.Invulnerable; break;
			}

			return false;
		}
	}
}
