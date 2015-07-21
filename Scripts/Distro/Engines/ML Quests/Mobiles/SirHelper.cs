using System;
using System.Collections.Generic;
using System.Text;

using Server;
using Server.Gumps;
using Server.Items;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
	public class SirHelper : Mage
	{
		private static readonly TimeSpan m_ShoutDelay = TimeSpan.FromSeconds( 20.0 );
		private static readonly TimeSpan m_ShoutCooldown = TimeSpan.FromDays( 1.0 );

		private DateTime m_NextShout;

		public override bool IsActiveVendor { get { return false; } }

		[Constructable]
		public SirHelper()
		{
			Name = "Sir Helper";
			Title = "the Profession Guide";

			Hue = 0x83EA;

			Direction = Direction.South;
			Frozen = true;
		}

		public override void InitSBInfo()
		{
		}

		public override bool GetGender()
		{
			return false; // male
		}

		public override void CheckMorph()
		{
		}

		public override void InitOutfit()
		{
			HairItemID = 0x203C;
			FacialHairItemID = 0x204D;
			HairHue = FacialHairHue = 0x8A7;

			AddItem( new Sandals() );

			Item item;

			item = new Cloak();
			item.ItemID = 0x26AD;
			item.Hue = 0x455;
			AddItem( item );

			item = new Robe();
			item.ItemID = 0x26AE;
			item.Hue = 0x4AB;
			AddItem( item );

			item = new Backpack();
			item.Movable = false;
			AddItem( item );
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( from.CanBeginAction( this ) )
			{
				from.BeginAction( this );

				Timer.DelayCall( m_ShoutCooldown, () => from.EndAction( this ) );
			}

			SpellHelper.Turn( this, from );

			from.SendGump( new SirHelperGump() );
		}

		public override void OnThink()
		{
			base.OnThink();

			if ( m_NextShout <= DateTime.Now )
			{
				Packet shoutPacket = null;

				foreach ( GameClient state in this.GetClientsInRange( 12 ) )
				{
					Mobile m = state.Mobile;

					if ( m.CanSee( this ) && m.InLOS( this ) && m.CanBeginAction( this ) )
					{
						if ( shoutPacket == null )
							shoutPacket = Packet.Acquire( new MessageLocalized( Serial, Body, MessageType.Regular, 946, 3, 1078099, Name, "" ) ); // Double Click On Me For Help!

						state.Send( shoutPacket );
					}
				}

				Packet.Release( shoutPacket );

				m_NextShout = DateTime.Now + m_ShoutDelay;
			}
		}

		private class SirHelperGump : Gump
		{
			public SirHelperGump()
				: base( 75, 25 )
			{
				AddPage( 1 );

				Closable = false;
				AddImageTiled( 50, 20, 400, 400, 0x1404 );
				AddImageTiled( 50, 29, 30, 390, 0x28DC );
				AddImageTiled( 34, 140, 17, 279, 0x242F );
				AddImage( 48, 135, 0x28AB );
				AddImage( -16, 285, 0x28A2 );
				AddImage( 0, 10, 0x28B5 );
				AddImage( 25, 0, 0x28B4 );
				AddImageTiled( 83, 15, 350, 15, 0x280A );
				AddImage( 34, 419, 0x2842 );
				AddImage( 442, 419, 0x2840 );
				AddImageTiled( 51, 419, 392, 17, 0x2775 );
				AddImageTiled( 415, 29, 44, 390, 0xA2D );
				AddImageTiled( 415, 29, 30, 390, 0x28DC );
				AddLabel( 100, 50, 0x481, "" );
				AddImage( 370, 50, 0x589 );
				AddImage( 379, 60, 0x15A9 );
				AddImage( 425, 0, 0x28C9 );
				AddImage( 90, 33, 0x232D );

				AddHtmlLocalized( 130, 45, 270, 16, 1060668, 0xFFFFFF, false, false ); // INFORMATION
				AddImageTiled( 130, 65, 175, 1, 0x238D );

				AddButton( 95, 395, 0x2EE6, 0x2EE8, 3, GumpButtonType.Reply, 0 );

				AddHtmlLocalized( 160, 108, 250, 16, 1078029, 0x2710, false, false ); // New Haven Profession Guide

				/* Welcome to New Haven! You seek profession help? You have come to the right place!
				 * To learn more about the profession training New Haven has to offer, seek out the
				 * following guildmasters:
				 * 
				 * Warrior: Alexander Dumas
				 * Location: The Warrior's Guild Hall is the first building to the Northeast.
				 * 
				 * Mage: Pyronarro
				 * Location: The Mage School is a few buildings to the North.
				 * 
				 * Blacksmith: Tiny DuPont
				 * Location: The Blacksmith Shop is the building directly North of the New Haven Bank.
				 * 
				 * Necromancer: Malifnae
				 * Location: The Necromancer School is a little ways out of New Haven to the Northeast.
				 * 
				 * Paladin: Brahman
				 * Location: The Paladin Training Area is on the second floor of an adjacent building
				 * connected to the Warrior's Guild Hall.
				 * 
				 * Samurai: Haochi
				 * Location: The Samurai Profession can be learned in a building a little ways North
				 * of the Mage School.
				 * 
				 * Ninja: Yago
				 * Location: The Ninja Profession can be learned in a building a little ways West of
				 * the Mage School.
				 * 
				 * Good journey to you. Hope you enjoy your stay in New Haven!
				 */
				AddHtmlLocalized( 98, 156, 312, 180, 1078028, 0x15F90, false, true );
			}
		}

		public SirHelper( Serial serial )
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

			Frozen = true;
		}
	}
}
