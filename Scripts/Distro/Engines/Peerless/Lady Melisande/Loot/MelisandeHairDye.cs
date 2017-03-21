using System;
using System.Text;
using Server.Gumps;
using Server.Network;

namespace Server.Items
{
	public class MelisandeHairDye : Item
	{
		public override int LabelNumber { get { return 1041060; } } // Hair Dye

		[Constructable]
		public MelisandeHairDye()
			: base( 0xEFF )
		{
			Weight = 1.0;
			Hue = Utility.Random( 26 ) + 1150;
		}

		public MelisandeHairDye( Serial serial )
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
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( from.InRange( this.GetWorldLocation(), 1 ) )
			{
				from.CloseGump( typeof( MelisandeHairDyeGump ) );
				from.SendGump( new MelisandeHairDyeGump( this ) );
			}
			else
			{
				from.LocalOverheadMessage( MessageType.Regular, 906, 1019045 ); // I can't reach that.
			}
		}
	}

	public class MelisandeHairDyeGump : Gump
	{
		private MelisandeHairDye m_MelisandeHairDye;

		public MelisandeHairDyeGump( MelisandeHairDye dye )
			: base( 50, 50 )
		{
			m_MelisandeHairDye = dye;

			Closable = true;
			Disposable = true;
			Dragable = true;
			Resizable = false;

			AddPage( 0 );

			AddBackground( 0, 0, 294, 175, 3600 );
			AddBackground( 0, 0, 70, 66, 3600 );
			AddBackground( 70, 103, 204, 51, 9270 );

			AddButton( 241, 121, 1209, 1209, 1, GumpButtonType.Reply, 0 );
			AddLabel( 78, 21, 193, @"This special hair dye is made of" );
			AddLabel( 78, 37, 193, @"a unique mixture of leaves," );
			AddLabel( 78, 53, 193, @"permanently changing one's hair" );
			AddLabel( 78, 69, 193, @"color until another dye is used." );
			AddLabel( 89, 117, 84, @"Use permanent Hair Dye" );

			AddItem( 12, 22, 3594, 1153 ); // Luna White used in gump
		}

		public override void OnResponse( NetState from, RelayInfo info )
		{
			if ( m_MelisandeHairDye.Deleted )
				return;

			Mobile m = from.Mobile;
			int[] switches = info.Switches;

			if ( !m_MelisandeHairDye.IsChildOf( m.Backpack ) )
			{
				m.SendLocalizedMessage( 1042010 ); //You must have the objectin your backpack to use it.
				return;
			}

			Item hair = m.FindItemOnLayer( Layer.Hair );
			Item beard = m.FindItemOnLayer( Layer.FacialHair );

			if ( info.ButtonID != 0 )
			{
				if ( m.HairItemID == 0 && m.FacialHairItemID == 0 )
				{
					m.SendLocalizedMessage( 502623 ); // You have no hair to dye and cannot use this
				}
				else
				{
					if ( m.HairItemID != 0 )
						m.HairHue = m_MelisandeHairDye.Hue;
					if ( m.FacialHairItemID != 0 )
						m.FacialHairHue = m_MelisandeHairDye.Hue;

					m.SendLocalizedMessage( 501199 );  // You dye your hair
					m_MelisandeHairDye.Delete();
					m.PlaySound( 0x4E );
				}
			}
			else
			{
				m.SendLocalizedMessage( 501200 ); // You decide not to dye your hair
			}
		}
	}
}
